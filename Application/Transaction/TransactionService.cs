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
        private readonly IDocumentNumberGenerator _numberSequenceService;
        private readonly IRepository<TransactionDocument> _transactionRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Payment> _paymentRepository;

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

        public PersonTransactionsResponse GetAllByPersonId(int personId, int offset = 0, int count = 20)
        {
            var totalCount = _transactionRepository.Query()
                .Where(x => x.PersonId == personId && !x.DeleteDate.HasValue)
                .Count();

            var transactions = _transactionRepository.Query()
                .Where(x => x.PersonId == personId && !x.DeleteDate.HasValue)
                .OrderByDescending(x => x.Date).ThenByDescending(x => x.Id)
                .Skip(offset * count)
                .Take(count)
                .Select(x => new TransactionEntry
                {
                    Id = x.Id,
                    Date = x.Date,
                    PersonId = x.PersonId,
                    TransactionType = x is Invoice ? TransactionDocumentType.Invoice :
                                      x is Payment ? TransactionDocumentType.Payment :
                                      x is CreditNote ? TransactionDocumentType.CreditNote :
                                      TransactionDocumentType.DebitNote,
                    Number = x.Number,
                    Description = x.Description,
                    IsRefunded = x.IsRefunded,
                    RefundedDocument = x.ReferencedDocumentId.HasValue ? new TransactionEntryReference
                    {
                        Number = x.ReferencedDocument.Number,
                        Id = x.ReferencedDocument.Id,
                        TransactionType = x.ReferencedDocument is Invoice ? TransactionDocumentType.Invoice :
                                          x.ReferencedDocument is Payment ? TransactionDocumentType.Payment :
                                          x.ReferencedDocument is CreditNote ? TransactionDocumentType.CreditNote :
                                          TransactionDocumentType.DebitNote
                    } : null,
                    Total = x.Total,
                    Balance = x.Balance
                })
                .OrderBy(x => x.Date).OrderBy(x => x.Id)
                .ToList();

            return new PersonTransactionsResponse(totalCount, transactions);
        }

        public void Cancel(CancelTransaction request)
        {
            var document = GetById(request.Id);

            if (IsLastTransaction(document) && !document.IsReferencingDocument)
                DeleteDocument(document);
            else
                CancelDocument(document, request);
        }

        private TransactionDocument GetById(int id)
        {
            var document = _transactionRepository.Get(id, x => x.Person);
            if (document == null)
                throw new NotFoundException();
            return document;
        }

        private bool IsLastTransaction(TransactionDocument document)
        {
            var lastId = _transactionRepository.Query()
                .Where(x => x.PersonId == document.PersonId && !x.DeleteDate.HasValue)
                .OrderByDescending(x => x.Date).ThenByDescending(x => x.Id)
                .Select(x => x.Id)
                .FirstOrDefault();

            return lastId == 0 || lastId == document.Id;
        }

        private void DeleteDocument(TransactionDocument document)
        {
            var person = document.Person;
            person.Cancel(document);
        }

        private void CancelDocument(TransactionDocument document, CancelTransaction request)
        {
            var person = document.Person;
            var cancelDocument = document.Void(_numberSequenceService);
            cancelDocument.Description = request.Description;

            person.Accept(cancelDocument);

            _transactionRepository.Add(cancelDocument);
        }
    }
}
