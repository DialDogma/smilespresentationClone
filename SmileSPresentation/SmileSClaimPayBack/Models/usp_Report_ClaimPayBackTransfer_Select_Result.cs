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
    
    public partial class usp_Report_ClaimPayBackTransfer_Select_Result
    {
        public Nullable<int> ClaimPayBackDetailId { get; set; }
        public string ClaimPayBackDetailCode { get; set; }
        public Nullable<int> ClaimPayBackId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string Branch { get; set; }
        public string Area { get; set; }
        public string ClaimGroupCode { get; set; }
        public string ClaimOnLineCode { get; set; }
        public Nullable<int> ItemCount { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string ClaimGroupType { get; set; }
        public Nullable<int> ProductGroupId { get; set; }
        public string ProductGroupDetail { get; set; }
        public Nullable<int> InsuranceCompanyId { get; set; }
        public string InsuranceCompany { get; set; }
        public string empClaimOnLineApprove { get; set; }
        public Nullable<System.DateTime> TransferDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> TransferUpdatedDate { get; set; }
        public Nullable<int> UpdatedByUserId { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string HospitalProvince { get; set; }
        public string HospitalName { get; set; }
        public string ClaimCompensateCode { get; set; }
        public string ClaimCode { get; set; }
        public string CustName { get; set; }
        public string Bank { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNo { get; set; }
        public string empClaimApproved { get; set; }
        public string empClaimPayBackCreated { get; set; }
        public string PhoneNo { get; set; }
    }
}
