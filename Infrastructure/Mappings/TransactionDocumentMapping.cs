using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings
{
    public class TransactionDocumentMapping : EntityTypeConfiguration<TransactionDocument>
    {
        public TransactionDocumentMapping()
        {
            HasKey(x => x.Id);

            Map<Invoice>(m => m.Requires("Type").HasValue((int)TransactionDocumentType.Invoice));
            Map<Payment>(m => m.Requires("Type").HasValue((int)TransactionDocumentType.Payment));
            Map<CreditNote>(m => m.Requires("Type").HasValue((int)TransactionDocumentType.CreditNote));
            Map<DebitNote>(m => m.Requires("Type").HasValue((int)TransactionDocumentType.DebitNote));

            HasRequired(x => x.Person).WithMany().HasForeignKey(x => x.PersonId);
            HasOptional(x => x.ReferencedDocument).WithMany().HasForeignKey(x => x.ReferencedDocumentId);

            HasIndex(x => x.Date).IsUnique(false);
            HasIndex(x => x.Number).IsUnique(false);
        }
    }
}
