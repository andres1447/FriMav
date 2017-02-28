using FriMav.Application;
using System.Web.Http;

namespace FriMav.Api.Controllers
{
    [RoutePrefix("api/catalog")]
    public class CatalogController : ApiController
    {
        private ICatalogService _catalogService;
        private IProductService _productService;

        public CatalogController(ICatalogService catalogService,
            IProductService productService)
        {
            _catalogService = catalogService;
            _productService = productService;
        }

        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_catalogService.GetAll());
        }

        [HttpGet]
        [Route("{catalogId}")]
        public IHttpActionResult Get(int catalogId)
        {
            var catalog = _catalogService.Get(catalogId);
            if (catalog != null)
                return Ok(catalog);

            return NotFound();
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(CatalogCreate catalog)
        {
            _catalogService.Create(catalog);
            return Ok();
        }

        [HttpDelete]
        [Route("{catalogId}")]
        public IHttpActionResult Delete(int catalogId)
        {
            var catalog = _catalogService.Get(catalogId);
            if (catalog != null)
            {
                _catalogService.Delete(catalog);
                return Ok();
            }
            return NotFound();
        }
    }
}
