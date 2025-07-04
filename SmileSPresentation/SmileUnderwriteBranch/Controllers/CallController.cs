using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using SmileUnderwriteBranch.Helper;
using SmileUnderwriteBranch.Models;
using SmileUnderwriteBranch.SSSDocService;
using SmileUnderwriteBranch.SmileDocService;
using SmileUnderwriteBranch.UnderwriteBranchService;
using System.Text.RegularExpressions;

namespace SmileUnderwriteBranch.Controllers
{
    [Authorization]
    public class CallController : Controller
    {
        /// <summary>
        /// INDEX
        /// </summary>
        /// <param name="queueID"></param>
        /// <param name="appCode"></param>
        /// <returns></returns>
        public ActionResult CallIndex(string queueID = "", string appCode = "")
        {
            //declare variable application code
            var applicationCode = "";
            //get userId
            var user_id = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.UserId = user_id;

            //check queueId
            if (queueID != "")
            {
                //decode
                var queueBase64EncodedBytes = Convert.FromBase64String(queueID);
                var QueueId = System.Text.Encoding.UTF8.GetString(queueBase64EncodedBytes);
                //set viewbag = queue id
                ViewBag.QueueId = QueueId;
                using (var db = new UnderwriteBranchEntities())
                {
                    //get PHqueueId by appcode
                    var lst = db.usp_PHQueueByQueueId_Select(Convert.ToInt32(QueueId)).First();
                    //get callstatus
                    var lst_callstatus = db.usp_CallStatus_Select(null).ToList();
                    ViewBag.CallStatusList = lst_callstatus;

                    var lstType = db.usp_PHQueueType_Select().ToList();
                    ViewBag.QueueType = lstType;

                    //set application code
                    applicationCode = lst.ApplicationCode;

                    //set session
                    Session["ViewSessionUser"] = user_id == lst.AssignToUserId;
                }
            }
            //check appcode
            else if (appCode != "")
            {
                //decode
                var AppCodeBase64EncodedBytes = Convert.FromBase64String(appCode);
                var AppCode = System.Text.Encoding.UTF8.GetString(AppCodeBase64EncodedBytes);
                //set application code
                applicationCode = AppCode;
                using (var db = new UnderwriteBranchEntities())
                {
                    //get PHqueueId by appcode
                    var lst = db.usp_PHQueueByAppCode_Select(AppCode).First();
                    //get callstatus
                    var lst_callstatus = db.usp_CallStatus_Select(null).ToList();
                    ViewBag.CallStatusList = lst_callstatus;

                    var lstType = db.usp_PHQueueType_Select().ToList();
                    ViewBag.QueueType = lstType;

                    //set viewbag = queue id
                    ViewBag.QueueId = lst.PHQueueId;

                    //set session
                    Session["ViewSessionUser"] = user_id == lst.AssignToUserId;
                }
            }
            else
            {
                //set viewbag queue id = 0
                ViewBag.QueueId = 0;
            }

            //DOCUMENT
            using (var docClient = new SSSDocService.DocumentServiceClient())
            {
                if (applicationCode != "")
                {
                    var docLink = docClient.GetDocumentV2(applicationCode);

                    if (docLink != null)
                    {
                        ViewBag.DocLink = docLink.ToList();
                    }

                    if (docLink == null)
                    {
                        ViewBag.FirstDoc = null;
                    }
                    else
                    {
                        var FirstDoc = docLink;
                        ViewBag.FirstDoc = FirstDoc.FirstOrDefault().PathFullDoc;
                    }
                }
            }
            //return
            return View();
        }

        public ActionResult CallIndexCM(string queueID = "", string appCode = "", string p = "")
        {
            //declare variable application code
            var applicationCode = "";
            //get userId
            var user = GlobalFunction.GetLoginDetail(this.HttpContext);
            ViewBag.UserId = user.User_ID;
            //วันที่เกินกำหนด
            ViewBag.CMDueDate = Properties.Settings.Default.CMDueDate;
            //เช็คเวลาหรือไม่
            ViewBag.IsCheckTime = Properties.Settings.Default.IsCMCheckTime;

            //get role
            var userRole = new SSOService.SSOServiceClient().GetRoleByUserName(user.UserName);
            var lstUserRole = userRole.Split(',').ToList();
            var rolesDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var rolesDirector = new[] { "SmileUnderwriteBranch_Director" }; //ผอ พื้นที่
            var rolesCM = new[] { "SmileUnderwriteBranch_CM" }; //ประธาน
            var rolesUnderwrite = new[] { "SmileCall_Underwrite" }; //คัดกรอง
            var rolesAdvisor = new[] { "SmileUnderwriteBranch_Advisor" }; //ที่ปรึกษา

            ViewBag.IsCM = false; //ประธาน
            ViewBag.IsAdvisor = false; //ที่ปรึกษา
            ViewBag.IsUnderwrite = false; //คัดกรอง
            ViewBag.IsDirector = false; //ผอ
            ViewBag.IsOther = false; //บุคคลอื่นๆ

            //From Page
            switch (p)
            {
                case "ch": // Page Call History
                case "tl": // Page Task List
                    if (lstUserRole.Intersect(rolesDirector).Count() > 0)
                    {
                        ViewBag.IsDirector = true;
                    }
                    else
                    {
                        ViewBag.IsOther = true;
                    }
                    break;

                case "twtc": // Page Task Wait To Consider
                    if (lstUserRole.Intersect(rolesCM).Count() > 0)
                    {
                        ViewBag.IsCM = true;
                    }
                    else
                    {
                        ViewBag.IsOther = true;
                    }
                    break;

                default: // อื่นๆ
                    if (lstUserRole.Intersect(rolesAdvisor).Count() > 0)
                    {
                        ViewBag.IsAdvisor = true;
                    }
                    else if (lstUserRole.Intersect(rolesUnderwrite).Count() > 0)
                    {
                        ViewBag.IsUnderwrite = true;
                    }
                    else
                    {
                        ViewBag.IsOther = true;
                    }
                    break;
            }

            //check queueId
            if (queueID != "")
            {
                //decode
                var queueBase64EncodedBytes = Convert.FromBase64String(queueID);
                var QueueId = System.Text.Encoding.UTF8.GetString(queueBase64EncodedBytes);
                ViewBag.QueueId = QueueId;
                using (var db = new UnderwriteBranchEntities())
                {
                    //get PHqueueId by appcode
                    var lst = db.usp_PHQueueByQueueId_Select(Convert.ToInt32(QueueId)).First();
                    ViewBag.QueueDetail = lst;
                    //get callstatus
                    var lst_callstatus = db.usp_CallStatus_Select(null).ToList();
                    ViewBag.CallStatusList = lst_callstatus;

                    var lstType = db.usp_PHQueueType_Select().ToList();
                    ViewBag.QueueType = lstType;

                    var lstPayment = db.usp_UWPaymentType_Select(null).Where(o => o.UWPaymentTypeId != 1).ToList();
                    ViewBag.PaymentType = lstPayment;

                    //set application code
                    applicationCode = lst.ApplicationCode;

                    var ListCustomerMobileNo = SplitPhoneNumbers(lst.CustomerMobileNo);
                    var ListPayerTelephoneNo = SplitPhoneNumbers(lst.PayerTelephoneNo);
                    var ListPayerOfficeTelephoneNo = SplitPhoneNumbers(lst.PayerOfficeTelephoneNo);

                    ViewBag.ListCustomerMobileNo = ListCustomerMobileNo;
                    ViewBag.ListPayerTelephoneNo = ListPayerTelephoneNo;
                    ViewBag.ListPayerOfficeTelephoneNo = ListPayerOfficeTelephoneNo;

                    var queueResult = db.usp_PHQueueResult_Get(Convert.ToInt32(QueueId)).FirstOrDefault();

                    ViewBag.QueueResult = queueResult;
                }
            }
            //check appcode
            else if (appCode != "")
            {
                //decode
                var AppCodeBase64EncodedBytes = Convert.FromBase64String(appCode);
                var AppCode = System.Text.Encoding.UTF8.GetString(AppCodeBase64EncodedBytes);
                //set application code
                applicationCode = AppCode;
                using (var db = new UnderwriteBranchEntities())
                {
                    //get PHqueueId by appcode
                    var lst = db.usp_PHQueueByAppCode_Select(AppCode).First();
                    ViewBag.QueueDetail = lst;
                    //get callstatus
                    var lst_callstatus = db.usp_CallStatus_Select(null).ToList();
                    ViewBag.CallStatusList = lst_callstatus;

                    var lstType = db.usp_PHQueueType_Select().ToList();
                    ViewBag.QueueType = lstType;

                    var lstPayment = db.usp_UWPaymentType_Select(null).ToList();
                    ViewBag.PaymentType = lstPayment;

                    //set viewbag = queue id
                    ViewBag.QueueId = lst.PHQueueId;

                    var ListCustomerMobileNo = SplitPhoneNumbers(lst.CustomerMobileNo);
                    var ListPayerTelephoneNo = SplitPhoneNumbers(lst.PayerTelephoneNo);
                    var ListPayerOfficeTelephoneNo = SplitPhoneNumbers(lst.PayerOfficeTelephoneNo);

                    ViewBag.ListCustomerMobileNo = ListCustomerMobileNo;
                    ViewBag.ListPayerTelephoneNo = ListPayerTelephoneNo;
                    ViewBag.ListPayerOfficeTelephoneNo = ListPayerOfficeTelephoneNo;

                    var queueResult = db.usp_PHQueueResult_Get(Convert.ToInt32(lst.PHQueueId)).FirstOrDefault();
                    if (queueResult != null)
                    {
                        var person = db.usp_DataCenter_PersonUserByUserId_Select(queueResult.CMIApproveByUserId).FirstOrDefault();

                        ViewBag.Person = person;
                    }
                }
            }
            else
            {
                //set viewbag queue id = 0
                ViewBag.QueueId = 0;
            }

            //DOCUMENT
            var sssDocClient = new SSSDocService.DocumentServiceClient();
            var smileDocClient = new SmileDocService.DocumentServiceClient();

            var mergDoclink = new List<Models.Document>();

            if (applicationCode != "")
            {
                //get smile doc
                var linkSmileDoc = smileDocClient.GetDocumentV2SmileDoc(applicationCode);
                //get sss doc
                var linkSSSDoc = sssDocClient.GetDocumentV2(applicationCode);

                if (linkSmileDoc != null)
                {
                    for (int i = 0; i < linkSmileDoc.Count(); i++)
                    {
                        mergDoclink.Add(new Models.Document
                        {
                            DocumentId = linkSmileDoc[i].DocumentId,
                            DocumentFileId = linkSmileDoc[i].DocumentFileId,
                            RunningNo = linkSmileDoc[i].RunningNo,
                            DocumentTypeId = linkSmileDoc[i].DocumentTypeId,
                            DocumentTypeName = linkSmileDoc[i].DocumentTypeName,
                            PathThumbnailImg = linkSmileDoc[i].PathThumbnailImg,
                            PathFullDoc = linkSmileDoc[i].PathFullDoc
                        });
                    }
                }

                if (linkSSSDoc != null)
                {
                    for (int i = 0; i < linkSSSDoc.Count(); i++)
                    {
                        mergDoclink.Add(new Models.Document
                        {
                            DocumentId = linkSSSDoc[i].DocumentId,
                            DocumentFileId = linkSSSDoc[i].DocumentFileId,
                            RunningNo = linkSSSDoc[i].RunningNo,
                            DocumentTypeId = linkSSSDoc[i].DocumentTypeId,
                            DocumentTypeName = linkSSSDoc[i].DocumentTypeName,
                            PathThumbnailImg = linkSSSDoc[i].PathThumbnailImg,
                            PathFullDoc = linkSSSDoc[i].PathFullDoc
                        });
                    }
                }

                if (linkSmileDoc == null && linkSSSDoc == null)
                {
                    ViewBag.FirstDoc = null;
                }
                else
                {
                    ViewBag.DocLink = mergDoclink.ToList();
                    ViewBag.FirstDoc = mergDoclink.FirstOrDefault().PathFullDoc;
                }
            }

            smileDocClient.Close();
            sssDocClient.Close();

            return View();
        }

        /// <summary>
        /// HISTORY
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public ActionResult CallHistory(string areaId, string isPass)
        {
            var user = GlobalFunction.GetLoginDetail(this.HttpContext);
            var userRole = new SSOService.SSOServiceClient().GetRoleByUserName(user.UserName);
            var lstUserRole = userRole.Split(',').ToList();
            var rolesDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var rolesDirector = new[] { "SmileUnderwriteBranch_Director" }; //ผอ พื้นที่

            if (lstUserRole.Intersect(rolesDirector).Count() > 0 || lstUserRole.Intersect(rolesDev).Count() > 0)
            {
                var isPassBase64encodeBytes = Convert.FromBase64String(isPass);
                isPass = System.Text.Encoding.UTF8.GetString(isPassBase64encodeBytes);

                ViewBag.UserId = user.User_ID;
                ViewBag.AreaId = areaId;
                ViewBag.IsPass = isPass;
                return View();
            }

            return RedirectToAction("UnAuthorize", "Auth");
        }

        /// <summary>
        /// GET QUEUE DETAIL
        /// </summary>
        /// <param name="queueId"></param>
        /// <returns></returns>
        public ActionResult GetQueueDetail(string queueId)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                var lst = db.usp_PHQueueByQueueId_Select(Convert.ToInt32(queueId)).ToList();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// PH QUEUE DETAIL
        /// </summary>
        /// <param name="queueId"></param>
        /// <returns></returns>
        public ActionResult GetPHQueueDetailById(string queueId)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                var lst = db.usp_GetPHQueueDetailById(Convert.ToInt32(queueId)).ToList();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="pHQueueId"></param>
        /// <param name="pHQueueStatusId"></param>
        /// <param name="callStatusId"></param>
        /// <param name="isFoundCustomer"></param>
        /// <param name="queueTypeId"></param>
        /// <param name="isUnderwriteBranchResult"></param>
        /// <param name="isResultStatusId"></param>
        /// <param name="isBankAccountAllow"></param>
        /// <param name="bankAccountAllowRemark"></param>
        /// <param name="isBranchInform"></param>
        /// <param name="isCallCenterInform"></param>
        /// <param name="isConfirm"></param>
        /// <param name="isContactByPhone"></param>
        /// <param name="isContactByFaceToFace"></param>
        /// <param name="remark"></param>
        /// <param name="denyHealth"></param>
        /// <param name="denyOccupation"></param>
        /// <param name="denyCantPay"></param>
        /// <param name="denyOther"></param>
        /// <param name="denyRemark"></param>
        /// <param name="callToCustomer"></param>
        /// <param name="callToPayer"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueueResult_Update(int pHQueueId,
                                                int pHQueueStatusId,
                                                int callStatusId,
                                                bool? isFoundCustomer,
                                                int? queueTypeId,
                                                bool? isUnderwriteBranchResult,
                                                bool? isResultStatusId,
                                                bool? isBankAccountAllow,
                                                string bankAccountAllowRemark,
                                                bool? isBranchInform,
                                                bool? isCallCenterInform,
                                                bool? isConfirm,
                                                bool? isContactByPhone,
                                                bool? isContactByFaceToFace,
                                                string remark,
                                                bool? denyHealth,
                                                bool? denyOccupation,
                                                bool? denyCantPay,
                                                bool? denyOther,
                                                string denyRemark,
                                                bool? callToCustomer,
                                                bool? callToPayer)
        {
            if (pHQueueStatusId != 6)
            {
                //GET USER ID
                var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

                using (var db = new UnderwriteBranchEntities())
                {
                    //SET DATA TO STORE PROCEDURE
                    var lst = db.usp_PHQueueResult_Update(pHQueueId,
                                                         isBankAccountAllow,
                                                         bankAccountAllowRemark,
                                                         isBranchInform,
                                                         isCallCenterInform,
                                                         isConfirm,
                                                         isContactByPhone,
                                                         isContactByFaceToFace,
                                                         isResultStatusId,
                                                         denyHealth,
                                                         denyOccupation,
                                                         denyCantPay,
                                                         denyOther,
                                                         denyRemark,
                                                         remark).First();

                    //CASE STATUS ID
                    switch (callStatusId)
                    {
                        case 1:
                            if (isUnderwriteBranchResult.Value)//สถานะการคัดกรอง  ถ้า = true (คัดกรองแล้ว)
                            {
                                //check ph queue status = 4 เกินกำหนด
                                if (pHQueueStatusId == 4)
                                {
                                    //UPDATE QUEUE STATUS /* 5 คือ STATUS ID = คัดกรองแล้วเกินกำหนด
                                    db.usp_PHQueueStatusUpdateByQueueId_Update(pHQueueId, 5, userId);
                                }
                                else
                                {
                                    //UPDATE QUEUE STATUS /* 3 คือ STATUS ID = คัดกรองแล้ว
                                    db.usp_PHQueueStatusUpdateByQueueId_Update(pHQueueId, 3, userId);
                                }

                                //UPDATE UnderwriteBranchResult / = คัดกรองแล้ว
                                db.usp_PHQueueUnderwriteBranchResultByQueueId_Update(pHQueueId, isUnderwriteBranchResult);
                                //UPDATE QueueType //ช่องทางการคัดกรอง
                                db.usp_PHQueueQueueTypeByQueueId_Update(pHQueueId, queueTypeId);
                            }
                            else
                            {
                                //UPDATE QUEUE STATUS /* 2 คือ STATUS ID = ยังไม่คัดกรอง
                                db.usp_PHQueueStatusUpdateByQueueId_Update(pHQueueId, 2, userId);
                                //UPDATE UnderwriteBranchResult / = คัดกรองแล้ว
                                db.usp_PHQueueUnderwriteBranchResultByQueueId_Update(pHQueueId, isUnderwriteBranchResult);
                            }
                            break;

                        case 2:
                            //check ph queue status = 4 เกินกำหนด
                            if (pHQueueStatusId == 4)
                            {
                                //UPDATE QUEUE STATUS /* 5 คือ STATUS ID = คัดกรองแล้วเกินกำหนด
                                db.usp_PHQueueStatusUpdateByQueueId_Update(pHQueueId, 5, userId);
                            }
                            else
                            {
                                //UPDATE QUEUE STATUS /* 3 คือ STATUS ID = คัดกรองแล้ว
                                db.usp_PHQueueStatusUpdateByQueueId_Update(pHQueueId, 3, userId);
                            }

                            //UPDATE UnderwriteBranchResult / = คัดกรองแล้ว
                            db.usp_PHQueueUnderwriteBranchResultByQueueId_Update(pHQueueId, isUnderwriteBranchResult);
                            //UPDATE QueueType //ช่องทางการคัดกรอง
                            db.usp_PHQueueQueueTypeByQueueId_Update(pHQueueId, queueTypeId);
                            break;

                        case 3:
                        case 4:
                        case 5:
                            //UPDATE QUEUE STATUS /* 2 คือ STATUS ID = ยังไม่คัดกรอง
                            db.usp_PHQueueStatusUpdateByQueueId_Update(pHQueueId, 2, userId);
                            //UPDATE UnderwriteBranchResult / = คัดกรองแล้ว
                            db.usp_PHQueueUnderwriteBranchResultByQueueId_Update(pHQueueId, isUnderwriteBranchResult);
                            break;
                    }

                    //UPDATE QUEUE CALL STATUS ID
                    db.usp_PHQueueCallStatusUpdateByQueueId_Update(pHQueueId, callStatusId);

                    //UPDATE IS FOUND CUSTOMER
                    db.usp_PHQueueIsFoundCustomerByQueueId_Update(pHQueueId, isFoundCustomer);

                    //UPDATE CALL TO
                    db.usp_PHQueueCallToByQueueId_Update(pHQueueId, callToCustomer, callToPayer);

                    //singnal R
                    var chat = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                    chat.Clients.Group(userId.ToString()).ReceiveGroupToUpdateQueue("Success");

                    //RETURN LIST
                    return Json(lst, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //RETURN LIST
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult QueueResult_UpdateV2(FormCollection form)
        {
            //สถานะการคัดกรอง
            var underwriteStatus = form["underwriteStatus"];

            if (!string.IsNullOrEmpty(underwriteStatus))
            {
                var user = form["userId"];
                var queueId = form["queueId"];
                //GROUP สถานะผ่านคัดกรอง
                //ช่องทางการคัดกรอง
                var typeSelect = form["typeSelect"];
                //typeSelectChlid split to array list
                var typeSelectChlid = form["typeSelectChild"];
                //การชำระเบี้ย
                var uwPaymentType = form["uwPaymentType"];
                //การคัดกรองด้านสุขภาพ
                var health = form["health"];
                var healthChildPass = form["healthChildPass"];
                var IsSpecify = form["IsSpecify"];
                var healthChildPassOtherRemark = form["healthChildPassOtherRemark"];
                //healthChildNotpass split to array list
                var healthChildNotpass = form["healthChildNotPass"];
                var healthChildNotpassOtherRemark = form["healthChildNotPassOtherRemark"];
                //หมายเหตุ/บันทึกเพิ่มเติม
                var remark = form["remark"];

                //GROUP สถานะไม่ผ่านคัดกรอง
                //สถานการโทร
                var callStatus = form["callStatus"];
                //สถานะการเข้าพบ
                var foundCustomer = form["foundCustomer"];

                //DECLARE
                int? userId = Convert.ToInt32(user);
                int? pHQueueId = Convert.ToInt32(queueId);
                int? queueTypeId = null;
                int? pHQueueStatusId = null;
                bool? callToCustomer = null;
                bool? callToPayer = null;
                int? callStatusId = null;
                bool? isFoundCustomer = null;
                bool? underwriteBranchResult = null;
                bool? isBankAccountAllow = null;
                string bankAccountAllowRemark = null;
                bool? isBranchInform = null;
                bool? isCallCenterInform = null;
                bool? isConfirm = null;
                bool? isContactByPhone = null;
                bool? isContactByFaceToFace = null;
                bool? isResultPass = null;
                bool? denyHealth = null;
                bool? denyOccupation = null;
                bool? denyCantPay = null;
                bool? denyOther = null;
                string denyRemark = null;
                int? uWPaymentTypeId = null;
                bool? resultPassStandard = null;
                bool? resultPassCondition = null;
                bool? resultPassIsSpecify = null;
                string resultPassConditionRemark = null;
                string remarkAll = null;

                using (var db = new UnderwriteBranchEntities())
                {
                    switch (underwriteStatus)
                    {
                        // 1 สถานะคัดกรองแล้ว
                        case "1":
                            pHQueueStatusId = 7;
                            queueTypeId = Convert.ToInt32(typeSelect); //ช่องทางการคัดกรอง
                            uWPaymentTypeId = Convert.ToInt32(uwPaymentType); //การชำระเบี้ย
                            underwriteBranchResult = true; //สถานะคัดกรอง
                            isResultPass = (health == "1"); // การคัดกรองด้านสุขภาพ ผ่าน=true,ไม่ผ่าน=false
                            remarkAll = remark; //หมายเหตุ/บันทึกเพิ่มเติม

                            var arrayTypeSelectChild = typeSelectChlid.Split(','); //ตัวเลือกเพิ่มเติม ช่องทางการคัดกรอง (ผู้ชำระเบี้ย,ผู้เอาประกัน)

                            if (isResultPass.Value)
                            {
                                //ผ่าน
                                switch (healthChildPass)
                                {
                                    case "1":
                                        resultPassStandard = true; //ผ่าน ตามมาตรฐานทุกประการ
                                        break;

                                    case "0":
                                        resultPassCondition = true; //ผ่าน  มีประวัติสุขภาพที่ติดข้อยกเว้น
                                        resultPassConditionRemark = healthChildPassOtherRemark;

                                        if (!string.IsNullOrEmpty(IsSpecify))
                                        {
                                            switch (IsSpecify)
                                            {
                                                case "1": //ข้อยกเว้น ระบุในใบสมัคร
                                                    resultPassIsSpecify = true;
                                                    break;

                                                case "0"://ไม่ระบุ
                                                    resultPassIsSpecify = false;
                                                    break;

                                                default:
                                                    break;
                                            }
                                        }
                                        break;

                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                var arrayhealthChildNotpass = healthChildNotpass.Split(',');
                                //ไม่ผ่าน
                                foreach (var item in arrayhealthChildNotpass)
                                {
                                    switch (item)
                                    {
                                        case "denyHealth": //ด้านสุขภาพ
                                            denyHealth = true;
                                            break;

                                        case "denyOccupation": //ด้านอาชีพ
                                            denyOccupation = true;
                                            break;

                                        case "denyCantPay": //ไม่สามารถชำระเบี้ย
                                            denyCantPay = true;
                                            break;

                                        case "denyOther": //อื่นๆ
                                            denyOther = true;
                                            denyRemark = healthChildNotpassOtherRemark;
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }

                            //type 2 = โทรเยี่ยมลูกค้า
                            if (Convert.ToInt32(typeSelect) == 2)
                            {
                                callStatusId = 2; //สถานะ รับสาย
                                isFoundCustomer = null; //โทรเยี่ยมลูกค้า ให้ isFoundCustomer = null

                                foreach (var item in arrayTypeSelectChild)
                                {
                                    switch (item)
                                    {
                                        case "customer":
                                            callToCustomer = true;
                                            break;

                                        case "payer":
                                            callToPayer = true;
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            //type 3 = เข้าพบลูกค้า
                            else if (Convert.ToInt32(typeSelect) == 3)
                            {
                                callStatusId = 1; //สถานะ รอข้อมูล
                                isFoundCustomer = true; //เข้าพบลูกค้า ให้ isFoundCustomer = true

                                foreach (var item in arrayTypeSelectChild)
                                {
                                    switch (item)
                                    {
                                        case "customer":
                                            callToCustomer = true;
                                            break;

                                        case "payer":
                                            callToPayer = true;
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            break;
                        // 0 ยังไม่คัดกรอง
                        case "0":
                            pHQueueStatusId = 2;
                            callStatusId = Convert.ToInt32(callStatus); //สถานะการโทร
                            underwriteBranchResult = false; //สถานะคัดกรอง
                            isFoundCustomer = (foundCustomer == "1");
                            break;

                        default:
                            break;
                    }

                    var result = db.usp_PHQueueResultV2_Update(pHQueueId,
                                                               queueTypeId,
                                                               pHQueueStatusId,
                                                               userId,
                                                               callToCustomer,
                                                               callToPayer,
                                                               callStatusId,
                                                               isFoundCustomer,
                                                               underwriteBranchResult,
                                                               isBankAccountAllow,
                                                               bankAccountAllowRemark,
                                                               isBranchInform,
                                                               isCallCenterInform,
                                                               isConfirm,
                                                               isContactByPhone,
                                                               isContactByFaceToFace,
                                                               isResultPass,
                                                               denyHealth,
                                                               denyOccupation,
                                                               denyCantPay,
                                                               denyOther,
                                                               denyRemark,
                                                               remark,
                                                               uWPaymentTypeId,
                                                               resultPassStandard,
                                                               resultPassCondition,
                                                               resultPassConditionRemark,
                                                               resultPassIsSpecify).FirstOrDefault();

                    //singnal R
                    var chat = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                    chat.Clients.Group(userId.ToString()).ReceiveGroupToUpdateQueue("Success");
                    //RETURN LIST
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //RETURN LIST
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Consert Insert ขอคำปรึกษา
        /// </summary>
        /// <param name="PHQueueId"></param>
        /// <param name="PHQueueConsultDetail"></param>
        /// <param name="RequesterTelephone"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConsultInsert(int PHQueueId, bool isAdvisor, string PHQueueConsultDetail, string RequesterTelephone)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                var user = GlobalFunction.GetLoginDetail(this.HttpContext);
                var result = db.usp_PHQueueConsult_Insert(PHQueueId, PHQueueConsultDetail, RequesterTelephone, isAdvisor, user.User_ID).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get Consult
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetConsultByPHQueueId(int PHQueueId, int? draw, string search, int? pageStart, int? pageSize, string sortField, string orderType)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                var lst = db.usp_PHQueueConsultByPHQueueId_Select(PHQueueId, search, pageStart, pageSize, sortField, orderType).ToList();

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

        /// <summary>
        /// อัพเดต ผลการพิจารณาประธานสาขา
        /// </summary>
        /// <param name="PHQueueId"></param>
        /// <param name="CMIsApprove"></param>
        /// <param name="CMRemark"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CMApprove(int PHQueueId,
                                      bool CMIsApprove,
                                      string CMRemark,
                                      bool? CMIsApprovePassCondition,
                                      bool? CMResultPassCondition,
                                      bool? CMResultPassIsSpecify,
                                      bool? CMDenyHealth,
                                      bool? CMDenyOccupation,
                                      bool? CMDenyCantPay,
                                      bool? CMDenyOther,
                                      string CMDenyRemark,
                                      string CMResultPassConditionRemark)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                var user = GlobalFunction.GetLoginDetail(this.HttpContext);

                //INSERT LOG
                db.usp_PHQueueCMConsiderLog_Insert(PHQueueId,
                                                    CMIsApprove,
                                                    CMIsApprovePassCondition,
                                                    user.User_ID,
                                                    CMRemark);

                //UPDATE LOG
                var resultUpdated1 = db.usp_CMApproveByPHQueueId_Update(PHQueueId,
                                                                            CMIsApprove,
                                                                            CMIsApprovePassCondition,
                                                                            user.User_ID,
                                                                            CMRemark).FirstOrDefault();

                var nCMDenyRemark = (CMDenyOther == true) ? CMDenyRemark : null;
                //UPDATE
                var resultUpdated2 = db.usp_CMPHQueueResult_Update(PHQueueId,
                                                                    CMResultPassCondition,
                                                                    CMResultPassIsSpecify,
                                                                    CMResultPassConditionRemark,
                                                                    CMDenyHealth,
                                                                    CMDenyOccupation,
                                                                    CMDenyCantPay,
                                                                    CMDenyOther,
                                                                    nCMDenyRemark,
                                                                    user.User_ID).FirstOrDefault();
                //Signal R
                var chat = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                chat.Clients.Group(user.User_ID.ToString()).ReceiveGroupToUpdateQueue("Success");

                //RETURN
                return Json(resultUpdated1, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///GET RESULT
        /// </summary>
        /// <param name="queueId"></param>
        /// <returns></returns>
        public ActionResult QueueResult_Get(int? queueId)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                //CALL USP
                var lst = db.usp_PHQueueResult_Get(queueId).FirstOrDefault();

                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get_PayerOtherApp
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        public ActionResult Get_PayerOtherApp(string appCode)
        {
            using (var client = new UnderwriteBranchServiceClient())
            {
                //GET DATA FROM SERVICE
                var result = client.Get_PayerOtherAppByAppCode(appCode).ToList();

                //RETURN JSON
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GET CALL STATUS
        /// </summary>
        /// <param name="callStatusId"></param>
        /// <returns></returns>
        public ActionResult Get_CallStatus(int? callStatusId = null)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                //CALL USP
                var lst = db.usp_CallStatus_Select(callStatusId).ToList();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Split PhoneNumbers
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private List<string> SplitPhoneNumbers(string str)
        {
            //Separator
            string[] separators = { ".", ",", "/", ":", ";", "\\" };

            var substrings = str.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            var list = new List<string>();
            if (substrings.Length > 0)
            {
                foreach (var item in substrings)
                {
                    var trimItem = item.Trim();
                    list.Add(trimItem.Replace("-", ""));
                }
            }

            return list;
        }
    }
}