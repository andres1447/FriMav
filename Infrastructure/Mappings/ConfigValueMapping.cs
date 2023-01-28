using FriMav.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FriMav.Infrastructure.Mappings
{
    public class ConfigValueMapping : EntityTypeConfiguration<ConfigValue>
    {
        public ConfigValueMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Code).IsRequired();
        }
    }
}
