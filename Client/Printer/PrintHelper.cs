using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
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

        public PrintHelper(string configPath)
        {
            _models = new Dictionary<string, Type>();
            _models.Add("Ticket", typeof(TicketModel));
            _models.Add("Invoice", typeof(InvoiceModel));
            _models.Add("PriceList", typeof(CatalogModel));

            _configPath = configPath;
            _jsonSerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            Configuration = (!File.Exists(_configPath)) ? InitPrintConfiguration() : LoadPrintConfiguration();
        }

        public string GetPrinters()
        {
            var printers = new List<string>();
            foreach (var printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                printers.Add(printer.ToString());
            }
            return JsonConvert.SerializeObject(printers);
        }

        public string GetPrintModes()
        {
            return JsonConvert.SerializeObject(Configuration.GetPrintModes());
        }

        public string GetPrintEntries()
        {
            return JsonConvert.SerializeObject(Configuration.PrintEntries);
        }

        public string GetTemplates()
        {
            return JsonConvert.SerializeObject(Configuration.GetTemplates());
        }

        public string GetTypes()
        {
            return JsonConvert.SerializeObject(Configuration.GetTypes());
        }

        public void UpdatePrintEntries(string jsonPrintDocument)
        {
            Configuration.PrintEntries =
                JsonConvert.DeserializeObject<List<PrintEntry>>(jsonPrintDocument, _jsonSerializerSettings);
            SavePrintConfiguration();
        }

        public void Print(string modelType, string jsonModel)
        {
            if (_models.ContainsKey(modelType))
            {
                var model = JsonConvert.DeserializeObject(jsonModel, _models[modelType], _jsonSerializerSettings);
                Configuration.Print(modelType, model);
            }
        }

        #region Private

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
            using (var fs = new FileStream(_configPath, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PrintConfiguration));
                serializer.Serialize(fs, Configuration);
            }
        }

        private PrintConfiguration LoadPrintConfiguration()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PrintConfiguration));
            using (var fs = new FileStream(_configPath, FileMode.Open))
            {
                return (PrintConfiguration)serializer.Deserialize(fs);
            }
        }

        #endregion
    }
}
