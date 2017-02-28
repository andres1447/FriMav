using FriMav.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriMav.Application
{
    public class ProductCreate
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }
        public int? FamilyId { get; set; }

        public Product ToDomain()
        {
            return new Product()
            {
                Code = this.Code,
                Name = this.Name,
                Price = this.Price,
                FamilyId = this.FamilyId,
                PriceDate = DateTime.Now,
                Active = this.Active
            };
        }
    }
}