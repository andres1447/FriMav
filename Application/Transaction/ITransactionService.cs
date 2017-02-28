using FriMav.Domain;
using FriMav.Domain.Proyections;
using System.Collections.Generic;

namespace FriMav.Application
{
    public interface ITransactionService
    {
        void Create(CreatePayment payment);
        void Create(Transaction transaction);
        void CreateWithoutSaving(Transaction transaction, bool managePayments);
        IEnumerable<Transaction> GetAll();
        Transaction Get(int transactionId);
        IEnumerable<TransactionEntry> FindAllWithReferenceByPersonId(int personId);
        void Cancel(CancelTransaction cancelation);
        bool IsReferenced(int transactionId);
    }
}
