using FriMav.Application.Billing;
using FriMav.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace FriMav.Application.Test.Transactions
{
    [TestClass]
    public class TransactionServiceTest : ApplicationTest
    {
        private const int CustomerId = 1;
        private const int InvoiceId = 10;

        private BillingService _billingService;
        private TransactionService _service;

        protected override void Setup()
        {
            _billingService = new BillingService(_billingRepository);
            _service = new TransactionService(
                _documentNumberGenerator,
                _transactionRepository,
                _customerRepository,
                _paymentRepository,
                _billingService);
        }

        [TestMethod]
        public void CancelDocumentRemovesTotalFromDailyBilling()
        {
            GivenCustomer(CustomerId, "pepe", 10);
            GivenInvoice(InvoiceId, CustomerId, GivenInvoiceItem(1, 1, 10));
            GivenDailyBillingIs(BillingSource.Invoice, 10);

            WhenCancelDocument(InvoiceId);

            ThenUpdatedCustomerBalanceTo(CustomerId, 0);
            ThenSavedDailyBillingWith(BillingSource.Invoice, 0);
            ThenTransactionIsDeleted(InvoiceId);
        }

        private void WhenCancelDocument(int id)
        {
            _service.Cancel(new CancelTransaction
            {
                Id = id
            });
        }
    }
}
