using System;
using System.Collections.Generic;
using System.Linq;
using FriMav.Domain;
using FriMav.Domain.Proyections;
using FriMav.Domain.Repositories;

namespace FriMav.Application
{
    public class InvoiceService : IInvoiceService
    {
        private IInvoiceRepository _invoiceRepository;
        private IProductRepository _productRepository;
        private ICustomerRepository _customerRepository;
        private IPriceForCustomerRepository _priceForCustomerRepository;
        private ITransactionService _transactionService;

        public InvoiceService(
            IInvoiceRepository invoiceRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository,
            IPriceForCustomerRepository priceForCustomerRepository,
            ITransactionService transactionService)
        {
            _invoiceRepository = invoiceRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _priceForCustomerRepository = priceForCustomerRepository;
            _transactionService = transactionService;
        }

        public void BeforeCreate(Invoice invoice)
        {
            invoice.Total = invoice.Items.Sum(it => it.Quantity * it.Price);
            UpdateCustomer(invoice);
        }

        public void Create(Invoice invoice)
        {
            using (var scope = new System.Transactions.TransactionScope())
            {
                BeforeCreate(invoice);
                _invoiceRepository.Save();
                scope.Complete();
            }
        }

        public IEnumerable<Invoice> GetAll()
        {
            return _invoiceRepository.GetAll();
        }

        public IEnumerable<Invoice> GetUndeliveredInvoices()
        {
            return _invoiceRepository.GetUndeliveredInvoices();
        }

        public Invoice Get(int invoiceId)
        {
            return _invoiceRepository.FindBy(x => x.TransactionId == invoiceId);
        }

        public void Update(Invoice invoice)
        {
            _invoiceRepository.Update(invoice);
            _invoiceRepository.DetectChanges();
            _invoiceRepository.Save();
        }

        public Invoice GetDisplay(int invoiceId)
        {
            return _invoiceRepository.GetDisplay(invoiceId);
        }

        #region Private

        private void UpdateCustomer(Invoice invoice)
        {
            var customer = _customerRepository.FindBy(c => c.PersonId == invoice.PersonId);

            customer.Shipping = invoice.Shipping;
            customer.PaymentMethod = invoice.PaymentMethod;
            AddTransactions(invoice, customer);
            UpdateCustomerPrices(invoice);
            _customerRepository.Update(customer);
        }

        private void AddTransactions(Invoice invoice, Customer customer)
        {
            var isPaid = invoice.PaymentMethod == PaymentMethod.Cash;
            _transactionService.CreateWithoutSaving(invoice, !isPaid);

            if (isPaid)
            {
                var payment = new Transaction()
                {
                    Date = invoice.Date,
                    PersonId = invoice.PersonId,
                    Total = invoice.Total,
                    TransactionType = TransactionType.Payment
                };
                invoice.Payments.Add(new TransactionPayment()
                {
                    Amount = -invoice.Total,
                    TargetTransaction = invoice
                });
                _transactionService.CreateWithoutSaving(payment, !isPaid);
            }
        }

        private void UpdateCustomerPrices(Invoice invoice)
        {
            var productIds = invoice.Items.Select(it => it.ProductId);
            var products = _productRepository.FindAllBy(p => productIds.Contains(p.ProductId));
            var prices = _priceForCustomerRepository.FindAllBy(p => productIds.Contains(p.ProductId) && p.PersonId == invoice.PersonId);

            PriceForCustomer custom;
            Product product;
            foreach (var item in invoice.Items)
            {
                custom = prices.FirstOrDefault(x => x.ProductId == item.ProductId);
                product = products.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (custom != null)
                {
                    if (product.Price == item.Price)
                    {
                        _priceForCustomerRepository.Delete(custom);
                    }
                    else if (custom.Price != item.Price)
                    {
                        custom.Price = item.Price;
                        _priceForCustomerRepository.Update(custom);
                    }
                }
                else if (product.Price != item.Price)
                {
                    _priceForCustomerRepository.Create(new PriceForCustomer()
                    {
                        ProductId = item.ProductId,
                        Price = item.Price,
                        PersonId = invoice.PersonId
                    });
                }
            }
        }

        #endregion
    }
}
