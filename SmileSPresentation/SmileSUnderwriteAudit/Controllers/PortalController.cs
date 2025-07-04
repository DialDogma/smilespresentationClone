using SmileSUnderwriteAudit.Helper;
using System.Linq;
using System.Web.Mvc;

namespace SmileSUnderwriteAudit.Controllers
{
    [Authorization]
    public class PortalController : Controller
    {
        // GET: Porter
        public ActionResult Index()
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            ViewBag.UserDetail = user;

            //Get Role
            var roleListRaw = OAuth2Helper.GetRoles();
            var roleList = roleListRaw.Split(',');
            var accessRole = GlobalFunction.GetAccessRoleUnderWriteAudit(roleList);
            ViewBag.AccessRole = roleList.ToList();
            return View();
        }
    }
}