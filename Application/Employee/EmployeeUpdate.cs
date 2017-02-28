using FriMav.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriMav.Application
{
    public class EmployeeUpdate
    {
        public int PersonId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Cuit { get; set; }
        public string Address { get; set; }

        public Employee ToDomain()
        {
            return new Employee()
            {
                PersonId = this.PersonId,
                Code = this.Code,
                Name = this.Name,
                Cuit = this.Cuit,
                Address = this.Address
            };
        }
    }
}