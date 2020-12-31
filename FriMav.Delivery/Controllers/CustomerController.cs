using FriMav.Application;
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

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_customerService.GetAll());
        }

        [HttpGet]
        [Route("codes")]
        public IHttpActionResult UsedCodes()
        {
            return Ok(_customerService.UsedCodes());
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var customer = _customerService.Get(id);
            return Ok(customer);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(CustomerCreate customer)
        {
            _customerService.Create(customer);
            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Update(int id, CustomerUpdate customer)
        {
            customer.Id = id;
            _customerService.Update(customer);
            return Ok();
        }

        [HttpGet]
        [Route("{id}/products")]
        public IHttpActionResult GetProductList(int id)
        {
            return Ok(_productService.GetPriceList(id));
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            _customerService.Delete(id);
            return Ok();
        }
    }
}
