using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Entities.Payrolls
{
    public class LiquidationDocument : Entity
    {
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime? DeleteDate { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public void Delete()
        {
            DeleteDate = DateTime.UtcNow;
        }
    }
}
