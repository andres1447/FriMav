using FriMav.Domain;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;

namespace FriMav.Infrastructure
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext() : base("name=BillingEntities")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public string ConnectionString
        {
            get
            {
                return Database.Connection.ConnectionString;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        IQueryable<TEntity> IDatabaseContext.Set<TEntity>()
        {
            return base.Set<TEntity>().AsQueryable();
        }

        public void Attach<TEntity>(TEntity entity) where TEntity : class
        {
            base.Set<TEntity>().Attach(entity);
        }

        public void Modify<TEntity>(TEntity entity) where TEntity : class
        {
            base.Entry(entity).State = EntityState.Modified;
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            base.Entry(entity).State = EntityState.Added;
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            base.Entry(entity).State = EntityState.Deleted;
        }

        public void DetectChanges()
        {
            base.ChangeTracker.DetectChanges();
        }
    }
}
