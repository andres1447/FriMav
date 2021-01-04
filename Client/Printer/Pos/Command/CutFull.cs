using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FriMav.Client.Printer.Pos.Command
{
    public class CutFull : RawPrinterCommand
    {
        public override void Apply(EpsonCommander commander)
        {
            commander.Cut(CutMode.Partial);
        }

        public override void Revert(EpsonCommander commander)
        {
        }
    }
}
