﻿using System;
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
            commander.CarriageReturn().Align(Align.Right);
        }

        public override void Revert(EpsonCommander commander)
        {
            commander.CarriageReturn().Align(Align.Left);
        }
    }
}
