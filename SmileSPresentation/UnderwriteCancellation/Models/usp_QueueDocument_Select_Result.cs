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
    
    public partial class usp_QueueDocument_Select_Result
    {
        public int DocumentId { get; set; }
        public string DocumentCode { get; set; }
        public string DocumentTypeDetail { get; set; }
        public int FileCount { get; set; }
        public string UrlLinkScan { get; set; }
        public string UrlLinkOpen { get; set; }
        public Nullable<int> TotalCount { get; set; }
    }
}
