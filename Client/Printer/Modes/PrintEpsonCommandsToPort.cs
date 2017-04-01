using System;
using System.Drawing;
using System.Drawing.Printing;
using FriMav.Client.Models;
using TheArtOfDev.HtmlRenderer.WinForms;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.IO;
using FriMav.Client.Printer.Pos;

namespace FriMav.Client.Printer
{
    public class PrintEpsonCommandsToPort : AbstractRazorPrintMode
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        public override string Name
        {
            get
            {
                return "print.mode.esc.port";
            }
        }

        public override void PrintTemplate(string dest, string template)
        {
            var pages = PosTemplateProcessor.Print(template);
            foreach (var page in pages)
            {
                Print(dest, page);
            }
        }

        public void Print(string dest, string template)
        {
            Byte[] buffer = new byte[template.Length];
            buffer = System.Text.Encoding.ASCII.GetBytes(template);

            try
            {
                SafeFileHandle fh = CreateFile(dest + ":", FileAccess.Write, 0, IntPtr.Zero, FileMode.OpenOrCreate, 0, IntPtr.Zero);
                if (!fh.IsInvalid)
                {
                    FileStream fs = new FileStream(fh, FileAccess.ReadWrite);
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Close();
                }

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }

        public override string ToString()
        {
            return PrintResource.Mode_PrintEpsonCommandsToPort;
        }
    }
}
