using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileUnderwriteBranch.Models
{
    public class StudyAreaDetail
    {
        public int StudyAreaId { get; set; }
        public string StudyAreaName { get; set; }
        public int StatusAll { get; set; }
        public int StatusComplete { get; set; }
        public int StatusOnProcess { get; set; }
        public int StatusOverdue { get; set; }
        public int StatusOverdueComplete { get; set; }
    }

    public class StudyAreaDetailCM
    {
        public int StudyAreaId { get; set; }
        public string StudyAreaName { get; set; }
        public int UserId { get; set; }
        public string DirectorFullName { get; set; }
        public int PHQueueAll { get; set; }
        public int PHQueueOnProcess { get; set; }
        public int PHQueueComplete { get; set; }
        public int PHQueueResultPass { get; set; }
        public int PHQueueResultNotPass { get; set; }
        public int PHQueueCMConsidered { get; set; }
        public int PHQueueCMOnProcess { get; set; }
        public int PHQueueCMApprove { get; set; }
        public int PHQueueCMNotApprove { get; set; }
    }

    /// <summary>
    /// Class object for Get_PHQueueStatusCountAll
    /// </summary>
    public class StatusDetail
    {
        public int PHQueueComplete { get; set; }
        public int PHQueueOnProcess { get; set; }
        public int PHQueueOverdue { get; set; }
        public int PHQueueOverdueComplete { get; set; }
        public int PHQueueCancel { get; set; }
        public int PHQueueDueComplete { get; set; }
        public int PHQueueNotYet { get; set; }
    }

    /// <summary>
    /// Class object for Get_StatusByStudyArea
    /// </summary>
    public class StatusDetailNotOverdue
    {
        public int PHQueueComplete { get; set; }
        public int PHQueueOnProcessAndOverdue { get; set; }
        public int PHQueueOverdueComplete { get; set; }
    }

    /// <summary>
    /// Class object for MonitorController > Get_PHQueueUnderwriteBranchResultCountAll_Select
    /// </summary>
    public class UnderwriteBranchResult
    {
        public int UnderwriteBranchResultTrue { get; set; }
        public int UnderwriteBranchResultFalse { get; set; }
        public int UnderwriteBranchResultNA { get; set; }
    }

    /// <summary>
    /// StatusDtail New /add 25/7/2562
    /// </summary>
    public class StatusDetailNew
    {
        public int PHQueueAll { get; set; }
        public int PHQueueOnProcess { get; set; }
        public int PHQueueResultPass { get; set; }
        public int PHQueueResultNotPass { get; set; }
    }

    public partial class Document
    {
        public string DocumentId { get; set; }
        public string DocumentFileId { get; set; }
        public int RunningNo { get; set; }
        public string DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; }
        public string PathThumbnailImg { get; set; }
        public string PathFullDoc { get; set; }
    }
}