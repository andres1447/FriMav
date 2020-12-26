using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings
{
    public class InvoiceMapping : EntityTypeConfiguration<Invoice>
    {
        public InvoiceMapping()
        {
            HasMany(x => x.Items).WithRequired().HasForeignKey(x => x.InvoiceId).WillCascadeOnDelete(true);
        }
    }
}
