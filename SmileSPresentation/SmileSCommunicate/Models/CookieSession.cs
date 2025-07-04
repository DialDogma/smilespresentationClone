using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSCommunicate.Models
{
    public class CookieSession
    {
        public CookieSession(bool status)
        {
            cookieStatus = status;
        }
        public bool cookieStatus { get; set; }
    }
}