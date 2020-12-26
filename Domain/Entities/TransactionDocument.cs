using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Entities
{
    public abstract class TransactionDocument : Entity
    {
        public int Number { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public bool IsRefunded { get; set; }
        public decimal Total { get; set; }
        public decimal Balance { get; set; }
        public string Description { get; set; }
        public int PersonId { get; set; }
        public string CustomerName { get; set; }
        public TransactionDocument RefundDocument { get; set; }
        public int? RefundDocumentId { get; set; }
        public Person Person { get; set; }
        public decimal PreviousBalance { get; internal set; }
    }
}
