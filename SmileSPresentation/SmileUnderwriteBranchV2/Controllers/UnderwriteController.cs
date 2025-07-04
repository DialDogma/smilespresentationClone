using ImageResizer;
using Microsoft.AspNet.SignalR;
using SmileUnderwriteBranchV2.Helper;
using SmileUnderwriteBranchV2.Models;
using SmileUnderwriteBranchV2.UnderwriteBranchService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SmileUnderwriteBranchV2.Controllers
{
    [Authorization]
    public class UnderwriteController : Controller
    {
        #region Context

        private readonly UnderwriteBranchV2Entities _context;

        public UnderwriteController()
        {
            _context = new UnderwriteBranchV2Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        #region view

        // GET: DashboardCM x
        /*[Authorization(Roles = "Developer,UnderwriteV2_Chairman")]*/

        public ActionResult DashboardCM()
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            //DCR
            var periodList = GlobalFunction.GetPeriodList();
            ViewBag.PeriodList = periodList;

            var branchList = _context.usp_BranchByChairmanUserId_Select(user.User_ID).ToList();
            ViewBag.BranchList = branchList;

            var currentBranch = -1;
            if (branchList.Count > 0) currentBranch = branchList[0].Branch_ID;

            ViewBag.CurrentBranchID = currentBranch;

            var currentDCR = Convert.ToDateTime(periodList[0].Value);
            ViewBag.CurrentDCR = currentDCR;

            return View();
        }

        // GET: Dashboard Business Promote Team
        [Authorization(Roles = "Developer,UnderwriteV2_BusinesPromoteTeam")]
        public ActionResult DashboardBPT()
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);

            //Get Role
            var roleListRaw = OAuth2Helper.GetRoles();
            var roleList = roleListRaw.Split(',');
            var accessRole = GlobalFunction.GetAccessRole(roleList);
            ViewBag.AccessRole = accessRole;

            int? userId = user.User_ID;
            if (roleList.Contains("Developer"))
            {
                //Dev ให้ส่ง null เพื่อดู --สาขาทั้งหมด--
                userId = null;
            }

            //DCR
            var periodList = GlobalFunction.GetPeriodList();
            ViewBag.PeriodList = periodList;

            var branchList = _context.usp_BranchByBusinessPromoteTeamUserId_Select(userId).ToList();
            ViewBag.BranchList = branchList;

            var currentDCR = Convert.ToDateTime(periodList[0].Value);
            ViewBag.CurrentDCR = currentDCR;

            return View();
        }

        [Authorization(Roles = "Developer,UnderwriteV2_BusinesPromoteTeam")]
        public ActionResult UnderwriteDashboard()
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);

            //Get Role
            var roleListRaw = OAuth2Helper.GetRoles();
            var roleList = roleListRaw.Split(',');
            var accessRole = GlobalFunction.GetAccessRole(roleList);
            ViewBag.AccessRole = accessRole;

            int? userId = user.User_ID;
            /*            if (roleList.Contains("Developer,UnderwriteV2_BusinesPromoteTeam"))
                        {
                            //Dev ให้ส่ง null เพื่อดู --สาขาทั้งหมด--
                            userId = null;
                        }*/

            //DCR
            var changePeriodDay = Properties.Settings.Default.ChangePeriodDay;
            var show = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.GetPeriodList(changePeriodDay, show);
            ViewBag.PeriodList = periodList;

            var branchList = _context.usp_BranchByBusinessPromoteTeamUserId_Select(null).ToList();
            ViewBag.BranchList = branchList;

            var currentDCR = Convert.ToDateTime(periodList[0].Value);
            ViewBag.CurrentDCR = currentDCR;

            return View();
        }

        // GET: Underwrite Detail
        public ActionResult UnderwriteDetail(string queueId = "", string appCode = "")
        {
            int? QueueId = null;
            string AppCode = null;

            try
            {
                if (queueId != null && queueId != "")
                {
                    byte[] dataQueue = Convert.FromBase64String(queueId);
                    var decodedQueueIdString = Encoding.UTF8.GetString(dataQueue);
                    QueueId = Convert.ToInt32(decodedQueueIdString);
                }
                else if (appCode != null && appCode != "")
                {
                    byte[] dataCode = Convert.FromBase64String(appCode);
                    var decodedAppCodeString = Encoding.UTF8.GetString(dataCode);
                    AppCode = decodedAppCodeString;
                }
                else
                {
                    return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "InnerException:" + e.InnerException + ", Message:" + e.Message });
            }

            try
            {
                //Get Role
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var accessRole = GlobalFunction.GetAccessRole(roleList);
                ViewBag.AccessRole = accessRole;
                var periodList = GlobalFunction.GetPeriodList();
                ViewBag.PeriodList = periodList;
                var branchList = _context.usp_Branch_Select(null).ToList();
                ViewBag.branchList = branchList;

                var result = _context.usp_QueueFullDetailByQueueId_Select(QueueId, AppCode).FirstOrDefault();

                if (result.IsLineOA == null)
                {
                    if (queueId != null && queueId != "")
                    {
                        return RedirectToAction("UnderwriteDetailVer2", new { queueId = queueId });
                    }
                    else if (appCode != null && appCode != "")
                    {
                        return RedirectToAction("UnderwriteDetailVer2", new { appCode = appCode });
                    }
                    else
                    {
                        return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
                    }

                }

                ViewBag.QueueFullDetail = result;
                ViewBag.QueueId = result.QueueId;
                ViewBag.AppCode = result.ApplicationCode;
                var applicationCode = result.ApplicationCode;
                var result2 = _context.usp_UnderwriteType_Select(null).ToList();
                ViewBag.UnderwriteType = result2;

                var result3 = _context.usp_UnderwritePaymentType_Select(null).ToList();
                ViewBag.UnderwritePaymentType = result3;

                var result4 = _context.usp_CallStatus_Select(null).ToList();
                ViewBag.CallStatusList = result4;

                var OpenSSS = Properties.Settings.Default.SSSpath;
                ViewBag.OpenSSS = OpenSSS;

                //check date for edit
                var CMDueDate = Properties.Settings.Default.CMDueDate;
                ViewBag.IsEdit = false;

                var now = DateTime.Now.Date;
                var AppDCR = result.AppStartCoverDate.Value;
                var CurrentMonth = new DateTime(now.Year, now.Month, 1);

                //--test--//

                //เช็ค (ปีของ app เท่าปีปัจจุบัน AND เดือนDCR app เท่ากับ เดือนปัจจุบัน AND วันที่ปัจจุบันไม่เกินวันที่กำหนดไว้ ให้สามารถแก้ไขผลได้) OR (ถ้าปี มากกว่าหรือเท่ากับปัจจุบัน  AND เป็น app dcr ล่วงหน้า ก็ให้แก้ไขผลได้เช่นกัน)
                if ((AppDCR == CurrentMonth && now.Day <= CMDueDate) || (AppDCR > CurrentMonth))
                {
                    ViewBag.IsEdit = true;
                }

                //DOCUMENT
                var DocumentClient = new SSSDocService.DocumentServiceClient();

                var mergDoclink = new List<Document>();

                if (applicationCode != "")
                {
                    //get smile doc
                    var linkSmileDoc = DocumentClient.GetDocumentV2SmileDoc(applicationCode);
                    //get sss doc
                    var linkSSSDoc = DocumentClient.GetDocumentV2(applicationCode);

                    if (linkSmileDoc != null)
                    {
                        for (int i = 0; i < linkSmileDoc.Count(); i++)
                        {
                            mergDoclink.Add(new Document
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
                            mergDoclink.Add(new Document
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

                DocumentClient.Close();

                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "InnerException:" + e.InnerException + ", Message:" + e.Message });
            }
        }

        // GET: Underwrite Detail coppy
        public ActionResult UnderwriteDetailVer2(string queueId = "", string appCode = "")
        {
            int? QueueId = null;
            string AppCode = null;

            try
            {
                if (queueId != null && queueId != "")
                {
                    byte[] dataQueue = Convert.FromBase64String(queueId);
                    var decodedQueueIdString = Encoding.UTF8.GetString(dataQueue);
                    QueueId = Convert.ToInt32(decodedQueueIdString);
                }
                else if (appCode != null && appCode != "")
                {
                    byte[] dataCode = Convert.FromBase64String(appCode);
                    var decodedAppCodeString = Encoding.UTF8.GetString(dataCode);
                    AppCode = decodedAppCodeString;
                }
                else
                {
                    return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "InnerException:" + e.InnerException + ", Message:" + e.Message });
            }

            try
            {
                //Get Role
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var accessRole = GlobalFunction.GetAccessRole(roleList);
                ViewBag.AccessRole = accessRole;
                var periodList = GlobalFunction.GetPeriodList();
                ViewBag.PeriodList = periodList;
                var branchList = _context.usp_Branch_Select(null).ToList();
                ViewBag.branchList = branchList;

                var result = _context.usp_QueueFullDetailByQueueId_Select(QueueId, AppCode).FirstOrDefault();

                ViewBag.QueueFullDetail = result;
                ViewBag.QueueId = result.QueueId;
                ViewBag.AppCode = result.ApplicationCode;
                var applicationCode = result.ApplicationCode;
                var result2 = _context.usp_UnderwriteType_Select(null).ToList();
                ViewBag.UnderwriteType = result2;

                var result3 = _context.usp_UnderwritePaymentType_Select(null).ToList();
                ViewBag.UnderwritePaymentType = result3;

                var result4 = _context.usp_CallStatus_Select(null).ToList();
                ViewBag.CallStatusList = result4;

                var OpenSSS = Properties.Settings.Default.SSSpath;
                ViewBag.OpenSSS = OpenSSS;

                //check date for edit
                var CMDueDate = Properties.Settings.Default.CMDueDate;
                ViewBag.IsEdit = false;

                var now = DateTime.Now.Date;
                var AppDCR = result.AppStartCoverDate.Value;
                var CurrentMonth = new DateTime(now.Year, now.Month, 1);

                //--test--//

                //เช็ค (ปีของ app เท่าปีปัจจุบัน AND เดือนDCR app เท่ากับ เดือนปัจจุบัน AND วันที่ปัจจุบันไม่เกินวันที่กำหนดไว้ ให้สามารถแก้ไขผลได้) OR (ถ้าปี มากกว่าหรือเท่ากับปัจจุบัน  AND เป็น app dcr ล่วงหน้า ก็ให้แก้ไขผลได้เช่นกัน)
                if ((AppDCR == CurrentMonth && now.Day <= CMDueDate) || (AppDCR > CurrentMonth))
                {
                    ViewBag.IsEdit = true;
                }

                //DOCUMENT
                var DocumentClient = new SSSDocService.DocumentServiceClient();

                var mergDoclink = new List<Document>();

                if (applicationCode != "")
                {
                    //get smile doc
                    var linkSmileDoc = DocumentClient.GetDocumentV2SmileDoc(applicationCode);
                    //get sss doc
                    var linkSSSDoc = DocumentClient.GetDocumentV2(applicationCode);

                    if (linkSmileDoc != null)
                    {
                        for (int i = 0; i < linkSmileDoc.Count(); i++)
                        {
                            mergDoclink.Add(new Document
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
                            mergDoclink.Add(new Document
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

                DocumentClient.Close();

                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "InnerException:" + e.InnerException + ", Message:" + e.Message });
            }
        }

        // GET: Underwrite
        [Authorization(Roles = "Developer,UnderwriteV2_Director,UnderwriteV2_BusinesPromoteTeam")]
        public ActionResult UnderwriteIndex(string queueId, int? checkId)
        {
            // checkId == 8 กดมาจากหน้าเกินกำหนด
            var encodeId = queueId;
            if (encodeId != null && encodeId != string.Empty)
            {
                byte[] data = Convert.FromBase64String(encodeId);
                string decodedString = Encoding.UTF8.GetString(data);

                var result = _context.usp_QueueFullDetailByQueueId_Select(Convert.ToInt32(decodedString), null).FirstOrDefault();
                ViewBag.AppCode = result.ApplicationCode;
                //เช็คว่าต้องเป็นสถานะคิว ยังไม่ได้คัดกรอง  || ยังไม่ได้ดำเนินการ
                if (result.QueueStatusId == 2 || result.QueueStatusId == 8)
                {
                    ViewBag.checkId = checkId;

                    ViewBag.QueueFullDetail = result;
                    var applicationCode = result.ApplicationCode;

                    var result2 = _context.usp_UnderwriteType_Select(null).ToList();
                    ViewBag.UnderwriteType = result2;

                    var result3 = _context.usp_UnderwritePaymentType_Select(null).ToList();
                    ViewBag.UnderwritePaymentType = result3;

                    var result4 = _context.usp_CallStatus_Select(null).ToList();
                    ViewBag.CallStatusList = result4;

                    var OpenSSS = Properties.Settings.Default.SSSpath;
                    ViewBag.OpenSSS = OpenSSS;

                    var giveType = _context.usp_GiveType_Select(null).ToList();
                    ViewBag.giveType = giveType;


                    //DOCUMENT
                    var DocumentClient = new SSSDocService.DocumentServiceClient();

                    var mergDoclink = new List<Document>();

                    if (applicationCode != "")
                    {
                        //get smile doc
                        var linkSmileDoc = DocumentClient.GetDocumentV2SmileDoc(applicationCode);
                        //get sss doc
                        var linkSSSDoc = DocumentClient.GetDocumentV2(applicationCode);

                        if (linkSmileDoc != null)
                        {
                            for (int i = 0; i < linkSmileDoc.Count(); i++)
                            {
                                mergDoclink.Add(new Document
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
                                mergDoclink.Add(new Document
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

                    DocumentClient.Close();

                    return View();
                }
                else
                {
                    return RedirectToAction("UnderwriteDetail", new { queueId = encodeId });
                }
            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
            }
        }

        // GET: Underwrite
        [Authorization(Roles = "Developer,UnderwriteV2_Director,UnderwriteV2_BusinesPromoteTeam")]
        public ActionResult UnderwriteEdit(string queueId)
        {
            var encodeId = queueId;
            if (encodeId != null && encodeId != string.Empty)
            {
                byte[] data = Convert.FromBase64String(encodeId);
                string decodedString = Encoding.UTF8.GetString(data);

                var PHCloseDate = Properties.Settings.Default.PHCloseDate;
                ViewBag.PHCloseDate = PHCloseDate;


                var result = _context.usp_QueueFullDetailByQueueId_Select(Convert.ToInt32(decodedString), null).FirstOrDefault();
                ViewBag.AppCode = result.ApplicationCode;
                //เช็คว่าต้องเป็นสถานะคิว ยังไม่ได้คัดกรอง
                /* if (result.QueueStatusId == 2 || result.QueueStatusId == 8)
                 {*/
                ViewBag.QueueFullDetail = result;
                var applicationCode = result.ApplicationCode;

                var result2 = _context.usp_UnderwriteType_Select(null).ToList();
                ViewBag.UnderwriteType = result2;

                var result3 = _context.usp_UnderwritePaymentType_Select(null).ToList();
                ViewBag.UnderwritePaymentType = result3;

                var result4 = _context.usp_CallStatus_Select(null).ToList();
                ViewBag.CallStatusList = result4;

                var OpenSSS = Properties.Settings.Default.SSSpath;
                ViewBag.OpenSSS = OpenSSS;

                var giveType = _context.usp_GiveType_Select(null).ToList();
                ViewBag.giveType = giveType;

                //DOCUMENT
                var DocumentClient = new SSSDocService.DocumentServiceClient();

                var mergDoclink = new List<Document>();

                if (applicationCode != "")
                {
                    //get smile doc
                    var linkSmileDoc = DocumentClient.GetDocumentV2SmileDoc(applicationCode);
                    //get sss doc
                    var linkSSSDoc = DocumentClient.GetDocumentV2(applicationCode);

                    if (linkSmileDoc != null)
                    {
                        for (int i = 0; i < linkSmileDoc.Count(); i++)
                        {
                            mergDoclink.Add(new Document
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
                            mergDoclink.Add(new Document
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

                DocumentClient.Close();

                return View();
                /* }
                 else
                 {
                     return RedirectToAction("UnderwriteDetail", new { queueId = encodeId });
                 }*/
            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
            }
        }

        // GET: Underwrite
        [Authorization(Roles = "Developer,UnderwriteV2_Director,UnderwriteV2_BusinesPromoteTeam")]

        public ActionResult UnderwriteIndexForUnderwriteOverdue(string queueId)
        {
            var encodeId = queueId;
            if (encodeId != null && encodeId != string.Empty)
            {
                byte[] data = Convert.FromBase64String(encodeId);
                string decodedString = Encoding.UTF8.GetString(data);

                var result = _context.usp_QueueFullDetailByQueueId_Select(Convert.ToInt32(decodedString), null).FirstOrDefault();
                ViewBag.AppCode = result.ApplicationCode;
                //เช็คว่าต้องเป็นสถานะคิว ยังไม่ได้คัดกรอง
                ViewBag.QueueFullDetail = result;
                var applicationCode = result.ApplicationCode;

                var result2 = _context.usp_UnderwriteType_Select(null).ToList();
                ViewBag.UnderwriteType = result2;

                var result3 = _context.usp_UnderwritePaymentType_Select(null).ToList();
                ViewBag.UnderwritePaymentType = result3;

                var result4 = _context.usp_CallStatus_Select(null).ToList();
                ViewBag.CallStatusList = result4;

                var OpenSSS = Properties.Settings.Default.SSSpath;
                ViewBag.OpenSSS = OpenSSS;

                //DOCUMENT
                var DocumentClient = new SSSDocService.DocumentServiceClient();

                var mergDoclink = new List<Document>();

                if (applicationCode != "")
                {
                    //get smile doc
                    var linkSmileDoc = DocumentClient.GetDocumentV2SmileDoc(applicationCode);
                    //get sss doc
                    var linkSSSDoc = DocumentClient.GetDocumentV2(applicationCode);

                    if (linkSmileDoc != null)
                    {
                        for (int i = 0; i < linkSmileDoc.Count(); i++)
                        {
                            mergDoclink.Add(new Document
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
                            mergDoclink.Add(new Document
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

                DocumentClient.Close();

                return View();

            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
            }
        }

        #endregion view

        #region api

        /// <summary>
        /// Get  Queue full Detail
        /// </summary>
        /// <param name="queueId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult QueueFullDetail(int queueId)
        {
            var result = _context.usp_QueueFullDetailByQueueId_Select(queueId, null).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Log
        /// </summary>
        /// <param name="queueId"></param>
        /// <param name="draw"></param>
        /// <param name="indexStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult QueueLog(int queueId, int draw, int? indexStart, int? pageSize, string sortField, string orderType, string search)
        {
            var result = _context.usp_QueueLogByQueueId_Select(queueId, indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                 {"draw", draw },
                 {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                 {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                 {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Question Update
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QuestionUpdate(FormCollection form)
        {
            //สถานะการคัดกรอง
            var underwriteStatus = form["underwriteStatus"];

            if (!string.IsNullOrEmpty(underwriteStatus))
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var queueId = form["hiddenQueueId"];
                //GROUP สถานะผ่านคัดกรอง
                //ช่องทางการคัดกรอง
                var typeSelect = form["typeSelect"];
                //typeSelectChlid split to array list
                var typeSelectChlid = form["typeSelectChild"];
                var txtCause = form["txtCause"];
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
                // var isUnderwritePassIsSpecifyNotComplete = form["isUnderwritePassIsSpecifyNotComplete"];
                //หมายเหตุ/บันทึกเพิ่มเติม
                var remark = form["remark"];

                //GROUP สถานะไม่ผ่านคัดกรอง
                //สถานการโทร
                var callStatus = form["callStatus"];
                //สถานะการเข้าพบ
                var foundCustomer = form["foundCustomer"];

                //DECLARE
                int? userId = Convert.ToInt32(user);
                bool? isUnderwrite = null;
                int? underwriteTypeId = null;
                int? queueStatusId = null;
                int? callStatusId = null;
                string callCauseRemark = null;
                bool? isNotFoundCustomer = null;
                bool? isUnderwriteCust = null;
                bool? isUnderwritePayer = null;
                int? underwritePaymentTypeId = null;
                bool? isUnderwritePass = null;
                bool? isUnderwritePassStandard = null;
                bool? isUnderwritePassCondition = null;
                bool? isUnderwritePassIsSpecify = null;
                bool? isUnderwritePassIsSpecifyNotComplete = null;
                string underwritePassSpecifyRemark = null;
                bool? isUnderwriteUnPassHealth = null;
                bool? isUnderwriteUnPassOccupation = null;
                bool? isUnderwriteUnPassCantPay = null;
                bool? isUnderwriteUnPassCantContact = null;
                bool? isUnderwriteUnPassOther = null;
                string underwriteUnPassOtherRemark = null;
                string remarkAll = null;

                using (var db = new UnderwriteBranchV2Entities())
                {
                    switch (underwriteStatus)
                    {
                        // 1 สถานะคัดกรองแล้ว
                        case "1":
                            isUnderwrite = true;
                            underwriteTypeId = Convert.ToInt32(typeSelect); //ช่องทางการคัดกรอง
                            underwritePaymentTypeId = Convert.ToInt32(uwPaymentType); //การชำระเบี้ย
                            isUnderwritePass = (health == "1"); // การคัดกรองด้านสุขภาพ ผ่าน=true,ไม่ผ่าน=false
                            remarkAll = remark; //หมายเหตุ/บันทึกเพิ่มเติม

                            var arrayTypeSelectChild = typeSelectChlid.Split(','); //ตัวเลือกเพิ่มเติม ช่องทางการคัดกรอง (ผู้ชำระเบี้ย,ผู้เอาประกัน)

                            if (isUnderwritePass.Value)
                            {
                                switch (Convert.ToInt32(typeSelect))
                                {
                                    //โทร - ผ่าน
                                    case 3:
                                        queueStatusId = 3; //โทรคัดกรอง มอบบัตรภายหลัง
                                        break;
                                    //เข้าพบ - ผ่าน
                                    case 2:
                                        queueStatusId = 5; //เข้าพบ มอบบัตรแล้ว
                                        break;

                                    default:
                                        break;
                                }

                                //ผ่าน
                                switch (healthChildPass)
                                {
                                    case "1":
                                        isUnderwritePassStandard = true; //ผ่าน ตามมาตรฐานทุกประการ
                                        break;

                                    case "0":
                                        isUnderwritePassCondition = true; //ผ่าน  มีประวัติสุขภาพที่ติดข้อยกเว้น
                                        underwritePassSpecifyRemark = healthChildPassOtherRemark;

                                        if (!string.IsNullOrEmpty(IsSpecify))
                                        {
                                            switch (IsSpecify)
                                            {
                                                case "1": //ระบุในใบสมัครครบถ้วน
                                                    isUnderwritePassIsSpecify = true;
                                                    isUnderwritePassIsSpecifyNotComplete = null;
                                                    break;

                                                case "0"://ไม่ระบุ
                                                    isUnderwritePassIsSpecify = false;
                                                    isUnderwritePassIsSpecifyNotComplete = null;
                                                    break;
                                                case "2"://ระบุในใบสมัครไม่ครบ
                                                    isUnderwritePassIsSpecify = true;
                                                    isUnderwritePassIsSpecifyNotComplete = true;
                                                    break;

                                                default:
                                                    break;
                                            }
                                        }
                                        break;

                                    default:
                                        return Json(new { IsResult = false, Msg = "การคัดกรองด้านสุขภาพ = null" }, JsonRequestBehavior.AllowGet);
                                }

                                //check not null
                                if (isUnderwritePassStandard == null && isUnderwritePassCondition == null)
                                {
                                    return Json(new { IsResult = false, Msg = "isUnderwritePassStandard = null && isUnderwritePassCondition = null" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                queueStatusId = 6; //ไม่ผ่านดัดกรอง
                                var arrayhealthChildNotpass = healthChildNotpass.Split(',');
                                //ไม่ผ่าน
                                foreach (var item in arrayhealthChildNotpass)
                                {
                                    switch (item)
                                    {
                                        case "denyHealth": //ด้านสุขภาพ
                                            isUnderwriteUnPassHealth = true;
                                            break;

                                        case "denyOccupation": //ด้านอาชีพ
                                            isUnderwriteUnPassOccupation = true;
                                            break;

                                        case "denyCantPay": //ไม่สามารถชำระเบี้ย
                                            isUnderwriteUnPassCantPay = true;
                                            break;

                                        case "denyCantContact": //ไม่สามารถติดต่อ
                                            isUnderwriteUnPassCantContact = true;
                                            break;

                                        case "denyOther": //อื่นๆ
                                            isUnderwriteUnPassOther = true;
                                            underwriteUnPassOtherRemark = healthChildNotpassOtherRemark;
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }

                            //type 3 = โทรคัดกรอง
                            if (Convert.ToInt32(typeSelect) == 3)
                            {
                                callStatusId = 2; //สถานะ รับสาย
                                isNotFoundCustomer = null; //โทรเยี่ยมลูกค้า ให้ isNotFoundCustomer = null
                                callCauseRemark = txtCause;
                                foreach (var item in arrayTypeSelectChild)
                                {
                                    switch (item)
                                    {
                                        case "customer":
                                            isUnderwriteCust = true;
                                            break;

                                        case "payer":
                                            isUnderwritePayer = true;
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            //type 2= เข้าพบลูกค้า
                            else if (Convert.ToInt32(typeSelect) == 2)
                            {
                                callStatusId = 1; //สถานะ รอข้อมูล
                                isNotFoundCustomer = false; //เข้าพบลูกค้า ให้ isNotFoundCustomer = false

                                foreach (var item in arrayTypeSelectChild)
                                {
                                    switch (item)
                                    {
                                        case "customer":
                                            isUnderwriteCust = true;
                                            break;

                                        case "payer":
                                            isUnderwritePayer = true;
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            break;
                        // 0 ยังไม่คัดกรอง
                        case "0":
                            isUnderwrite = false;
                            queueStatusId = 2; //ยังไม่ได้คัดกรอง
                            callStatusId = Convert.ToInt32(callStatus); //สถานะการโทร
                            isNotFoundCustomer = (foundCustomer == "0");
                            break;

                        default:
                            break;
                    }
                    //var result = 1;
                    /* var result = db.usp_QueueResultByQueueId_Insert(Convert.ToInt32(queueId),
                                                                     queueStatusId,
                                                                     isUnderwrite,
                                                                     isNotFoundCustomer,
                                                                     underwriteTypeId,
                                                                     callStatusId,
                                                                     callCauseRemark,
                                                                     isUnderwriteCust,
                                                                     isUnderwritePayer,
                                                                     underwritePaymentTypeId,
                                                                     isUnderwritePass,
                                                                     isUnderwritePassStandard,
                                                                     isUnderwritePassCondition,
                                                                     isUnderwritePassIsSpecify,
                                                                     underwritePassSpecifyRemark,
                                                                     isUnderwriteUnPassHealth,
                                                                     isUnderwriteUnPassOccupation,
                                                                     isUnderwriteUnPassCantPay,
                                                                     isUnderwriteUnPassCantContact,
                                                                     isUnderwriteUnPassOther,
                                                                     underwriteUnPassOtherRemark,
                                                                     remarkAll,
                                                                     userId).FirstOrDefault();*/

                    var result = db.usp_QueueResultByQueueId_Insert(Convert.ToInt32(queueId),
                        queueStatusId,
                        isUnderwrite,
                        isNotFoundCustomer,
                        underwriteTypeId,
                        callStatusId,
                        callCauseRemark,
                        isUnderwriteCust,
                        isUnderwritePayer,
                        underwritePaymentTypeId,
                        isUnderwritePass,
                        isUnderwritePassStandard,
                        isUnderwritePassCondition,
                        isUnderwritePassIsSpecify,
                        isUnderwritePassIsSpecifyNotComplete,
                        underwritePassSpecifyRemark,
                        isUnderwriteUnPassHealth,
                        isUnderwriteUnPassOccupation,
                        isUnderwriteUnPassCantPay,
                        isUnderwriteUnPassCantContact,
                        isUnderwriteUnPassOther,
                       underwriteUnPassOtherRemark,
                       remark,
                       userId).FirstOrDefault();

                    //singnal R
                    var chat = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                    chat.Clients.Group(userId.ToString()).ReceiveGroupNotice("Success");

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
        /// Question Update V2
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueueResultByQueueIdInsert(
                int? queueId,
                string isUnderwriteCust,
                string isUnderwritePayer,
                string isBeware,
                string bewareRemark,
                string remark,
                string coBrandNo,
                string uRLPath,
                string physicalPath,
                string fileName,
              string isLineOA,
                string giveRemark
            )
        {

            int? userId = Convert.ToInt32(GlobalFunction.GetLoginDetail(HttpContext).User_ID);

            bool isUnderwriteCust_u = false;
            bool isUnderwritePayer_u = false;
            bool isLineOA_u = isLineOA == "1" ? true : false;
            bool isBeware_u = isBeware == "1" ? true : false;

            if (isUnderwriteCust == "customer")
            {
                isUnderwriteCust_u = true;
            }
            if (isUnderwritePayer == "payer")
            {
                isUnderwritePayer_u = true;
            }


            using (var db = new UnderwriteBranchV2Entities())
            {

                var result = db.usp_QueueResultByQueueIdV2_Insert(
                    queueId,
                    isUnderwriteCust_u,
                    isUnderwritePayer_u,
                    isBeware_u,
                    bewareRemark,
                    remark,
                    coBrandNo,
                    uRLPath,
                    physicalPath,
                    fileName,
                    isLineOA_u,
                    giveRemark,
                    userId
                    ).FirstOrDefault();

                //singnal R
                var chat = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                chat.Clients.Group(userId.ToString()).ReceiveGroupNotice("Success");

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Queue Approve //ประธานพิจารณาผล
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueueApproveUpdate(int QueueId,
                                               int ApproveStatusId,
                                               string ApproveRemark,
                                               bool? CMIsApprovePassCondition,
                                               bool? CMResultPassIsSpecify,
                                               bool? CMDenyHealth,
                                               bool? CMDenyOccupation,
                                               bool? CMDenyCantPay,
                                               bool? CMDenyCantContact,
                                               bool? CMDenyOther,
                                               string CMDenyRemark,
                                               string CMResultPassSpecifyRemark)
        {
            var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

            var nCMDenyRemark = (CMDenyOther == true) ? CMDenyRemark : null;
            //UPDATE
            var resultUpdated = _context.usp_QueueApproveByQueueId_Update(QueueId,
                                                                          ApproveStatusId,
                                                                          userId,
                                                                          ApproveRemark,
                                                                          CMIsApprovePassCondition,
                                                                          CMResultPassIsSpecify,
                                                                          CMResultPassSpecifyRemark,
                                                                          CMDenyHealth,
                                                                          CMDenyOccupation,
                                                                          CMDenyCantPay,
                                                                          CMDenyCantContact,
                                                                          CMDenyOther,
                                                                          nCMDenyRemark).FirstOrDefault();

            //Signal R
            var chat = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            chat.Clients.Group(userId.ToString()).ReceiveGroupNotice("Success");

            return Json(new { IsResult = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult QueueCoBrandInsert(int giverTypeId, int queueId, string coBrandNo, string giverDate, string uRLPath, string physicalPath, string fileName, string trackingNo, string giveTypeRemark, bool isLineOA, string giveRemark)
        {
            try
            {
                DateTime? giverDateConvert = null;
                if (giverDate != "")
                {
                    giverDateConvert = Convert.ToDateTime(giverDate);
                }

                var UserDetail = OAuth2Helper.GetLoginDetail();
                //TODO : give type
                var result = _context.usp_QueueCoBrand_Insert(queueId, coBrandNo, UserDetail.User_ID, giverDateConvert, giverTypeId, uRLPath, physicalPath, fileName, UserDetail.User_ID, trackingNo, giveTypeRemark, isLineOA, giveRemark).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                //throw new Exception(e.Message, e.InnerException);
                return Json(new { IsResult = false, Msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Image Upload
        /// </summary>
        /// <param name="file"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file, FormCollection form)
        {
            try
            {
                // ดึง User_ID จาก Session
                var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                // รับค่าจากฟอร์ม
                var queueId = form["queueId"];
                var queueCreatedDate = form["createdDate"];

                // แปลงเป็นปี+เดือน เช่น 202504
                var yearMonth = Convert.ToDateTime(queueCreatedDate).ToString("yyyyMM", new CultureInfo("en-US"));

                // อ่านค่าจาก Web.config (Settings.settings)
                string basePhysicalPath = Properties.Settings.Default.FileImgPath + ("PH");           // เช่น D:\FileImg\
                string baseUrl = Properties.Settings.Default.AbsolutePathImage + ("PH");              // เช่น http://yourdomain.com/FileImg/

                // สร้าง path สำหรับเก็บไฟล์
                string folderPath = Path.Combine(basePhysicalPath, yearMonth, $"QueueID_{queueId}");

                // ตั้งชื่อไฟล์ เช่น 210_20250422104500.jpg
                var fileName = $"{queueId}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.jpg";
                string fullPath = Path.Combine(folderPath, fileName);

                // สร้างไดเรกทอรีถ้ายังไม่มี
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // ลบไฟล์เดิมทั้งหมดก่อนอัปโหลดใหม่
                DirectoryInfo di = new DirectoryInfo(folderPath);
                foreach (FileInfo fi in di.EnumerateFiles())
                {
                    fi.Delete();
                }

                // Resize ภาพก่อนเซฟ (ใช้ ImageResizer)
                ImageJob img = new ImageJob(file, fullPath, new ResizeSettings("width=2000;height=2000;format=jpg;mode=max"))
                {
                    CreateParentDirectory = true
                };
                img.Build();

                // คืนค่า URL ที่จะใช้เปิดรูป และพาธจริง
                string relativePath = $"{yearMonth}/QueueID_{queueId}/{fileName}";
                string urlPath = baseUrl.TrimEnd('/') + "/" + relativePath.Replace("\\", "/");
                string physicalPath = fullPath;


                return Json(new
                {
                    id = 123,
                    success = true,
                    response = "File uploaded.",
                    fileImgName = fileName,
                    urlPath = urlPath,
                    physicalPath = physicalPath
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    success = false,
                    response = e.Message
                });
            }
        }




        [HttpPost]
        public ActionResult File_delete(int queueId, string queueCreatedDate)
        {
            try
            {
                //Year - Month
                var yearMonth = Convert.ToDateTime(queueCreatedDate).ToString("yyyyMM", new CultureInfo("en-Us"));
                //path folder
                string path = Properties.Settings.Default.FileImgPath + String.Format("\\PH\\{0}\\QueueID_{1}", yearMonth, queueId);

                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo fi in di.EnumerateFiles())
                {
                    fi.Delete();
                }

                return Json(new
                {
                    IsResult = true,
                    Msg = "success"
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    IsResult = false,
                    Msg = e.Message
                });
            }
        }

        /// <summary>
        /// สถานะคิวงานคัดกรอง
        /// </summary>
        /// <param name="dcr"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetQueueStatus(string dcr, int? branchId)
        {
            var cDCR = Convert.ToDateTime(dcr);
            //สถานะคิวงานคัดกรอง
            var objQueueStatus = _context.usp_pivotQueueStatusByBranchId_Select(cDCR, branchId, null, null, null).FirstOrDefault();

            if (objQueueStatus == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(objQueueStatus, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPivotQueueStatus(string dcr, int? branchId)
        {
            var cDCR = Convert.ToDateTime(dcr);
            int? Branch_Id = null;
            if (branchId != -1)
            {
                Branch_Id = branchId;
            }
            //สถานะคิวงานคัดกรอง
            var objQueueStatus = _context.usp_pivotQueueStatus_Select(cDCR, null, Branch_Id, null).FirstOrDefault();

            if (objQueueStatus == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(objQueueStatus, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetPivotQueueFollowUp_dt(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string startCoverDate, int? payerBranchId, string searchDetail)
        {
            try
            {
                DateTime? nStartCoverdate = null;
                if (startCoverDate != "-1" && startCoverDate != null)
                {
                    nStartCoverdate = Convert.ToDateTime(startCoverDate);
                }
                int? BranchId = null;
                if (payerBranchId != -1)
                {
                    BranchId = payerBranchId;
                }
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_pivotQueueFollowup_Select(BranchId, nStartCoverdate, (searchDetail == "" ? null : searchDetail.Trim()), indexStart, pageSize, sortField, orderType).ToList();

                var dt = new Dictionary<string, object>
                {
                    {"draw", draw },
                    {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                    {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                    {"data", result.ToList()}
                };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        /// <summary>
        /// พิจารณาคิวงาน
        /// </summary>
        /// <param name="dcr"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetQueueApprove(string dcr, int? branchId)
        {
            var cDCR = Convert.ToDateTime(dcr);
            //พิจารณาคิวงาน
            var objQueueApprove = _context.usp_pivotQueueApproveByBranchId_Select(cDCR, branchId, null, null, null).FirstOrDefault();

            if (objQueueApprove == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            return Json(objQueueApprove, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ยังไม่คัดกรองคนในสาขา
        /// </summary>
        /// <param name="dcr"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetQueueUnderwritePending(string dcr, int? branchId)
        {
            var cDCR = Convert.ToDateTime(dcr);

            //ยังไม่คัดกรองคนในสาขา
            var objQueueUnderwritePending = _context.usp_pivotQueueUnderwritePendingByBranchId_Select(cDCR, branchId).ToList();

            return Json(objQueueUnderwritePending, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ยังไม่คัดกรองคนนอกสาขา
        /// </summary>
        /// <param name="dcr"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetQueueUnderwritePendingOutsider(string dcr, int? branchId, string userList)
        {
            var cDCR = Convert.ToDateTime(dcr);

            //ยังไม่คัดกรองคนนอกสาขา
            var objQueueUnderwritePendingOutsider = _context.usp_pivotQueueUnderwritePendingOutsiderByBranchId_Select(cDCR, branchId, userList).FirstOrDefault();

            if (objQueueUnderwritePendingOutsider == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            return Json(objQueueUnderwritePendingOutsider, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// คิวงานคนในสาขา
        /// </summary>
        /// <param name="dcr"></param>
        /// <param name="assignToUser"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetQueueStatusAssignToUser(string dcr, int? assignToUser)
        {
            var cDCR = Convert.ToDateTime(dcr);

            //คิวงานคนในสาขา
            var objQueueStatusAssignToUser = _context.usp_pivotQueueStatusByAssignToUserId_Select(cDCR, assignToUser).FirstOrDefault();

            if (objQueueStatusAssignToUser == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            return Json(objQueueStatusAssignToUser, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// คิวงานคนนอกสาขา
        /// </summary>
        /// <param name="dcr"></param>
        /// <param name="assignToUser"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetQueueStatusOutsider(string dcr, int? branchId, string userList)
        {
            var cDCR = Convert.ToDateTime(dcr);

            //คิวงานคนนอกสาขา
            var objQueueStatusOutsider = _context.usp_pivotQueueStatusOutsiderByBranchId_Select(cDCR, branchId, userList).FirstOrDefault();

            if (objQueueStatusOutsider == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            return Json(objQueueStatusOutsider, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get_PayerOtherApp
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        public ActionResult GetPayerOtherApp(string appCode)
        {
            using (var client = new UnderwriteBranchServiceClient())
            {
                //GET DATA FROM SERVICE
                var result = client.Get_PayerOtherAppByAppCode(appCode).ToList();

                //RETURN JSON
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion api
    }
}