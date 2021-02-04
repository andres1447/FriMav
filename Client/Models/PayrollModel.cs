using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Client.Models
{
    public class PayrollModel
    {
        public DateTime Date { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public decimal Salary { get; set; }
        public List<UnliquidatedDocumentModel> Liquidation { get; set; }
        public decimal Balance { get; set; }
        public decimal Total { get; set; }
    }

    public class UnliquidatedDocumentModel
    {
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Balance { get; set; }
    }
}
