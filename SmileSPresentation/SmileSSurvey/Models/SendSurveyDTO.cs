using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSSurvey.Models
{
    public class SendSurveyDTO
    {
        public int? formId { get; set; }
        public string sFCaseId { get; set; }
        public Guid? customerId { get; set; }
        public string phoneNo { get; set; }
        public string employeeCode { get; set; }
        public List<ReferenceDTO> reference { get; set; }
        public string sendBy { get; set; }
        public DateTime? sendDate { get; set; }
        public Boolean isSendNow { get; set; }



    }
}