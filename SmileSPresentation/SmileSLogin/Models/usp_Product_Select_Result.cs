//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSLogin.Models
{
    using System;
    
    public partial class usp_Product_Select_Result
    {
        public int Product_ID { get; set; }
        public string ProductCode { get; set; }
        public string ProductDetail { get; set; }
        public Nullable<int> ProductType_ID { get; set; }
        public Nullable<int> InsuranceCompany_ID { get; set; }
        public Nullable<double> PremiumPer3Year { get; set; }
        public Nullable<double> PremiumPerYear { get; set; }
        public Nullable<double> PremiumPerMonth { get; set; }
        public Nullable<bool> IsPackage { get; set; }
        public Nullable<int> CreateBy_ID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string ProductTypeCode { get; set; }
        public string ProductTypeDetail { get; set; }
        public Nullable<int> ProductGroup_ID { get; set; }
        public string ProductGroupDetail { get; set; }
    }
}
