using FriMav.Application.Configurations;
using FriMav.Domain.Entities;
using System.Web.Http;

namespace FriMav.Api.Controllers
{
    [RoutePrefix("api/configValues")]
    public class ConfigValuesController : ApiController
    {
        private IConfigurationService _configurationService;

        public ConfigValuesController(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_configurationService.GetAll());
        }

        [HttpPut]
        [Route("")]
        public IHttpActionResult Create(ConfigValue configuration)
        {
            _configurationService.Update(configuration);
            return Ok();
        }
    }
}
