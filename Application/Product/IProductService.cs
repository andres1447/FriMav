using FriMav.Domain;
using FriMav.Domain.Proyections;
using FriMav.Domain.Repositories;
using System.Collections.Generic;

namespace FriMav.Application
{
    public interface IProductService
    {
        Product Get(int id);
        IEnumerable<Product> FindAllByIds(IEnumerable<int> ids);
        IEnumerable<Product> GetAllActive();
        IEnumerable<Product> GetAllActiveInFamily(int familyId);
        IEnumerable<Product> GetAll();
        IEnumerable<ProductPriceForCustomer> GetAllProductPriceForCustomer(int customerId);
        IPagedList<Product> GetPaged(string code, string name, int pageIndex, int pageSize);
        void Create(Product product);
        void Update(Product product);
        void Delete(Product product);
        bool ExistsActiveCode(string code);
    }
}
