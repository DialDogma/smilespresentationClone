//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSCriticalIllness.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DisclosureLog
    {
        public int DisclosureLogId { get; set; }
        public Nullable<int> DisclosureId { get; set; }
        public Nullable<int> DisclosureStatusId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
    
        public virtual Disclosure Disclosure { get; set; }
    }
}
