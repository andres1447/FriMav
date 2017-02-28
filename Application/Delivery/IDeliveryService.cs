using FriMav.Domain;
using FriMav.Domain.Proyections;
using FriMav.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public interface IDeliveryService
    {
        void Create(DeliveryCreate command);
        IEnumerable<Delivery> GetAll();
        Delivery Get(int deliveryId);
        void Delete(Delivery delivery);
        IEnumerable<DeliveryListing> GetListing();
    }
}
