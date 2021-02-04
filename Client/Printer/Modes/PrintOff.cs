using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace FriMav.Client.Printer
{
    public class PrintOff : IPrintMode
    {
        public string Name => "print.mode.off";

        public bool Applies(string mode)
        {
            return false;
        }

        public void Print(string printer, dynamic model, string modelType)
        {
        }

        public override string ToString()
        {
            return PrintResource.Mode_PrintOff;
        }
    }
}
