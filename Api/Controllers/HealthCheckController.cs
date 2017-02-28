using FriMav.Application;
using FriMav.Domain;
using System.Web.Http;

namespace FriMav.Api.Controllers
{
    [RoutePrefix("api/healthcheck")]
    public class HealthCheckController : ApiController
    {
        private IHealthCheckService _healthCheckService;

        public HealthCheckController(IHealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult KeepAlive()
        {
            _healthCheckService.KeepAlive();
            return Ok();
        }
    }
}
