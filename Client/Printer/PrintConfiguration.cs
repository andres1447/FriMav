using FriMav.Client.Utils;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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
            _types = new List<SelectItem>()
            {
                new SelectItem("Ticket", "Ticket"),
                new SelectItem("Factura", "Invoice"),
                new SelectItem("Reparto", "Delivery"),
                new SelectItem("Lista de precios", "PriceList"),
                new SelectItem("Falta", "Absency"),
                new SelectItem("Adelanto", "Advance"),
                new SelectItem("Mercadería empleados", "EmployeeTicket"),
                new SelectItem("Préstamo", "Loan"),
                new SelectItem("Vacaciones", "Vacation"),
                new SelectItem("Liquidación sueldo", "Payroll"),
                new SelectItem("Cuenta cliente", "CustomerAccount")
            };
        }

        public void Sync()
        {
            var abscentTypes = _types.Where(t => !PrintEntries.Any(pe => pe.Type == t.Value)).ToList();
            foreach (var type in abscentTypes)
            {
                PrintEntries.Add(new PrintEntry(type.Value));
            }
        }

        public void SetPrintModes(IEnumerable<IPrintMode> printModes)
        {
            _printModes = printModes;
        }

        public IEnumerable<SelectItem> GetPrintModes()
        {
            return _printModes.Select(x => new SelectItem(x.ToString(), x.Name));
        }

        public IEnumerable<SelectItem> GetTypes()
        {
            return _types;
        }

        public IEnumerable<SelectItem> GetTemplates()
        {
            var path = Path.Combine(Application.StartupPath, ConfigurationManager.AppSettings["TemplatesPath"]);
            return Directory.GetFiles(path).Select(x => new SelectItem(Path.GetFileName(x), x));
        }

        public void Print(string type, dynamic model)
        {
            var document = PrintEntries.FirstOrDefault(x => x.Type == type && !string.IsNullOrEmpty(x.Mode));
            if (document == null) return;

            var printMode = _printModes.FirstOrDefault(x => x.Applies(document.Mode));
            if (printMode == null) return;

            using (var stream = new StreamReader(document.Template))
            {
                printMode.Print(document.Destination, model, stream.ReadToEnd());
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
        public PrintEntry(string type) : this(type, "", "", "") { }
        public PrintEntry(string type, string destination) : this(type, destination, "") { }

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
