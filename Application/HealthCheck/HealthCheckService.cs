using FriMav.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public class HealthCheckService : IHealthCheckService
    {
        IProductRepository _productRepository;

        public HealthCheckService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void KeepAlive()
        {
            _productRepository.Count();
        }
    }
}
