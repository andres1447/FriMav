using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Management;
using System.Windows.Forms;

namespace FriMav.Client.Printer
{
    public class PrintToPrinter : AbstractRazorPrintMode
    {
        private readonly WebBrowser _browser;

        public override string Name
        {
            get
            {
                return "print.mode.printer";
            }
        }

        public PrintToPrinter(WebBrowser browser)
        {
            _browser = browser;
            _browser.DocumentCompleted += DocumentCompleted;

            SetFeatureBrowserFeature("FEATURE_BROWSER_EMULATION", 9000);
            SetFeatureBrowserFeature("FEATURE_IVIEWOBJECTDRAW_DMLT9_WITH_GDI", 1);
            SetFeatureBrowserFeature("FEATURE_GPU_RENDERING", 0);
            SetPageSetting("header", "");
            SetPageSetting("footer", "");
            SetPageSetting("margin_top", 0);
            SetPageSetting("margin_left", 0);
            SetPageSetting("margin_right", 0);
            SetPageSetting("margin_bottom", 0);
            SetPageSetting("shrink_To_fit", "no");
            SetPageSetting("PaperSize", 5, RegistryValueKind.DWord);
        }

        public string _printDestination;

        public override void PrintTemplate(string dest, string template)
        {
            _printDestination = dest;
            _browser.ScrollBarsEnabled = false;
            _browser.DocumentText = template;
        }

        private static void SetFeatureBrowserFeature(string feature, uint value)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
            {
                return;
            }

            var appName = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            Registry.SetValue(
              @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\" + feature,
              appName,
              value,
              RegistryValueKind.DWord);
        }

        public static void SetPageSetting<T>(string key, T value)
        {
            Registry.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\PageSetup",
                key,
                value);
        }

        public static void SetPageSetting<T>(string key, T value, RegistryValueKind valueKind)
        {
            Registry.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\PageSetup",
                key,
                value,
                valueKind);
        }

        public string ChangeDefaultPrinter(string printer)
        {
            string currentDefault = null;
            ManagementObject target = null;
            using (ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer"))
            {
                using (ManagementObjectCollection objectCollection = objectSearcher.Get())
                {
                    foreach (ManagementObject mo in objectCollection)
                    {
                        if ((bool)mo["Default"])
                        {
                            currentDefault = mo["Name"].ToString().Trim();
                        }
                        if (string.Compare(mo["Name"].ToString(), printer, true) == 0)
                        {
                            target = mo;
                        }
                    }
                    if (target == null)
                        throw new ArgumentException();

                    target.InvokeMethod("SetDefaultPrinter", null, null);
                }
            }

            return currentDefault;
        }

        void DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var browser = sender as WebBrowser;
            if (null == browser)
            {
                return;
            }

            var currentDefaultPrinter = ChangeDefaultPrinter(_printDestination);

            browser.Print();

            if (!string.IsNullOrEmpty(currentDefaultPrinter))
                ChangeDefaultPrinter(currentDefaultPrinter);
        }

        public override string ToString()
        {
            return PrintResource.Mode_PrintToPrinter;
        }
    }
}
