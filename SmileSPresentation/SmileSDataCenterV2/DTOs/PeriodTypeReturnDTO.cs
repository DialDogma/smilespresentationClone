using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSDataCenterV2.DTOs
{
    public class PeriodTypeReturnDTO
    {
        public int PeriodTypeId { get; set; }
        public string PeriodTypeName { get; set; }
        public float TotalPremium { get; set; }
    }
}