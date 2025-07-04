using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSClaimPayBack.Models
{
    public class DashboardBillingModel
    {
        public decimal? BillingAmountTotal { get; set; }
        public decimal? PaymentAmountTotal { get; set; }
        public decimal? OutstandingBalanceTotal { get; set; }
        public List<usp_DashboardBillingAmountTotal_Detail_Select_Result> BillingAmountList { get; set; }
        public List<usp_DashboardPaymentAmountTotal_Detail_Select_Result> PaymentAmountList { get; set; }
        public List<usp_DashboardOutstandingBalanceTotal_Detail_Select_Result> OutstandingBalanceList { get; set; }
    }
}