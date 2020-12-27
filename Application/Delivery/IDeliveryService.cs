using FriMav.Domain;
using FriMav.Domain.Entities;
using FriMav.Domain.Proyections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public interface IDeliveryService
    {
        IEnumerable<Delivery> GetAll();

        DeliveryResponse Get(int id);

        List<DeliveryListing> GetListing();
        IEnumerable<UndeliveredInvoice> GetUndeliveredInvoices();

        [Transactional]
        void Create(DeliveryCreate command);

        [Transactional]
        void Delete(int id);
    }
}
