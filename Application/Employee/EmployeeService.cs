using System.Collections.Generic;
using FriMav.Domain;
using FriMav.Domain.Entities;

namespace FriMav.Application
{
    public class EmployeeService : IEmployeeService
    {
        private IRepository<Employee> _employeeRepository;
        private IRepository<Zone> _zoneRepository;

        public EmployeeService(
            IRepository<Employee> employeeRepository,
            IRepository<Zone> zoneRepository)
        {
            _employeeRepository = employeeRepository;
            _zoneRepository = zoneRepository;
        }

        public void Create(EmployeeCreate request)
        {
            var employee = new Employee
            {
                Code = request.Code,
                Name = request.Name,
                Cuit = request.Cuit,
                Shipping = Shipping.Self,
                Address = request.Address,
                PaymentMethod = PaymentMethod.Credit,
                ZoneId = request.ZoneId,
                Zone = request.ZoneId.HasValue ? _zoneRepository.Get(request.ZoneId.Value) : null,
            };
            _employeeRepository.Add(employee);
        }

        public void Delete(int id)
        {
            var employee = _employeeRepository.Get(id);
            if (employee == null)
                throw new NotFoundException();

            _employeeRepository.Delete(employee);
        }

        public Person Get(int id)
        {
            return _employeeRepository.Get(id);
        }

        public List<Employee> GetAll()
        {
            return _employeeRepository.GetAll();
        }

        public void Update(EmployeeUpdate employee)
        {
            var saved = _employeeRepository.Get(employee.Id);

            saved.Code = employee.Code;
            saved.Name = employee.Name;
            saved.Cuit = employee.Cuit;
            saved.Address = employee.Address;
        }

        public bool Exists(string code)
        {
            return _employeeRepository.Exists(x => x.Code.EndsWith(code));
        }
    }
}
