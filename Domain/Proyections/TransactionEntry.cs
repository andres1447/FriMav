using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Proyections
{
    public class TransactionEntry
    {
        public int TransactionId { get; set; }
        public TransactionType TransactionType { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public decimal Balance { get; set; }
        public string Description { get; set; }
        public int FiscalYear { get; set; }
        public int PersonId { get; set; }
        public int? ReferenceId { get; set; }

        public TransactionEntryReference Reference { get; set; }
    }

    public class TransactionEntryReference
    {
        public int TransactionId { get; set; }
        public int Number { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
