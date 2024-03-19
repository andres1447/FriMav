using FriMav.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FriMav.Infrastructure.Mappings
{
    public class DailyBillingMapping : EntityTypeConfiguration<DailyBilling>
    {
        public DailyBillingMapping()
        {
            HasKey(x => x.Id);
        }
    }
}
