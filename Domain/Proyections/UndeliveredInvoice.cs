using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Proyections
{
    public class UndeliveredInvoice
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int PersonId { get; set; }
        public DateTime Date { get; set; }
        public string CustomerCode { get; set; }
        public decimal Total { get; set; }
        public string CustomerName { get; set; }
        public string DeliveryAddress { get; set; }
        public object Products { get; set; }
    }
}
