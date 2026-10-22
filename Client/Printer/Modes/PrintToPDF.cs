using SelectPdf;
using System;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
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

        private static readonly Regex PdfA4Meta = new Regex(
            @"<meta\s+name=[""']pdf-a4[""']\s+content=[""']true[""']",
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
            var a4 = PdfA4Meta.IsMatch(template);
            if (PdfSinglePageMeta.IsMatch(template))
                ConfigureSinglePage(converter, template);
            else if (a4)
                ConfigureA4Pages(converter);

            using (var file = new FileStream(dest, FileMode.Create))
            {
                PdfDocument doc = converter.ConvertHtmlString(template);
                if (!a4)
                {
                    doc.Save(file);
                    doc.Close();
                    return;
                }

                using (var buffer = new MemoryStream())
                {
                    doc.Save(buffer);
                    doc.Close();
                    var stamped = StampA4Background(buffer.ToArray());
                    file.Write(stamped, 0, stamped.Length);
                }
            }
        }

        // SelectPdf clips each page to the HTML fragment, so the rest of the
        // sheet stays white. Paint the same document gradient on the full page first.
        private static byte[] StampA4Background(byte[] pdf)
        {
            var enc = Encoding.GetEncoding(28591);
            var text = enc.GetString(pdf);
            float formHeight = 0f;
            foreach (Match bbox in Regex.Matches(text, @"/BBox\s*\[\s*[-\d\.]+\s+[-\d\.]+\s+[-\d\.]+\s+([-\d\.]+)\s*\]"))
            {
                float height = Parse(bbox.Groups[1].Value);
                if (height > formHeight)
                    formHeight = height;
            }
            if (formHeight < 1f)
                return pdf;

            var trailer = Regex.Match(text, @"trailer\s*<<(?<body>.*?)>>\s*startxref\s*(?<xref>\d+)", RegexOptions.Singleline);
            if (!trailer.Success)
                return pdf;
            int prev = int.Parse(trailer.Groups["xref"].Value, CultureInfo.InvariantCulture);
            var sizeMatch = Regex.Match(trailer.Groups["body"].Value, @"/Size\s+(\d+)");
            var rootMatch = Regex.Match(trailer.Groups["body"].Value, @"/Root\s+(\d+\s+\d+\s+R)");
            var infoMatch = Regex.Match(trailer.Groups["body"].Value, @"/Info\s+(\d+\s+\d+\s+R)");
            if (!sizeMatch.Success || !rootMatch.Success)
                return pdf;

            int nextId = int.Parse(sizeMatch.Groups[1].Value, CultureInfo.InvariantCulture);
            var append = new MemoryStream();
            var writer = new StreamWriter(append, enc) { NewLine = "\n", AutoFlush = true };
            var offsets = new System.Collections.Generic.List<long>();

            int search = 0;
            while (true)
            {
                int typeAt = text.IndexOf("/Type /Page", search);
                if (typeAt < 0)
                    break;
                if (typeAt + 11 < text.Length && text[typeAt + 11] == 's')
                {
                    search = typeAt + 11;
                    continue;
                }

                int objAt = text.LastIndexOf("obj", typeAt, StringComparison.Ordinal);
                int endObj = text.IndexOf("endobj", typeAt, StringComparison.Ordinal);
                if (objAt < 0 || endObj < 0)
                    break;

                int numStart = objAt;
                while (numStart > 0 && text[numStart - 1] == ' ')
                    numStart--;
                while (numStart > 0 && char.IsDigit(text[numStart - 1]))
                    numStart--;
                while (numStart > 0 && text[numStart - 1] == ' ')
                    numStart--;
                while (numStart > 0 && char.IsDigit(text[numStart - 1]))
                    numStart--;
                var pageDict = text.Substring(numStart, endObj - numStart);
                var pageNum = Regex.Match(pageDict, @"(\d+)\s+0\s+obj");
                var box = Regex.Match(pageDict, @"/MediaBox\s*\[\s*([-\d\.]+)\s+([-\d\.]+)\s+([-\d\.]+)\s+([-\d\.]+)\s*\]");
                var contents = Regex.Match(pageDict, @"/Contents\s*\[\s*(\d+)\s+0\s+R\s*\]");
                if (!pageNum.Success || !box.Success || !contents.Success)
                {
                    search = endObj + 6;
                    continue;
                }

                var decoded = ReadContentStream(pdf, text, int.Parse(contents.Groups[1].Value, CultureInfo.InvariantCulture));
                if (decoded == null)
                {
                    search = endObj + 6;
                    continue;
                }

                var content = enc.GetString(decoded);
                var matrix = Regex.Match(content, @"([-\d\.]+)\s+[-\d\.]+\s+[-\d\.]+\s+([-\d\.]+)\s+[-\d\.]+\s+([-\d\.]+)\s+cm");
                if (!matrix.Success)
                {
                    search = endObj + 6;
                    continue;
                }

                float scale = Parse(matrix.Groups[2].Value);
                float ty = Parse(matrix.Groups[3].Value);
                float pageWidth = Parse(box.Groups[3].Value) - Parse(box.Groups[1].Value);
                float pageHeight = Parse(box.Groups[4].Value) - Parse(box.Groups[2].Value);
                if (scale <= 0f || pageWidth <= 0f || pageHeight <= 0f)
                {
                    search = endObj + 6;
                    continue;
                }

                var painted = new MemoryStream();
                var paint = new StreamWriter(painted, enc) { NewLine = "\n", AutoFlush = true };
                paint.WriteLine("q");
                const int strips = 64;
                for (int i = 0; i < strips; i++)
                {
                    float y0 = pageHeight * i / strips;
                    float y1 = pageHeight * (i + 1) / strips;
                    float formY = ((y0 + y1) * 0.5f - ty) / scale;
                    float r, g, b;
                    SampleBackground(formY, formHeight, out r, out g, out b);
                    paint.WriteLine(string.Format(CultureInfo.InvariantCulture,
                        "{0:0.#####} {1:0.#####} {2:0.#####} rg", r, g, b));
                    paint.WriteLine(string.Format(CultureInfo.InvariantCulture,
                        "0 {0:0.#####} {1:0.#####} {2:0.#####} re f", y0, pageWidth, y1 - y0));
                }
                paint.WriteLine("Q");
                paint.Flush();
                var prefix = painted.ToArray();
                var combined = new byte[prefix.Length + decoded.Length];
                Buffer.BlockCopy(prefix, 0, combined, 0, prefix.Length);
                Buffer.BlockCopy(decoded, 0, combined, prefix.Length, decoded.Length);

                int contentId = nextId++;
                long contentOffset = pdf.Length + append.Length;
                writer.WriteLine(contentId + " 0 obj");
                writer.WriteLine("<< /Length " + combined.Length + " >>");
                writer.WriteLine("stream");
                writer.Flush();
                append.Write(combined, 0, combined.Length);
                writer.WriteLine();
                writer.WriteLine("endstream");
                writer.WriteLine("endobj");
                writer.Flush();

                var updated = Regex.Replace(pageDict, @"/Contents\s*\[\s*\d+\s+0\s+R\s*\]",
                    "/Contents [" + contentId + " 0 R]");
                int pageId = int.Parse(pageNum.Groups[1].Value, CultureInfo.InvariantCulture);
                long pageOffset = pdf.Length + append.Length;
                writer.Write(updated);
                if (!updated.EndsWith("\n"))
                    writer.WriteLine();
                writer.WriteLine("endobj");
                writer.Flush();

                while (offsets.Count <= pageId)
                    offsets.Add(-1);
                while (offsets.Count <= contentId)
                    offsets.Add(-1);
                offsets[pageId] = pageOffset;
                offsets[contentId] = contentOffset;
                search = endObj + 6;
            }

            if (offsets.Count == 0 || !offsets.Exists(offset => offset >= 0))
                return pdf;

            long xrefOffset = pdf.Length + append.Length;
            writer.Write("xref\n");
            writer.Write("0 1\n");
            writer.Write("0000000000 65535 f \n");
            int index = 1;
            while (index < offsets.Count)
            {
                if (offsets[index] < 0)
                {
                    index++;
                    continue;
                }
                int start = index;
                while (index < offsets.Count && offsets[index] >= 0)
                    index++;
                writer.Write(start + " " + (index - start) + "\n");
                for (int id = start; id < index; id++)
                    writer.Write(offsets[id].ToString("0000000000", CultureInfo.InvariantCulture) + " 00000 n \n");
            }
            writer.WriteLine("trailer");
            writer.WriteLine("<<");
            writer.WriteLine("/Size " + nextId);
            writer.WriteLine("/Prev " + prev);
            writer.WriteLine("/Root " + rootMatch.Groups[1].Value);
            if (infoMatch.Success)
                writer.WriteLine("/Info " + infoMatch.Groups[1].Value);
            writer.WriteLine(">>");
            writer.WriteLine("startxref");
            writer.WriteLine(xrefOffset.ToString(CultureInfo.InvariantCulture));
            writer.WriteLine("%%EOF");
            writer.Flush();

            var result = new byte[pdf.Length + append.Length];
            Buffer.BlockCopy(pdf, 0, result, 0, pdf.Length);
            append.Position = 0;
            append.Read(result, pdf.Length, (int)append.Length);
            return result;
        }

        private static void SampleBackground(float formY, float formHeight, out float r, out float g, out float b)
        {
            float t = formY / formHeight;
            if (t < 0f) t = 0f;
            if (t > 1f) t = 1f;
            r = (243f + (251f - 243f) * t) / 255f;
            g = (235f + (247f - 235f) * t) / 255f;
            b = (224f + (240f - 224f) * t) / 255f;
            float fromTop = 1f - t;
            if (fromTop < 0.55f)
            {
                float alpha = 0.08f * (1f - fromTop / 0.55f);
                r = r * (1f - alpha) + (140f / 255f) * alpha;
                g = g * (1f - alpha) + (28f / 255f) * alpha;
                b = b * (1f - alpha) + (28f / 255f) * alpha;
            }
        }

        private static byte[] ReadContentStream(byte[] pdf, string text, int id)
        {
            var marker = id.ToString(CultureInfo.InvariantCulture) + " 0 obj";
            int at = 0;
            while (true)
            {
                at = text.IndexOf(marker, at, StringComparison.Ordinal);
                if (at < 0)
                    return null;
                if (at == 0 || !char.IsDigit(text[at - 1]))
                    break;
                at += marker.Length;
            }
            int streamAt = text.IndexOf("stream", at, StringComparison.Ordinal);
            int endAt = text.IndexOf("endstream", streamAt, StringComparison.Ordinal);
            if (streamAt < 0 || endAt < 0)
                return null;
            int data = streamAt + 6;
            if (data < text.Length && text[data] == '\r') data++;
            if (data < text.Length && text[data] == '\n') data++;
            int dataEnd = endAt;
            if (dataEnd > data && text[dataEnd - 1] == '\n') dataEnd--;
            if (dataEnd > data && text[dataEnd - 1] == '\r') dataEnd--;
            if (dataEnd <= data)
                return null;
            var raw = new byte[dataEnd - data];
            Buffer.BlockCopy(pdf, data, raw, 0, raw.Length);
            bool flate = text.IndexOf("/FlateDecode", at, streamAt - at, StringComparison.Ordinal) >= 0;
            if (!flate)
                return raw;
            try
            {
                int start = raw.Length > 2 && raw[0] == 0x78 ? 2 : 0;
                using (var input = new MemoryStream(raw, start, raw.Length - start))
                using (var deflate = new DeflateStream(input, CompressionMode.Decompress))
                using (var output = new MemoryStream())
                {
                    deflate.CopyTo(output);
                    return output.ToArray();
                }
            }
            catch (InvalidDataException)
            {
                return null;
            }
        }

        private static float Parse(string value)
        {
            return float.Parse(value, CultureInfo.InvariantCulture);
        }

        private static void ConfigureA4Pages(HtmlToPdf converter)
        {
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.WebPageWidth = 1024;
            converter.Options.WebPageHeight = 0;
            converter.Options.MarginTop = 0;
            converter.Options.MarginBottom = 0;
            converter.Options.MarginLeft = 0;
            converter.Options.MarginRight = 0;
            converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
            converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;
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
