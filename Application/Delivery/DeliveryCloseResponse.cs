using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public class DeliveryCloseResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Number { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public List<DeliveryCloseInvoice> Invoices { get; set; }
    }

    public class DeliveryCloseInvoice : DeliveryInvoice
    {
        public bool AllowPayment { get; set; }
    }
}
