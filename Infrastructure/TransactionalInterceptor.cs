using Castle.Core.Internal;
using Castle.DynamicProxy;
using FriMav.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace FriMav.Infrastructure
{
    public class TransactionalInterceptor : IInterceptor
    {
        private readonly DbContext _db;

        public TransactionalInterceptor(DbContext db)
        {
            _db = db;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.CustomAttributes.Any(t => t.AttributeType == typeof(TransactionalAttribute)))
            {
                using (var transaction = _db.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        invocation.Proceed();
                        _db.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            else
            {
                _db.Configuration.AutoDetectChangesEnabled = false;
                invocation.Proceed();
                _db.Configuration.AutoDetectChangesEnabled = true;
            }
        }
    }
}
