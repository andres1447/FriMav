using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain
{
    public interface IDocumentNumberGenerator
    {
        int NextForDelivery();
        int NextForInvoice();
        int NextForPayment();
    }
}
