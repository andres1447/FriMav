using SelectPdf;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FriMav.Client.Printer
{
    class PrintToPDF : AbstractRazorPrintMode
    {
        private static readonly Regex LocalImageSrc = new Regex(
            @"src\s*=\s*[""'](?!data:|https?:|file:)([^""']+)[""']",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex PdfSinglePageMeta = new Regex(
            @"<meta\s+name=[""']pdf-single-page[""']\s+content=[""']true[""']",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

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

            HtmlToPdf converter = new HtmlToPdf();
            if (PdfSinglePageMeta.IsMatch(template))
                ConfigureSinglePage(converter, template);

            using (var file = new FileStream(dest, FileMode.Create))
            {
                PdfDocument doc = converter.ConvertHtmlString(template);
                doc.Save(file);
                doc.Close();
            }
        }

        private static void ConfigureSinglePage(HtmlToPdf converter, string html)
        {
            const int webPageWidth = 1024;

            var htmlToImage = new HtmlToImage();
            htmlToImage.WebPageWidth = webPageWidth;
            htmlToImage.WebPageHeight = 0;

            using (Image preview = htmlToImage.ConvertHtmlString(html))
            {
                // Keep A4 width; height grows with content so the PDF has no trailing blank.
                const float pageWidthPt = 595f;
                float pageHeightPt = pageWidthPt * preview.Height / (float)preview.Width;
                if (pageHeightPt < 1f)
                    pageHeightPt = 1f;

                converter.Options.WebPageWidth = webPageWidth;
                converter.Options.WebPageHeight = 0;
                converter.Options.PdfPageSize = PdfPageSize.Custom;
                converter.Options.PdfPageCustomSize = new SizeF(pageWidthPt, pageHeightPt);
                converter.Options.MarginTop = 0;
                converter.Options.MarginBottom = 0;
                converter.Options.MarginLeft = 0;
                converter.Options.MarginRight = 0;
                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
                converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
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
    }
}
