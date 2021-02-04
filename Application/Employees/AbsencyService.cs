using FriMav.Domain;
using FriMav.Domain.Entities;
using FriMav.Domain.Entities.Payrolls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application.Employees
{
    public class AbsencyService : IAbsencyService
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Absency> _absencyRepository;
        private readonly IRepository<Payroll> _payrollRepository;

        public AbsencyService(
            IRepository<Employee> employeeRepository,
            IRepository<Absency> absencyRepository,
            IRepository<Payroll> payrollRepository)
        {
            _employeeRepository = employeeRepository;
            _absencyRepository = absencyRepository;
            _payrollRepository = payrollRepository;
        }

        public void Create(AbsencyCreate request)
        {
            var employee = _employeeRepository.GetById(request.EmployeeId);
            _absencyRepository.Add(new Absency
            {
                Date = request.Date,
                Description = request.Description,
                Amount = -employee.DailySalary(),
                Employee = employee,
                EmployeeId = employee.Id
            });
        }

        public void Delete(int id)
        {
            var absency = _absencyRepository.GetById(id);
            if (_payrollRepository.IsAlreadyLiquidated(absency))
                throw new ValidationException("No se puede eliminar una ausencia que ya fue liquidada");
            absency.Delete();
        }
    }
}
