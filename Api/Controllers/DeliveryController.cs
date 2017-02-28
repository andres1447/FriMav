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
        [Route("{deliveryId}")]
        public IHttpActionResult Get(int deliveryId)
        {
            var delivery = _deliveryService.Get(deliveryId);
            if (delivery != null)
                return Ok(delivery);

            return NotFound();
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(DeliveryCreate delivery)
        {
            _deliveryService.Create(delivery);
            return Ok();
        }

        [HttpDelete]
        [Route("{deliveryId}")]
        public IHttpActionResult Delete(int deliveryId)
        {
            var delivery = _deliveryService.Get(deliveryId);
            if (delivery != null)
            {
                _deliveryService.Delete(delivery);
                return Ok();
            }
            return NotFound();
        }
    }
}
