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
    using System.Collections.Generic;
    
    public partial class TmpClaimOnLinePayAuto_Transaction
    {
        public int TmpClaimOnLinePayAuto_TransactionId { get; set; }
        public string ClaimOnLinePayAuto_TransactionCode { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> PayAutoTypeId { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public Nullable<decimal> PaymentAmount { get; set; }
        public string TransRefNo { get; set; }
        public string PayTransferResponse { get; set; }
        public Nullable<int> ClaimOnLineId { get; set; }
        public Nullable<System.Guid> PayListHeaderId { get; set; }
        public Nullable<int> ClaimOnLineTransferId { get; set; }
        public Nullable<int> PremiumSourceStatusId { get; set; }
    }
}
