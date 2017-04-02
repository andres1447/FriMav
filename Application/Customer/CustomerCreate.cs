using FriMav.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriMav.Application
{
    public class CustomerCreate
    {
        public string Code { get; set; }
        public Shipping Shipping { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Name { get; set; }
        public string Cuit { get; set; }
        public string Address { get; set; }
        public Nullable<int> ZoneId { get; set; }

        public Customer ToDomain()
        {
            return new Customer()
            {
                Code = this.Code,
                Shipping = this.Shipping,
                PaymentMethod = this.PaymentMethod,
                Name = this.Name,
                Cuit = this.Cuit,
                Address = this.Address,
                ZoneId = this.ZoneId
            };
        }
    }
}