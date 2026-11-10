using CefSharp;
using CefSharp.WinForms;
using SelectPdf;
using System;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace FriMav.Client.Printer
{
    class PrintToPDF : AbstractRazorPrintMode
    {
        // SelectPdf lays the HTML out at this width and then scales it to the A4 page.
        private const double LayoutWidth = 1024d;
        private const double A4WidthMicrons = 210000d;
        private const double A4HeightMicrons = 297000d;
        private const double MicronsPerInch = 25400d;
        private const double CssPixelsPerInch = 96d;
        // PDF page height cannot exceed 200 inches.
        private const double MaxPageMicrons = 200d * MicronsPerInch;

        private static readonly Regex LocalImageSrc = new Regex(
            @"src\s*=\s*[""'](?!data:|https?:|file:)([^""']+)[""']",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex PdfSinglePageMeta = new Regex(
            @"<meta\s+name=[""']pdf-single-page[""']\s+content=[""']true[""']",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex PdfA4Meta = new Regex(
            @"<meta\s+name=[""']pdf-a4[""']\s+content=[""']true[""']",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // 210mm at 96px/in is ~794px. Dividing by the 1024px layout width is the
        // same fit SelectPdf applied, so type size and wrapping stay as they are.
        private static double FitZoom
        {
            get { return (A4WidthMicrons / MicronsPerInch * CssPixelsPerInch) / LayoutWidth; }
        }

        public override string Name
        {
            get
            {
                return "print.mode.pdf";
            }
        }

        public override void PrintTemplate(string dest, string template)
        {
            var templatesPath = Path.Combine(
                Application.StartupPath,
                ConfigurationManager.AppSettings["TemplatesPath"] ?? "Templates");

            template = EmbedLocalImages(template, templatesPath);

            if (PdfSinglePageMeta.IsMatch(template) || PdfA4Meta.IsMatch(template))
                PrintWithChromium(dest, template, PdfSinglePageMeta.IsMatch(template));
            else
                PrintWithSelectPdf(dest, template);
        }

        private static void PrintWithSelectPdf(string dest, string template)
        {
            HtmlToPdf converter = new HtmlToPdf();
            using (var file = new FileStream(dest, FileMode.Create))
            {
                PdfDocument doc = converter.ConvertHtmlString(template);
                doc.Save(file);
                doc.Close();
            }
        }

        private static void PrintWithChromium(string dest, string template, bool singlePage)
        {
            var job = new PdfJob();
            HiddenHost host = null;
            ChromiumWebBrowser browser = null;
            var htmlFile = Path.Combine(Path.GetTempPath(), "frimav-print-" + Guid.NewGuid().ToString("n") + ".html");
            File.WriteAllText(htmlFile, template, System.Text.Encoding.UTF8);
            var url = new Uri(htmlFile).AbsoluteUri;

            Action start = () =>
            {
                try
                {
                    host = new HiddenHost();
                    host.ShowInTaskbar = false;
                    host.FormBorderStyle = FormBorderStyle.None;
                    host.StartPosition = FormStartPosition.Manual;
                    host.Location = new Point(-4200, -4200);
                    host.ClientSize = new Size((int)LayoutWidth, 1600);

                    browser = new ChromiumWebBrowser("about:blank");
                    browser.Dock = DockStyle.None;
                    browser.Location = new Point(0, 0);
                    browser.Size = host.ClientSize;
                    browser.BrowserSettings = new BrowserSettings
                    {
                        FileAccessFromFileUrls = CefState.Enabled,
                        UniversalAccessFromFileUrls = CefState.Enabled
                    };
                    host.Controls.Add(browser);

                    browser.FrameLoadEnd += (sender, args) =>
                    {
                        if (!args.Frame.IsMain)
                            return;
                        if (args.Url == null || args.Url.IndexOf("frimav-print-", StringComparison.OrdinalIgnoreCase) < 0)
                            return;
                        if (Interlocked.Exchange(ref job.Started, 1) != 0)
                            return;

                        ApplyScaleAndPrint(browser, dest, singlePage, job);
                    };

                    browser.LoadError += (sender, args) =>
                    {
                        if (!args.Frame.IsMain || args.ErrorCode == CefErrorCode.Aborted)
                            return;
                        job.Fail(new InvalidOperationException(args.ErrorText));
                    };

                    // Navigation before the browser exists is stored and then ignored.
                    var requested = 0;
                    Action load = () =>
                    {
                        if (!browser.IsBrowserInitialized || Interlocked.Exchange(ref requested, 1) != 0)
                            return;
                        browser.Load(url);
                    };
                    browser.IsBrowserInitializedChanged += (sender, args) =>
                    {
                        if (!browser.IsBrowserInitialized)
                            return;
                        if (browser.InvokeRequired)
                            browser.BeginInvoke(load);
                        else
                            load();
                    };
                    host.Show();
                    load();
                }
                catch (Exception ex)
                {
                    job.Fail(ex);
                }
            };

            var ui = Application.OpenForms.Count > 0 ? Application.OpenForms[0] : null;
            try
            {
                if (ui != null && ui.InvokeRequired)
                    ui.BeginInvoke(start);
                else
                    start();

                WaitFor(job.Done, ui);
            }
            finally
            {
                Action cleanup = () =>
                {
                    if (browser != null)
                        browser.Dispose();
                    if (host != null)
                    {
                        host.Close();
                        host.Dispose();
                    }
                };

                try
                {
                    if (host != null && host.InvokeRequired)
                        host.Invoke(cleanup);
                    else
                        cleanup();
                }
                catch (InvalidOperationException)
                {
                }

                try
                {
                    if (File.Exists(htmlFile))
                        File.Delete(htmlFile);
                }
                catch (IOException)
                {
                }
            }

            if (job.Error != null)
                throw job.Error;
        }

        private static void ApplyScaleAndPrint(ChromiumWebBrowser browser, string dest, bool singlePage, PdfJob job)
        {
            var zoom = FitZoom.ToString("0.########", CultureInfo.InvariantCulture);
            var script =
                "(function(){" +
                "var h=Math.max(document.body?document.body.scrollHeight:0, document.documentElement.scrollHeight||0);" +
                "var s=document.documentElement.style;" +
                "s.webkitPrintColorAdjust='exact';" +
                "s.zoom='" + zoom + "';" +
                "if(document.body) document.body.style.webkitPrintColorAdjust='exact';" +
                "return h;" +
                "})()";

            browser.EvaluateScriptAsync(script).ContinueWith(measure =>
            {
                if (measure.IsFaulted || measure.IsCanceled)
                {
                    job.Fail(measure.Exception == null
                        ? new InvalidOperationException("No se pudo medir el PDF.")
                        : measure.Exception.GetBaseException());
                    return;
                }

                if (measure.Result == null || !measure.Result.Success)
                {
                    var message = measure.Result == null ? "No se pudo medir el PDF." : measure.Result.Message;
                    job.Fail(new InvalidOperationException(message));
                    return;
                }

                double layoutHeight = 0d;
                if (measure.Result.Result != null)
                    layoutHeight = Convert.ToDouble(measure.Result.Result, CultureInfo.InvariantCulture);

                var settings = CreateSettings(singlePage, layoutHeight);
                browser.PrintToPdfAsync(dest, settings).ContinueWith(print =>
                {
                    if (print.IsFaulted || print.IsCanceled)
                    {
                        job.Fail(print.Exception == null
                            ? new InvalidOperationException("Chromium no pudo generar el PDF.")
                            : print.Exception.GetBaseException());
                        return;
                    }

                    if (!print.Result)
                        job.Fail(new InvalidOperationException("Chromium no pudo generar el PDF."));
                    else
                        job.Succeed();
                });
            });
        }

        private static PdfPrintSettings CreateSettings(bool singlePage, double layoutHeightPx)
        {
            var settings = new PdfPrintSettings
            {
                MarginType = CefPdfPrintMarginType.None,
                BackgroundsEnabled = true,
                HeaderFooterEnabled = false,
                Landscape = false,
                SelectionOnly = false,
                PageWidth = (int)A4WidthMicrons,
                PageHeight = (int)A4HeightMicrons
            };

            if (!singlePage)
                return settings;

            var visualPx = layoutHeightPx * FitZoom;
            var microns = visualPx / CssPixelsPerInch * MicronsPerInch + 2000d;
            if (microns < 1000d)
                return settings;
            if (microns > MaxPageMicrons)
                microns = MaxPageMicrons;

            settings.PageHeight = (int)Math.Ceiling(microns);
            return settings;
        }

        private static void WaitFor(ManualResetEvent done, Control ui)
        {
            var deadline = DateTime.UtcNow.AddMinutes(3);
            if (ui != null && ui.InvokeRequired)
            {
                if (!done.WaitOne(TimeSpan.FromMinutes(3)))
                    throw new TimeoutException("La generación del PDF superó el tiempo de espera.");
                return;
            }

            while (!done.WaitOne(15))
            {
                if (DateTime.UtcNow >= deadline)
                    throw new TimeoutException("La generación del PDF superó el tiempo de espera.");
                Application.DoEvents();
            }
        }

        private static string EmbedLocalImages(string html, string templatesPath)
        {
            return LocalImageSrc.Replace(html, match =>
            {
                var relativePath = match.Groups[1].Value.Replace('/', Path.DirectorySeparatorChar);
                var fullPath = Path.GetFullPath(Path.Combine(templatesPath, relativePath));
                if (!fullPath.StartsWith(Path.GetFullPath(templatesPath), StringComparison.OrdinalIgnoreCase)
                    || !File.Exists(fullPath))
                {
                    return match.Value;
                }

                var mime = GetMimeType(fullPath);
                var dataUri = "data:" + mime + ";base64," + Convert.ToBase64String(File.ReadAllBytes(fullPath));
                return "src=\"" + dataUri + "\"";
            });
        }

        private static string GetMimeType(string path)
        {
            switch (Path.GetExtension(path).ToLowerInvariant())
            {
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".webp":
                    return "image/webp";
                case ".svg":
                    return "image/svg+xml";
                default:
                    return "image/jpeg";
            }
        }

        public override string ToString()
        {
            return PrintResource.Mode_PrintToPDF;
        }

        private class PdfJob
        {
            public Exception Error;
            public int Started;
            public readonly ManualResetEvent Done = new ManualResetEvent(false);

            public void Fail(Exception ex)
            {
                Error = ex;
                Done.Set();
            }

            public void Succeed()
            {
                Done.Set();
            }
        }

        private class HiddenHost : Form
        {
            protected override bool ShowWithoutActivation
            {
                get { return true; }
            }

            protected override CreateParams CreateParams
            {
                get
                {
                    const int WS_EX_TOOLWINDOW = 0x80;
                    var parameters = base.CreateParams;
                    parameters.ExStyle |= WS_EX_TOOLWINDOW;
                    return parameters;
                }
            }
        }
    }
}
