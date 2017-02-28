using System;
using System.Collections.Generic;

namespace FriMav.Domain.Proyections
{
    public class PaymentDisplay
    {
        public int PaymentId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Total { get; set; }
        public int PersonId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }

        public PaymentDisplay()
        {
        }
    }
}
