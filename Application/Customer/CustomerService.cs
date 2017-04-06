using System;
using System.Collections.Generic;
using FriMav.Domain;
using FriMav.Domain.Repositories;

namespace FriMav.Application
{
    public class CustomerService : ICustomerService
    {
        private IPersonRepository _personRepository;
        private IZoneRepository _zoneRepository;

        public CustomerService(
            IPersonRepository personRepository,
            IZoneRepository zoneRepository)
        {
            _personRepository = personRepository;
            _zoneRepository = zoneRepository;
        }

        public void Create(Person customer)
        {
            _personRepository.Create(customer);
            _personRepository.Save();
        }

        public void Delete(int personId)
        {
            Person customer = new Person { PersonId = personId };
            _personRepository.Attach(customer);
            Delete(customer);
        }

        public void Delete(Person customer)
        {
            _personRepository.Delete(customer);
            _personRepository.Save();
        }

        public Person Get(int personId)
        {
            return _personRepository.GetWithZone(personId);
        }

        public IEnumerable<Person> GetAll()
        {
            return _personRepository.GetAllByType(PersonType.Customer);
        }

        public void Update(Person customer)
        {
            var saved = Get(customer.PersonId);

            saved.Code = customer.Code;
            saved.Name = customer.Name;
            saved.Cuit = customer.Cuit;
            saved.Address = customer.Address;
            saved.PaymentMethod = customer.PaymentMethod;
            saved.Shipping = customer.Shipping;
            saved.ZoneId = customer.ZoneId;
            if (customer.ZoneId.HasValue)
            {
                saved.Zone = new Zone { ZoneId = customer.ZoneId.Value };
                _zoneRepository.Attach(saved.Zone);
            }

            _personRepository.Update(saved);
            _personRepository.DetectChanges();
            _personRepository.Save();
        }

        public bool Exists(string code)
        {
            return _personRepository.Exists(PersonType.Customer, code);
        }

        public IEnumerable<Person> GetAllInZone(int zoneId)
        {
            return _personRepository.GetAllByTypeInZone(PersonType.Customer, zoneId);
        }
    }
}
