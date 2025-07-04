using SmileSSurvey.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmileSSurvey.Dtos
{
    public class SurveyRequestDTO
    {
        [Required]
        public int FormId { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public Boolean IsSendSMS { get; set; }

        public string Criteria { get; set; } // ใช้ Criteria เพื่อรับค่าหลายเงื่อนไข เช่น ชื่อ, รหัส, หรือเบอร์โทร เพื่อแสดงผลข้อมูล
        
        public DateTime? CheckinDate { get; set; }

        public int? CreatedByUserId { get; set; }
    }
}