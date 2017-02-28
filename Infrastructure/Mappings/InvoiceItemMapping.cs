using FriMav.Domain;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace FriMav.Infrastructure.Mappings
{
    class InvoiceItemMapping : BaseMapping<InvoiceItem>
    {
        public override void Mapping()
        {
            this.ToTable("InvoiceItem");
            this.HasKey(it => it.InvoiceItemId);
            this.HasRequired(it => it.Product).WithMany().HasForeignKey(x => x.ProductId).WillCascadeOnDelete(false);
        }
    }
}
