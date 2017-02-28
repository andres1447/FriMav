using System;
using System.Collections.Generic;
using FriMav.Domain;
using FriMav.Domain.Repositories;

namespace FriMav.Application
{
    public class CustomerService : ICustomerService
    {
        private ICustomerRepository _customerRepository;

        public CustomerService(
            ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public void Create(Customer customer)
        {
            _customerRepository.Create(customer);
            _customerRepository.Save();
        }

        public void Delete(int personId)
        {
            Customer customer = new Customer { PersonId = personId };
            _customerRepository.Attach(customer);
            Delete(customer);
        }

        public void Delete(Customer customer)
        {
            _customerRepository.Delete(customer);
            _customerRepository.Save();
        }

        public Customer Get(int personId)
        {
            return _customerRepository.GetWithZone(personId);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _customerRepository.GetAll();
        }

        public void Update(Customer customer)
        {
            var saved = Get(customer.PersonId);

            saved.Code = customer.Code;
            saved.Name = customer.Name;
            saved.Cuit = customer.Cuit;
            saved.Address = customer.Address;
            saved.PaymentMethod = customer.PaymentMethod;
            saved.Shipping = customer.Shipping;
            saved.ZoneId = customer.ZoneId;

            _customerRepository.Update(saved);
            _customerRepository.DetectChanges();
            _customerRepository.Save();
        }

        public bool Exists(string code)
        {
            return _customerRepository.Exists(code);
        }

        public IEnumerable<Customer> GetAllInZone(int zoneId)
        {
            return _customerRepository.GetAllInZone(zoneId);
        }
    }
}
