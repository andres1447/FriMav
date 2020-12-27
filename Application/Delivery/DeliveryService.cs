using FriMav.Domain;
using System.Collections.Generic;
using System.Linq;
using FriMav.Domain.Proyections;
using FriMav.Domain.Entities;

namespace FriMav.Application
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IRepository<Delivery> _deliveryRepository;
        private readonly IRepository<Invoice> _invoiceRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IDocumentNumberGenerator _numberSequenceService;

        public DeliveryService(
            IRepository<Delivery> deliveryRepository,
            IRepository<Invoice> invoiceReplository,
            IDocumentNumberGenerator numberSequenceService,
            IRepository<Employee> employeeRepository)
        {
            _deliveryRepository = deliveryRepository;
            _invoiceRepository = invoiceReplository;
            _numberSequenceService = numberSequenceService;
            _employeeRepository = employeeRepository;
        }

        public void Create(DeliveryCreate command)
        {
            var employee = _employeeRepository.Get(command.EmployeeId);
            if (employee == null)
                throw new NotFoundException();

            var invoices = _invoiceRepository.Query().Where(x => command.Invoices.Contains(x.Id)).ToList();
            if (invoices.Count < command.Invoices.Count)
                throw new NotFoundException();

            var delivery = new Delivery
            {
                Number = _numberSequenceService.NextForDelivery(),
                EmployeeId = command.EmployeeId,
                Employee = employee,
                Invoices = invoices
            };
            _deliveryRepository.Add(delivery);
        }

        public DeliveryResponse Get(int id)
        {
            var response = _deliveryRepository.Query()
                .Where(x => x.Id == id)
                .Select(delivery => new DeliveryResponse
                {
                    Id = delivery.Id,
                    Date = delivery.Date,
                    Number = delivery.Number,
                    EmployeeCode = delivery.Employee.Code,
                    EmployeeName = delivery.Employee.Name,
                    Invoices = delivery.Invoices.Select(invoice => new DeliveryInvoice
                    {
                        Id = invoice.Id,
                        Date = invoice.Date,
                        Number = invoice.Number,
                        Total = invoice.Total,
                        PreviousBalance = invoice.PreviousBalance,
                        Balance = invoice.Balance,
                        CustomerCode = invoice.Person.Code,
                        CustomerName = invoice.CustomerName,
                        DeliveryAddress = invoice.DeliveryAddress,
                        DeliveryZone = invoice.Person.Zone != null ? invoice.Person.Zone.Name : null
                    }).ToList(),
                    Products = delivery.Invoices.SelectMany(x => x.Items).GroupBy(i => i.Id).Select(p => new DeliveryProduct
                    {
                        ProductId = p.Key,
                        Name = p.FirstOrDefault().ProductName,
                        Quantity = p.Sum(c => c.Quantity)
                    }).ToList()
                }).FirstOrDefault();

            if (response == null)
                throw new NotFoundException();

            return response;
        }

        public IEnumerable<Delivery> GetAll()
        {
            return _deliveryRepository.GetAll();
        }

        public void Delete(int id)
        {
            var delivery = _deliveryRepository.Get(id);
            if (delivery == null)
                throw new NotFoundException();

            delivery.Delete();
        }

        public List<DeliveryListing> GetListing()
        {
            return _deliveryRepository
                .Query()
                .Where(x => !x.DeleteDate.HasValue)
                .Select(x => new DeliveryListing
                {
                    Id = x.Id,
                    Date = x.Date,
                    Employee = x.Employee.Name,
                    Number = x.Number,
                    Invoices = x.Invoices.Count()
                }).ToList();
        }

        public IEnumerable<UndeliveredInvoice> GetUndeliveredInvoices()
        {
            var deliveries = _deliveryRepository.Query();

            return _invoiceRepository.Query()
                .Where(x => x.Shipping == Shipping.Delivery && !deliveries.Any(d => !d.DeleteDate.HasValue && d.Invoices.Contains(x)))
            .Select(x => new UndeliveredInvoice
            {
                Id = x.Id,
                Date = x.Date,
                Number = x.Number,
                CustomerCode = x.Person.Code,
                PersonId = x.PersonId,
                CustomerName = x.CustomerName,
                DeliveryAddress = x.DeliveryAddress,
                Total = x.Total,
                Products = x.Items.Select(it => new DeliveryProduct
                {
                    ProductId = it.ProductId,
                    Name = it.ProductName,
                    Quantity = it.Quantity
                }).ToList()
            }).ToList();
        }
    }
}
