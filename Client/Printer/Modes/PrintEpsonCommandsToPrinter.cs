using FriMav.Client.Printer.Pos;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace FriMav.Client.Printer.Modes
{
    public class PrintEpsonCommandsToPrinter : AbstractRazorPrintMode
    {
        public override string Name
        {
            get
            {
                return "print.mode.raw.printer";
            }
        }

        public override void PrintTemplate(string dest, string template)
        {
            RawPrinterHelper.SendStringToPrinter(dest, PosTemplateProcessor.Build(template));
        }

        public override string ToString()
        {
            return PrintResource.Mode_PrintEpsonCommandsToPrinter;
        }
    }
}
