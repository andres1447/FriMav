using FriMav.Domain.Entities.Payrolls;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings.Payrolls
{
    public class GoodsSoldMapping : EntityTypeConfiguration<GoodsSold>
    {
        public GoodsSoldMapping()
        {
            HasKey(x => x.Id);

            HasMany(x => x.Items).WithRequired().HasForeignKey(x => x.GoodsSoldId).WillCascadeOnDelete(true);
        }
    }
}
