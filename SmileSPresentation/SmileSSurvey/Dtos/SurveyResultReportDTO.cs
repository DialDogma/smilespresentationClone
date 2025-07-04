using SmileSSurvey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSSurvey.Dtos
{
    public class SurveyResultReportDTO
    {
        public int SurveyId { get; set; }
        public int FormId { get; set; }

        public string FormName { get; set; }
        public int? EmployeeId { get; set; }
        public string Criteria { get; set; }
        public Boolean? IsSendSMS { get; set; }
        public Boolean? IsRead { get; set; }
        public Boolean? IsSubmit { get; set; }

        public List<SurveyResultQuestionReportDTO> Questions { get; set; }
    }

    public class SurveyResultQuestionReportDTO
    {
        public int QuestionId { get; set; }
        public string QuestionDetail { get; set; }
        public List<SurveyResultAnswerReportDTO> Answers { get; set; }
    }

    public class SurveyResultAnswerReportDTO
    {
        public int? AnswerId { get; set; }
        public string AnswerDetail { get; set; }
        public string AnswerMore { get; set; }
    }
}