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
    
    public partial class usp_StagingSourceTempDetail_Preview_Select_Result
    {
        public int Id { get; set; }
        public string TmpCode { get; set; }
        public string ReferenceCode { get; set; }
        public Nullable<int> SourceTypeId { get; set; }
        public string SourceType { get; set; }
        public Nullable<int> CardDetailTypeId { get; set; }
        public string CardTypeDetail { get; set; }
        public string CardDetail { get; set; }
        public Nullable<int> TItleId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string BuildingName { get; set; }
        public Nullable<System.DateTime> StartCoverDate { get; set; }
        public Nullable<System.DateTime> EndCoverDate { get; set; }
        public Nullable<bool> IsImport { get; set; }
        public Nullable<bool> IsValid { get; set; }
        public string Valid { get; set; }
        public string ValidateDescription { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public string PaymentType { get; set; }
        public Nullable<int> StartCoverDateYear { get; set; }
        public Nullable<int> StartCoverDateMonth { get; set; }
        public Nullable<int> StartCoverDateDay { get; set; }
        public Nullable<int> EndCoverDateYear { get; set; }
        public Nullable<int> EndCoverDateMonth { get; set; }
        public Nullable<int> EndCoverDateDay { get; set; }
    }
}
