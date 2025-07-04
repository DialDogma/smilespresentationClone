using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSClaimPayBack.Models
{
    public class ImportClaimPayBackSaveBalanceRequestModel
    {
        public string ClaimHeaderGroupCode { get; set; }
        public string ClaimHeader { get; set; }
        public string Total_Amount { get; set; }
        public string Cover_Amount { get; set; }
        //public string BillingDate { get; set; }
    }
}