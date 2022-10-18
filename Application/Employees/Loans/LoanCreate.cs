using System;
using System.Collections.Generic;

namespace FriMav.Application
{
    public class LoanCreate
    {
        public int EmployeeId { get; set; }
        public string Description { get; set; }
        public List<LoanCreateFee> Fees { get; set; }
    }
    public class LoanCreateFee
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}