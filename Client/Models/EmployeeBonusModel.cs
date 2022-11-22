using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Client.Models
{
    public class EmployeeBonusModel : EmployeeDocumentModel
    {
        public decimal Salary { get; set; }

        public decimal Amount() => Salary / 2;
    }
}
