//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileMotorV1.Models
{
    using System;
    
    public partial class usp_PersonRelatedApplication_Update_Result
    {
        public int PersonRelatedApplicaton_ID { get; set; }
        public Nullable<int> PersonRelatedApplicatonType_ID { get; set; }
        public Nullable<int> Person_ID { get; set; }
        public Nullable<int> MotorApplication_ID { get; set; }
        public Nullable<int> CreateBy_ID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> PersonTransaction_ID { get; set; }
    }
}
