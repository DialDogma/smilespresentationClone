using System;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace SmileClaimV1.CustomControl
{
    public class IdentityModels : IdentityUser
    {
    }
    public class ApplicationDbContext : IdentityDbContext<IdentityModels>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {

        }
    }
}