namespace UnderwriteCancellation.Models
{
    public class UnderwriteCancelDto_Insert
    {
        public int? QueueId { get; set; }
        public bool? IsAudit { get; set; } = false;
        public int? CallStatusId { get; set; }
        public string CallRemark { get; set; } = null;
        public bool? IsIssue { get; set; } = false;
        public string IssueRemark { get; set; } = null;
        public bool? IsCancelCause1 { get; set; } = false;
        public bool? IsCancelCause2 { get; set; } = false;
        public bool? IsCancelCause3 { get; set; } = false;
        public bool? IsCancelCause4 { get; set; } = false;
        public bool? IsCancelCause5 { get; set; } = false;
        public bool? IsCancelCause6 { get; set; } = false;
        public bool? IsCancelCause7 { get; set; } = false;
        public bool? IsCancelCause8 { get; set; } = false;
        public bool? IsCancelCause9 { get; set; } = false;
        public bool? IsCancelCause10 { get; set; } = false;
        public bool? IsCancelCause11 { get; set; } = false;
        public bool? IsCancelCause12 { get; set; } = false;
        public bool? IsCancelCause13 { get; set; } = false;
        public bool? IsCancelCause14 { get; set; } = false;
        public bool? IsCancelCause15 { get; set; } = false;
        public bool? IsCancelCause16 { get; set; } = false;
        public bool? IsCancelCause17 { get; set; } = false;
        public string CancelCauseRemark { get; set; } = null;
        public int? CreatedByUserId { get; set; }
    }
}