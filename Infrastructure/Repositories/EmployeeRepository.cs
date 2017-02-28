using FriMav.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using FriMav.Domain.Repositories;

namespace FriMav.Infrastructure.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public override IEnumerable<Employee> GetAll()
        {
            return _databaseContext.Set<Employee>()
                        .OrderBy(x => DbFunctions.Right("0000000000" + x.Code, 10))
                        .ToList();
        }

        public bool Exists(string code)
        {
            return _databaseContext.Set<Employee>()
                .Any(x => x.Code == code);
        }
    }
}
