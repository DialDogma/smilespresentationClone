using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSUnderwrite.DatacenterOrganizeService;
using SmileSUnderwrite.Helper;
using SmileSUnderwrite.Models;
using SmileSUnderwrite.SmileSPAService;
using System.Globalization;

namespace SmileSUnderwrite.Controllers
{
    [Authorization]
    public class PAUnderwriteCallController : Controller
    {
        //private readonly CultureInfo dateTH = new CultureInfo("th-TH");

        #region dbContext

        private UnderwriteDBContext _context;

        public PAUnderwriteCallController()

        {
            _context = new UnderwriteDBContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion dbContext

        // PAUnderwriteCall view
        public ActionResult PAUnderwriteCall(int queueId)
        {
            var user = Convert.ToInt32(Session["User_ID"]);
            //insert underwrite by queue id
            var underwrite = _context.usp_Underwrite_Insert(queueId, user).FirstOrDefault();
            //call other insurance
            //using(var datacenter = new OrganizeServiceClient())
            //{
            //    ViewBag.OtherInsurance = datacenter.GetOtherInsurance(null).Where(x => x.InsuranceId != 14).OrderBy(x => x.Insurance);
            //}
            ViewBag.OtherInsurance = _context.usp_InsuranceForUdw_Select().ToList();

            //get call detail id
            var underwriteDetail = _context.usp_Underwrite_Select(underwrite).FirstOrDefault();
            ViewBag.UnderwiteId = underwrite;
            ViewBag.UnderwriteDetail = underwriteDetail;
            //get question id
            var queueDetail = _context.usp_Queue_Select(queueId, null, null, null, null, null, null, null, null, null, "", "", "", null).FirstOrDefault();
            ViewBag.QuestionId = queueDetail.QuestionId;
            ViewBag.QueueTypeDetail = queueDetail.QueueTypeDetail;
            ViewBag.QueueCreated = queueDetail.CreatedDate;
            //call ref id
            ViewBag.ReferenceCode = queueDetail.ReferenceCode;

            //get coop name
            var client = new ApplicationServiceClient();
            var obj = client.GetApplicationDetailForPAUnderwrite(queueDetail.ReferenceCode);
            if (obj != null)
            {
                ViewBag.AppId = obj.App_id;
                ViewBag.SchoolId = obj.School_id;
                ViewBag.CoopName = obj.ContactName;
                ViewBag.AppDetail = obj;
            }

            //check if underwriteDetail is not null
            if (underwriteDetail != null)
            {
                //check if question id
                if (underwriteDetail.QuestionId == 2)
                {
                    var resultQ2 = _context.usp_UdwQ2_Select(underwrite).FirstOrDefault();
                    if (resultQ2 != null)
                    {
                        ViewBag.ResultQ2 = resultQ2;

                        if (resultQ2.A9 != null)
                        {
                            ViewBag.DatePayment = resultQ2.A9.Value.ToString("dd/MM/yyyy");
                        }
                    }
                }
                else if (underwriteDetail.QuestionId == 3)
                {
                    var resultQ3 = _context.usp_UdwQ3_Select(underwrite).FirstOrDefault();
                    if (resultQ3 != null)
                    {
                        ViewBag.ResultQ3 = resultQ3;
                    }
                }
                else if (underwriteDetail.QuestionId == 4)
                {
                    var resultQ4 = _context.usp_UdwQ4_Select(underwrite).FirstOrDefault();
                    if (resultQ4 != null)
                    {
                        ViewBag.ResultQ4 = resultQ4;
                        if (resultQ4.A31 != null)
                        {
                            ViewBag.DatePayment2 = resultQ4.A31.Value.ToString("dd/MM/yyyy");
                        }
                    }
                }
            }
            //check if have call
            var callSelect = _context.usp_Call_Select(underwrite, true).FirstOrDefault();

            if (callSelect != null)
            {
                ViewBag.CallDetail = callSelect;
            }
            ViewBag.Remark = underwriteDetail.Remark;
            return View();
        }

        //PAUnderwriteCall post
        [HttpPost]
        public ActionResult PAUnderwriteCallPost(FormCollection form)
        {
            //collection form for call insert
            var radioValue = form["radio_call"];
            var underwriteId = Convert.ToInt32(form["hd_underwriteId"]);
            //check if checked
            if (radioValue == "option1")
            {
                var contactName = form["txtRecieverName"];
                //try insert call
                //insert create by user
                try
                {
                    var result = _context.usp_Call_Insert(underwriteId, 2, null, contactName, Convert.ToInt32(Session["User_ID"]));
                }
                catch (Exception e)
                {
                    return RedirectToAction("InternalServerError", "Error", new { exception = e });
                }
            }
            else if (radioValue == "option2")
            {
                //try insert call
                //insert create by user
                try
                {
                    var result = _context.usp_Call_Insert(underwriteId, 3, null, null, Convert.ToInt32(Session["User_ID"]));
                }
                catch (Exception e)
                {
                    return RedirectToAction("InternalServerError", "Error", new { exception = e });
                }
            }
            else if (radioValue == "option3")
            {
                //try insert call
                //TODO:insert create by user
                try
                {
                    var result = _context.usp_Call_Insert(underwriteId, 4, null, null, Convert.ToInt32(Session["User_ID"]));
                }
                catch (Exception e)
                {
                    return RedirectToAction("InternalServerError", "Error", new { exception = e });
                }
            }
            else if (radioValue == "option4")
            {
                var anotherCause = form["txtEtc"];

                //try insert call
                //TODO:insert create by user
                try
                {
                    var result = _context.usp_Call_Insert(underwriteId, 5, anotherCause, null, Convert.ToInt32(Session["User_ID"]));
                }
                catch (Exception e)
                {
                    return RedirectToAction("InternalServerError", "Error", new { exception = e });
                }
            }
            //call underwrite detail
            var underwriteDetail = _context.usp_Underwrite_Select(underwriteId).FirstOrDefault();
            //check if underwrite not null
            if (underwriteDetail != null)
            {
                //q2
                bool A2 = (form["chk_option1"] == "on") ? true : false;
                string A3 = "";
                if (A2)
                {
                    A3 = form["select_InsuranceCo"];
                }
                bool A4 = (form["chk_option2"] == "on") ? true : false;
                string A5 = "";
                if (A4)
                {
                    A5 = form["txtCause"];
                }
                bool A6 = (form["chk_option3"] == "on") ? true : false;
                bool A7 = (form["chk_option4"] == "on") ? true : false;
                bool A10 = (form["chk_option6"] == "on") ? true : false;
                string A11 = "";
                if (A10)
                {
                    A11 = form["txtAnotherCause"];
                }

                //q3 exclude this
                bool A8 = (form["chk_option5"] == "on") ? true : false;
                DateTime? A9 = null;
                if (A8)
                {
                    if (form["DatePayment"] != "")
                    {
                        A9 = Convert.ToDateTime(form["DatePayment"]);
                    }
                }

                //q4
                bool A20 = (form["chk_option2_1"] == "on") ? true : false;
                bool A21 = (form["chk_option2_2"] == "on") ? true : false;
                var chk_opt2_2_1 = form["chk_option2_2_1"];
                var A22 = false;
                var A23 = "";
                var A24 = false;
                var A25 = "";
                var A26 = false;
                var A27 = "";
                var A28 = false;
                var A29 = "";
                if (chk_opt2_2_1 != null)
                {
                    var arrChk = chk_opt2_2_1.Split(',');
                    for (int i = 0; i < arrChk.Length; i++)
                    {
                        switch (arrChk[i])
                        {
                            case "A22":
                                A22 = true;
                                A23 = form["txtPersonUnequal"];
                                break;

                            case "A24":
                                A24 = true;
                                A25 = form["txtDiscountUnequal"];
                                break;

                            case "A26":
                                A26 = true;
                                A27 = form["txtPaymentUnequal"];
                                break;

                            case "A28":
                                A28 = true;
                                A29 = form["txtEtcUnequal"];
                                break;

                            default:
                                break;
                        }
                    }
                }

                //bool A22 = (form["chk_option2_2_1"] == "A22") ? true : false;
                //string A23 = "";
                //if (A22)
                //{
                //    A23 = form["txtPersonUnequal"];
                //}
                //bool A24 = (form["chk_option2_2_2"] == "A24") ? true : false;
                //string A25 = "";
                //if (A24)
                //{
                //    A25 = form["txtDiscountUnequal"];
                //}
                //bool A26 = (form["chk_option2_2_3"] == "A26") ? true : false;
                //string A27 = "";
                //if (A26)
                //{
                //    A27 = form["txtPaymentUnequal"];
                //}
                //bool A28 = (form["chk_option2_2_4"] == "A28") ? true : false;
                //string A29 = "";
                //if (A28)
                //{
                //    A29 = form["txtEtcUnequal"];
                //}

                bool A30 = (form["chk_option2_3"] == "on") ? true : false;
                DateTime? A31 = null;
                string A32 = "";
                if (A30)
                {
                    if (form["DatePayment2"] != "")
                    {
                        A31 = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(form["DatePayment2"]).ToString());
                    }
                    A32 = form["txtAgentName"];
                }

                //call result
                var radioCallResult = form["radio_callResult"];
                double corruptAmount = 0;
                int underwriteStatusId = 1;

                //remark
                var remark = form["txtRemark"];

                //check what question id
                if (underwriteDetail.QuestionId == 2)
                {
                    //check if radio checked(call result)
                    if (radioCallResult == "option1")
                    {
                        var corruptAmountText = form["txtAmountCorrupt"];
                        if (corruptAmountText != "")
                        {
                            corruptAmount = Convert.ToDouble(corruptAmountText);
                        }
                        underwriteStatusId = 3;
                    }
                    else if (radioCallResult == "option2")
                    {
                        corruptAmount = 0;
                        underwriteStatusId = 2;
                    }

                    //try insert Q2
                    //TODO:insert create by user
                    try
                    {
                        var resultQ2 = _context.usp_UdwQ2_InsertOrUpdate(underwriteId, underwriteStatusId
                            , corruptAmount, remark, Convert.ToInt32(Session["User_ID"]), A2, A3, A4, A5, A6, A7, A8, A9, A10, A11).FirstOrDefault();
                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("InternalServerError", "Error", new { exception = e });
                    }
                }
                else if (underwriteDetail.QuestionId == 3)
                {
                    //check if radio checked(call result)
                    if (radioCallResult == "option1")
                    {
                        var corruptAmountText = form["txtAmountCorrupt"];
                        if (corruptAmountText != "")
                        {
                            corruptAmount = Convert.ToDouble(corruptAmountText);
                        }

                        underwriteStatusId = 3;
                    }
                    else if (radioCallResult == "option2")
                    {
                        corruptAmount = 0;
                        underwriteStatusId = 2;
                    }
                    try
                    {
                        var resultQ3 = _context.usp_UdwQ3_InsertOrUpdate(underwriteId, underwriteStatusId
                            , corruptAmount, remark, Convert.ToInt32(Session["User_ID"]), A2, A3, A4, A5, A6, A7, A10, A11).FirstOrDefault();
                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("InternalServerError", "Error", new { exception = e });
                    }
                }
                else if (underwriteDetail.QuestionId == 4)
                {
                    //check if radio checked(call result)
                    if (radioCallResult == "option1")
                    {
                        var corruptAmountText = form["txtAmountCorrupt"];
                        if (corruptAmountText != "")
                        {
                            corruptAmount = Convert.ToDouble(corruptAmountText);
                        }
                        underwriteStatusId = 3;
                    }
                    else if (radioCallResult == "option2")
                    {
                        corruptAmount = 0;
                        underwriteStatusId = 2;
                    }
                    try
                    {
                        var resultQ4 = _context.usp_UdwQ4_InsertOrUpdate(underwriteId, underwriteStatusId
                            , corruptAmount, remark, Convert.ToInt32(Session["User_ID"]), A20, A21, A22, A23, A24, A25, A26, A27, A28, A29, A30, A31, A32).FirstOrDefault();
                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("InternalServerError", "Error", new { exception = e });
                    }
                }
                else
                {
                    return RedirectToAction("NotFound", "Error");
                }
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
            var submitType = form["hd_submitType"];
            if (submitType == "submit")
            {
                _context.usp_QueueStatus_Update(underwriteDetail.QueueId, 4, Convert.ToInt32(Session["User_ID"]));
            }
            else if (submitType == "wait")
            {
                _context.usp_QueueStatus_Update(underwriteDetail.QueueId, 3, Convert.ToInt32(Session["User_ID"]));
            }
            return RedirectToAction("QueueCallIndex", "QueueCall");
        }

        [HttpPost]
        public JsonResult UnderwriteHistory(int? schoolId, int? indexStart, int? pageSize,
            string sortField, string orderType, string search)
        {
            try
            {
                var result = _context.usp_UnderWriteHistoryBySchoolId_SelectV2(schoolId.ToString(),
                    indexStart, pageSize, sortField, orderType, search).ToList();

                var dt = new Dictionary<string, object>
            {
                //{"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };
                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                var r = new List<usp_UnderWriteHistoryBySchoolId_Select_Result>();
                var dt2 = new Dictionary<string, object>
            {
                //{"draw", draw },
                {"recordsTotal", 0},
                {"recordsFiltered", 0},
                {"data", r}
            };
                return Json(dt2, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AssignQueue(int queueId, int assignToUserId, int createByUserId)
        {
            var result = _context.usp_AssignQueue(queueId, assignToUserId, Convert.ToInt32(Session["User_ID"])).FirstOrDefault();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //Underwrite history
        public ActionResult UnderwriteHistory(string schoolId)
        {
            var schoolBase64 = GlobalFunction.Base64StringDecode(schoolId);

            var result = _context.usp_UnderWriteHistoryBySchoolId_SelectV2(schoolBase64,
                null, null, null, null, null).FirstOrDefault();
            ViewBag.NoData = null;
            if (result != null)
            {
                ViewBag.SchoolName = result.Detail2;

                ViewBag.SchoolId = Convert.ToInt32(schoolBase64);
                ViewBag.Remark = result.Remark;
            }
            else
            {
                ViewBag.NoData = "ไม่มีประวัติการโทรในฐานข้อมูล";
            }

            return View();
        }
    }
}