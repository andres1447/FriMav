using FriMav.Domain.Entities.Payrolls;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings.Payrolls
{
    public class LoanMapping : EntityTypeConfiguration<Loan>
    {
        public LoanMapping()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.Employee).WithMany().HasForeignKey(x => x.EmployeeId).WillCascadeOnDelete(false);
            HasMany(x => x.Fees).WithRequired().HasForeignKey(x => x.LoanId).WillCascadeOnDelete(false);
        }
    }
}
