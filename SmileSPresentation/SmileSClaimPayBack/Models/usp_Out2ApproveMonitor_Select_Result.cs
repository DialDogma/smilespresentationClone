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
    
    public partial class usp_Out2ApproveMonitor_Select_Result
    {
        public string tempCode { get; set; }
        public string billingRequestGroup { get; set; }
        public string insuranceName { get; set; }
        public Nullable<int> amount { get; set; }
        public Nullable<decimal> amountTotal { get; set; }
        public string StatusName { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string ClaimType { get; set; }
        public Nullable<System.DateTime> BillingDate { get; set; }
        public Nullable<System.DateTime> ReceiveDate { get; set; }
    }
}
