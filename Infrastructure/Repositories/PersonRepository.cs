using FriMav.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using FriMav.Domain.Repositories;
using System;

namespace FriMav.Infrastructure.Repositories
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public IEnumerable<Person> GetAllByType(PersonType type)
        {
            return _databaseContext.Set<Person>()
                    .Where(p => p.PersonType == type)
                    .OrderBy(p => DbFunctions.Right("0000000000" + p.Code, 10))
                    .ToList();
        }

        public IEnumerable<Person> GetAllByTypeInZone(PersonType type, int zoneId)
        {
            return _databaseContext.Set<Person>()
                    .Where(p => p.PersonType == type && p.ZoneId == zoneId)
                    .OrderBy(p => DbFunctions.Right("0000000000" + p.Code, 10))
                    .ToList();
        }

        public Person GetWithZone(int personId)
        {
            return _databaseContext.Set<Person>()
                        .Include(x => x.Zone)
                        .FirstOrDefault(x => x.PersonId == personId);
        }

        public bool Exists(PersonType type, string code)
        {
            return _databaseContext.Set<Person>().Any(p => p.PersonType == type && p.Code == code);
        }
    }
}
