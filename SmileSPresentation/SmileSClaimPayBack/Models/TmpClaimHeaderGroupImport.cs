//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSClaimPayBack.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TmpClaimHeaderGroupImport
    {
        public int TmpClaimHeaderGroupImportId { get; set; }
        public string TmpCode { get; set; }
        public string ClaimHeaderGroupCode { get; set; }
        public Nullable<int> ItemCount { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<System.DateTime> BillingDate { get; set; }
        public Nullable<bool> IsValid { get; set; }
        public string ValidateResult { get; set; }
        public Nullable<int> InsuranceCompanyId { get; set; }
        public Nullable<int> ClaimHeaderGroupTypeId { get; set; }
        public string ClaimTypeCode { get; set; }
    }
}
