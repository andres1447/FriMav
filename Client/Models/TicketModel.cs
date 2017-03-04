using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Client.Models
{
    public class TicketModel
    {
        public DateTime Date { get; set; }
        public string Brand { get; set; }
        public IEnumerable<TicketItemModel> Items { get; set; }

        public decimal Total()
        {
            return Items.Sum(x => x.Amount());
        }
    }

    public class TicketItemModel
    {
        public string Product { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }

        public decimal Amount()
        {
            return Quantity * Price;
        }
    }
}
