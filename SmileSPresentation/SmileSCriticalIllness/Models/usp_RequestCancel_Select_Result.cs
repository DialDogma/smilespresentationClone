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
    
    public partial class usp_RequestCancel_Select_Result
    {
        public int RequestCancelId { get; set; }
        public string PolicyNo { get; set; }
        public Nullable<int> ApplicationId { get; set; }
        public string ApplicationCode { get; set; }
        public Nullable<System.DateTime> StartCoverDate { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string Detail1 { get; set; }
        public string Detail2 { get; set; }
        public string Detail3 { get; set; }
        public string Detail4 { get; set; }
        public Nullable<System.DateTime> CancelDate { get; set; }
        public Nullable<int> CancelStatusId { get; set; }
        public string CancelStatus { get; set; }
        public Nullable<int> CancelCauseId { get; set; }
        public string CancelCause { get; set; }
        public string CancelRemark { get; set; }
        public Nullable<int> ApproveStatusId { get; set; }
        public string ApproveStatus { get; set; }
        public string DocumentCode { get; set; }
        public int DocumentFileCount { get; set; }
        public string UrlScan { get; set; }
        public string UrlOpen { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public string Detail5 { get; set; }
    }
}
