using FriMav.Domain;
using FriMav.Domain.Repositories;
using System.Data.Entity;
using System.Linq;

namespace FriMav.Infrastructure.Repositories
{
    public class ProductFamilyRepository : BaseRepository<ProductFamily>, IProductFamilyRepository
    {
        public ProductFamilyRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}
