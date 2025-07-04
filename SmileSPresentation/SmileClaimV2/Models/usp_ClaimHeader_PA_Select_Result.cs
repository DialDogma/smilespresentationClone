//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileClaimV2.Models
{
    using System;
    
    public partial class usp_ClaimHeader_PA_Select_Result
    {
        public string Code { get; set; }
        public string CustomerDetail_id { get; set; }
        public string Application_id { get; set; }
        public string School_id { get; set; }
        public string School { get; set; }
        public string FullName { get; set; }
        public string Status_id { get; set; }
        public string Customer_status { get; set; }
        public string Hospital_id { get; set; }
        public string Hospital_Detail { get; set; }
        public string ClaimType_id { get; set; }
        public string ClaimType_Detail { get; set; }
        public string ClaimStyle_id { get; set; }
        public string ClaimStyle_Detail { get; set; }
        public Nullable<System.DateTime> DateHappen { get; set; }
        public Nullable<System.DateTime> DateIn { get; set; }
        public Nullable<System.DateTime> DateOut { get; set; }
        public Nullable<System.DateTime> DateNotice { get; set; }
        public string Product_id { get; set; }
        public string Product { get; set; }
        public string InsuranceCompany_id { get; set; }
        public string InsuranceCompany { get; set; }
        public string ChiefComplain_id { get; set; }
        public string ChiefComplain_Detail { get; set; }
        public string ICD10_1 { get; set; }
        public string ICD10_1_Detail { get; set; }
        public string Remark { get; set; }
        public string Claim_Status_id { get; set; }
        public string Claim_Status_Detail { get; set; }
        public string CreatedBy_id { get; set; }
        public string CreatedBy_Detail { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsExgratia { get; set; }
        public string DenyCause_id { get; set; }
        public string DenyCause_Detail { get; set; }
        public Nullable<double> Amount_Total { get; set; }
        public Nullable<double> Amount_Pay { get; set; }
        public Nullable<double> Amount_UnPay { get; set; }
        public Nullable<double> Amount_Compensate_in { get; set; }
        public Nullable<double> Amount_Compensate_out { get; set; }
        public Nullable<double> Amount_Compensate { get; set; }
        public Nullable<double> Amount_Net { get; set; }
        public string Unpay_Remark { get; set; }
        public Nullable<bool> IsContinueClaim { get; set; }
        public string ContinueFromClaim_id { get; set; }
        public string AccidentCause_id { get; set; }
        public string AccidentDetail { get; set; }
        public string AccidentCause { get; set; }
        public string ContinueFromDetail { get; set; }
        public Nullable<int> TotalCount { get; set; }
    }
}
