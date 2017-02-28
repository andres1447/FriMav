using CefSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FriMav.Client.Utils
{
    public class CefHelper
    {
        private JsonSerializerSettings _jsonSerializerSettings;
        private IWebBrowser _browser;

        public CefHelper(IWebBrowser browser)
        {
            _browser = browser;
            _jsonSerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public void showDevTools()
        {
            _browser.ShowDevTools();
        }
    }
}
