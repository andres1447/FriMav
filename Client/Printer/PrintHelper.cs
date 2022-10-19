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

        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings()
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private static readonly Dictionary<string, Type> Models = new Dictionary<string, Type>
        {
            { "Ticket", typeof(TicketModel) },
            { "Invoice", typeof(InvoiceModel) },
            { "PriceList", typeof(CatalogModel) },
            { "Delivery", typeof(DeliveryModel) },
            { "EmployeeTicket", typeof(EmployeeTicketModel) },
            { "Absency", typeof(EmployeeAbsencyModel) },
            { "Advance", typeof(EmployeeAdvanceModel) },
            { "Loan", typeof(EmployeeLoanModel) },
            { "Payroll", typeof(PayrollModel) },
            { "CustomerAccount", typeof(CustomerAccountModel) },
            { "Vacation", typeof(EmployeeVacationModel) }
        };

        private PrintConfiguration Configuration { get; set; }

        public PrintHelper(string configPath, IEnumerable<IPrintMode> printModes)
        {
            _configPath = configPath;
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
                    JsonConvert.DeserializeObject<List<PrintEntry>>(jsonPrintDocument, SerializerSettings);
                SavePrintConfiguration();
            });
        }

        public void Print(string modelType, string jsonModel)
        {
            if (!Models.ContainsKey(modelType)) return;

            var model = JsonConvert.DeserializeObject(jsonModel, Models[modelType], SerializerSettings);
            Configuration.Print(modelType, model);
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
