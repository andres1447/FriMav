using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FriMav.Client
{
    static class Client
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-AR");

            if (!Directory.Exists(string.Format("{0}\\Temp", Application.StartupPath)))
            {
                Directory.CreateDirectory(string.Format("{0}\\Temp", Application.StartupPath));
            }

            Application.Run(new ClientContainer());
        }
    }
}
