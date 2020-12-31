using FriMav.Domain;
using FriMav.Domain.Entities;
using System.Collections.Generic;

namespace FriMav.Application
{
    public interface IProductService
    {
        ProductResponse Get(int id);
        IEnumerable<ProductResponse> GetAllActive();
        IEnumerable<ProductResponse> GetAllActiveInFamily(int typeId);
        IEnumerable<ProductResponse> GetAll();
        IPagedList<ProductResponse> GetPaged(string code, string name, int pageIndex, int pageSize);
        IEnumerable<PriceListItem> GetPriceList(int id);
        List<string> UsedCodes();

        [Transactional]
        void Create(ProductCreate product);

        [Transactional]
        void Update(ProductUpdate product);

        [Transactional]
        void Delete(int id);
    }
}
