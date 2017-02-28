using FriMav.Domain;
using FriMav.Domain.Proyections;
using System.Collections.Generic;

namespace FriMav.Application
{
    public interface ICatalogService
    {
        Catalog Get(int catalogId);
        IEnumerable<Catalog> GetAll();
        void Create(CatalogCreate command);
        void Update(Catalog catalog);
        void Delete(Catalog catalog);
    }
}
