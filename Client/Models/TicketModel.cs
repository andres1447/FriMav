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
        public IEnumerable<ItemModel> Items { get; set; }

        public decimal Total()
        {
            return Items.Sum(x => x.Amount());
        }
    }
}
