using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSSurvey.Models
{
    public class SurveyResultDTO
    {
        public int SurveyViewLogId { get; set; }
        public int SurveyId { get; set; }
        public Nullable<int> SurveyQuestionId { get; set; }
        public Nullable<int> SurveyAnswerId { get; set; }
        public string AnswerMore { get; set; }
    }
}