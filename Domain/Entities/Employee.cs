using FriMav.Domain.Entities.Payrolls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Entities
{
    public class Employee : Person
    {
        public const int LaborableDays = 6;

        public decimal Salary { get; set; }
        public DateTime JoinDate { get; set; }

        public decimal DailySalary()
        {
            return Salary / LaborableDays;
        }

        public void Accept(Payroll payroll)
        {
            Balance = payroll.Balance;
            if (Balance > 0)
                Balance = 0;
        }
    }
}
