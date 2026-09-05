using System.Collections.Generic;

namespace FriMav.Application
{
    public class ProductPriceListGroup
    {
        public string Name { get; set; }
        public List<ProductPriceListItem> Products { get; set; }
    }

    public class ProductPriceListItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
