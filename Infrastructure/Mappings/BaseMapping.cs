using FriMav.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings
{
    public abstract class BaseMapping<T> : EntityTypeConfiguration<T> where T : class
    {
        public BaseMapping()
        {
            this.Mapping();
        }

        public abstract void Mapping();
    }
}
