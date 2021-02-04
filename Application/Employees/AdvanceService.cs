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
    public class AdvanceService : IAdvanceService
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Advance> _advanceRepository;
        private readonly IRepository<Payroll> _payrollRepository;

        public AdvanceService(
            IRepository<Employee> employeeRepository,
            IRepository<Advance> advanceRepository,
            IRepository<Payroll> payrollRepository)
        {
            _employeeRepository = employeeRepository;
            _advanceRepository = advanceRepository;
            _payrollRepository = payrollRepository;
        }

        public void Create(AdvanceCreate request)
        {
            var employee = _employeeRepository.GetById(request.EmployeeId);
            _advanceRepository.Add(new Advance
            {
                Date = request.Date,
                Description = request.Description,
                Amount = -request.Amount,
                Employee = employee,
                EmployeeId = employee.Id
            });
        }

        public void Delete(int id)
        {
            var advance = _advanceRepository.GetById(id);
            if (_payrollRepository.IsAlreadyLiquidated(advance))
                throw new ValidationException("No se puede eliminar un adelanto que ya fue liquidado");
            advance.Delete();
        }
    }
}
