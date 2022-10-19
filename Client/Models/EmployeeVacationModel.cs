using System;

namespace FriMav.Client.Models
{
    public class EmployeeVacationModel : EmployeeDocumentModel
    {
        public int Weeks { get; set; }

        public DateTime Until() => Date.AddDays(7 * Weeks - 1);
    }
}
