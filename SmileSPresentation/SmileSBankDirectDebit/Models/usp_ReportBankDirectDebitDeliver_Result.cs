//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSBankDirectDebit.Models
{
    using System;
    
    public partial class usp_ReportBankDirectDebitDeliver_Result
    {
        public string BankDirectDebitHeaderCode { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string Branch { get; set; }
        public string BankId { get; set; }
        public string Bank { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string Ref1 { get; set; }
        public int DeliverStatusId { get; set; }
        public string DeliverStatus { get; set; }
        public string Register { get; set; }
        public Nullable<System.DateTime> ApplyDate { get; set; }
        public Nullable<int> StatusCode { get; set; }
        public Nullable<int> Status { get; set; }
        public string CauseText { get; set; }
        public int BankDirectDebitDetailId { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public string StatusDisplay { get; set; }
    }
}
