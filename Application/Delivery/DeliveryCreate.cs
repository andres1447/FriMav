using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public class DeliveryCreate
    {
        public int EmployeeId { get; set; }
        public ICollection<int> Invoices { get; set; }
    }
}
