using System;

namespace FriMav.Application.Employees
{
    public class AbsencyCreate
    {
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}