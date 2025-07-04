using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSClaimPayBack.Models
{
    public class ImportClaimHeaderGroupRequestModel
    {
        public string ClaimHeaderGroupCode { get; set; }
        public string ItemCount { get; set; }
        public string Total_Amount { get; set; }
    }
}