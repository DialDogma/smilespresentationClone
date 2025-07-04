using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmileSDataCenterV2.DTOs
{
    public class OrganizeAddDto
    {
        [Required]
        public int? organizeTypeId { get; set; }

        [Required]
        public string organizeDetail { get; set; }

        public string buildingName { get; set; }
        public string villageName { get; set; }
        public string no { get; set; }
        public string moo { get; set; }
        public string floor { get; set; }
        public string soi { get; set; }
        public string road { get; set; }

        [Required]
        public string subDistrictCode { get; set; }

        [Required]
        public string zipCode { get; set; }

        [Required]
        public string createByUserCode { get; set; }

        [Required]
        public int? organizeSubTypeId { get; set; }

        public string taxNumber { get; set; }
    }
}