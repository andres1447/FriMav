using CefSharp;
using CefSharp.WinForms;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace FriMav.Client.Printer
{
    class PrintToPDF : AbstractRazorPrintMode
    {
        private const int A4WidthMicrons = 210000;
        private const int A4HeightMicrons = 297000;

        // Single page mode lays the template out at this width and scales it to A4 width.
        private const int SinglePageLayoutWidthPx = 1024;
        private const double MicronsPerCssPx = 25400.0 / 96.0;
        // PDF viewers (Acrobat among them) do not support pages taller than 200 inches.
        private const double MaxPageHeightMicrons = 200 * 25400.0;
        private const int SinglePageSlackPx = 24;
        // The page is a hair taller than the pinned document so rounding never creates a second page.
        private const double PageOverhangMicrons = 1000;

        private static readonly Regex LocalImageSrc = new Regex(
            @"src\s*=\s*[""'](?!data:|https?:|file:)([^""']+)[""']",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex PdfSinglePageMeta = new Regex(
            @"<meta\s+name=[""']pdf-single-page[""']\s+content=[""']true[""']",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly ChromiumWebBrowser _browser;
        private readonly object _printLock = new object();

        public PrintToPDF(ChromiumWebBrowser browser)
        {
            _browser = browser;
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

            lock (_printLock)
            {
                string url = null;
                try
                {
                    EnsureBrowserReady();
                    url = LoadTemplate(template);
                    PdfPrintSettings settings;
                    if (PdfSinglePageMeta.IsMatch(template))
                    {
                        settings = PrepareSinglePage();
                    }
                    else
                    {
                        settings = CreateA4Settings();
                    }
                    if (!_browser.PrintToPdfAsync(dest, settings).GetAwaiter().GetResult())
                        throw new InvalidOperationException("No se pudo guardar el PDF.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Lista de precios",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                finally
                {
                    UnregisterTemplate(url);
                }
            }
        }

        private void EnsureBrowserReady()
        {
            if (_browser.IsBrowserInitialized)
                return;

            using (var ready = new ManualResetEventSlim(false))
            {
                EventHandler<IsBrowserInitializedChangedEventArgs> onReady = null;
                onReady = (sender, args) =>
                {
                    _browser.IsBrowserInitializedChanged -= onReady;
                    ready.Set();
                };

                _browser.IsBrowserInitializedChanged += onReady;
                try
                {
                    if (_browser.IsBrowserInitialized)
                        ready.Set();
                    else if (!ready.Wait(TimeSpan.FromSeconds(30)))
                        throw new TimeoutException("El generador de PDF no está listo.");
                }
                finally
                {
                    _browser.IsBrowserInitializedChanged -= onReady;
                }
            }
        }

        private string LoadTemplate(string html)
        {
            var url = "http://frimav.local/print/" + Guid.NewGuid().ToString("N") + "/";
            using (var loaded = new ManualResetEventSlim(false))
            {
                EventHandler<FrameLoadEndEventArgs> onFrameLoadEnd = null;
                onFrameLoadEnd = (sender, args) =>
                {
                    if (!args.Frame.IsMain || args.Url == null || args.Url.IndexOf(url, StringComparison.OrdinalIgnoreCase) < 0)
                        return;

                    _browser.FrameLoadEnd -= onFrameLoadEnd;
                    loaded.Set();
                };

                _browser.FrameLoadEnd += onFrameLoadEnd;
                try
                {
                    _browser.LoadHtml(html, url);
                    if (!loaded.Wait(TimeSpan.FromMinutes(2)))
                        throw new TimeoutException("La lista de precios tardó demasiado en prepararse.");

                    return url;
                }
                catch
                {
                    UnregisterTemplate(url);
                    throw;
                }
                finally
                {
                    _browser.FrameLoadEnd -= onFrameLoadEnd;
                }
            }
        }

        private PdfPrintSettings PrepareSinglePage()
        {
            var contentHeightPx = EvaluateNumber(string.Format(CultureInfo.InvariantCulture,
                @"(function () {{
                    var root = document.documentElement;
                    root.style.width = '{0}px';
                    root.style.zoom = '1';
                    return Math.max(root.scrollHeight, document.body.scrollHeight,
                        Math.ceil(document.body.getBoundingClientRect().bottom));
                }})()",
                SinglePageLayoutWidthPx));

            if (contentHeightPx <= 0)
                throw new InvalidOperationException("No se pudo medir la lista de precios.");

            // Print layout comes out a few pixels taller than the screen measurement,
            // so the document is pinned to a slightly larger height and clipped there.
            var documentHeightPx = Math.Ceiling(contentHeightPx) + SinglePageSlackPx;

            var zoom = (A4WidthMicrons / MicronsPerCssPx) / SinglePageLayoutWidthPx;
            var heightMicrons = documentHeightPx * zoom * MicronsPerCssPx + PageOverhangMicrons;
            if (heightMicrons > MaxPageHeightMicrons)
                zoom *= (MaxPageHeightMicrons - PageOverhangMicrons) / (documentHeightPx * zoom * MicronsPerCssPx);

            EvaluateNumber(string.Format(CultureInfo.InvariantCulture,
                @"(function () {{
                    var root = document.documentElement;
                    root.style.zoom = '{0}';
                    root.style.height = '{1}px';
                    root.style.overflow = 'hidden';
                    document.body.style.height = '100%';
                    document.body.style.overflow = 'hidden';
                    var page = document.querySelector('.page');
                    if (page) {{
                        page.style.boxSizing = 'border-box';
                        page.style.minHeight = '100%';
                    }}
                    return 1;
                }})()",
                zoom,
                documentHeightPx));

            var pageWidthMicrons = SinglePageLayoutWidthPx * zoom * MicronsPerCssPx;
            var pageHeightMicrons = Math.Min(
                MaxPageHeightMicrons,
                documentHeightPx * zoom * MicronsPerCssPx + PageOverhangMicrons);

            return CreateSettings((int)Math.Ceiling(pageWidthMicrons), (int)Math.Ceiling(pageHeightMicrons));
        }

        private static PdfPrintSettings CreateA4Settings()
        {
            var settings = CreateSettings(A4WidthMicrons, A4HeightMicrons);
            // None ignores @page margins. Any margin here is painted white and
            // the page background cannot cover it.
            settings.MarginType = CefPdfPrintMarginType.None;
            return settings;
        }

        private double EvaluateNumber(string script)
        {
            var response = _browser.EvaluateScriptAsync(script, TimeSpan.FromSeconds(30)).GetAwaiter().GetResult();
            if (!response.Success)
                throw new InvalidOperationException(response.Message);

            return Convert.ToDouble(response.Result, CultureInfo.InvariantCulture);
        }

        private static PdfPrintSettings CreateSettings(int widthMicrons, int heightMicrons)
        {
            return new PdfPrintSettings
            {
                BackgroundsEnabled = true,
                HeaderFooterEnabled = false,
                Landscape = false,
                MarginType = CefPdfPrintMarginType.None,
                PageWidth = widthMicrons,
                PageHeight = heightMicrons
            };
        }

        private void UnregisterTemplate(string url)
        {
            if (string.IsNullOrEmpty(url))
                return;

            var factory = _browser.ResourceHandlerFactory as DefaultResourceHandlerFactory;
            if (factory != null)
                factory.UnregisterHandler(url);
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
    }
}
