using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Proyections
{
    public class TransactionEntry
    {
        public int Id { get; set; }
        public TransactionDocumentType TransactionType { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public decimal Balance { get; set; }
        public string Description { get; set; }
        public int PersonId { get; set; }
        public bool IsRefunded { get; set; }
        public string ExternalDocumentNumber { get; set; }
        public TransactionEntryReference RefundedDocument { get; set; }
    }

    public class TransactionEntryReference
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public TransactionDocumentType TransactionType { get; set; }
    }
}
