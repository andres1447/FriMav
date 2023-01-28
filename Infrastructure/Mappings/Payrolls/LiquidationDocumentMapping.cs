using FriMav.Domain.Entities.Payrolls;
using System.Data.Entity.ModelConfiguration;

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
            Map<AttendBonus>(m => m.Requires("Type").HasValue((int)LiquidationDocumentType.AttendBonus));

            HasRequired(x => x.Employee).WithMany().HasForeignKey(x => x.EmployeeId);

            HasIndex(x => x.Date).IsUnique(false);
        }
    }
}
