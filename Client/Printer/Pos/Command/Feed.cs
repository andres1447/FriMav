using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FriMav.Client.Printer.Pos.Command
{
    public class Feed : RawPrinterCommand
    {
        [XmlAttribute("Lines")]
        public int Lines { get; set; }

        public override void Apply(EpsonCommander commander)
        {
            commander.Feed(Lines);
        }

        public override void Revert(EpsonCommander commander)
        {
        }
    }
}
