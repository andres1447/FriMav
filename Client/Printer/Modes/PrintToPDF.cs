using SelectPdf;
using System;
using System.IO;
using System.Windows.Forms;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace FriMav.Client.Printer
{
    class PrintToPDF : AbstractRazorPrintMode
    {
        public override string Name
        {
            get
            {
                return "print.mode.pdf";
            }
        }

        public override void PrintTemplate(string dest, string template)
        {
            HtmlToPdf converter = new HtmlToPdf();
            using (var file = new FileStream(dest, FileMode.Create))
            {
                PdfDocument doc = converter.ConvertHtmlString(template);
                doc.Save(file);
                doc.Close();
            }
        }

        public override string ToString()
        {
            return PrintResource.Mode_PrintToPDF;
        }
    }
}
