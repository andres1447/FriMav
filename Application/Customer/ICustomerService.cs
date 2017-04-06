using FriMav.Domain;
using System.Collections.Generic;

namespace FriMav.Application
{
    public interface ICustomerService
    {
        IEnumerable<Person> GetAll();
        IEnumerable<Person> GetAllInZone(int zoneId);
        Person Get(int personId);
        void Create(Person customer);
        void Update(Person customer);
        void Delete(int personId);
        void Delete(Person customer);
        bool Exists(string code);
    }
}
