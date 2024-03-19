using System.Collections.Generic;
using System.Linq;

namespace FriMav.Application
{
    public class TicketCreate
    {
        public List<TicketItemCreate> Items { get; set; }

        public decimal Total() => Items.Select(x => x.Total()).DefaultIfEmpty(0).Sum();
    }
}
