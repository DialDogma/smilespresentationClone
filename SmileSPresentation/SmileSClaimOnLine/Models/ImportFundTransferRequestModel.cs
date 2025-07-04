using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSClaimOnLine.Models
{
    public class ImportFundTransferRequestModel
    {
        public string Branch { get; set; }
        public string Zone { get; set; }
        public string ClaimHeaderGroupCode { get; set; }
        public string Product { get; set; }
        public string ItemCount { get; set; }
        public string Total_Amount { get; set; }
        public string ClaimType { get; set; }
        public string SendDate { get; set; }
        public string PayDate { get; set; }
        public string InturanceCompany { get; set; }

    }
}