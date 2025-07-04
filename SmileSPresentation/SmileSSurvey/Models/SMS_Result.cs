using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSSurvey.Models
{
    public class SMS_Result
    {
        public string Status { get; set; }
        public string Detail { get; set; }
        public string Language { get; set; }
        public string UsedCredit { get; set; }
        public string SmsId { get; set; }
        public string SumPhone { get; set; }

        public string ReferenceId { get; set; }
    
    }
}