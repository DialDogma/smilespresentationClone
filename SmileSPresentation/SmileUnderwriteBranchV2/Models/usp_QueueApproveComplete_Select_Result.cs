//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileUnderwriteBranchV2.Models
{
    using System;
    
    public partial class usp_QueueApproveComplete_Select_Result
    {
        public int QueueId { get; set; }
        public string ApplicationCode { get; set; }
        public string CustomerName { get; set; }
        public string PayerName { get; set; }
        public string DirectorName { get; set; }
        public string BranchDetail { get; set; }
        public Nullable<bool> IsUnderwritePass { get; set; }
        public Nullable<int> ApproveStatusId { get; set; }
        public string ApproveStatus { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public string UnderwriterName { get; set; }
    }
}
