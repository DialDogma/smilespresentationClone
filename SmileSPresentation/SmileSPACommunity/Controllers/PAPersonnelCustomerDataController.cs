using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Vml.Wordprocessing;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using SmileSPACommunity.Helper;
using SmileSPACommunity.Models;
using SmileSPACommunity.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace SmileSPACommunity.Controllers
{
    [Authorization]
    public class PAPersonnelCustomerDataController : Controller
    {
        private PACommunityEntities _db;
        private string pattern = @"^0\d{8,9}$";

        public PAPersonnelCustomerDataController()
        {
            _db = new PACommunityEntities();
        }

        #region ผู้ขออนุมัติ

        [HttpGet]
        public ActionResult Index()
        {
            // role
            var roleDev = new[] { "Developer" };
            var rolePA = new[] { "PAPersonnel_PAUnderwrite" };

            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
            var role = new SSOService.SSOServiceClient().GetRoleByUserName(username);
            var rolelist = role.Split(',').ToList();
            if (rolelist.Intersect(rolePA).Count() > 0 || rolelist.Intersect(roleDev).Count() > 0)
            {
                ViewBag.Branchs = _db.usp_Branch_Select(null, 0, 99, null, null, null).ToList();
                ViewBag.Title = "รออนุมัติเพิ่มรายชื่อ";
            }
            else
            {
                ViewBag.Branchs = _db.usp_Branch_Select(branchId, 0, 99, null, null, null).ToList();
                ViewBag.Title = "เพิ่มรายชื่อผู้เอาประกัน";
            }

            ViewBag.educationYear = _db.usp_PolicyYear_Select(0, 999, null, null, null).ToList();
            ViewBag.status = _db.usp_EndorsePersonnelCustomerDetailStatus_Select(null).ToList();
            ViewBag.branchId = branchId;

            return View("Index");
        }

        [HttpPost]
        public JsonResult GetCustomerDetailData(int draw, int educationYears, string brances, string status, int? pageSize = null, int? indexStart = null, string sortField = null, string orderType = null, string search = null)
        {
            int? _status = null;
            if (status != "-1")
            {
                _status = Convert.ToInt32(status);
            }

            int? _branch = null;
            if (brances != "-1")
            {
                _branch = Convert.ToInt32(brances);
            }

            var results = _db.usp_EndorsePersonnelCustomerDetail_Select(educationYears,
                _branch,
                _status,
                search,
                indexStart,
                pageSize,
                sortField,
                orderType).ToList(); 

            return Json(new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", results.Count() != 0  ? results.FirstOrDefault().TotalCount : results.Count() },
                {"recordsFiltered", results.Count() != 0  ? results.FirstOrDefault().TotalCount : results.Count() },
                {"data", results.ToList()}
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SaveEditInsured()
        {
            // role
            var roleDev = new[] { "Developer" };
            var rolePA = new[] { "PAPersonnel_PAUnderwrite" };

            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = new SSOService.SSOServiceClient().GetRoleByUserName(username);
            var rolelist = role.Split(',').ToList();
            if (rolelist.Intersect(rolePA).Count() > 0 || rolelist.Intersect(roleDev).Count() > 0)
            {
                ViewBag.Branchs = _db.usp_Branch_Select(null, 0, 99, null, null, null).ToList();
                ViewBag.Title = "รออนุมัติเพิ่มรายชื่อ";
            }
            else
            {
                var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
                ViewBag.Branchs = _db.usp_Branch_Select(branchId, 0, 99, null, null, null).ToList();
                ViewBag.Title = "เพิ่มรายชื่อผู้เอาประกัน";
            }

            ViewBag.educationYear = _db.usp_PolicyYear_Select(0, 999, null, null, null).ToList();
            return View("SaveEditInsured");
        }

        [HttpGet]
        public JsonResult GetDataSaveEditInsured(int year, int? draw = null, int? pageSize = null, int? indexStart = null, string sortField = null, string orderType = null, string search = null)
        {
            // role
            var roleDev = new[] { "Developer" };
            var rolePA = new[] { "PAPersonnel_PAUnderwrite" };
            var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = new SSOService.SSOServiceClient().GetRoleByUserName(username);
            var rolelist = role.Split(',').ToList();
            int? newBranchId = branchId;
            if (rolelist.Intersect(rolePA).Count() > 0 || rolelist.Intersect(roleDev).Count() > 0)
            {
                newBranchId = null;
            }


            var results = _db.usp_PersonnelApplication_Select(null, newBranchId, year, 4, indexStart, pageSize, sortField, orderType, search).ToList();

            return Json(new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", results.Count() != 0 ? results.FirstOrDefault()?.TotalCount : results.Count()},
                {"recordsFiltered", results.Count() != 0 ? results.FirstOrDefault()?.TotalCount : results.Count()},
                {"data", results.ToList()}
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SaveEditInsuredViewData(string appId)
        {
            int decodeAppId = Convert.ToInt32(Base64Decode(appId));
            var User_ID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var result = _db.usp_EndorsePersonnelCustomerDatailHeader_Insert(decodeAppId, User_ID);
            foreach (var get in result.ToList())
            {
                ViewBag.Id = get.Result;
                ViewBag.IsResult = (get.IsResult == true ? "true" : "false");
            }


            ViewBag.AppId = appId;

            return View("SaveEditInsuredViewData");
        }

        [HttpPost]
        public JsonResult GetInsuredData(string appId, int? draw = null, int? pageSize = null, int? indexStart = null, string sortField = null, string orderType = null, string search = null)
        {
            int decodeAppId = Convert.ToInt32(Base64Decode(appId));
            var results = _db.usp_PersonnelCustomerDetail_Select(decodeAppId, null, null, null, indexStart, pageSize, sortField, orderType, search).ToList();
            return Json(new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", results.Count() != 0 ? results.FirstOrDefault()?.TotalCount : results.Count()},
                {"recordsFiltered", results.Count() != 0 ? results.FirstOrDefault()?.TotalCount : results.Count()},
                {"data", results.ToList()}
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDocumentInsuredData(string referentId, int? draw = null, int? pageSize = null, int? indexStart = null, string sortField = null, string orderType = null, string search = null)
        {
            int decodeAppId = Convert.ToInt32(Base64Decode(referentId));
            var results = _db.usp_DocumentEndorsePersonnelCustomerDetail_Select(decodeAppId, 45, indexStart, pageSize, sortField, orderType, search).ToList();

            return Json(new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", results.Count() != 0 ? results.FirstOrDefault()?.TotalCount : results.Count()},
                {"recordsFiltered", results.Count() != 0 ? results.FirstOrDefault()?.TotalCount : results.Count()},
                {"data", results.ToList()}
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetInsuredDataUpdateLists(string referentId, int? draw = null, int? pageSize = null, int? indexStart = null, string sortField = null, string orderType = null, string search = null)
        {
            int decodeAppId = Convert.ToInt32(Base64Decode(referentId));
            var results = _db.usp_EndorsePersonnelCustomerDetailItem_Select(decodeAppId, null, indexStart, pageSize, sortField, orderType, search).ToList();

            return Json(new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", results.Count() != 0 ? results.FirstOrDefault()?.TotalCount : results.Count()},
                {"recordsFiltered", results.Count() != 0 ? results.FirstOrDefault()?.TotalCount : results.Count()},
                {"data", results.ToList()}
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FormEditInsuredPerson(string PersonnelCustomerDetailId, int EndorsePersonnelCustomerDetailHeaderId)
        {
            // decode 
            int deCodePersonnelCustomerDetailId = Convert.ToInt32(Base64Decode(PersonnelCustomerDetailId));
            var result = _db.usp_PersonnelCustomerDetailById_Select(deCodePersonnelCustomerDetailId).FirstOrDefault();

            ViewBag.Titles = new SelectList(_db.usp_Title_Select(null, 0, 999, null, null, null).ToList(), "TitleId", "Title", result.TitleId);
            ViewBag.EndorsePersonnelCustomerDetailHeaderId = EndorsePersonnelCustomerDetailHeaderId;

            return PartialView("FormEditInsuredPerson", result);
        }

        [HttpPost]
        public ActionResult FormEditInsuredPerson(usp_PersonnelCustomerDetailById_Select_Result model, int EndorsePersonnelCustomerDetailHeaderId)
        {
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            try
            {
                // get before data
                var getOldData = _db.usp_PersonnelCustomerDetailById_Select(model.PersonnelCustomerDetailId).FirstOrDefault();

                // ตรวจสอบเลขบัตร ปชช.
                if (model.IdCardNumber != null && VerifyPeopleID(model.IdCardNumber) == false)
                {
                    return Json(new { valid = false, message = "หมายเลขบัตรประชาชนไม่ถูกต้องกรุณาตรวจสอบ" });
                }

                if ((model.IdCardNumber == null ? "" : model.IdCardNumber) == getOldData.IdCardNumber && model.TitleId == getOldData.TitleId && model.FirstName == getOldData.FirstName &&
                    model.LastName == getOldData.LastName &&
                    model.Position == getOldData.Position &&
                    model.MobileNumber == getOldData.MobileNumber &&
                    (model.PassPortNumber == null ? "" : model.PassPortNumber) == (getOldData.PassPortNumber == null ? "" : getOldData.PassPortNumber))
                {
                    return Json(new { valid = false, message = "ไม่สามารถบันทึกได้เนื่องจากท่านไม่ได้ทำการแก้ไขข้อมูล" });
                }

                // ตรวจรายการซ้ำ ของข้อมูลเลขบัตร ปชช./ เลขที่ passport
                var checkDataInlistUpdated = _db.usp_EndorsePersonnelCustomerDetailItem_Select(EndorsePersonnelCustomerDetailHeaderId, null, null, null, null, null, null).ToList();
                if (model.IdCardNumber != null && checkDataInlistUpdated.Any(a => a.ToIdCard == model.IdCardNumber))
                {
                    return Json(new { valid = false, message = "หมายเลขบัตรประชาชน มีในรายการแก้ไขผู้เอาประกันแล้ว" });
                }

                if (model.PassPortNumber != null && checkDataInlistUpdated.Any(a => a.ToPassport == model.PassPortNumber))
                {
                    return Json(new { valid = false, message = "หมายเลข Passport มีในรายการแก้ไขผู้เอาประกันแล้ว" });
                }

                if (model.MobileNumber != null && checkDataInlistUpdated.Any(a => a.ToMobileNumber == model.MobileNumber))
                {
                    return Json(new { valid = false, message = "เบอร์โทรศัพท์มีในรายการแก้ไขผู้เอาประกันแล้ว" });
                }

                if (!Regex.IsMatch(model.MobileNumber, pattern))
                {
                    return Json(new { valid = false, message = "รูปแบบเบอร์โทรศัพท์ไม่ถูกต้อง" });
                }


                // insert data
                var result = _db.usp_EndorsePersonnelCustomerDetailItem_Insert(
                     EndorsePersonnelCustomerDetailHeaderId,
                     model.PersonnelCustomerDetailId,
                     getOldData.PersonnelCustomerDetailCode,
                     getOldData.IdCardNumber,
                     getOldData.PassPortNumber,
                     getOldData.TitleId,
                     getOldData.FirstName,
                     getOldData.LastName,
                     getOldData.Position,
                     getOldData.MobileNumber,
                     model.IdCardNumber,
                     model.PassPortNumber,
                     model.TitleId,
                     model.FirstName,
                     model.LastName,
                     model.Position,
                     model.MobileNumber,
                     userId);

                foreach (var responseResult in result)
                {
                    if (responseResult.IsResult == false)
                    {
                        return Json(new { valid = responseResult.IsResult, message = responseResult.Msg });
                    }
                }

            }
            catch (Exception error)
            {
                return Json(new { valid = false, message = (error.InnerException == null ? error.Message : error.InnerException.Message) });
            }

            return Json(new { valid = true });
        }

        [HttpGet]
        public ActionResult InsuredPersonViewDetail(int endorsePersonnelCustomerDetailItemId)
        {
            var result = _db.usp_EndorsePersonnelCustomerDetailItemById_Select(endorsePersonnelCustomerDetailItemId).FirstOrDefault();
            return PartialView("InsuredPersonDetail", result);
        }

        [HttpPost]
        public ActionResult InsuredPersonDeleteDetail(int endorsePersonnelCustomerDetailItemId)
        {
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            try
            {
                var result = _db.usp_EndorsePersonnelCustomerDetailItem_Delete(endorsePersonnelCustomerDetailItemId, userId);
                foreach (var responseResult in result)
                {
                    if (responseResult.IsResult == false)
                    {
                        return Json(new { valid = responseResult.IsResult, message = responseResult.Msg });
                    }
                    else
                    {
                        return Json(new { valid = true });
                    }
                }

            }
            catch (Exception error)
            {
                return Json(new { valid = false, message = (error.InnerException == null ? error.Message : error.InnerException.Message) });
            }



            return Json(new { valid = true });
        }

        [HttpGet]
        public ActionResult FormEditCustomerDetailItem(int EndorsePersonnelCustomerDetailItemId, int EndorsePersonnelCustomerDetailHeaderId)
        {
            var result = _db.usp_EndorsePersonnelCustomerDetailItem_Select(EndorsePersonnelCustomerDetailHeaderId, EndorsePersonnelCustomerDetailItemId, null, null, null, null, null).FirstOrDefault();

            ViewBag.Titles = new SelectList(_db.usp_Title_Select(null, 0, 999, null, null, null).ToList(), "TitleId", "Title", result.ToTitleId);
            ViewBag.EndorsePersonnelCustomerDetailHeaderId = EndorsePersonnelCustomerDetailHeaderId;

            return PartialView("FormEditCustomerDetailItem", result);

            //usp_EndorsePersonnelCustomerDetailItem_Update
        }

        [HttpPost]
        public ActionResult FormEditCustomerDetailItem(usp_EndorsePersonnelCustomerDetailItem_Select_Result model)
        {
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            if (model.ToIdCard == null && model.ToPassport == null)
            {
                return Json(new { valid = false, message = "กรุณากรอกข้อมูล" });
            }

            if (model.ToIdCard != null && !VerifyPeopleID(model.ToIdCard))
            {
                return Json(new { valid = false, message = "หมายเลขบัตรประชาชนไม่ถูกต้องกรุณาตรวจสอบ" });
            }



            try
            {
                // ตรวจรายการซ้ำ ของข้อมูลเลขบัตร ปชช./ เลขที่ passport
                var checkDataInlistUpdated = _db.usp_EndorsePersonnelCustomerDetailItem_Select(model.EndorsePersonnelCustomerDetailHeaderId, null, null, null, null, null, null).ToList();
                if (model.ToIdCard != null && checkDataInlistUpdated.Where(w => w.EndorsePersonnelCustomerDetailItemId != model.EndorsePersonnelCustomerDetailItemId).Any(a => a.ToIdCard == model.ToIdCard))
                {
                    return Json(new { valid = false, message = "หมายเลขบัตรประชาชน มีในรายการแก้ไขผู้เอาประกันแล้ว" });
                }

                if (model.ToPassport != null && checkDataInlistUpdated.Where(w => w.EndorsePersonnelCustomerDetailItemId != model.EndorsePersonnelCustomerDetailItemId).Any(a => a.ToPassport == model.ToPassport))
                {
                    return Json(new { valid = false, message = "หมายเลข Passport มีในรายการแก้ไขผู้เอาประกันแล้ว" });
                }

                if (model.ToMobileNumber != null && checkDataInlistUpdated.Where(w => w.EndorsePersonnelCustomerDetailItemId != model.EndorsePersonnelCustomerDetailItemId).Any(a => a.ToMobileNumber == model.ToMobileNumber))
                {
                    return Json(new { valid = false, message = "เบอร์โทรศัพท์มีในรายการแก้ไขผู้เอาประกันแล้ว" });
                }

                if (!Regex.IsMatch(model.ToMobileNumber, pattern))
                {
                    return Json(new { valid = false, message = "รูปแบบเบอร์โทรศัพท์ไม่ถูกต้อง" });
                }


                // insert data
                var result = _db.usp_EndorsePersonnelCustomerDetailItem_Update(
                     model.EndorsePersonnelCustomerDetailItemId,
                     model.ToIdCard,
                     model.ToPassport,
                     model.ToTitleId,
                     model.ToFirstName,
                     model.ToLastName,
                     model.ToPosition,
                     model.ToMobileNumber,
                     userId);

                foreach (var responseResult in result)
                {
                    if (responseResult.IsResult == false)
                    {
                        return Json(new { valid = responseResult.IsResult, message = responseResult.Msg });
                    }
                    else
                    {
                        return Json(new { valid = true });
                    }
                }

            }
            catch (Exception error)
            {
                return Json(new { valid = false, message = (error.InnerException == null ? error.Message : error.InnerException.Message) });
            }

            return Json(new { valid = true });
        }

        [HttpPost]
        public ActionResult SendToApprove(int EndorsePersonnelCustomerDetailHeaderId)
        {
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            try
            {
                var getCustomerDetailItems = _db.usp_EndorsePersonnelCustomerDetailItem_Select(EndorsePersonnelCustomerDetailHeaderId, null, null, null, null, null, null).ToList();
                if (getCustomerDetailItems.Count() == 0)
                {
                    return Json(new { valid = false, message = "ไม่มีรายการแก้ไขกรุณาตรวจสอบข้อมูล" });
                }

                // add transaction to approve process
                var result = _db.usp_EndorsePersonnelCustomerDetailHeader_Sent_Update(EndorsePersonnelCustomerDetailHeaderId, userId).FirstOrDefault();
                if (result.IsResult == false)
                {
                    return Json(new { valid = false, message = result.Msg });
                }
            }
            catch (Exception error)
            {
                return Json(new { valid = false, message = (error.InnerException == null ? error.Message : error.InnerException.Message) });
            }

            return Json(new { valid = true });
        }

        [HttpPost]
        public ActionResult GetCommentInfo(string approveCause, string remark)
        {
            var modal = new CommentInfoDto
            {
                approveCause = approveCause,
                remark = (remark == "null" ? "-" : remark)
            };

            return PartialView("GetCommentInfo", modal);
        }

        #endregion

        #region  ผู้อนุมัติ

        [HttpGet]
        public ActionResult InsuredNameLists(string PersonnelApplicationId, string endorsePersonnelCustomerDetailHeaderId, string isApp)
        {
            // role
            var roleDev = new[] { "Developer" };
            var rolePA = new[] { "PAPersonnel_PAUnderwrite" };

            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = new SSOService.SSOServiceClient().GetRoleByUserName(username);
            var rolelist = role.Split(',').ToList();
            if (rolelist.Intersect(rolePA).Count() > 0 || rolelist.Intersect(roleDev).Count() > 0)
            {
                ViewBag.isApp = true;
            }
            else
            {
                ViewBag.isApp = false;
            }


            ViewBag.PersonnelApplicationId = PersonnelApplicationId;
            ViewBag.endorsePersonnelCustomerDetailHeaderId = endorsePersonnelCustomerDetailHeaderId;
            ViewBag.statusId = Convert.ToInt32(Base64Decode(isApp));

            return View("InsuredNameLists");
        }

        [HttpGet]
        public ActionResult ApproveCustomerData(int endorsePersonnelCustomerDetailHeaderId, int statusId)
        {
            ViewBag.ApproveCause = _db.usp_ApproveCause_Select(null, 9, 0, 9999, null, null, null).ToList();
            ViewBag.endorsePersonnelCustomerDetailHeaderId = endorsePersonnelCustomerDetailHeaderId;
            ViewBag.statusId = statusId;
            return PartialView("ApproveCustomerData");
        }

        [HttpPost]
        public ActionResult ApproveCustomerDetail(ApproveCustomerDetailDto model)
        {
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            try
            {
                int? _approveStatus = null;
                if (model.statusId == 4)
                {
                    _approveStatus = model.approveStatus;
                }

                var result = _db.usp_EndorsePersonnelCustomerDetail_Approve_Update(model.endorsePersonnelCustomerDetailHeaderId, model.statusId, _approveStatus, model.approveRemark, userId).FirstOrDefault();
                if (result.IsResult == false)
                {
                    return Json(new { valid = false, message = result.Msg });
                }
            }
            catch (Exception error)
            {
                return Json(new { valid = false, message = error.InnerException == null ? error.Message : error.InnerException.Message });
            }

            return Json(new { valid = true });

        }

        #endregion

        #region function helper

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private Boolean VerifyPeopleID(string idcardno)
        {
            if (string.IsNullOrEmpty(idcardno))
            {
                return false;
            }
            if (idcardno.Length != 13)
            {
                return false;
            }
            bool isDigit = Regex.IsMatch(idcardno, @"^[0-9]*$");
            if (!isDigit)
            {
                return false;
            }
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                sum += Convert.ToInt32(idcardno.Substring(i, 1)) * (13 - i);
            }
            int checksum = (11 - (sum % 11)) % 10;
            if (checksum != Convert.ToInt32(idcardno.Substring(12)))
            {
                return false;
            }
            return true;

        }


        #endregion

    }
}