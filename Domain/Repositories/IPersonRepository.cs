using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Repositories
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        IEnumerable<Person> GetAllByTypeInZone(PersonType type, int zoneId);
        IEnumerable<Person> GetAllByType(PersonType type);
        Person GetWithZone(int personId);
        bool Exists(PersonType type, string code);
    }
}
