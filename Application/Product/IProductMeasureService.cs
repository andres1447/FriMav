using FriMav.Domain;
using FriMav.Domain.Entities;
using System.Collections.Generic;

namespace FriMav.Application
{
    public interface IProductMeasureService
    {
        IEnumerable<ProductMeasure> GetAll();
        ProductMeasure Get(int id);

        [Transactional]
        void Create(ProductMeasure measure);

        [Transactional]
        void Update(ProductMeasure measure);

        [Transactional]
        void Delete(int id);
    }
}
