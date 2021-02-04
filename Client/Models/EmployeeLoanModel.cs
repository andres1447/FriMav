using System.Collections.Generic;
using System.Linq;

namespace FriMav.Client.Models
{
    public class EmployeeLoanModel : EmployeeDocumentModel
    {
        public List<LoanFeeModel> Fees { get; set; }

        public decimal Total()
        {
            return Fees.Select(x => x.Amount).DefaultIfEmpty(0).Sum();
        }
    }
}
