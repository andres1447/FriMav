using System;

namespace FriMav.Domain.Proyections
{
    public class SurplusTransaction
    {
        public int TransactionId { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public decimal Paid { get; set; }

        public decimal Surplus()
        {
            return Total - Paid;
        }
    }
}
