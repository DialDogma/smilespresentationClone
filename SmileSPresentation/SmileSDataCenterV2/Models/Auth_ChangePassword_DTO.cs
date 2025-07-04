using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSDataCenterV2.Models
{
    public class Auth_ChangePassword_DTO
    {
        public string Username { get; set; }
        public string PasswordNew { get; set; }
    }
}