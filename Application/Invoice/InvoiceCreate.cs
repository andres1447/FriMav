using FriMav.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FriMav.Application
{
    public class InvoiceCreate
    {
        public DateTime Date { get; set; }
        public int PersonId { get; set; }
        public Shipping Shipping { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string DeliveryAddress { get; set; }

        public virtual ICollection<InvoiceItemCommand> Items { get; set; }

        public Invoice ToDomain()
        {
            return new Invoice()
            {
                Date = this.Date,
                PersonId = this.PersonId,
                Shipping = this.Shipping,
                DeliveryAddress = this.DeliveryAddress,
                PaymentMethod = this.PaymentMethod,
                TransactionType = TransactionType.Invoice,
                Items = this.Items.Select(it => new InvoiceItem()
                {
                    ProductId = it.ProductId,
                    Quantity = it.Quantity,
                    Price = it.Price
                }).ToList()
            };
        }
    }

    public class InvoiceItemCommand
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}