//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSClaimOnLine.Models
{
    using System;
    
    public partial class usp_ClaimOnLineRawdataSheet1Detail_Select_Result
    {
        public int RawdataDetailSheet1Id { get; set; }
        public Nullable<int> RawdataHeaderId { get; set; }
        public Nullable<int> RowNumber { get; set; }
        public string ClaimOnLineCode { get; set; }
        public string ClaimHeaderGroupCode { get; set; }
        public string ClaimCode { get; set; }
        public Nullable<double> Pay { get; set; }
        public string ProductType { get; set; }
        public string Detail { get; set; }
        public string Branch { get; set; }
        public string ServiceByEmpCode { get; set; }
        public string ServiceByEmpName { get; set; }
        public Nullable<System.DateTime> TransferDateStart { get; set; }
        public Nullable<System.DateTime> ReturnDateFinal { get; set; }
        public Nullable<System.DateTime> ReceiveDocDate { get; set; }
        public Nullable<System.DateTime> ClaimCreateDate { get; set; }
        public string ClaimApproveByEmpCode { get; set; }
        public Nullable<int> ZebraCarOwnerByEmpId { get; set; }
        public string ClaimOnLineCreateByCode { get; set; }
        public string ClaimOnLineCreateByName { get; set; }
        public string ClaimOnLineStatus { get; set; }
        public string Area { get; set; }
        public string StudyArea { get; set; }
    }
}
