using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Proyections
{
    public class DeliveryListing
    {
        public int DeliveryId { get; set; }
        public DateTime Date { get; set; }
        public string Employee { get; set; }
        public int Invoices { get; set; }
    }
}
