using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSSurvey.Models
{
    public class SurveyQuestionAnswerDTO
    {
        public int SurveyQuestionId { get; set; }
        public Nullable<int> QuestionId { get; set; }
        public string QuestionDetail { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public List<usp_SurveyAnswer_Select_Result> Answers { get; set; } = new List<usp_SurveyAnswer_Select_Result>();
    }
}