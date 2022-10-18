using System;
using System.Collections.Generic;

namespace FriMav.Application
{
    public class LoanResponse
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public List<LoanFeeResponse> Fees { get; set; }
    }
}