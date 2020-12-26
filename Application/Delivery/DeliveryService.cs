using FriMav.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FriMav.Domain.Proyections;
using FriMav.Domain.Entities;

namespace FriMav.Application
{
    public class DeliveryService : IDeliveryService
    {
        private IRepository<Delivery> _deliveryRepository;
        private IRepository<Invoice> _invoiceRepository;
        private IDocumentNumberGenerator _numberSequenceService;

        public DeliveryService(
            IRepository<Delivery> deliveryRepository,
            IRepository<Invoice> invoiceReplository,
            IDocumentNumberGenerator numberSequenceService)
        {
            _deliveryRepository = deliveryRepository;
            _invoiceRepository = invoiceReplository;
            _numberSequenceService = numberSequenceService;
        }

        public void Create(DeliveryCreate command)
        {
            var invoices = _invoiceRepository.Query().Where(x => command.Invoices.Contains(x.Id)).ToList();
            var delivery = new Delivery
            {
                Date = command.Date,
                Number = _numberSequenceService.NextForDelivery(),
                EmployeeId = command.EmployeeId,
                Invoices = invoices
            };
            _deliveryRepository.Add(delivery);
        }

        public Delivery Get(int id)
        {
            var delivery = _deliveryRepository.Get(id, x => x.Invoices);
            if (delivery == null)
                throw new NotFoundException();

            return delivery;
        }

        public IEnumerable<Delivery> GetAll()
        {
            return _deliveryRepository.GetAll();
        }

        public void Delete(int id)
        {
            var delivery = Get(id);
            delivery.Delete();
        }

        public List<DeliveryListing> GetListing()
        {
            return _deliveryRepository
                .Query(x => x.Invoices, x => x.Employee)
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

            return _invoiceRepository.Query().Where(x => !deliveries.Any(d => d.DeleteDate.HasValue && d.Invoices.Contains(x)))
            .Select(x => new UndeliveredInvoice
            {
                Id = x.Id,
                Date = x.Date,
                Number = x.Number,
                PersonCode = x.Person.Code,
                PersonId = x.PersonId,
                PersonName = x.Person.Name
            }).ToList();
        }
    }
}
