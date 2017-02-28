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
        [XmlAttribute("Lenght")]
        public int Lenght { get; set; }

        public override void Apply(EpsonCommander commander)
        {
            commander.Line(Lenght);
        }

        public override void Revert(EpsonCommander commander)
        {
        }
    }
}
