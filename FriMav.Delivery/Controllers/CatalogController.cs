using FriMav.Application;
using System.Web.Http;

namespace FriMav.Api.Controllers
{
    [RoutePrefix("api/catalog")]
    public class CatalogController : ApiController
    {
        private ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_catalogService.GetAll());
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var catalog = _catalogService.Get(id);
            return Ok(catalog);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(CatalogCreate catalog)
        {
            _catalogService.Create(catalog);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            _catalogService.Delete(id);
            return Ok();
        }
    }
}
