using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSClaimOnLine.DTOs
{
    public class GetResponseSurveyDTO
    {
        public Nullable<bool> IsResult { get; set; }
        public string Result { get; set; }
        public string Msg { get; set; }
        public string SendPhoneNo { get; set; }
    }
}