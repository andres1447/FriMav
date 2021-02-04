using FriMav.Domain;
using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application.Test
{
    public class MemoryRepository<T> : IRepository<T> where T : Entity
    {
        private List<T> _list = new List<T>();

        public void Add(T element)
        {
            _list.Add(element);
        }

        public void Delete(T element)
        {
            _list.Remove(element);
        }

        public bool Exists(int id)
        {
            return _list.Any(x => x.Id == id);
        }

        public bool Exists(Expression<Func<T, bool>> clause)
        {
            return _list.Any(clause.Compile());
        }

        public T Get(int id)
        {
            return _list.FirstOrDefault(x => x.Id == id);
        }

        public T Get(int id, params Expression<Func<T, object>>[] includes)
        {
            return Get(id);
        }

        public List<T> GetAll()
        {
            return _list.ToList();
        }

        public List<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            return _list.ToList();
        }

        public IQueryable<T> Query(params Expression<Func<T, object>>[] includes)
        {
            return _list.ToList().AsQueryable();
        }
    }
}
