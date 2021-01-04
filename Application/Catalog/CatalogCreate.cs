using FriMav.Application;
using FriMav.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FriMav.Application
{
    public class CatalogCreate
    {
        [Required]
        public string Name { get; set; }
        public List<int> Products { get; set; }
    }
}