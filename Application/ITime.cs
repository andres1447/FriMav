﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public interface ITime
    {
        DateTime UtcNow();
    }
}
