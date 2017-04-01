using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Client.Printer.Pos.Command
{
    public class AlignRight : RawPrinterCommand
    {
        public override void Apply(EpsonCommander commander)
        {
            commander.Align(Align.Right).CarriageReturn();
        }

        public override void Revert(EpsonCommander commander)
        {
            commander.Align(Align.Left).CarriageReturn();
        }
    }
}
