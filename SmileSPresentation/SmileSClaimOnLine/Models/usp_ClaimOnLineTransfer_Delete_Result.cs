//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSClaimOnLine.Models
{
    using System;
    
    public partial class usp_ClaimOnLineTransfer_Delete_Result
    {
        public int ClaimOnLineAccountId { get; set; }
        public Nullable<int> BankId { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public Nullable<double> Balance { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreateByUserId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateByUserId { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
