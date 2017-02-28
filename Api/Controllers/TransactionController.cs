using FriMav.Application;
using System.Web.Http;

namespace FriMav.Api.Controllers
{
    [RoutePrefix("api/transaction")]
    public class TransactionController : ApiController
    {
        private ITransactionService _transactionService;

        public TransactionController(
            ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        [Route("{transactionId}")]
        public IHttpActionResult Get(int transactionId)
        {
            return Ok(_transactionService.Get(transactionId));
        }

        [HttpGet]
        [Route("person/{personId}")]
        public IHttpActionResult GetAllByPersonId(int personId)
        {
            return Ok(_transactionService.FindAllWithReferenceByPersonId(personId));
        }

        [HttpPost]
        [Route("cancel")]
        public IHttpActionResult Cancel(CancelTransaction cancelation)
        {
            _transactionService.Cancel(cancelation);
            return Ok();
        }
    }
}
