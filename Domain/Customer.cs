//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FriMav.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class Customer : Person
    {
        public string Code { get; set; }
        public Shipping Shipping { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Nullable<int> ZoneId { get; set; }
    
        public virtual Zone Zone { get; set; }
    }
}
