//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSUnderwrite.Models
{
    using System;
    
    public partial class usp_Underwrite_Select_Result
    {
        public int UnderwriteId { get; set; }
        public Nullable<int> QueueId { get; set; }
        public int UnderwriteStatusId { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedByUserId { get; set; }
        public string UnderwriteStatus { get; set; }
        public string CreatedByEmpCode { get; set; }
        public string CreatedName { get; set; }
        public int CallStatusId { get; set; }
        public string CallStatus { get; set; }
        public Nullable<int> CallRound { get; set; }
        public Nullable<int> QuestionId { get; set; }
        public string ReferenceCode { get; set; }
        public Nullable<double> CorruptAmount { get; set; }
    }
}
