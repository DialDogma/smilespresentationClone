using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSClaimOnLine.DTOs
{
    public class PostTemplateSurveyDTO
    {
        public int formId { get; set; }
        public string sFCaseId { get; set; }
        public Guid? customerId { get; set; }
        public string phoneNo { get; set; }
        public string employeeCode { get; set; }
        public List<Reference> reference { get; set; }
        public bool isSendNow { get; set; }
        public string sendBy { get; set; }
        public DateTime sendDate { get; set; }

        public class Reference
        {
            public string refId { get; set; }
        }
    }
}