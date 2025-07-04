using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSPACommunity.ViewModels
{
    public class ApproveCustomerDetailDto
    {
        public int endorsePersonnelCustomerDetailHeaderId { get; set; }
        public int approveStatus { get; set; }
        public int statusId { get; set; }
        public string approveRemark { get; set; }
    }

    public class CommentInfoDto
    {
        public string approveCause { get; set; }
        public string remark { get; set; }
    }
}