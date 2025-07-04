using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SmileSDataCenterV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSDataCenterV2.Helper;

namespace SmileSDataCenterV2.Controllers
{
    [Authorization(Roles = "Developer")]
    public class RoleController : Controller
    {
        private DataCenterV1Entities _context;

        public RoleController()
        {
            _context = new DataCenterV1Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(FormCollection form)
        {
            //Add New Role
            //Get roleName
            var roleName = form["Name"];

            //create usermanager
            UserManager userManager = new UserManager();
            //create role store & manager
            var rolestore = new RoleStore<IdentityRole>(new ApplicationDbContext());
            var rolemanager = new RoleManager<IdentityRole>(rolestore);
            //add role name to result
            var result = new IdentityRole() { Name = roleName };
            //check if role does not exist
            if (!rolemanager.RoleExists(result.Name))
            {
                //create new role
                rolemanager.Create(result);
                //redirect
                return RedirectToAction("AddSuccess", new { name = roleName });
            }
            else
            {
                //show error
                ViewBag.ErrorText = "Role ซ้ำ !";
            }

            return View();
        }

        public ActionResult AddSuccess(string name)
        {
            ViewBag.RoleName = name;
            return View();
        }

        public JsonResult GetDatatable(int draw, int pageSize, int pageStart, string sortField, string orderType, string search)
        {
            var list = _context.usp_AspNetRoles_Datatable(pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        public JsonResult GetDatatable_Edit(int draw, int pageSize, int pageStart, string sortField, string orderType, string search, string roleId)
        {
            var list = _context.usp_GetUsers_ByRoleId_Datatable(pageStart, pageSize, sortField, orderType, search, roleId).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }
    }
}