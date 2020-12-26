using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings
{
    public class ProductMapping : EntityTypeConfiguration<Product>
    {
        public ProductMapping()
        {
            HasKey(x => x.Id);

            Property(x => x.Code).IsRequired().HasMaxLength(10);
            Property(x => x.Name).IsRequired().HasMaxLength(128);

            HasOptional(x => x.Type).WithMany().HasForeignKey(x => x.ProductTypeId);
        }
    }
}
