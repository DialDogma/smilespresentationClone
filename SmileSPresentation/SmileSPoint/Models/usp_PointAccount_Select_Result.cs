//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSPoint.Models
{
    using System;
    
    public partial class usp_PointAccount_Select_Result
    {
        public int PointAccountId { get; set; }
        public string PointAccountName { get; set; }
        public Nullable<int> PointAccountTypeId { get; set; }
        public string ReferenceCode { get; set; }
        public string ReferenceDetail { get; set; }
        public Nullable<double> PointRemain { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedById { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public bool IsActive { get; set; }
    }
}
