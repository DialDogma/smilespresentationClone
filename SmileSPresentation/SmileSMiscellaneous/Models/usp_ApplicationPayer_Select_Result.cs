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
    
    public partial class usp_ApplicationPayer_Select_Result
    {
        public int ApplicationPayerId { get; set; }
        public string ApplicationPayerCode { get; set; }
        public Nullable<int> ApplicationId { get; set; }
        public Nullable<int> PersonType_ID { get; set; }
        public string PersonTypeDetail { get; set; }
        public string TitleDetail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PayerName { get; set; }
        public string IdCardNumber { get; set; }
        public string PassPortNumber { get; set; }
        public string MobileNumber { get; set; }
        public Nullable<bool> IsSameCustomer { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> Title_ID { get; set; }
        public Nullable<int> TotalCount { get; set; }
    }
}
