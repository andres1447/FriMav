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
    public interface IDbContext
    {
        DbSet<T> Set<T>() where T : class;
        int SaveChanges();
    }

    public class FriMavDbContext : DbContext, IDbContext
    {
        public FriMavDbContext() : base("FriMavDb")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FriMavDbContext, Configuration>());
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> Items { get; set; }
        public DbSet<TransactionDocument> TransactionDocuments { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<DebitNote> DebitNotes { get; set; }
        public DbSet<CreditNote> CreditNotes { get; set; }

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
