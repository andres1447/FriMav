using FriMav.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using FriMav.Domain.Repositories;

namespace FriMav.Infrastructure.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public override IEnumerable<Customer> GetAll()
        {
            return _databaseContext.Set<Customer>()
                        .Include(x => x.Zone)
                        .OrderBy(x => DbFunctions.Right("0000000000" + x.Code, 10))
                        .ToList();
        }

        public IEnumerable<Customer> GetAllInZone(int zoneId)
        {
            return _databaseContext.Set<Customer>()
                        .Where(x => x.ZoneId == zoneId)
                        .OrderBy(x => DbFunctions.Right("0000000000" + x.Code, 10))
                        .ToList();
        }

        public Customer GetWithZone(int personId)
        {
            return _databaseContext.Set<Customer>()
                        .Include(x => x.Zone)
                        .FirstOrDefault(x => x.PersonId == personId);
        }

        public bool Exists(string code)
        {
            return _databaseContext.Set<Customer>()
                .Any(x => x.Code == code);
        }
    }
}
