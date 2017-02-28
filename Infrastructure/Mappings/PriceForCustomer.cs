using FriMav.Domain;

namespace FriMav.Infrastructure.Mappings
{
    class PriceForCustomerMapping : BaseMapping<PriceForCustomer>
    {
        public override void Mapping()
        {
            this.ToTable("PriceForCustomer");
            this.HasKey(it => new { it.PersonId, it.ProductId });
            this.HasRequired(it => it.Customer).WithMany().HasForeignKey(x => x.PersonId).WillCascadeOnDelete(false);
            this.HasRequired(it => it.Product).WithMany().HasForeignKey(x => x.ProductId).WillCascadeOnDelete(false);
        }
    }
}
