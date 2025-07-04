using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSDataCenterV2.Models;
using SmileSDataCenterV2.Helper;

namespace SmileSDataCenterV2.Controllers
{
    [Authorization(Roles = "Developer")]
    public class MenuController : Controller
    {
        private DataCenterV1Entities _context;

        public MenuController()
        {
            _context = new DataCenterV1Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Menu
        public ActionResult Index()
        {
            ViewBag.Projects = _context.Projects.ToList();
            return View();
        }

        public JsonResult GetDatatable(int draw, int pageSize, int pageStart, string sortField, string orderType, string search, string projectName)
        {
            var list = _context.usp_Menu_Datatable(pageStart, pageSize, sortField, orderType, search, projectName).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReOrder(string projectName)
        {
            ViewBag.MenuList = _context.usp_Menu_Datatable(0, 1000, null, null, null, projectName).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult ReOrder(FormCollection form)
        {
            //TODO: Update
            return View();
        }

        public ActionResult Add(string projectName)
        {
            return View();
        }
    }
}