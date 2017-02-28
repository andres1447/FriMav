using System.Collections.Generic;
using FriMav.Domain;
using FriMav.Domain.Repositories;

namespace FriMav.Application
{
    public class EmployeeService : IEmployeeService
    {
        private IEmployeeRepository _employeeRepository;

        public EmployeeService(
            IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public void Create(Employee employee)
        {
            _employeeRepository.Create(employee);
            _employeeRepository.Save();
        }

        public void Delete(int personId)
        {
            Employee employee = new Employee { PersonId = personId };
            _employeeRepository.Attach(employee);
            Delete(employee);
        }

        public void Delete(Employee employee)
        {
            _employeeRepository.Delete(employee);
            _employeeRepository.Save();
        }

        public Employee Get(int personId)
        {
            return _employeeRepository.FindBy(x => x.PersonId == personId);
        }

        public IEnumerable<Employee> GetAll()
        {
            return _employeeRepository.GetAll();
        }

        public void Update(Employee employee)
        {
            var saved = _employeeRepository.FindBy(c => c.PersonId == employee.PersonId);

            saved.Code = employee.Code;
            saved.Name = employee.Name;
            saved.Cuit = employee.Cuit;
            saved.Address = employee.Address;

            _employeeRepository.Update(saved);
            _employeeRepository.DetectChanges();
            _employeeRepository.Save();
        }

        public bool Exists(string code)
        {
            return _employeeRepository.Exists(code);
        }
    }
}
