using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain
{
    public interface IRepository<T> where T : Entity
    {
        bool Exists(int id);
        bool Exists(Expression<Func<T, bool>> clause);
        void Add(T element);
        T Get(int id);
        T Get(int id, params Expression<Func<T, object>>[] includes);
        List<T> GetAll();
        List<T> GetAll(params Expression<Func<T, object>>[] includes);
        IQueryable<T> Query(params Expression<Func<T, object>>[] includes);
        void Delete(T element);
    }
}
