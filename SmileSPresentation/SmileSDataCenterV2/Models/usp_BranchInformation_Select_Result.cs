//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSDataCenterV2.Models
{
    using System;
    
    public partial class usp_BranchInformation_Select_Result
    {
        public int BranchInformation_ID { get; set; }
        public string BranchDetail { get; set; }
        public string ManagerName { get; set; }
        public string ChairmanName { get; set; }
        public string DirectorName { get; set; }
        public string BusinessPromoteTeamName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> TotalCount { get; set; }
    }
}
