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
    
    public partial class Salary
    {
        public int SalaryId { get; set; }
        public int EmployeeId { get; set; }
        public System.DateTime Since { get; set; }
        public decimal Amount { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
