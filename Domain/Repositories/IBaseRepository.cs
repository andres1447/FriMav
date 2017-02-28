using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        int Count();
        void Attach(T entity);
        IEnumerable<T> GetAll();
        T FindBy(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindAllBy(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindAllBy<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, bool desc = false);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<string> Save();
        void DetectChanges();
    }
}
