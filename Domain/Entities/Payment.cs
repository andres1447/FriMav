using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Entities
{
    public class Payment : TransactionDocument
    {
        public override TransactionDocument CreateVoidDocument(IDocumentNumberGenerator numberGenerator)
        {
            return new DebitNote { Number = numberGenerator.NextForDebitNote() };
        }
    }
}
