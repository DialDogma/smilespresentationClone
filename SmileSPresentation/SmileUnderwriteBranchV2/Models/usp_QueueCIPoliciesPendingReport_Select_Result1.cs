//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileUnderwriteBranchV2.Models
{
    using System;
    
    public partial class usp_QueueCIPoliciesPendingReport_Select_Result1
    {
        public int QueueId { get; set; }
        public string ApplicationCode { get; set; }
        public string ชื่อผู้เอาประกัน { get; set; }
        public string ชื่อผู้ชำระเบี้ย { get; set; }
        public string ชื่อหน่วยงาน { get; set; }
        public string ตำบล { get; set; }
        public string อำเภอ { get; set; }
        public string จังหวัด { get; set; }
        public string แผนประกัน { get; set; }
        public Nullable<double> เบี้ยประกัน { get; set; }
        public Nullable<System.DateTime> วันที่ส่งคิวงาน { get; set; }
        public int จำนวนวัน { get; set; }
    }
}
