using FriMav.Application;
using System.Web.Http;

namespace FriMav.Api.Controllers
{
    [RoutePrefix("api/invoice")]
    public class InvoiceController : ApiController
    {
        private IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_invoiceService.GetAll());
        }

        [Route("undelivered")]
        public IHttpActionResult GetUndelivered()
        {
            return Ok(_invoiceService.GetUndeliveredInvoices());
        }

        [HttpGet]
        [Route("{invoiceId}")]
        public IHttpActionResult Get(int invoiceId)
        {
            var invoice = _invoiceService.GetDisplay(invoiceId);
            if (invoice != null)
                return Ok(invoice);

            return NotFound();
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(InvoiceCreate invoice)
        {
            var transaction = invoice.ToDomain();
            _invoiceService.Create(transaction);
            return Ok(new { Number = transaction.Number, Balance = transaction.Balance, Total = transaction.Total });
        }
    }
}
