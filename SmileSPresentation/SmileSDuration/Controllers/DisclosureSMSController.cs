using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSDuration.ComunicateService;
using SmileSDuration.Helper;
using SmileSDuration.Models;

namespace SmileSDuration.Controllers
{
    [Authorization(Roles = "Developer")]
    public class DisclosureSMSController : Controller
    {
        // GET: DisclosureSMS
        public ActionResult Index()
        {
            return View();
        }

        // GET: Search For SMS
        public ActionResult SearchSMS()
        {
            using (var db = new CommunicateV1Entities1())
            {
                ViewBag.SMSType = db.usp_SMSType_Select().ToList();
                return View();
            }
        }

        // GET: DisclosureSMS Detail for Datatable
        public ActionResult GetDatatables_Detail(int top)
        {
            using (var db = new CustomerBaseEntities())
            {
                //GET DATATA FOR INSERT TO TEMP
                var lst = db.usp_DisclosureSMSDetail_Select(top).ToList();

                if (lst.Count > 0)
                {
                    using (var dbDuration = new DurationV1Entities())
                    {
                        //Add New Header
                        var resultTempHeader = dbDuration.usp_TempImportHeaderV2_Insert().FirstOrDefault();
                        var Id = resultTempHeader.TempImportHeaderID;
                        foreach (var item in lst)
                        {
                            var resultTempDetail = dbDuration.usp_TempImportDetail_Insert(
                                Id,
                                null,
                                item.ComunicateTypeId,
                                item.ApplicationCode,
                                null,
                                null,
                                null,
                                null,
                                item.MobileNumber,
                                item.Message,
                                null,
                                null,
                                null,
                                null,
                                null,
                                null,
                                null,
                                Convert.ToString(item.DisclosureSMSDetailId));
                        }
                        var lstResultTemp = dbDuration.usp_PreviewV2_Select(Id).ToList();
                        return Json(lstResultTemp, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
        }

        // GET: ComunicateSearchSMS for Datatable
        public ActionResult GetDatatables_ComunicateSearchSMS(int draw, int smsTypeId, string mobilePhoneNo, int indexStart, int pageSize, string sortField, string orderType)
        {
            using (var db = new DurationV1Entities())
            {
                //GET DATATA
                var list = db.usp_ComunicateSearchSMS_Select(smsTypeId, mobilePhoneNo, null, null, indexStart, pageSize, sortField, orderType, null).ToList();

                var dt = new Dictionary<string, object>
                {
                    {"draw", draw },
                    {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                    {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                    {"data", list.ToList()}
                };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: DisclosureSMS Detail for Datatable
        public ActionResult DisclosureSMSDetail_CountSentSMS()
        {
            using (var db = new CustomerBaseEntities())
            {
                var obj = db.usp_DisclosureSMSDetail_CountSentSMS_Select().FirstOrDefault();

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        public void PreviewDisclosureSMS(int tempHeaderId)
        {
            using (var dbDuration = new DurationV1Entities())
            {
                var lstResultTemp = dbDuration.usp_PreviewV2_Select(tempHeaderId).Select(o => new
                {
                    ID = o.RefNo,
                    เบอร์โทรศัพท์ = o.MobilePhoneNo,
                    ApplicationID = o.AppID,
                    ข้อความ = o.Message
                }).ToList();

                ExcelManager.ExportToExcel(HttpContext, lstResultTemp, "PreviewSMS", "PreviewSMS");
            }
        }

        public ActionResult SubmitToSendSMS(int tempHeaderId)
        {
            using (var db = new DurationV1Entities())
            {
                var tempImportHeaderID = tempHeaderId;

                //INSERT TEPM IMPORT HEADER
                db.usp_TempImportHeader_Insert(tempImportHeaderID);

                //GET LIST BY TEMP IMPORT HEADER ID
                var listDuration = db.usp_ComunicateByTempImportHeaderID_ForDisclosureSMS_Select(tempImportHeaderID).ToList();

                foreach (var item in listDuration)
                {
                    using (var dbCusbase = new CustomerBaseEntities())
                    {
                        //check
                        var isValidBeforeSend = dbCusbase.usp_DisclosureSMSDetailValid_Select(int.Parse(item.RefNo)).FirstOrDefault();
                        //valid == true
                        if (isValidBeforeSend.IsResult == true)
                        {
                            //UPDATE BEFORE SEND
                            dbCusbase.usp_DisclosureSMSDetail_IsSentSMS_Update(int.Parse(item.RefNo), item.ComunicateID);

                            var sms = new SMS
                            {
                                Message = item.Message,
                                SenderID = 3, //3 = ผู้ส่ง SiamSmile
                                SMSTypeID = item.MappingSMSTypeId,
                                SendDate = DateTime.Now,
                                CreatedDate = DateTime.Now,
                                CreatedByID = 1,
                                SectionID = 1
                            };

                            using (var smsService = new SmsServiceClient())
                            {
                                //CALL SERVICE SEND SMS
                                var result = smsService.SendSmsV2(sms, item.MobilePhoneNo);
                                //UPDATE TO DB.DURATION
                                db.usp_ComunicateTransactionByComunicateID_Update(item.ComunicateID, result.SMSTransectionID);
                                //UPDATE AFTER SEND
                                dbCusbase.usp_DisclosureSMSDetail_TrStatus_Update(int.Parse(item.RefNo), result.Status);
                            }
                        }
                    }
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
    }
}