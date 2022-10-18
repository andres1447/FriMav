using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public class EmployeeAccountResponse
    {
        public EmployeeAccountResponse(int totalCount, List<UnliquidatedDocument> items)
        {
            TotalCount = totalCount;
            Items = items;
        }

        public List<UnliquidatedDocument> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
