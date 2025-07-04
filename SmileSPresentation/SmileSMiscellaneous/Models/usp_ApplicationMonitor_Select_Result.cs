//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSMiscellaneous.Models
{
    using System;
    
    public partial class usp_ApplicationMonitor_Select_Result
    {
        public int ApplicationId { get; set; }
        public string ApplicationCode { get; set; }
        public Nullable<int> ProductTypeID { get; set; }
        public string ProductType { get; set; }
        public int Product_ID { get; set; }
        public string ProductDetail { get; set; }
        public Nullable<int> CountCustomer { get; set; }
        public Nullable<double> SumPremiumAmount { get; set; }
        public Nullable<System.DateTime> StartCoverDate { get; set; }
        public Nullable<System.DateTime> EndCoverDate { get; set; }
        public Nullable<int> CoveragePeriod { get; set; }
        public Nullable<int> PeriodTypeId { get; set; }
        public string PeriodType { get; set; }
        public string PersonTypeDetail { get; set; }
        public string TitleDetail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string InsuredName { get; set; }
        public string FullAgentName { get; set; }
        public string AgentCode { get; set; }
        public string AgentName { get; set; }
        public string Zebra { get; set; }
        public Nullable<int> ApplicationStatusId { get; set; }
        public string ApplicationStatus { get; set; }
        public Nullable<int> PaymentStatusId { get; set; }
        public string PaymentStatus { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> TotalCount { get; set; }
    }
}
