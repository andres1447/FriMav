using FriMav.Application;
using FriMav.Domain;
using System.Web.Http;

namespace FriMav.Api.Controllers
{
    [RoutePrefix("api/family")]
    public class ProductFamilyController : ApiController
    {
        private IProductTypeService _productFamilyService;
        private IProductService _productService;

        public ProductFamilyController(
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
        [Route("{familyId}")]
        public IHttpActionResult Get(int familyId)
        {
            var family = _productFamilyService.Get(familyId);
            if (family != null)
                return Ok(family);

            return NotFound();
        }

        [HttpGet]
        [Route("{familyId}/products")]
        public IHttpActionResult GetFamilyProducts(int familyId)
        {
            return Ok(_productService.GetAllActiveInFamily(familyId));
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(ProductFamily family)
        {
            _productFamilyService.Create(family);
            return Ok();
        }

        [HttpDelete]
        [Route("{familyId}")]
        public IHttpActionResult Delete(int familyId)
        {
            var family = _productFamilyService.Get(familyId);
            if (family != null)
            {
                _productFamilyService.Delete(family);
                return Ok();
            }
            return NotFound();
        }

        [HttpPut]
        [Route("")]
        public IHttpActionResult Update(ProductFamily family)
        {
            _productFamilyService.Update(family);
            return Ok();
        }
    }
}
