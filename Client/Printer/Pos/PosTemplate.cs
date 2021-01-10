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
        [XmlAttribute]
        public int LinesPerPage { get; set; }

        [XmlAttribute]
        public int CharactersPerLine { get; set; }

        [XmlAttribute]
        public int InlineCopies { get; set; }

        [XmlElement("Header")]
        public PosTemplateSection Header { get; set; }

        [XmlElement("Body")]
        public PosTemplateSection Body { get; set; }

        [XmlElement("Footer")]
        public PosTemplateSection Footer { get; set; }

        [XmlElement("Center", typeof(AlignCenter))]
        [XmlElement("Left", typeof(AlignLeft))]
        [XmlElement("Right", typeof(AlignRight))]
        [XmlElement("Emphasis", typeof(Emphasis))]
        [XmlElement("Feed", typeof(Feed))]
        [XmlElement("Line", typeof(Line))]
        [XmlElement("NewLine", typeof(NewLine))]
        [XmlElement("Text", typeof(Text))]
        [XmlElement("CutPartial", typeof(CutPartial))]
        [XmlElement("CutFull", typeof(CutFull))]
        public List<RawPrinterCommand> Commands { get; set; }

        public PosTemplate()
        {
            CharactersPerLine = 40;
            InlineCopies = 0;
            LinesPerPage = 0;
            Commands = new List<RawPrinterCommand>();
            Header = new PosTemplateSection();
            Body = new PosTemplateSection();
            Footer = new PosTemplateSection();
        }

        public int GetTotalLines()
        {
            return (Header.GetLineCount() + Body.GetLineCount() + Footer.GetLineCount()) * (1 + InlineCopies);
        }

        public bool FitsInOnePage()
        {
            return LinesPerPage == 0 || (GetTotalLines() <= LinesPerPage);
        }

        public int GetFixedLinesPerPage()
        {
            return Header.Commands.Count + Footer.Commands.Count;
        }

        public int GetMaxBodyLinesPerPage()
        {
            return (int)Math.Ceiling((decimal)(Body.Commands.Count / (LinesPerPage - GetFixedLinesPerPage())));
        }

        public IEnumerable<string> Execute()
        {
            var pages = new List<string>();
            var helper = new EpsonCommander(CharactersPerLine);
            foreach (var command in Commands)
            {
                command.Execute(helper);
            }
            pages.Add(helper.Build());
            /*if (FitsInOnePage())
            {
                helper.Init();
                for (int x = 0; x < InlineCopies + 1; x++)
                {
                    Header.ExecuteSection(helper);
                    Body.ExecuteSection(helper);
                    Footer.ExecuteSection(helper);
                }
                helper.FormFeed();
                helper.Init();
                pages.Add(helper.Build());
            }
            else
            {
                for (int x = 0; x < InlineCopies + 1; x++)
                {
                    helper.Clear();
                    helper.Init();
                    Header.ExecuteSection(helper);
                    Body.ExecuteSection(helper);
                    Footer.ExecuteSection(helper);
                    helper.FormFeed();
                    helper.Init();
                    pages.Add(helper.Build());
                }
            }*/
            return pages;
        }
    }

    public class PosTemplateSection
    {
        [XmlAttribute]
        public int MinLines { get; set; }

        [XmlElement("Center", typeof(AlignCenter))]
        [XmlElement("Left", typeof(AlignLeft))]
        [XmlElement("Right", typeof(AlignRight))]
        [XmlElement("Emphasis", typeof(Emphasis))]
        [XmlElement("Feed", typeof(Feed))]
        [XmlElement("Line", typeof(Line))]
        [XmlElement("NewLine", typeof(NewLine))]
        [XmlElement("Text", typeof(Text))]
        [XmlElement("CutPartial", typeof(CutPartial))]
        [XmlElement("CutFull", typeof(CutFull))]
        public List<RawPrinterCommand> Commands { get; set; }

        public PosTemplateSection()
        {
            Commands = new List<RawPrinterCommand>();
        }

        public int GetLineCount()
        {
            return Math.Min(Commands.Count, MinLines);
        }

        public void ExecuteSection(EpsonCommander helper)
        {
            foreach (var command in Commands)
            {
                command.Execute(helper);
                if (!command.HasNewLine)
                {
                    helper.NewLine();
                }
            }
            for (int x = 0; x < MinLines - Commands.Count; x++)
            {
                helper.NewLine();
            }
        }
    }
}
