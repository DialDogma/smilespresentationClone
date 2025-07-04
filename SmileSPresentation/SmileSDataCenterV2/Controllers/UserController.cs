using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using OfficeOpenXml;
using SmileSDataCenterV2.Models;
using SmileSDataCenterV2.Helper;
using SmileSDataCenterV2.WebServices;

namespace SmileSDataCenterV2.Controllers
{
    [Authorization]
    public class UserController : Controller
    {
        private DataCenterV1Entities _context;

        public UserController()
        {
            _context = new DataCenterV1Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetDatatable(int draw, int pageSize, int pageStart, string sortField, string orderType, string search)
        {
            var list = _context.usp_AspNetUsers_Datatable(pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ResetPassword(string username)
        {
            ViewBag.UserInfo = _context.usp_GetUserDetailByUserName(username).FirstOrDefault();
            ViewBag.Username = username;
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(FormCollection form)
        {
            var user = form["Username"];

            //Reset Password Datacenter
            var service = new SmileSDataCenterV2.WebServices.SmileSLoginService();
            if (service.ResetPassword(user))
            {
                //Reset Password SSS
                if (service.ResetPassword_SSS(user))
                {
                    return RedirectToAction("ResetPasswordSuccess", new { username = user });
                }
            }
            ViewBag.UserInfo = _context.usp_GetUserDetailByUserName(user).FirstOrDefault();
            ViewBag.Username = user;
            ViewBag.ErrorText = "reset password ไม่สำเร็จ";
            return View();
        }

        public ActionResult ResetPasswordSuccess(string username)
        {
            ViewBag.Username = username;
            return View();
        }

        public ActionResult EditRoles(string username)
        {
            ViewBag.UserInfo = _context.usp_GetUserDetailByUserName(username).FirstOrDefault();
            var model = _context.usp_RolesEditByUserName(username).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult EditRoles(FormCollection form)
        {
            var username = form["Username"];
            var userId = _context.usp_GetUserDetailByUserName(username).FirstOrDefault().Id;

            //Clear all roles
            _context.usp_RolesClearByUserName(username);

            foreach (var item in form.AllKeys)
            {
                //Find chk_
                var leftString = item.Substring(0, 4);
                if (leftString == "chk_")
                {
                    //Get RoleId
                    var roleId = item.Replace("chk_", "");
                    //Add Role
                    _context.usp_UserRolesAdd(userId, roleId);
                }
            }

            ViewBag.UserInfo = _context.usp_GetUserDetailByUserName(username).FirstOrDefault();
            ViewBag.Message = "บันทึกเรียบร้อย";
            var model = _context.usp_RolesEditByUserName(username).ToList();
            return View(model);
        }

        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(FormCollection form)
        {
            var manager = new UserManager();

            var user = new ApplicationUser() { UserName = form["select_Emp"] };

            IdentityResult result = manager.Create(user, form["select_Emp"]);

            var personUser = new usp_PersonUser_Insert_Result();

            //If success
            if (result.Succeeded)
            {
                //Add default roles here
                //get PersonId
                var empDetail = _context.usp_EmployeeByEmployeeCode_Select(form["select_Emp"]).FirstOrDefault();

                //execute usp_PersonUser_Insert to map person and employee
                personUser = _context.usp_PersonUser_Insert(user.Id, empDetail.Person_ID, empDetail.Employee_ID, GlobalFunction.GetLoginDetail(this.HttpContext).User_ID).FirstOrDefault();
                ViewBag.Result = "Success";
            }
            else
            {
                ViewBag.Result = "Fail";
            }

            return View();
        }

        public ActionResult AddUserToRole(string empCode)
        {
            var empdetail = _context.usp_EmployeeSearch_Select(empCode).FirstOrDefault();
            if (empdetail != null)
            {
                ViewBag.Name = empdetail.TitleDetail + " " + empdetail.FirstName + " " + empdetail.LastName;
                ViewBag.Position = empdetail.EmployeePositionDetail;
                ViewBag.Department = empdetail.DepartmentDetail;
                ViewBag.EmpCode = empdetail.EmployeeCode;
                ViewBag.roleResult = _context.usp_RoleTemplate_Select(null, 0, null, null, null, null).ToList();
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }

            return View();
        }

        public JsonResult GetRolesinTemplate(int templateId)
        {
            var result = _context.usp_RoleTemplateXAspNetRoles_Select(templateId).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployee(string q)
        {
            var result = _context.usp_EmployeeSearch_Select(q).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeDetail(string empCode)
        {
            var result = _context.usp_EmployeeSearch_Select(empCode).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserId(string empCode)
        {
            var result = _context.usp_UserIDByEmployeeCode_Select(empCode).FirstOrDefault();

            if (result == null)
            {
                result = 0;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region userRoleImport

        public ActionResult UserRoleImport()
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            var delete = _context.usp_AspUserRolesTmpImport_Delete(user.User_ID);
            ViewBag.Roles = _context.usp_RolesEditByUserName(user.UserName).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult ReadExcelFile(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                if (Path.GetExtension(file.FileName) == ".xlsx" || Path.GetExtension(file.FileName) == ".xls")
                {
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var ws = package.Workbook.Worksheets.First();
                        int count = 0;

                        for (int n = 2; n <= ws.Dimension.End.Row; n++)
                        {
                            if (ws.Cells[n, 1].Value != null)
                            {
                                var userName = ws.Cells[n, 1].Value.ToString();
                                if (userName.Length != 5)
                                {
                                    return Json("InvalidData", JsonRequestBehavior.AllowGet);
                                }
                                count++;
                            }
                        }
                        return Json(count, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("Invalid", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Invalid", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveImport(HttpPostedFileBase file, List<string> roles,string remark)
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            //clear tmp
            var delete = _context.usp_AspUserRolesTmpImport_Delete(user.User_ID);

            if (file != null && file.ContentLength > 0)
            {
                if (Path.GetExtension(file.FileName) == ".xlsx" || Path.GetExtension(file.FileName) == ".xls")
                {
                    //read excel
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var ws = package.Workbook.Worksheets.First();
                        for (int i = 2; i <= ws.Dimension.End.Row; i++)
                        {
                            if (ws.Cells[i, 1].Value != null)
                            {
                                for (int j = 0; j < roles.Count; j++)
                                {
                                    var userName = ws.Cells[i, 1].Value.ToString();
                                    var roleName = roles[j];
                                   
                                    var addRole = _context.usp_AspUserRolesTmpImport_Insert(roleName, userName, user.User_ID);
                                }
                            }
                        }

                        var validate = _context.usp_AspUserRolesTmpImport_Validate(user.User_ID).ToList();
                        if (validate.Count == 0)
                        {
                            var save = _context.usp_AspUserRolesImport_Insert(user.User_ID, remark);
                            return Json("Success", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var rs = _context.usp_AspUserRolesTmpImport_Validate(user.User_ID).ToList();

                          

                            //get new GUID
                            var dataSessionKey = Guid.NewGuid().ToString();
                            //TempData GUID
                            TempData["keyReport"] = dataSessionKey;
                            //TempData Data
                            TempData[dataSessionKey] = rs;
                        }
                    }
                }
            }
            return Json("Fail", JsonRequestBehavior.AllowGet);
        }

   
        public void DownloadError()
        {
            var dataKey = TempData["keyReport"].ToString();

            if (TempData[dataKey] != null)
            {
                var data = TempData[dataKey] as List<usp_AspUserRolesTmpImport_Validate_Result>;

                var newData = data.Select(_ => new { Name = _.Name, Detail = _.Detail }).ToList();

                ExcelManager.ExportToExcel(this.HttpContext, newData, "sheet1", "UserRolesImport_Invalid_Report_"+ DateTime.Now.ToString());
            }
        }

        public JsonResult GetRolesImportTransaction(int draw, int pageSize, int indexStart, string search)
        {
            var list = _context.usp_AspNetUserRolesTransaction_Select(indexStart, pageSize, null, null, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public void ExportRolesImportTransactionDetail(int id, string isUpdate)
        {
            List<usp_AspNetUserRolesTransactionDetail_Select_Result> data = new List<usp_AspNetUserRolesTransactionDetail_Select_Result>();
            if (isUpdate == "1") { 
                data = _context.usp_AspNetUserRolesTransactionDetail_Select(id, true, null, null, null, null, null).ToList();
            }
            else
            {
                data = _context.usp_AspNetUserRolesTransactionDetail_Select(id, null, null, null, null, null, null).ToList();
            }
                var newData = data.Select(x => new
                {
                    Username = x.UserName,
                    Employee = x.Employee,
                    Role_Name = x.Name,
                    เดิมมีสิทธิ์อยู่แล้ว = x.เดิมมีสิทธิ์อยู่แล้ว
                }).ToList();

                ExcelManager.ExportToExcel(this.HttpContext, newData, "sheet1", "UserRolesImported_ID_"+id+"_Report_"+ DateTime.Now.ToString());
            }

       
        public JsonResult UndoRolesImport(int id)
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            var undo = _context.usp_AspUserRolesImportUndo_Update(id, user.User_ID).FirstOrDefault();
            return Json(1, JsonRequestBehavior.AllowGet);
        }



        #endregion userRoleImport
    }
}
