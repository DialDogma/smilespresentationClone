//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSUnderwrite.Models
{
    using System;
    
    public partial class usp_QueueLog_Select_Result
    {
        public int QueueLogId { get; set; }
        public Nullable<int> QueueId { get; set; }
        public Nullable<int> TransactionTypeId { get; set; }
        public Nullable<int> FromUserId { get; set; }
        public Nullable<int> ToUserId { get; set; }
        public Nullable<int> FromStatusId { get; set; }
        public Nullable<int> ToStatusId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
        public string CreatedByEmployeeCode { get; set; }
        public string CreatedByName { get; set; }
        public string TransactionType { get; set; }
        public string FromEmployeeCode { get; set; }
        public string FromName { get; set; }
        public string ToEmployeeCode { get; set; }
        public string ToName { get; set; }
        public string FromStatus { get; set; }
        public string ToStatus { get; set; }
        public Nullable<int> TotalCount { get; set; }
    }
}
