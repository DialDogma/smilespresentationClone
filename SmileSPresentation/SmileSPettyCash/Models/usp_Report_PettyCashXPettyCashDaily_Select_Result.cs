//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSPettyCash.Models
{
    using System;
    
    public partial class usp_Report_PettyCashXPettyCashDaily_Select_Result
    {
        public int PettyCashXPettyCashDailyId { get; set; }
        public string PettyCashXPettyCashDailyCode { get; set; }
        public Nullable<int> PettyCashDailyId { get; set; }
        public Nullable<System.DateTime> PettyCashDailyDate { get; set; }
        public Nullable<int> PettyCashId { get; set; }
        public string PettyCashCode { get; set; }
        public string PettyCashName { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string BranchCode { get; set; }
        public string Branch { get; set; }
        public Nullable<decimal> PreviousBalance { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> RemainBalance { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
        public string CreatedByCode { get; set; }
        public string CreatedByName { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedByUserId { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> PettyCashXPettyCashDailyStatusId { get; set; }
        public string PettyCashXPettyCashDailyStatus { get; set; }
        public Nullable<int> TotalCount { get; set; }
    }
}
