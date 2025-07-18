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
    
    public partial class usp_ClaimOnLineReserveBranchDetailPA_Select_Result
    {
        public string ClaimOnlineCode { get; set; }
        public string ClaimDetail { get; set; }
        public string ProductType { get; set; }
        public Nullable<int> ProductGroup_ID { get; set; }
        public string ProductGroup { get; set; }
        public string ClaimCode { get; set; }
        public string CustomerName { get; set; }
        public string School { get; set; }
        public string Branch { get; set; }
        public string Area { get; set; }
        public Nullable<double> ClaimAmount { get; set; }
        public Nullable<System.DateTime> TransferDateLatest { get; set; }
        public Nullable<double> ReturnAmountTotal { get; set; }
        public Nullable<System.DateTime> ReturnDateLatest { get; set; }
        public Nullable<System.DateTime> minTransferDate { get; set; }
        public string TransferRemark { get; set; }
        public Nullable<int> TransferByEmployeeId { get; set; }
        public string TransferByEmployeeCode { get; set; }
        public string TransferByEmployeeName { get; set; }
        public string PayeeType { get; set; }
        public Nullable<int> PayeerBankId { get; set; }
        public string PayeeBank { get; set; }
        public string PayeeAccountNo { get; set; }
        public string PayeeAccountName { get; set; }
        public string ServiceByEmployeeCode { get; set; }
        public Nullable<int> ServiceByUserId { get; set; }
        public string ServiceByEmployeeName { get; set; }
        public string ZebraCarOwnerByEmployeeId { get; set; }
        public string ZebraCarOwnerByEmployeeName { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
        public string CreatedByEmployeeCode { get; set; }
        public string CreatedEmployeeName { get; set; }
        public Nullable<System.DateTime> ClaimCreatedDate { get; set; }
    }
}
