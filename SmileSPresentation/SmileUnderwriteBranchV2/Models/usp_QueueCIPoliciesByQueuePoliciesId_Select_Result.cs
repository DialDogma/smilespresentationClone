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
    
    public partial class usp_QueueCIPoliciesByQueuePoliciesId_Select_Result
    {
        public Nullable<int> QueueId { get; set; }
        public Nullable<System.DateTime> StartCoverDate { get; set; }
        public Nullable<System.DateTime> QueueCreatedDate { get; set; }
        public int QueuePoliciesId { get; set; }
        public Nullable<System.DateTime> GiveDate { get; set; }
        public Nullable<int> GiveTypeId { get; set; }
        public Nullable<int> GiverUserId { get; set; }
        public string GiverName { get; set; }
        public string URLPath { get; set; }
        public string PhysicalPath { get; set; }
        public string FileName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreaetdByUserId { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedByUserId { get; set; }
        public string TrackingNo { get; set; }
        public string GiveTypeRemark { get; set; }
    }
}
