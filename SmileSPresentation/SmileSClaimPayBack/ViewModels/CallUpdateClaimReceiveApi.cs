using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSClaimPayBack.ViewModels
{
    public class CallUpdateClaimReceiveApi
    {
        public Guid claimPaybackPaymentHeaderId { get; set; }
        public Guid? claimOnLineId { get; set; }
        public int receiveTypeId { get; set; }
        public int transferTypeId { get; set; }
        public string remark { get; set; }
        public bool isActive { get; set; }
        public int createdByUserId { get; set; }
        public DateTime createdDate { get; set; }
        public int updatedByUserId { get; set; }
        public DateTime updatedDate { get; set; }
        public List<ClaimReceiveItem> claimReceiveItem { get; set; }
    }

    public class ClaimReceiveItem
    {
        public Guid claimReceiveGroupId { get; set; }
        public Guid claimPaybackPaymentDetailId { get; set; }
        public Nullable<System.Guid> claimOnlineItemId { get; set; }
        public Nullable<System.Guid> nplDetailId { get; set; }
        public string claimCode { get; set; }
        public decimal amount { get; set; }
        public bool isActive { get; set; }
        public int createdByUserId { get; set; }
        public DateTime createdDate { get; set; }
        public int updatedByUserId { get; set; }
        public DateTime updatedDate { get; set; }
    }

    public class ApproveMonitor
    {
        public string tempCode { get; set; }
        public string billingRequestGroup { get; set; }
        public string insuranceName { get; set; }
        public int amount { get; set; }
        public string amountTotal { get; set; }
        public string claimType { get; set; }
        public string statusName { get; set; }
        public DateTime? receiveDate { get; set; }
        public DateTime billingDate { get; set; }
        public int StatusId { get; set; }

    }

    public class ApproveMonitorDetail
    {
        public int TmpBillingRequestResultId { get; set; }
        public string BillingRequestItem { get; set; }
        public string ClaimHeaderGroupCode { get; set; }
        public string ClaimCode { get; set; }
        public string PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string AccountDetail { get; set; }
        public string Remark { get; set; }
        public string ReasonDetail { get; set; }
        public int StatusId { get; set; }

    }


    public class ApproveDetailForm
    {
        public bool isAll { get; set; }
        public int[] id { get; set; }
        public string[] reasonMaster { get; set; }
        public string tempCode { get; set; }
        public string BillingRequestGroupCode { get; set; }
    }

    public class ReasonMasters
    {
        public string[] reasonMaster { get; set; }
    }

}