using FriMav.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using FriMav.Domain.Repositories;
using System.Data.SqlClient;
using Dapper;
using FriMav.Domain.Proyections;
using System;

namespace FriMav.Infrastructure.Repositories
{
    public class CatalogRepository : BaseRepository<Catalog>, ICatalogRepository
    {
        public CatalogRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public Catalog GetByIdWithProducts(int catalogId)
        {
            return _databaseContext.Set<Catalog>()
                .Include(x => x.Products)
                .FirstOrDefault(x => x.CatalogId == catalogId);
        }
    }
}
