using System;
using System.Collections.Generic;
using System.Linq;

namespace FriMav.Client.Models
{
    public class InvoiceModel
    {
        public DateTime Date { get; set; }
        public string Number { get; set; }
        public string DeliveryAddress { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public decimal Balance { get; set; }
        public IEnumerable<InvoiceItemModel> Items { get; set; }

        public decimal Total()
        {
            return Items.Sum(x => x.Amount());
        }
    }

    public class InvoiceItemModel
    {
        public string Product { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }

        public decimal Amount()
        {
            return Quantity * Price;
        }
    }
}
