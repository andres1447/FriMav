using FriMav.Domain;
using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FriMav.Application
{
    public class InvoiceCreate
    {
        [Required]
        public int PersonId { get; set; }
        public string CustomerName { get; set; }
        public Shipping? Shipping { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public string DeliveryAddress { get; set; }

        public List<InvoiceItemCreate> Items { get; set; }
    }

    public class InvoiceItemCreate
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}