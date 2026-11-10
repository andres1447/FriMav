using System.Collections.Generic;
using System.Linq;
using FriMav.Domain;
using FriMav.Domain.Entities;

namespace FriMav.Application
{
    public class ProductMeasureService : IProductMeasureService
    {
        private readonly IRepository<ProductMeasure> _productMeasureRepository;

        public ProductMeasureService(IRepository<ProductMeasure> productMeasureRepository)
        {
            _productMeasureRepository = productMeasureRepository;
        }

        public void Create(ProductMeasure measure)
        {
            _productMeasureRepository.Add(measure);
        }

        public void Delete(int id)
        {
            var measure = _productMeasureRepository.Get(id);
            if (measure == null)
                throw new NotFoundException();

            measure.Delete();
        }

        public ProductMeasure Get(int id)
        {
            return _productMeasureRepository.Get(id);
        }

        public IEnumerable<ProductMeasure> GetAll()
        {
            return _productMeasureRepository.Query()
                .Where(x => !x.DeleteDate.HasValue)
                .ToList();
        }

        public void Update(ProductMeasure measure)
        {
            var saved = _productMeasureRepository.Get(measure.Id);
            if (saved == null)
                throw new NotFoundException();

            saved.Name = measure.Name;
            saved.ShortName = measure.ShortName;
        }
    }
}
