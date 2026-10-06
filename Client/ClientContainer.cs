using CefSharp;
using CefSharp.WinForms;
using FriMav.Client.Printer;
using FriMav.Client.Printer.Modes;
using FriMav.Client.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FriMav.Client
{
    public partial class ClientContainer : Form
    {
        private ChromiumWebBrowser browser;
        private ChromiumWebBrowser pdfBrowser;
        private CefHelper helper;

        public ClientContainer()
        {
            WindowState = FormWindowState.Maximized;
            InitializeComponent();
        }

        private void ClientContainer_Load(object sender, EventArgs e)
        {
            Cef.Initialize(new CefSettings()
            {
                Locale = "es-AR"
            });

            string start = string.Format("{0}\\App\\dist\\index.html", Application.StartupPath);
            if (!File.Exists(start))
            {
                MessageBox.Show("Error The html file doesn't exists : " + start);
            }

            browser = new ChromiumWebBrowser(start);
            pdfBrowser = CreatePdfBrowser();
            helper = new CefHelper(browser);

            browser.RegisterJsObject("CefHelper", helper);
            browser.RegisterJsObject("PrintHelper", new PrintHelper(Application.StartupPath + "\\PrintConfig.json", CreatePrintModes()));
            browser.RegisterJsObject("ClientConfig", new ClientConfig
            {
                Host = ConfigurationManager.AppSettings["ApiHost"]
            });
            this.Controls.Add(browser);

            browser.Dock = DockStyle.Fill;
            browser.BringToFront();
            //browser.IsBrowserInitializedChanged += ShowDevTools;
            // Allow the use of local resources in the browser
            BrowserSettings browserSettings = new BrowserSettings();
            browserSettings.FileAccessFromFileUrls = CefState.Enabled;
            browserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            browser.BrowserSettings = browserSettings;
            browser.Focus();
        }

        private IEnumerable<IPrintMode> CreatePrintModes()
        {
            var browser = new WebBrowser();
            Controls.Add(browser);
            browser.Visible = false;
            return new List<IPrintMode>()
            {
                new PrintOff(),
                new PrintToPrinter(browser),
                new PrintToImage(),
                new PrintToPDF(pdfBrowser),
                new PrintToFile(),
                new PrintEpsonCommandsToPort(),
                new PrintEpsonCommandsToPrinter()
            };
        }

        private ChromiumWebBrowser CreatePdfBrowser()
        {
            // ChromiumWebBrowser defaults to Dock.Fill. A second fill browser
            // consumes the whole form and leaves the app with no space to render.
            var browser = new ChromiumWebBrowser("about:blank")
            {
                Dock = DockStyle.None,
                Size = new Size(1, 1),
                Location = new Point(0, 0),
                TabStop = false
            };
            Controls.Add(browser);
            browser.SendToBack();
            return browser;
        }

        private void ShowDevTools(object sender, IsBrowserInitializedChangedEventArgs e)
        {
            if (e.IsBrowserInitialized)
                helper.showDevTools();
        }

        private void ClientContainer_FormClosing(object sender, FormClosingEventArgs e)
        {
            CefSharpSettings.WcfTimeout = TimeSpan.Zero;
            if (pdfBrowser != null)
            {
                pdfBrowser.Dispose();
                Controls.Remove(pdfBrowser);
            }
            browser.Dispose();
            Controls.Remove(browser);
            Cef.Shutdown();
        }
    }
}
