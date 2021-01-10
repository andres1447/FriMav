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

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_deliveryService.GetListing());
        }

        [HttpGet]
        [Route("pending")]
        public IHttpActionResult HasPending()
        {
            return Ok(_deliveryService.HasPendingDeliveries());
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
        public IHttpActionResult Create(DeliveryCreate request)
        {
            _deliveryService.Create(request);
            return Ok();
        }

        [HttpGet]
        [Route("{id}/close")]
        public IHttpActionResult GetForClose(int id)
        {
            var delivery = _deliveryService.GetForClose(id);
            return Ok(delivery);
        }

        [HttpPost]
        [Route("{id}/close")]
        public IHttpActionResult Close([FromUri]int id, DeliveryClose request)
        {
            request.Id = id;
            _deliveryService.Close(request);
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
