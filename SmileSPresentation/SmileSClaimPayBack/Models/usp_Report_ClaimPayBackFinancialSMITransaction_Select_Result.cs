//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSClaimPayBack.Models
{
    using System;
    
    public partial class usp_Report_ClaimPayBackFinancialSMITransaction_Select_Result
    {
        public string InsuranceCompany_Name { get; set; }
        public string Branch { get; set; }
        public string Hospital { get; set; }
        public string ProductGroupDetailName { get; set; }
        public string ClaimGroupType { get; set; }
        public string ClaimGroupCode { get; set; }
        public Nullable<int> ItemCount { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> ClaimCompensate { get; set; }
        public Nullable<int> ClaimNo { get; set; }
        public string COL { get; set; }
        public string Province { get; set; }
        public Nullable<int> CustomerName { get; set; }
        public string BankName { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNo { get; set; }
        public Nullable<int> PhoneNo { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ApprovedUser { get; set; }
        public string CteatedUser { get; set; }
        public string ClaimAdmitType { get; set; }
        public Nullable<System.DateTime> SMIPaid { get; set; }
    }
}
