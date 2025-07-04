using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmileSUnderwriteAudit.Models
{
    public class CallStatusMotorUnderWrite
    {
        [Required]
        public int CallStatusId { get; set; }

        [Required]
        [StringLength(250)]
        public string CallStatus { get; set; }

        public bool IsActive { get; set; } = true;
    }
}