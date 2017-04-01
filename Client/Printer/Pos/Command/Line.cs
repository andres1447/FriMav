using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FriMav.Client.Printer.Pos.Command
{
    public class Line : RawPrinterCommand
    {
        [XmlText]
        public string Lenght { get; set; }

        public override void Apply(EpsonCommander commander)
        {
            if (!string.IsNullOrEmpty(Lenght))
            {
                commander.Line(Convert.ToInt32(Lenght));
            }
            else
            {
                commander.Line();
            }
        }

        public override void Revert(EpsonCommander commander)
        {
        }

        public override bool HasNewLine
        {
            get
            {
                return true;
            }
        }
    }
}
