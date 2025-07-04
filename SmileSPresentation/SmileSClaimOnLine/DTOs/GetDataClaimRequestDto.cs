using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSClaimOnLine.DTOs
{
    public class GetDataClaimRequestDto
    {
        public Guid? trans_no { get; set; }
        public Guid? customer_guid { get; set; }
        public string app_id { get; set; }
        public person person { get; set; }
        public policy policy { get; set; }
        public claim_data claim_data { get; set; }
    }

    public class person
    {
        public string sex { get; set; }
        public string birth_date { get; set; }
        public string age { get; set; }
        public string occupation { get; set; }
    }

    public class policy
    {
        public string product_name { get; set; }
        public string start_cover_date { get; set; }
        public string age_policy { get; set; }
        public string memo { get; set; }
        public decimal? loss_claim { get; set; }
    }

    public class claim_data
    {
        public string hospital_id { get; set; }
        public string hospital_name { get; set; }
        public string age { get; set; }
        public string birth_date { get; set; }
        public string date_happen { get; set; }
        public string insurer_code { get; set; }
        public string claim_admit_type { get; set; }
        public string follow_up { get; set; }
        public string icd10_1 { get; set; }
        public string icd10_2 { get; set; }
        public string icd10_3 { get; set; }
        public string code { get; set; }
        public double opd_compensation { get; set; }
        public double opd_claim { get; set; }
        public string province { get; set; }
        public string chief_complain { get; set; }
        public string gender { get; set; }
    }
}