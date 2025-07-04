using SmileSClaimOnLine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSClaimOnLine.Helper;
using SmileSClaimOnLine.EmployeeService;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Data.Entity.Core.Objects;

namespace SmileSClaimOnLine.Controllers
{
    [Authorization]
    public class ClaimController : Controller
    {
        #region dbContext

        private ClaimOnLineDBContext _context;

        public ClaimController()

        {
            _context = new ClaimOnLineDBContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion dbContext

        private const string _controllerName = nameof(ClaimController);

        // GET: Claim
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ClaimOnLineSearch(string claimonlineCode = null)
        {
            if (claimonlineCode != null)
            {
                //DeCode
                var queueBase64EncodedBytes = Convert.FromBase64String(claimonlineCode);
                var ClaimOnLineCode = System.Text.Encoding.UTF8.GetString(queueBase64EncodedBytes);

                ViewBag.ClaimOnlineCode = ClaimOnLineCode;
            }
            else
            {
                ViewBag.ClaimOnlineCode = "0";
            }

            ViewBag.Zone = _context.usp_Zone_Select(null, true).ToList();
            return View();
        }

        [Authorization(Roles = "Developer")]
        public ActionResult CreateClaimForPH(string claimonlineCode)
        {
            //DeCode
            var queueBase64EncodedBytes = Convert.FromBase64String(claimonlineCode);
            var ClaimOnLineCode = System.Text.Encoding.UTF8.GetString(queueBase64EncodedBytes);

            ViewBag.ClaimOnlineCode = ClaimOnLineCode;
            ViewBag.ClaimType = _context.usp_ClaimType_Select(0, 999, null, null, null).ToList().Where(s => s.Code == "2000");
            ViewBag.StatusClaim = _context.usp_StatusGroupClaim_Select(0, 999, null, null, null).ToList();
            ViewBag.Province = _context.usp_Province_Select(0, 999, null, null, null).ToList();
            return View();
        }

        [Authorization(Roles = "Developer")]
        public ActionResult CreateClaimForPA(string claimonlineCode)
        {
            //DeCode
            var queueBase64EncodedBytes = Convert.FromBase64String(claimonlineCode);
            var ClaimOnLineCode = System.Text.Encoding.UTF8.GetString(queueBase64EncodedBytes);

            ViewBag.ClaimOnlineCode = ClaimOnLineCode;

            ViewBag.CodeGroupPAYear = _context.usp_CodeGroupPAYear_Select(0, 9999, null, null, null).ToList();
            ViewBag.Province = _context.usp_Province_Select(0, 999, null, null, null).ToList();
            ViewBag.StatusClaim = _context.usp_StatusGroupClaim_Select(0, 999, null, null, null).ToList();
            ViewBag.ClaimStyle = _context.usp_ClaimStyle_Select(0, 9999, null, null, null).ToList().Where(s => s.Code == "4140");
            ViewBag.Branch = _context.usp_Branch_Select(null, null, true).ToList();
            return View();
        }

        /// <summary>
        /// สร้าง COL
        /// </summary>
        /// <returns></returns>
        [Authorization(Roles = "Developer")]
        public ActionResult CreateCOL()
        {
            ViewBag.Province = _context.usp_Province_Select(0, 999, null, null, null).ToList();
            ViewBag.StatusClaim = _context.usp_StatusGroupClaim_Select(0, 999, null, null, null).ToList().Where(x => x.Code == "3521");
            ViewBag.ClaimType = _context.usp_ClaimType_Select(0, 999, null, null, null).ToList();

            ViewBag.Bank = _context.usp_Bank_Select(null, null, true).ToList().Where(x => x.Bank_ID != 2);

            var result = _context.usp_ProductType_Select(null, null).ToList();
            var ww = result.Where(x => x.ProductType_ID == 6).ToList();

            ViewBag.PersonContactType = _context.usp_PersonContactType_Select(2).ToList();

            ViewBag.LinkOverview = Properties.Settings.Default.LinkClaimOverview;

            ViewBag.ProductType = ww;

            return View();
        }

        /// <summary>
        /// สร้าง COL PA
        /// </summary>
        /// <returns></returns>
        [Authorization(Roles = "Developer")]
        public ActionResult CreateCOLPA()
        {
            var userDetail = GlobalFunction.GetLoginDetail(this.HttpContext);

            ViewBag.Province = _context.usp_Province_Select(0, 999, null, null, null).ToList();
            ViewBag.StatusClaim = _context.usp_StatusGroupClaim_Select(0, 999, null, null, null).ToList().Where(x => x.Code == "3521"); ;
            ViewBag.ClaimType = _context.usp_ClaimType_Select(0, 999, null, null, null).ToList();
            ViewBag.CodeGroupPAYear = _context.usp_CodeGroupPAYear_Select(0, 9999, null, null, null).ToList();

            var branch = _context.usp_BranchByAreaId_Select(userDetail.Branch_ID, null, null, null, null, null, null, null).FirstOrDefault();
            ViewBag.BranchCode = branch.BranchCode;
            ViewBag.Bank = _context.usp_Bank_Select(null, null, true).ToList().Where(x => x.Bank_ID != 2);

            ViewBag.PersonContactType = _context.usp_PersonContactType_Select(3).ToList();

            var result = _context.usp_ProductType_Select(null, null).ToList();
            var ww = result.Where(x => x.ProductType_ID == 26).ToList();

            ViewBag.LinkOverview = Properties.Settings.Default.LinkClaimOverviewPA;

            ViewBag.ProductType = ww;
            return View();
        }

        public ActionResult ClaimDetailMonitoring()
        {
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();

            var roleMO = new[] { "SmileSClaimOnLine_Operation" }; //MO
            var roleDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var roleClaimonLine = new[] { "SmileSClaimOnLine_Manager" }; //ฝ่าย PA Underwrite
            var roleSupervisor = new[] { "SmileSClaimOnLine_Supervisor" };
            
            
            if (rolelist.Intersect(roleDev).Count() > 0)
            {
                ViewBag.IsAddClaim = true;
                ViewBag.ShowAll = true;
                return View();
            }
            //ถ้าเป็น สิทธิ์ของ Dev และ PA ให้แสดงทั้งหมด
            else if (rolelist.Intersect(roleClaimonLine).Count() > 0 || rolelist.Intersect(roleDev).Count() > 0 || rolelist.Intersect(roleSupervisor).Count() > 0)
            {
                ViewBag.IsAddClaim = false;
                ViewBag.ShowAll = true;
                return View();
            }
            //ถ้าเป็น สิทธิ์ของ MO ให้แสดงแค่ของตัวเอง
            else if (rolelist.Intersect(roleMO).Count() > 0)
            {
                ViewBag.IsAddClaim = false;
                ViewBag.ShowAll = false;
                return View();
            }
            else
            {
                return RedirectToAction("UnAuthorize", "Auth");
            }
        }

        #region "Method"

        public ActionResult GetHospitalNameForPHDetail(string q, string provinceId)
        {
            var rs = _context.usp_CompanyGroupHospital_Select(provinceId, 0, 10, null, null, q).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetICDTen(string q)
        {
            var rs = (from icdten in _context.vw_ICD10
                      where icdten.Expr1.Contains(q)
                      select icdten).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEmployeeDetail(string q)
        {
            var rs = _context.usp_GetEmployeeDetail_Select(0, 10, null, null, q).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCancelCauseClaimDetail()
        {
            var rs = _context.usp_CodeGroup_Select("9420", 0, 9999, null, null, null).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClaimOnlineMonitorDetail(string claimOnlineCode, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            //int? Zone_ID = null;
            //int? Branch_Id = null;

            //if (zoneId != "-1" && zoneId != "")
            //{
            //    Zone_ID = Convert.ToInt32(zoneId);
            //}

            //if (branchId != "-1" && branchId != "")
            //{
            //    Branch_Id = Convert.ToInt32(branchId);
            //}

            var rs = _context.usp_CreateClaimOnLine_Select(claimOnlineCode, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", rs.Count() != 0 ? rs.FirstOrDefault()?.TotalCount : rs.Count()},
                {"recordsFiltered", rs.Count() != 0 ? rs.FirstOrDefault()?.TotalCount : rs.Count()},
                {"data", rs.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #region "PA"
        [Authorization(Roles = "Developer")]
        public ActionResult SavePAClaim(string customerDetailCode, string hospitalId, string claimTypeId, string claimStypeId, string dateHappen, string dateNotice,
                                        string dateIn, string dateOut, string accidentCauseId, string statusId, string createdByBranchId, string claimPayByCode,
                                        string claimOnLineCode, string denyCauseId, string chiefComplainId, string accidentDetail)
        {
            var empCode = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var conDateHappen = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateHappen));
            var conDateNotice = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateNotice));
            var conDateIn = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateIn));
            var conDateOut = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateOut));

            string[] rs = new string[3];

            var result = _context.usp_ClaimHeaderForPA_Insert(customerDetailCode, hospitalId, claimTypeId, claimStypeId, conDateHappen, conDateNotice, conDateIn, conDateOut,
                                                               accidentCauseId, statusId, empCode, createdByBranchId, claimPayByCode, claimOnLineCode, denyCauseId, chiefComplainId, accidentDetail, "").Single();

            rs[0] = result.IsResult.Value.ToString();
            rs[1] = result.Result;
            rs[2] = result.Msg;

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCustomerDetailPA(string customerDetailCode)
        {
            var rs = _context.usp_GetCreateClaimOnLineforPA_Select(customerDetailCode).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAccidentClaimDetail(string q)
        {
            var rs = _context.usp_AccidentCause_Select(0, 20, null, null, q).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClaimTypeXProductCategoryDetail(string productCategoryID)
        {
            var rs = _context.usp_ClaimTypeXProductCategory_Select(productCategoryID, 0, 999, null, null, null).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSchoolDetail(string q, int PAYear, string provinceID)
        {
            if (provinceID == "-1")
            {
                provinceID = null;
            }

            var rs = _context.usp_SchoolDetail_Select(PAYear, provinceID, 0, 10, null, null, q).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetApplicationPADetail(string provinceID, string schoolID, int PAYear, int? draw = null, int? pageSize = null,
                                                   int? pageStart = null, string sortField = null, string orderType = null, string search = null,
                                                   string firstName = null, string lastName = null)
        {
            string province_ID = null;
            string school_ID = null;

            if (provinceID != "" && provinceID != null && provinceID != "-1")
            {
                province_ID = provinceID;
            }

            if (schoolID != "" && schoolID != null)
            {
                school_ID = schoolID;
            }

            var rs = _context.usp_CreateClaimOnLineforPASearch_Select(PAYear, Convert.ToInt32(province_ID), school_ID, firstName,
                                                                lastName, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", rs.Count() != 0 ? rs.FirstOrDefault()?.TotalCount : rs.Count()},
                {"recordsFiltered", rs.Count() != 0 ? rs.FirstOrDefault()?.TotalCount : rs.Count()},
                {"data", rs.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetSchoolSelect(int provinceId, int PAYear, int? draw = null, int? pageSize = null,
                                           int? indexStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var rs = _context.usp_School_Select(PAYear, provinceId, indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", rs.Count() != 0 ? rs.FirstOrDefault()?.TotalCount : rs.Count()},
                {"recordsFiltered", rs.Count() != 0 ? rs.FirstOrDefault()?.TotalCount : rs.Count()},
                {"data", rs.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetPAApplicationDetail(string appCode, int? draw = null, int? pageSize = null, int? indexStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var rs = _context.usp_PAApplicationDetail_Select(appCode, indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", rs.Count() != 0 ? rs.FirstOrDefault()?.TotalCount : rs.Count()},
                {"recordsFiltered", rs.Count() != 0 ? rs.FirstOrDefault()?.TotalCount : rs.Count()},
                {"data", rs.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPACustomerDetail(string customerDetailCode)
        {
            var rs = _context.usp_PACustomerDetailByCustomerCode_Select(customerDetailCode).FirstOrDefault();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        #endregion "PA"

        #region "PH"

        public JsonResult GetApplicationPHDetail(int? draw, int? pageSize, int? pageStart, string sortField, string orderType, string appCode, string firstName, string lastName, string cardID, string search)
        {
            var rs = _context.usp_CreateClaimOnLineforPHSearch_Select(applicationCode: appCode,
                                                                      firstName: firstName,
                                                                      lastName: lastName,
                                                                      cardDetail: cardID,
                                                                      indexStart: pageStart,
                                                                      pageSize: pageSize,
                                                                      sortField: sortField,
                                                                      orderType: orderType,
                                                                      searchDetail: search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", rs.Count() != 0 ? rs.FirstOrDefault()?.TotalCount : rs.Count()},
                {"recordsFiltered", rs.Count() != 0 ? rs.FirstOrDefault()?.TotalCount : rs.Count()},
                {"data", rs.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [Authorization(Roles = "Developer")]
        public ActionResult SaveCliamHeaderForPH(string applicationID, string hospitalID, string claimTypeID, string claimAdmitTypeID, string dateHappen, string dateNotice,
                                                 string chiefComplainID, string statusID, string claimOnLineCode, string denyCauseID, string claimPayCode)
        {
            var empCode = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var conDateHappen = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateHappen));
            var conDateNotice = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateNotice));

            string[] rs = new string[3];

            var result = _context.usp_ClaimHeaderForPH_Insert(applicationID, hospitalID, claimTypeID, claimAdmitTypeID,
                                                              conDateHappen, conDateNotice, chiefComplainID, statusID, empCode,
                                                              claimPayCode, claimOnLineCode, denyCauseID, "").Single();

            rs[0] = result.IsResult.Value.ToString();
            rs[1] = result.Result;
            rs[2] = result.Msg;

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOPDCountForAppID(string applicationID)
        {
            var rs = _context.usp_CountOPDByAppForPH_Select(applicationID, 0, 999, null, null, null).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetApplicationByAppID(string applicationID)
        {
            var rs = _context.usp_GetCreateClaimOnLineforPH_Select(applicationID).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClaimAdmitTypeDetail(string claimTypeID)
        {
            var rs = _context.usp_ClaimAdmitType_Select(claimTypeID, 0, 9999, null, null, null).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetChiefComplainDetail(string q)
        {
            var rs = _context.usp_ChiefComplain_Select(0, 10, null, null, q).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        #endregion "PH"

        #region Claim Online - New

        /// <summary>
        /// Get Bank Account
        /// </summary>
        /// <param name="productGroupId"></param>
        /// <param name="appCode"></param>
        /// <returns></returns>
        public ActionResult GetBankAccount(int? productGroupId, string appCode)
        {
            var rs = _context.usp_BankAccountByApplicationCode_Select(productGroupId, appCode).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Contact
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        public ActionResult GetPersonContact(string appCode)
        {
            var rs = _context.usp_PAPersonContactByApplicationCode_Select(appCode).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        public ActionResult GetPersonContactPH(string appCode)
        {
            var rs = _context.usp_PHPersonContactByApplicationCode_Select(appCode).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Document Insert
        /// </summary>
        /// <param name="documentTypeId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorization(Roles = "Developer")]
        public ActionResult DocumentInset(string documentTypeId)
        {
            var userDetail = GlobalFunction.GetLoginDetail(this.HttpContext);
            var remark = "";

            var rs = _context.usp_Document_Insert(userDetail.User_ID, documentTypeId, remark).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Document Update
        /// </summary>
        /// <param name="documentTypeId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DocumentUpdate(string documentCode, int claimOnLineId)
        {
            var userDetail = GlobalFunction.GetLoginDetail(this.HttpContext);
            var arrDocCode = documentCode.Split(',');
            var count = 0;
            if (arrDocCode.Length > 0)
            {
                foreach (var item in arrDocCode)
                {
                    var rs = _context.usp_Document_Update(item, claimOnLineId, userDetail.User_ID).FirstOrDefault();

                    if (rs.IsResult.Value) count++;
                }

                if (count == arrDocCode.Length) return Json(new { IsResult = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { IsResult = false }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Document Select
        /// </summary>
        /// <param name="documentTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DocumentSelectByDocCode(string documentCode)
        {
            var rs = _context.usp_Document_Select(null, null, documentCode, 0, 99, null, null, null).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Document Validate
        /// </summary>
        /// <param name="documentCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DocumentValidate(string documentCode)
        {
            var rs = _context.usp_DocumentValidateByDocumentCode_Select(documentCode).FirstOrDefault();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// History PH
        /// </summary>
        /// <param name="applicationCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetClaimPHHistory(string applicationCode)
        {
            var rs = _context.usp_ClaimPHHistory_Select(applicationCode, 0, 9999, null, null, null).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// History PA
        /// </summary>
        /// <param name="applicationCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetClaimPAHistory(string customerDetailCode)
        {
            var rs = _context.usp_ClaimPAHistory_Select(customerDetailCode, 0, 9999, null, null, null).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// insert COL
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <param name="serviceByUserId"></param>
        /// <param name="noticeByUserId"></param>
        /// <param name="noticeByEmpId"></param>
        /// <param name="zebraCarOwnerByEmpId"></param>
        /// <param name="detail"></param>
        /// <param name="claimCount"></param>
        /// <param name="payeeTypeId"></param>
        /// <param name="payeeBankId"></param>
        /// <param name="payeeAccountNo"></param>
        /// <param name="payeeAccountName"></param>
        /// <param name="personContactTypeId"></param>
        /// <param name="personContactName"></param>
        /// <param name="personContactPhoneNo"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorization(Roles = "Developer")]
        public ActionResult ClaimOnLineInsert(int? productTypeId,
            string serviceByUserId,
            int? noticeByUserId,
            int? noticeByEmpId,
            string zebraCarOwnerByEmpId,
            string detail,
            int? claimCount,
            int? payeeTypeId,
            int? payeeBankId,
            string payeeAccountNo,
            string payeeAccountName,
            int? personContactTypeId,
            string personContactTypeOther,
            string personContactName,
            string personContactPhoneNo)
        {
            var userDetail = GlobalFunction.GetLoginDetail(this.HttpContext);

            var client = new EmployeeServiceClient();

            var result = client.GetUserIDByEmpCode(serviceByUserId);

            var result2 = client.GetEmployeeByEmployeeCode(zebraCarOwnerByEmpId);

            client.Close();

            var areaId = _context.usp_BranchByAreaId_Select(userDetail.Branch_ID, null, null, null, null, null, null, null).ToList();

            var rs = _context.usp_ClaimOnLineV2_Insert(productTypeId,
                                                      detail,
                                                      claimCount,
                                                      userDetail.Branch_ID,
                                                      result,
                                                      result,
                                                      noticeByEmpId,
                                                      payeeTypeId,
                                                      payeeBankId,
                                                      payeeAccountNo,
                                                      payeeAccountName,
                                                      userDetail.User_ID,
                                                      result2.Employee_ID,
                                                      areaId[0].AreaID,
                                                      personContactTypeId,
                                                      personContactTypeOther,
                                                      personContactName,
                                                      personContactPhoneNo).FirstOrDefault();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="claimOnLineId"></param>
        /// <param name="ss"></param>
        /// <param name="claimCode"></param>
        /// <param name="applicationCode"></param>
        /// <param name="schoolName"></param>
        /// <param name="custTitle"></param>
        /// <param name="custFirstName"></param>
        /// <param name="custLastName"></param>
        /// <param name="payerTitle"></param>
        /// <param name="payerFirstName"></param>
        /// <param name="payerLastName"></param>
        /// <param name="startCoverDate"></param>
        /// <param name="endCoverDate"></param>
        /// <param name="appStatus"></param>
        /// <param name="dateIn"></param>
        /// <param name="dateOut"></param>
        /// <param name="claimAdmitType"></param>
        /// <param name="hospitalProvince"></param>
        /// <param name="hospitalName"></param>
        /// <param name="claimStatus"></param>
        /// <param name="claimAmount"></param>
        /// <param name="claimRemark"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorization(Roles = "Developer")]
        public ActionResult ClaimOnLineTmpInsert(int? claimOnLineId,
                                                string claimCode,
                                                string applicationCode,
                                                string schoolName,
                                                string customerName,
                                                string payerName,
                                                string startCoverDate,
                                                string endCoverDate,
                                                string appStatus,
                                                string dateIn,
                                                string dateOut,
                                                string claimAdmitType,
                                                string hospitalProvince,
                                                string hospitalName,
                                                string claimStatus,
                                                string claimAmount,
                                                string claimRemark,
                                                string provinceid,
                                                string province,
                                                string hospitalid)
        {
            //check claimAdmitType
            if (!(claimAdmitType.Length > 0))
                return Json(new { IsResult = false, Result = "", Msg = "claimAdmitType is null or empty!" }, JsonRequestBehavior.AllowGet);

            DateTime? appStartCoverDate = null;
            DateTime? appEndCoverDate = null;
            DateTime? appDateIn = null;
            DateTime? appDateOut = null;

            if (startCoverDate != null && startCoverDate != "")
            {
                appStartCoverDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(startCoverDate));
            }
            if (endCoverDate != null && endCoverDate != "")
            {
                appEndCoverDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(endCoverDate));
            }
            if (dateIn != null && dateIn != "")
            {
                appDateIn = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateIn));
            }
            if (dateOut != null && dateOut != "")
            {
                appDateOut = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateOut));
            }

            var userDetail = GlobalFunction.GetLoginDetail(this.HttpContext);
            var rs = _context.usp_ClaimOnLineTmp_Insert(claimOnLineId,
                                                        claimCode,
                                                        applicationCode,
                                                        schoolName,
                                                        customerName,
                                                        payerName,
                                                        appStartCoverDate,
                                                        appEndCoverDate,
                                                        appStatus,
                                                        appDateIn,
                                                        appDateOut,
                                                        claimAdmitType,
                                                        hospitalProvince,
                                                        hospitalName,
                                                        claimStatus,
                                                        Convert.ToDouble(claimAmount),
                                                        claimRemark,
                                                        userDetail.User_ID).FirstOrDefault();
            if (rs.IsResult == true)
            {
                var result = _context.usp_ClaimApprovedTransection_Insert(Convert.ToString(claimCode), Convert.ToString(userDetail.EmployeeCode), applicationCode, appDateIn, appDateOut, Convert.ToDouble(claimAmount), provinceid, province, hospitalid).FirstOrDefault();
            }
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Validate PH
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="hospitalId"></param>
        /// <param name="claimTypeId"></param>
        /// <param name="claimAdmitTypeId"></param>
        /// <param name="dateHappen"></param>
        /// <param name="dateNotice"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorization(Roles = "Developer")]
        public ActionResult ClaimHeaderForPH_Validate(string appId, string hospitalId, string claimTypeId, string claimAdmitTypeId, string dateHappen, string dateNotice, string dateIn, string dateOut)
        {
            DateTime? nDateHappen = null;
            DateTime? nDateNotice = null;
            DateTime? nDateIn = null;
            DateTime? nDateOut = null;

            if (dateHappen != null && dateHappen != "")
            {
                nDateHappen = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateHappen));
            }

            if (dateNotice != null && dateNotice != "")
            {
                nDateNotice = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateNotice));
            }

            if (dateIn != null && dateIn != "")
            {
                nDateIn = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateIn));
            }

            if (dateOut != null && dateOut != "")
            {
                nDateOut = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateOut));
            }
            var rs = _context.usp_ClaimHeaderForPH_Validate_New(appId, hospitalId, claimTypeId, claimAdmitTypeId, nDateHappen, nDateNotice, nDateIn ,nDateOut).FirstOrDefault();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// validate PA
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="hospitalId"></param>
        /// <param name="accidentCauseId"></param>
        /// <param name="chiefComplainId"></param>
        /// <param name="dateHappen"></param>
        /// <param name="dateNotice"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorization(Roles = "Developer")]
        public ActionResult ClaimHeaderForPA_Validate(string customerId, string hospitalId, string accidentCauseId, string chiefComplainId, string dateHappen, string dateNotice)
        {
            DateTime? nDateHappen = null;
            DateTime? nDateNotice = null;

            if (dateHappen != null && dateHappen != "")
            {
                nDateHappen = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateHappen));
            }

            if (dateNotice != null && dateNotice != "")
            {
                nDateNotice = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateNotice));
            }

            var rs = _context.usp_ClaimHeaderForPA_Validate(customerId, hospitalId, nDateHappen, nDateNotice, accidentCauseId, chiefComplainId).FirstOrDefault();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///Save Cliam Header For PH Cancel
        /// </summary>
        /// <param name="applicationID"></param>
        /// <param name="hospitalID"></param>
        /// <param name="claimTypeID"></param>
        /// <param name="claimAdmitTypeID"></param>
        /// <param name="dateHappen"></param>
        /// <param name="dateNotice"></param>
        /// <param name="chiefComplainID"></param>
        /// <param name="statusID"></param>
        /// <param name="claimOnLineCode"></param>
        /// <param name="denyCauseID"></param>
        /// <param name="claimPayCode"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorization(Roles = "Developer")]
        public ActionResult SaveCliamHeaderForPHCancel(string applicationID,
                                                        string hospitalID,
                                                        string claimTypeID,
                                                        string claimAdmitTypeID,
                                                        string dateHappen,
                                                        string dateNotice,
                                                        string dateIn,
                                                        string dateOut,
                                                        string dayCount,
                                                        string chiefComplainID,
                                                        string statusID,
                                                        string claimOnLineCode,
                                                        string denyCauseID,
                                                        string claimPayCode,
                                                        string remark,
                                                        string claimicdtenid)
        {
            //check claimAdmitTypeID
            if (!(claimAdmitTypeID.Length > 0))
                return Json(new { IsResult = false, Result = "", Msg = "claimAdmitTypeID is null or empty!" }, JsonRequestBehavior.AllowGet);

            var user = GlobalFunction.GetLoginDetail(this.HttpContext);
            DateTime? cDateHappen = null;
            DateTime? cDateNotice = null;
            DateTime? cDateIn = null;
            DateTime? cDateOut = null;

            if (dateHappen.Trim() != "") cDateHappen = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateHappen));
            if (dateNotice.Trim() != "") cDateNotice = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateNotice));
            if (dateIn.Trim() != "") cDateIn = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateIn));
            if (dateOut.Trim() != "") cDateOut = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateOut));

            int? days = null;
            if (dayCount.Trim() != "") days = Convert.ToInt32(dayCount);

            var branch = _context.usp_BranchByAreaId_Select(user.Branch_ID, null, null, null, null, null, null, null).FirstOrDefault();

            var result = _context.usp_ClaimHeaderForPHCancel_Insert(app_id: applicationID,
                                                                   hospital_id: hospitalID,
                                                                    claimType_id: claimTypeID,
                                                                    claimAdmitType_id: claimAdmitTypeID,
                                                                    dateHappen: cDateHappen,
                                                                    dateNotice: cDateNotice,
                                                                    dateIn: cDateIn,
                                                                    dateOut: cDateOut,
                                                                    iPDDayCount: days,
                                                                    chiefComplain_id: chiefComplainID,
                                                                    status_id: statusID,
                                                                    createdByEmpCode: user.UserName,
                                                                    claimPayBy_id: claimPayCode,
                                                                    claimOnLineCode: claimOnLineCode,
                                                                    denyCause_id: denyCauseID,
                                                                    remark: remark,
                                                                    branchCode: branch.BranchCode,
                                                                    iCD10_1: claimicdtenid).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Save Cliam Header For PA Cancel
        /// </summary>
        /// <param name="customerDetailCode"></param>
        /// <param name="hospitalId"></param>
        /// <param name="claimTypeId"></param>
        /// <param name="claimStypeId"></param>
        /// <param name="dateHappen"></param>
        /// <param name="dateNotice"></param>
        /// <param name="dateIn"></param>
        /// <param name="dateOut"></param>
        /// <param name="accidentCauseId"></param>
        /// <param name="statusId"></param>
        /// <param name="createdByBranchId"></param>
        /// <param name="claimPayByCode"></param>
        /// <param name="claimOnLineCode"></param>
        /// <param name="denyCauseId"></param>
        /// <param name="chiefComplainId"></param>
        /// <param name="accidentDetail"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorization(Roles = "Developer")]
        public ActionResult SaveClaimHeaderForPACancel(string customerDetailCode
                                                       , string hospitalId
                                                       , string claimTypeId
                                                       , string claimStypeId
                                                       , string dateIn
                                                       , string dateOut
                                                       , string accidentCauseId
                                                       , string statusId
                                                       , string createdByBranchId
                                                       , string claimPayByCode
                                                       , string claimOnLineCode
                                                       , string denyCauseId
                                                       , string chiefComplainId
                                                       , string accidentDetail)
        {
            var empCode = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var conDateIn = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateIn));
            var conDateOut = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateOut));

            //วันที่เเจ้ง ให้ใช้วันที่บันทึก , วันที่เกิดเหตุให้ใช้วันที่เข้า 01Feb21-Bow
            var conDateHappen = conDateIn;
            var conDateNotice = DateTime.Now;

            string[] rs = new string[3];

            var result = _context.usp_ClaimHeaderForPACancel_Insert(customerDetailCode, hospitalId, claimTypeId, claimStypeId, conDateHappen, conDateNotice, conDateIn, conDateOut,
                                                               accidentCauseId, statusId, empCode, createdByBranchId, claimPayByCode, claimOnLineCode, denyCauseId, chiefComplainId, accidentDetail, accidentDetail).Single();

            rs[0] = result.IsResult.Value.ToString();
            rs[1] = result.Result;
            rs[2] = result.Msg;

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="hospitalId"></param>
        /// <param name="accidentCauseId"></param>
        /// <param name="chiefComplainId"></param>
        /// <param name="dateHappen"></param>
        /// <param name="dateNotice"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ClaimOnLineIsActiveUpdate(int claimOnLineId)
        {
            var rs = _context.usp_ClaimOnLineIsActiveByClaimOnLineId_Update(claimOnLineId).FirstOrDefault();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// OPD By ProductCode
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OPDByProductCode(string productId)
        {
            var result = _context.usp_OPDByProductCode_Select(productId).FirstOrDefault().Value;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOPDRemain(string appId, string dtHappen)
        {
            var dt = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dtHappen));

            var result = _context.usp_CustomerBenefit_OPD_Select(appId, dt).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PACustomerBenefit(string customerCode)
        {
            var result = _context.usp_PACustomerBenefitByCustomerCode_Select(customerCode.Trim()).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Claim Online - New

        #endregion "Method"
    }
}