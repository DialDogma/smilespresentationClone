using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSDataCenterV2.DTOs
{
    public class ProductPackageReturnDTO
    {
        public int ProductPackageId { get; set; }
        public string ProductPackageName { get; set; }
        public List<ProductReturnDTO> Products { get; set; }
        public List<PeriodTypeReturnDTO> PeriodTypes { get; set; }
    }
}