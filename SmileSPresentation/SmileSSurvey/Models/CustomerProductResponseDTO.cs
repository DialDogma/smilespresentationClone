using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSSurvey.Models
{
    public class CustomerProductResponseDTO
    {
        public List<CustomerProductByClaim> data { get; set; } = new List<CustomerProductByClaim>();
        public bool? isSuccess { get; set; }
        public DateTime serverDateTime { get; set; }
    }

    public class CustomerProductByClaim
    {
        public string Titelname { get; set; }
        public string Surname { get; set; }
        public string Lastname { get; set; }
        public string FullName { get; set; }
        public string ProductName { get; set; }
    }
}