using FriMav.Domain;
using FriMav.Domain.Proyections;
using FriMav.Domain.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FriMav.Infrastructure.Repositories
{
    public class PriceForCustomerRepository : BaseRepository<PriceForCustomer>, IPriceForCustomerRepository
    {
        public PriceForCustomerRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public IEnumerable<ProductPriceForCustomer> GetAllProductPriceForCustomer(int personId)
        {
            return (from product in _databaseContext.Set<Product>()
                    join price in _databaseContext.Set<PriceForCustomer>() on new { p = product.ProductId, c = personId } equals new { p = price.ProductId, c = price.PersonId } into customerPrice
                    from custPrice in customerPrice.DefaultIfEmpty()
                    orderby DbFunctions.Right("0000000000" + product.Code, 10)
                    select new ProductPriceForCustomer
                    {
                        ProductId = product.ProductId,
                        Code = product.Code,
                        FamilyId = product.FamilyId,
                        Name = product.Name,
                        Price = product.Price,
                        PriceForCustomer = (custPrice != null ? custPrice.Price : product.Price)
                    }).ToList();
        }
    }
}
