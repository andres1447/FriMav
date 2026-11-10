using FriMav.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FriMav.Infrastructure.Mappings
{
    public class ProductMeasureMapping : EntityTypeConfiguration<ProductMeasure>
    {
        public ProductMeasureMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired().HasMaxLength(128);
            Property(x => x.ShortName).IsRequired().HasMaxLength(2);
        }
    }
}
