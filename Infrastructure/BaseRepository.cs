using FriMav.Domain;
using FriMav.Domain.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace FriMav.Infrastructure
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected IDatabaseContext _databaseContext { get; set; }

        public BaseRepository(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _databaseContext.Set<T>().ToList();
        }

        public virtual IEnumerable<T> FindAllBy(Expression<Func<T, bool>> predicate)
        {
            return _databaseContext.Set<T>().Where(predicate).ToList();
        }

        public virtual IEnumerable<T> FindAllBy<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, bool desc = false)
        {
            var query = _databaseContext.Set<T>().Where(predicate);
            if (desc)
            {
                return query.OrderByDescending(order).ToList();
            }
            return query.OrderBy(order).ToList();
        }

        public virtual T FindBy(Expression<Func<T, bool>> predicate)
        {
            return _databaseContext.Set<T>().FirstOrDefault(predicate);
        }

        public virtual void Create(T entity)
        {
            _databaseContext.Add(entity);
        }

        public virtual void Update(T entity)
        {
            _databaseContext.Modify(entity);
        }

        public virtual void Delete(T entity)
        {
            _databaseContext.Delete(entity);
        }

        public IEnumerable<string> Save()
        {
            try
            {
                _databaseContext.SaveChanges();
                return new List<string>();
            }
            catch (DbEntityValidationException ex)
            {
                return ex.EntityValidationErrors
                            .SelectMany(entry => entry.ValidationErrors
                            .Select(e => e.ErrorMessage));
            }
        }

        public void Attach(T entity)
        {
            _databaseContext.Attach(entity);
        }

        public void DetectChanges()
        {
            _databaseContext.DetectChanges();
        }

        public int Count()
        {
            return _databaseContext.Set<T>().Count();
        }
    }
}
