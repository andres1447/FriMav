using System.Collections.Generic;
using System.Linq;
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
            if (_employeeRepository.Query().Any(x => x.Code == request.Code))
                throw new AlreadyExistsException();

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
            if (_employeeRepository.Query().Any(x => x.Id != employee.Id && x.Code == employee.Code))
                throw new AlreadyExistsException();

            var saved = _employeeRepository.Get(employee.Id);

            saved.Code = employee.Code;
            saved.Name = employee.Name;
            saved.Cuit = employee.Cuit;
            saved.Address = employee.Address;
        }

        public List<string> UsedCodes()
        {
            return _employeeRepository.Query().Select(x => x.Code).ToList();
        }
    }
}
