using FriMav.Domain.Proyections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Repositories
{
    public interface IDeliveryRepository : IBaseRepository<Delivery>
    {
        Delivery GetWithInvoices(int deliveryId);
        IEnumerable<DeliveryListing> GetListing();
    }
}
