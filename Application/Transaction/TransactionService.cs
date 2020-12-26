using System;
using System.Collections.Generic;
using FriMav.Domain.Entities;
using FriMav.Domain;
using System.Linq;
using FriMav.Domain.Proyections;

namespace FriMav.Application
{
    public class TransactionService : ITransactionService
    {
        private IDocumentNumberGenerator _numberSequenceService;
        private IRepository<TransactionDocument> _transactionRepository;
        private IRepository<Customer> _customerRepository;
        private IRepository<Payment> _paymentRepository;

        public TransactionService(
            IDocumentNumberGenerator numberSequenceService,
            IRepository<TransactionDocument> transactionRepository,
            IRepository<Customer> customerRepository,
            IRepository<Payment> paymentRepository)
        {
            _numberSequenceService = numberSequenceService;
            _transactionRepository = transactionRepository;
            _customerRepository = customerRepository;
            _paymentRepository = paymentRepository;
        }

        public IEnumerable<TransactionDocument> GetAll()
        {
            return _transactionRepository.GetAll();
        }

        public TransactionDocument Get(int id)
        {
            return _transactionRepository.Get(id);
        }

        public void Create(CreatePayment request)
        {
            var customer = _customerRepository.Get(request.PersonId);
            if (customer == null)
                throw new NotFoundException();

            var number = _numberSequenceService.NextForPayment();
            var payment = new Payment()
            {
                Total = -request.Total,
                PersonId = request.PersonId,
                Person = customer,
                Description = request.Description,
                Number = number
            };
            customer.Accept(payment);
            _paymentRepository.Add(payment);
        }

        public IEnumerable<TransactionEntry> GetAllByPersonId(int personId, int offset = 0, int count = 20)
        {
            return _transactionRepository.Query()
                .Where(x => x.PersonId == personId)
                .OrderByDescending(x => x.Date)
                .Skip(offset * count)
                .Take(count)
                .Select(x => new TransactionEntry
                {
                    Id = x.Id,
                    Date = x.Date,
                    PersonId = x.PersonId,
                    TransactionType = x is Invoice ? TransactionDocumentType.Invoice :
                                      x is Payment ? TransactionDocumentType.Payment :
                                      x is DebitNote ? TransactionDocumentType.DebitNote :
                                      TransactionDocumentType.CreditNote,
                    Number = x.Number,
                    Description = x.Description,
                    RefundDocument = x.IsRefunded ? new TransactionEntryReference
                    {
                        Number = x.RefundDocument.Number,
                        Id = x.RefundDocument.Id,
                        TransactionType = x.RefundDocument is Invoice ? TransactionDocumentType.Invoice :
                                          x.RefundDocument is Payment ? TransactionDocumentType.Payment :
                                          x.RefundDocument is DebitNote ? TransactionDocumentType.DebitNote :
                                          TransactionDocumentType.CreditNote
                    } : null,
                    Total = x.Total,
                    Balance = x.Balance
                })
                .OrderBy(x => x.Date)
                .ToList();
        }
    }
}
