using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSClaimOnLine.DTOs
{
    public class GetClaimOnLineStatus5ResponseDto
    {
        public int ClaimOnLinePayAuto_TransactionId { get; set; }
        public string ClaimOnLinePayAuto_TransactionCode { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> PayAutoTypeId { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public Nullable<decimal> PaymentAmount { get; set; }
        public Nullable<int> ToBankId { get; set; }
        public string ToAccountNo { get; set; }
        public string ToAccountName { get; set; }
        public string TransRefNo { get; set; }
        public string PayTransferResponse { get; set; }
        public Nullable<int> ClaimOnLineId { get; set; }
        public Nullable<System.Guid> PayListHeaderId { get; set; }
        public Nullable<int> ClaimOnLineTransferId { get; set; }
        public string PremiumSourceStatusId { get; set; }
        public string ClaimOnLineCode { get; set; }
        public string SMSPhoneNo { get; set; }
        public string SMSMessageSurvey { get; set; }
        public Nullable<int> TransferFromMenu { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public string ToBankName { get; set; }
        public int? PayListStatusId { get; set; }
        public string PayListStatusName { get; set; }
        public decimal Amount { get; set; }
    }
}