using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Entities
{
    public class NumberSequence : Entity
    {
        public string Key { get; set; }
        public int CurrentId { get; set; }
    }
}
