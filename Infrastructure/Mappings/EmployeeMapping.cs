using FriMav.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;

namespace FriMav.Infrastructure.Mappings
{
    class EmployeeMapping : BaseMapping<Employee>
    {
        public override void Mapping()
        {
            this.ToTable("Employee");
            this.HasKey(e => e.PersonId);

            this.Property(e => e.Code)
                .HasMaxLength(10)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("idx_employee_code", 1) { IsUnique = true }));

            this.HasMany(e => e.Salaries).WithRequired(s => s.Employee).HasForeignKey(s => s.EmployeeId).WillCascadeOnDelete(true);
        }
    }
}
