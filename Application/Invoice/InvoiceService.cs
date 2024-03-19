using System;
using System.Collections.Generic;
using System.Linq;
using FriMav.Application.Billing;
using FriMav.Domain;
using FriMav.Domain.Entities;

namespace FriMav.Application
{

    public class InvoiceService : IInvoiceService
    {
        private readonly IRepository<Invoice> _invoiceRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerPrice> _customerPriceRepository;
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IDocumentNumberGenerator _documentNumberGenerator;
        private readonly IBillingService _billingService;

        public InvoiceService(
            IRepository<Invoice> invoiceRepository,
            IRepository<Product> productRepository,
            IRepository<Customer> customerRepository,
            IRepository<CustomerPrice> priceForCustomerRepository,
            IDocumentNumberGenerator documentNumberGenerator,
            IRepository<Payment> paymentRepository,
            IBillingService billingService)
        {
            _invoiceRepository = invoiceRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _customerPriceRepository = priceForCustomerRepository;
            _documentNumberGenerator = documentNumberGenerator;
            _paymentRepository = paymentRepository;
            _billingService = billingService;
        }

        public InvoiceResult Create(InvoiceCreate request)
        {
            var customer = GetRequestCustomer(request);

            var invoice = MapInvoice(request, customer);

            UpdateCustomerPrices(customer, invoice);

            invoice.ApplySurcharge(request.Surcharge);
            invoice.Total = invoice.CalculateTotal();

            customer.Accept(invoice);
            customer.LastSurcharge = request.Surcharge;

            _invoiceRepository.Add(invoice);

            if (request.PaymentMethod == PaymentMethod.Cash)
            {
                CreateCancellingPayment(invoice, customer);
            }

            _billingService.SaveBilling(BillingSource.Invoice, invoice.Date, invoice.Total);

            return new InvoiceResult(invoice.Number, invoice.Total, invoice.Balance);
        }

        public void CreateTicket(TicketCreate request)
        {
            _billingService.SaveBilling(BillingSource.Ticket, DateTime.UtcNow, request.Total());
        }

        public void CancelTicket(TicketCreate request)
        {
            _billingService.SaveBilling(BillingSource.Ticket, DateTime.UtcNow, -request.Total());
        }

        private void CreateCancellingPayment(Invoice invoice, Customer customer)
        {
            var number = _documentNumberGenerator.NextForPayment();
            var payment = invoice.CreateCancellingPayment(number);

            customer.Accept(payment);

            _paymentRepository.Add(payment);
        }

        private Invoice MapInvoice(InvoiceCreate request, Customer customer)
        {
            var items = MapInvoiceItems(request);
            var invoice = new Invoice
            {
                Person = customer,
                PersonId = customer.Id,
                CustomerName = request.CustomerName,
                Shipping = request.Shipping ?? customer.Shipping ?? Shipping.Self,
                PaymentMethod = request.PaymentMethod ?? customer.PaymentMethod ?? PaymentMethod.Cash,
                Number = _documentNumberGenerator.NextForInvoice(),
                DeliveryAddress = request.DeliveryAddress,
                Items = items
            };
            return invoice;
        }

        private List<InvoiceItem> MapInvoiceItems(InvoiceCreate request)
        {
            var products = GetInvoiceProducts(request);
            return request.Items.Join(products, x => x.ProductId, x => x.Id, (x, y) => new InvoiceItem
            {
                Price = x.Price,
                Quantity = x.Quantity,
                ProductId = x.ProductId,
                ProductName = y.Name,
                Product = y
            }).ToList();
        }

        private IEnumerable<Product> GetInvoiceProducts(InvoiceCreate request)
        {
            var productIds = request.Items.Select(x => x.ProductId).ToList();
            return _productRepository.Query().Where(x => productIds.Contains(x.Id)).ToList();
        }

        private Customer GetRequestCustomer(InvoiceCreate request)
        {
            var customer = _customerRepository.Get(request.PersonId, x => x.CustomerPrices);
            if (customer == null)
                throw new NotFoundException();
            return customer;
        }

        public IEnumerable<Invoice> GetAll()
        {
            return _invoiceRepository.GetAll();
        }

        public Invoice Get(int id)
        {
            return _invoiceRepository.Get(id, x => x.Person, x => x.Items.Select(i => i.Product));
        }

        public Invoice GetDisplay(int id)
        {
            return _invoiceRepository.Get(id, x => x.Person, x => x.Items);
        }

        public void DontDeliver(int id)
        {
            var invoice = _invoiceRepository.Get(id);
            if (invoice == null) throw new NotFoundException();

            invoice.Shipping = Shipping.Self;
        }

        private void UpdateCustomerPrices(Customer customer, Invoice invoice)
        {
            foreach (var item in invoice.Items)
            {
                var custom = customer.CustomerPrices.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (custom != null)
                {
                    if (item.Product.Price == item.Price)
                    {
                        _customerPriceRepository.Delete(custom);
                    }
                    else if (custom.Price != item.Price)
                    {
                        custom.Price = item.Price;
                    }
                }
                else if (item.Product.Price != item.Price)
                {
                    _customerPriceRepository.Add(new CustomerPrice()
                    {
                        ProductId = item.ProductId,
                        Price = item.Price,
                        CustomerId = invoice.PersonId
                    });
                }
            }
        }

        public void AssignExternalReferenceNumber(int id, string referenceNumber)
        {
            var invoice = _invoiceRepository.Get(id);
            if (invoice == null) throw new NotFoundException();
            invoice.ExternalReferenceNumber = referenceNumber;
        }
    }
}
