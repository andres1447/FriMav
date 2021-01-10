using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings
{
    public class DeliveryMapping : EntityTypeConfiguration<Delivery>
    {
        public DeliveryMapping()
        {
            HasKey(x => x.Id);
            HasRequired(x => x.Employee).WithMany().HasForeignKey(x => x.EmployeeId);
            HasMany(x => x.Invoices).WithMany().Map(m =>
            {
                m.MapLeftKey("DeliveryId");
                m.MapRightKey("InvoiceId");
            });
            HasMany(x => x.Payments).WithMany().Map(m =>
            {
                m.MapLeftKey("DeliveryId");
                m.MapRightKey("PaymentId");
            });
        }
    }
}
