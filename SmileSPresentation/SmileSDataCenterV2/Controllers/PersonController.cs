using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using SmileSDataCenterV2.Models;
using SmileSDataCenterV2.Helper;

namespace SmileSDataCenterV2.Controllers
{
    public class PersonController : Controller
    {
        #region Declare

        private readonly DataCenterV1Entities _dataCenterV1Entities;

        public PersonController()
        {
            _dataCenterV1Entities = new DataCenterV1Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _dataCenterV1Entities.Dispose();
        }

        #endregion Declare

        // GET: Person
        public ActionResult Index()
        {
            return View();
        }

        [Authorization(Roles = "Developer")]
        public ActionResult AddPerson()
        {
            ViewBag.listPersonTitle = _dataCenterV1Entities.usp_Title_Select(null, null).ToList();

            ViewBag.listPersonSex = _dataCenterV1Entities.usp_Sex_Select(null).ToList();

            ViewBag.listPersonMarital = _dataCenterV1Entities.usp_MaritalStatus_Select(null).ToList();

            ViewBag.listPersonOccupation = _dataCenterV1Entities.usp_Occupation_Select(null).ToList();

            ViewBag.listPersonProvince = _dataCenterV1Entities.usp_Province2_Select(null).ToList();

            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Supervisor")]
        public ActionResult PromotionalBusinessTeamManagement()
        {
            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Supervisor")]
        public ActionResult CustomerDirectorManagement()
        {
            ViewBag.Provinces = _dataCenterV1Entities.usp_Province_Select(null).ToList();
            ViewBag.Branchs = _dataCenterV1Entities.usp_Branch_SelectV2(null, null, 0, 9999, null, null, null).ToList();
            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Supervisor")]
        public ActionResult BranchPresidentManagement()
        {
            ViewBag.Branch = _dataCenterV1Entities.usp_Branch_SelectV2(null, null, 0, 9999, null, null, null).ToList();
            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Supervisor")]
        public ActionResult CustomerDirectorManagementV2()
        {
            ViewBag.Provinces = _dataCenterV1Entities.usp_Province_Select(null).ToList();
            ViewBag.Branchs = _dataCenterV1Entities.usp_Branch_SelectV2(null, null, 0, 9999, null, null, null).ToList();
            return View();
        }

        public void CustomerDirectorManagementExport(int? provinceId = null, string districtId = null,
                                                            int? branchId = null, string keyword = null)
        {
            var rs = _dataCenterV1Entities.usp_DirectorManagement_Select(provinceId, districtId == "-1" ? null : districtId, branchId == -1 ? null : branchId, 0, 99999999, null, null, keyword).Select(s => new
            {
                อำเภอ = s.DistrictDetail,
                จังหวัด = s.ProvinceDetail
                                                                                                                                                ,
                สาขา = s.BranchDetail,
                ผอ_บล = s.PersonName
            }).ToList();

            var rs1 = _dataCenterV1Entities.usp_DirectorManagementReportTab2_Select(provinceId, districtId == "-1" ? null : districtId, branchId == -1 ? null : branchId, 0, 99999999, null, null, keyword)
                .Select(s => new
                {
                    สาขา = s.BranchDetail,
                    รหัสผู้รับผิดชอบ = s.FromEmployeeCode,
                    ผู้รับผิดชอบ = s.FromPersonName,
                    รหัสผู้รับผิดชอบใหม่ = s.ToEmployeeCode,
                    ผู้รับผิดชอบใหม่ = s.ToPersonName,
                    รหัสผู้ทำรายการ = s.CreateEmployeeCode,
                    ผู้ทำรายการ = s.CreatePersonName,
                    วันที่ทำรายการ = s.CreateDate
                }).ToList();

            var dt = GlobalFunction.ToDataTable(rs);
            var dt2 = GlobalFunction.ToDataTable(rs1);

            ExcelManager.ExportToExcel(this.HttpContext, "รายงานจัดการผอ.บล.", dt, "รายงานจัดการผอ.บล.", dt2, "ประวัติการเปลี่ยนแปลง ผอ.บล.");
        }

        public void BranchPresidentManagementExport(int? branchId = null, string keyword = null)
        {
            var result = _dataCenterV1Entities.usp_ChairmanManagement_Select(branchId == -1 ? null : branchId, 0, 99999999, null, null, keyword)
                .Select(s => new
                {
                    สาขา = s.BranchDetail,
                    รหัสผู้รับผิดชอบ = s.EmployeeCode,
                    ผู้รับผิดชอบ = s.EmployeeName
                }).ToList();
            var result2 = _dataCenterV1Entities.usp_ChairmanManagementReportTab2_Select(branchId == -1 ? null : branchId, 0, 99999999, null, null, keyword)
                .Select(s => new
                {
                    สาขา = s.BranchDetail,
                    รหัสผู้รับผิดชอบ = s.FromEmployeeCode,
                    ผู้รับผิดชอบ = s.FromPersonName,
                    รหัสผู้รับผิดชอบใหม่ = s.ToEmployeeCode,
                    ผู้รับผิดชอบใหม่ = s.ToPersonName,
                    รหัสผู้ทำรายการ = s.CreateEmployeeCode,
                    ผู้ทำรายการ = s.CreatePersonName,
                    วันที่ทำรายการ = s.CreateDate
                }).ToList();

            var dt = GlobalFunction.ToDataTable(result);
            var dt2 = GlobalFunction.ToDataTable(result2);

            ExcelManager.ExportToExcel(this.HttpContext, "รายงานจัดการประธานสาขา", dt, "รายงานจัดการประธานสาขา", dt2, "ประวัติการเปลี่ยนประธานสาขา");
        }

        public void CustomerDirectorManagementExportV2(string provinceId = null, string districtId = null, string subDistrictId = null,
                                                             int? branchId = null, string keyword = null)
        {
            var rs = _dataCenterV1Entities.usp_DirectorSubDistrict_Select(provinceId, districtId == "-1" ? null : districtId, subDistrictId == "-1" | subDistrictId == "" ? null : subDistrictId, branchId == -1 ? null : branchId, 0, 99999999, null, null, keyword).Select(s => new
            {
                ตำบล = s.SubDistrictDetail,
                อำเภอ = s.DistrictDetail,
                จังหวัด = s.ProvinceDetail
                                                                                                                                                ,
                สาขา = s.BranchDetail,
                ผอ_บล = s.PersonName
            }).ToList();

            var rs1 = _dataCenterV1Entities.usp_DirectorSubDistrictLog_Select(provinceId, districtId == "-1" ? null : districtId, subDistrictId == "-1" | subDistrictId == "" ? null : subDistrictId, branchId == -1 ? null : branchId, 0, 99999999, null, null, keyword)
                .Select(s => new
                {
                    ตำบล = s.SubDistrictDetail,
                    อำเภอ = s.DistrictDetail,
                    จังหวัด = s.ProvinceDetail,
                    สาขา = s.BranchDetail,
                    รหัสผู้รับผิดชอบ = s.FromEmployeeCode,
                    ผู้รับผิดชอบ = s.FromPersonName,
                    รหัสผู้รับผิดชอบใหม่ = s.ToEmployeeCode,
                    ผู้รับผิดชอบใหม่ = s.ToPersonName,
                    รหัสผู้ทำรายการ = s.CreatedEmployeeCode,
                    ผู้ทำรายการ = s.CreatedPersonName,
                    วันที่ทำรายการ = s.CreatedDate
                }).ToList();

            var dt = GlobalFunction.ToDataTable(rs);
            var dt2 = GlobalFunction.ToDataTable(rs1);

            ExcelManager.ExportToExcel(this.HttpContext, "รายงานจัดการผอ.บล.", dt, "รายงานจัดการผอ.บล.", dt2, "ประวัติการเปลี่ยนแปลง ผอ.บล.");
        }

        #region GetData

        public JsonResult GetCustomerDirectorManagement(int? provinceId = null, string districtId = null,
                                                            int? branchId = null, int? draw = null, int? pageSize = null,
                                                            int? pageStart = null, string sortField = null,
                                                            string orderType = null, string search = null)
        {
            var list = _dataCenterV1Entities.usp_DirectorManagement_Select(provinceId, districtId == "-1" ? null : districtId, branchId == -1 ? null : branchId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDistrictByProvinceId(int? provinceId = null)
        {
            var rs = _dataCenterV1Entities.usp_District_ByProvinceID_Select(provinceId).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBusinessPromoteTeamByID(int? businessPromoteTeamId = null)
        {
            var rs = _dataCenterV1Entities.usp_BusinesPromoteTeam_Select(businessPromoteTeamId, 0, 99, null, null, null).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBusinessPromoteTeamMonitor(int? businessPromoteTeamId = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var list = _dataCenterV1Entities.usp_BusinesPromoteTeam_Select(businessPromoteTeamId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeDetailBySearch(string keyword)
        {
            var rs = _dataCenterV1Entities.usp_EmployeeSearch_Select(keyword).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataTablePerson(int draw, int pageSize, int pageStart, string sortField, string orderType, string search)
        {
            var list = _dataCenterV1Entities.usp_PersonSearch_SelectV2(pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPersonDetail(int? personId)
        {
            var result = _dataCenterV1Entities.usp_Person_Select(personId).FirstOrDefault();

            //var date = result.BirthDate.Value.ToString("dd/MM/yyyy");

            //result.BirthDate =

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataTableEmployee(int draw, int pageSize, int pageStart, string sortField, string orderType, string search)
        {
            var list = _dataCenterV1Entities.usp_Employee_SelectV2(null, null, null, null, null, null, null, null, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeDetail(string empCode)
        {
            var result = _dataCenterV1Entities.usp_EmployeeByEmployeeCode_Select(empCode).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataTableOrganize(int draw, int pageSize, int pageStart, string sortField, string orderType, string search)
        {
            var list = _dataCenterV1Entities.usp_Organize_Select(null, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOccupationLevel(int occupationId)
        {
            var result = _dataCenterV1Entities.usp_OccupationLevel_ByOccupationID_Select(occupationId).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDistrict(int provinceId)
        {
            var result = _dataCenterV1Entities.usp_District_ByProvinceID_Select(provinceId).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubDistrict(int districtId)
        {
            var result = _dataCenterV1Entities.usp_SubDistrict_ByDistrictID_Select(districtId).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetZipCode(int subdistrictId)
        {
            var result = _dataCenterV1Entities.usp_SubDistrict_Select(subdistrictId).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckCardDetail(string cardDetail)
        {
            var result = _dataCenterV1Entities.usp_PersonIDSearchByCardV2_Select(cardDetail, null).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRoles()
        {
            var result = _dataCenterV1Entities.usp_AspNetRoles_Select().ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRequestAllChairman(int? branchid, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _dataCenterV1Entities.usp_ChairmanManagement_Select(branchid == -1 ? null : branchid, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubDistrictByDistrictId(int? districtId = null)
        {
            var rs = _dataCenterV1Entities.usp_SubDistrict_ByDistrictID_Select(districtId).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerDirectorManagementV2(string provinceId = null, string districtId = null, string subDistrictId = null,
                                                            int? branchId = null, int? draw = null, int? pageSize = null,
                                                            int? pageStart = null, string sortField = null,
                                                            string orderType = null, string search = null)
        {
            var list = _dataCenterV1Entities.usp_DirectorSubDistrict_Select(provinceId, districtId == "-1" ? null : districtId, subDistrictId == "-1" | subDistrictId == "" ? null : subDistrictId, branchId == -1 ? null : branchId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion GetData

        #region InsertData

        public ActionResult UpdateCustomerDirector(string districtId = null, string employeeCode = null)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var customerDirect_userID = _dataCenterV1Entities.usp_UserIDByEmployeeCode_Select(employeeCode).Single();

            var result = _dataCenterV1Entities.usp_DirectorManagement_Update(districtId, customerDirect_userID, userID).Single();

            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result.ToString();
            lstArr[2] = result.Msg.ToString();

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateBusinessPromoteTeam(int? businessPromoteTeamId = null, string employeeCode = null, bool? isActive = null)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var businesPromoteTeam_userID = _dataCenterV1Entities.usp_UserIDByEmployeeCode_Select(employeeCode).Single();

            var result = _dataCenterV1Entities.usp_BusinesPromoteTeam_Update(businessPromoteTeamId, businesPromoteTeam_userID, isActive, userID).Single();

            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result.ToString();
            lstArr[2] = result.Msg.ToString();

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveBusinesPromoteTeam(string employeeCode)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var businesPromoteTeam_userID = _dataCenterV1Entities.usp_UserIDByEmployeeCode_Select(employeeCode).Single();

            var result = _dataCenterV1Entities.usp_BusinesPromoteTeam_Insert(businesPromoteTeam_userID, userID).Single();

            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result.ToString();
            lstArr[2] = result.Msg.ToString();

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreatePerson(FormCollection form)
        {
            var titleId = Convert.ToInt32(form["title"]);
            var firstName = form["input_FirstName"];
            var lastName = form["input_LastName"];
            var sexId = Convert.ToInt32(form["sex"]);
            var maritalStatusId = Convert.ToInt32(form["marital"]);
            var birthDate = Convert.ToDateTime(form["birthdate"]).ToString("dd/MM/yyyy", new CultureInfo("en-US"));
            var occupationLevelId = Convert.ToInt32(form["occupation"]);
            var identityCard = form["identityCardNumber"];
            var passport = form["passportNumber"];
            var taxIdentityNumber = form["input_TaxpayerNumber"];
            var tel = form["phone_number"];
            var mobile = form["phone_number2"];

            var buildingName = form["officeName"];
            var villageName = form["villageName"];
            var no = form["houseNo"];
            var moo = form["villageNo"];
            var floor = form["floor"];
            var room = form["room"];
            var soi = form["lane"];
            var road = form["road"];
            var subdistrictId = form["subdistrict"];
            var zipcode = form["zipcode"];

            var organizeId = Convert.ToInt32(form["input_organizeId"]);

            var createdByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var bDate = Convert.ToDateTime(birthDate);

            var returnValue = new ObjectParameter("ResultPerson_ID", typeof(int));

            var result = _dataCenterV1Entities.usp_OrganizePerson_Insert(titleId
                                                                            , firstName
                                                                            , lastName
                                                                            , ""
                                                                            , bDate
                                                                            , 0
                                                                            , 0
                                                                            , sexId
                                                                            , maritalStatusId
                                                                            , occupationLevelId
                                                                            , identityCard
                                                                            , passport
                                                                            , taxIdentityNumber
                                                                            , tel
                                                                            , mobile
                                                                            , buildingName
                                                                            , villageName
                                                                            , no
                                                                            , moo
                                                                            , floor
                                                                            , room
                                                                            , soi
                                                                            , road
                                                                            , subdistrictId
                                                                            , zipcode
                                                                            , organizeId
                                                                            , createdByUserId
                                                                            , returnValue).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateUser(FormCollection form)
        {
            var manager = new UserManager();

            var user = new ApplicationUser() { UserName = form["username"] };

            IdentityResult result = manager.Create(user, form["password"]);

            var personUser = new usp_PersonUser_Insert_Result();

            if (result.Succeeded)
            {
                foreach (var item in form.AllKeys)
                {
                    //Find chk_
                    var leftString = item.Substring(0, 4);
                    if (leftString == "chk_")
                    {
                        //Get RoleId
                        var roleId = item.Replace("chk_", "");

                        _dataCenterV1Entities.usp_AspNetUserRoles_Insert(user.Id, roleId);
                    }
                }

                var personId = Convert.ToInt32(form["personId"]);
                var empId = Convert.ToInt32(form["employeeId"]);

                personUser = _dataCenterV1Entities.usp_PersonUser_Insert(user.Id, personId, empId, GlobalFunction.GetLoginDetail(this.HttpContext).User_ID).FirstOrDefault();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetUpdateChairMan(int branchId, string userid)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var customerDirect_userID = _dataCenterV1Entities.usp_UserIDByEmployeeCode_Select(userid).Single();
            var result = _dataCenterV1Entities.usp_ChairmanManagement_Update(branchId, customerDirect_userID, userID).SingleOrDefault();

            var lstArr = new string[3];
            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result;
            lstArr[2] = result.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateCustomerDirectorV2(string subDistrictId = null, string employeeCode = null)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var customerDirect_userID = _dataCenterV1Entities.usp_UserIDByEmployeeCode_Select(employeeCode).Single();

            var result = _dataCenterV1Entities.usp_DirectorSubDistrict_Upsert(subDistrictId, customerDirect_userID, userID).Single();

            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result.ToString();
            lstArr[2] = result.Msg.ToString();

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        #endregion InsertData
    }
}