using FriMav.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;

namespace FriMav.Infrastructure.Mappings
{
    class CustomerMapping : BaseMapping<Customer>
    {
        public override void Mapping()
        {
            this.ToTable("Customer");
            this.HasKey(c => c.PersonId);

            this.Property(c => c.Code)
                .HasMaxLength(10)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("idx_customer_code", 1) { IsUnique = true }));
        }
    }
}
