using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public class DeliveryResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Number { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public List<DeliveryInvoice> Invoices { get; set; }
        public List<DeliveryProduct> Products { get; set; }
    }
}
