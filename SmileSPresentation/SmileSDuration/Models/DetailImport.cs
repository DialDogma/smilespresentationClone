using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSDuration.Models
{
    public class DetailImport
    {
        public string ComunicateCode { get; set; }
        public int ComTypeID { get; set; }
        public string AppID { get; set; }
        public string CusName { get; set; }
        public string PayerName { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountTotal { get; set; }
        public string MobileNo { get; set; }
        public string Message { get; set; }
        public string LetterDate { get; set; } //วันที่แจ้งเอกสาร
        public string PeriodOwe { get; set; } //จำนวนงวดค้างชำระ (2)
        public string DateRange { get; set; } //ช่วงวันที่ขาดชำระเบี้ย (2)
        public string DueDate { get; set; } //กำหนดชำระวันที่ (2)
        public string EndCover { get; set; } //วันสิ้นสุดความคุ้มครอง(2)
        public string DateOwe { get; set; } //ขาดชำระเบี้ยตั้งแต่วันที่ (3)
        public string RefNo { get; set; } //
    }

    public class SMSQueueHeaderDetailViewModel
    {
        public int ProjectId { get; set; }
        public int SMSTypeId { get; set; }
        public string Remark { get; set; }
        public int Total { get; set; }
        public string SendDate { get; set; }
        public SMSQueueHeaderDetail[] Data { get; set; }
    }

    public class SMSQueueHeaderDetail
    {
        public string PhoneNo { get; set; }
        public string Message { get; set; }
    }

    public class SMSResult
    {
        public string status { get; set; }
        public string detail { get; set; }
        public string referenceHeaderID { get; set; }
    }
}