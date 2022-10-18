using FriMav.Domain.Entities.Payrolls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FriMav.Application
{
    public class GoodsSoldResponse
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Total { get; set; }
        public List<GoodsSoldItemResponse> Items { get; set; } = new List<GoodsSoldItemResponse>();
    }

    public class GoodsSoldItemResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public static class GoodsSoldResponseExtensions
    {
        public static GoodsSoldResponse ToGoodsSoldResponse(this GoodsSold goods)
        {
            return new GoodsSoldResponse
            {
                Id = goods.Id,
                EmployeeId = goods.EmployeeId,
                Date = goods.CreationDate,
                Description = goods.Description,
                Total = goods.Items.Select(x => x.Quantity * x.Price).DefaultIfEmpty(0).Sum(),
                Items = goods.Items.Select(x => new GoodsSoldItemResponse
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    Price = x.Price
                }).ToList()
            };
        }
    }
}