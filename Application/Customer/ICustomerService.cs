using FriMav.Domain;
using System.Collections.Generic;

namespace FriMav.Application
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAll();
        IEnumerable<Customer> GetAllInZone(int zoneId);
        Customer Get(int personId);
        void Create(Customer customer);
        void Update(Customer customer);
        void Delete(int personId);
        void Delete(Customer customer);
        bool Exists(string code);
    }
}
