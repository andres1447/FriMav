﻿using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure
{
    internal static class MissingDllHack
    {
        // Must reference a type in EntityFramework.SqlServer.dll so that this dll will be
        // included in the output folder of referencing projects without requiring a direct 
        // dependency on Entity Framework. See http://stackoverflow.com/a/22315164/1141360.
        private static SqlProviderServices instance = SqlProviderServices.Instance;
    }
}
