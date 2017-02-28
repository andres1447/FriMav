using FriMav.Domain;
using FriMav.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public interface IProductFamilyService
    {
        IEnumerable<ProductFamily> GetAll();
        ProductFamily Get(int familyId);
        void Create(ProductFamily family);
        void Update(ProductFamily family);
        void Delete(ProductFamily family);
    }
}
