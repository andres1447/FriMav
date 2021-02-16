using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Client.Models
{
    public class CustomerAccountModel
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal Balance { get; set; }
        public List<CustomerTransaction> Transactions = new List<CustomerTransaction>();
    }

    public class CustomerTransaction
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Total { get; set; }
        public decimal Balance { get; set; }
    }
}
