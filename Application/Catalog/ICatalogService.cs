using FriMav.Domain;
using FriMav.Domain.Entities;
using FriMav.Domain.Proyections;
using System.Collections.Generic;

namespace FriMav.Application
{
    public interface ICatalogService
    {
        Catalog Get(int id);
        IEnumerable<Catalog> GetAll();

        [Transactional]
        void Create(CatalogCreate command);

        [Transactional]
        void Update(Catalog catalog);

        [Transactional]
        void Delete(int id);
    }
}
