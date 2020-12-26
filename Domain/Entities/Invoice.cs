using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Entities
{
    public class Invoice : TransactionDocument
    {
        public Shipping Shipping { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string DeliveryAddress { get; set; }

        public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

        public decimal CalculateTotal()
        {
            return Items.Select(x => x.Price * x.Quantity).DefaultIfEmpty(0).Sum();
        }

        public Payment CreateCancellingPayment(int number) => new Payment
        {
            Date = Date,
            Number = number,
            PersonId = Id,
            Person = Person,
            Total = -Total,
            Balance = Balance - Total
        };
    }
}
