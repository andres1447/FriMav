using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings
{
    public class NumberSequenceMapping : EntityTypeConfiguration<NumberSequence>
    {
        public NumberSequenceMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Key).IsRequired();
        }
    }
}
