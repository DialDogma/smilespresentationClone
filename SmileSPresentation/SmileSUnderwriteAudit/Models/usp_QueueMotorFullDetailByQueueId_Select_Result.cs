//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSUnderwriteAudit.Models
{
    using System;
    
    public partial class usp_QueueMotorFullDetailByQueueId_Select_Result
    {
        public int QueueId { get; set; }
        public string QueueCode { get; set; }
        public Nullable<int> QueueStatusId { get; set; }
        public string QueueStatus { get; set; }
        public string ApplicationCode { get; set; }
        public string ApplicationStatus { get; set; }
        public Nullable<System.DateTime> StartCoverDate { get; set; }
        public Nullable<System.DateTime> EndCoverDate { get; set; }
        public Nullable<System.DateTime> IssuesPolicyDate { get; set; }
        public string InsuranceCompanyDetail { get; set; }
        public string ProductDetail { get; set; }
        public Nullable<double> Premium { get; set; }
        public string ClaimTypeDetail { get; set; }
        public string DeliverPolicyTo { get; set; }
        public string ZebraCarOwnerEmployeeCode { get; set; }
        public string Coverage1 { get; set; }
        public string Coverage2 { get; set; }
        public string Coverage3 { get; set; }
        public string Coverage4 { get; set; }
        public string Coverage5 { get; set; }
        public string VehicleBrandDetail { get; set; }
        public string VehicleModelDetail { get; set; }
        public string VehicleTypeDetail { get; set; }
        public string VehicleUseTypeDetail { get; set; }
        public string VehicleRegistrationYear { get; set; }
        public string VehicleRegistrationNumber { get; set; }
        public string VehicleChassisNumber { get; set; }
        public string VehicleEngineSize { get; set; }
        public Nullable<double> SumInsured { get; set; }
        public string VehicleIsFinacial { get; set; }
        public string VehicleIsCamera { get; set; }
        public string DriveLocation { get; set; }
        public string DriveDetail { get; set; }
        public string VehicleRemark { get; set; }
        public string InsuredTypeDetail { get; set; }
        public string InsuredCoperateCode { get; set; }
        public string InsuredCoperateTypeDetail { get; set; }
        public string InsuredTitle { get; set; }
        public string InsuredFirstName { get; set; }
        public string InsuredLastName { get; set; }
        public string InsuredIdentityCard { get; set; }
        public Nullable<System.DateTime> InsuredBirthDate { get; set; }
        public Nullable<int> InsuredAges { get; set; }
        public string InsuredPhone { get; set; }
        public string InsuredEmail { get; set; }
        public Nullable<int> InsuredDistrictId { get; set; }
        public string InsuredAddress { get; set; }
        public string InsuredOccupation { get; set; }
        public string InsuredOccupationLevel { get; set; }
        public string InsuredOccupationDetail { get; set; }
        public string InsuredOfficeName { get; set; }
        public string PayerTitle { get; set; }
        public string PayerFirstName { get; set; }
        public string PayerLastName { get; set; }
        public string PayerIdentityCard { get; set; }
        public Nullable<System.DateTime> PayerBirthDate { get; set; }
        public Nullable<int> PayerAges { get; set; }
        public string PayerPhone { get; set; }
        public string PayerEmail { get; set; }
        public string PayerDistrictId { get; set; }
        public string PayerAddress { get; set; }
        public string PayerOccupation { get; set; }
        public string PayerOccupationLevel { get; set; }
        public string PayerOccupationDetail { get; set; }
        public string PayerOfficeName { get; set; }
        public Nullable<int> PayerBranchId { get; set; }
        public Nullable<int> PayerStudyAreaId { get; set; }
        public string DriverTitle { get; set; }
        public string DriverFirstName { get; set; }
        public string DriverLastName { get; set; }
        public Nullable<System.DateTime> DriverBirthDate { get; set; }
        public Nullable<int> DriverAges { get; set; }
        public string DriverPhone { get; set; }
        public string DriverOccupation { get; set; }
        public string DriverOccupationLevel { get; set; }
        public string DriverOccupationDetail { get; set; }
        public string SaleEmployeeCode { get; set; }
        public string SaleEmployeeName { get; set; }
        public string SaleEmployeeBranch { get; set; }
        public Nullable<int> AssignToUserId { get; set; }
        public string AssignToDirectorName { get; set; }
        public Nullable<System.DateTime> AssignDate { get; set; }
        public Nullable<System.DateTime> CloseDate { get; set; }
        public Nullable<System.DateTime> QueueExpireDate { get; set; }
        public Nullable<int> ApproveStatusId { get; set; }
        public string ApproveStatus { get; set; }
        public Nullable<int> ApproveByUserId { get; set; }
        public string ApproveByChairmanName { get; set; }
        public string ApproveRemark { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public Nullable<bool> QueueIsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedByUserId { get; set; }
        public Nullable<int> DurationDay { get; set; }
        public Nullable<int> QueueResultId { get; set; }
        public Nullable<bool> IsUnderwrite { get; set; }
        public Nullable<bool> IsNotFoundCustomer { get; set; }
        public Nullable<int> UnderwriteTypeId { get; set; }
        public string UnderwriteType { get; set; }
        public Nullable<int> CallStatusId { get; set; }
        public string CallStatus { get; set; }
        public string CallCauseRemark { get; set; }
        public Nullable<bool> IsUnderwriteInsured { get; set; }
        public Nullable<bool> IsUnderwritePayer { get; set; }
        public Nullable<int> UnderwritePaymentTypeId { get; set; }
        public string UnderwritePaymentTypeDetail { get; set; }
        public Nullable<bool> IsUnderwritePass { get; set; }
        public Nullable<bool> IsUnderwritePassStandard { get; set; }
        public Nullable<bool> IsUnderwritePassCondition { get; set; }
        public Nullable<bool> IsUnderwritePassIsSpecify { get; set; }
        public string UnderwritePassSpecifyRemark { get; set; }
        public Nullable<bool> IsUnderwriteUnPassVehicleSpec { get; set; }
        public Nullable<bool> IsUnderwriteUnPassVehicleUseType { get; set; }
        public Nullable<bool> IsUnderwriteUnPassCantPay { get; set; }
        public Nullable<bool> IsUnderwriteUnPassCantContact { get; set; }
        public Nullable<bool> IsUnderwriteUnPassOther { get; set; }
        public string UnderwriteUnPassOtherRemark { get; set; }
        public string QueueResultRemark { get; set; }
        public Nullable<bool> QueueResultIsActive { get; set; }
        public Nullable<System.DateTime> QueueResultCreatedDate { get; set; }
        public Nullable<int> QueueResultCreatedByUserId { get; set; }
        public Nullable<System.DateTime> QueueResultUpdatedDate { get; set; }
        public Nullable<int> QueueResultUpdatedByUserId { get; set; }
        public Nullable<bool> ApproveIsUnderwritePassCondition { get; set; }
        public Nullable<bool> ApproveIsUnderwritePassIsSpecify { get; set; }
        public string ApproveUnderwritePassSpecifyRemark { get; set; }
        public Nullable<bool> ApproveIsUnderwriteUnPassVehicleSpec { get; set; }
        public Nullable<bool> ApproveIsUnderwriteUnPassVehicleUseType { get; set; }
        public Nullable<bool> ApproveIsUnderwriteUnPassCantPay { get; set; }
        public Nullable<bool> ApproveIsUnderwriteUnPassCantContact { get; set; }
        public Nullable<bool> ApproveIsUnderwriteUnPassOther { get; set; }
        public string ApproveUnderwriteUnPassOtherRemark { get; set; }
        public Nullable<int> QueuePoliciesId { get; set; }
        public Nullable<int> GiverUserId { get; set; }
        public string GiverEmployeeName { get; set; }
        public Nullable<System.DateTime> GiveDate { get; set; }
        public string URLPath { get; set; }
        public string PhysicalPath { get; set; }
        public string FileName { get; set; }
        public Nullable<int> GiveTypeId { get; set; }
        public string GiveType { get; set; }
    }
}
