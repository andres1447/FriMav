using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Entities
{
    public abstract class Person : Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Cuit { get; set; }
        public string Address { get; set; }
        public decimal Balance { get; set; }
        public Shipping? Shipping { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public int? ZoneId { get; set; }
        public Zone Zone { get; set; }
        public DateTime? DeleteDate { get; set; }

        // Solo lo usa Customer, fix para error de insert con null value
        public decimal LastSurcharge { get; set; } = 0;

        public void Delete()
        {
            DeleteDate = DateTime.Now;
        }

        public bool IsDeleted()
        {
            return DeleteDate.HasValue;
        }

        public void WithoutZone()
        {
            Zone = null;
            ZoneId = null;
        }

        public void Cancel(TransactionDocument document)
        {
            Balance -= document.Total;
            document.DeleteDate = DateTime.UtcNow;
        }

        public void Accept(TransactionDocument document)
        {
            Balance += document.Total;
            document.Balance = Balance;
        }
    }
}
