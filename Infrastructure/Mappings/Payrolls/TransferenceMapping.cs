using FriMav.Domain.Entities.Payrolls;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings.Payrolls
{
    public class TransferenceMapping : EntityTypeConfiguration<Transference>
    {
        public TransferenceMapping()
        {
            HasKey(x => x.Id);
        }
    }
}
