//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UnderwriteCancellation.Models
{
    using System;
    
    public partial class usp_QueueDetail_Select_Result
    {
        public int QueueDetailId { get; set; }
        public string ApplicationCode { get; set; }
        public string ProductGroup { get; set; }
        public string Product { get; set; }
        public string AppStatus { get; set; }
        public string InsuredName { get; set; }
        public string PayerRelate { get; set; }
        public string QueueDetailRemark { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public Nullable<bool> IsAudit { get; set; }
    }
}
