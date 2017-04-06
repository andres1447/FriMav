using System.Collections.Generic;
using FriMav.Domain;
using FriMav.Domain.Repositories;

namespace FriMav.Application
{
    public class ZoneService : IZoneService
    {
        private IZoneRepository _zoneRepository;
        private IPersonRepository _customerRepository;

        public ZoneService(
            IZoneRepository zoneRepository,
            IPersonRepository customerRepository)
        {
            _zoneRepository = zoneRepository;
            _customerRepository = customerRepository;
        }

        public void Create(Zone zone)
        {
            _zoneRepository.Create(zone);
            _zoneRepository.Save();
        }

        public void Delete(Zone zone)
        {
            var customers = _customerRepository.FindAllBy(x => x.ZoneId == zone.ZoneId);
            foreach (var customer in customers)
            {
                customer.Zone = null;
                customer.ZoneId = null;
                _customerRepository.Update(customer);
            }
            _zoneRepository.Delete(zone);
            _zoneRepository.Save();
        }

        public Zone Get(int zoneId)
        {
            return _zoneRepository.FindBy(x => x.ZoneId == zoneId);
        }

        public IEnumerable<Zone> GetAll()
        {
            return _zoneRepository.GetAll();
        }

        public void Update(Zone zone)
        {
            _zoneRepository.Update(zone);
            _zoneRepository.DetectChanges();
            _zoneRepository.Save();
        }
    }
}
