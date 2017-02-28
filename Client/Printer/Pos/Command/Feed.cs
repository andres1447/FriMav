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
        [XmlText]
        public string Lines { get; set; }

        public override void Apply(EpsonCommander commander)
        {
            commander.Feed(Convert.ToInt32(Lines));
        }

        public override void Revert(EpsonCommander commander)
        {
        }
    }
}
