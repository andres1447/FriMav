using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Client.Printer.Pos
{
    public enum Align
    {
        Left = 0,
        Center = 1,
        Right = 2
    }

    public enum Cut
    {
        Full = 65,
        Partial = 66
    }

    public class EpsonCommander
    {
        public static string ESC = Convert.ToString((char)27);
        public static string GS = Convert.ToString((char)29);
        public static string CR = Convert.ToString((char)13);
        public static string LF = Convert.ToString((char)10);
        public static string FF = Convert.ToString((char)12);

        private string _buffer;
        private int _lineLength;

        public EpsonCommander(int lineLenght)
        {
            _lineLength = lineLenght;
            _buffer = "";
            Init();
        }

        public EpsonCommander Init()
        {
            _buffer += (ESC + "@");
            return this;
        }

        public EpsonCommander FormFeed()
        {
            _buffer += FF;
            return this;
        }

        public EpsonCommander CarriageReturn()
        {
            _buffer += CR;
            return this;
        }

        public EpsonCommander ReverseFeed(int lines)
        {
            _buffer += (ESC + "e" + (char)lines);
            return this;
        }

        public EpsonCommander Feed(int lines)
        {
            _buffer += (ESC + "d" + (char)lines);
            return this;
        }

        public EpsonCommander Cut(Cut mode, int lines)
        {
            _buffer += (GS + "V" + ((char)(int)mode) + (char)lines);
            return this;
        }

        public EpsonCommander Line(int length)
        {
            Underline(true);
            for (int x = 0; x < length; ++x)
                _buffer += ' ';
            Underline(false);
            return NewLine();
        }

        public EpsonCommander Text(string text)
        {
            _buffer += text;
            return this;
        }

        public EpsonCommander Align(Align align)
        {
            _buffer += (ESC + "a" + (char)((int)align));
            return this;
        }

        public EpsonCommander Underline(bool on)
        {
            _buffer += (ESC + "-" + (on ? (char)1 : (char)0));
            return this;
        }

        public EpsonCommander Emphasis(bool on)
        {
            _buffer += (ESC + "E" + (on ? (char)1 : (char)0));
            return this;
        }

        public EpsonCommander Double(bool on)
        {
            _buffer += (ESC + "G" + (on ? (char)1 : (char)0));
            return this;
        }

        public EpsonCommander NewLine()
        {
            _buffer += (CR + LF);
            return this;
        }

        public string Build()
        {
            return _buffer;
        }
    }
}
