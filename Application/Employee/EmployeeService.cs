using System.Collections.Generic;
using FriMav.Domain;
using FriMav.Domain.Repositories;

namespace FriMav.Application
{
    public class EmployeeService : IEmployeeService
    {
        private IPersonRepository _personRepository;

        public EmployeeService(
            IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public void Create(Person employee)
        {
            _personRepository.Create(employee);
            _personRepository.Save();
        }

        public void Delete(int personId)
        {
            Person employee = new Person { PersonId = personId };
            _personRepository.Attach(employee);
            Delete(employee);
        }

        public void Delete(Person employee)
        {
            _personRepository.Delete(employee);
            _personRepository.Save();
        }

        public Person Get(int personId)
        {
            return _personRepository.FindBy(x => x.PersonId == personId);
        }

        public IEnumerable<Person> GetAll()
        {
            return _personRepository.GetAllByType(PersonType.Employee);
        }

        public void Update(Person employee)
        {
            var saved = _personRepository.FindBy(c => c.PersonId == employee.PersonId);

            saved.Code = employee.Code;
            saved.Name = employee.Name;
            saved.Cuit = employee.Cuit;
            saved.Address = employee.Address;

            _personRepository.Update(saved);
            _personRepository.DetectChanges();
            _personRepository.Save();
        }

        public bool Exists(string code)
        {
            return _personRepository.Exists(PersonType.Employee, code);
        }
    }
}
