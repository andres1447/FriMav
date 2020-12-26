using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings
{
    public class PersonMapping : EntityTypeConfiguration<Person>
    {
        public PersonMapping()
        {
            HasKey(x => x.Id);
            Map<Customer>(m => m.Requires("Type").HasValue((int)PersonType.Customer));
            Map<Employee>(m => m.Requires("Type").HasValue((int)PersonType.Employee));

            Property(x => x.Code).IsRequired().HasMaxLength(10);
            Property(x => x.Name).IsRequired();
            HasOptional(x => x.Zone).WithMany().HasForeignKey(x => x.ZoneId);
        }
    }
}
