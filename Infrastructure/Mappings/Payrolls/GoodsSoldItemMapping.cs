using FriMav.Domain.Entities.Payrolls;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings.Payrolls
{
    public class GoodsSoldItemMapping : EntityTypeConfiguration<GoodsSoldItem>
    {
        public GoodsSoldItemMapping()
        {
            HasKey(x => new { x.Id, x.GoodsSoldId });
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
