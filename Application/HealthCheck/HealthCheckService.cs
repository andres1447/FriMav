using FriMav.Domain;
using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public class HealthCheckService : IHealthCheckService
    {
        private readonly IRepository<Product> _productRepository;

        public HealthCheckService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public void KeepAlive()
        {
            _productRepository.GetAll();
        }
    }
}
