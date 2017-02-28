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
    class NumberSequenceMapping : BaseMapping<NumberSequence>
    {
        public override void Mapping()
        {
            this.ToTable("NumberSequence");
            this.HasKey(p => p.Key);
            this.Property(p => p.CurrentId).IsRequired();
        }
    }
}
