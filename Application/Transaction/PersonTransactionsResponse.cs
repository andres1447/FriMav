using FriMav.Domain.Proyections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public class PersonTransactionsResponse
    {
        public PersonTransactionsResponse(int totalCount, List<TransactionEntry> items)
        {
            TotalCount = totalCount;
            Items = items;
        }

        public List<TransactionEntry> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
