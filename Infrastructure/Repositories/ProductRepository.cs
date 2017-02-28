using FriMav.Domain;
using FriMav.Domain.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FriMav.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public Product GetWithFamily(int productId)
        {
            return _databaseContext.Set<Product>()
                        .Include("Family")
                        .FirstOrDefault(x => x.ProductId == productId);
        }

        public override IEnumerable<Product> GetAll()
        {
            return _databaseContext.Set<Product>()
                        .Include(x => x.Family)
                        .OrderBy(x => DbFunctions.Right("0000000000" + x.Code, 10))
                        .ToList();
        }

        public IEnumerable<Product> GetAllActive()
        {
            return _databaseContext.Set<Product>()
                        .Where(x => x.Active)
                        .OrderBy(x => DbFunctions.Right("0000000000" + x.Code, 10))
                        .ToList();
        }

        public IEnumerable<Product> GetAllActiveInFamily(int familyId)
        {
            return _databaseContext.Set<Product>()
                        .Where(x => x.FamilyId == familyId && x.Active)
                        .OrderBy(x => DbFunctions.Right("0000000000" + x.Code, 10))
                        .ToList();
        }

        public IPagedList<Product> GetPaged(string code, string name, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var products = _databaseContext.Set<Product>().AsQueryable();
            if (!string.IsNullOrEmpty(code))
            {
                products = products.Where(it => it.Code == code);
            }
            if (!string.IsNullOrEmpty(name))
            {
                products = products.Where(it => it.Name == name);
            }
            products.Include(x => x.Family);
            products = products.OrderByDescending(x => DbFunctions.Right("0000000000" + x.Code, 10));
            return new PagedList<Product>(products, pageIndex, pageSize);
        }

        public bool ExistsActiveCode(string code)
        {
            return _databaseContext.Set<Product>().Any(x => x.Code == code && x.Active);
        }
    }
}
