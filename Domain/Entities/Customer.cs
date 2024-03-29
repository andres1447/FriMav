﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Entities
{
    public class Customer : Person
    {
        public const int TicketCustomerId = 1;
        public const int DefaultCustomerId = 2;

        public decimal LastSurcharge { get; set; }
        public ICollection<CustomerPrice> CustomerPrices { get; set; } = new List<CustomerPrice>();
    }
}
