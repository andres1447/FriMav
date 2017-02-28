using System;
using System.Collections.Generic;
using System.Linq;
using FriMav.Domain;
using FriMav.Domain.Repositories;
using FriMav.Domain.Proyections;

namespace FriMav.Application
{
    public class TransactionService : ITransactionService
    {
        private INumberSequenceService _numberSequenceService;
        private ITransactionRepository _transactionRepository;
        private ICustomerRepository _customerRepository;
        private ITransactionPaymentRepository _transactionPaymentRepository;

        public TransactionService(
            INumberSequenceService numberSequenceService,
            ITransactionRepository transactionRepository,
            ITransactionPaymentRepository transactionPaymentRepository,
            ICustomerRepository customerRepository)
        {
            _numberSequenceService = numberSequenceService;
            _transactionRepository = transactionRepository;
            _transactionPaymentRepository = transactionPaymentRepository;
            _customerRepository = customerRepository;
        }

        public IEnumerable<Transaction> GetAll()
        {
            return _transactionRepository.GetAll();
        }

        public Transaction Get(int transactionId)
        {
            return _transactionRepository.GetWithReference(x => x.TransactionId == transactionId);
        }

        public void Create(CreatePayment payment)
        {
            Create(new Transaction()
            {
                Date = payment.Date,
                Total = -payment.Total,
                TransactionType = TransactionType.Payment,
                PersonId = payment.PersonId,
                Description = payment.Description
            });
        }

        public void CreateWithoutSaving(Transaction transaction, bool managePayments)
        {
            transaction.Number = _numberSequenceService.NextOrInit(transaction.TransactionType.ToString());
            transaction.FiscalYear = transaction.Date.Year;
            _transactionRepository.Create(transaction);
            AdjustCustomerBalance(transaction);
            if (managePayments)
            {
                ManageTransactionPayments(transaction);
            }
        }

        public void Create(Transaction transaction)
        {
            using (var scope = new System.Transactions.TransactionScope())
            {
                CreateWithoutSaving(transaction, true);
                _transactionRepository.Save();
                scope.Complete();
            }
        }

        public IEnumerable<TransactionEntry> FindAllWithReferenceByPersonId(int personId)
        {
            return _transactionRepository.FindAllWithReferenceByPersonId(personId);
        }

        public void Cancel(CancelTransaction cancelation)
        {
            using (var scope = new System.Transactions.TransactionScope())
            {
                var tran = _transactionRepository.FindBy(x => x.TransactionId == cancelation.TransactionId);
                CreateWithoutSaving(new Transaction()
                {
                    Date = DateTime.UtcNow,
                    Description = cancelation.Description,
                    Total = -tran.Total,
                    TransactionType = GetInverseTransactionType(tran.TransactionType),
                    PersonId = tran.PersonId,
                    ReferenceId = tran.TransactionId
                }, true);
                tran.IsRefunded = true;
                _transactionRepository.Update(tran);
                _transactionRepository.Save();
                scope.Complete();
            }
        }

        public bool IsReferenced(int transactionId)
        {
            return _transactionRepository.IsReferenced(transactionId);
        }

        #region Private

        protected TransactionType GetInverseTransactionType(TransactionType type)
        {
            switch (type)
            {
                case TransactionType.Invoice: return TransactionType.CreditNote;
                case TransactionType.Payment: return TransactionType.DebitNote;
                case TransactionType.CreditNote: return TransactionType.DebitNote;
                case TransactionType.DebitNote: return TransactionType.CreditNote;
                default: return TransactionType.Adjustment;
            }
        }

        protected void ManageTransactionPayments(Transaction transaction)
        {
            if (transaction.Total > 0)
            {
                PayUnpaidTransactions(transaction);
            }
            else if (transaction.Total < 0)
            {
                PayTransactionWithSurplus(transaction);
            }
        }

        protected void PayUnpaidTransactions(Transaction transaction)
        {
            decimal amount = transaction.Total;
            decimal remaining;
            foreach (var unpaidTransaction in _transactionRepository.GetUnpaidTransactionsByPersonId(transaction.PersonId))
            {
                remaining = unpaidTransaction.Remaining();
                if (amount > remaining)
                {
                    amount -= remaining;
                    _transactionPaymentRepository.Create(new TransactionPayment
                    {
                        Amount = remaining,
                        TargetTransactionId = unpaidTransaction.TransactionId,
                        SourceTransaction = transaction
                    });
                }
                else
                {
                    _transactionPaymentRepository.Create(new TransactionPayment
                    {
                        Amount = amount,
                        TargetTransactionId = unpaidTransaction.TransactionId,
                        SourceTransaction = transaction
                    });
                    break;
                }
            }
        }

        protected void PayTransactionWithSurplus(Transaction transaction)
        {
            decimal amount = -transaction.Total;
            decimal surplus;
            foreach (var surplusTransaction in _transactionRepository.GetTransactionsWithSurplusByPersonId(transaction.PersonId))
            {
                surplus = surplusTransaction.Surplus();
                if (amount > surplus)
                {
                    amount -= surplus;
                    transaction.Payments.Add(new TransactionPayment
                    {
                        Amount = surplus,
                        SourceTransactionId = surplusTransaction.TransactionId
                    });
                }
                else
                {
                    transaction.Payments.Add(new TransactionPayment
                    {
                        Amount = amount,
                        SourceTransactionId = surplusTransaction.TransactionId
                    });
                    break;
                }
            }
        }

        protected void AdjustCustomerBalance(Transaction transaction)
        {
            Customer customer = _customerRepository.FindBy(x => x.PersonId == transaction.PersonId);
            customer.Balance += transaction.Total;
            transaction.Balance = customer.Balance;
            _customerRepository.Update(customer);
        }

        #endregion
    }
}
