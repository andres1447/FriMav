using System;

namespace FriMav.Domain.Entities.Payrolls
{
    public class LoanFee : LiquidationDocument
    {
        public int LoanId { get; set; }
    }
}