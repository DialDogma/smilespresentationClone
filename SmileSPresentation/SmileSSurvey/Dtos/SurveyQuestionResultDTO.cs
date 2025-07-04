using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSSurvey.Dtos
{
    public class SurveyQuestionResultDTO
    {
        public int SurveyQuesionId { get; set; }
        public List<SurveyAnswerDTO> SurveyAnswers { get; set; }
    }
}