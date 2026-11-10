using System;

namespace FriMav.Domain.Entities
{
    public class ProductMeasure : Entity
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public DateTime? DeleteDate { get; set; }

        public void Delete()
        {
            DeleteDate = DateTime.UtcNow;
        }
    }
}
