using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using FriMav.Client.Models;
using System;

namespace FriMav.Client.Printer
{
    public class PrintHelper
    {
        private string _configPath;
        private JsonSerializerSettings _jsonSerializerSettings;
        private PrintConfiguration Configuration { get; set; }
        private IDictionary<string, Type> _models;

        public PrintHelper(string configPath, IEnumerable<IPrintMode> printModes)
        {
            _models = new Dictionary<string, Type>();
            _models.Add("Ticket", typeof(TicketModel));
            _models.Add("Invoice", typeof(InvoiceModel));
            _models.Add("PriceList", typeof(CatalogModel));
            _models.Add("Delivery", typeof(DeliveryModel));

            _configPath = configPath;
            _jsonSerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            Configuration = LoadPrintConfiguration();
            Configuration.SetPrintModes(printModes);
        }

        public string GetPrinters()
        {
            return CefFxResponse.WrapResult(() => {
                var printers = new List<string>();
                foreach (var printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    printers.Add(printer.ToString());
                }
                return printers;
            });
        }

        public string GetPrintModes()
        {
            return CefFxResponse.WrapResult(() => Configuration.GetPrintModes());
        }

        public string GetPrintEntries()
        {
            return CefFxResponse.WrapResult(() => Configuration.PrintEntries);
        }

        public string GetTemplates()
        {
            return CefFxResponse.WrapResult(() => Configuration.GetTemplates());
        }

        public string GetTypes()
        {
            return CefFxResponse.WrapResult(() => Configuration.GetTypes());
        }

        public string UpdatePrintEntries(string jsonPrintDocument)
        {
            return CefFxResponse.WrapResult(() =>
            {
                Configuration.PrintEntries =
                    JsonConvert.DeserializeObject<List<PrintEntry>>(jsonPrintDocument, _jsonSerializerSettings);
                SavePrintConfiguration();
            });
        }

        public void Print(string modelType, string jsonModel)
        {
            if (_models.ContainsKey(modelType))
            {
                var model = JsonConvert.DeserializeObject(jsonModel, _models[modelType], _jsonSerializerSettings);
                Configuration.Print(modelType, model);
            }
        }

        private PrintConfiguration InitPrintConfiguration()
        {
            var config = new PrintConfiguration();
            foreach (var type in config.GetTypes())
            {
                config.PrintEntries.Add(new PrintEntry { Type = type.Value });
            }
            return config;
        }

        private void SavePrintConfiguration()
        {
            File.WriteAllText(_configPath, JsonConvert.SerializeObject(Configuration, Formatting.Indented));
        }

        private PrintConfiguration LoadPrintConfiguration()
        {
            if (!File.Exists(_configPath)) return InitPrintConfiguration();

            var content = File.ReadAllText(_configPath);
            var printConfig = JsonConvert.DeserializeObject<PrintConfiguration>(content);
            printConfig.Sync();
            return printConfig;
        }
    }
}
