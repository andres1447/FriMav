using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings
{
    public class CatalogMapping : EntityTypeConfiguration<Catalog>
    {
        public CatalogMapping()
        {
            HasMany(x => x.Products).WithMany().Map(m => m.MapLeftKey("CatalogId").MapRightKey("ProductId"));
        }
    }
}
