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
    
    public partial class usp_ClaimPayBackTransfer_Update_Result
    {
        public Nullable<bool> IsResult { get; set; }
        public string Result { get; set; }
        public string Msg { get; set; }
        public System.Guid ClaimOnLineId { get; set; }
        public Nullable<System.Guid> ClaimOnLineItemId { get; set; }
        public string ClaimPayBackCode { get; set; }
        public string ClaimGroupCode { get; set; }
        public Nullable<int> ClaimPayBackXClaimId { get; set; }
        public string ClaimCode { get; set; }
        public Nullable<decimal> ClaimPay { get; set; }
        public int ReceiveTypeId { get; set; }
        public int TransferTypeId { get; set; }
        public Nullable<int> UpdatedByUserId { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}
