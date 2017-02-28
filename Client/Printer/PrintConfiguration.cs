using FriMav.Client.Models;
using FriMav.Client.Printer.Modes;
using FriMav.Client.Utils;
using FriMav.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FriMav.Client.Printer
{
    public class PrintConfiguration
    {
        public List<PrintEntry> PrintEntries { get; set; }

        private List<SelectItem> _types;
        private IEnumerable<IPrintMode> _printModes;

        public PrintConfiguration()
        {
            PrintEntries = new List<PrintEntry>();
            _printModes = new List<IPrintMode>()
            {
                new PrintToPrinter(),
                new PrintToImage(),
                new PrintToPDF(),
                new PrintEpsonCommandsToPort(),
                new PrintEpsonCommandsToPrinter()
            };
            _types = new List<SelectItem>()
            {
                new SelectItem("Ticket", "Ticket"),
                new SelectItem("Factura", "Invoice"),
                new SelectItem("Lista de precios", "PriceList")
            };
        }

        public IEnumerable<SelectItem> GetPrintModes()
        {
            return _printModes.Select(x => new SelectItem(x.ToString(), x.Name));
        }

        public IEnumerable<SelectItem> GetTypes()
        {
            return _types;
        }

        public IEnumerable<string> GetTemplates()
        {
            return Directory.GetFiles(ConfigurationManager.AppSettings["TemplatesPath"]);
        }

        public void Print(string type, dynamic model)
        {
            var document = PrintEntries.FirstOrDefault(x => x.Type == type);
            using (var stream = new StreamReader(document.Template))
            {
                _printModes.FirstOrDefault(x => x.Applies(document.Mode))
                            .Print(document.Destination, model, stream.ReadToEnd());
            }
        }
    }

    public class PrintEntry
    {
        public string Type { get; set; }
        public string Destination { get; set; }
        public string Template { get; set; }
        public string Mode { get; set; }

        public PrintEntry() : this("", "", "", "") { }

        public PrintEntry(string type, string destination) : this(destination, "", "") { }

        public PrintEntry(string type, string destination, string template) : this(type, destination, template, "") { }

        public PrintEntry(string type, string destination, string template, string mode)
        {
            Type = type;
            Destination = destination;
            Template = template;
            Mode = mode;
        }
    }
}
