using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings
{
    public class ZoneMapping : EntityTypeConfiguration<Zone>
    {
        public ZoneMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired().HasMaxLength(128);
        }
    }
}
