using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSCriticalIllness.Models;
using SmileSCriticalIllness.Helper;

namespace SmileSCriticalIllness.Controllers
{
    [Authorization]
    public class ApplicationController : Controller
    {
        #region Context

        private readonly CriticalIllnessEntities _context;

        public ApplicationController()
        {
            _context = new CriticalIllnessEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        #region View

        // GET: New Application
        public ActionResult NewApp(string appId = null, string leadID = null)
        {
            using (var db = new CriticalIllnessEntities())
            {
                if (appId != null && appId != "")
                {
                    try
                    {
                        var base64EncodedBytes = Convert.FromBase64String(appId);

                        appId = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                        ViewBag.AppId = appId;

                        ViewBag.AppStatus = _context.usp_ApplicationDetail_Select(null, null, null, null, null, null,
                            Convert.ToInt32(appId), null, null, null, null, null, null, null).FirstOrDefault()
                            ?.ApplicationStatusId;
                    }
                    catch (Exception)
                    {
                        ViewBag.AppId = null;
                    }
                }
                else
                {
                    ViewBag.AppStatus = 2;
                }

                ViewBag.CardType = db.usp_CardTypeId_Select(null, null, null, null, null, null).ToList();
                ViewBag.TitleName = db.usp_Title_Select(null, null, 999, null, null, null).ToList();
                ViewBag.Sex = db.usp_Sex_Select(null, null, null, null, null, null).ToList();
                ViewBag.MaritalStatus = db.usp_MaritalStatus_Select(null, null, null, null, null, null).ToList();
                ViewBag.BloodType = db.usp_BloodType_Select(null, null, null, null, null, null).ToList();
                ViewBag.Occupation = db.usp_Occupation_Select(null, null, 999, null, null, null).ToList();
                ViewBag.Province = db.usp_Province_Select(null, null, 99, null, null, null).ToList();
                ViewBag.RelationType = db.usp_RelationType_Select(null, null, 999, null, null, null).ToList();
                ViewBag.InsuranceCompany = db.usp_Organize_Select(null, null, 999, null, null, null).ToList();
                ViewBag.MemoTypeList = db.usp_MemoType_Select(null, null, null, null, null, null, null).ToList();

                ViewBag.LeadId = leadID;

                return View();
            }
        }

        // GET: Detail Application
        public ActionResult DetailApp(string appCode)
        {
            if (appCode == null && appCode == "")
            {
                return RedirectToAction("InternalServerError", "Error", new { errorText = "ไม่พบรายการ application นี้" });
            }
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(appCode);

                appCode = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                ViewBag.AppId = appCode;
            }
            catch (Exception)
            {
                ViewBag.AppId = null;
            }

            //call app detail
            var appDetail = _context.usp_ApplicationDetail_Select(null, null, null, null, null, null, null, appCode, null, null,
                99, null, null, null).FirstOrDefault();
            ViewBag.Application = appDetail;
            ViewBag.StartCoverDate = appDetail.StartCoverDate;
            ViewBag.appId_Int = appDetail.ApplicationId;

            if (appDetail == null)
            {
                return RedirectToAction("InternalServerError", "Error", new { errorText = "ไม่พบข้อมูล application นี้" });
            }
            //customer detail
            ViewBag.CustId = appDetail.CustId;

            //payer detail
            ViewBag.payerId = appDetail.PayerId;

            return View();
        }

        #endregion View

        #region method

        [HttpGet]
        public ActionResult CheckMaxCover(int? applicationId, int? productId)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var obj = db.usp_CheckMaxCover_Validate(applicationId, productId).FirstOrDefault();

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult CalculateBMI(int? applicationId, string startCoverDate)
        {
            using (var db = new CriticalIllnessEntities())
            {
                DateTime? nStartCoverDate = null;

                if (startCoverDate != "")
                {
                    nStartCoverDate = Convert.ToDateTime(startCoverDate);
                }

                var obj = db.usp_CalculateBMI_Select(applicationId, nStartCoverDate).FirstOrDefault();

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetDistrict(int? districtId, int? provinceId)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var lstDistrict = db.usp_District_Select(districtId, provinceId, null, 99999, null, null, null).ToList();

                return Json(lstDistrict, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetSubDistrict(int? subDistrictId, int? districtId, int? provinceId)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var lstSubDistrict = db.usp_SubDistrict_Select(subDistrictId, districtId, provinceId, null, 99999, null, null, null).ToList();

                return Json(lstSubDistrict, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetOccupationLevel(int? occupationId)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var lstOccupationLevel = db.usp_OccupationLevel_Select(null, occupationId, null, 999, null, null, null).ToList();

                return Json(lstOccupationLevel, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetRelationType(int? relationTypeId)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var lstRelationType = db.usp_RelationType_Select(relationTypeId, null, 999, null, null, null).ToList();

                return Json(lstRelationType, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult PersonSearch(int cardTypeId, string cardDetail)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var result = db.usp_PersonSearch_Select(cardTypeId, cardDetail).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AgentSearch(string search)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var result = db.usp_Employee_Select(null, null, null, null, 0, 10, null, null, search).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult CarSearch(string search)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var result = db.usp_Zebra_Select(null, 0, 10, null, null, search).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult CalculateAge(string birthDate)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var dateFrom = Convert.ToDateTime(birthDate);

                var dateCurrent = DateTime.Now;

                var result = db.usp_CalculateAge_Select(dateFrom, dateCurrent).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetNewAppCustomer(int applicationId)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var result = db.usp_NewApp_CustomerByAppId_Select(applicationId).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetNewAppPayer(int applicationId)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var result = db.usp_NewApp_PayerByAppId_Select(applicationId).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetNewAppPolicy(int applicationId)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var result = db.usp_NewApp_PolicyByAppId_Select(applicationId).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetNewAppQuestionnaire(int applicationId)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var result = db.usp_Questionaire_Select(null, applicationId, null, null, null, null, null).FirstOrDefault();

                if (result == null) return Json(0, JsonRequestBehavior.AllowGet);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetNewAppHeirs(int draw, int applicationId, int? indexStart, int? pageSize, string sortField, string orderType, string search)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var lst = db.usp_Heir_Select(applicationId, indexStart, pageSize, sortField, orderType, search).ToList();

                var dt = new Dictionary<string, object>
                {
                    {"draw",draw },
                    {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"data", lst.ToList()}
                };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetNewAppHeirsById(int personRelateApplicationId)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var obj = db.usp_Heir_SelectById(personRelateApplicationId).FirstOrDefault();

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetProduct(int? productId, int? insuranceCompanyId, string cusBirthDate, string startCoverDate)
        {
            using (var db = new CriticalIllnessEntities())
            {
                DateTime? dateFrom = null;

                if (cusBirthDate != "")
                {
                    dateFrom = Convert.ToDateTime(cusBirthDate);
                }

                var dateTo = Convert.ToDateTime(startCoverDate);

                var result = db.usp_Product_Select(productId, insuranceCompanyId, dateFrom, dateTo, 0, 99, null, null, null).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetProductByProductId(int? productId)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var result = db.usp_Product_Select(productId, null, null, null, 0, 99, null, null, null).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        private void CustomerApplicationInsertOrUpdate(int applicationId, string CustType)
        {
            var userDetail = GlobalFunction.GetLoginDetail(this.HttpContext);
            var CreatedByUserId = userDetail.User_ID;

            using (var db = new CriticalIllnessEntities())
            {
                var rsCustomerApplication = db.usp_CustomerApplication_CI_Person_InsertOrUpdate(applicationId, CustType, CreatedByUserId);
            }
        }

        [HttpPost]
        public ActionResult InsertNewApp(FormCollection form)
        {
            var userDetail = GlobalFunction.GetLoginDetail(this.HttpContext);
            var TitleId = Convert.ToInt32(form["titleName"]);
            var FirstName = form["fName"];
            var LastName = form["lName"];
            var BirthDate = Convert.ToDateTime(form["birthDate"]);

            var IdentityCardNo = form["identityCard"];
            var PassportNo = form["passport"];

            var SexId = Convert.ToInt32(form["sex"]);
            var MaritalStatusId = Convert.ToInt32(form["status"]);
            var BloodTypeId = Convert.ToInt32(form["bloodGroup"]);
            var OccupationLevelId = Convert.ToInt32(form["occupationLevel"]);
            var Position = form["position"];
            var JobDetail = form["jobDescription"];
            var Salary1 = Convert.ToDouble(form["income"]);
            var Salary2 = Convert.ToDouble(form["otherIncome"]);
            var Weight = Convert.ToDouble(form["weight"]);
            var Height = Convert.ToDouble(form["height"]);

            var MobilePhoneNumber = form["mobilePhone"];
            var OfficePhoneNumber = form["officePhone"];
            var Email = form["email"];

            var C_BuildingName = form["organizationName_C"];
            var C_VillageName = form["village_C"];
            var C_No = form["addressNo_C"];
            var C_Moo = form["unit_C"];
            var C_Floor = form["floor_C"];
            var C_Soi = form["alley_C"];
            var C_Road = form["road_C"];
            var C_SubDistrictId = form["sub-district_C"];

            var H_BuildingName = form["organizationName_H"];
            var H_VillageName = form["village_H"];
            var H_No = form["addressNo_H"];
            var H_Moo = form["unit_H"];
            var H_Floor = form["floor_H"];
            var H_Soi = form["alley_H"];
            var H_Road = form["road_H"];
            var H_SubDistrictId = form["sub-district_H"];

            var W_BuildingName = form["organizationName_W"];
            var W_VillageName = form["village_W"];
            var W_No = form["addressNo_W"];
            var W_Moo = form["unit_W"];
            var W_Floor = form["floor_W"];
            var W_Soi = form["alley_W"];
            var W_Road = form["road_W"];
            var W_SubDistrictId = form["sub-district_W"];

            var LeadId = form["LeadId_hidden"];

            var BranchId = userDetail.Branch_ID;
            var CreatedByUserId = userDetail.User_ID;

            using (var db = new CriticalIllnessEntities())
            {
                var result = db.usp_NewApp_Customer_Insert(TitleId, FirstName, LastName, BirthDate, IdentityCardNo, PassportNo, SexId, MaritalStatusId, BloodTypeId, OccupationLevelId, Position, JobDetail, Salary1, Salary2, Weight, Height, MobilePhoneNumber, OfficePhoneNumber, Email, H_BuildingName, H_VillageName, H_No, H_Moo, H_Floor, H_Soi, H_Road, H_SubDistrictId, W_BuildingName, W_VillageName, W_No, W_Moo, W_Floor, W_Soi, W_Road, W_SubDistrictId, C_BuildingName, C_VillageName, C_No, C_Moo, C_Floor, C_Soi, C_Road, C_SubDistrictId, BranchId, CreatedByUserId).FirstOrDefault();

                if (result.IsResult == true)
                {
                    if (LeadId != null && LeadId != "")
                    {
                        db.usp_CRMLead_Insert(result.Result, 13, LeadId);
                    }

                    CustomerApplicationInsertOrUpdate(Int32.Parse(result.Result), "C");
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateNewApp(FormCollection form)
        {
            var userDetail = GlobalFunction.GetLoginDetail(this.HttpContext);

            var applicationId = Convert.ToInt32(form["applicationId_hidden"]);
            var TitleId = Convert.ToInt32(form["titleName"]);
            var FirstName = form["fName"];
            var LastName = form["lName"];
            var BirthDate = Convert.ToDateTime(form["birthDate"]);

            var IdentityCardNo = form["identityCard"];
            var PassportNo = form["passport"];

            var SexId = Convert.ToInt32(form["sex"]);
            var MaritalStatusId = Convert.ToInt32(form["status"]);
            var BloodTypeId = Convert.ToInt32(form["bloodGroup"]);
            var OccupationLevelId = Convert.ToInt32(form["occupationLevel"]);
            var Position = form["position"];
            var JobDetail = form["jobDescription"];
            var Salary1 = Convert.ToDouble(form["income"]);
            var Salary2 = Convert.ToDouble(form["otherIncome"]);
            var Weight = Convert.ToDouble(form["weight"]);
            var Height = Convert.ToDouble(form["height"]);

            var MobilePhoneNumber = form["mobilePhone"];
            var OfficePhoneNumber = form["officePhone"];
            var Email = form["email"];

            var C_BuildingName = form["organizationName_C"];
            var C_VillageName = form["village_C"];
            var C_No = form["addressNo_C"];
            var C_Moo = form["unit_C"];
            var C_Floor = form["floor_C"];
            var C_Soi = form["alley_C"];
            var C_Road = form["road_C"];
            var C_SubDistrictId = form["sub-district_C"];

            var H_BuildingName = form["organizationName_H"];
            var H_VillageName = form["village_H"];
            var H_No = form["addressNo_H"];
            var H_Moo = form["unit_H"];
            var H_Floor = form["floor_H"];
            var H_Soi = form["alley_H"];
            var H_Road = form["road_H"];
            var H_SubDistrictId = form["sub-district_H"];

            var W_BuildingName = form["organizationName_W"];
            var W_VillageName = form["village_W"];
            var W_No = form["addressNo_W"];
            var W_Moo = form["unit_W"];
            var W_Floor = form["floor_W"];
            var W_Soi = form["alley_W"];
            var W_Road = form["road_W"];
            var W_SubDistrictId = form["sub-district_W"];

            var BranchId = userDetail.Branch_ID;
            var CreatedByUserId = userDetail.User_ID;

            using (var db = new CriticalIllnessEntities())
            {
                var result = db.usp_NewApp_Customer_Update(applicationId, TitleId, FirstName, LastName, BirthDate, IdentityCardNo, PassportNo, SexId, MaritalStatusId, BloodTypeId, OccupationLevelId, Position, JobDetail, Salary1, Salary2, Weight, Height, MobilePhoneNumber, OfficePhoneNumber, Email, H_BuildingName, H_VillageName, H_No, H_Moo, H_Floor, H_Soi, H_Road, H_SubDistrictId, W_BuildingName, W_VillageName, W_No, W_Moo, W_Floor, W_Soi, W_Road, W_SubDistrictId, C_BuildingName, C_VillageName, C_No, C_Moo, C_Floor, C_Soi, C_Road, C_SubDistrictId, CreatedByUserId).FirstOrDefault();

                if (result.IsResult == true)
                {
                    CustomerApplicationInsertOrUpdate(applicationId, "C");
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateNewAppPayer(FormCollection form)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var userDetail = GlobalFunction.GetLoginDetail(this.HttpContext);
                var applicationId = Convert.ToInt32(form["applicationIdPayer_hidden"]);
                var RelationTypeId = Convert.ToInt32(form["relationType"]);

                var BranchId = userDetail.Branch_ID;
                var CreatedByUserId = userDetail.User_ID;

                //บุคคลเดียวกัน ใช้ข้อมูลผู้เอาประกัน
                if (RelationTypeId == 39)
                {
                    var obj = db.usp_NewApp_CustomerByAppId_Select(applicationId).FirstOrDefault();

                    var result1 = db.usp_NewApp_Payer_Update(obj.ApplicationId, RelationTypeId, obj.TitleId, obj.FirstName, obj.LastName, obj.IdentityCardNo, obj.PassportNo, obj.OccupationLevelId, obj.MobilePhoneNumber, obj.Email, obj.H_BuildingName, obj.H_VillageName, obj.H_No, obj.H_Moo, obj.H_Floor, obj.H_Soi, obj.H_Road, obj.H_SubDistrictId, obj.W_BuildingName, obj.W_VillageName, obj.W_No, obj.W_Moo, obj.W_Floor, obj.W_Soi, obj.W_Road, obj.W_SubDistrictId, CreatedByUserId).FirstOrDefault();

                    if (result1.IsResult == true)
                    {
                        CustomerApplicationInsertOrUpdate(applicationId, "P");
                    }

                    return Json(result1, JsonRequestBehavior.AllowGet);
                }

                var TitleId = Convert.ToInt16(form["titleNamePayer"]);
                var FirstName = form["fNamePayer"];
                var LastName = form["lNamePayer"];
                var IdentityCardNo = form["identityCardPayer"];
                var PassportNo = form["passportPayer"];
                var OccupationLevelId = Convert.ToInt32(form["occupationLevelPayer"]);
                var MobilePhoneNumber = form["mobilePhonePayer"];
                var Email = form["emailPayer"];

                var H_BuildingName = form["organizationNamePayer_H"];
                var H_VillageName = form["villagePayer_H"];
                var H_No = form["addressNoPayer_H"];
                var H_Moo = form["unitPayer_H"];
                var H_Floor = form["floorPayer_H"];
                var H_Soi = form["alleyPayer_H"];
                var H_Road = form["roadPayer_H"];
                var H_SubDistrictId = form["sub-districtPayer_H"];

                var W_BuildingName = form["organizationNamePayer_W"];
                var W_VillageName = form["villagePayer_W"];
                var W_No = form["addressNoPayer_W"];
                var W_Moo = form["unitPayer_W"];
                var W_Floor = form["floorPayer_W"];
                var W_Soi = form["alleyPayer_W"];
                var W_Road = form["roadPayer_W"];
                var W_SubDistrictId = form["sub-districtPayer_W"];

                var result2 = db.usp_NewApp_Payer_Update(applicationId, RelationTypeId, TitleId, FirstName, LastName, IdentityCardNo, PassportNo, OccupationLevelId, MobilePhoneNumber, Email, H_BuildingName, H_VillageName, H_No, H_Moo, H_Floor, H_Soi, H_Road, H_SubDistrictId, W_BuildingName, W_VillageName, W_No, W_Moo, W_Floor, W_Soi, W_Road, W_SubDistrictId, CreatedByUserId).FirstOrDefault();

                if (result2.IsResult == true)
                {
                    CustomerApplicationInsertOrUpdate(applicationId, "P");
                }

                return Json(result2, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateNewAppPolicy(FormCollection form)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var userDetail = GlobalFunction.GetLoginDetail(this.HttpContext);
                var applicationId = Convert.ToInt32(form["applicationIdPolicy_hidden"]);
                var productId = Convert.ToInt32(form["product"]);
                var startCoverDate = Convert.ToDateTime(form["startCoverDate"]);
                var endCoverDate = Convert.ToDateTime(form["endCoverDate"]);
                var billBook = form["billBook"];
                var billNo = form["billNo"];
                var billDate = Convert.ToDateTime(form["billDate"]);
                var disclosureStatusId = Convert.ToInt32(form["allowRD"]);
                var saleEmpId = Convert.ToInt32(form["agentCode"]);
                //var zebraId = Convert.ToInt32(form["carCode"]);

                //var carDetail = db.usp_Zebra_Select(zebraId, 0, 10, null, null, null).FirstOrDefault();

                //var zebraOwnerEmpId = carDetail.OwnerEmployeeId;
                var zebraId = 0;
                var zebraOwnerEmpId = Convert.ToInt32(form["carCode"]);
                var updatedByUserId = userDetail.User_ID;

                var result = db.usp_NewApp_Policy_Update(applicationId, productId, startCoverDate, endCoverDate, billBook, billNo, billDate, disclosureStatusId, saleEmpId, zebraId, zebraOwnerEmpId, updatedByUserId).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult InsertHeirs(FormCollection form)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var userDetail = GlobalFunction.GetLoginDetail(this.HttpContext);
                var ApplicationId = Convert.ToInt32(form["applicationIdHeirs_hidden"]);
                var RelationId = Convert.ToInt32(form["relationHeirs"]);

                //บุคคลเดียวกัน ใช้ข้อมูลผู้เอาประกัน
                if (RelationId == 39)
                {
                    var obj = db.usp_NewApp_CustomerByAppId_Select(ApplicationId).FirstOrDefault();

                    var result1 = db.usp_Heir_Insert(obj.ApplicationId, RelationId, obj.TitleId, obj.FirstName, obj.LastName, obj.H_BuildingName, obj.H_VillageName, obj.H_No, obj.H_Moo, obj.H_Floor, obj.H_Soi, obj.H_Road, obj.H_SubDistrictId, userDetail.User_ID).FirstOrDefault();

                    return Json(result1, JsonRequestBehavior.AllowGet);
                }

                var TitleId = Convert.ToInt32(form["titleNameHeirs"]);
                var FirstName = form["fNameHeirs"];
                var LastName = form["lNameHeirs"];
                var BuildingName = form["organizationNameHeirs"];
                var VillageName = form["villageHeirs"];
                var No = form["addressNoHeirs"];
                var Moo = form["unitHeirs"];
                var Floor = form["floorHeirs"];
                var Soi = form["alleyHeirs"];
                var Road = form["roadHeirs"];
                var SubDistrictId = form["sub-DistrictHeirs"];
                var CreatedByUserId = userDetail.User_ID;

                var result = db.usp_Heir_Insert(ApplicationId, RelationId, TitleId, FirstName, LastName, BuildingName, VillageName, No, Moo, Floor, Soi, Road, SubDistrictId, CreatedByUserId).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateHeirs(FormCollection form)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var userDetail = GlobalFunction.GetLoginDetail(this.HttpContext);
                var PersonRelateApplicationId = Convert.ToInt32(form["personRelateApplicationId_hidden"]);
                var ApplicationId = Convert.ToInt32(form["applicationIdHeirs_hidden"]);
                var RelationId = Convert.ToInt32(form["relationHeirs"]);

                //บุคคลเดียวกัน ใช้ข้อมูลผู้เอาประกัน
                if (RelationId == 39)
                {
                    var obj = db.usp_NewApp_CustomerByAppId_Select(ApplicationId).FirstOrDefault();

                    var result1 = db.usp_Heir_Update(PersonRelateApplicationId, RelationId, obj.TitleId, obj.FirstName, obj.LastName, obj.H_BuildingName, obj.H_VillageName, obj.H_No, obj.H_Moo, obj.H_Floor, obj.H_Soi, obj.H_Road, obj.H_SubDistrictId, userDetail.User_ID).FirstOrDefault();

                    return Json(result1, JsonRequestBehavior.AllowGet);
                }

                var TitleId = Convert.ToInt32(form["titleNameHeirs"]);
                var FirstName = form["fNameHeirs"];
                var LastName = form["lNameHeirs"];
                var BuildingName = form["organizationNameHeirs"];
                var VillageName = form["villageHeirs"];
                var No = form["addressNoHeirs"];
                var Moo = form["unitHeirs"];
                var Floor = form["floorHeirs"];
                var Soi = form["alleyHeirs"];
                var Road = form["roadHeirs"];
                var SubDistrictId = form["sub-DistrictHeirs"];
                var CreatedByUserId = userDetail.User_ID;

                var result = db.usp_Heir_Update(PersonRelateApplicationId, RelationId, TitleId, FirstName, LastName, BuildingName, VillageName, No, Moo, Floor, Soi, Road, SubDistrictId, CreatedByUserId).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteHeirs(int relateApplicationId)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var userDetail = GlobalFunction.GetLoginDetail(this.HttpContext);
                var PersonRelateApplicationId = Convert.ToInt32(relateApplicationId);

                var result = db.usp_Heir_Delete(PersonRelateApplicationId, userDetail.User_ID).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult InsertQuestionnaire(FormCollection form)
        {
            using (var db = new CriticalIllnessEntities())
            {
                var userDetail = GlobalFunction.GetLoginDetail(this.HttpContext);
                var ApplicationId = Convert.ToInt32(form["applicationIdQuestionnaire_hidden"]);

                //Declare
                var a1 = false;
                var a1_01 = false;
                var a1_02 = false;
                var a1_03 = false;
                var a1_04 = false;
                var a1_05 = false;
                var a1_06 = false;
                var a1_07 = "";

                var a2 = false;
                var a2_01 = false;
                var a2_02 = false;

                var a3 = false;
                var a3_01 = false;
                var a3_02 = false;
                var a3_03 = false;
                var a3_04 = false;
                var a3_05 = false;
                var a3_06 = false;
                var a3_07 = false;
                var a3_08 = false;
                var a3_08_01 = "";
                var a3_08_02 = "";
                var a3_08_03 = false;
                var a3_08_04 = false;
                var a3_08_05 = false;
                var a3_08_06 = "";
                var a3_08_07 = "";
                var a3_08_08 = false;
                var a3_08_09 = false;
                var a3_08_10 = false;
                var a3_08_11 = false;
                var a3_08_12 = false;
                var a3_08_13 = false;
                var a3_09 = false;
                var a3_09_01 = "";
                var a3_09_02 = "";
                var a3_09_03 = false;
                var a3_09_04 = false;
                var a3_09_05 = false;
                var a3_09_06 = "";
                var a3_09_07 = "";
                var a3_09_08 = false;
                var a3_09_09 = false;
                var a3_09_10 = false;
                var a3_09_11 = false;
                var a3_09_12 = false;
                var a3_09_13 = false;

                var a4 = false;
                var a4_01 = "";
                var a4_02 = "";

                //Check Value & Set Value
                //A1
                var List_A1 = Convert.ToBoolean(Convert.ToInt16(form["A1"]));
                switch (List_A1)
                {
                    case true:
                        a1 = true;
                        a1_07 = form["A1_07"];
                        var List_ChildA1 = form["child_A1"].Split(',');

                        foreach (var item in List_ChildA1)
                        {
                            switch (item.ToString())
                            {
                                case "A1_01":
                                    a1_01 = true;
                                    break;

                                case "A1_02":
                                    a1_02 = true;
                                    break;

                                case "A1_03":
                                    a1_03 = true;
                                    break;

                                case "A1_04":
                                    a1_04 = true;
                                    break;

                                case "A1_05":
                                    a1_05 = true;
                                    break;

                                case "A1_06":
                                    a1_06 = true;
                                    break;

                                default:
                                    break;
                            }
                        }
                        break;

                    default:
                        break;
                }

                //A2
                var List_A2 = Convert.ToBoolean(Convert.ToInt16(form["A2"]));
                switch (List_A2)
                {
                    case true:
                        a2 = true;
                        var ChildA2 = form["child_A2"];

                        switch (ChildA2)
                        {
                            case "A2_01":
                                a2_01 = true;
                                break;

                            case "A2_02":
                                a2_02 = true;
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }

                //A3
                var List_A3 = Convert.ToBoolean(Convert.ToInt16(form["A3"]));
                switch (List_A3)
                {
                    case true:
                        a3 = true;
                        var ChildA3 = form["child_A3"];

                        switch (ChildA3)
                        {
                            case "A3_01":
                                a3_01 = true;
                                break;

                            case "A3_02":
                                a3_02 = true;
                                break;

                            case "A3_03":
                                a3_03 = true;
                                break;

                            case "A3_04":
                                a3_04 = true;
                                break;

                            case "A3_05":
                                a3_05 = true;
                                break;

                            case "A3_06":
                                a3_06 = true;
                                break;

                            case "A3_07":
                                a3_07 = true;
                                break;

                            case "A3_08":
                                a3_08 = true;
                                a3_08_01 = form["A3_08_01"];
                                a3_08_02 = form["A3_08_02"];

                                var ChildA3_08 = form["child_A3_08"];
                                switch (ChildA3_08)
                                {
                                    case "A3_08_03":
                                        a3_08_03 = true;
                                        break;

                                    case "A3_08_04":
                                        a3_08_04 = true;
                                        break;

                                    case "A3_08_05":
                                        a3_08_05 = true;
                                        break;

                                    default:
                                        break;
                                }

                                a3_08_06 = form["A3_08_06"];
                                a3_08_07 = form["A3_08_07"];

                                var ChildA3_08_Sub_08_09 = form["childA3_08_Sub_08_09"];
                                switch (ChildA3_08_Sub_08_09)
                                {
                                    case "A3_08_08":
                                        a3_08_08 = true;
                                        break;

                                    case "A3_08_09":
                                        a3_08_09 = true;
                                        break;

                                    default:
                                        break;
                                }

                                var ChildA3_08_Sub_10_11_12_13 = form["childA3_08_Sub_10_11_12_13"];
                                switch (ChildA3_08_Sub_10_11_12_13)
                                {
                                    case "A3_08_10":
                                        a3_08_10 = true;
                                        break;

                                    case "A3_08_11":
                                        a3_08_11 = true;
                                        break;

                                    case "A3_08_12":
                                        a3_08_12 = true;
                                        break;

                                    case "A3_08_13":
                                        a3_08_13 = true;
                                        break;

                                    default:
                                        break;
                                }
                                break;

                            case "A3_09":
                                a3_09 = true;
                                a3_09_01 = form["A3_09_01"];
                                a3_09_02 = form["A3_09_02"];

                                var ChildA3_09 = form["child_A3_09"];
                                switch (ChildA3_09)
                                {
                                    case "A3_09_03":
                                        a3_09_03 = true;
                                        break;

                                    case "A3_09_04":
                                        a3_09_04 = true;
                                        break;

                                    case "A3_09_05":
                                        a3_09_05 = true;
                                        break;

                                    default:
                                        break;
                                }

                                a3_09_06 = form["A3_09_06"];
                                a3_09_07 = form["A3_09_07"];

                                var ChildA3_09_Sub_08_09 = form["childA3_09_Sub_08_09"];
                                switch (ChildA3_09_Sub_08_09)
                                {
                                    case "A3_09_08":
                                        a3_09_08 = true;
                                        break;

                                    case "A3_09_09":
                                        a3_09_09 = true;
                                        break;

                                    default:
                                        break;
                                }

                                var ChildA3_09_Sub_10_11_12_13 = form["childA3_09_Sub_10_11_12_13"];
                                switch (ChildA3_09_Sub_10_11_12_13)
                                {
                                    case "A3_09_10":
                                        a3_09_10 = true;
                                        break;

                                    case "A3_09_11":
                                        a3_09_11 = true;
                                        break;

                                    case "A3_09_12":
                                        a3_09_12 = true;
                                        break;

                                    case "A3_09_13":
                                        a3_09_13 = true;
                                        break;

                                    default:
                                        break;
                                }
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }

                //A4
                var List_A4 = Convert.ToBoolean(Convert.ToInt16(form["A4"]));
                switch (List_A4)
                {
                    case true:
                        a4 = true;
                        a4_01 = form["A4_01"];
                        a4_02 = form["A4_02"];
                        break;

                    default:
                        break;
                }

                var result = db.usp_Questionaire_InsertOrUpdate(ApplicationId,
                                                                userDetail.User_ID,
                                                                a1,
                                                                a1_01,
                                                                a1_02,
                                                                a1_03,
                                                                a1_04,
                                                                a1_05,
                                                                a1_06,
                                                                a1_07,
                                                                a2,
                                                                a2_01,
                                                                a2_02,
                                                                a3,
                                                                a3_01,
                                                                a3_02,
                                                                a3_03,
                                                                a3_04,
                                                                a3_05,
                                                                a3_06,
                                                                a3_07,
                                                                a3_08,
                                                                a3_08_01,
                                                                a3_08_02,
                                                                a3_08_03,
                                                                a3_08_04,
                                                                a3_08_05,
                                                                a3_08_06,
                                                                a3_08_07,
                                                                a3_08_08,
                                                                a3_08_09,
                                                                a3_08_10,
                                                                a3_08_11,
                                                                a3_08_12,
                                                                a3_08_13,
                                                                a3_09,
                                                                a3_09_01,
                                                                a3_09_02,
                                                                a3_09_03,
                                                                a3_09_04,
                                                                a3_09_05,
                                                                a3_09_06,
                                                                a3_09_07,
                                                                a3_09_08,
                                                                a3_09_09,
                                                                a3_09_10,
                                                                a3_09_11,
                                                                a3_09_12,
                                                                a3_09_13,
                                                                a4,
                                                                a4_01,
                                                                a4_02).FirstOrDefault();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //add memo
        [HttpPost]
        public ActionResult MemoInsert(int applicationId, int memoTypeId, string memoText)
        {
            var createdByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _context.usp_ApplicationMemo_Insert(applicationId, memoTypeId, memoText, createdByUserId).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CheckDoc(int applicationId)
        {
            var result = _context.usp_CheckDocument_Validate(applicationId).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SendToApprove(int applicationId)
        {
            var updatedByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _context.usp_NewApp_SentToUdw_Update(applicationId, updatedByUserId).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion method
    }
}