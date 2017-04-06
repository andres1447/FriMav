using FriMav.Domain;
using FriMav.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FriMav.Domain.Proyections;

namespace FriMav.Application
{
    public class DeliveryService : IDeliveryService
    {
        private IDeliveryRepository _deliveryRepository;
        private IInvoiceRepository _invoiceRepository;
        private INumberSequenceService _numberSequenceService;

        public DeliveryService(
            IDeliveryRepository deliveryRepository,
            IInvoiceRepository invoiceReplository,
            INumberSequenceService numberSequenceService)
        {
            _deliveryRepository = deliveryRepository;
            _invoiceRepository = invoiceReplository;
            _numberSequenceService = numberSequenceService;
        }

        public void Create(DeliveryCreate command)
        {
            using (var scope = new System.Transactions.TransactionScope())
            {
                var delivery = new Delivery
                {
                    Date = command.Date,
                    Number = _numberSequenceService.NextOrInit("Delivery"),
                    PersonId = command.EmployeeId,
                    Invoices = command.Invoices.Select(x => new Invoice { TransactionId = x }).ToList()
                };
                foreach (var invoice in delivery.Invoices)
                {
                    _invoiceRepository.Attach(invoice);
                }
                _deliveryRepository.Create(delivery);
                _deliveryRepository.DetectChanges();
                _deliveryRepository.Save();
                scope.Complete();
            }
        }

        public Delivery Get(int deliveryId)
        {
            return _deliveryRepository.GetWithInvoices(deliveryId);
        }

        public IEnumerable<Delivery> GetAll()
        {
            return _deliveryRepository.GetAll();
        }

        public void Delete(Delivery delivery)
        {
            delivery.DeleteDate = DateTime.UtcNow;
            _deliveryRepository.Update(delivery);
            _deliveryRepository.Save();
        }

        public IEnumerable<DeliveryListing> GetListing()
        {
            return _deliveryRepository.GetListing();
        }
    }
}
