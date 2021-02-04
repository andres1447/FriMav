using FriMav.Domain.Entities;
using FriMav.Domain.Entities.Payrolls;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings.Payrolls
{
    public class LiquidationDocumentMapping : EntityTypeConfiguration<LiquidationDocument>
    {
        public LiquidationDocumentMapping()
        {
            HasKey(x => x.Id);

            Map<Salary>(m => m.Requires("Type").HasValue((int)LiquidationDocumentType.Salary));
            Map<Advance>(m => m.Requires("Type").HasValue((int)LiquidationDocumentType.Advance));
            Map<Absency>(m => m.Requires("Type").HasValue((int)LiquidationDocumentType.Absency));
            Map<GoodsSold>(m => m.Requires("Type").HasValue((int)LiquidationDocumentType.GoodsSold));
            Map<LoanFee>(m => m.Requires("Type").HasValue((int)LiquidationDocumentType.LoanFee));

            HasRequired(x => x.Employee).WithMany().HasForeignKey(x => x.EmployeeId);

            HasIndex(x => x.Date).IsUnique(false);
        }
    }
}
