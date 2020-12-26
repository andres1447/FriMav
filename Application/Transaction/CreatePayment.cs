using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public class CreatePayment
    {
        public decimal Total { get; set; }
        public string Description { get; set; }
        public int PersonId { get; set; }
    }
}
