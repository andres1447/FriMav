using FriMav.Application;
using FriMav.Domain;
using System.Web.Http;

namespace FriMav.Api.Controllers
{
    [RoutePrefix("api/employee")]
    public class EmployeeController : ApiController
    {
        private IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_employeeService.GetAll());
        }

        [HttpGet]
        [Route("{personId}")]
        public IHttpActionResult Get(int personId)
        {
            var employee = _employeeService.Get(personId);
            if (employee != null)
                return Ok(employee);

            return NotFound();
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(EmployeeCreate employee)
        {
            _employeeService.Create(employee.ToDomain());
            return Ok();
        }

        [HttpPut]
        [Route("{personId}")]
        public IHttpActionResult Update(int personId, EmployeeUpdate employee)
        {
            var saved = _employeeService.Get(personId);
            if (saved == null)
                return NotFound();

            _employeeService.Update(employee.ToDomain());
            return Ok();
        }

        [HttpDelete]
        [Route("{personId}")]
        public IHttpActionResult Delete(int personId)
        {
            var employee = _employeeService.Get(personId);
            if (employee != null)
            {
                _employeeService.Delete(employee);
                return Ok();
            }
            return NotFound();
        }
    }
}
