using System.Collections.Generic;
using System.Linq;

namespace FriMav.Client.Models
{
    public class EmployeeTicketModel : EmployeeDocumentModel
    {
        public List<TicketItemModel> Items { get; set; }

        public decimal Total()
        {
            return Items.Select(x => x.Amount()).DefaultIfEmpty(0).Sum();
        }
    }
}
