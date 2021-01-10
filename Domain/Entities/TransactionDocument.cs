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
        public TransactionDocument ReferencedDocument { get; set; }
        public int? ReferencedDocumentId { get; set; }
        public Person Person { get; set; }
        public DateTime? DeleteDate { get; set; }

        public abstract TransactionDocument CreateVoidDocument(IDocumentNumberGenerator numberGenerator);

        public TransactionDocument Void(IDocumentNumberGenerator numberGenerator)
        {
            var document = CreateVoidDocument(numberGenerator);
            document.Total = -Total;
            document.Person = Person;
            document.PersonId = PersonId;
            document.CustomerName = CustomerName;
            document.ReferencedDocument = this;

            IsRefunded = true;

            return document;
        }

        public bool IsReferencingDocument => ReferencedDocumentId.HasValue;
    }
}
