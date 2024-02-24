using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Entities
{
    public class Product : Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime PriceDate { get; set; } = DateTime.UtcNow;
        public DateTime? DeleteDate { get; set; }
        public int? ProductTypeId { get; set; }
        public ProductType Type { get; set; }

        public void Delete()
        {
            DeleteDate = DateTime.UtcNow;
        }

        public void UndoDelete()
        {
            DeleteDate = null;
        }
    }
}
