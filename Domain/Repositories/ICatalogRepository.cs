using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Repositories
{
    public interface ICatalogRepository : IBaseRepository<Catalog>
    {
        Catalog GetByIdWithProducts(int catalogId);
    }
}
