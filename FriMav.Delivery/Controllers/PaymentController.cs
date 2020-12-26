using FriMav.Application;
using System.Web.Http;

namespace FriMav.Api.Controllers
{
    [RoutePrefix("api/payment")]
    public class PaymentController : ApiController
    {
        private ITransactionService _transactionService;

        public PaymentController(
            ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(CreatePayment payment)
        {
            _transactionService.Create(payment);
            return Ok();
        }
    }
}
