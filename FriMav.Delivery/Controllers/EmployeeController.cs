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

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var employee = _employeeService.Get(id);
            return Ok(employee);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(EmployeeCreate employee)
        {
            _employeeService.Create(employee);
            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Update(int id, EmployeeUpdate employee)
        {
            employee.Id = id;
            _employeeService.Update(employee);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            _employeeService.Delete(id);
            return Ok();
        }
    }
}
