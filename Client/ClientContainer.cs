using CefSharp;
using CefSharp.WinForms;
using FriMav.Client.Printer;
using FriMav.Client.Utils;
using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace FriMav.Client
{
    public partial class ClientContainer : Form
    {
        private ChromiumWebBrowser browser;
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
            helper = new CefHelper(browser);
            browser.RegisterJsObject("CefHelper", helper);
            browser.RegisterJsObject("PrintHelper", new PrintHelper(Application.StartupPath + "\\PrintConfig.xml"));
            browser.RegisterJsObject("ClientConfig", new ClientConfig
            {
                Host = ConfigurationManager.AppSettings["ApiHost"]
            });
            this.Controls.Add(browser);

            browser.Dock = DockStyle.Fill;
            browser.LoadingStateChanged += loaded;
            // Allow the use of local resources in the browser
            BrowserSettings browserSettings = new BrowserSettings();
            browserSettings.FileAccessFromFileUrls = CefState.Enabled;
            browserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            browser.BrowserSettings = browserSettings;
        }

        private void loaded(object sender, LoadingStateChangedEventArgs e)
        {
            /*if (e.IsLoading == true)
                helper.showDevTools();*/
        }

        private void ClientContainer_FormClosing(object sender, FormClosingEventArgs e)
        {
            CefSharpSettings.WcfTimeout = TimeSpan.Zero;
            browser.Dispose();
            this.Controls.Remove(browser);
            Cef.Shutdown();
        }
    }
}
