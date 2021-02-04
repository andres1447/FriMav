using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public class GoodsSoldCreate
    {
        public int EmployeeId { get; set; }
        public List<GoodsSoldCreateItem> Items { get; set; }
    }

    public class GoodsSoldCreateItem
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
