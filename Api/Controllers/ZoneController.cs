using FriMav.Application;
using FriMav.Domain;
using System.Web.Http;

namespace FriMav.Api.Controllers
{
    [RoutePrefix("api/zone")]
    public class ZoneController : ApiController
    {
        private IZoneService _zoneService;
        private ICustomerService _customerService;

        public ZoneController(
            IZoneService zoneService,
            ICustomerService customerService)
        {
            _zoneService = zoneService;
            _customerService = customerService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_zoneService.GetAll());
        }

        [HttpGet]
        [Route("{zoneId}")]
        public IHttpActionResult Get(int zoneId)
        {
            var zone = _zoneService.Get(zoneId);
            if (zone != null)
                return Ok(zone);

            return NotFound();
        }

        [HttpGet]
        [Route("{zoneId}/customers")]
        public IHttpActionResult GetFamilyCustomers(int zoneId)
        {
            return Ok(_customerService.GetAllInZone(zoneId));
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(Zone zone)
        {
            _zoneService.Create(zone);
            return Ok();
        }

        [HttpDelete]
        [Route("{zoneId}")]
        public IHttpActionResult Delete(int zoneId)
        {
            var zone = _zoneService.Get(zoneId);
            if (zone != null)
            {
                _zoneService.Delete(zone);
                return Ok();
            }
            return NotFound();
        }

        [HttpPut]
        [Route("")]
        public IHttpActionResult Update(Zone zone)
        {
            _zoneService.Update(zone);
            return Ok();
        }
    }
}
