using FriMav.Domain;
using System.Data.Entity;

namespace FriMav.Infrastructure.Mappings
{
    class SalaryMapping : BaseMapping<Salary>
    {
        public override void Mapping()
        {
            this.ToTable("Salary");
            this.HasKey(it => it.SalaryId);
            this.HasRequired(it => it.Employee).WithMany().HasForeignKey(x => x.EmployeeId).WillCascadeOnDelete(true);
        }
    }
}
