using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Entities.Payrolls
{
    public class Loan : Entity
    {
        public Employee Employee { get; set; }
        public DateTime? DeleteDate { get; set; }
        public ICollection<LoanFee> Fees {get;set;}
        public int EmployeeId { get; set; }

        public void Delete()
        {
            DeleteDate = DateTime.UtcNow;
            foreach (var fee in Fees)
            {
                fee.Delete();
            }
        }
    }
}
