using FriMav.Domain;
using FriMav.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FriMav.Application.Test
{
    public abstract class ApplicationTest
    {
        protected IDocumentNumberGenerator _documentNumberGenerator;
        protected MemoryRepository<TransactionDocument> _transactionRepository;
        protected MemoryRepository<Customer> _customerRepository;
        protected MemoryRepository<Payment> _paymentRepository;
        protected MemoryRepository<DailyBilling> _billingRepository;
        protected MemoryRepository<Invoice> _invoiceRepository;
        protected MemoryRepository<Product> _productRepository;
        protected MemoryRepository<CustomerPrice> _customerPriceRepository;
        private TransactionDocument invoice;

        [TestInitialize]
        public void Init()
        {
            _documentNumberGenerator = Substitute.For<IDocumentNumberGenerator>();
            _transactionRepository = new MemoryRepository<TransactionDocument>();
            _customerRepository = new MemoryRepository<Customer>();
            _paymentRepository = new MemoryRepository<Payment>();
            _billingRepository = new MemoryRepository<DailyBilling>();
            _invoiceRepository = new MemoryRepository<Invoice>();
            _productRepository = new MemoryRepository<Product>();
            _customerPriceRepository = new MemoryRepository<CustomerPrice>();
            Setup();
        }

        protected abstract void Setup();

        protected void GivenCustomer(int id, string name, decimal balance = 0)
        {
            _customerRepository.Add(new Customer
            {
                Id = id,
                Name = name,
                CustomerPrices = new List<CustomerPrice>(),
                Balance = balance
            });
        }

        protected void GivenProduct(int id, string code)
        {
            _productRepository.Add(new Product
            {
                Id = id,
                Code = code
            });
        }

        protected void GivenInvoice(int id, int customerId, params InvoiceItem[] items)
        {
            var invoice = new Invoice
            {
                Id = id,
                PersonId = customerId,
                Person = _customerRepository.Get(customerId),
                CreationDate = DateTime.UtcNow,
                Date = DateTime.UtcNow,
                Items = items.ToList(),
                Total = items.Select(x => x.Quantity * x.Price).DefaultIfEmpty(0).Sum()
            };
            _invoiceRepository.Add(invoice);
            _transactionRepository.Add(invoice);
        }

        protected void ThenTransactionIsDeleted(int id)
        {
            var transaction = _transactionRepository.GetAll().FirstOrDefault(x => x.Id == id);
            Assert.IsNotNull(transaction.DeleteDate);
        }

        protected InvoiceItem GivenInvoiceItem(int productId, decimal quantity, decimal price)
        {
            return new InvoiceItem
            {
                ProductId = productId,
                Quantity = quantity,
                Price = price
            };
        }

        protected void GivenDailyBillingIs(BillingSource source, decimal total)
        {
            _billingRepository.Add(new DailyBilling
            {
                Total = total,
                Source = source
            });
        }

        protected void ThenSavedDailyBillingWith(BillingSource source, decimal total)
        {
            var billing = _billingRepository.GetAll().FirstOrDefault(x => x.Source == source && x.Total == total);
            Assert.IsNotNull(billing);
        }

        protected void ThenUpdateCustomerSurchargeTo(int customerId, decimal surcharge)
        {
            var customer = _customerRepository.Get(customerId);
            Assert.AreEqual(surcharge, customer.LastSurcharge);
        }
        protected void ThenSaveCustomerPriceFor(int customerId, int productId, decimal price)
        {
            var customerPrice = _customerPriceRepository.GetAll().FirstOrDefault(x => x.CustomerId == customerId && x.ProductId == productId);
            Assert.IsNotNull(customerPrice);
            Assert.AreEqual(price, customerPrice.Price);
        }

        protected void ThenSavedPaymentWith(int customerId, decimal amount)
        {
            var payment = _paymentRepository.GetAll().FirstOrDefault(x => x.PersonId == customerId);
            Assert.IsNotNull(payment);
            Assert.AreEqual(amount, payment.Total);
        }

        protected void ThenUpdatedCustomerBalanceTo(int customerId, decimal balance)
        {
            Assert.AreEqual(balance, _customerRepository.Get(customerId).Balance);
        }
    }
}
