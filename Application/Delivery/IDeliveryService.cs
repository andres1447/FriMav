using FriMav.Domain;
using FriMav.Domain.Entities;
using FriMav.Domain.Proyections;
using System.Collections.Generic;

namespace FriMav.Application
{
    public interface IDeliveryService
    {
        IEnumerable<Delivery> GetAll();
        DeliveryResponse Get(int id);
        DeliveryListingResponse GetListing(bool closed, int offset, int count);
        IEnumerable<UndeliveredInvoice> GetUndeliveredInvoices();
        PendingDeliveriesResponse HasPendingDeliveries();
        DeliveryCloseResponse GetForClose(int id);

        [Transactional]
        void Create(DeliveryCreate command);

        [Transactional]
        void Delete(int id);

        [Transactional]
        void Close(DeliveryClose request);
    }
}
