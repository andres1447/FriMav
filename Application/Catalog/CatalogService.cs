using System;
using System.Collections.Generic;
using System.Linq;
using FriMav.Domain;
using FriMav.Domain.Entities;
using FriMav.Domain.Proyections;

namespace FriMav.Application
{
    public class CatalogService : ICatalogService
    {
        private IRepository<Catalog> _catalogRepository;
        private IRepository<Product> _productRepository;

        public CatalogService(
            IRepository<Catalog> catalogRepository,
            IRepository<Product> productRepository)
        {
            _catalogRepository = catalogRepository;
            _productRepository = productRepository;
        }

        public void Create(CatalogCreate command)
        {
            var catalog = new Catalog { Name = command.Name };
            var products = _productRepository.Query().Where(x => command.Products.Contains(x.Id)).ToList();
            foreach (var product in products)
            {
                catalog.Products.Add(product);
            }
            _catalogRepository.Add(catalog);
        }

        public void Delete(int id)
        {
            var catalog = _catalogRepository.Get(id, x => x.Products);
            if (catalog == null)
                throw new NotFoundException();

            _catalogRepository.Delete(catalog);
        }

        public IEnumerable<Catalog> GetAll()
        {
            return _catalogRepository.GetAll();
        }

        public Catalog Get(int id)
        {
            return _catalogRepository.Get(id, x => x.Products);
        }

        public void Update(Catalog catalog)
        {
            var saved = _catalogRepository.Get(catalog.Id, x => x.Products);
            if (saved == null)
                throw new NotFoundException();
            saved.Name = catalog.Name;
            saved.Products.Clear();
            saved.Products = catalog.Products;
        }
    }
}
