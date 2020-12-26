using FriMav.Domain;
using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public interface IProductTypeService
    {
        IEnumerable<ProductType> GetAll();
        ProductType Get(int id);

        [Transactional]
        void Create(ProductType type);

        [Transactional]
        void Update(ProductType type);

        [Transactional]
        void Delete(int id);
    }
}
