using FriMav.Application;
using System.Web.Http;

namespace FriMav.Api.Controllers
{
    [RoutePrefix("api/invoice")]
    public class InvoiceController : ApiController
    {
        private IInvoiceService _invoiceService;
        private IDeliveryService _deliveryService;

        public InvoiceController(
            IInvoiceService invoiceService,
            IDeliveryService deliveryService)
        {
            _invoiceService = invoiceService;
            _deliveryService = deliveryService;
        }

        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_invoiceService.GetAll());
        }

        [Route("undelivered")]
        public IHttpActionResult GetUndelivered()
        {
            return Ok(_deliveryService.GetUndeliveredInvoices());
        }

        [HttpGet]
        [Route("{invoiceId}")]
        public IHttpActionResult Get(int invoiceId)
        {
            var invoice = _invoiceService.GetDisplay(invoiceId);
            return Ok(invoice);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(InvoiceCreate request)
        {
            var invoice = _invoiceService.Create(request);
            return Ok(invoice);
        }
    }
}
