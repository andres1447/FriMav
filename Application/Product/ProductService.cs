using FriMav.Domain;
using FriMav.Domain.Proyections;
using FriMav.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FriMav.Application
{
    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;
        private IPriceForCustomerRepository _priceForCustomerRepository;

        public ProductService(
            IProductRepository productRepository,
            IPriceForCustomerRepository priceForCustomerRepository)
        {
            _productRepository = productRepository;
            _priceForCustomerRepository = priceForCustomerRepository;
        }

        public Product Get(int id)
        {
            return _productRepository.GetWithFamily(id);
        }

        public IEnumerable<Product> FindAllByIds(IEnumerable<int> ids)
        {
            return _productRepository.FindAllBy(x => ids.Contains(x.ProductId));
        }

        public IEnumerable<ProductPriceForCustomer> GetAllProductPriceForCustomer(int customerId)
        {
            return _priceForCustomerRepository.GetAllProductPriceForCustomer(customerId);
        }

        public IEnumerable<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public void Create(Product product)
        {
            _productRepository.Create(product);
            _productRepository.Save();
        }

        public void Update(Product product)
        {
            var saved = Get(product.ProductId);
            saved.Name = product.Name;
            saved.FamilyId = product.FamilyId;
            if (saved.Price != product.Price)
            {
                saved.PriceDate = DateTime.Now;
            }
            saved.Price = product.Price;
            saved.Code = product.Code;
            saved.Active = product.Active;
            _productRepository.Update(saved);
            _productRepository.DetectChanges();
            _productRepository.Save();
        }

        public void Delete(Product product)
        {
            product.Active = false;
            _productRepository.Update(product);
            _productRepository.Save();
        }

        public IPagedList<Product> GetPaged(string code, string name, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            return _productRepository.GetPaged(code, name, pageIndex, pageSize);
        }

        public bool ExistsActiveCode(string code)
        {
            return _productRepository.ExistsActiveCode(code);
        }

        public IEnumerable<Product> GetAllActive()
        {
            return _productRepository.GetAllActive();
        }

        public IEnumerable<Product> GetAllActiveInFamily(int familyId)
        {
            return _productRepository.GetAllActiveInFamily(familyId);
        }
    }
}
