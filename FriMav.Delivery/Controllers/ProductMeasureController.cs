using FriMav.Application;
using FriMav.Domain.Entities;
using System.Web.Http;

namespace FriMav.Api.Controllers
{
    [RoutePrefix("api/productmeasure")]
    public class ProductMeasureController : ApiController
    {
        private readonly IProductMeasureService _productMeasureService;

        public ProductMeasureController(IProductMeasureService productMeasureService)
        {
            _productMeasureService = productMeasureService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_productMeasureService.GetAll());
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            return Ok(_productMeasureService.Get(id));
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(ProductMeasure request)
        {
            _productMeasureService.Create(request);
            return Ok();
        }

        [HttpPut]
        [Route("")]
        [Route("{id}")]
        public IHttpActionResult Update(ProductMeasure measure)
        {
            _productMeasureService.Update(measure);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            _productMeasureService.Delete(id);
            return Ok();
        }
    }
}
