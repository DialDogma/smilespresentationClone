using System.Linq;
using System.Web.Mvc;
using SmileSPoint.Helper;
using SmileSPoint.Models;

namespace SmileSPoint.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        private SmilePointEntities _context;

        public HomeController()
        {
            _context = new SmilePointEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            //Get employee ID
            var empId = Helper.GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;

            //get point remain
            var pointAccount = _context.PointAccounts.FirstOrDefault(x => x.ReferenceCode == empId && x.PointAccountTypeId == 10);

            if (pointAccount != null)
            {
                ViewBag.PointRemain = pointAccount.PointRemain.Value.ToString("#,##0.00");
                ViewBag.PointAccountId = pointAccount.PointAccountId;
            }
            else
            {
                ViewBag.PointRemain = 0;
                ViewBag.PointAccountId = 0;
            }

            return View();
        }

        public ActionResult AnotherLink()
        {
            return View("Index");
        }
    }
}