using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Client.Models
{
    public class CatalogModel
    {
        public string Name { get; set; }
        public IEnumerable<ProductModel> Products { get; set; } 
    }
}
