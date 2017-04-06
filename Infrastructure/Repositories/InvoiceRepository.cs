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

        public IEnumerable<UndeliveredInvoice> GetUndeliveredInvoices()
        {
            var deliveries = _databaseContext.Set<Delivery>();

            var q = _databaseContext.Set<Invoice>()
                    .Where(i => i.Shipping == Shipping.Delivery && !i.IsRefunded && i.Person.PersonType == PersonType.Customer && !(deliveries.Any(d => d.Invoices.Contains(i) && !d.DeleteDate.HasValue)))
                    .Select(i => new UndeliveredInvoice
                     {
                         TransactionId = i.TransactionId,
                         Date = i.Date,
                         Number = i.Number,
                         PersonName = i.Person.Name,
                         PersonId = i.PersonId,
                         PersonCode = i.Person.Code
                     });

            return q.ToList();
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
