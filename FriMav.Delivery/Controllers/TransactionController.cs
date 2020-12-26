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
        public IHttpActionResult GetAllByPersonId(int personId)
        {
            return Ok(_transactionService.GetAllByPersonId(personId));
        }
    }
}
