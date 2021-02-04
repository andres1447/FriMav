using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Entities.Payrolls
{
    public class GoodsSold : LiquidationDocument
    {
        public ICollection<GoodsSoldItem> Items { get; set; } = new List<GoodsSoldItem>();
    }

    public class GoodsSoldItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public int GoodsSoldId { get; set; }
    }
}
