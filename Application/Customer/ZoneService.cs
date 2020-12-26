using System.Collections.Generic;
using System.Linq;
using FriMav.Domain;
using FriMav.Domain.Entities;

namespace FriMav.Application
{
    public class ZoneService : IZoneService
    {
        private IRepository<Zone> _zoneRepository;
        private IRepository<Person> _personRepository;

        public ZoneService(
            IRepository<Zone> zoneRepository,
            IRepository<Person> personRepository)
        {
            _zoneRepository = zoneRepository;
            _personRepository = personRepository;
        }

        public void Create(Zone zone)
        {
            _zoneRepository.Add(zone);
        }

        public void Delete(int id)
        {
            var zone = _zoneRepository.Get(id);
            var people = _personRepository.Query().Where(x => x.ZoneId == id);
            foreach (var person in people)
            {
                person.WithoutZone();
            }
            _zoneRepository.Delete(zone);
        }

        public Zone Get(int id)
        {
            return _zoneRepository.Get(id);
        }

        public IEnumerable<Zone> GetAll()
        {
            return _zoneRepository.GetAll();
        }

        public void Update(Zone zone)
        {
            var saved = _zoneRepository.Get(zone.Id);
            saved.Name = zone.Name;
        }
    }
}
