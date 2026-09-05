using System.Collections.Generic;

namespace FriMav.Client.Models
{
    public class StorePriceListModel
    {
        public IEnumerable<StorePriceListGroupModel> Groups { get; set; }
    }

    public class StorePriceListGroupModel
    {
        public string Name { get; set; }
        public IEnumerable<StorePriceListProductModel> Products { get; set; }
    }

    public class StorePriceListProductModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
