//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSClaimOnLine.Models
{
    using System;
    
    public partial class usp_RefundTransferReport_Result
    {
        public string ClaimOnLineCode { get; set; }
        public string AreaDetail { get; set; }
        public string BranchDetail { get; set; }
        public string ServiceByEmployeeId { get; set; }
        public string ServiceByEmployeeName { get; set; }
        public string CreateByEmployeeId { get; set; }
        public string CreateByEmployeeName { get; set; }
        public Nullable<double> Amount { get; set; }
        public string RefundCause { get; set; }
        public string Remark { get; set; }
        public Nullable<int> ClaimCount { get; set; }
        public Nullable<System.DateTime> ClaimOnLineCreateDate { get; set; }
        public Nullable<System.DateTime> ClaimOnLineTransferCreateDate { get; set; }
    }
}
