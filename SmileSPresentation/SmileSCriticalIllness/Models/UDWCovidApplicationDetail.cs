//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSCriticalIllness.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UDWCovidApplicationDetail
    {
        public int UDWCovidApplicationDetailId { get; set; }
        public Nullable<int> UDWCovidApplicationHeaderId { get; set; }
        public Nullable<int> CovidApplicationId { get; set; }
        public string CovidApplicationCode { get; set; }
        public Nullable<int> CovidApplicationStatusId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<System.DateTime> StartCoverDate { get; set; }
        public Nullable<System.DateTime> EndCoverDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentityCardNo { get; set; }
        public Nullable<int> SexId { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public Nullable<int> AgeAtRegister_Year { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string Email { get; set; }
        public string LineId { get; set; }
        public string Address { get; set; }
        public string SubDistrictId { get; set; }
        public string FullAddress { get; set; }
        public string SaleId { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedByUserId { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual UDWCovidApplicationHeader UDWCovidApplicationHeader { get; set; }
    }
}
