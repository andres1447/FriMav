using FriMav.Domain;
using FriMav.Domain.Entities;
using System.Collections.Generic;

namespace FriMav.Application
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAll();

        IEnumerable<Customer> GetAllInZone(int zoneId);

        Customer Get(int id);

        bool Exists(string code);

        [Transactional]
        void Create(CustomerCreate request);

        [Transactional]
        void Update(CustomerUpdate request);

        [Transactional]
        void Delete(int id);
    }
}
