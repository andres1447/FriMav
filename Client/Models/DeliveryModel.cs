using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Client.Models
{
    public class DeliveryModel
    {
        public DateTime Date { get; set; }
        public DeliveryEmployeeModel Employee { get; set; }
        public DeliveryInvoiceModel Invoices { get; set; }
        public DeliveryProductModel Products { get; set; }
    }

    public class DeliveryEmployeeModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class DeliveryInvoiceModel
    {
        public int Number { get; set; }
        public DeliveryCustomerModel Customer { get; set; }
        public decimal Total { get; set; }
    }

    public class DeliveryCustomerModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class DeliveryProductModel
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
    }
}
