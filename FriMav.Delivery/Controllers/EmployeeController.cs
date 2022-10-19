using FriMav.Application;
using FriMav.Application.Employees;
using System.Web.Http;

namespace FriMav.Api.Controllers
{
    [RoutePrefix("api/employee")]
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAbsencyService _absencyService;
        private readonly IAdvanceService _advanceService;
        private readonly ILoanService _loanService;
        private readonly IGoodsSoldService _goodSoldService;

        public EmployeeController(
            IEmployeeService employeeService,
            IAbsencyService absencyService,
            IAdvanceService advanceService,
            ILoanService loanService,
            IGoodsSoldService goodSoldService)
        {
            _employeeService = employeeService;
            _absencyService = absencyService;
            _advanceService = advanceService;
            _loanService = loanService;
            _goodSoldService = goodSoldService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_employeeService.GetAll());
        }

        [HttpGet]
        [Route("codes")]
        public IHttpActionResult UsedCodes()
        {
            return Ok(_employeeService.UsedCodes());
        }

        [HttpPost]
        [Route("absency")]
        public IHttpActionResult Absency(AbsencyCreate request)
        {
            _absencyService.Create(request);
            return Ok();
        }

        [HttpDelete]
        [Route("absency/{id:int}")]
        public IHttpActionResult DeleteAbsency(int id)
        {
            _absencyService.Delete(id);
            return Ok();
        }

        [HttpPost]
        [Route("advance")]
        public IHttpActionResult Advance(AdvanceCreate request)
        {
            _advanceService.Create(request);
            return Ok();
        }

        [HttpDelete]
        [Route("advance/{id:int}")]
        public IHttpActionResult DelteAdvance(int id)
        {
            _advanceService.Delete(id);
            return Ok();
        }

        [HttpPost]
        [Route("loan")]
        public IHttpActionResult Loan(LoanCreate request)
        {
            _loanService.Create(request);
            return Ok();
        }

        [HttpDelete]
        [Route("loan/{id:int}")]
        public IHttpActionResult DelteLoan(int id)
        {
            _loanService.Delete(id);
            return Ok();
        }

        [HttpGet]
        [Route("loan/{id:int}")]
        public IHttpActionResult GetLoan(int id)
        {
            return Ok(_loanService.Get(id));
        }

        [HttpPost]
        [Route("goods")]
        public IHttpActionResult Goods(GoodsSoldCreate request)
        {
            _goodSoldService.Create(request);
            return Ok();
        }

        [HttpDelete]
        [Route("goods/{id:int}")]
        public IHttpActionResult DelteGoods(int id)
        {
            _goodSoldService.Delete(id);
            return Ok();
        }

        [HttpGet]
        [Route("goods/{id:int}")]
        public IHttpActionResult GetGood(int id)
        {
            return Ok(_goodSoldService.Get(id));
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var employee = _employeeService.Get(id);
            return Ok(employee);
        }

        [HttpGet]
        [Route("{id}/loanFees")]
        public IHttpActionResult LoanFees(int id)
        {
            var fees = _loanService.GetRemainingFees(id);
            return Ok(fees);
        }

        [HttpGet]
        [Route("{id:int}/unliquidated")]
        public IHttpActionResult UnliquidatedDocuments(int id)
        {
            return Ok(_employeeService.GetUnliquidatedDocuments(id));
        }

        [HttpGet]
        [Route("{id:int}/liquidated")]
        public IHttpActionResult UnliquidatedDocuments(int id, int offset = 0, int count = 20)
        {
            return Ok(_employeeService.GetLiquidatedDocuments(id, offset, count));
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(EmployeeCreate employee)
        {
            _employeeService.Create(employee);
            return Ok();
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, EmployeeUpdate employee)
        {
            employee.Id = id;
            _employeeService.Update(employee);
            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            _employeeService.Delete(id);
            return Ok();
        }

        [HttpGet]
        [Route("payroll")]
        public IHttpActionResult GetPayrolls()
        {
            return Ok(_employeeService.GetPayrolls());
        }

        [HttpPost]
        [Route("{id:int}/payroll")]
        public IHttpActionResult ClosePayroll(int id)
        {
            _employeeService.ClosePayroll(id);
            return Ok();
        }

        [HttpPost]
        [Route("payroll")]
        public IHttpActionResult CloseAllPayrolls()
        {
            _employeeService.ClosePayroll();
            return Ok();
        }
    }
}
