using System;
using System.Collections.Generic;

namespace FriMav.Application
{
    public class PayrollResponse
    {
        public DateTime Date { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public decimal Salary { get; set; }
        public List<UnliquidatedDocument> Liquidation { get; set; }
        public decimal Balance { get; set; }
        public decimal Total { get; set; }
        public int EmployeeId { get; set; }
    }
}