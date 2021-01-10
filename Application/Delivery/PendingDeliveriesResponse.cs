using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public class PendingDeliveriesResponse
    {
        public bool HasPending { get; set; }

        public PendingDeliveriesResponse(bool hasPending)
        {
            HasPending = hasPending;
        }
    }
}
