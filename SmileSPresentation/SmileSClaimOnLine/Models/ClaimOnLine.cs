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
    using System.Collections.Generic;
    
    public partial class ClaimOnLine
    {
        public int ClaimOnLineId { get; set; }
        public string ClaimOnLineCode { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
        public string Detail { get; set; }
        public Nullable<int> ClaimCount { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<int> ServiceByUserId { get; set; }
        public Nullable<int> NoticeByUserId { get; set; }
        public Nullable<int> NoticeByEmpId { get; set; }
        public Nullable<int> NoticeByTeamId { get; set; }
        public Nullable<int> NoticeByBranchId { get; set; }
        public Nullable<int> NoticeByStudyAreaId { get; set; }
        public Nullable<System.DateTime> NoticeDate { get; set; }
        public Nullable<int> ClaimOnLineStatusId { get; set; }
        public Nullable<int> PayeetypeId { get; set; }
        public Nullable<int> PayeeBankId { get; set; }
        public string PayeeAccountNo { get; set; }
        public string PayeeAccountName { get; set; }
        public Nullable<int> TransferCount { get; set; }
        public Nullable<double> TransferAmountTotal { get; set; }
        public Nullable<System.DateTime> TransferDateLatest { get; set; }
        public Nullable<int> RealClaimCountTotal { get; set; }
        public Nullable<double> RealClaimAmountTotal { get; set; }
        public Nullable<int> ReturnCount { get; set; }
        public Nullable<double> ReturnAmountTotal { get; set; }
        public Nullable<System.DateTime> ReturnDateLatest { get; set; }
        public Nullable<System.DateTime> ReturnDateFinal { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreateByUserId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateByUserId { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> ZebraCarOwnerByEmpId { get; set; }
        public Nullable<int> AreaId { get; set; }
        public Nullable<int> PersonContactTypeId { get; set; }
        public string PersonContactTypeOther { get; set; }
        public string PersonContactName { get; set; }
        public string PersonContactPhoneNo { get; set; }
        public Nullable<int> Version { get; set; }
        public Nullable<System.DateTime> TransferDateStarted { get; set; }
        public Nullable<int> TransferFirstPersonId { get; set; }
        public Nullable<int> ClaimAIApprovedById { get; set; }
        public Nullable<System.DateTime> ClaimAIApproedDate { get; set; }
    }
}
