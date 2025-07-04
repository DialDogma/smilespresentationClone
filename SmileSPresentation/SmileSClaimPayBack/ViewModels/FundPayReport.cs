using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;

namespace SmileSClaimPayBack.ViewModels
{
    public class FundPayReport
    {
        public string Rows { get; set; }
        public string SubGroupCode { get; set; }
        public int? ItemCount { get; set; }
        public string HospitalName { get; set; }
        public string Amount { get; set; }
        public string BillingCode { get; set; }
        public string BillingAmount { get; set; }
    }
     
}