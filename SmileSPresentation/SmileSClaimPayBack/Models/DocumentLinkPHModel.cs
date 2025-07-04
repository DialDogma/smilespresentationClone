using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSClaimPayBack.Models
{
    public class DocumentLinkPHModel
    {
        public int? No { get; set; }
        public string BillingDate { get; set; }
        public string DueDate { get; set; }
        public string Branch { get; set; }
        public string ClaimHeaderGroupCode { get; set; }
        public string PolicyNo { get; set; }
        public string App_id { get; set; }
        public string Product { get; set; }
        public string Province { get; set; }
        public string Code { get; set; }
        public string ZcardId { get; set; }
        public string CustomerName { get; set; }
        public string StartCoverDate { get; set; }
        public string ClaimAdmitType { get; set; }
        public string HospitalName { get; set; }
        public string ICD10_1Code { get; set; }
        public string ICD10 { get; set; }
        public string DateHappen { get; set; }
        public string DateIn { get; set; }
        public string DateOut { get; set; }
        public decimal? Net { get; set; }
        public decimal? Compensate_Include { get; set; }
        public decimal? Pay { get; set; }
        public decimal? Pay_Total { get; set; }
        public int? IPDCount { get; set; }
        public int? ICUCount { get; set; }
        public string ClaimType { get; set; }
        public string BillingRequestGroupCode { get; set; }
        public string BillingRequestItemCode { get; set; }
        public string DocumentLink { get; set; }
        public string PaymentReferenceID { get; set; }
        public string CoverAmount { get; set; }
        public string UncoverAmount { get; set; }
        public string UnCoverRemark { get; set; }
        public string DecisionStatus { get; set; }
        public string RejectResult { get; set; }
        public string DecisionDate { get; set; }
        public string EstimatePaymentDate { get; set; }
        public string Remark2 { get; set; }
        public string PaymentDate { get; set; }
        public string AmountPayment { get; set; }
        public string BankName { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }
        public string Remark3 { get; set; }

    }
}