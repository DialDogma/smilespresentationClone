using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSClaimOnLine.DTOs
{
    public class IClaimResponseDto
    {
        public string msg { get; set; }
        public string trans_no { get; set; }
        public string customer_guid { get; set; }
        public bool is_valid { get; set; }
        public int error_ratio { get; set; }
        public ErrorDescription[] error_description { get; set; }
    }

    public class ErrorDescription
    {
        public string id { get; set; }
        public string message { get; set; }
    }
}