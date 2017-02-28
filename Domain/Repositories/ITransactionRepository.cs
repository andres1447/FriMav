using FriMav.Domain.Proyections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Repositories
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {
        Transaction GetWithReference(Expression<Func<Transaction, bool>> predicate);
        IEnumerable<UnpaidTransaction> GetUnpaidTransactionsByPersonId(int personId);
        IEnumerable<SurplusTransaction> GetTransactionsWithSurplusByPersonId(int personId);
        IEnumerable<TransactionEntry> FindAllWithReferenceByPersonId(int personId);
        bool IsReferenced(int transactionId);
    }
}
