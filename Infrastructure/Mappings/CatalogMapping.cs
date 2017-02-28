using FriMav.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings
{
    class CatalogMapping : BaseMapping<Catalog>
    {
        public override void Mapping()
        {
            this.ToTable("Catalog");
            this.HasKey(c => c.CatalogId);
            this.Property(c => c.Name).HasMaxLength(255);

            this.HasMany(c => c.Products)
                .WithMany()
                .Map(x =>
                {
                    x.MapLeftKey("CatalogId");
                    x.MapRightKey("ProductId");
                    x.ToTable("CatalogProduct");
                });
        }
    }
}
