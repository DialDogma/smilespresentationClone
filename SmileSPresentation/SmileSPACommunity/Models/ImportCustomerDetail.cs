using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSPACommunity.Models
{
    public class ImportCustomerDetail
    {
        public string title { get; set; }
        public string firstName { get; set; }
        public string lasttName { get; set; }
        public string zCardID { get; set; }
        public string passport { get; set; }
        public string birthdate_Day { get; set; }
        public string birthdate_Month { get; set; }
        public string birthdate_Year { get; set; }
        public string occupation { get; set; }
        public string moblienumber { get; set; }
        public string customerDetailType { get; set; }

    }
}