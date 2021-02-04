using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace FriMav.Client.Printer
{
    public class PrintToFile : AbstractRazorPrintMode
    {
        public override string Name
        {
            get
            {
                return "print.mode.file";
            }
        }

        public override void PrintTemplate(string dest, string template)
        {
            File.WriteAllText(dest, template);
        }

        public override string ToString()
        {
            return PrintResource.Mode_PrintToFile;
        }
    }
}
