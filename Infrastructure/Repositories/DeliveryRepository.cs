using FriMav.Domain;
using FriMav.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using FriMav.Domain.Proyections;

namespace FriMav.Infrastructure.Repositories
{
    public class DeliveryRepository : BaseRepository<Delivery>, IDeliveryRepository
    {
        public DeliveryRepository(IDatabaseContext databaseContext) : base(databaseContext) { }

        public IEnumerable<DeliveryListing> GetListing()
        {
            return _databaseContext.Set<Delivery>()
                .Where(d => !d.DeleteDate.HasValue)
                .Select(x => new DeliveryListing
                {
                    DeliveryId = x.DeliveryId,
                    Date = x.Date,
                    Number = x.Number,
                    Employee = x.Employee.Name,
                    Invoices = x.Invoices.Count
                }).ToList();
        }

        public Delivery GetWithInvoices(int deliveryId)
        {
            return _databaseContext.Set<Delivery>()
                .Where(x => x.DeliveryId == deliveryId)
                .Include(d => d.Employee)
                .Include(d => d.Invoices.Select(i => i.Person))
                .Include(d => d.Invoices.Select(i => i.Items.Select(it => it.Product)))
                .FirstOrDefault();
        }
    }
}
