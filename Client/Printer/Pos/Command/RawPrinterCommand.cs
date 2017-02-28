using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FriMav.Client.Printer.Pos.Command
{
    public abstract class RawPrinterCommand
    {
        [XmlElement("Center", typeof(AlignCenter))]
        [XmlElement("Left", typeof(AlignLeft))]
        [XmlElement("Right", typeof(AlignRight))]
        [XmlElement("Emphasis", typeof(Emphasis))]
        [XmlElement("Feed", typeof(Feed))]
        [XmlElement("Line", typeof(Line))]
        [XmlElement("NewLine", typeof(NewLine))]
        [XmlElement("Text", typeof(Text))]
        public List<RawPrinterCommand> Children { get; set; }

        public abstract void Apply(EpsonCommander commander);
        public abstract void Revert(EpsonCommander commander);

        public RawPrinterCommand()
        {
            Children = new List<RawPrinterCommand>();
        }

        public void Execute(EpsonCommander commander)
        {
            Apply(commander);
            foreach (var child in Children)
            {
                child.Execute(commander);
            }
            Revert(commander);
        }
    }
}
