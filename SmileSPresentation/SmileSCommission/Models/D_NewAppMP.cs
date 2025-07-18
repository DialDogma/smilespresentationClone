//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSCommission.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class D_NewAppMP
    {
        public int Id { get; set; }
        public Nullable<int> Period_Id { get; set; }
        public string App_Id { get; set; }
        public string Status { get; set; }
        public string ContractTypeDetail { get; set; }
        public Nullable<System.DateTime> StartCover { get; set; }
        public Nullable<int> StartCover_D { get; set; }
        public Nullable<int> StartCover_M { get; set; }
        public Nullable<int> StartCover_Y { get; set; }
        public Nullable<System.DateTime> EndCover { get; set; }
        public Nullable<int> EndCover_D { get; set; }
        public Nullable<int> EndCover_M { get; set; }
        public Nullable<int> EndCover_Y { get; set; }
        public string ProductType_Id { get; set; }
        public string ProductTypeDetail { get; set; }
        public string Product_Id { get; set; }
        public string ProductDetail { get; set; }
        public Nullable<int> PremiumPerMonth { get; set; }
        public Nullable<int> PremiumPerYear { get; set; }
        public Nullable<int> PeriodType_ID { get; set; }
        public string PeriodTypeDetail { get; set; }
        public string CustName { get; set; }
        public string PayerName { get; set; }
        public string PayerDistrictID { get; set; }
        public string PayerDistrict { get; set; }
        public string PayerProvinceID { get; set; }
        public string PayerProvince { get; set; }
        public string PayerBranch_Id { get; set; }
        public string PayerBranch { get; set; }
        public string PayerStudyArea_Id { get; set; }
        public string PayerStudyArea { get; set; }
        public string Sale1_Employee_Code { get; set; }
        public string SaleName { get; set; }
        public string Sale1Branch_ID { get; set; }
        public string Sale1_BranchDetail { get; set; }
        public string VehicleRegistrationDetail { get; set; }
        public string VehicleRegistrationNumber { get; set; }
        public string VehicleRegistrationProvince { get; set; }
        public string CreateBy_Employee_Code { get; set; }
        public string ApproveBy_Employee_Code { get; set; }
        public string SaleID2 { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedDate_D { get; set; }
        public Nullable<int> CreatedDate_M { get; set; }
        public Nullable<int> CreatedDate_Y { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public Nullable<int> ApproveDate_D { get; set; }
        public Nullable<int> ApproveDate_M { get; set; }
        public Nullable<int> ApproveDate_Y { get; set; }
        public string SaleID_Service { get; set; }
        public string SaleName_Service { get; set; }
        public string SaleID_NewAppShare { get; set; }
        public string SaleName_NewAppShare { get; set; }
        public string ZebraCar_Code { get; set; }
        public string ZebraCar_No { get; set; }
        public string ZebraCarOwner_Id { get; set; }
    }
}
