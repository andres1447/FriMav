using FriMav.Application;
using FriMav.Domain.Entities;
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
            return Ok(_productService.GetAll());
        }

        [HttpGet]
        [Route("active")]
        public IHttpActionResult GetActive()
        {
            return Ok(_productService.GetAllActive());
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            return Ok(_productService.Get(id));
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(ProductCreate product)
        {
            _productService.Create(product);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            _productService.Delete(id);
            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Update(int id, ProductUpdate product)
        {
            product.Id = id;
            _productService.Update(product);
            return Ok();
        }
    }
}
