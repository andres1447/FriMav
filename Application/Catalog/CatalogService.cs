using System;
using System.Collections.Generic;
using System.Linq;
using FriMav.Domain;
using FriMav.Domain.Proyections;
using FriMav.Domain.Repositories;

namespace FriMav.Application
{
    public class CatalogService : ICatalogService
    {
        private ICatalogRepository _catalogRepository;
        private IProductRepository _productRepository;

        public CatalogService(
            ICatalogRepository catalogRepository,
            IProductRepository productRepository)
        {
            _catalogRepository = catalogRepository;
            _productRepository = productRepository;
        }

        public void Create(CatalogCreate command)
        {
            var catalog = new Catalog { Name = command.Name };
            Product product;
            foreach (var productId in command.Products)
            {
                product = new Product { ProductId = productId };
                _productRepository.Attach(product);
                catalog.Products.Add(product);
            }
            _catalogRepository.Create(catalog);
            _catalogRepository.Save();
        }

        public void Delete(Catalog catalog)
        {
            _catalogRepository.Delete(catalog);
            _catalogRepository.Save();
        }

        public IEnumerable<Catalog> GetAll()
        {
            return _catalogRepository.GetAll();
        }

        public Catalog Get(int catalogId)
        {
            return _catalogRepository.GetByIdWithProducts(catalogId);
        }

        public void Update(Catalog catalog)
        {
            var saved = _catalogRepository.GetByIdWithProducts(catalog.CatalogId);
            saved.Name = catalog.Name;
            saved.Products.Clear();
            saved.Products = catalog.Products;

            _catalogRepository.Update(saved);
            _catalogRepository.DetectChanges();
            _catalogRepository.Save();
        }
    }
}
