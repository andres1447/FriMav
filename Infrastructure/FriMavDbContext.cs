using FriMav.Domain.Entities;
using FriMav.Infrastructure.Mappings;
using FriMav.Infrastructure.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure
{
    public class FriMavDbContext : DbContext
    {
        public FriMavDbContext() : base("FriMavDb")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<FriMavDbContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FriMavDbContext, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            var mappings = GetType().Assembly.GetTypes().Where(x => x.Name.EndsWith("Mapping")).ToList();
            foreach (var mapping in mappings)
            {
                modelBuilder.Configurations.Add((dynamic)Activator.CreateInstance(mapping));
            }
        }
    }
}
