using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FriMav.Client.Printer.Pos.Command
{
    public class NewLine : RawPrinterCommand
    {
        public override void Apply(EpsonCommander commander)
        {
        }

        public override void Revert(EpsonCommander commander)
        {
            commander.NewLine();
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
