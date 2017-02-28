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

        public DeliveryService(
            IDeliveryRepository deliveryRepository,
            IInvoiceRepository invoiceReplository)
        {
            _deliveryRepository = deliveryRepository;
            _invoiceRepository = invoiceReplository;
        }

        public void Create(DeliveryCreate command)
        {
            var delivery = new Delivery
            {
                Date = command.Date,
                PersonId = command.EmployeeId,
                Invoices = _invoiceRepository.FindAllBy(x => command.Invoices.Contains(x.TransactionId)).ToList()
            };
            foreach (var invoice in delivery.Invoices)
            {
                invoice.Delivery = delivery;
            }
            _deliveryRepository.Create(delivery);
            _deliveryRepository.DetectChanges();
            _deliveryRepository.Save();
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
            delivery.Invoices.Clear();
            _deliveryRepository.Delete(delivery);
            _deliveryRepository.DetectChanges();
            _deliveryRepository.Save();
        }

        public IEnumerable<DeliveryListing> GetListing()
        {
            return _deliveryRepository.GetListing();
        }
    }
}
