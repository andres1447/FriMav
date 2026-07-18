using System;

namespace FriMav.Application.Employees
{
    public class TransferenceCreate
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int EmployeeId { get; set; }
    }
}