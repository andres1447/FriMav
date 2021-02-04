using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public class Time : ITime
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
