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
    
    public partial class usp_PersonnelApplicationTransactionHeader_Select_Result
    {
        public int PersonnelApplicationTransactionHeaderId { get; set; }
        public string PersonnelApplicationTransactionHeaderCode { get; set; }
        public Nullable<int> PersonnelApplicationId { get; set; }
        public string PersonnelApplicationCode { get; set; }
        public Nullable<int> TransactionTypeId { get; set; }
        public string TransactionType { get; set; }
        public string Detail { get; set; }
        public string Remark { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
        public string CreatedByCode { get; set; }
        public string CreatedByName { get; set; }
        public Nullable<int> CreatedByBranchId { get; set; }
        public string CreatedByBranch { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public Nullable<int> TotalCount { get; set; }
    }
}
