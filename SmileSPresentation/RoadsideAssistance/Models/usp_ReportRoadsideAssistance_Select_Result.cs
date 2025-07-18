//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RoadsideAssistance.Models
{
    using System;
    
    public partial class usp_ReportRoadsideAssistance_Select_Result
    {
        public int RoadsideAssistanceId { get; set; }
        public string RoadsideAssistanceCode { get; set; }
        public string ContactNumber { get; set; }
        public Nullable<int> RoadsideAssistanceStatusId { get; set; }
        public string RoadsideAssistanceStatus { get; set; }
        public string VehicleRegistrationDetail { get; set; }
        public string VehicleRegistrationNumber { get; set; }
        public Nullable<int> VehicleRegistrationProvinceId { get; set; }
        public string VehicleRegistrationProvince { get; set; }
        public string LocationDetail { get; set; }
        public string Remark { get; set; }
        public Nullable<int> RoadsideAssistanceServiceTypeId { get; set; }
        public string RoadsideAssistanceServiceType { get; set; }
        public Nullable<int> PayTypeId { get; set; }
        public string PayType { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<decimal> CustomerAmount { get; set; }
        public Nullable<decimal> ServiceAmount { get; set; }
        public string ReferanceCode { get; set; }
        public string ReferanceName { get; set; }
        public Nullable<System.DateTime> ClosedDate { get; set; }
        public Nullable<int> ClosedByUserId { get; set; }
        public string ClosedByCode { get; set; }
        public string ClosedByName { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
        public string CreatedByCode { get; set; }
        public string CreatedByName { get; set; }
        public Nullable<int> MemberId { get; set; }
        public string MemberCode { get; set; }
        public Nullable<int> CardDetailTypeId { get; set; }
        public string CardDetailType { get; set; }
        public string CardDetail { get; set; }
        public Nullable<int> TitleId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string BuildingName { get; set; }
        public Nullable<int> FreeServiceTotal { get; set; }
        public Nullable<int> FreeServiceUsed { get; set; }
        public Nullable<System.DateTime> StartCoverDate { get; set; }
        public Nullable<System.DateTime> EndCoverDate { get; set; }
        public string SourceTypeIdList { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public string VehicleRegistrationFull { get; set; }
        public string UseService { get; set; }
        public string CancelServiceRemark { get; set; }
        public string CancelRemark { get; set; }
        public string SourceTypeIdListText { get; set; }
        public Nullable<System.DateTime> DateHappen { get; set; }
        public Nullable<decimal> EmployeePointAmount { get; set; }
        public Nullable<System.DateTime> ReferanceClosedDate { get; set; }
        public string ServiceCancelCauseDetail { get; set; }
        public string OtherServiceCancelRemark { get; set; }
    }
}
