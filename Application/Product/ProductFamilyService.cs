using System.Collections.Generic;
using FriMav.Domain;
using FriMav.Domain.Repositories;

namespace FriMav.Application
{
    public class ProductFamilyService : IProductFamilyService
    {
        private IProductFamilyRepository _productFamilyRepository;
        private IProductRepository _productRepository;

        public ProductFamilyService(
            IProductFamilyRepository productFamilyRepository,
            IProductRepository productRepository)
        {
            _productFamilyRepository = productFamilyRepository;
            _productRepository = productRepository;
        }

        public void Create(ProductFamily customer)
        {
            _productFamilyRepository.Create(customer);
            _productFamilyRepository.Save();
        }

        public void Delete(ProductFamily family)
        {
            var products = _productRepository.FindAllBy(x => x.FamilyId == family.FamilyId);
            foreach (var product in products)
            {
                product.Family = null;
                product.FamilyId = null;
                _productRepository.Update(product);
            }
            _productFamilyRepository.Delete(family);
            _productFamilyRepository.Save();
        }

        public ProductFamily Get(int familyId)
        {
            return _productFamilyRepository.FindBy(x => x.FamilyId == familyId);
        }

        public IEnumerable<ProductFamily> GetAll()
        {
            return _productFamilyRepository.GetAll();
        }

        public void Update(ProductFamily family)
        {
            _productFamilyRepository.Update(family);
            _productFamilyRepository.DetectChanges();
            _productFamilyRepository.Save();
        }
    }
}
