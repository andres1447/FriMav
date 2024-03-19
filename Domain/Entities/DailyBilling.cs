using System;

namespace FriMav.Domain.Entities
{
    public enum BillingSource
    {
        Ticket,
        Invoice
    }

    public class DailyBilling : Entity
    {
        public BillingSource Source { get; set; }
        public decimal Total { get; set; }
    }
}
