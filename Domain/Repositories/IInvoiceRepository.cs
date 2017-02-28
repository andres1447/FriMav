
using FriMav.Domain.Proyections;
using System.Collections.Generic;

namespace FriMav.Domain.Repositories
{
    public interface IInvoiceRepository : IBaseRepository<Invoice>
    {
        Invoice GetDisplay(int invoiceId);
        IEnumerable<Invoice> GetUndeliveredInvoices();
    }
}
