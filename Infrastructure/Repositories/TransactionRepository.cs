using FriMav.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using FriMav.Domain.Repositories;
using System.Data.SqlClient;
using Dapper;
using FriMav.Domain.Proyections;
using System;
using System.Linq.Expressions;

namespace FriMav.Infrastructure.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public IEnumerable<TransactionEntry> FindAllWithReferenceByPersonId(int personId)
        {
            var query = from t in _databaseContext.Set<Transaction>()
                        join tr in _databaseContext.Set<Transaction>() on t.ReferenceId equals tr.TransactionId into reference
                        from tre in reference.DefaultIfEmpty()
                        orderby t.Date descending
                        select new TransactionEntry
                        {
                            TransactionId = t.TransactionId,
                            Date = t.Date,
                            TransactionType = t.TransactionType,
                            Description = t.Description,
                            PersonId = t.PersonId,
                            Number = t.Number,
                            FiscalYear = t.FiscalYear,
                            Balance = t.Balance,
                            ReferenceId = t.ReferenceId,
                            Total = t.Total,
                            Reference = tre == null ? null : new TransactionEntryReference
                            {
                                TransactionId = tre.TransactionId,
                                TransactionType = tre.TransactionType,
                                Number = tre.Number
                            }
                        };
            return query.OrderByDescending(x => x.Date).ToList();
        }

        public IEnumerable<UnpaidTransaction> GetUnpaidTransactionsByPersonId(int personId)
        {
            var query = (from t in _databaseContext.Set<Transaction>()
                         join tp in _databaseContext.Set<TransactionPayment>() on t.TransactionId equals tp.TargetTransactionId into payments
                         where t.Total < 0 && payments.Select(x => x.Amount).DefaultIfEmpty(0).Sum() < -t.Total && t.PersonId == personId
                         select new UnpaidTransaction
                         {
                             TransactionId = t.TransactionId,
                             Date = t.Date,
                             Total = -t.Total,
                             Paid = payments.Select(x => x.Amount).DefaultIfEmpty(0).Sum()
                         });
            return query.ToList();
        }

        public IEnumerable<SurplusTransaction> GetTransactionsWithSurplusByPersonId(int personId)
        {
            return (from t in _databaseContext.Set<Transaction>()
                    join tp in _databaseContext.Set<TransactionPayment>() on t.TransactionId equals tp.SourceTransactionId into payments
                    where t.Total > 0 && payments.Select(x => x.Amount).DefaultIfEmpty(0).Sum() < t.Total && t.PersonId == personId
                    select new SurplusTransaction
                    {
                        TransactionId = t.TransactionId,
                        Date = t.Date,
                        Total = t.Total,
                        Paid = payments.Select(x => x.Amount).DefaultIfEmpty(0).Sum()
                    }).ToList();
        }

        public Transaction GetWithReference(Expression<Func<Transaction, bool>> predicate)
        {
            return _databaseContext.Set<Transaction>()
                .Include(x => x.Reference)
                .FirstOrDefault(predicate);
        }

        public bool IsReferenced(int transactionId)
        {
            return _databaseContext.Set<Transaction>().Any(x => x.ReferenceId == transactionId);
        }
    }
}
