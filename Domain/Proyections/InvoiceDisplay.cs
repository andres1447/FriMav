using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;

namespace FriMav.Domain.Proyections
{
    public class InvoiceDisplay
    {
        public int InvoiceId { get; set; }
        public DateTime Date { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal Total { get; set; }
        public int PersonId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public Shipping Shipping { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public ICollection<InvoiceItemDisplay> Items { get; set; }

        public InvoiceDisplay()
        {
            Items = new List<InvoiceItemDisplay>();
        }
    }

    public class InvoiceItemDisplay
    {
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
