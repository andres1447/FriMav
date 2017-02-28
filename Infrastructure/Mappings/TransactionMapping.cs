using FriMav.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings
{
    public class TransactionMapping : BaseMapping<Transaction>
    {
        public override void Mapping()
        {
            ToTable("Transaction");
            HasKey(t => t.TransactionId);
            HasRequired(t => t.Person).WithMany().HasForeignKey(t => t.PersonId).WillCascadeOnDelete(false);
            Property(t => t.Date).IsRequired();
            Property(t => t.TransactionType).IsRequired();
            Property(t => t.Description).HasMaxLength(255);
            //HasOptional(t => t.Invoice).WithMany().HasForeignKey(t => t.InvoiceId).WillCascadeOnDelete(true);
            HasOptional(t => t.Reference).WithMany().HasForeignKey(t => t.ReferenceId).WillCascadeOnDelete(false);
        }
    }
}
