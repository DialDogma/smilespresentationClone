//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UnderwriteCancellation.Models
{
    using System;
    
    public partial class usp_QueueReport1_Select_Result
    {
        public int QueueId { get; set; }
        public Nullable<System.DateTime> Period { get; set; }
        public string PayerName { get; set; }
        public string PayerIdCardNo { get; set; }
        public string PayerPhone { get; set; }
        public string PayerBuildingName { get; set; }
        public Nullable<int> QueueDetailAllCount { get; set; }
        public string CallStatusDetail { get; set; }
        public string CallRemark { get; set; }
        public int CallStatusId3 { get; set; }
        public int CallStatusId6 { get; set; }
        public int CallStatusId4 { get; set; }
        public int CallStatusId5 { get; set; }
        public int CallStatusCount { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeePersonName { get; set; }
        public string UpdatedDate { get; set; }
        public string QueueStatusDetail { get; set; }
        public Nullable<bool> IsIssue { get; set; }
        public string IssueRemark { get; set; }
        public string CancelCauseFullDetail { get; set; }
        public string CancelCauseRemark { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public string PayerBranchDetail { get; set; }
        public string PayerAreaDetail { get; set; }
        public string UpdatedTime { get; set; }
    }
}
