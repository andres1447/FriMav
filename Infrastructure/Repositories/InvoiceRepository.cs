using FriMav.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using FriMav.Domain.Repositories;
using System.Data.SqlClient;
using Dapper;
using FriMav.Domain.Proyections;
using System;

namespace FriMav.Infrastructure.Repositories
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public override IEnumerable<Invoice> GetAll()
        {
            return _databaseContext.Set<Invoice>()
                        .Include(i => i.Items.Select(it => it.Product))
                        .ToList();
        }

        public IEnumerable<Invoice> GetUndeliveredInvoices()
        {
            return _databaseContext.Set<Transaction>()
                .AsNoTracking()
                .OfType<Invoice>()
                .Where(i => i.Shipping == Shipping.Delivery && !i.DeliveryId.HasValue && !i.IsRefunded)
                .Include(i => i.Person)
                .ToList();
        }

        public Invoice GetDisplay(int invoiceId)
        {
            return _databaseContext.Set<Invoice>()
                .AsNoTracking()
                .Include(i => i.Items.Select(it => it.Product))
                .Include(i => i.Person)
                .FirstOrDefault(i => i.TransactionId == invoiceId);
        }
    }
}
