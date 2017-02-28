using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Client.Printer.Pos.Command
{
    public class AlignLeft : RawPrinterCommand
    {
        public override void Apply(EpsonCommander commander)
        {
            commander.CarriageReturn().Align(Align.Left);
        }

        public override void Revert(EpsonCommander commander)
        {
        }
    }
}
