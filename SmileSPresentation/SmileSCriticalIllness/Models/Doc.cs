using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSCriticalIllness.Models
{
    public class Doc
    {
        public string FastName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public int Premium { get; set; }
        public int MaxCover { get; set; }
        public string Occupation { get; set; }
        public int Sum { get; set; }
        public string AppId { get; set; }
        public string Phone { get; set; }
        public string HBD { get; set; }
        public string StartCoverDate { get; set; }
        public string EndCoverDate { get; set; }
        public int Duty { get; set; }
        public int Vat { get; set; }
        public int Age { get; set; }
    }
}