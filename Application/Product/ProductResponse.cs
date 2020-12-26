using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int? ProductTypeId { get; set; }
        public ProductTypeResponse Type { get; set; }
        public bool Active { get; set; }

        public static Expression<Func<Product, ProductResponse>> Expression;

        public static Func<Product, ProductResponse> Mapper;

        static ProductResponse()
        {
            Expression = product => new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Code = product.Code,
                Active = !product.DeleteDate.HasValue,
                Price = product.Price,
                ProductTypeId = product.ProductTypeId,
                Type = !product.ProductTypeId.HasValue ? null : new ProductTypeResponse
                {
                    Id = product.Type.Id,
                    Name = product.Type.Name
                }
            };
            Mapper = Expression.Compile();
        }
    }

    public class ProductTypeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
