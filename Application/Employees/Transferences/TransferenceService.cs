using FriMav.Domain;
using FriMav.Domain.Entities;
using FriMav.Domain.Entities.Payrolls;

namespace FriMav.Application.Employees
{
    public class TransferenceService : ITransferenceService
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Transference> _transferenceRepository;
        private readonly IRepository<Payroll> _payrollRepository;

        public TransferenceService(
            IRepository<Employee> employeeRepository,
            IRepository<Transference> transferenceRepository,
            IRepository<Payroll> payrollRepository)
        {
            _employeeRepository = employeeRepository;
            _transferenceRepository = transferenceRepository;
            _payrollRepository = payrollRepository;
        }

        public void Create(TransferenceCreate request)
        {
            var employee = _employeeRepository.GetById(request.EmployeeId);
            _transferenceRepository.Add(new Transference
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
            var transference = _transferenceRepository.GetById(id);
            if (_payrollRepository.IsAlreadyLiquidated(transference))
                throw new ValidationException("No se puede eliminar una transferencia que ya fue liquidada");
            transference.Delete();
        }
    }
}
