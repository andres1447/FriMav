using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Entities
{
    public class CreditNote : TransactionDocument
    {
        public override TransactionDocument CreateVoidDocument(IDocumentNumberGenerator numberGenerator)
        {
            return new DebitNote() { Number = numberGenerator.NextForCreditNote() };
        }
    }
}
