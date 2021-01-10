using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public class DeliveryClose
    {
        public int Id { get; set; }
        public List<DeliveryPayment> Payments { get; set; }
    }

    public class DeliveryPayment
    {
        public int InvoiceId { get; set; }
        public decimal Total { get; set; }
    }
}
