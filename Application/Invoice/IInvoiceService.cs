using FriMav.Domain;
using FriMav.Domain.Proyections;
using System.Collections.Generic;

namespace FriMav.Application
{
    public interface IInvoiceService
    {
        Invoice Get(int invoiceId);
        Invoice GetDisplay(int invoiceId);
        IEnumerable<Invoice> GetAll();
        IEnumerable<UndeliveredInvoice> GetUndeliveredInvoices();
        void BeforeCreate(Invoice invoice);
        void Create(Invoice invoice);
        void Update(Invoice invoice);
    }
}
