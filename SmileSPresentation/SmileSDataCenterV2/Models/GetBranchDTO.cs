using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSDataCenterV2.Models
{
    public class GetBranchDTO
    {
        public int? BranchId { get; set; }
        public int? AreaId { get; set; }
        public int? IndexStart { get; set; }
        public int? PageSize { get; set; }

        public string SortField { get; set; }
        public string OrderType { get; set; }
        public string SearchDetail { get; set; }

    }
}