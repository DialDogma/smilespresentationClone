using SmileUnderwriteBranchV2.Helper;
using System.Linq;
using System.Web.Mvc;

namespace SmileUnderwriteBranchV2.Controllers
{
    [Authorization]
    public class PortalController : Controller
    {
        // GET: Porter
        public ActionResult Index()
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);

            //Get Role
            var roleListRaw = OAuth2Helper.GetRoles();
            var roleList = roleListRaw.Split(',');
            var accessRole = GlobalFunction.GetAccessRole(roleList);
            ViewBag.AccessRole = roleList.ToList();

            return View();
        }
    }
}