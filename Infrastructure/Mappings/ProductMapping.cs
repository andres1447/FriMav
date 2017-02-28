using FriMav.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;

namespace FriMav.Infrastructure.Mappings
{
    class ProductMapping : BaseMapping<Product>
    {
        public override void Mapping()
        {
            this.ToTable("Product");
            this.HasKey(p => p.ProductId);

            this.Property(p => p.Code)
                .HasMaxLength(10)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("idx_product_code", 1) { IsUnique = true }));

            this.Property(p => p.Name).HasMaxLength(196).IsRequired();
            this.Property(p => p.Price).IsRequired();
            this.HasOptional(p => p.Family).WithMany().HasForeignKey(p => p.FamilyId).WillCascadeOnDelete(false);
        }
    }
}
