//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSCommissionModelDesigner.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class D_ClaimPAStudent
    {
        public int Id { get; set; }
        public Nullable<int> Period_Id { get; set; }
        public string ClaimGroup { get; set; }
        public string ClaimCode { get; set; }
        public string Status_Id { get; set; }
        public string Status { get; set; }
        public string App_Id { get; set; }
        public string CustomerDetail_Id { get; set; }
        public string CustName { get; set; }
        public Nullable<System.DateTime> DateHappen { get; set; }
        public Nullable<double> DateHappen_D { get; set; }
        public Nullable<double> DateHappen_M { get; set; }
        public Nullable<double> DateHappen_Y { get; set; }
        public Nullable<System.DateTime> DateNotice { get; set; }
        public Nullable<double> DateNotice_D { get; set; }
        public Nullable<double> DateNotice_M { get; set; }
        public Nullable<double> DateNotice_Y { get; set; }
        public Nullable<System.DateTime> DateIssue { get; set; }
        public Nullable<double> DateIssue_D { get; set; }
        public Nullable<double> DateIssue_M { get; set; }
        public Nullable<double> DateIssue_Y { get; set; }
        public string AdmitType_Id { get; set; }
        public string AdmitType { get; set; }
        public string ChiefComplain_Id { get; set; }
        public string ChiefComplain { get; set; }
        public string ICD10 { get; set; }
        public string ICD10_Detail { get; set; }
        public string AccidentCause_Id { get; set; }
        public string AccidentCause { get; set; }
        public string Detail { get; set; }
        public Nullable<double> Pay { get; set; }
        public Nullable<double> Compensate_net { get; set; }
        public Nullable<double> ToTal { get; set; }
        public string CreatedBy_Id { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<double> CreatedDate_D { get; set; }
        public Nullable<double> CreatedDate_M { get; set; }
        public Nullable<double> CreatedDate_Y { get; set; }
        public string ProductGroup_Id { get; set; }
        public string ProductGroup { get; set; }
        public string Product_Id { get; set; }
        public string Product { get; set; }
        public string Premium { get; set; }
        public string InsuranceCompany_id { get; set; }
        public string InsuranceCompany { get; set; }
        public string PayerName { get; set; }
        public string StartCoverDate { get; set; }
        public string StartCoverDate_D { get; set; }
        public string StartCoverDate_M { get; set; }
        public string StartCoverDate_Y { get; set; }
        public string PayerProvince_Id { get; set; }
        public string PayerProvince { get; set; }
        public string PayerAmphoe_Id { get; set; }
        public string PayerAmphoe { get; set; }
        public string PayerBranch_Id { get; set; }
        public string PayerBranch { get; set; }
        public string PayerStudyArea_Id { get; set; }
        public string PayerStudyArea { get; set; }
        public string ApprovedBy_Id { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public Nullable<double> ApprovedDate_D { get; set; }
        public Nullable<double> ApprovedDate_M { get; set; }
        public Nullable<double> ApprovedDate_Y { get; set; }
        public string Sale_Id1 { get; set; }
        public string SaleName { get; set; }
        public string ClaimPayBy_Id { get; set; }
        public string ClaimPayBy { get; set; }
    }
}
