using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Proyections
{
    public class DeliveryDisplay
    {
        public DateTime Date { get; set; }
        public DeliveryEmployee Employee { get; set; }
        public IEnumerable<DeliveryItem> Items { get; set; }
        public IEnumerable<DeliveryInvoice> Invoices { get; set; }
    }

    public class DeliveryEmployee
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
    }

    public class DeliveryInvoice
    {
        public int TransactionId { get; set; }
        public int PersonId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }

    public class DeliveryItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
    }
}
