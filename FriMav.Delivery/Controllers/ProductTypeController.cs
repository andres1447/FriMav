using FriMav.Application;
using FriMav.Domain.Entities;
using System.Web.Http;

namespace FriMav.Api.Controllers
{
    [RoutePrefix("api/producttype")]
    public class ProductTypeController : ApiController
    {
        private IProductTypeService _productFamilyService;
        private IProductService _productService;

        public ProductTypeController(
            IProductTypeService productFamilyService,
            IProductService productService)
        {
            _productFamilyService = productFamilyService;
            _productService = productService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_productFamilyService.GetAll());
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            return Ok(_productFamilyService.Get(id));
        }

        [HttpGet]
        [Route("{id}/products")]
        public IHttpActionResult GetFamilyProducts(int id)
        {
            return Ok(_productService.GetAllActiveInFamily(id));
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(ProductType request)
        {
            _productFamilyService.Create(request);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            _productFamilyService.Delete(id);
            return Ok();
        }

        [HttpPut]
        [Route("")]
        public IHttpActionResult Update(ProductType type)
        {
            _productFamilyService.Update(type);
            return Ok();
        }
    }
}
