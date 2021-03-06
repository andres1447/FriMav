﻿using System;
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

    public enum CutMode
    {
        Full = 48,
        Partial = 49
    }

    public class EpsonCommander
    {
        public static string ESC = Convert.ToString((char)27);
        public static string GS = Convert.ToString((char)29);
        public static string CR = Convert.ToString((char)13);
        public static string LF = Convert.ToString((char)10);
        public static string FF = Convert.ToString((char)12);

        private StringBuilder _buffer;
        private int _lineLength;
        
        public EpsonCommander(int lineLenght)
        {
            _lineLength = lineLenght;
            _buffer = new StringBuilder();
            _buffer.Clear();
        }

        public EpsonCommander Clear()
        {
            _buffer.Clear();
            return this;
        }

        public EpsonCommander Init()
        {
            _buffer.Append(ESC + "@");
            return this;
        }

        public EpsonCommander FormFeed()
        {
            _buffer.Append(FF);
            return this;
        }

        public EpsonCommander CarriageReturn()
        {
            _buffer.Append(CR);
            return this;
        }

        public EpsonCommander ReverseFeed(int lines)
        {
            _buffer.Append(ESC + "e" + (char)lines);
            return this;
        }

        public EpsonCommander Feed(int lines)
        {
            _buffer.Append(ESC + "d" + (char)lines);
            return this;
        }

        public EpsonCommander Cut(CutMode mode)
        {
            _buffer.Append(GS + "V" + ((char)(int)mode));
            return this;
        }

        public EpsonCommander Line()
        {
            Line(_lineLength);
            return this;
        }

        public EpsonCommander Line(int length)
        {
            Underline(true);
            for (int x = 0; x < length; ++x)
                _buffer.Append(' ');
            Underline(false);
            return NewLine();
        }

        public EpsonCommander Text(string text)
        {
            _buffer.Append(text);
            return this;
        }

        public EpsonCommander Align(Align align)
        {
            _buffer.Append(ESC + "a" + (char)((int)align));
            return this;
        }

        public EpsonCommander Underline(bool on)
        {
            _buffer.Append(ESC + "-" + (on ? (char)1 : (char)0));
            return this;
        }

        public EpsonCommander Emphasis(bool on)
        {
            _buffer.Append(ESC + (on ? "E" : "F"));
            return this;
        }

        public EpsonCommander Double(bool on)
        {
            _buffer.Append(ESC + "G" + (on ? (char)1 : (char)0));
            return this;
        }

        public EpsonCommander CharacterSize(string size)
        {
            _buffer.Append(ESC + "X" + size);
            return this;
        }

        public EpsonCommander NewLine()
        {
            _buffer.Append(CR + LF);
            return this;
        }

        public string Build()
        {
            return _buffer.ToString();
        }
    }
}
