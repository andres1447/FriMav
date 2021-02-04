using FriMav.Domain.Entities.Payrolls;
using System;

namespace FriMav.Application
{
    public class UnliquidatedDocument
    {
        public decimal Amount { get; set; }
        public LiquidationDocumentType Type { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Balance { get; set; }
        public int Id { get; set; }
        public int? LoanId { get; set; }
    }
}