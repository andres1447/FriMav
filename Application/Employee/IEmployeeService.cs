using FriMav.Domain;
using FriMav.Domain.Entities;
using System.Collections.Generic;

namespace FriMav.Application
{
    public interface IEmployeeService
    {

        [Transactional]
        void Create(EmployeeCreate request);

        [Transactional]
        void Update(EmployeeUpdate request);

        [Transactional]
        void Delete(int id);

        List<Employee> GetAll();
        Person Get(int personId);
        List<string> UsedCodes();
    }
}
