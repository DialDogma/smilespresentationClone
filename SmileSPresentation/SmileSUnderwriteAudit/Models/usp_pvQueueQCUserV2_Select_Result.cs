//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSUnderwriteAudit.Models
{
    using System;
    
    public partial class usp_pvQueueQCUserV2_Select_Result
    {
        public string PersonName { get; set; }
        public int QueueAllCount { get; set; }
        public int QueuePendingCount { get; set; }
        public int QueueCompleteCount { get; set; }
        public Nullable<int> TotalCount { get; set; }
    }
}
