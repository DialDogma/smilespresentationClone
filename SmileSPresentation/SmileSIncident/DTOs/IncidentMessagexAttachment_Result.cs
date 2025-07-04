using SmileSIncident.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSIncident.DTOs
{
    public class IncidentMessagexAttachment_Result
    {
        public usp_IncidentMessage_Select_Result ReplyMessage { get; set; }
        public List<usp_IncidentAttachment_Select_Result> Attachments { get; set; } = new List<usp_IncidentAttachment_Select_Result>();
    }
}