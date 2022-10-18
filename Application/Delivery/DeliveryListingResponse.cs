using FriMav.Domain.Proyections;
using System.Collections.Generic;

namespace FriMav.Application
{
    public class DeliveryListingResponse
    {
        public DeliveryListingResponse(int totalCount, List<DeliveryListing> items)
        {
            TotalCount = totalCount;
            Items = items;
        }

        public List<DeliveryListing> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
