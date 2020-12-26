using FriMav.Application;
using System.Web.Http;

namespace FriMav.Api.Controllers
{
    [RoutePrefix("api/delivery")]
    public class DeliveryController : ApiController
    {
        private IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_deliveryService.GetListing());
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var delivery = _deliveryService.Get(id);
            return Ok(delivery);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(DeliveryCreate delivery)
        {
            _deliveryService.Create(delivery);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            _deliveryService.Delete(id);
            return Ok();
        }
    }
}
