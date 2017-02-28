using FriMav.Domain;
using System.Data.Entity;

namespace FriMav.Infrastructure.Mappings
{
    class InvoiceMapping : BaseMapping<Invoice>
    {
        public override void Mapping()
        {
            this.ToTable("Invoice");
            this.HasKey(i => i.TransactionId);
            this.Property(i => i.DeliveryAddress).HasMaxLength(192);
            this.HasRequired(i => i.Person).WithMany().HasForeignKey(x => x.PersonId).WillCascadeOnDelete(false);
            this.HasMany(i => i.Items).WithRequired().HasForeignKey(it => it.TransactionId).WillCascadeOnDelete(true);
            this.HasOptional(i => i.Delivery).WithMany(d => d.Invoices).HasForeignKey(i => i.DeliveryId).WillCascadeOnDelete(false);
        }
    }
}
