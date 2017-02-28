using System.Linq;

namespace FriMav.Infrastructure
{
    public interface IDatabaseContext
    {
        string ConnectionString { get; }
        int SaveChanges();
        void DetectChanges();
        IQueryable<TEntity> Set<TEntity>() where TEntity : class;
        void Attach<TEntity>(TEntity entity) where TEntity : class;
        void Modify<TEntity>(TEntity entity) where TEntity : class;
        void Add<TEntity>(TEntity entity) where TEntity : class;
        void Delete<TEntity>(TEntity entity) where TEntity : class;
    }
}
