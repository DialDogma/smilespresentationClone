using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSSurvey.Models
{
    public class GetClaimRecordsByClaimIdDTO
    {
        public List<Data> data { get; set; } = new List<Data>();
        public bool? isSuccess { get; set; }
        public DateTime serverDateTime { get; set; }
    }

    public class Data
    {
        public string Name { get; set; }
        public string ProductType { get; set; }
        public decimal? Amount { get; set; }
        public string ServiceName { get; set; }
    }
}