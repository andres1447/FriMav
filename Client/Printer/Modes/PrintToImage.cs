using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace FriMav.Client.Printer
{
    public class PrintToImage : AbstractRazorPrintMode
    {
        public override string Name
        {
            get
            {
                return "print.mode.image";
            }
        }

        public override void PrintTemplate(string dest, string template)
        {
            Image image = HtmlRender.RenderToImage(template);
            using (var file = new FileStream(dest, FileMode.Create))
            {
                image.Save(file, ImageFormat.Png);
            }
        }

        public override string ToString()
        {
            return PrintResource.Mode_PrintToImage;
        }
    }
}
