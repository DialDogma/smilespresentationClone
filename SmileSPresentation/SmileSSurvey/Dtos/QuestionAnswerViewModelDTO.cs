using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSSurvey.Dtos
{
    public class QuestionAnswerViewModelDTO
    {
        public int SurveyQuestionId { get; set; }
        public string Criteria { get; set; }
        public string QuestionDetail { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<AnswerViewModelDTO> Answers { get; set; } = new List<AnswerViewModelDTO>();
    }
}