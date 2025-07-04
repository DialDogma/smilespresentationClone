using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmileMotorV1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SmileMotorV1.Logic
{
    public class RoleAction
    {
        internal void AddUserAndRole()
        {
            Models.ApplicationDbContext context = new ApplicationDbContext();
            IdentityResult IdRoleResult;
            IdentityResult IdUserResult;

            var roleStore = new RoleStore<IdentityRole>(context);

            var roleMgr = new RoleManager<IdentityRole>(roleStore);

            if (!roleMgr.RoleExists("canEdit"))
            {
                IdRoleResult = roleMgr.Create(new IdentityRole { Name = "canEdit" });
            }

            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var appUser = new ApplicationUser
            {
                UserName = "canEditUser@SmileMotorV1.com",
                Email = "canEditUser@SmileMotorV1.com"
            };
            IdUserResult = userMgr.Create(appUser, "Pa$$word1");

            if (!userMgr.IsInRole(userMgr.FindByEmail("canEditUser@SmileMotorV1.com").Id, "canEdit"))
            {
                IdUserResult = userMgr.AddToRole(userMgr.FindByEmail("canEditUser@SmileMotorV1.com").Id, "canEdit");
            }
        }
    }
}