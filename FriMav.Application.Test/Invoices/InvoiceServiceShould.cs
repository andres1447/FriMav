using FriMav.Application.Billing;
using FriMav.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FriMav.Application.Test.Invoices
{
    [TestClass]
    public class InvoiceServiceShould : ApplicationTest
    {
        private const int CustomerId = 1;
        private const int ProductId = 10;

        private BillingService _billingService;
        private InvoiceService _service;
        private Invoice _invoice;

        protected override void Setup()
        {
            _billingService = new BillingService(_billingRepository);
            _service = new InvoiceService(
                _invoiceRepository,
                _productRepository,
                _customerRepository,
                _customerPriceRepository,
                _documentNumberGenerator,
                _paymentRepository,
                _billingService);
        }

        [TestMethod]
        public void SaveInvoicePaidByCredit()
        {
            GivenProduct(ProductId, "a");
            GivenCustomer(CustomerId, "pepe");

            WhenCreateWith(PaymentMethod.Credit, 0, Item(ProductId, 1, 10));

            ThenSavedInvoiceWithCustomerId(CustomerId);
            ThenSavedInvoiceWithItem(ProductId, 1, 10);
            ThenSavedInvoiceWithBalance(10);
            ThenUpdatedCustomerBalanceTo(CustomerId, 10);
            ThenSaveCustomerPriceFor(CustomerId, ProductId, 10);
        }

        [TestMethod]
        public void SaveInvoicePaidByCash()
        {
            GivenProduct(ProductId, "a");
            GivenCustomer(CustomerId, "pepe");

            WhenCreateWith(PaymentMethod.Cash, 0, Item(ProductId, 1, 10));

            ThenSavedPaymentWith(CustomerId, -10);
            ThenUpdatedCustomerBalanceTo(CustomerId, 0);
        }

        [TestMethod]
        public void SaveInvoiceWithSurchargePaidByCredit()
        {
            GivenProduct(ProductId, "a");
            GivenCustomer(CustomerId, "pepe");

            WhenCreateWith(PaymentMethod.Credit, 0.1m, Item(ProductId, 1, 10));

            ThenSavedInvoiceWithCustomerId(CustomerId);
            ThenSavedInvoiceWithItem(ProductId, 1, 11);
            ThenSavedInvoiceWithBalance(11);
            ThenUpdatedCustomerBalanceTo(CustomerId, 11);
            ThenSaveCustomerPriceFor(CustomerId, ProductId, 10);
            ThenUpdateCustomerSurchargeTo(CustomerId, 0.1m);
        }

        [TestMethod]
        public void SaveInvoiceWithSurchargePaidByCash()
        {
            GivenProduct(ProductId, "a");
            GivenCustomer(CustomerId, "pepe");

            WhenCreateWith(PaymentMethod.Cash, 0.1m, Item(ProductId, 1, 10));

            ThenSavedPaymentWith(CustomerId, -11);
            ThenUpdatedCustomerBalanceTo(CustomerId, 0);
            ThenUpdateCustomerSurchargeTo(CustomerId, 0.1m);
        }

        [TestMethod]
        public void SaveBilling()
        {
            GivenProduct(ProductId, "a");
            GivenCustomer(CustomerId, "pepe");

            WhenCreateWith(PaymentMethod.Cash, 0.1m, Item(ProductId, 1, 10));

            ThenSavedDailyBillingWith(BillingSource.Invoice, 11);
        }

        [TestMethod]
        public void UpdateExistingBilling()
        {
            GivenProduct(ProductId, "a");
            GivenCustomer(CustomerId, "pepe");

            WhenCreateWith(PaymentMethod.Cash, 0.1m, Item(ProductId, 1, 10));
            WhenCreateWith(PaymentMethod.Cash, 0, Item(ProductId, 1, 20));

            ThenSavedDailyBillingWith(BillingSource.Invoice, 31);
        }

        [TestMethod]
        public void CreateTicketSaveBilling()
        {
            GivenProduct(ProductId, "a");
            GivenCustomer(CustomerId, "pepe");

            WhenCreateTicketWith(TicketItem(ProductId, 1, 11));

            ThenSavedDailyBillingWith(BillingSource.Ticket, 11);
        }

        [TestMethod]
        public void CreateTicketUpdateExistingBilling()
        {
            GivenProduct(ProductId, "a");
            GivenCustomer(CustomerId, "pepe");

            WhenCreateWith(PaymentMethod.Cash, 0.1m, Item(ProductId, 1, 10));
            WhenCreateTicketWith(TicketItem(ProductId, 1, 10));
            WhenCreateTicketWith(TicketItem(ProductId, 1, 20));

            ThenSavedDailyBillingWith(BillingSource.Ticket, 30);
        }

        private void WhenCreateWith(PaymentMethod method, decimal surcharge = 0, params InvoiceItemCreate[] items)
        {
            _service.Create(new InvoiceCreate
            {
                CustomerName = "pepe",
                PaymentMethod = method,
                PersonId = 1,
                Surcharge = surcharge,
                Items = items.ToList()
            });

            _invoice = _invoiceRepository.GetAll().First();
        }

        private void WhenCreateTicketWith(params TicketItemCreate[] items)
        {
            _service.CreateTicket(new TicketCreate
            {
                Items = items.ToList()
            });
        }

        private void ThenSavedInvoiceWithBalance(decimal balance)
        {
            Assert.AreEqual(balance, _invoice.Balance);
        }

        private void ThenSavedInvoiceWithItem(int productId, decimal quantity, decimal price)
        {
            var item = _invoice.Items.FirstOrDefault(x => x.ProductId == productId);
            Assert.IsNotNull(item);
            Assert.AreEqual(quantity, item.Quantity);
            Assert.AreEqual(price, item.Price);
        }

        private void ThenSavedInvoiceWithCustomerId(int customerId)
        {
            Assert.AreEqual(_invoice.PersonId, customerId);
        }

        protected static InvoiceItemCreate Item(int id, int quantity, int price)
        {
            return new InvoiceItemCreate
            {
                ProductId = id,
                Quantity = quantity,
                Price = price
            };
        }

        protected static TicketItemCreate TicketItem(int id, int quantity, int price)
        {
            return new TicketItemCreate
            {
                ProductId = id,
                Quantity = quantity,
                Price = price
            };
        }
    }
}
