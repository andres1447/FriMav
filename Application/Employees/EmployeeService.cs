using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FriMav.Domain;
using FriMav.Domain.Entities;
using FriMav.Domain.Entities.Payrolls;

namespace FriMav.Application
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ITime _time; 
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Zone> _zoneRepository;
        private readonly IRepository<Payroll> _payrollRepository;
        private readonly IRepository<LiquidationDocument> _liquidationDocumentRepository;

        public EmployeeService(
            ITime time,
            IRepository<Employee> employeeRepository,
            IRepository<Zone> zoneRepository,
            IRepository<Payroll> payrollRepository,
            IRepository<LiquidationDocument> liquidationDocumentRepository)
        {
            _time = time;
            _employeeRepository = employeeRepository;
            _zoneRepository = zoneRepository;
            _payrollRepository = payrollRepository;
            _liquidationDocumentRepository = liquidationDocumentRepository;
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
                Salary = request.Salary,
                JoinDate = request.JoinDate ?? DateTime.UtcNow
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

        public Employee Get(int id)
        {
            return _employeeRepository.Get(id);
        }

        public List<EmployeeResponse> GetAll()
        {
            var lastDayOfWeek = GetLastDayOfWeek();
            var payrolls = GetThisWeekPayrolls().Select(x => x.EmployeeId);
            var employees = GetActiveEmployees();
            var unliquidatedMovements = GetUnliquidatedDocuments(lastDayOfWeek);

            return employees.GroupJoin(unliquidatedMovements, x => x.Id, x => x.EmployeeId, (employee, unliquidated) =>
            new EmployeeResponse
            {
                Id = employee.Id,
                Name = employee.Name,
                Code = employee.Code,
                Salary = employee.Salary,
                IsLiquidated = payrolls.Contains(employee.Id),
                Absences = unliquidated.OfType<Absency>().Count(),
                LiquidationBalance = unliquidated.Select(x => x.Amount).DefaultIfEmpty(0).Sum() + employee.Balance + employee.Salary,
                Balance = employee.Balance
            }).ToList();
        }

        private DateTime GetLastDayOfWeek()
        {
            return _time.UtcNow().LastDayOfWeek().EndOfDay();
        }

        private IQueryable<LiquidationDocument> GetUnliquidatedDocuments(DateTime until)
        {
            var liquidatedMovements = _payrollRepository.Query()
                .Where(x => !x.DeleteDate.HasValue)
                .SelectMany(x => x.Liquidation)
                .Where(x => !x.DeleteDate.HasValue && x.Date <= until);

            return _liquidationDocumentRepository.Query()
                .Where(x => !x.DeleteDate.HasValue && x.Date <= until && !liquidatedMovements.Any(y => y.Id == x.Id));
        }

        public void Update(EmployeeUpdate employee)
        {
            if (GetActiveEmployees().Any(x => x.Id != employee.Id && x.Code == employee.Code))
                throw new AlreadyExistsException();

            var saved = _employeeRepository.Get(employee.Id);

            saved.Code = employee.Code;
            saved.Name = employee.Name;
            saved.Cuit = employee.Cuit;
            saved.Address = employee.Address;
            saved.Salary = employee.Salary;
            saved.JoinDate = employee.JoinDate ?? DateTime.UtcNow;
        }

        public List<string> UsedCodes()
        {
            return GetActiveEmployees().Select(x => x.Code).ToList();
        }

        public Payroll ClosePayroll(int employeeId)
        {
            if (ExistsClosedPayrollForThisWeek(employeeId))
                throw new AlreadyExistsException();

            var lastDayOfWeek = GetLastDayOfWeek();
            var employee = GetById(employeeId);

            var liquidation = GetUnliquidatedDocuments(employeeId, lastDayOfWeek);
            var payroll = ClosePayroll(employee, liquidation);

            return payroll;
        }

        private bool ExistsClosedPayrollForThisWeek(int employeeId)
        {
            return GetThisWeekPayrolls().Where(x => x.EmployeeId == employeeId).Any();
        }

        private IQueryable<Payroll> GetThisWeekPayrolls()
        {
            var firstDayOfWeek = GetFirstDayOfWeek();
            var lastDayOfWeek = GetLastDayOfWeek();
            return _payrollRepository.Query().Where(x =>
                    !x.DeleteDate.HasValue &&
                    x.CreationDate >= firstDayOfWeek &&
                    x.CreationDate <= lastDayOfWeek);
        }

        private DateTime GetFirstDayOfWeek()
        {
            return _time.UtcNow().FirstDayOfWeek().Date;
        }

        private Payroll ClosePayroll(Employee employee, List<LiquidationDocument> liquidation)
        {
            liquidation.OfType<Absency>().ToList().ForEach(absency => absency.Amount = -employee.DailySalary());
            liquidation.Add(new Salary { Amount = employee.Salary, Employee = employee, EmployeeId = employee.Id });

            var payroll = new Payroll()
            {
                Employee = employee,
                EmployeeId = employee.Id,
                PreviousBalance = employee.Balance,
                Liquidation = liquidation,
                Balance = employee.Balance + liquidation.Select(x => x.Amount).DefaultIfEmpty(0).Sum()
            };

            employee.Accept(payroll);

            _payrollRepository.Add(payroll);

            return payroll;
        }

        public List<Payroll> ClosePayroll()
        {
            var lastDayOfWeek = GetLastDayOfWeek();

            var employees = GetActiveEmployees().ToList();
            var unliquidatedDocuments = GetUnliquidatedDocuments(lastDayOfWeek).ToList();

            var response = new List<Payroll>();
            foreach (var employee in employees)
            {
                var employeeDocuments = unliquidatedDocuments.Where(x => x.EmployeeId == employee.Id).ToList();
                response.Add(ClosePayroll(employee, employeeDocuments));
            }
            return response;
        }

        private List<LiquidationDocument> GetUnliquidatedDocuments(int employeeId, DateTime lastDayOfWeek)
        {
            return GetUnliquidatedDocuments(lastDayOfWeek).Where(x => x.EmployeeId == employeeId).ToList();
        }

        private Employee GetById(int employeeId)
        {
            var employee = _employeeRepository.Get(employeeId);
            if (employee == null)
                throw new NotFoundException();
            return employee;
        }

        protected IQueryable<Employee> GetActiveEmployees()
        {
            return _employeeRepository.Query().Where(x => !x.DeleteDate.HasValue);
        }

        public List<PayrollResponse> GetPayrolls()
        {
            var thisWeekLiquidatedEmployees = GetThisWeekPayrolls().Select(x => x.EmployeeId);
            var emps = thisWeekLiquidatedEmployees.ToList();
            var unliquidatedEmployees = _employeeRepository.Query()
                .Where(x => !x.DeleteDate.HasValue && !thisWeekLiquidatedEmployees.Contains(x.Id));

            var lastDayOfWeek = GetLastDayOfWeek();
            var unliquidatedDocuments = GetUnliquidatedDocuments(lastDayOfWeek);
            var payrolls = unliquidatedEmployees.Select(x => new PayrollResponse
            {
                EmployeeId = x.Id,
                EmployeeCode = x.Code,
                EmployeeName = x.Name,
                Salary = x.Salary,
                Balance = x.Balance,
                Liquidation = unliquidatedDocuments.Where(l => l.EmployeeId == x.Id)
                                .Select(MapUnliquidatedDocument).ToList()
            }).ToList();
            BuildPayrolls(payrolls);
            return payrolls;
        }

        private void BuildPayrolls(List<PayrollResponse> payrolls)
        {
            foreach (var payroll in payrolls)
            {
                payroll.Date = DateTime.UtcNow;
                payroll.Liquidation = BuildLiquidation(payroll.Salary, payroll.Balance, payroll.Liquidation);
                payroll.Total = payroll.Liquidation.Last().Balance;
            }
        }

        private List<UnliquidatedDocument> BuildLiquidation(decimal salary, decimal balance, List<UnliquidatedDocument> unliquidated)
        {
            var result = new List<UnliquidatedDocument>();
            result.Add(new UnliquidatedDocument
            {
                Date = GetFirstDayOfWeek(),
                Type = LiquidationDocumentType.Previous,
                Amount = balance,
            });
            result.AddRange(unliquidated);
            result.Add(new UnliquidatedDocument
            {
                Date = GetLastDayOfWeek(),
                Type = LiquidationDocumentType.Salary,
                Amount = salary
            });
            CalculateBalance(result);
            return result;
        }

        public List<UnliquidatedDocument> GetUnliquidatedDocuments(int id)
        {
            var lastDayOfWeek = GetLastDayOfWeek();
            var employee = GetById(id);
            var unliquidated = GetUnliquidatedDocuments(employee, lastDayOfWeek);
            return unliquidated;
        }

        private List<UnliquidatedDocument> GetUnliquidatedDocuments(Employee employee, DateTime lastDayOfWeek)
        {
            var unliquidated = GetUnliquidatedDocuments(lastDayOfWeek)
                            .Where(x => x.EmployeeId == employee.Id)
                            .OrderBy(x => x.Date).ThenBy(x => x.Id)
                            .Select(MapUnliquidatedDocument).ToList();
            
            return BuildLiquidation(employee.Salary, employee.Balance, unliquidated);
        }

        private static void CalculateBalance(List<UnliquidatedDocument> unliquidated)
        {
            if (unliquidated.Count > 0)
            {
                var balance = 0m;
                for (var i = 0; i < unliquidated.Count; ++i)
                {
                    balance += unliquidated[i].Amount;
                    unliquidated[i].Balance = balance;
                }
            }
        }

        private static readonly Expression<Func<LiquidationDocument, UnliquidatedDocument>> MapUnliquidatedDocument = x => new UnliquidatedDocument
        {
            Id = x.Id,
            Date = x.Date,
            Description = x.Description,
            Type = x is Advance ? LiquidationDocumentType.Advance :
                   x is Absency ? LiquidationDocumentType.Absency :
                   x is GoodsSold ? LiquidationDocumentType.GoodsSold :
                   LiquidationDocumentType.LoanFee,
            Amount = x.Amount,
            LoanId = x is LoanFee ? (x as LoanFee).LoanId : default(int?)
        };
    }
}
