using ImageResizer;
using Microsoft.AspNet.SignalR;
using SmileUnderwriteBranchV2.Helper;
using SmileUnderwriteBranchV2.Models;
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
    public class CancerUnderwriteController : Controller
    {
        #region Context

        private UnderwriteBranchV2CancerEntities _context;
        private UnderwriteBranchV2Entities _contextPH;

        public CancerUnderwriteController()
        {

            _context = new UnderwriteBranchV2CancerEntities();
            _contextPH = new UnderwriteBranchV2Entities();
        }

        #endregion Context

        #region View

        // GET: CancerUnderwrite
        [Authorization(Roles = "Developer,UnderwriteV2_Director")]
        public ActionResult CancerUnderwriteIndex(string queueId)
        {
            var encodeId = queueId;
            if (encodeId != null && encodeId != string.Empty)
            {
                byte[] data = Convert.FromBase64String(encodeId);
                string decodedString = Encoding.UTF8.GetString(data);


                var result = _context.usp_QueueCIFullDetailByQueueId_Select(Convert.ToInt32(decodedString), null).FirstOrDefault();
                ViewBag.AppCode = result.ApplicationCode;
                //เช็คว่าต้องเป็นสถานะคิว ยังไม่ได้คัดกรอง
                if (result.QueueStatusId == 2 || result.QueueStatusId == 8)
                {
                    ViewBag.QueueFullDetail = result;
                    var applicationCode = result.ApplicationCode;

                    ViewBag.QueueId = result.QueueId;
                    ViewBag.AppCode = result.ApplicationCode;

                    var result2 = _context.usp_UnderwriteTypeCI_Select(null).ToList();
                    ViewBag.UnderwriteType = result2;

                    var result3 = _context.usp_UnderwritePaymentTypeCI_Select(null).ToList();
                    ViewBag.UnderwritePaymentType = result3;

                    var result4 = _context.usp_CallStatusCI_Select(null).ToList();
                    ViewBag.CallStatusList = result4;

                    var OpenSSS = Properties.Settings.Default.SSSpath;
                    ViewBag.OpenSSS = OpenSSS;

                    var giveType = _contextPH.usp_GiveType_Select(null).ToList();
                    ViewBag.giveType = giveType;


                    //ชำระเบี้ยให้อีก xxx คน
                    var countPaymentHistory = _context.usp_QueueCIPaymentHistory_Select(Convert.ToInt32(decodedString), null, null, null, null).Count();
                    ViewBag.countPaymentHistory = countPaymentHistory;

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

                    // StartCoverDate BE 2 ตำแหน่ง
                    if (result.StartCoverDate != null)
                    {
                        ViewBag.StartCoverDate = string.Format("{0}/{1}/{2} {3}", result.StartCoverDate.Value.Day.ToString("00", null), result.StartCoverDate.Value.Month.ToString("00", null), result.StartCoverDate.Value.Year + 543, result.StartCoverDate.Value.ToString("HH:mm:ss"));
                    }

                    //InsuredBirthDate
                    if (result.InsuredBirthDate != null)
                    {
                        ViewBag.InsuredBirthDate = string.Format("{0}/{1}/{2}", result.InsuredBirthDate.Value.Day.ToString("00", null), result.InsuredBirthDate.Value.Month.ToString("00", null), result.InsuredBirthDate.Value.Year + 543);
                    }

                    return View();
                }
                else
                {
                    return RedirectToAction("CancerUnderwriteDetail", new { queueId = encodeId });
                }
            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
            }
        }

        [Authorization(Roles = "Developer,UnderwriteV2_Director,UnderwriteV2_BusinesPromoteTeam")]
        public ActionResult CancerUnderwriteEditV2(string queueId)
        {
            var encodeId = queueId;
            if (encodeId != null && encodeId != string.Empty)
            {
                byte[] data = Convert.FromBase64String(encodeId);
                string decodedString = Encoding.UTF8.GetString(data);

                var result = _context.usp_QueueCIFullDetailByQueueId_Select(Convert.ToInt32(decodedString), null).FirstOrDefault();
                ViewBag.AppCode = result.ApplicationCode;
                //เช็คว่าต้องเป็นสถานะคิว ยังไม่ได้คัดกรอง

                ViewBag.QueueFullDetail = result;
                var applicationCode = result.ApplicationCode;

                ViewBag.QueueId = result.QueueId;
                ViewBag.AppCode = result.ApplicationCode;

                var result2 = _context.usp_UnderwriteTypeCI_Select(null).ToList();
                ViewBag.UnderwriteType = result2;

                var result3 = _context.usp_UnderwritePaymentTypeCI_Select(null).ToList();
                ViewBag.UnderwritePaymentType = result3;

                var result4 = _context.usp_CallStatusCI_Select(null).ToList();
                ViewBag.CallStatusList = result4;

                var OpenSSS = Properties.Settings.Default.SSSpath;
                ViewBag.OpenSSS = OpenSSS;

                var giveType = _contextPH.usp_GiveType_Select(null).ToList();
                ViewBag.giveType = giveType;

                //ชำระเบี้ยให้อีก xxx คน
                var countPaymentHistory = _context.usp_QueueCIPaymentHistory_Select(Convert.ToInt32(decodedString), null, null, null, null).Count();
                ViewBag.countPaymentHistory = countPaymentHistory;

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

                // StartCoverDate BE 2 ตำแหน่ง
                if (result.StartCoverDate != null)
                {
                    ViewBag.StartCoverDate = string.Format("{0}/{1}/{2} {3}", result.StartCoverDate.Value.Day.ToString("00", null), result.StartCoverDate.Value.Month.ToString("00", null), result.StartCoverDate.Value.Year + 543, result.StartCoverDate.Value.ToString("HH:mm:ss"));
                }

                //InsuredBirthDate
                if (result.InsuredBirthDate != null)
                {
                    ViewBag.InsuredBirthDate = string.Format("{0}/{1}/{2}", result.InsuredBirthDate.Value.Day.ToString("00", null), result.InsuredBirthDate.Value.Month.ToString("00", null), result.InsuredBirthDate.Value.Year + 543);
                }

                return View();

            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
            }
        }
        [Authorization(Roles = "Developer,UnderwriteV2_Director,UnderwriteV2_BusinesPromoteTeam")]
        public ActionResult CancerUnderwriteEdit(string queueId)
        {
            var encodeId = queueId;
            if (encodeId != null && encodeId != string.Empty)
            {
                byte[] data = Convert.FromBase64String(encodeId);
                string decodedString = Encoding.UTF8.GetString(data);
                var CancerCloseDate = Properties.Settings.Default.CancerCloseDate;
                ViewBag.CancerCloseDate = CancerCloseDate;
                var result = _context.usp_QueueCIFullDetailByQueueId_Select(Convert.ToInt32(decodedString), null).FirstOrDefault();
                if (result.IsPolicies != true)
                {
                    return RedirectToAction("CancerUnderwriteeditV2", new { queueId = queueId });

                }



                ViewBag.AppCode = result.ApplicationCode;
                //เช็คว่าต้องเป็นสถานะคิว ยังไม่ได้คัดกรอง

                ViewBag.QueueFullDetail = result;
                var applicationCode = result.ApplicationCode;

                ViewBag.QueueId = result.QueueId;
                ViewBag.AppCode = result.ApplicationCode;

                var result2 = _context.usp_UnderwriteTypeCI_Select(null).ToList();
                ViewBag.UnderwriteType = result2;

                var result3 = _context.usp_UnderwritePaymentTypeCI_Select(null).ToList();
                ViewBag.UnderwritePaymentType = result3;

                var result4 = _context.usp_CallStatusCI_Select(null).ToList();
                ViewBag.CallStatusList = result4;

                var OpenSSS = Properties.Settings.Default.SSSpath;
                ViewBag.OpenSSS = OpenSSS;

                var giveType = _contextPH.usp_GiveType_Select(null).ToList();
                ViewBag.giveType = giveType;

                //ชำระเบี้ยให้อีก xxx คน
                var countPaymentHistory = _context.usp_QueueCIPaymentHistory_Select(Convert.ToInt32(decodedString), null, null, null, null).Count();
                ViewBag.countPaymentHistory = countPaymentHistory;

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

                // StartCoverDate BE 2 ตำแหน่ง
                if (result.StartCoverDate != null)
                {
                    ViewBag.StartCoverDate = string.Format("{0}/{1}/{2} {3}", result.StartCoverDate.Value.Day.ToString("00", null), result.StartCoverDate.Value.Month.ToString("00", null), result.StartCoverDate.Value.Year + 543, result.StartCoverDate.Value.ToString("HH:mm:ss"));
                }

                //InsuredBirthDate
                if (result.InsuredBirthDate != null)
                {
                    ViewBag.InsuredBirthDate = string.Format("{0}/{1}/{2}", result.InsuredBirthDate.Value.Day.ToString("00", null), result.InsuredBirthDate.Value.Month.ToString("00", null), result.InsuredBirthDate.Value.Year + 543);
                }

                return View();

            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
            }
        }
        public ActionResult CancerUnderwriteDetailV2(string queueId, string appId)
        {
            int? QueueId = null;
            string AppCode = null;

            try
            {
                if (!string.IsNullOrEmpty(queueId))
                {
                    byte[] dataQueue = Convert.FromBase64String(queueId);
                    var decodedQueueIdString = Encoding.UTF8.GetString(dataQueue);
                    QueueId = Convert.ToInt32(decodedQueueIdString);
                }
                else if (!string.IsNullOrEmpty(appId))
                {
                    byte[] dataAppId = Convert.FromBase64String(appId);
                    AppCode = Encoding.UTF8.GetString(dataAppId);
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

                var result = _context.usp_QueueCIFullDetailByQueueId_Select(QueueId, AppCode).FirstOrDefault();


                ViewBag.QueueCancerFullDetail = result;
                ViewBag.QueueId = result.QueueId;

                ViewBag.AppCode = result.ApplicationCode;
                var applicationCode = result.ApplicationCode;
                var result2 = _context.usp_UnderwriteTypeCI_Select(null).ToList();
                ViewBag.UnderwriteType = result2;

                var result3 = _context.usp_UnderwritePaymentTypeCI_Select(null).ToList();
                ViewBag.UnderwritePaymentType = result3;

                var result4 = _context.usp_CallStatusCI_Select(null).ToList();
                ViewBag.CallStatusList = result4;

                var OpenSSS = Properties.Settings.Default.SSSpath;
                ViewBag.OpenSSS = OpenSSS;

                //ชำระเบี้ยให้อีก xxx คน
                var countPaymentHistory = _context.usp_QueueCIPaymentHistory_Select(QueueId, null, null, null, null).Count();
                ViewBag.countPaymentHistory = countPaymentHistory;

                //check date for edit
                var CMDueDate = Properties.Settings.Default.CMDueDate;
                ViewBag.IsEdit = false;

                var now = DateTime.Now.Date;
                var AppDCR = result.StartCoverDate.Value;
                var CurrentMonth = new DateTime(now.Year, now.Month, 1);

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

                // StartCoverDate BE 2 ตำแหน่ง
                if (result.StartCoverDate != null)
                {
                    ViewBag.StartCoverDate = string.Format("{0}/{1}/{2} {3}", result.StartCoverDate.Value.Day.ToString("00", null), result.StartCoverDate.Value.Month.ToString("00", null), result.StartCoverDate.Value.Year + 543, result.StartCoverDate.Value.ToString("HH:mm:ss"));
                }

                //InsuredBirthDate
                if (result.InsuredBirthDate != null)
                {
                    ViewBag.InsuredBirthDate = string.Format("{0}/{1}/{2}", result.InsuredBirthDate.Value.Day.ToString("00", null), result.InsuredBirthDate.Value.Month.ToString("00", null), result.InsuredBirthDate.Value.Year + 543);
                }

                //GiveDate
                if (result.GiveDate != null)
                {
                    ViewBag.GiveDate = string.Format("{0}/{1}/{2}", result.GiveDate.Value.Day.ToString("00", null), result.GiveDate.Value.Month.ToString("00", null), result.GiveDate.Value.Year + 543);
                }

                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "InnerException:" + e.InnerException + ", Message:" + e.Message });
            }
        }

        public ActionResult CancerUnderwriteDetail(string queueId, string appId)
        {
            int? QueueId = null;
            string AppCode = null;

            try
            {
                if (!string.IsNullOrEmpty(queueId))
                {
                    byte[] dataQueue = Convert.FromBase64String(queueId);
                    var decodedQueueIdString = Encoding.UTF8.GetString(dataQueue);
                    QueueId = Convert.ToInt32(decodedQueueIdString);
                }
                else if (!string.IsNullOrEmpty(appId))
                {
                    byte[] dataAppId = Convert.FromBase64String(appId);
                    AppCode = Encoding.UTF8.GetString(dataAppId);
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

                var result = _context.usp_QueueCIFullDetailByQueueId_Select(QueueId, AppCode).FirstOrDefault();

                if (result.IsPolicies != true)
                {
                    if (queueId != null && queueId != "")
                    {

                        return RedirectToAction("CancerUnderwriteDetailV2", new { queueId = queueId });
                    }
                    else if (appId != null && appId != "")
                    {
                        return RedirectToAction("CancerUnderwriteDetailV2", new { appId = appId });
                    }
                    else
                    {
                        return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
                    }


                }
                ViewBag.QueueCancerFullDetail = result;
                ViewBag.QueueId = result.QueueId;

                ViewBag.AppCode = result.ApplicationCode;
                var applicationCode = result.ApplicationCode;
                var result2 = _context.usp_UnderwriteTypeCI_Select(null).ToList();
                ViewBag.UnderwriteType = result2;

                var result3 = _context.usp_UnderwritePaymentTypeCI_Select(null).ToList();
                ViewBag.UnderwritePaymentType = result3;

                var result4 = _context.usp_CallStatusCI_Select(null).ToList();
                ViewBag.CallStatusList = result4;

                var OpenSSS = Properties.Settings.Default.SSSpath;
                ViewBag.OpenSSS = OpenSSS;

                //ชำระเบี้ยให้อีก xxx คน
                var countPaymentHistory = _context.usp_QueueCIPaymentHistory_Select(QueueId, null, null, null, null).Count();
                ViewBag.countPaymentHistory = countPaymentHistory;

                //check date for edit
                var CMDueDate = Properties.Settings.Default.CMDueDate;
                ViewBag.IsEdit = false;

                var now = DateTime.Now.Date;
                var AppDCR = result.StartCoverDate.Value;
                var CurrentMonth = new DateTime(now.Year, now.Month, 1);

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

                // StartCoverDate BE 2 ตำแหน่ง
                if (result.StartCoverDate != null)
                {
                    ViewBag.StartCoverDate = string.Format("{0}/{1}/{2} {3}", result.StartCoverDate.Value.Day.ToString("00", null), result.StartCoverDate.Value.Month.ToString("00", null), result.StartCoverDate.Value.Year + 543, result.StartCoverDate.Value.ToString("HH:mm:ss"));
                }

                //InsuredBirthDate
                if (result.InsuredBirthDate != null)
                {
                    ViewBag.InsuredBirthDate = string.Format("{0}/{1}/{2}", result.InsuredBirthDate.Value.Day.ToString("00", null), result.InsuredBirthDate.Value.Month.ToString("00", null), result.InsuredBirthDate.Value.Year + 543);
                }

                //GiveDate
                if (result.GiveDate != null)
                {
                    ViewBag.GiveDate = string.Format("{0}/{1}/{2}", result.GiveDate.Value.Day.ToString("00", null), result.GiveDate.Value.Month.ToString("00", null), result.GiveDate.Value.Year + 543);
                }

                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "InnerException:" + e.InnerException + ", Message:" + e.Message });
            }
        }

        [Authorization(Roles = "Developer,UnderwriteV2_Chairman")]
        public ActionResult CancerDashboardCM()
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            //DCR
            var changePeriodDay = Properties.Settings.Default.CancerChangePeriodDay;
            var numberOfPeriodToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.GetPeriodList(changePeriodDay, numberOfPeriodToShow);
            ViewBag.PeriodList = periodList;

            var branchList = _contextPH.usp_BranchByChairmanUserId_Select(user.User_ID).ToList();
            ViewBag.BranchList = branchList;

            var currentBranch = -1;
            if (branchList.Count > 0) currentBranch = branchList[0].Branch_ID;

            ViewBag.CurrentBranchID = currentBranch;

            var currentDCR = Convert.ToDateTime(periodList[0].Value);
            ViewBag.CurrentDCR = currentDCR;

            return View();
        }

        [Authorization(Roles = "Developer,UnderwriteV2_BusinesPromoteTeam")]
        public ActionResult CancerDashboardBPT()
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
            var changePeriodDay = Properties.Settings.Default.CancerChangePeriodDay;
            var numberOfPeriodToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.GetPeriodList(changePeriodDay, numberOfPeriodToShow);
            ViewBag.PeriodList = periodList;

            var branchList = _contextPH.usp_BranchByBusinessPromoteTeamUserId_Select(userId).ToList();
            ViewBag.BranchList = branchList;

            var currentDCR = Convert.ToDateTime(periodList[0].Value);
            ViewBag.CurrentDCR = currentDCR;
            return View();
        }

        [Authorization(Roles = "Developer,UnderwriteV2_BusinesPromoteTeam")]
        public ActionResult CancerUnderwriteDashboard()
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
            var changePeriodDay = Properties.Settings.Default.CancerChangePeriodDay;
            var numberOfPeriodToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.GetPeriodList(changePeriodDay, numberOfPeriodToShow);
            ViewBag.PeriodList = periodList;

            var branchList = _contextPH.usp_BranchByBusinessPromoteTeamUserId_Select(null).ToList();
            ViewBag.BranchList = branchList;

            var currentDCR = Convert.ToDateTime(periodList[0].Value);
            ViewBag.CurrentDCR = currentDCR;
            return View();
        }


        public ActionResult CancerUnderwriteDetailForUnderwriteOverdue(string queueId)
        {
            var encodeId = queueId;
            if (encodeId != null && encodeId != string.Empty)
            {
                byte[] data = Convert.FromBase64String(encodeId);
                string decodedString = Encoding.UTF8.GetString(data);

                var result = _context.usp_QueueCIFullDetailByQueueId_Select(Convert.ToInt32(decodedString), null).FirstOrDefault();
                ViewBag.AppCode = result.ApplicationCode;
                //เช็คว่าต้องเป็นสถานะคิว ยังไม่ได้คัดกรอง
                if (result.QueueStatusId == 2)
                {
                    ViewBag.QueueFullDetail = result;
                    var applicationCode = result.ApplicationCode;

                    ViewBag.QueueId = result.QueueId;
                    ViewBag.AppCode = result.ApplicationCode;

                    var result2 = _context.usp_UnderwriteTypeCI_Select(null).ToList();
                    ViewBag.UnderwriteType = result2;

                    var result3 = _context.usp_UnderwritePaymentTypeCI_Select(null).ToList();
                    ViewBag.UnderwritePaymentType = result3;

                    var result4 = _context.usp_CallStatusCI_Select(null).ToList();
                    ViewBag.CallStatusList = result4;

                    var OpenSSS = Properties.Settings.Default.SSSpath;
                    ViewBag.OpenSSS = OpenSSS;

                    //ชำระเบี้ยให้อีก xxx คน
                    var countPaymentHistory = _context.usp_QueueCIPaymentHistory_Select(Convert.ToInt32(decodedString), null, null, null, null).Count();
                    ViewBag.countPaymentHistory = countPaymentHistory;

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

                    // StartCoverDate BE 2 ตำแหน่ง
                    if (result.StartCoverDate != null)
                    {
                        ViewBag.StartCoverDate = string.Format("{0}/{1}/{2} {3}", result.StartCoverDate.Value.Day.ToString("00", null), result.StartCoverDate.Value.Month.ToString("00", null), result.StartCoverDate.Value.Year + 543, result.StartCoverDate.Value.ToString("HH:mm:ss"));
                    }

                    //InsuredBirthDate
                    if (result.InsuredBirthDate != null)
                    {
                        ViewBag.InsuredBirthDate = string.Format("{0}/{1}/{2}", result.InsuredBirthDate.Value.Day.ToString("00", null), result.InsuredBirthDate.Value.Month.ToString("00", null), result.InsuredBirthDate.Value.Year + 543);
                    }

                    return View();
                }
                else
                {
                    return RedirectToAction("CancerUnderwriteDetail", new { queueId = encodeId });
                }
            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
            }
        }
        #endregion View

        #region API

        public ActionResult GetPayerOtherApp(string queueId)
        {
            try
            {
                var test = int.Parse(queueId);
                var result = _context.usp_QueueCIPaymentHistory_Select(test, null, null, null, null).ToList();

                return Json(new { IsResult = true, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsResult = false, Msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

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
                string basePhysicalPath = Properties.Settings.Default.FileImgPath + ("Cancer");           // เช่น D:\FileImg\
                string baseUrl = Properties.Settings.Default.AbsolutePathImage + ("Cancer");              // เช่น http://yourdomain.com/FileImg/

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
                string path = AppDomain.CurrentDomain.BaseDirectory + String.Format("\\Cancer\\{0}\\QueueID_{1}", yearMonth, queueId);

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

        [HttpPost]
        [Authorization(Roles = "Developer,UnderwriteV2_Director,UnderwriteV2_Chairman,UnderwriteV2_BusinesPromoteTeam")]
        public ActionResult CancerQuestionUpdate(FormCollection form)
        {
            //สถานะการคัดกรอง
            var underwriteStatus = "1";

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
                var cancer = form["cancer"];
                var cancerChildPass = form["cancerChildPass"];
                var IsSpecify = form["IsSpecify"];
                var cancerChildPassOtherRemark = form["cancerChildPassOtherRemark"];
                //cancerChildNotpass split to array list
                var cancerChildNotpass = form["cancerChildNotPass"];
                var cancerChildNotpassOtherRemark = form["cancerChildNotPassOtherRemark"];
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
                bool? isUnderwriteUnPassCancer = null;
                bool? isUnderwriteUnPassOccupation = null;
                bool? isUnderwriteUnPassCantPay = null;
                bool? isUnderwriteUnPassCantContact = null;
                bool? isUnderwriteUnPassOther = null;
                string underwriteUnPassOtherRemark = null;
                string remarkAll = null;

                using (var db = new UnderwriteBranchV2CancerEntities())
                {
                    switch (underwriteStatus)
                    {
                        // 1 สถานะคัดกรองแล้ว
                        case "1":
                            isUnderwrite = true;
                            underwriteTypeId = Convert.ToInt32(typeSelect); //ช่องทางการคัดกรอง
                            underwritePaymentTypeId = Convert.ToInt32(uwPaymentType); //การชำระเบี้ย
                            isUnderwritePass = (cancer == "1"); // การคัดกรองด้านสุขภาพ ผ่าน=true,ไม่ผ่าน=false
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
                                switch (cancerChildPass)
                                {
                                    case "1":
                                        isUnderwritePassStandard = true; //ผ่าน ตามมาตรฐานทุกประการ
                                        break;

                                    case "0":
                                        isUnderwritePassCondition = true; //ผ่าน  มีประวัติสุขภาพที่ติดข้อยกเว้น
                                        underwritePassSpecifyRemark = cancerChildPassOtherRemark;

                                        if (!string.IsNullOrEmpty(IsSpecify))
                                        {
                                            switch (IsSpecify)
                                            {
                                                case "1": //ระบุในใบสมัครครบถ้วน
                                                    isUnderwritePassIsSpecify = true;
                                                    isUnderwritePassIsSpecifyNotComplete = null;
                                                    break;

                                                case "0"://ไม่ระบุ23
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
                                var arraycancerChildNotpass = cancerChildNotpass.Split(',');
                                //ไม่ผ่าน
                                foreach (var item in arraycancerChildNotpass)
                                {
                                    switch (item)
                                    {
                                        case "denycancer": //ด้านสุขภาพ
                                            isUnderwriteUnPassCancer = true;
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
                                            underwriteUnPassOtherRemark = cancerChildNotpassOtherRemark;
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
                    var result = db.usp_QueueCIResultByQueueId_Insert(Convert.ToInt32(queueId),
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
                                                                    isUnderwriteUnPassCancer,
                                                                    isUnderwriteUnPassOccupation,
                                                                    isUnderwriteUnPassCantPay,
                                                                    isUnderwriteUnPassCantContact,
                                                                    isUnderwriteUnPassOther,
                                                                    underwriteUnPassOtherRemark,
                                                                    remarkAll,
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



        /*    [HttpPost]
            public ActionResult CancerQueuePoliciesInsert(int queueId, string giverDate, string uRLPath, string physicalPath, string fileName, string trackingNo, int giveType, string giveTypeRemark)
            {
                try
                {
                    DateTime? giverDateConvert = null;
                    if (giverDate != "")
                    {
                        giverDateConvert = GlobalFunction.ConvertDatePickerToDate_BE(giverDate, null);
                    }

                    var UserDetail = OAuth2Helper.GetLoginDetail();

                    //TODO : give type

                    var result = _context.usp_QueueCIPolicies_Insert(queueId, UserDetail.User_ID, giverDateConvert, giveType, (uRLPath == "" ? null : uRLPath), (physicalPath == "" ? null : physicalPath), (fileName == "" ? null : fileName), UserDetail.User_ID, trackingNo, giveTypeRemark).FirstOrDefault();

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    //throw new Exception(e.Message, e.InnerException);
                    return Json(new { IsResult = false, Msg = e.Message }, JsonRequestBehavior.AllowGet);
                }
            }*/

        [HttpPost]
        /*  public ActionResult QueueCancerPoliciesInsert(int queueId, string giverDate, string uRLPath, string physicalPath, string fileName, string trackingNo, int giveType, string giveTypeRemark)
          {
              try
              {
                  DateTime? giverDateConvert = null;
                  if (giverDate != "")
                  {
                      giverDateConvert = GlobalFunction.ConvertDatePickerToDate_BE(giverDate, null);
                  }

                  var UserDetail = OAuth2Helper.GetLoginDetail();

                  //TODO : give type

                  var result = _context.usp_QueueCIPolicies_Insert(queueId, UserDetail.User_ID, giverDateConvert, giveType, uRLPath, physicalPath, fileName, UserDetail.User_ID, trackingNo, giveTypeRemark).FirstOrDefault();
                  //var result = _context.usp_QueueCIPolicies_Insert(queueId, UserDetail.User_ID, giverDateConvert, giveType, uRLPath, physicalPath, fileName, UserDetail.User_ID, trackingNo, giveTypeRemark).FirstOrDefault();

                  return Json(result, JsonRequestBehavior.AllowGet);
              }
              catch (Exception e)
              {
                  //throw new Exception(e.Message, e.InnerException);
                  return Json(new { IsResult = false, Msg = e.Message }, JsonRequestBehavior.AllowGet);
              }
          }*/
        //public ActionResult QueueCancerPoliciesInsertBackup(int? queueId, int? giverUserId, string giverDate, int? giveTypeId, string uRLPath, string physicalPath, string fileName, string trackingNo, string giveTypeRemark)
        //{
        //    try
        //    {
        //        DateTime? giverDateConvert = null;
        //        if (giverDate != "")
        //        {
        //            giverDateConvert = GlobalFunction.ConvertDatePickerToDate_BE(giverDate, null);
        //        }


        //        var createdByUserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

        //        var result = _context.usp_QueueCIPolicies_Insert(queueId, createdByUserId, giverDateConvert, giveTypeId, uRLPath, physicalPath, fileName, createdByUserId, trackingNo, giveTypeRemark).FirstOrDefault();

        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message, e.InnerException);
        //    }
        //}
        //[HttpPost]
        public ActionResult QueueCIResultByQueueIdInsert(int queueId, string isUnderwriteInsured, string isUnderwritePayer, string isBeware, string bewareRemark, string remark, string uRLPath, string physicalPath, string fileName, bool? isPolicies, string giveRemark)
        {
            {
                try
                {
                    var BewareData = isBeware == "1" ? bewareRemark : "";
                    var isUnderwriteInsuredConvert = isUnderwriteInsured == "insure" ? true : false;
                    var isUnderwritePayerConvert = isUnderwritePayer == "payer" ? true : false;
                    var isBewareConvert = isBeware == "1" ? true : false;
                    isPolicies = true;
                    var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                    //DECLARE
                    int? createdByUserId = Convert.ToInt32(user);


                    using (var db = new UnderwriteBranchV2CancerEntities())
                    {

                        //var result = 1;
                        var result = db.usp_QueueCIResultByQueueIdV2_Insert(queueId,
                                                                            isUnderwriteInsuredConvert,
                                                                            isUnderwritePayerConvert,
                                                                            isBewareConvert,
                                                                            BewareData,
                                                                            remark,
                                                                            uRLPath,
                                                                            physicalPath,
                                                                            fileName,

                                                                            giveRemark,
                                                                            createdByUserId).FirstOrDefault();

                        //singnal R
                        var chat = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                        chat.Clients.Group(createdByUserId.ToString()).ReceiveGroupNotice("Success");

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                {
                    //throw new Exception(e.Message, e.InnerException);
                    return Json(new { IsResult = false, Msg = e.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult QueueCancerPoliciesInsert(int? queueId, int? giverUserId, string giverDate, int? giveTypeId, string uRLPath, string physicalPath, string fileName, string trackingNo, string giveTypeRemark, string giveRemark)
        {
            try
            {

                giverDate = DateTime.Now.ToString(); // edit
                trackingNo = null; //editt
                DateTime? giverDateConvert = null;
                if (giverDate != "")
                {
                    giverDateConvert = GlobalFunction.ConvertDatePickerToDate_BE(giverDate, null);
                }


                var createdByUserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                var result = _context.usp_QueueCIPolicies_Insert(queueId, createdByUserId, giverDateConvert, giveTypeId, uRLPath, physicalPath, fileName, createdByUserId, trackingNo, giveTypeRemark, giveRemark).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpGet]
        public ActionResult GetQueueCIUnderwritePending(string dcr, int? branchId)
        {
            var cDCR = Convert.ToDateTime(dcr);

            //ยังไม่คัดกรองคนในสาขา
            var objQueueCIUnderwritePending = _context.usp_pivotQueueCIUnderwritePendingByBranchId_Select(cDCR, branchId).ToList();

            return Json(objQueueCIUnderwritePending, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetQueueCIUnderwritePendingOutsider(string dcr, int? branchId, string userList)
        {
            var cDCR = Convert.ToDateTime(dcr);

            //ยังไม่คัดกรองคนนอกสาขา
            var objQueueCIUnderwritePendingOutsider = _context.usp_pivotQueueCIUnderwritePendingOutsiderByBranchId_Select(cDCR, branchId, userList).FirstOrDefault();

            if (objQueueCIUnderwritePendingOutsider == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            return Json(objQueueCIUnderwritePendingOutsider, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetQueueCIApprove(string dcr, int? branchId)
        {
            var cDCR = Convert.ToDateTime(dcr);
            //พิจารณาคิวงาน
            var objQueueApprove = _context.usp_pivotQueueCIApproveByBranchId_Select(cDCR, branchId, null, null, null).FirstOrDefault();

            if (objQueueApprove == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            return Json(objQueueApprove, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetQueueCIStatus(string dcr, int? branchId)
        {
            var cDCR = Convert.ToDateTime(dcr);
            //สถานะคิวงานคัดกรอง
            var objQueueStatus = _context.usp_pivotQueueCIStatusByBranchId_Select(cDCR, branchId, null, null, null).FirstOrDefault();

            if (objQueueStatus == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(objQueueStatus, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetQueueCIStatusAssignToUser(string dcr, int? assignToUser)
        {
            var cDCR = Convert.ToDateTime(dcr);

            //คิวงานคนในสาขา
            var objQueueStatusAssignToUser = _context.usp_pivotQueueCIStatusByAssignToUserId_Select(cDCR, assignToUser).FirstOrDefault();

            if (objQueueStatusAssignToUser == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            return Json(objQueueStatusAssignToUser, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult QueueLog(int queueId, int draw, int? indexStart, int? pageSize, string sortField, string orderType, string search)
        {
            var result = _context.usp_QueueCILogByQueueId_Select(queueId, indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                 {"draw", draw },
                 {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                 {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                 {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorization(Roles = "Developer,UnderwriteV2_Director,UnderwriteV2_Chairman,UnderwriteV2_BusinesPromoteTeam")]
        public ActionResult QueueCIApproveUpdate(int QueueId,
                                               int ApproveStatusId,
                                               string ApproveRemark,
                                               bool? CMIsApprovePassCondition,
                                               bool? CMResultPassIsSpecify,
                                               bool? CMDenyCancer,
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
            var resultUpdated = _context.usp_QueueCIApproveByQueueId_Update(QueueId,
                                                                          ApproveStatusId,
                                                                          userId,
                                                                          ApproveRemark,
                                                                          CMIsApprovePassCondition,
                                                                          CMResultPassIsSpecify,
                                                                          CMResultPassSpecifyRemark,
                                                                          CMDenyCancer,
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

        [HttpGet]
        public ActionResult GetCancerPivotQueueStatus(string dcr, int? branchId)
        {
            var cDCR = Convert.ToDateTime(dcr);
            int? Branch_Id = null;
            if (branchId != -1)
            {
                Branch_Id = branchId;
            }
            //สถานะคิวงานคัดกรอง
            var objQueueStatus = _context.usp_pivotQueueCIStatus_Select(cDCR, null, Branch_Id, null).FirstOrDefault();

            if (objQueueStatus == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(objQueueStatus, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetPivotCancerQueueFollowUp_dt(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string startCoverDate, int? payerBranchId, string searchDetail)
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
                var result = _context.usp_pivotQueueCIFollowup_Select(BranchId, nStartCoverdate, (searchDetail == "" ? null : searchDetail.Trim()), indexStart, pageSize, sortField, orderType).ToList();

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

        #endregion API
    }
}