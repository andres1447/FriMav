﻿using FriMav.Application;
using FriMav.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FriMav.Application
{
    public class CatalogCreate
    {
        public string Name { get; set; }
        public List<int> Products { get; set; }
    }
}