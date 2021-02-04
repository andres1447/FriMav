using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Entities.Payrolls
{
    public class Payroll : Entity
    {
        public DateTime? DeleteDate { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal Balance { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }

        public List<LiquidationDocument> Liquidation { get; set; }
    }
}
