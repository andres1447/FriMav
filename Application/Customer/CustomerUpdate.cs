using FriMav.Domain;
using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FriMav.Application
{
    public class CustomerUpdate
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }
        public Shipping Shipping { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        public string Name { get; set; }
        public string Cuit { get; set; }
        public string Address { get; set; }
        public int? ZoneId { get; set; }
    }
}