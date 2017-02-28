using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Client.Printer.Pos.Command
{
    public class Emphasis : RawPrinterCommand
    {
        public override void Apply(EpsonCommander commander)
        {
            commander.Emphasis(true);
        }

        public override void Revert(EpsonCommander commander)
        {
            commander.Emphasis(false);
        }
    }
}
