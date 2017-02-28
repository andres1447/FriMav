using FriMav.Application;
using System.Web.Http;

namespace FriMav.Api.Controllers
{
    [RoutePrefix("api/product")]
    public class ProductController : ApiController
    {
        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll(string code = "", string name = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            if (string.IsNullOrEmpty(code) && string.IsNullOrEmpty(name) && pageIndex == 0 && pageSize == int.MaxValue)
                return Ok(_productService.GetAll());

            return Ok(_productService.GetPaged(code, name, pageIndex, pageSize));
        }

        [HttpGet]
        [Route("active")]
        public IHttpActionResult GetActive()
        {
            return Ok(_productService.GetAllActive());
        }

        [HttpGet]
        [Route("{productId}")]
        public IHttpActionResult Get(int productId)
        {
            var product = _productService.Get(productId);
            if (product != null)
                return Ok(product);

            return NotFound();
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(ProductCreate product)
        {
            _productService.Create(product.ToDomain());
            return Ok();
        }

        [HttpDelete]
        [Route("{productId}")]
        public IHttpActionResult Delete(int productId)
        {
            var product = _productService.Get(productId);
            if (product != null)
            {
                _productService.Delete(product);
                return Ok();
            }
            return NotFound();
        }

        [HttpPut]
        [Route("{productId}")]
        public IHttpActionResult Update(int productId, ProductUpdate product)
        {
            var saved = _productService.Get(productId);
            if (saved == null)
                return NotFound();

            _productService.Update(product.ToDomain());
            return Ok();
        }
    }
}
