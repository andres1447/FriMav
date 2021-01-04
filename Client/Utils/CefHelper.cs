using CefSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FriMav.Client.Utils
{
    public class CefHelper
    {
        private IWebBrowser _browser;

        public CefHelper(IWebBrowser browser)
        {
            _browser = browser;
        }

        public void showDevTools()
        {
            _browser.ShowDevTools();
        }
    }
}
