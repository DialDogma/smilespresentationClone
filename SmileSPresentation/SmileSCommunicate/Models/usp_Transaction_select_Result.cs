//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSCommunicate.Models
{
    using System;
    
    public partial class usp_Transaction_select_Result
    {
        public int SMSId { get; set; }
        public string PhoneNo { get; set; }
        public string Message { get; set; }
        public string Reference { get; set; }
        public int SMSTypeId { get; set; }
        public string SMSTypeDetail { get; set; }
        public System.DateTime SendDate { get; set; }
        public string TransactionStatusDetail { get; set; }
        public string Success { get; set; }
        public string Fail { get; set; }
        public string Block { get; set; }
        public string Expired { get; set; }
        public string Processing { get; set; }
        public string Unknown { get; set; }
        public Nullable<int> Credit { get; set; }
        public int TotalCount { get; set; }
        public Nullable<int> TransactionStatusID { get; set; }
    }
}
