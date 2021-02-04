using FriMav.Domain;
using FriMav.Domain.Entities;
using FriMav.Domain.Entities.Payrolls;
using System.Linq;

namespace FriMav.Application
{
    public class LoanService : ILoanService
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Loan> _loanRepository;
        private readonly IRepository<Payroll> _payrollRepository;

        public LoanService(
            IRepository<Loan> loanRepository,
            IRepository<Employee> employeeRepository, 
            IRepository<Payroll> payrollRepository)
        {
            _loanRepository = loanRepository;
            _employeeRepository = employeeRepository;
            _payrollRepository = payrollRepository;
        }

        public void Create(LoanCreate request)
        {
            var employee = _employeeRepository.GetById(request.EmployeeId);
            var loan = MapLoan(request, employee);

            _loanRepository.Add(loan);
        }

        public void Delete(int id)
        {
            var loan = _loanRepository.GetById(id, x => x.Fees);
            if (HasAnyLiquidatedFee(loan))
                throw new ValidationException("No puede eliminarse un préstamo con alguna cuota liquidada.");

            loan.Delete();
        }

        private bool HasAnyLiquidatedFee(Loan loan)
        {
            var feeIds = loan.Fees.Select(x => x.Id).ToList();
            return _payrollRepository.Query()
                            .Where(x => !x.DeleteDate.HasValue && x.EmployeeId == loan.EmployeeId)
                            .SelectMany(x => x.Liquidation)
                            .OfType<LoanFee>()
                            .Any(x => feeIds.Contains(x.Id));
        }

        private Loan MapLoan(LoanCreate request, Employee employee)
        {
            return new Loan
            {
                Employee = employee,
                EmployeeId = employee.Id,
                Fees = request.Fees.Select(x => MapFee(request, employee, x)).ToList()
            };
        }

        private static LoanFee MapFee(LoanCreate request, Employee employee, LoanCreateFee x)
        {
            return new LoanFee
            {
                Date = x.Date,
                Description = request.Description,
                Amount = -x.Amount,
                Employee = employee,
                EmployeeId = employee.Id
            };
        }

        public LoanResponse Get(int id)
        {
            var liquidatedDocuments = _payrollRepository.Query().Where(x => !x.DeleteDate.HasValue)
                .SelectMany(x => x.Liquidation);

            var loan = _loanRepository.Query().Where(x => x.Id == id).Select(x => new LoanResponse
            {
                Id = x.Id,
                CreationDate = x.CreationDate,
                EmployeeId = x.EmployeeId,
                EmployeeCode = x.Employee.Code,
                EmployeeName = x.Employee.Name,
                Fees = x.Fees.Select(f => new LoanFeeResponse
                {
                    Id = f.Id,
                    Date = f.Date,
                    Amount = f.Amount,
                    IsLiquidated = liquidatedDocuments.Any(l => l.Id == f.Id)
                }).ToList()
            }).FirstOrDefault();

            if (loan == null)
                throw new NotFoundException();

            return loan;
        }
    }
}
