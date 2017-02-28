using FriMav.Domain;
using System.Data.Entity;

namespace FriMav.Infrastructure.Mappings
{
    class DeliveryMapping : BaseMapping<Delivery>
    {
        public override void Mapping()
        {
            this.ToTable("Delivery");
            this.HasKey(i => i.DeliveryId);
            this.Property(i => i.Date).IsRequired();
            this.HasMany(i => i.Invoices).WithOptional(x => x.Delivery).HasForeignKey(it => it.DeliveryId).WillCascadeOnDelete(false);
            this.HasRequired(i => i.Employee).WithMany().HasForeignKey(x => x.PersonId).WillCascadeOnDelete(false);
        }
    }
}
