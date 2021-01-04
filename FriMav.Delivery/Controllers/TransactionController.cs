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
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            return Ok(_transactionService.Get(id));
        }

        [HttpGet]
        [Route("person/{personId}")]
        public IHttpActionResult GetAllByPersonId(int personId, [FromUri]int offset = 0, [FromUri]int count = 20)
        {
            return Ok(_transactionService.GetAllByPersonId(personId, offset, count));
        }

        [HttpPost]
        [Route("cancel")]
        public IHttpActionResult Cancel([FromBody] CancelTransaction request)
        {
            _transactionService.Cancel(request);
            return Ok();
        }
    }
}
