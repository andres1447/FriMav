using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings
{
    public class InvoiceItemMapping : EntityTypeConfiguration<InvoiceItem>
    {
        public InvoiceItemMapping()
        {
            HasKey(x => new { x.Id, x.InvoiceId });
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasRequired(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
        }
    }
}
