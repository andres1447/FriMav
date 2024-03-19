using FriMav.Application.Billing;
using System.Web.Http;

namespace FriMav.Delivery.Controllers
{
    [RoutePrefix("api/billing")]
    public class BillingController : ApiController
    {
        private readonly IBillingMediator _billingMediator;

        public BillingController(IBillingMediator billingMediator)
        {
            _billingMediator = billingMediator;
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult GetReport(BillingReportRequest request)
        {
            return Ok(_billingMediator.Execute(request));
        }
    }
}
