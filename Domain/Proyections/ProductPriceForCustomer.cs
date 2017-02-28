using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Proyections
{
    public class ProductPriceForCustomer
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public int? FamilyId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal PriceForCustomer { get; set; }
    }
}
