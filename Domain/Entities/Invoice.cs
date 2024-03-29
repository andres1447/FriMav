﻿using System;
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
            PersonId = PersonId,
            Person = Person,
            Total = -Total,
            Balance = Balance - Total
        };

        public override TransactionDocument CreateVoidDocument(IDocumentNumberGenerator numberGenerator)
        {
            return new CreditNote { Number = numberGenerator.NextForCreditNote() };
        }

        public void ApplySurcharge(decimal surcharge)
        {
            if (surcharge == 0) return;

            var coeficient = 1 + surcharge;
            foreach (var item in Items)
            {
                item.Price *= coeficient;
            }
        }
    }
}
