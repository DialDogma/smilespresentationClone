
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmilesSSytemMonitor.Models
{
    public class ServerMonitorForCreate
    {
        public string ServerName { get; set; }
        public double Disk_C_Free { get; set; }
        public double Disk_D_Free { get; set; }
        public double Disk_E_Free { get; set; }
        public double Disk_F_Free { get; set; }
        public double Disk_C_Total { get; set; }
        public double Disk_D_Total { get; set; }
        public double Disk_E_Total { get; set; }
        public double Disk_F_Total { get; set; }
        public double RAM { get; set; }
        public double CPU { get; set; }
        public double DiskIO { get; set; }
    }
}