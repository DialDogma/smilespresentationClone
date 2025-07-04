using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSSurvey.Dtos
{
    public class ResultDtoResponse
    {
        public bool? IsResult { get; set; }
        public string Result { get; set; }
        public string Msg { get; set; }
        public string Url { get; set; }
    }
}