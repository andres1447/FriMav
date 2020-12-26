using FriMav.Domain;
using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure
{
    public class EntityRepository<T> : IRepository<T> where T : Entity
    {
        DbContext _db;

        public EntityRepository(DbContext db)
        {
            _db = db;
        }

        public void Add(T element)
        {
            _db.Set<T>().Add(element);
        }

        public T Get(int id)
        {
            return _db.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public T Get(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = _db.Set<T>().AsQueryable();
            foreach (var include in includes)
                query = query.Include(include);
            return query.FirstOrDefault(x => x.Id == id);
        }

        public List<T> GetAll()
        {
            return _db.Set<T>().ToList();
        }

        public List<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            return Query(includes).ToList();
        }

        public IQueryable<T> Query(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _db.Set<T>();
            if (includes.Length > 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            return query;
        }

        public void Delete(T element)
        {
            _db.Set<T>().Remove(element);
        }

        public bool Exists(int id)
        {
            return _db.Set<T>().Any(x => x.Id == id);
        }

        public bool Exists(Expression<Func<T, bool>> clause)
        {
            return _db.Set<T>().Any(clause);
        }
    }
}
