using SmileSPACommunity.Models;
using SmileSPACommunity.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.Data.Entity.Core.Objects;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SmileSPACommunity.Controllers
{
    public class PAPersonnelController : Controller
    {
        private PACommunityEntities _db;

        public PAPersonnelController()
        {
            _db = new PACommunityEntities();
        }


        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        // GET: PAPersonnel
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult PAPersonnelMonitor()  //บันทึกกรมธรรม์ - หน้า Monitor
        {
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = new SSOService.SSOServiceClient().GetRoleByUserName(username);
            var rolelist = role.Split(',').ToList();

            var roleDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var rolePA = new[] { "PAPersonnel_PAUnderwrite" }; //ฝ่าย PA Underwrite

            //ถ้าเป็น สิทธิ์ของ Dev และ Underwrite ให้แสดงสาขาทั้งหมด
            if (rolelist.Intersect(rolePA).Count() > 0 || rolelist.Intersect(roleDev).Count() > 0)
            {
                ViewBag.Branch = _db.usp_Branch_Select(null, 0, 99, null, null, null).ToList();
            }
            else
            {
                var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
                ViewBag.Branch = _db.usp_Branch_Select(branchId, 0, 99, null, null, null).ToList();
            }

            ViewBag.Status = _db.usp_PersonnelApplicationStatus_Select(null, 0, 999, null, null, null).ToList();
            ViewBag.PolicyYear = _db.usp_PolicyYear_Select(0, 999, null, null, null).ToList();
            return View();
        }

        public ActionResult PAPersonnelNewApp(string appID = null) //บันทึก New App
        {
            int? d_AppID;
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = new SSOService.SSOServiceClient().GetRoleByUserName(username);
            var rolelist = role.Split(',').ToList();

            var roleDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var rolePA = new[] { "PAPersonnel_PAUnderwrite" }; //ฝ่าย PA Underwrite

            var branchID = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;

            if (appID == null)
            {
                d_AppID = null;

                ViewBag.BranchID = branchID;
                ViewBag.Branch = GlobalFunction.GetLoginDetail(this.HttpContext).BranchDetail;

            }
            else
            {
                var appIDBase64EncodedBytes = Convert.FromBase64String(appID);
                var deCode_AppID = System.Text.Encoding.UTF8.GetString(appIDBase64EncodedBytes);

                d_AppID = Convert.ToInt32(deCode_AppID);
            }

            ViewBag.AppID = d_AppID;
            // Check Role
            if (rolelist.Intersect(rolePA).Count() > 0 || rolelist.Intersect(roleDev).Count() > 0)
            {
                ViewBag.Province = _db.usp_PovinceByBranch_Select(null, 0, 999, null, null, null).ToList();
            }
            else
            {
                ViewBag.Province = _db.usp_PovinceByBranch_Select(branchID, 0, 999, null, null, null).ToList();
            }

            ViewBag.PlanID = _db.usp_PersonnelProduct_Select(null, 0, 99, null, null, null).ToList();
            ViewBag.PolicyYear = _db.usp_PolicyYear_Select(0, 999, null, null, null).ToList().OrderByDescending(x => x.Code);

            return View();
        }


        public ActionResult PAPersonnelSearch()     //ค้นหาข้อมูลผู้เอาประกัน
        {
            ViewBag.Branchs = _db.usp_Branch_Select(null, 0, 99, null, null, null).ToList();
            ViewBag.PolicyYear = _db.usp_PolicyYear_Select(0, 999, null, null, null).ToList();
            ViewBag.CurrentYear = DateTime.Now.ToString("yyyy");
            ViewBag.Provinces = _db.usp_Province_Select(null, 0, 99, null, null, null).ToList();

            return View();
        }

        public ActionResult PAPersonnelDetail(string appId = null) //รายละเอียดข้อมูล
        {
            //DeCode
            var ApplicationIdBase64EncodedBytes = Convert.FromBase64String(appId);
            var deCode_ApplicationId = System.Text.Encoding.UTF8.GetString(ApplicationIdBase64EncodedBytes);

            ViewBag.ApplicationId = deCode_ApplicationId;
            ViewBag.Round = _db.usp_PersonnelApplicationRoundByApplicationId_Select(Convert.ToInt32(deCode_ApplicationId), 0, 999, null, null, null).ToList();
            return View();
        }

        public ActionResult PAPersonnelUnApproveMonitor() //Monitor ไม่อนุมัติ   
        {
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = new SSOService.SSOServiceClient().GetRoleByUserName(username);
            var rolelist = role.Split(',').ToList();

            var roleDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var rolePA = new[] { "PACommunity_PAUnderwrite" }; //ฝ่าย PA Underwrite

            //ถ้าเป็น สิทธิ์ของ Dev และ Underwrite ให้แสดงสาขาทั้งหมด
            if (rolelist.Intersect(rolePA).Count() > 0 || rolelist.Intersect(roleDev).Count() > 0)
            {
                ViewBag.BranchId = null;
            }
            else
            {
                var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
                ViewBag.BranchId = branchId;
            }

            ViewBag.PolicyYear = _db.usp_PolicyYear_Select(0, 999, null, null, null).ToList();
            return View();
        }


        public ActionResult PAPersonnelNewAppForEdit(string roundId) //แก้ไข NewApp     
        {
            int? d_roundId;
            if (roundId != null)
            {
                var roundIdBase64EncodedBytes = Convert.FromBase64String(roundId);
                var deCode_roundId = System.Text.Encoding.UTF8.GetString(roundIdBase64EncodedBytes);

                d_roundId = Convert.ToInt32(deCode_roundId);
            }
            else
            {
                d_roundId = null;
            }


            ViewBag.RoundId = d_roundId;
            ViewBag.PlanID = _db.usp_PersonnelProduct_Select(null, 0, 99, null, null, null).ToList();


            return View();
        }


        public ActionResult PAPersonnelImportCustomer(string appId = null) //นำเข้ารายชื่อ     
        {
            int? d_AppID;
            var appIDBase64EncodedBytes = Convert.FromBase64String(appId);
            var deCode_AppID = System.Text.Encoding.UTF8.GetString(appIDBase64EncodedBytes);
            d_AppID = Convert.ToInt32(deCode_AppID);

            ViewBag.AppID = d_AppID;

            return View();
        }

        public ActionResult PAPersonnelManageCustomer(string roundId) //จัดการรายชื่อ    
        {
            //DeCode
            var roundIdBase64EncodedBytes = Convert.FromBase64String(roundId);
            var decode_roundId = System.Text.Encoding.UTF8.GetString(roundIdBase64EncodedBytes);

            ViewBag.RoundId = decode_roundId;
            ViewBag.RoundCancelCause = _db.usp_PersonnelApplicationRoundCancelCause_Select(null, 0, 99, null, null, null).ToList();

            return View();
        }

        public ActionResult PAPersonnelImportPolicyNumber() //นำเข้าเลขกรมธรรม์    
        {
            return View();
        }

        public ActionResult PAPersonnelImportGroupControl()
        {
            return View();
        }

        public JsonResult GetSchool(string q, int? policyYear, int? provinceId)
        {
            var result = _db.usp_PAApplicationSearch_Select(policyYear, provinceId, 0, 999, null, null, q).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSchoolDetail(string appCode)
        {
            var result = _db.usp_GetPAApplicationDetail(appCode).Single();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoSaveNewApp(int PAYear, string PersonnelAppName, string RefAppCode,
                                         string RefOrganizeId, int ProductId, int BranchId, string StartCoverDate,
                                         string EffectiveDate, string EndCoverDate)
        {

            var refOrganizeSchoolId = Convert.ToInt32(RefOrganizeId);
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var startCoverDate = GlobalFunction.ConvertDatePickerToDate_BE(StartCoverDate);
            var effectiveDate = GlobalFunction.ConvertDatePickerToDate_BE(EffectiveDate);
            var endCoverDate = GlobalFunction.ConvertDatePickerToDate_BE(EndCoverDate);

            var result = _db.usp_PersonnelApplication_Insert(PAYear, PersonnelAppName, RefAppCode,
                                                             refOrganizeSchoolId, ProductId, BranchId,
                                                             startCoverDate, effectiveDate, endCoverDate,
                                                             userID).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPAApplicationDetailByAppID(int appId)
        {
            var result = _db.usp_GetPersonnelApplicationDetail(appId).Single();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoUpdateByAppId(int PersonnelAppID, int ProductID)
        {
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _db.usp_PersonnelApplication_Update(PersonnelAppID, ProductID, userId).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPersonnelByPlanId(int productId)
        {
            var result = _db.usp_PersonnelProduct_Select(productId, 0, 99, null, null, null).Single();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetApplicationMonitor(int? branchId = null, int? year = null, int? appStatusId = null, int? draw = null, int? pageSize = null, int? indexStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_PersonnelApplication_Select(null, branchId, year, appStatusId, indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }


        public JsonResult IsValidateforImport(HttpPostedFileBase file)
        {
            var lst = GetExcelImportCustomerDetail(file);

            var lst_detail = lst.Item1;

            var rs = new { IsResult = true, Result = "", Msg = "" };

            int c = 1;

            if (lst_detail.Count() == 0)
            {
                rs = new
                {
                    IsResult = false,
                    Result = "Failure",
                    Msg = $"ไม่มีข้อมูลนำเข้า"
                };

                return Json(rs, JsonRequestBehavior.AllowGet);
            }



            foreach (var item in lst_detail)
            {
                c += 1;

                if (item.title == "" || item.title == null)
                {
                    rs = new
                    {
                        IsResult = false,
                        Result = "Failure",
                        Msg = $"รบกวนตรวจสอบคำนำหน้า บรรทัดที่ ${c}"
                    };

                    return Json(rs, JsonRequestBehavior.AllowGet);
                }

                if (item.firstName == "")
                {
                    rs = new
                    {
                        IsResult = false,
                        Result = "Failure",
                        Msg = $"รบกวนกรอกชื่อ บรรทัดที่ ${c}"
                    };

                    return Json(rs, JsonRequestBehavior.AllowGet);
                }

                if (item.firstName.Count() > 250)
                {
                    rs = new
                    {
                        IsResult = false,
                        Result = "Failure",
                        Msg = $"รบกวนตรวจสอบชื่อ บรรทัดที่ ${c}"
                    };

                    return Json(rs, JsonRequestBehavior.AllowGet);
                }

                if (item.lasttName == "")
                {
                    rs = new
                    {
                        IsResult = false,
                        Result = "Failure",
                        Msg = $"รบกวนกรอกนามสกุล บรรทัดที่ ${c}"
                    };

                    return Json(rs, JsonRequestBehavior.AllowGet);
                }


                if (item.lasttName.Count() > 250)
                {
                    rs = new
                    {
                        IsResult = false,
                        Result = "Failure",
                        Msg = $"รบกวนตรวจสอบนามสกุล บรรทัดที่ ${c}"
                    };

                    return Json(rs, JsonRequestBehavior.AllowGet);
                }


                //if (item.zCardID == "" && item.passport == "")
                //{

                //    rs = new
                //    {
                //        IsResult = false,
                //        Result = "Failure",
                //        Msg = $"รบกวนตรวจสอบเลขบัตรประชาชน หรือ Passport ต้องใส่อย่างใดอย่างนึง บรรทัดที่ ${c}"
                //    };

                //    return Json(rs, JsonRequestBehavior.AllowGet);
                //}

                //if (item.zCardID != "" && item.passport != "")
                //{
                //    rs = new
                //    {
                //        IsResult = false,
                //        Result = "Failure",
                //        Msg = $"รบกวนตรวจสอบเลขบัตรประชาชน หรือ Passport ต้องใส่อย่างใดอย่างนึง บรรทัดที่ ${c}"
                //    };

                //    return Json(rs, JsonRequestBehavior.AllowGet);
                //}

                //if (item.zCardID != "")
                //{
                //    if (IsNumeric(item.zCardID) == false)
                //    {

                //        rs = new
                //        {
                //            IsResult = false,
                //            Result = "Failure",
                //            Msg = $"รบกวนตรวจสอบเลขบัตรประชาชน ต้องเป็นตัวเลขเท่านั้น บรรทัดที่ ${c}"
                //        };

                //        return Json(rs, JsonRequestBehavior.AllowGet);
                //    }
                //    else
                //    {
                //        //if (item.zCardID.Count() != 13)
                //        //{
                //        //    rs = new
                //        //    {
                //        //        IsResult = false,
                //        //        Result = "Failure",
                //        //        Msg = $"รบกวนตรวจสอบเลขบัตรประชาชน ต้องครบ 13 หลัก บรรทัดที่ ${c}"
                //        //    };

                //        //    return Json(rs, JsonRequestBehavior.AllowGet);

                //        //}
                //        //else
                //        //{
                //        //    if (IsValidCheckPersonID(item.zCardID) == false) 
                //        //    {
                //        //        rs = new
                //        //        {
                //        //            IsResult = false,
                //        //            Result = "Failure",
                //        //            Msg = $"รบกวนตรวจสอบเลขบัตรประชาชน  บรรทัดที่ ${c}"
                //        //        };

                //        //        return Json(rs, JsonRequestBehavior.AllowGet);
                //        //    }
                //        //}
                //    }
                //}

                if (item.position == "")
                {
                    rs = new
                    {
                        IsResult = false,
                        Result = "Failure",
                        Msg = $"รบกวนกรอกตำแหน่ง บรรทัดที่ ${c}"
                    };

                    return Json(rs, JsonRequestBehavior.AllowGet);
                }

                if (item.phoneNumber != "")
                {
                    if (IsNumeric(item.phoneNumber) == false)
                    {
                        rs = new
                        {
                            IsResult = false,
                            Result = "Failure",
                            Msg = $"รบกวนตรวจสอบเบอร์โทร ต้องเป็นตัวเลขเท่านั้น บรรทัดที่ ${c}"
                        };

                        return Json(rs, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (item.phoneNumber.Count() != 10)
                        {
                            rs = new
                            {
                                IsResult = false,
                                Result = "Failure",
                                Msg = $"รบกวนตรวจสอบเบอร์โทร ต้องครบ 10 หลัก บรรทัดที่ ${c}"
                            };

                            return Json(rs, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {

                            //Check degit 
                            string[] digit = { "08", "06", "09", "01" };

                            string phone_digit = item.phoneNumber.Substring(0, 2);

                            var ischeck_digit = digit.Contains(phone_digit);

                            if (ischeck_digit == false)
                            {
                                rs = new
                                {
                                    IsResult = false,
                                    Result = "Failure",
                                    Msg = $"รบกวนตรวจสอบเบอร์โทร บรรทัดที่ ${c}"
                                };

                                return Json(rs, JsonRequestBehavior.AllowGet);
                            }

                        }
                    }
                }

            }
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        public ActionResult ImportCustomerDetail(HttpPostedFileBase file)
        {
            var lst = GetExcelImportCustomerDetail(file);

            //var db = new PACommunityEntities();
            var lst_detail = lst.Item1;
            var result = new { IsResult = true, Result = "", Msg = "", TmpCode = "" };

            var tmCode = new Guid();

            tmCode = Guid.NewGuid();

            //Loop Insert Data
            foreach (var item in lst_detail)
            {
                try
                {
                    var rs = _db.usp_TmpPersonnelCustomerDetail_Insert(tmCode, item.title, item.firstName,
                                                                        item.lasttName, item.zCardID, item.passport,
                                                                        item.position, item.phoneNumber).Single();


                    if (rs.IsResult == false)
                    {
                        return Json(rs, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                    throw;
                }
            }

            //Insert Validate
            var rs_validate = _db.usp_TmpPersonnelCustomerDetail_Validate(tmCode).Single();

            result = new
            {
                IsResult = rs_validate.IsResult.Value,
                Result = rs_validate.Result.ToString(),
                Msg = rs_validate.Msg.ToString(),
                TmpCode = tmCode.ToString()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        private bool IsValidCheckPersonID(string pid)
        {

            char[] numberChars = pid.ToCharArray();

            int total = 0;
            int mul = 13;
            int mod = 0, mod2 = 0;
            int nsub = 0;
            int numberChars12 = 0;

            for (int i = 0; i < numberChars.Length - 1; i++)
            {
                int num = 0;
                int.TryParse(numberChars[i].ToString(), out num);

                total = total + num * mul;
                mul = mul - 1;

                //Debug.Log(total + " - " + num + " - "+mul);
            }

            mod = total % 11;
            nsub = 11 - mod;
            mod2 = nsub % 10;

            //Debug.Log(mod);
            //Debug.Log(nsub);
            //Debug.Log(mod2);


            int.TryParse(numberChars[12].ToString(), out numberChars12);

            //Debug.Log(numberChars12);

            if (mod2 != numberChars12)
                return false;
            else
                return true;
        }


        public JsonResult GetTempImportCustomerDetailPreview(string tempCode, int? draw = null, int? pageSize = null, int? indexStart = null, string sortField = null, string orderType = null, string search = null)
        {

            var result = _db.usp_TmpPersonnelCustomerDetail_Preview(Guid.Parse(tempCode), indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTempCustomerErrorCount(string tempCode)
        {
            var result = _db.usp_GetTmpPersonnelCustomerDetailErrorCount(Guid.Parse(tempCode)).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult InsertCustomerDetailAndSendPolicy(string tmpCode, int appId, string effectiveDate)
        {
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var code = Guid.Parse(tmpCode);
            var d_effectiveDate = GlobalFunction.ConvertDatePickerToDate_BE(effectiveDate);

            var rs = _db.usp_PersonnelCustomerDetail_Insert(code, appId, userId, d_effectiveDate).Single();

            if (rs.IsResult == true)
            {
                var rs_send = _db.usp_PersonnelApplicationRoundSent_Update(Convert.ToInt32(rs.Result), userId).Single();

                return Json(rs_send, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }

        public Tuple<List<ImportPersonnelCustomerDetail>> GetExcelImportCustomerDetail(HttpPostedFileBase file)
        {
            List<ImportPersonnelCustomerDetail> detaillist = new List<ImportPersonnelCustomerDetail>();

            using (var package = new ExcelPackage(file.InputStream))
            {
                var cs = package.Workbook.Worksheets;
                var ws = cs.First();
                var nr = ws.Dimension.End.Row;

                try
                {
                    for (var ri = 2; ri <= nr; ri++)
                    {

                        detaillist.Add(new ImportPersonnelCustomerDetail()
                        {
                            title = CheckNull(ws.Cells[ri, 1].Value),
                            firstName = CheckNull(ws.Cells[ri, 2].Value),
                            lasttName = CheckNull(ws.Cells[ri, 3].Value),
                            zCardID = CheckNull(ws.Cells[ri, 4].Value),
                            passport = CheckNull(ws.Cells[ri, 5].Value),
                            position = CheckNull(ws.Cells[ri, 6].Value),
                            phoneNumber = CheckNull(ws.Cells[ri, 7].Value)
                        });


                        //if (ws.Cells[ri, 1].Value == null)
                        //{
                        //    ws.DeleteRow(nr);
                        //}
                        //else
                        //{
                        //    detaillist.Add(new ImportPersonnelCustomerDetail()
                        //    {
                        //        title = ws.Cells[ri, 1].Value.ToString(),
                        //        firstName = CheckNull(ws.Cells[ri, 2].Value),
                        //        lasttName = CheckNull(ws.Cells[ri, 3].Value),
                        //        zCardID = CheckNull(ws.Cells[ri, 4].Value),
                        //        passport = CheckNull(ws.Cells[ri, 5].Value),
                        //        position = CheckNull(ws.Cells[ri, 6].Value),
                        //        phoneNumber = CheckNull(ws.Cells[ri, 7].Value)
                        //    });
                        //}
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return System.Tuple.Create(detaillist);
        }

        public static bool IsNumeric(object Expression)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        private string CheckNull(object value)
        {
            if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return "";
            }
        }

        public JsonResult GetApplicationRoundMonitor(int? appRoundId = null, int? branchId = null, int? year = null, string appRoundStatusIdList = null, int? draw = null, int? pageSize = null, int? indexStart = null, string sortField = null, string orderType = null, string searchDetail = null, bool isEndorse = false)
        {
            var result = _db.usp_PersonnelApplicationRound_Select(year, branchId, appRoundId, appRoundStatusIdList, isEndorse, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPAPersonnelAppIDByRoundID(int? appRoundId = null, bool isEndorse = false)
        {
            var result = _db.usp_PersonnelApplicationRound_Select(null, null, appRoundId, null, isEndorse, null, null, null, null, null).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteImportCustomerDetail(int roundId, int cancelCauseId, string cancelRemark)
        {
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _db.usp_PersonnelApplicationRoundCancel_Update(roundId, cancelCauseId, cancelRemark, userId).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPersonnelCustomerDetail(int appId, int roundId, int? draw = null, int? pageSize = null, int? indexStart = null, string sortField = null, string orderType = null, string searchDetail = null)
        {
            var result = _db.usp_PersonnelCustomerDetail_Select(appId, null, roundId, "1,2", indexStart, pageSize, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdatePersonelCustomerDetail(int roundId)
        {
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _db.usp_PersonnelApplicationRoundSent_Update(roundId, userId).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Get excel import
        public ActionResult ImportExcelPolicyFile(HttpPostedFileBase file)
        {
            var lstArr = new string[3];
            var lstValidate = new string[4];

            var tmCode = new ObjectParameter("Result", typeof(string));

            var lst = GetExcelImportPolicyNumber(file);

            if (lst == null)
            {
                lstValidate[0] = "False";
                lstValidate[1] = "0";
                lstValidate[2] = "ข้อมูลไม่ถูกต้องกรุณาตรวจสอบไฟล์";
                lstValidate[3] = "";
                return Json(lstValidate, JsonRequestBehavior.AllowGet);
            }

            var lst_Policy = lst.Item1;

            //using (var db = new PACommunityEntities())
            //{
            //    //Gen Code Tmp
            //    db.usp_GenerateCode_ForReportGroup("Tmp", tmCode);
            //}
            //var tmp_Code = tmCode.Value.ToString();
            var tmp_Code = Guid.NewGuid();
            //Loop Insert Data
            foreach (var item in lst_Policy)
            {
                try
                {
                    //Insert Temp
                    var rs = _db.usp_TmpPersonnelApplicationPolicy_Insert(tmp_Code, item.ApplicationCode == "" ? null : item.ApplicationCode, item.PolicyNumber == "" ? null : item.PolicyNumber).Single();
                    string message = rs.Msg.ToString();
                    if (message.Trim().Contains("@ApplicationCode is not null"))
                    { message = "มีรายการที่ไม่มี Application Code"; }
                    if (message.Trim().Contains("@Policyno is not null"))
                    { message = "มีรายการที่ไม่มี เลข Policy"; }
                    lstArr[0] = rs.IsResult.Value.ToString();
                    lstArr[1] = rs.Result.ToString();
                    lstArr[2] = message;

                    if (lstArr[0] == "False")
                    {
                        return Json(lstArr, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }

            //Insert Validate
            var rs_validate = _db.usp_TmpPersonnelApplicationPolicy_Validate(tmp_Code).SingleOrDefault();

            lstValidate[0] = rs_validate.IsResult.Value.ToString();
            lstValidate[1] = rs_validate.Result.ToString();
            lstValidate[2] = rs_validate.Msg.ToString();
            lstValidate[3] = tmp_Code.ToString();

            return Json(lstValidate, JsonRequestBehavior.AllowGet);
        }
        public Tuple<List<ImportPolicyNumberList>> GetExcelImportPolicyNumber(HttpPostedFileBase file)
        {
            List<ImportPolicyNumberList> policyNumberlist = new List<ImportPolicyNumberList>();

            using (var package = new ExcelPackage(file.InputStream))
            {
                var cs = package.Workbook.Worksheets;
                var ws = cs.First();
                var nr = ws.Dimension.End.Row;

                try
                {
                    for (var ri = 2; ri <= nr; ri++)
                    {
                        var app = ws.Cells[ri, 1].Value;
                        var poli = ws.Cells[ri, 2].Value;
                        string app_str = "";
                        string poli_str = "";
                        if (app != null)
                        { app_str = ws.Cells[ri, 1].Value.ToString(); }
                        if (poli != null)
                        { poli_str = ws.Cells[ri, 2].Value.ToString(); }
                        ImportPolicyNumberList item = new ImportPolicyNumberList();
                        item.ApplicationCode = app_str;
                        item.PolicyNumber = poli_str;
                        policyNumberlist.Add(item);
                        //}
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return System.Tuple.Create(policyNumberlist);
        }
        public JsonResult GetTmpPolicyNumberList(string tmpCode, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_TmpPersonnelApplicationPolicy_Preview(Guid.Parse(tmpCode), pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                { "draw", draw },
                { "recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                { "recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                { "data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DoConfirmImportPolicyList(string tmpCode)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_PersonnelApplicationPolicy_Update(Guid.Parse(tmpCode), userID).SingleOrDefault();

            lstArr[0] = rs.IsResult.Value.ToString();
            lstArr[1] = rs.Result;
            lstArr[2] = rs.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportPolicyNoQueueReport(FormCollection form)
        {
            //await Task.Yield();
            var tempCode = form["tempCode"];
            var data_sheet1 = _db.usp_TmpPersonnelApplicationPolicy_Preview(Guid.Parse(tempCode), 0, 99999999, null, null, null).Select(s => new
            {
                s.PersonnelApplicationCode,
                s.PolicyNo,
                สถานะการนำเข้า = s.VaildateResult == "" ? "ผ่าน" : "ไม่ผ่าน",
                สาเหตุ = s.VaildateResult
            }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet1 = package.Workbook.Worksheets.Add("Sheet1");
                workSheet1.Cells.LoadFromCollection(data_sheet1, true);

                // Select only the header cells
                var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                // Set their text to bold.
                headerCells1.Style.Font.Bold = true;
                // Set their background color
                headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerCells1.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                // Apply the auto-filter to all the columns
                var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                allCells1.AutoFilter = true;
                // Auto-fit all the columns
                allCells1.AutoFitColumns();

                package.Save();

                stream.Position = 0;
                Session["DownloadExcel_FilePolicyNumberListReport"] = package.GetAsByteArray();
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DownloadPolicyNoQueueReport()
        {
            if (Session["DownloadExcel_FilePolicyNumberListReport"] != null)
            {
                byte[] data = Session["DownloadExcel_FilePolicyNumberListReport"] as byte[];
                string excelName = $"รายงานตรวจสอบรายการอัพเดตเลขกรมธรรม์-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        #region "Report"

        [HttpGet]
        public async Task<ActionResult> ExportCustomerDetailPreviewReport(string tempCode)
        {
            await Task.Yield();
            var result = _db.usp_TmpPersonnelCustomerDetail_Preview(Guid.Parse(tempCode), 0, 999999, null, null, null).ToList()
             .Select((x) => new
             {
                 คำนำหน้า = x.TitleName,
                 ชื่อ = x.FirstName,
                 นามสกุล = x.LastName,
                 บัตรประชาชน = x.IdCardNumber,
                 Passport = x.PassPortNumber,
                 ตำแหน่ง = x.Position,
                 เบอร์โทร = x.MobileNumber,
                 หมายเหตุ = x.VaildateResult,
             }).ToList();

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var workSheet1 = package.Workbook.Worksheets.Add("Detail");
                workSheet1.Cells.LoadFromCollection(result, true);

                var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                headerCells1.Style.Font.Bold = true;
                headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerCells1.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                allCells1.AutoFilter = true;
                allCells1.AutoFitColumns();


                package.Save();

                stream.Position = 0;
                Session["DownloadExcelCustomerDetailPreview"] = package.GetAsByteArray();

                return Json("", JsonRequestBehavior.AllowGet);
            }

            //var dt1 = GlobalFunction.ToDataTable(result);
            //ExcelManager.ExportToExcel(this.HttpContext, "รายงานผลการนำเข้ารายชื่อผู้เอาประกัน_PA_ยิ้มแฉ่ง", dt1);
        }

        public ActionResult ExportCustomerDetailPreviewReportDownload()
        {
            //var dataKey = TempData["keyReport"].ToString();
            if (Session["DownloadExcelCustomerDetailPreview"] != null)
            {
                //var period = TempData[dataKey] as string;

                byte[] data = Session["DownloadExcelCustomerDetailPreview"] as byte[];
                string excelName = $"รายงานผลการนำเข้ารายชื่อผู้เอาประกัน_PA_ยิ้มแฉ่ง_{DateTime.Now}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }


        #endregion

        #region "PAPersonnelSearch"
        public JsonResult GetPersonnelApplication(
            int? year = null,
            int? branchId = null,
            string schoolName = null,
            string firstName = null,
            string lastName = null,
            int? draw = null,
            int? pageSize = null,
            int? indexStart = null,
            string sortField = null,
            string orderType = null,
            string searchDetail = null,
            int? provinceId = null
            )
        {
            var result = _db.usp_PesonnelApplicationSearch_Select(year, branchId, provinceId, schoolName, firstName, lastName, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }
        #endregion


        public JsonResult GetCustomerCountByRoundId(int applicationRoundId)
        {
            var rs = _db.usp_PersonnelApplicationRound_Select(null, null, applicationRoundId, null, null, 0, 9999, null, null, null).Sum(s => s.CustomerCount);

            return Json(rs, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCustomerCountByAppId(int applicationId)
        {
            var rs = _db.usp_PersonnelApplicationRoundByApplicationId_Select(applicationId, 0, 9999, null, null, null).Sum(s => s.CustomerCount);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNewApplicationMonitor(int? branchId = null, int? year = null, int? appStatusId = null, int? draw = null, int? pageSize = null, int? indexStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_PersonnelApplicationForPolicyMonitor_Select(null, branchId, year, appStatusId, indexStart, pageSize, sortField, orderType, search).ToList();
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetCustomerDetailHistory(int draw, int personnelApplicationId, int? indexStart = null, int? pageSize = null, string sortField = null, string orderType = null, string search = null)
        {
            var results = _db.usp_EndorsePersonnelCustomerDetailHistory_Select(personnelApplicationId, indexStart, pageSize, sortField, orderType, search).ToList();
            return Json(new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", results.Count() },
                {"recordsFiltered", results.Count() },
                {"data", results.ToList()}
            }, JsonRequestBehavior.AllowGet);
        }
    }
}