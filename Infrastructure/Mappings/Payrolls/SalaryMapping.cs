using FriMav.Domain.Entities.Payrolls;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings.Payrolls
{
    public class SalaryMapping : EntityTypeConfiguration<Salary>
    {
        public SalaryMapping()
        {
            HasKey(x => x.Id);
        }
    }
}
