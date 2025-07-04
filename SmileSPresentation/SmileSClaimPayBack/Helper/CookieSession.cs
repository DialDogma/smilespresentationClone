using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSClaimPayBack.Helper
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