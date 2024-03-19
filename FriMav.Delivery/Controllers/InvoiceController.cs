using FriMav.Application;
using System.Web.Http;

namespace FriMav.Api.Controllers
{

    [RoutePrefix("api/invoice")]
    public partial class InvoiceController : ApiController
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

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_invoiceService.GetAll());
        }

        [HttpGet]
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

        [HttpPost]
        [Route("ticket")]
        public IHttpActionResult CreateTicket(TicketCreate request)
        {
            _invoiceService.CreateTicket(request);
            return Ok();
        }

        [HttpPost]
        [Route("ticket/cancel")]
        public IHttpActionResult CancelTicket(TicketCreate request)
        {
            _invoiceService.CancelTicket(request);
            return Ok();
        }

        [HttpPost]
        [Route("{id}/dontDeliver")]
        public IHttpActionResult DontSend([FromUri]int id)
        {
            _invoiceService.DontDeliver(id);
            return Ok();
        }

        [HttpPost]
        [Route("{id}/externalReferenceNumber")]
        public IHttpActionResult AssignExternalReferenceNumber([FromUri] int id, AssignExternalReferenceNumberRequest request)
        {
            _invoiceService.AssignExternalReferenceNumber(id, request.Number);
            return Ok();
        }
    }
}
