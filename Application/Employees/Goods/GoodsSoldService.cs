using FriMav.Domain;
using FriMav.Domain.Entities;
using FriMav.Domain.Entities.Payrolls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public class GoodsSoldService : IGoodsSoldService
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<GoodsSold> _goodsSoldRepository;
        private readonly IRepository<Product> _productRepository;

        public GoodsSoldService(
            IRepository<GoodsSold> goodsSoldRepository,
            IRepository<Employee> employeeRepository,
            IRepository<Product> productRepository)
        {
            _goodsSoldRepository = goodsSoldRepository;
            _employeeRepository = employeeRepository;
            _productRepository = productRepository;
        }

        public void Create(GoodsSoldCreate request)
        {
            var employee = _employeeRepository.GetById(request.EmployeeId);
            var goodsSold = MapGoodsSold(request, employee);

            _goodsSoldRepository.Add(goodsSold);
        }

        public void Delete(int id)
        {
            var goodsSold = _goodsSoldRepository.GetById(id);
            goodsSold.Delete();
        }

        private GoodsSold MapGoodsSold(GoodsSoldCreate request, Employee employee)
        {
            var productIds = request.Items.Select(x => x.ProductId).ToList();
            var products = _productRepository.Query().Where(x => productIds.Contains(x.Id));

            return new GoodsSold
            {
                Employee = employee,
                EmployeeId = employee.Id,
                Amount = -CalculateTotal(request),
                Items = MapItems(request, products)
            };
        }

        private static List<GoodsSoldItem> MapItems(GoodsSoldCreate request, IQueryable<Product> products)
        {
            return request.Items.Select(x => new GoodsSoldItem
            {
                ProductId = x.ProductId,
                ProductName = products.First(p => p.Id == x.ProductId).Name,
                Quantity = x.Quantity,
                Price = x.Price
            }).ToList();
        }

        private static decimal CalculateTotal(GoodsSoldCreate request)
        {
            return request.Items.Select(x => x.Quantity * x.Price).DefaultIfEmpty(0).Sum();
        }

        public GoodsSoldResponse Get(int id)
        {
            var goods = _goodsSoldRepository.Get(id, x => x.Items);
            return goods.ToGoodsSoldResponse();
        }
    }
}
