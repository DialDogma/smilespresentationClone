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
    using System.Collections.Generic;
    
    public partial class SMSQueueDetail
    {
        public int SMSQueueDetailId { get; set; }
        public Nullable<int> SMSQueueHeaderId { get; set; }
        public Nullable<int> SMSTypeID { get; set; }
        public string PhoneNo { get; set; }
        public string Message { get; set; }
        public Nullable<int> SMSID { get; set; }
        public Nullable<int> SMSQueueSentId { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string Ref { get; set; }
    
        public virtual SMSQueueHeader SMSQueueHeader { get; set; }
        public virtual SMSQueueSent SMSQueueSent { get; set; }
    }
}
