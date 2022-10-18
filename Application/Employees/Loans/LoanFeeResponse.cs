using System;

namespace FriMav.Application
{
    public class LoanFeeResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public bool IsLiquidated { get; set; }
    }
}