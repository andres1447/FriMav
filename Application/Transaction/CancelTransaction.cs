using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FriMav.Application
{
    public class CancelTransaction
    {
        public string Description { get; set; }

        [Required]
        public int Id { get; set; }
    }
}