using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSUnderwrite.Models
{
    public class Mock_Miw
    {
    }

    public class Monitor
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public int Amoount_Close { get; set; }
        public int Amount_Waiting { get; set; }
    }

    public class MonitorDetail
    {
        public string SchoolName { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string SubDistrict { get; set; }
        public string Payment { get; set; }
        public string CreatedBy { get; set; }
        public DateTime QueueDate { get; set; }
        public int Day { get; set; }
        
    }

    public class Detail
    {
        public DateTime QueueDate { get; set; }
        public string CreatedBy { get; set; }
        public string Status { get; set; }
    }

    public class QueueCallDetail
    {
        public string SchoolName { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string SubDistrict { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string StatusCall { get; set; }
        public string Remark { get; set; }

    }

    public class HisUnderWrite
    {
        public DateTime UnderWriteDate { get; set; }
        public string QueueType { get; set; }
        public string ContactStatus { get; set; }
        public string Remark { get; set; }
    }
}