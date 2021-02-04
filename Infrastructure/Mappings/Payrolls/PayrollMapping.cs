using FriMav.Domain.Entities.Payrolls;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings.Payrolls
{
    public class PayrollMapping : EntityTypeConfiguration<Payroll>
    {
        public PayrollMapping()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.Employee).WithMany().HasForeignKey(x => x.EmployeeId);
            HasMany(x => x.Liquidation).WithMany().Map(m =>
            {
                m.MapLeftKey("PayrollId");
                m.MapRightKey("LiquidationDocumentId");
            });
        }
    }
}
