using FriMav.Domain;
using FriMav.Domain.Entities;
using FriMav.Domain.Proyections;
using System.Collections.Generic;

namespace FriMav.Application
{
    public interface IInvoiceService
    {
        Invoice Get(int id);
        Invoice GetDisplay(int id);
        IEnumerable<Invoice> GetAll();

        [Transactional]
        InvoiceResult Create(InvoiceCreate request);
    }
}
