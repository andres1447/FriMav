﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriMav.Application
{
    public class CancelTransaction
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int TransactionId { get; set; }
    }
}