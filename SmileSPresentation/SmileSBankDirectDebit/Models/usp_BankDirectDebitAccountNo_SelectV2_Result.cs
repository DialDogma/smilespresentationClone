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
    
    public partial class usp_BankDirectDebitAccountNo_SelectV2_Result
    {
        public string AccountNo { get; set; }
        public string PayerId { get; set; }
        public string PayerName { get; set; }
        public Nullable<int> Count { get; set; }
        public Nullable<int> BankDirectDebitStatusId { get; set; }
        public string BankDirectDebitStatus { get; set; }
        public int EmpBranchId { get; set; }
        public string EmpBranchCode { get; set; }
        public string EmpBranch { get; set; }
        public string BankId { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public Nullable<int> MappingStatusShow01Id { get; set; }
        public string MappingStatusShow01 { get; set; }
    }
}
