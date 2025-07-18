//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSPettyCash.Models
{
    using System;
    
    public partial class usp_RequisitionDetailById_Select_Result
    {
        public int RequisitionDetailId { get; set; }
        public Nullable<int> RequisitionHeaderId { get; set; }
        public string RequisitionHeaderCode { get; set; }
        public int PettyCashTransactionId { get; set; }
        public string Remark { get; set; }
        public Nullable<int> RequisitionDetailStatusId { get; set; }
        public string RequisitionDetailStatus { get; set; }
        public Nullable<int> RejectRequisitionCauseId { get; set; }
        public string RejectRequisitionCause { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedByUserId { get; set; }
        public int PettyCashTransactionGroupId { get; set; }
        public string PettyCashTransactionGroup { get; set; }
        public int PettyCashTransactionTypeId { get; set; }
        public string PettyCashTransactionType { get; set; }
        public string PettyCashTransactionTypeDescription { get; set; }
        public System.DateTime BillDate { get; set; }
        public string BillBook { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> OrganizeCompanyId { get; set; }
        public string OrganizeCompany { get; set; }
        public Nullable<int> PettyCashTransactionCreatedByUserId { get; set; }
        public string PettyCashTransactionCreatedByCode { get; set; }
        public string PettyCashTransactionCreatedByName { get; set; }
        public string PettyCashXPettyCashDailyCode { get; set; }
        public Nullable<int> pxcdCreatedByUserId { get; set; }
        public string pxcdCreatedByCode { get; set; }
        public string pxcdCreatedByName { get; set; }
        public Nullable<System.DateTime> pxcdCreatedDate { get; set; }
        public Nullable<int> pxcdUpdatedByUserId { get; set; }
        public Nullable<System.DateTime> pxcdUpdatedDate { get; set; }
        public Nullable<int> pxcdClosedByUserId { get; set; }
        public string pxcdClosedByCode { get; set; }
        public string pxcdClosedByName { get; set; }
        public Nullable<System.DateTime> pxcdClosedDate { get; set; }
        public Nullable<int> PettyCashXPettyCashDailyStatusId { get; set; }
        public string PettyCashXPettyCashDailyStatus { get; set; }
        public Nullable<int> PaymentId { get; set; }
        public Nullable<int> PaymentTypeId { get; set; }
        public string PaymentType { get; set; }
        public Nullable<int> PaymentBankId { get; set; }
        public string PaymentBank { get; set; }
        public string PaymentBankAccountName { get; set; }
        public string PaymentBankAccountNo { get; set; }
        public Nullable<decimal> PaymentAmount { get; set; }
        public Nullable<int> DocumentId { get; set; }
        public string DocumentCode { get; set; }
        public int DocumentFileCount { get; set; }
        public string UrlLinkOpenSSSDoc { get; set; }
        public string RequisitionDetailRemark { get; set; }
        public string PettyCashTransactionCode { get; set; }
    }
}
