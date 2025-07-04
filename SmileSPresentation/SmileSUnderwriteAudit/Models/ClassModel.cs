using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSUnderwriteAudit.Models
{
    public partial class Document
    {
        public string DocumentId { get; set; }
        public string DocumentFileId { get; set; }
        public int RunningNo { get; set; }
        public string DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; }
        public string PathThumbnailImg { get; set; }
        public string PathFullDoc { get; set; }
    }
}