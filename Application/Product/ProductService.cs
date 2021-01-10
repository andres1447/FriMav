using FriMav.Domain;
using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FriMav.Application
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductType> _productTypeRepository;
        private readonly IRepository<CustomerPrice> _customerPriceRepository;

        public ProductService(
            IRepository<Product> productRepository,
            IRepository<ProductType> productTypeRepository,
            IRepository<CustomerPrice> customerPriceRepository)
        {
            _productRepository = productRepository;
            _productTypeRepository = productTypeRepository;
            _customerPriceRepository = customerPriceRepository;
        }

        public ProductResponse Get(int id)
        {
            var product = GetById(id);
            return ProductResponse.Mapper(product);
        }

        private Product GetById(int id)
        {
            var product = _productRepository.Get(id, x => x.Type);
            if (product == null)
                throw new NotFoundException();
            return product;
        }

        public IEnumerable<ProductResponse> GetAll()
        {
            return _productRepository.Query().Select(ProductResponse.Expression).ToList();
        }


        public void Create(ProductCreate request)
        {
            if (_productRepository.Query().Any(x => x.Code == request.Code))
                throw new AlreadyExistsException();

            var type = request.ProductTypeId.HasValue ? _productTypeRepository.Get(request.ProductTypeId.Value) : null;
            var product = new Product
            {
                Code = request.Code,
                Name = request.Name,
                Price = request.Price,
                ProductTypeId = request.ProductTypeId,
                Type = type
            };
            _productRepository.Add(product);
        }

        public void Update(ProductUpdate product)
        {
            if (_productRepository.Query().Any(x => x.Id != product.Id && x.Code == product.Code))
                throw new AlreadyExistsException();

            var saved = GetById(product.Id);
            saved.Name = product.Name;
            saved.ProductTypeId = product.ProductTypeId;
            if (saved.Price != product.Price)
            {
                saved.PriceDate = DateTime.UtcNow;
            }
            saved.Price = product.Price;
            saved.Code = product.Code;
            if (!product.Active)
                saved.Delete();
        }

        public void Delete(int id)
        {
            var product = GetById(id);
            product.Delete();
        }

        public IPagedList<ProductResponse> GetPaged(string code, string name, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            return null;
        }

        public IEnumerable<ProductResponse> GetAllActive()
        {
            return _productRepository.Query()
                .Where(x => !x.DeleteDate.HasValue)
                .Select(ProductResponse.Expression).ToList();
        }

        public IEnumerable<ProductResponse> GetAllActiveInFamily(int typeId)
        {
            return _productRepository.Query()
                .Where(x => x.DeleteDate.HasValue && x.ProductTypeId == typeId)
                .Select(ProductResponse.Expression).ToList();
        }

        public IEnumerable<PriceListItem> GetPriceList(int id)
        {
            return _productRepository.Query().Where(x => !x.DeleteDate.HasValue)
                .GroupJoin(_customerPriceRepository.Query().Where(x => x.CustomerId == id),
                x => x.Id, x => x.ProductId,
                (x, y) => new PriceListItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Price = y.Select(c => c.Price).DefaultIfEmpty(x.Price).FirstOrDefault(),
                    BasePrice = x.Price,
                }).ToList();
        }

        public List<string> UsedCodes()
        {
            return _productRepository.Query().Select(x => x.Code).ToList();
        }
    }
}
