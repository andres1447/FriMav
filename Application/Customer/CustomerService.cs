using System;
using System.Collections.Generic;
using FriMav.Domain;
using FriMav.Domain.Entities;

namespace FriMav.Application
{
    public class CustomerService : ICustomerService
    {
        private IRepository<Customer> _customerRepository;
        private IRepository<Zone> _zoneRepository;

        public CustomerService(
            IRepository<Customer> customerRepository,
            IRepository<Zone> zoneRepository)
        {
            _customerRepository = customerRepository;
            _zoneRepository = zoneRepository;
        }

        public void Create(CustomerCreate request)
        {
            var customer = new Customer
            {
                Code = request.Code,
                Name = request.Name,
                Cuit = request.Cuit,
                Shipping = request.Shipping,
                Address = request.Address,
                PaymentMethod = request.PaymentMethod,
                ZoneId = request.ZoneId
            };
            _customerRepository.Add(customer);
        }

        public void Delete(int id)
        {
            var customer = _customerRepository.Get(id);
            if (customer == null)
                throw new NotFoundException();

            customer.Delete();
        }

        public bool Exists(string code)
        {
            throw new NotImplementedException();
        }

        public Customer Get(int personId)
        {
            return _customerRepository.Get(personId, x => x.Zone);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _customerRepository.GetAll();
        }

        public IEnumerable<Customer> GetAllInZone(int zoneId)
        {
            throw new NotImplementedException();
        }

        public void Update(CustomerUpdate customer)
        {
            var saved = _customerRepository.Get(customer.Id, x => x.Zone);
            var zone = customer.ZoneId.HasValue ? _zoneRepository.Get(customer.ZoneId.Value) : null;
            saved.Code = customer.Code;
            saved.Name = customer.Name;
            saved.Cuit = customer.Cuit;
            saved.Address = customer.Address;
            saved.PaymentMethod = customer.PaymentMethod;
            saved.Shipping = customer.Shipping;
            saved.ZoneId = customer.ZoneId;
            saved.Zone = zone;
        }
    }
}
