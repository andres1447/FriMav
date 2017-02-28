using FriMav.Client.Printer.Pos.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FriMav.Client.Printer.Pos
{
    [XmlRoot("EscCommandTemplate")]
    public class PosTemplate
    {
        [XmlElement("Center", typeof(AlignCenter))]
        [XmlElement("Left", typeof(AlignLeft))]
        [XmlElement("Right", typeof(AlignRight))]
        [XmlElement("Emphasis", typeof(Emphasis))]
        [XmlElement("Feed", typeof(Feed))]
        [XmlElement("Line", typeof(Line))]
        [XmlElement("NewLine", typeof(NewLine))]
        [XmlElement("Text", typeof(Text))]
        public List<RawPrinterCommand> Commands { get; set; }

        public PosTemplate()
        {
            Commands = new List<RawPrinterCommand>();
        }
    }
}
