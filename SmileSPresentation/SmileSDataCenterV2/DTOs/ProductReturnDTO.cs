using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSDataCenterV2.DTOs
{
    public class ProductReturnDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public float Premium { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public int ProductGroupId { get; set; }
        public string ProductGroupName { get; set; }
    }
}