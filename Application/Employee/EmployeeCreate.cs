using FriMav.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FriMav.Application
{
    public class EmployeeCreate
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }
        public string Cuit { get; set; }
        public string Address { get; set; }
        public int? ZoneId { get; set; }
    }
}