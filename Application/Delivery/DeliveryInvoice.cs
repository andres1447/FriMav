using System;

namespace FriMav.Application
{
    public class DeliveryInvoice
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Number { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryZone { get; set; }
        public decimal Total { get; internal set; }
        public decimal PreviousBalance { get; internal set; }
        public decimal Balance { get; internal set; }
    }
}
