using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSClaimPayBack.Models
{
    public class MedicalBillAllDataListModel
    {
        public Dictionary<int, IEnumerable<usp_MedicalBillsReport_Select_Result>> MedicalBillReports { get; set; }
        public MedicalBillAllDataListModel()
        {
            MedicalBillReports = new Dictionary<int, IEnumerable<usp_MedicalBillsReport_Select_Result>>(); // สร้าง List ใน Constructor
        }
    }
}