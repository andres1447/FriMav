using FriMav.Domain;
using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public static class RepositoryExtensions
    {
        public static T GetById<T>(this IRepository<T> repository, int id) where T : Entity
        {
            var employee = repository.Get(id);
            if (employee == null)
                throw new NotFoundException();

            return employee;
        }

        public static T GetById<T>(this IRepository<T> repository, int id, params Expression<Func<T, object>>[] includes) where T : Entity
        {
            var employee = repository.Get(id, includes);
            if (employee == null)
                throw new NotFoundException();

            return employee;
        }
    }
}
