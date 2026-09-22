using FriMav.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace FriMav.Application
{
    public class ProductUpdate
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int? ProductTypeId { get; set; }
        public bool Active { get; set; }
        public ProductMeasure Measure { get; set; }
    }
}