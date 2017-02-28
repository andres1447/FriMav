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
    class PersonMapping : BaseMapping<Person>
    {
        public override void Mapping()
        {
            this.ToTable("Person");
            this.HasKey(p => p.PersonId);
            this.Property(p => p.Name).IsRequired().HasMaxLength(256);
            this.Property(p => p.Cuit).HasMaxLength(11);
            this.Property(p => p.Address).HasMaxLength(192);
        }
    }
}
