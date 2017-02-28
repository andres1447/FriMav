using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FriMav.Client.Printer.Pos.Command
{
    public class Text : RawPrinterCommand
    {
        [XmlText]
        public string Value { get; set; }

        public override void Apply(EpsonCommander commander)
        {
            commander.Text(Value);
        }

        public override void Revert(EpsonCommander commander)
        {
        }
    }
}
