using FriMav.Domain.Proyections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Repositories
{
    public interface IPriceForCustomerRepository : IBaseRepository<PriceForCustomer>
    {
        IEnumerable<ProductPriceForCustomer> GetAllProductPriceForCustomer(int customerId);
    }
}
