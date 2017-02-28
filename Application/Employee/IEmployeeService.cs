using FriMav.Domain;
using System.Collections.Generic;

namespace FriMav.Application
{
    public interface IEmployeeService
    {
        IEnumerable<Employee> GetAll();
        Employee Get(int personId);
        void Create(Employee customer);
        void Update(Employee customer);
        void Delete(int personId);
        void Delete(Employee customer);
        bool Exists(string code);
    }
}
