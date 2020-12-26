using FriMav.Application;
using FriMav.Domain;
using FriMav.Domain.Entities;
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
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            return Ok(_zoneService.Get(id));
        }

        [HttpGet]
        [Route("{id}/customers")]
        public IHttpActionResult GetFamilyCustomers(int id)
        {
            return Ok(_customerService.GetAllInZone(id));
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(Zone zone)
        {
            _zoneService.Create(zone);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            _zoneService.Delete(id);
            return Ok();
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
