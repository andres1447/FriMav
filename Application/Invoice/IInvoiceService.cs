using FriMav.Domain;
using FriMav.Domain.Entities;
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

        [Transactional]
        void CreateTicket(TicketCreate request);

        [Transactional]
        void CancelTicket(TicketCreate request);

        [Transactional]
        void DontDeliver(int id);

        [Transactional]
        void AssignExternalReferenceNumber(int id, string referenceNumber);
    }
}
