using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Entities
{
    public enum TransactionDocumentType : int
    {
        Invoice = 1,
        Payment = 2,
        CreditNote = 3,
        DebitNote = 4
    }
}
