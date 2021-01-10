using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Client.Models
{
    public class DeliveryModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Number { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public List<DeliveryInvoiceModel> Invoices { get; set; }
        public List<DeliveryProductModel> Products { get; set; }
    }

    public class DeliveryInvoiceModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Number { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryZone { get; set; }
        public decimal Total { get; set; }
        public decimal Balance { get; set; }
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
        public int ProductId { get; set; }
    }
}
