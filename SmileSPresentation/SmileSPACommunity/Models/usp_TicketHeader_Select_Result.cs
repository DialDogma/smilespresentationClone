//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSPACommunity.Models
{
    using System;
    
    public partial class usp_TicketHeader_Select_Result
    {
        public int ApplicationId { get; set; }
        public string ApplicationCode { get; set; }
        public string ApplicationName { get; set; }
        public string PolicyNo { get; set; }
        public Nullable<System.DateTime> ApplicationStartCoverDate { get; set; }
        public Nullable<System.DateTime> ApplicationEndCoverDate { get; set; }
        public Nullable<int> ApplicationStatusId { get; set; }
        public string ApplicationStatus { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string Product { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string Branch { get; set; }
        public string SubDistrictID { get; set; }
        public string SubDistrict { get; set; }
        public string DistrictID { get; set; }
        public string District { get; set; }
        public string ProvinceID { get; set; }
        public string Province { get; set; }
        public Nullable<int> TotalCount { get; set; }
    }
}
