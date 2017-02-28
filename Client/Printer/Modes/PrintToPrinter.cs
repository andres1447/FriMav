using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace FriMav.Client.Printer
{
    public class PrintToPrinter : AbstractRazorPrintMode
    {
        public override string Name
        {
            get
            {
                return "print.mode.printer";
            }
        }

        public override void PrintTemplate(string dest, string template)
        {
            Image image = HtmlRender.RenderToImage(template);
            PrintDocument document = new PrintDocument();
            document.PrinterSettings.PrinterName = dest;
            document.PrintPage += (ev, args) =>
            {
                Point point = new Point(0, 0);
                args.Graphics.DrawImage(image, point);
            };
            document.Print();

            /*WebBrowser webBrowser = new WebBrowser();
            webBrowser.DocumentCompleted += webBrowser_DocumentCompleted;
            webBrowser.DocumentText = template;*/
        }

        void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser browser = sender as WebBrowser;
            if (null == browser)
            {
                return;
            }

            browser.Print();
            browser.Dispose();
        }

        public override string ToString()
        {
            return PrintResource.Mode_PrintToPrinter;
        }
    }
}
