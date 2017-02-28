using System;

namespace FriMav.Domain.Proyections
{
    public class UnpaidInvoice
    {
        public int InvoiceId { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public decimal Paid { get; set; }

        public decimal Remaining()
        {
            return Total - Paid;
        }
    }
}
