using FriMav.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriMav.Application
{
    public class ProductUpdate
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int? FamilyId { get; set; }
        public bool Active { get; set; }

        public Product ToDomain()
        {
            return new Product()
            {
                ProductId = this.ProductId,
                Code = this.Code,
                Name = this.Name,
                Price = this.Price,
                FamilyId = this.FamilyId,
                Active = this.Active
            };
        }
    }
}