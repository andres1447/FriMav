using System;
using System.Linq;
using FriMav.Application.Configurations;
using FriMav.Domain;
using FriMav.Domain.Entities;
using FriMav.Domain.Entities.Payrolls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace FriMav.Application.Test.Employees
{
    [TestClass]
    public class EmployeeServiceShould
    {
        private const int ID = 1;
        private const decimal SALARY = 10000;
        private const decimal GOODS_SOLD_AMOUNT = 1000;

        private EmployeeService _employeeService;
        private ITime _time;
        private IRepository<Employee> _employeeRepository;
        private IRepository<Zone> _zoneRepository;
        private IRepository<Payroll> _payrollRepository;
        private IRepository<LiquidationDocument> _liquidationDocumentRepository;
        private IRepository<ConfigValue> _configurationRepository;
        private IConfigurationService _configurationService;

        [TestInitialize]
        public void SetUp()
        {
            _time = Substitute.For<ITime>();
            _time.UtcNow().Returns(DateTime.UtcNow);

            _employeeRepository = new MemoryRepository<Employee>();
            _zoneRepository = new MemoryRepository<Zone>();
            _payrollRepository = new MemoryRepository<Payroll>();
            _liquidationDocumentRepository = new MemoryRepository<LiquidationDocument>();
            _configurationRepository = new MemoryRepository<ConfigValue>();
            _configurationService = new ConfigurationService(_configurationRepository);

            _employeeService = new EmployeeService(
                _time,
                _employeeRepository,
                _zoneRepository,
                _payrollRepository,
                _liquidationDocumentRepository, _configurationService);
        }

        [TestMethod]
        public void OnClosePayrollAddSalary()
        {
            GivenAnEmployee(ID, SALARY);

            var payroll = WhenClosePayrollForEmployee(ID);

            ThenPayrollHasSalary(payroll, SALARY);
            ThenPayrollHasBalance(payroll, SALARY);
        }

        [TestMethod]
        public void OnClosePayrollAddGoodsSold()
        {
            GivenAnEmployee(ID, SALARY);
            GivenGoodsSoldToEmployee(ID, -GOODS_SOLD_AMOUNT);

            var payroll = WhenClosePayrollForEmployee(ID);

            ThenPayrollHasGoodsSold(payroll, -GOODS_SOLD_AMOUNT);
            ThenPayrollHasBalance(payroll, SALARY - GOODS_SOLD_AMOUNT);
        }

        [TestMethod]
        public void OnClosePayrollAddUnliquidatedDocuments()
        {
            GivenAnEmployee(ID, SALARY);
            GivenPreviouslyLiquidatedGoodsSoldToEmployee(ID, -GOODS_SOLD_AMOUNT);

            var payroll = WhenClosePayrollForEmployee(ID);

            ThenPayrollHasBalance(payroll, SALARY);
        }

        [TestMethod]
        public void OnClosePayrollDontAddUnliquidatedDocumentsFromFuturePeriod()
        {
            GivenAnEmployee(ID, SALARY);
            var goods = GivenGoodsSoldToEmployee(ID, -GOODS_SOLD_AMOUNT);
            goods.Date = DateTime.UtcNow.AddDays(7);

            var payroll = WhenClosePayrollForEmployee(ID);

            ThenPayrollHasBalance(payroll, SALARY);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void DontClosePayrollIfExistsOneThisWeek()
        {
            GivenAnEmployee(ID, SALARY);
            GivenAnExistingPayrollForEmployee(ID);

            WhenClosePayrollForEmployee(ID);
        }

        [TestMethod]
        public void AddAttendBonusIfIsLastWeekOfMonthAndNoAbsencyInMonth()
        {
            GivenAnEmployee(ID, SALARY);
            GivenConfiguration(Constants.AttendBonusPercentageKey, 0.10m);
            GivenDateIs(new DateTime(2023, 02, 03));

            var payroll = WhenClosePayrollForEmployee(ID);

            ThenPayrollHasAttendBonus(payroll, 4 * SALARY * 0.10m);
        }

        [TestMethod]
        public void DontAddAttendBonusIfPercentageIsZero()
        {
            GivenAnEmployee(ID, SALARY);
            GivenConfiguration(Constants.AttendBonusPercentageKey, 0m);
            GivenDateIs(new DateTime(2023, 02, 03));

            var payroll = WhenClosePayrollForEmployee(ID);

            ThenPayrollHasNoAttendBonus(payroll);
        }

        [TestMethod]
        public void DontAddAttendBonusIfNotIsLastWeekOfMonth()
        {
            GivenAnEmployee(ID, SALARY);
            GivenConfiguration(Constants.AttendBonusPercentageKey, 0.10m);
            GivenDateIs(new DateTime(2023, 01, 15));

            var payroll = WhenClosePayrollForEmployee(ID);

            ThenPayrollHasNoAttendBonus(payroll);
        }

        [TestMethod]
        public void DontAddAttendBonusIfHasAbsencyInClosingMonth()
        {
            GivenAnEmployee(ID, SALARY);
            GivenConfiguration(Constants.AttendBonusPercentageKey, 0.1m);
            GivenDateIs(new DateTime(2023, 02, 03));
            GivenEmployeeHasAbsencyOnDate(new DateTime(2023, 01, 15));

            var payroll = WhenClosePayrollForEmployee(ID);

            ThenPayrollHasNoAttendBonus(payroll);
        }

        [TestMethod]
        public void AddAttendBonusIfHasDeletedAbsencyInClosingMonth()
        {
            GivenAnEmployee(ID, SALARY);
            GivenConfiguration(Constants.AttendBonusPercentageKey, 0.1m);
            GivenDateIs(new DateTime(2023, 02, 03));
            GivenEmployeeHasDeletedAbsencyOnDate(new DateTime(2023, 01, 15));

            var payroll = WhenClosePayrollForEmployee(ID);

            ThenPayrollHasAttendBonus(payroll, 4 * SALARY * 0.10m);
        }

        [TestMethod]
        public void AddAttendBonusIfHasAbsencyInNextMonthSameWeek()
        {
            GivenAnEmployee(ID, SALARY);
            GivenConfiguration(Constants.AttendBonusPercentageKey, 0.1m);
            GivenDateIs(new DateTime(2023, 02, 03));
            GivenEmployeeHasAbsencyOnDate(new DateTime(2023, 02, 04));

            var payroll = WhenClosePayrollForEmployee(ID);

            ThenPayrollHasAttendBonus(payroll, 4 * SALARY * 0.10m);
        }

        [TestMethod]
        public void AddAttendBonusIfHasAbsencyInSameMonthDifferentYear()
        {
            GivenAnEmployee(ID, SALARY);
            GivenConfiguration(Constants.AttendBonusPercentageKey, 0.1m);
            GivenDateIs(new DateTime(2023, 02, 03));
            GivenEmployeeHasAbsencyOnDate(new DateTime(2022, 01, 04));

            var payroll = WhenClosePayrollForEmployee(ID);

            ThenPayrollHasAttendBonus(payroll, 4 * SALARY * 0.10m);
        }

        private void GivenEmployeeHasAbsencyOnDate(DateTime dateTime)
        {
            _liquidationDocumentRepository.Add(new Absency
            {
                EmployeeId = ID,
                Employee = _employeeRepository.Get(ID),
                Date = dateTime
            });
        }

        private void GivenEmployeeHasDeletedAbsencyOnDate(DateTime dateTime)
        {
            _liquidationDocumentRepository.Add(new Absency
            {
                EmployeeId = ID,
                Employee = _employeeRepository.Get(ID),
                Date = dateTime,
                DeleteDate = dateTime
            });
        }

        private void GivenDateIs(DateTime dateTime)
        {
            _time.UtcNow().Returns(dateTime);
        }

        private void ThenPayrollHasNoAttendBonus(Payroll payroll)
        {
            Assert.AreEqual(0, payroll.Liquidation.OfType<AttendBonus>().Count());
        }

        private void ThenPayrollHasAttendBonus(Payroll payroll, decimal value)
        {
            Assert.AreEqual(1, payroll.Liquidation.OfType<AttendBonus>().Count());
            Assert.AreEqual(value, payroll.Liquidation.OfType<AttendBonus>().First().Amount);
        }

        private void GivenConfiguration(string code, decimal value)
        {
            _configurationRepository.Add(new ConfigValue
            {
                Code = code,
                DecimalValue = value
            });
        }

        private void GivenAnExistingPayrollForEmployee(int id)
        {
            _payrollRepository.Add(new Payroll
            {
                EmployeeId = id,
                CreationDate = DateTime.UtcNow
            });
        }

        private void GivenPreviouslyLiquidatedGoodsSoldToEmployee(int id, decimal amount)
        {
            var goodsSold = GivenGoodsSoldToEmployee(id, amount);
            var payroll = new Payroll
            {
                CreationDate = DateTime.UtcNow.AddDays(-7),
                EmployeeId = id,
                Balance = amount,
                Liquidation = new System.Collections.Generic.List<LiquidationDocument>
                {
                    goodsSold
                }
            };
            _payrollRepository.Add(payroll);
        }

        private GoodsSold GivenGoodsSoldToEmployee(int id, decimal amount)
        {
            var goodsSold = new GoodsSold
            {
                Amount = amount,
                Date = DateTime.UtcNow,
                EmployeeId = id
            };
            _liquidationDocumentRepository.Add(goodsSold);
            return goodsSold;
        }

        private void ThenPayrollHasGoodsSold(Payroll payroll, decimal goodsSoldAmount)
        {
            Assert.IsTrue(payroll.Liquidation.OfType<GoodsSold>().Any(x => x.Amount == goodsSoldAmount));
        }

        private void ThenPayrollHasSalary(Payroll payroll, decimal salary)
        {
            Assert.IsTrue(payroll.Liquidation.OfType<Salary>().Any(x => x.Amount == salary));
        }

        private void ThenPayrollHasBalance(Payroll payroll, decimal balance)
        {
            Assert.AreEqual(balance, payroll.Balance);
        }

        private Payroll WhenClosePayrollForEmployee(int id)
        {
            return _employeeService.ClosePayroll(id);
        }

        private void GivenAnEmployee(int id, decimal salary)
        {
            _employeeRepository.Add(new Employee
            {
                Id = id,
                Code = id.ToString(),
                Name = $"Employee_{id}",
                Salary = salary
            });
        }
    }
}
