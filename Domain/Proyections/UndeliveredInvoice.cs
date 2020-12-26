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
        public string PersonCode { get; set; }
        public string PersonName { get; set; }
    }
}
