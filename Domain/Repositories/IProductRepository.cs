using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        IEnumerable<Product> GetAllActive();
        IEnumerable<Product> GetAllActiveInFamily(int familyId);
        Product GetWithFamily(int productId);
        IPagedList<Product> GetPaged(string code, string name, int pageIndex = 0, int pageSize = int.MaxValue);
        bool ExistsActiveCode(string code);
    }
}
