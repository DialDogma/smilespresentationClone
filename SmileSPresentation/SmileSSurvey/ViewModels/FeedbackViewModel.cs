using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSSurvey.ViewModels
{
    public class FeedbackViewModel
    {
        public int SurveyQuestion1 { get; set; }
        public int SurveyAnswers1 { get; set; }
        public int SurveyQuestion2 { get; set; }

        public List<string> SurveyAnswers2 { get; set; }

        public int SurveyQuestion3 { get; set; }
        public string SurveyAnswers3 { get; set; }
        public int SurveyId { get; set; }
        public int ViewLogId { get; set; }
    }
}