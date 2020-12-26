using FriMav.Domain;
using FriMav.Domain.Entities;
using System.Data.Entity;
using System.Linq;

namespace FriMav.Infrastructure
{
    public class DocumentNumberGenerator : IDocumentNumberGenerator
    {
        public const string Delivery = "Delivery";
        public const string Invoice = "Invoice";
        public const string Payment = "Payment";
        public const string CreditNote = "CreditNote";
        public const string DebitNote = "DebitNote";

        private DbContext _db;

        public DocumentNumberGenerator(DbContext db)
        {
            _db = db;
        }

        public int NextForDelivery()
        {
            return NextOrInit(Delivery);
        }

        public int NextForInvoice()
        {
            return NextOrInit(Invoice);
        }

        public int NextForPayment()
        {
            return NextOrInit(Payment);
        }

        public int NextOrInit(string key)
        {
            var sequence = _db.Set<NumberSequence>().FirstOrDefault(x => x.Key == key);
            if (sequence == null)
            {
                sequence = new NumberSequence { Key = key, CurrentId = 1 };
                _db.Set<NumberSequence>().Add(sequence);
            }
            else
            {
                sequence.CurrentId++;
            }
            return sequence.CurrentId;
        }
    }
}
