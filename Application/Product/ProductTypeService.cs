using System.Collections.Generic;
using FriMav.Domain;
using FriMav.Domain.Entities;

namespace FriMav.Application
{
    public class ProductTypeService : IProductTypeService
    {
        private IRepository<ProductType> _productTypeRepository;

        public ProductTypeService(
            IRepository<ProductType> productTypeRepository)
        {
            _productTypeRepository = productTypeRepository;
        }

        public void Create(ProductType family)
        {
            _productTypeRepository.Add(family);
        }

        public void Delete(int id)
        {
            var type = _productTypeRepository.Get(id);
            if (type == null)
                throw new NotFoundException();

            _productTypeRepository.Delete(type);
        }

        public ProductType Get(int id)
        {
            return _productTypeRepository.Get(id);
        }

        public IEnumerable<ProductType> GetAll()
        {
            return _productTypeRepository.GetAll();
        }

        public void Update(ProductType type)
        {
            var product = _productTypeRepository.Get(type.Id);
            product.Name = type.Name;
        }
    }
}
