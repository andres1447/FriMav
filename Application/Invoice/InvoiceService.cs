using System.Collections.Generic;
using System.Linq;
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

        public InvoiceService(
            IRepository<Invoice> invoiceRepository,
            IRepository<Product> productRepository,
            IRepository<Customer> customerRepository,
            IRepository<CustomerPrice> priceForCustomerRepository,
            IDocumentNumberGenerator documentNumberGenerator,
            IRepository<Payment> paymentRepository)
        {
            _invoiceRepository = invoiceRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _customerPriceRepository = priceForCustomerRepository;
            _documentNumberGenerator = documentNumberGenerator;
            _paymentRepository = paymentRepository;
        }

        public InvoiceResult Create(InvoiceCreate request)
        {
            var customer = GetRequestCustomerOrDefault(request);

            var invoice = MapInvoice(request, customer);

            customer.Accept(invoice);

            _invoiceRepository.Add(invoice);

            if (request.PaymentMethod == PaymentMethod.Cash)
            {
                Payment payment = CreateCancellingPayment(invoice);

                customer.Accept(payment);

                _paymentRepository.Add(payment);
            }

            UpdateCustomer(customer, invoice);

            return new InvoiceResult(invoice.Number, invoice.Total, invoice.Balance);
        }

        private Payment CreateCancellingPayment(Invoice invoice)
        {
            var number = _documentNumberGenerator.NextForPayment();
            return invoice.CreateCancellingPayment(number);
        }

        private Invoice MapInvoice(InvoiceCreate request, Customer customer)
        {
            var invoice = new Invoice
            {
                Person = customer,
                PersonId = customer.Id,
                CustomerName = request.CustomerName,
                Shipping = request.Shipping ?? customer.Shipping ?? Shipping.Self,
                PaymentMethod = request.PaymentMethod ?? customer.PaymentMethod ?? PaymentMethod.Cash,
                Number = _documentNumberGenerator.NextForInvoice(),
                DeliveryAddress = request.DeliveryAddress,
                Items = MapInvoiceItems(request)
            };
            invoice.Total = invoice.CalculateTotal();
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
                ProductName = y.Name
            }).ToList();
        }

        private IEnumerable<Product> GetInvoiceProducts(InvoiceCreate request)
        {
            var productIds = request.Items.Select(x => x.ProductId).ToList();
            return _productRepository.Query().Where(x => productIds.Contains(x.Id)).ToList();
        }

        private Customer GetRequestCustomerOrDefault(InvoiceCreate request)
        {
            var customerId = request.CustomerId ?? Customer.DefaultCustomerId;
            var customer = _customerRepository.Get(customerId, x => x.CustomerPrices);
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
            return _invoiceRepository.Get(id, x => x.Person, x => x.Items);
        }

        public Invoice GetDisplay(int id)
        {
            return _invoiceRepository.Get(id, x => x.Person, x => x.Items);
        }

        private void UpdateCustomer(Customer customer, Invoice invoice)
        {
            customer.Shipping = invoice.Shipping;
            UpdateCustomerPrices(customer, invoice);
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
    }
}
