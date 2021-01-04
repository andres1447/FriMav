﻿using FriMav.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FriMav.Application
{
    public class ProductCreate
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }
        public int? ProductTypeId { get; set; }
    }
}