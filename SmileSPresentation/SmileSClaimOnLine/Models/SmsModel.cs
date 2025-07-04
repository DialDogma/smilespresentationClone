using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSClaimOnLine.Models
{
    public class SmsModel
    {
        public class SMS_Result
        {
            public string Status { get; set; }
            public string Detail { get; set; }
            public string Language { get; set; }
            public string UsedCredit { get; set; }
            public string SumPhone { get; set; }
            public string ReferenceId { get; set; }
        }


        public class SMSSendListRequest
        {
            public int ProjectId { get; set; }
            public int SMSTypeId { get; set; }
            public string Remark { get; set; }
            public int Total { get; set; }
            public DateTime? SendDate { get; set; }
            public List<SendListDetail> Data { get; set; }

        }

        public class SendListDetail
        {
            public string Ref { get; set; }
            public string PhoneNo { get; set; }
            public string Message { get; set; }
        }
    }
}

