using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSClaimOnLine.DTOs
{
    public class SurveyDto
    {
        public string msg { get; set; } = "";
        public bool flgSendSMSWithSurvey { get; set; } = false;
        public string MsgSurvey { get; set; } = "";
    }
}