using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FriMav.Client.Printer.Pos.Command
{
    public class Text : RawPrinterCommand
    {
        [XmlAttribute("Align")]
        public string Alignment { get; set; }

        [XmlAttribute]
        public string Width { get; set; }

        [XmlText]
        public string Value { get; set; }

        public override void Apply(EpsonCommander commander)
        {
            string data;
            if (string.IsNullOrEmpty(Width))
            {
                data = Value;
            }
            else
            {
                int width = Convert.ToInt32(Width);
                string text = Normalize(Value, width);
                data = new string(' ', width);
                switch (Alignment)
                {
                    case "C": data = data.Insert(GetCenteredOffset(width, text.Length), text); break;
                    case "R": data = data.Insert(width - text.Length, text); break;
                    case "L":
                    default: data = data.Insert(0, text); break;
                }
            }
            commander.Text(data);
        }

        protected int GetCenteredOffset(int width, int length)
        {
            return Math.Max(((width / 2) - (length / 2)), 0);
        }

        protected string Normalize(string value, int width)
        {
            if (value.Length > width)
            {
                value.Substring(0, width - 3);
                return value + "...";
            }
            return value;
        }

        public override void Revert(EpsonCommander commander)
        {
        }
    }
}
