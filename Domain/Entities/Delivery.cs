using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Entities
{
    public class Delivery : Entity
    {
        public int Number { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public DateTime? DeleteDate { get; set; }
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public void Delete()
        {
            DeleteDate = DateTime.UtcNow;
        }
    }
}
