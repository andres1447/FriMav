using System;
using System.Collections.Generic;

namespace FriMav.Client.Models
{
    public class OrderModel
    {
        public DateTime Date { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public IEnumerable<OrderItemModel> Items { get; set; }
    }

    public class OrderItemModel
    {
        public string Product { get; set; }
        public decimal Quantity { get; set; }
        public string Measure { get; set; }
    }
}
