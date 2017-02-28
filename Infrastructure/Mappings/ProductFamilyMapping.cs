using FriMav.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace FriMav.Infrastructure.Mappings
{
    class ProductFamilyMapping : BaseMapping<ProductFamily>
    {
        public override void Mapping()
        {
            this.ToTable("ProductFamily");
            this.HasKey(pf => pf.FamilyId);
            this.Property(pf => pf.Name).HasMaxLength(196).IsRequired();
        }
    }
}
