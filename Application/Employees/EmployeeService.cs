using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FriMav.Application.Configurations;
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
        private readonly IConfigurationService _configurationService;

        public EmployeeService(
            ITime time,
            IRepository<Employee> employeeRepository,
            IRepository<Zone> zoneRepository,
            IRepository<Payroll> payrollRepository,
            IRepository<LiquidationDocument> liquidationDocumentRepository,
            IConfigurationService configurationService)
        {
            _time = time;
            _employeeRepository = employeeRepository;
            _zoneRepository = zoneRepository;
            _payrollRepository = payrollRepository;
            _liquidationDocumentRepository = liquidationDocumentRepository;
            _configurationService = configurationService;
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
            var penalizedEmployees = GetPenalizedEmployees();
            return employees
                .GroupJoin(penalizedEmployees, e => e.Id, e => e, (Employee, Penalized) => new { Employee, IsPenalized = Penalized.Any() ? true : false })
                .GroupJoin(unliquidatedMovements, x => x.Employee.Id, x => x.EmployeeId, (x, unliquidated) =>
            new EmployeeResponse
            {
                Id = x.Employee.Id,
                Name = x.Employee.Name,
                Code = x.Employee.Code,
                Salary = x.Employee.Salary,
                IsLiquidated = payrolls.Contains(x.Employee.Id),
                Absences = unliquidated.OfType<Absency>().Count(),
                LiquidationBalance = unliquidated.Select(d => d.Amount).DefaultIfEmpty(0).Sum() + x.Employee.Balance + x.Employee.Salary,
                Balance = x.Employee.Balance,
                IsPenalized = x.IsPenalized
            }).ToList();
        }

        private IQueryable<int> GetPenalizedEmployees()
        {
            var currentDate = _time.UtcNow();
            var twoMonthsBefore = currentDate.AddMonths(-2);
            return _liquidationDocumentRepository.Query()
                .OfType<Absency>()
                .Where(x => x.Date <= currentDate && x.Date >= twoMonthsBefore)
                .Select(x => x.EmployeeId)
                .Distinct();
        }

        private DateTime GetLastDayOfWeek() => _time.UtcNow().LastDayOfWeek().EndOfDay();

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

            var attendBonus = GetAttendBonusContext();

            var lastDayOfWeek = GetLastDayOfWeek();
            var employee = GetById(employeeId);

            var liquidation = GetUnliquidatedDocuments(employeeId, lastDayOfWeek);
            var payroll = ClosePayroll(employee, liquidation, attendBonus);

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

        private Payroll ClosePayroll(Employee employee, List<LiquidationDocument> liquidation, AttendBonusContext attendBonus)
        {
            liquidation.OfType<Absency>().ToList().ForEach(absency => absency.Amount = -employee.DailySalary());
            liquidation.Add(new Salary { Amount = employee.Salary, Employee = employee, EmployeeId = employee.Id });

            if (attendBonus.IsEgible(employee))
                liquidation.Add(new AttendBonus { Amount = employee.MonthlySalary * attendBonus.Percentage, Employee = employee, EmployeeId = employee.Id });

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
            var attendBonus = GetAttendBonusContext();

            var response = new List<Payroll>();
            foreach (var employee in employees)
            {
                var employeeDocuments = unliquidatedDocuments.Where(x => x.EmployeeId == employee.Id).ToList();
                response.Add(ClosePayroll(employee, employeeDocuments, attendBonus));
            }
            return response;
        }

        private AttendBonusContext GetAttendBonusContext()
        {
            var percentage = _configurationService.GetDecimal(Constants.AttendBonusPercentageKey);

            var currentDate = _time.UtcNow();
            var firstDateOfWeek = currentDate.FirstDayOfWeek();
            var lastDateOfWeek = currentDate.LastDayOfWeek();
            var isClosingMonth = lastDateOfWeek.Month != firstDateOfWeek.Month || lastDateOfWeek.IsLastDayOfMonth();

            var absencies = percentage <= 0
                ? new List<int>()
                : isClosingMonth
                    ? GetAbsenciesInMonthOf(firstDateOfWeek)
                    : GetAbsenciesInMonthOf(currentDate);

            return new AttendBonusContext
            {
                IsClosingMonth = isClosingMonth,
                Percentage = percentage,
                MonthlyAbsents = absencies
            };
        }

        private List<int> GetAbsenciesInMonthOf(DateTime currentDate)
        {
            return _liquidationDocumentRepository.Query()
                .OfType<Absency>()
                .Where(x => x.Date.Month == currentDate.Month && x.Date.Month == currentDate.Month)
                .Select(x => x.Employee.Id)
                .Distinct()
                .ToList();
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

            var payrolls = unliquidatedEmployees.Select(x => new EmployeeLiquidation
            {
                Employee = x,
                Liquidation = unliquidatedDocuments.Where(l => l.EmployeeId == x.Id).Select(MapUnliquidatedDocument).ToList()
            }).ToList();
            
            return BuildPayrolls(payrolls);
        }

        private List<PayrollResponse> BuildPayrolls(List<EmployeeLiquidation> payrolls)
        {
            var attendBonus = GetAttendBonusContext();
            return payrolls.Select(x =>
            {
                var liquidation = BuildLiquidation(x, attendBonus);
                return new PayrollResponse
                {
                    EmployeeCode = x.Employee.Code,
                    EmployeeId = x.Employee.Id,
                    EmployeeName = x.Employee.Name,
                    Balance = x.Employee.Balance,
                    Salary = x.Employee.Salary,
                    Date = DateTime.UtcNow,
                    Liquidation = liquidation,
                    Total = liquidation.Last().Balance,
                    HasAttendBonus = !attendBonus.IsClosingMonth && attendBonus.WillApply(x.Employee.Id)
                };
            }).ToList();
        }

        private List<UnliquidatedDocument> BuildLiquidation(EmployeeLiquidation payroll, AttendBonusContext attendBonus)
        {
            var firstDayOfWeek = GetFirstDayOfWeek();
            var minimumDay = payroll.Liquidation.Select(x => x.Date).DefaultIfEmpty(firstDayOfWeek).Min();
            var previousBalanceDay = minimumDay < firstDayOfWeek ? minimumDay : firstDayOfWeek;
            var result = new List<UnliquidatedDocument>();
            result.Add(new UnliquidatedDocument
            {
                Date = previousBalanceDay,
                Type = LiquidationDocumentType.Previous,
                Amount = payroll.Employee.Balance,
            });
            result.AddRange(payroll.Liquidation);
            result.Add(new UnliquidatedDocument
            {
                Date = GetLastDayOfWeek(),
                Type = LiquidationDocumentType.Salary,
                Amount = payroll.Employee.Salary
            });
            if (attendBonus.IsEgible(payroll.Employee))
                result.Add(new UnliquidatedDocument
                {
                    Date = GetLastDayOfWeek(),
                    Type = LiquidationDocumentType.AttendBonus,
                    Amount = attendBonus.Percentage * payroll.Employee.MonthlySalary
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

            var attendBonus = GetAttendBonusContext();

            var liquidation = new EmployeeLiquidation
            {
                Employee = employee,
                Liquidation = unliquidated
            };

            return BuildLiquidation(liquidation, attendBonus);
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

        public EmployeeAccountResponse GetLiquidatedDocuments(int employeeId, int offset = 0, int count = 20)
        {
            var lastDayOfWeek = GetLastDayOfWeek();
            var employeeDocuments = _liquidationDocumentRepository.Query()
                            .Where(x => x.EmployeeId == employeeId && !x.DeleteDate.HasValue && x.CreationDate <= lastDayOfWeek);
            var totalCount = employeeDocuments.Count();

            var items = employeeDocuments
                .OrderByDescending(x => x.Date).ThenByDescending(x => x.Id)
                .Select(x => new UnliquidatedDocument
                {
                    Id = x.Id,
                    Date = x.Date,
                    Description = x.Description,
                    Type = x is Advance ? LiquidationDocumentType.Advance :
                           x is Absency ? LiquidationDocumentType.Absency :
                           x is GoodsSold ? LiquidationDocumentType.GoodsSold :
                           x is Salary ? LiquidationDocumentType.Salary
                                : LiquidationDocumentType.LoanFee,
                    Amount = x.Amount,
                    Balance = x.Amount + employeeDocuments.Where(d => d.CreationDate < x.Date).Select(d => d.Amount).DefaultIfEmpty(0).Sum(),
                    LoanId = x is LoanFee ? (x as LoanFee).LoanId : default(int?)
                })
                .Skip(offset * count)
                .Take(count)
                .OrderBy(x => x.Date).ThenBy(x => x.Id)
                .ToList();

            return new EmployeeAccountResponse(totalCount, items);
        }

        private static readonly Expression<Func<LiquidationDocument, UnliquidatedDocument>> MapUnliquidatedDocument = x => new UnliquidatedDocument
        {
            Id = x.Id,
            Date = x.Date,
            Description = x.Description,
            Type = x is Advance
                ? LiquidationDocumentType.Advance
                : x is Absency
                    ? LiquidationDocumentType.Absency
                    : x is GoodsSold
                        ? LiquidationDocumentType.GoodsSold
                        : x is LoanFee
                            ? LiquidationDocumentType.LoanFee
                            : LiquidationDocumentType.AttendBonus,
            Amount = x.Amount,
            Balance = 0,
            LoanId = x is LoanFee ? (x as LoanFee).LoanId : default(int?)
        };

        private class EmployeeLiquidation
        {
            public Employee Employee { get; set; }
            public List<UnliquidatedDocument> Liquidation { get; set; }
        }

        private class AttendBonusContext
        {
            public bool IsClosingMonth { get; set; }
            public decimal Percentage { get; set; }
            public List<int> MonthlyAbsents { get; set; }

            internal bool IsEgible(Employee employee)
            {
                return IsClosingMonth && WillApply(employee.Id);
            }

            internal bool WillApply(int employeeId)
            {
                return Percentage > 0 && !MonthlyAbsents.Contains(employeeId);
            }
        }
    }
}
