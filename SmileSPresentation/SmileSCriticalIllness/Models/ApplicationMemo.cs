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
    
    public partial class ApplicationMemo
    {
        public int ApplicationMemoId { get; set; }
        public string ApplicationMemoCode { get; set; }
        public Nullable<int> ApplicationId { get; set; }
        public Nullable<int> MemoTypeId { get; set; }
        public string MemoText { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedByUserId { get; set; }
    
        public virtual Application Application { get; set; }
    }
}
