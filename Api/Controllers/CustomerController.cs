using FriMav.Application;
using FriMav.Domain;
using System.Web.Http;

namespace FriMav.Api.Controllers
{
    [RoutePrefix("api/customer")]
    public class CustomerController : ApiController
    {
        private ICustomerService _customerService;
        private IProductService _productService;

        public CustomerController(
            ICustomerService customerService,
            IProductService productService)
        {
            _customerService = customerService;
            _productService = productService;
        }

        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_customerService.GetAll());
        }

        [HttpGet]
        [Route("{personId}")]
        public IHttpActionResult Get(int personId)
        {
            var customer = _customerService.Get(personId);
            if (customer != null)
                return Ok(customer);

            return NotFound();
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(CustomerCreate customer)
        {
            _customerService.Create(customer.ToDomain());
            return Ok();
        }

        [HttpPut]
        [Route("{personId}")]
        public IHttpActionResult Update(int personId, CustomerUpdate customer)
        {
            var saved = _customerService.Get(personId);
            if (saved == null)
                return NotFound();

            _customerService.Update(customer.ToDomain());
            return Ok();
        }

        [HttpGet]
        [Route("{personId}/products")]
        public IHttpActionResult GetProductList(int personId)
        {
            return Ok(_productService.GetAllProductPriceForCustomer(personId));
        }

        [HttpDelete]
        [Route("{personId}")]
        public IHttpActionResult Delete(int personId)
        {
            var customer = _customerService.Get(personId);
            if (customer != null)
            {
                _customerService.Delete(customer);
                return Ok();
            }
            return NotFound();
        }
    }
}
