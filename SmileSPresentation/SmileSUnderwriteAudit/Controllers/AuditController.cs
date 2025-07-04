using Microsoft.AspNet.SignalR;
using SmileSUnderwriteAudit.Helper;
using SmileSUnderwriteAudit.Models;
using SmileSUnderwriteAudit.UnderwriteBranchService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace SmileSUnderwriteAudit.Controllers
{
    [Authorization]
    public class AuditController : Controller
    {
        #region Context

        private readonly UnderwriteAuditEntities _context;
        private readonly UnderwriteBranchV2Entities _contextUDWBranch;

        public AuditController()
        {
            _context = new UnderwriteAuditEntities();
            _contextUDWBranch = new UnderwriteBranchV2Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            _contextUDWBranch.Dispose();
        }

        #endregion Context

        #region View

        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure,UnderwriteAudit_AdminInsure,UnderwriteAudit_ConditionInsure")]
        public ActionResult AuditIndex(string queueAuditId)
        {
            var encodeId = queueAuditId;
            if (encodeId != null && encodeId != string.Empty)
            {
                //decode
                byte[] data = Convert.FromBase64String(encodeId);
                string decodedString = Encoding.UTF8.GetString(data);

                //call stored procedure
                var result = _context.usp_QueueAuditFullDetailByQueueAuditId_Select(Convert.ToInt32(decodedString)).FirstOrDefault();
                var OpenGoogleDoc = Properties.Settings.Default.GoogleDocPathPH;
                try
                {
                    var appCode = result.ApplicationCode;
                    var insureName = result.CustFirstName + " " + result.CustLastName;
                    var payerName = result.PayerFirstName + " " + result.PayerLastName;

                    string appStartCoverDateFormatted = result.AppStartCoverDate.HasValue
         ? result.AppStartCoverDate.Value.ToString("d/M/yyyy", new System.Globalization.CultureInfo("th-TH"))
         : string.Empty;

                    string modifiedURL = OpenGoogleDoc.Replace("AppID", result.ApplicationCode)
                                                          .Replace("Name1", insureName)
                                                          .Replace("Name2", payerName)
                                                          .Replace("DCR", appStartCoverDateFormatted);

                    ViewBag.OpenGoogleDoc = modifiedURL;
                }
                catch
                {
                    ViewBag.OpenGoogleDoc = OpenGoogleDoc;
                }
                //ถ้า null return notfouond
                if (result is null) return RedirectToAction("NotFound", "Error");

                //Get Role
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var accessRole = GlobalFunction.GetAccessRoleUnderWriteAudit(roleList);
                ViewBag.AccessRole = accessRole;

                //set app codehttp://localhost:61595/2
                ViewBag.AppCode = result.ApplicationCode;

                var OpenSSS = Properties.Settings.Default.SSSpath;
                ViewBag.OpenSSS = OpenSSS;

                //1 = na ให้ set default เป็น 2
                ViewBag.QueueAuditStatusId = (result.QueueAuditStatusId == 1 ? 2 : result.QueueAuditStatusId);

                ViewBag.QueueFullDetail = result;
                var applicationCode = result.ApplicationCode;

                var result2 = _context.usp_QueueAuditStatus_Select(null).ToList();
                ViewBag.QueueAuditStatus = result2;

                var result3 = _context.usp_CallStatus_Select(null).ToList();
                ViewBag.CallStatusList = result3;

                //var result4 = _context.usp_AuditUnderwriteResultStatus_Select(null).ToList();
                //ViewBag.AuditUnderwriteResultStatus = result4;

                var result5 = _context.usp_AuditHealthStatus_Select(null).ToList();
                ViewBag.AuditHealthStatus = result5;

                var result6 = _context.usp_AuditHealthSpecifyStatus_Select(null).ToList();
                ViewBag.AuditHealthSpecifyStatus = result6;

                var result7 = _contextUDWBranch.usp_UnderwriteType_Select(null).ToList();
                ViewBag.UnderwriteType = result7;

                var result8 = _contextUDWBranch.usp_UnderwritePaymentType_Select(null).ToList();
                ViewBag.UnderwritePaymentType = result8;

                var result9 = _contextUDWBranch.usp_CallStatus_Select(null).ToList();
                ViewBag.CallStatusListUDWBranch = result9;

                var result10 = _context.usp_AuditInsureStatus_Select(null).ToList();
                ViewBag.AuditInsureStatus = result10;

                var result11 = _context.usp_AuditInsureConsiderStatus_Select(null).ToList();
                ViewBag.AuditInsureConsiderStatus = result11;

                var result12 = _context.usp_AuditUnderwriteStatus_Select(null).ToList();
                ViewBag.AuditUnderwriteStatus = result12;

                var result13 = _context.usp_AuditCobrandGivenStatus_Select(null).ToList();
                ViewBag.AuditCobrandGivenStatus = result13;

                //DOCUMENT
                var DocumentClient = new SSSDocService.DocumentServiceClient();

                var mergDoclink = new List<Models.Document>();

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

                DocumentClient.Close();

                return View();
            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
            }
        }

        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure,UnderwriteAudit_AdminInsure,UnderwriteAudit_ConditionInsure")]
        public ActionResult AuditIndexV2(string queueAuditId, int? insuredChack)
        {
            var encodeId = queueAuditId;
            if (encodeId != null && encodeId != string.Empty)
            {
                //decode
                byte[] data = Convert.FromBase64String(encodeId);
                string decodedString = Encoding.UTF8.GetString(data);


                //call stored procedure
                var result = _context.usp_QueueAuditFullDetailByQueueAuditIdV2_Select(Convert.ToInt32(decodedString)).FirstOrDefault();

                if (insuredChack == 1)
                {
                    ViewBag.insuredChack = true;
                }
                else
                {
                    ViewBag.insuredChack = false;
                }

                //ถ้า null return notfouond
                if (result is null) return RedirectToAction("AuditIndex", new { queueAuditId = queueAuditId });

                var startNewVersionPH = Properties.Settings.Default.StartNewVersionPH;
                DateTime? oldPeriod = Convert.ToDateTime(startNewVersionPH);
                DateTime? nPeriod = Convert.ToDateTime(result.AppStartCoverDate);
                if (oldPeriod > nPeriod)
                {
                    return RedirectToAction("AuditIndex", new { queueAuditId = queueAuditId });
                }



                //Get Role
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var accessRole = GlobalFunction.GetAccessRoleUnderWriteAudit(roleList);
                ViewBag.AccessRole = accessRole;

                //set app code
                ViewBag.AppCode = result.ApplicationCode;

                var OpenSSS = Properties.Settings.Default.SSSpath;
                ViewBag.OpenSSS = OpenSSS;
                //1 = na ให้ set default เป็น 2
                ViewBag.QueueAuditStatusId = (result.QueueAuditStatusId == 1 ? 2 : result.QueueAuditStatusId);
                ViewBag.AuditInsureStatusId = (result.AuditInsureStatusId == 1 ? 2 : result.AuditInsureStatusId);
                ViewBag.QueueFullDetail = result;
                var applicationCode = result.ApplicationCode;

                var result2 = _context.usp_QueueAuditStatus_Select(null).ToList();
                ViewBag.QueueAuditStatus = result2;

                var result3 = _context.usp_CallStatus_Select(null).ToList();
                ViewBag.CallStatusList = result3;

                //var result4 = _context.usp_AuditUnderwriteResultStatus_Select(null).ToList();
                //ViewBag.AuditUnderwriteResultStatus = result4;

                var result5 = _context.usp_AuditHealthStatus_Select(null).ToList();
                ViewBag.AuditHealthStatus = result5;

                var result6 = _context.usp_AuditHealthSpecifyStatus_Select(null).ToList();
                ViewBag.AuditHealthSpecifyStatus = result6;

                var result7 = _contextUDWBranch.usp_UnderwriteType_Select(null).ToList();
                ViewBag.UnderwriteType = result7;

                var result8 = _contextUDWBranch.usp_UnderwritePaymentType_Select(null).ToList();
                ViewBag.UnderwritePaymentType = result8;

                var result9 = _contextUDWBranch.usp_CallStatus_Select(null).ToList();
                ViewBag.CallStatusListUDWBranch = result9;

                var result10 = _context.usp_AuditInsureStatus_Select(null).ToList();
                ViewBag.AuditInsureStatus = result10;

                var result11 = _context.usp_AuditInsureConsiderStatus_Select(null).ToList();
                ViewBag.AuditInsureConsiderStatus = result11;

                var result12 = _context.usp_AuditUnderwriteStatus_Select(null).ToList();
                ViewBag.AuditUnderwriteStatus = result12;

                var result13 = _context.usp_AuditCobrandGivenStatus_Select(null).ToList();
                ViewBag.AuditCobrandGivenStatus = result13;

                var result14 = _context.usp_SaleMethodType_Select(null, null).ToList();
                ViewBag.SaleMethodType = result14;

                var result15 = _context.usp_SaleServiceType_Select(null, null).ToList();
                ViewBag.SaleServiceType = result15;

                var result16 = _context.usp_InsuranceCardType_Select(null, null).ToList();
                ViewBag.InsuranceCardType = result16;

                var result17 = _context.usp_ReceivingManualType_Select(null, null).ToList();
                ViewBag.ReceivingManualType = result17;

                var result18 = _context.usp_UnderwritingBehaviorType_Select(null, null).ToList();
                ViewBag.UnderwritingBehaviorType = result18;

                var result19 = _context.usp_NotAuditedCause_Select(null, null).ToList();
                ViewBag.NotAuditedCause = result19;

                var result20 = _context.usp_AuditInsureNotConsideredVerifiedType_Select(null, null).ToList();
                ViewBag.AuditInsureNotConsideredVerifiedType = result20;

                var result21 = _context.usp_AuditInsureNotConsideredResultStatus_Select(null, null).ToList();
                ViewBag.AuditInsureNotConsideredResultStatus = result21;


                //DOCUMENT
                var DocumentClient = new SSSDocService.DocumentServiceClient();

                var mergDoclink = new List<Models.Document>();

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

                DocumentClient.Close();

                return View();
            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
            }
        }


        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure,UnderwriteAudit_AdminInsure,UnderwriteAudit_ConditionInsure")]
        public ActionResult AuditView(string queueAuditId)
        {
            var encodeId = queueAuditId;
            if (encodeId != null && encodeId != string.Empty)
            {
                ViewBag.ViewMode = true;

                //decode
                byte[] data = Convert.FromBase64String(encodeId);
                string decodedString = Encoding.UTF8.GetString(data);

                //call stored procedure
                var result = _context.usp_QueueAuditFullDetailByQueueAuditId_Select(Convert.ToInt32(decodedString)).FirstOrDefault();

                //ถ้า null return notfouond
                if (result is null) return RedirectToAction("NotFound", "Error");

                //Get Role
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var accessRole = GlobalFunction.GetAccessRoleUnderWriteAudit(roleList);
                ViewBag.AccessRole = accessRole;

                //set app code
                ViewBag.AppCode = result.ApplicationCode;

                var OpenSSS = Properties.Settings.Default.SSSpath;
                ViewBag.OpenSSS = OpenSSS;

                //1 = na ให้ set default เป็น 2
                ViewBag.QueueAuditStatusId = (result.QueueAuditStatusId == 1 ? 2 : result.QueueAuditStatusId);

                ViewBag.QueueFullDetail = result;
                var applicationCode = result.ApplicationCode;

                var result2 = _context.usp_QueueAuditStatus_Select(null).ToList();
                ViewBag.QueueAuditStatus = result2;

                var result3 = _context.usp_CallStatus_Select(null).ToList();
                ViewBag.CallStatusList = result3;

                var result4 = _context.usp_AuditUnderwriteResultStatus_Select(null).ToList();
                ViewBag.AuditUnderwriteResultStatus = result4;

                var result5 = _context.usp_AuditHealthStatus_Select(null).ToList();
                ViewBag.AuditHealthStatus = result5;

                var result6 = _context.usp_AuditHealthSpecifyStatus_Select(null).ToList();
                ViewBag.AuditHealthSpecifyStatus = result6;

                var result7 = _contextUDWBranch.usp_UnderwriteType_Select(null).ToList();
                ViewBag.UnderwriteType = result7;

                var result8 = _contextUDWBranch.usp_UnderwritePaymentType_Select(null).ToList();
                ViewBag.UnderwritePaymentType = result8;

                var result9 = _contextUDWBranch.usp_CallStatus_Select(null).ToList();
                ViewBag.CallStatusListUDWBranch = result9;

                var result10 = _context.usp_AuditInsureStatus_Select(null).ToList();
                ViewBag.AuditInsureStatus = result10;

                var result11 = _context.usp_AuditInsureConsiderStatus_Select(null).ToList();
                ViewBag.AuditInsureConsiderStatus = result11;

                var result12 = _context.usp_AuditUnderwriteStatus_Select(null).ToList();
                ViewBag.AuditUnderwriteStatus = result12;

                var result13 = _context.usp_AuditCobrandGivenStatus_Select(null).ToList();
                ViewBag.AuditCobrandGivenStatus = result13;

                //DOCUMENT
                var DocumentClient = new SSSDocService.DocumentServiceClient();

                var mergDoclink = new List<Models.Document>();

                if (applicationCode != "")
                {
                    //get smile doc
                    var linkSmileDoc = DocumentClient.GetDocumentV2SmileDoc(applicationCode);
                    //get sss doc`
                    var linkSSSDoc = DocumentClient.GetDocumentV2(applicationCode);

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

                DocumentClient.Close();

                return View();
            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
            }
        }


        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure,UnderwriteAudit_AdminInsure,UnderwriteAudit_ConditionInsure")]
        public ActionResult AuditViewV2(string queueAuditId, int? insuredChack)
        {
            var encodeId = queueAuditId;
            if (encodeId != null && encodeId != string.Empty)
            {
                ViewBag.ViewMode = true;

                //decode
                byte[] data = Convert.FromBase64String(encodeId);
                string decodedString = Encoding.UTF8.GetString(data);

                //call stored procedure
                var result = _context.usp_QueueAuditFullDetailByQueueAuditIdV2_Select(Convert.ToInt32(decodedString)).FirstOrDefault();

                if (insuredChack == 1)
                {
                    ViewBag.insuredChack = true;
                }
                else
                {
                    ViewBag.insuredChack = false;
                }

                //ถ้า null return notfouond
                if (result is null) return RedirectToAction("AuditView", new { queueAuditId = queueAuditId });
                var startNewVersionPH = Properties.Settings.Default.StartNewVersionPH;
                DateTime? oldPeriod = Convert.ToDateTime(startNewVersionPH);
                DateTime? nPeriod = Convert.ToDateTime(result.AppStartCoverDate);
                if (oldPeriod > nPeriod)
                {
                    return RedirectToAction("AuditView", new { queueAuditId = queueAuditId });
                }
                if (result.IsLineOADetail == null)
                {
                    if (queueAuditId != null && queueAuditId != "")
                    {
                        return RedirectToAction("AuditView", new { queueAuditId = queueAuditId });
                    }
                    else
                    {
                        return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
                    }

                }

                //Get Role
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var accessRole = GlobalFunction.GetAccessRoleUnderWriteAudit(roleList);
                ViewBag.AccessRole = accessRole;

                //set app code
                ViewBag.AppCode = result.ApplicationCode;

                var OpenSSS = Properties.Settings.Default.SSSpath;
                ViewBag.OpenSSS = OpenSSS;

                //1 = na ให้ set default เป็น 2
                ViewBag.QueueAuditStatusId = (result.QueueAuditStatusId == 1 ? 2 : result.QueueAuditStatusId);

                ViewBag.QueueFullDetail = result;
                var applicationCode = result.ApplicationCode;

                var result2 = _context.usp_QueueAuditStatus_Select(null).ToList();
                ViewBag.QueueAuditStatus = result2;

                var result3 = _context.usp_CallStatus_Select(null).ToList();
                ViewBag.CallStatusList = result3;

                var result4 = _context.usp_AuditUnderwriteResultStatus_Select(null).ToList();
                ViewBag.AuditUnderwriteResultStatus = result4;

                var result5 = _context.usp_AuditHealthStatus_Select(null).ToList();
                ViewBag.AuditHealthStatus = result5;

                var result6 = _context.usp_AuditHealthSpecifyStatus_Select(null).ToList();
                ViewBag.AuditHealthSpecifyStatus = result6;

                var result7 = _contextUDWBranch.usp_UnderwriteType_Select(null).ToList();
                ViewBag.UnderwriteType = result7;

                var result8 = _contextUDWBranch.usp_UnderwritePaymentType_Select(null).ToList();
                ViewBag.UnderwritePaymentType = result8;

                var result9 = _contextUDWBranch.usp_CallStatus_Select(null).ToList();
                ViewBag.CallStatusListUDWBranch = result9;

                var result10 = _context.usp_AuditInsureStatus_Select(null).ToList();
                ViewBag.AuditInsureStatus = result10;

                var result11 = _context.usp_AuditInsureConsiderStatus_Select(null).ToList();
                ViewBag.AuditInsureConsiderStatus = result11;

                var result12 = _context.usp_AuditUnderwriteStatus_Select(null).ToList();
                ViewBag.AuditUnderwriteStatus = result12;

                var result13 = _context.usp_AuditCobrandGivenStatus_Select(null).ToList();
                ViewBag.AuditCobrandGivenStatus = result13;

                var result14 = _context.usp_SaleMethodType_Select(null, null).ToList();
                ViewBag.SaleMethodType = result14;

                var result15 = _context.usp_SaleServiceType_Select(null, null).ToList();
                ViewBag.SaleServiceType = result15;

                var result16 = _context.usp_InsuranceCardType_Select(null, null).ToList();
                ViewBag.InsuranceCardType = result16;

                var result17 = _context.usp_ReceivingManualType_Select(null, null).ToList();
                ViewBag.ReceivingManualType = result17;

                var result18 = _context.usp_UnderwritingBehaviorType_Select(null, null).ToList();
                ViewBag.UnderwritingBehaviorType = result18;

                var result19 = _context.usp_NotAuditedCause_Select(null, null).ToList();
                ViewBag.NotAuditedCause = result19;

                var result20 = _context.usp_AuditInsureNotConsideredVerifiedType_Select(null, null).ToList();
                ViewBag.AuditInsureNotConsideredVerifiedType = result20;

                var result21 = _context.usp_AuditInsureNotConsideredResultStatus_Select(null, null).ToList();
                ViewBag.AuditInsureNotConsideredResultStatus = result21;
                //DOCUMENT
                var DocumentClient = new SSSDocService.DocumentServiceClient();

                var mergDoclink = new List<Models.Document>();

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

                DocumentClient.Close();

                return View();
            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
            }
        }
        #endregion View

        #region method

        [HttpPost]
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure,UnderwriteAudit_AdminInsure,UnderwriteAudit_ConditionInsure")]
        public ActionResult QuestionAuditUpdate(FormCollection form)
        {
            //สถานะการตรวจสอบ
            var queueAuditStatus = form["QueueAuditStatusId"];

            if (!string.IsNullOrEmpty(queueAuditStatus))
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                //queue audit id
                var QueueAuditId = form["QueueAuditId"];
                //สถานการโทร
                var CallStatus = form["CallStatus"];

                //ผลการตรวจสอบ ผอ.บล.
                //var AuditUnderwriteResultStatus = form["AuditUnderwriteResultStatus"];
                var AuditUnderwriteResultStatusRemark = form["AuditUnderwriteResultStatusRemark"];

                //ผลการตรวจสอบการคัดกรอง
                var AuditUnderwriteStatus = form["AuditUnderwriteStatus"];

                //ผลการตรวจสอบการมอบบัตร
                var AuditCobrandGivenStatus = form["AuditCobrandGivenStatus"];

                //การตรวจสอบด้านสุขภาพ
                var AuditHealthStatus = form["AuditHealthStatusId"];
                var AuditHealthRemark = form["AuditHealthRemark"];
                //รอพิจารณา -> ระบุในใบสมัคร-ไม่ระบุ
                var AuditHealthSpecifyStatus = form["AuditHealthSpecifyStatus"];
                var AuditHealthSpecifyRemark = form["AuditHealthSpecifyRemark"];
                //หมายเหตุ
                var remark = form["remark"];

                //DECLARE
                int? userId = Convert.ToInt32(user);
                int? queueAuditStatusId = Convert.ToInt32(queueAuditStatus);
                int? queueAuditId = Convert.ToInt32(QueueAuditId);

                // 1 = na ค่า ตั้งต้น
                int? callStatusId = 1;
                int? auditUnderwriteResultStatusId = 1;
                int? auditHealthStatusId = 1;
                int? auditHealthSpecifyStatusId = 1;
                string auditRemark = null;
                string auditUnderwriteResultRemark = null;
                string auditHealthRemark = null;
                int? auditUnderwriteStatusId = 1;
                int? auditCobrandGivenStatusId = 1;

                switch (queueAuditStatusId)
                {
                    //ยังไม่ได้ตรวจสอบ
                    case 2:
                        //สถานะการโทร
                        callStatusId = Convert.ToInt32(CallStatus);
                        //หมายเหตุ *หลัก
                        auditRemark = remark;
                        break;

                    //ตรวจสอบแล้ว
                    case 3:
                        //สถานะการโทร - โทรแล้ว
                        callStatusId = 5;
                        //ผลการตรวจสอบ ผอ.บล.
                        //auditUnderwriteResultStatusId = Convert.ToInt32(AuditUnderwriteResultStatus);
                        //หมายเหตุ ผลการตรวจสอบ ผอ.บล.
                        auditUnderwriteResultRemark = AuditUnderwriteResultStatusRemark;

                        //ผลการตรวจสอบการคัดกรอง
                        auditUnderwriteStatusId = Convert.ToInt32(AuditUnderwriteStatus);
                        //ผลการตรวจสอบการมอบบัตร
                        auditCobrandGivenStatusId = Convert.ToInt32(AuditCobrandGivenStatus);

                        //ผลการตรวจสอบ ด้านสุขภาพ
                        auditHealthStatusId = Convert.ToInt32(AuditHealthStatus);
                        //เช็ค id ผลการตรวจสอบสุขภาพ
                        switch (auditHealthStatusId)
                        {
                            //ผ่าน
                            case 2:
                                //หมายเหตุ-การตรวจสอบด้านสุขภาพ
                                auditHealthRemark = AuditHealthRemark;
                                break;
                            //รอพิจารณา
                            case 3:
                                //รอพิจารณา -> ระบุในใบสมัคร-ไม่ระบุ
                                auditHealthSpecifyStatusId = Convert.ToInt32(AuditHealthSpecifyStatus);
                                //เลือก รอพิจารณา ให้ระบุข้อยกเว้น ใน หมายเหตุ-การตรวจสอบด้านสุขภาพ
                                auditHealthRemark = AuditHealthSpecifyRemark;
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }

                var result = _context.usp_QueueAudit1Result_Insert(queueAuditId,
                                                                   queueAuditStatusId,
                                                                   callStatusId,
                                                                   auditRemark,
                                                                   auditUnderwriteResultStatusId,
                                                                   auditUnderwriteResultRemark,
                                                                   auditHealthStatusId,
                                                                   auditHealthSpecifyStatusId,
                                                                   auditHealthRemark,
                                                                   userId,
                                                                   auditUnderwriteStatusId,
                                                                   auditCobrandGivenStatusId).FirstOrDefault();

                //singnal R
                var chat = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                chat.Clients.Group(userId.ToString()).ReceiveGroupNotice("Success");

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //RETURN LIST
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        private bool? QuestionAuditUpdateV2CheckBoolType(string value)
        {
            switch (value)
            {
                case "0":
                    // ไม่มี/ไม่ถูกต้อง
                    return false;
                case "1":
                    // มี/ถูกต้อง
                    return true;
                case "2":
                default:
                    // จำไม่ได้/null
                    return null;
            }
        }

        [HttpPost]
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure,UnderwriteAudit_AdminInsure,UnderwriteAudit_ConditionInsure")]
        public ActionResult QuestionAuditUpdateV2(FormCollection form)
        {
            //สถานะการตรวจสอบ
            var queueAuditStatus = form["queueAuditStatusId"];

            if (!string.IsNullOrEmpty(queueAuditStatus))
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                var QueueAuditId = form["queueAuditId"];

                /// ยังไม่ได้ตรวจสอบ
                var CallStatusId = form["callStatusId"];
                var NotAuditCauseId = form["notAuditedCause"];          //สาเหตุ
                var AuditRemarkCallStatus = form["auditRemarkCallStatusId"];
                var AuditRemarkNotAuditedCause = form["auditRemarkNotAuditedCause"]; //หมายเหตุ

                /// ตรวจสอบแล้ว
                // การนำเสนอขาย และบริการหลังการขาย
                // วิธีการขาย
                var SaleMethodTypeId = form["saleMethodType"];
                var SaleMethodTypeRemark = form["saleMethodTypeRemark"];
                // บริการหลังการขาย
                var SaleServiceTypeId = form["saleServiceType"];
                var SaleServiceTypeRemark = form["saleServiceTypeRemark"];
                // มอบบัตรให้กับ
                var CobrandInsuredIsValid = form["cobrandInsuredIsValid"];
                var CobrandInsuredRemark = form["cobrandInsuredRemark"];
                // ประเภทบัตร
                var LINEOAIsValid = form["lineOAIsValid"];
                var LINEOARemark = form["lineOARemark"];
                // ข้อมูล ผอ.บล
                var UnderwriteByUserId = form["underwriteByUserId"];
                var UnderwriteByUserRemark = form["underwriteByUserRemark"];
                // เพิ่มเพื่อนกับสยามสไมล์ทาง line OA
                var AddLINEIsSuccess = form["addLINEIsSuccess"];
                // รูปแนบบัตรประกัน
                var InsuranceCardTypeId = form["insuranceCardType"];
                // คู่มือ
                var IsReceivedManual = form["isReceivedManual"];
                var ReceivingManualTypeId = form["receivingManualTypeId"];
                // คะแนนความผึงพอใจ
                var FeedbackRate = form["feedbackRate"];
                var FeedbackRemark = form["feedbackRemark"];
                // ผลด้านบริการ
                var ServiceResultIsIssue = form["serviceResultIsIssue"];
                var ServiceResultRemark = form["serviceResultRemark"];
                // พฤติกรรมการคัดกรอง(MA)
                var UnderwritingBehaviorTypeId = form["underwritingBehaviorTypeId"];
                var UnderwritingBehaviorRemark = form["underwritingBehaviorRemark"];

                // ยืนยันข้อมูลส่วนบุคคล
                // ชื่อ-สกุล ผู้เอาประกัน
                var InsuredNameIsValid = form["insuredNameIsValid"];
                var InsuredNameRemark = form["insuredNameRemark"];
                // ชื่อ-สกุล ผู้ชำระเบี้ย
                var PayerNameIsValid = form["payerNameIsValid"];
                var PayerNameRemark = form["payerNameRemark"];
                // วันเดือนปีเกิด
                var BirthDateIsValid = form["birthDateIsValid"];
                var BirthDateRemark = form["birthDateRemark"];
                // น้ำหนักส่วนสูง
                var WeightHeightIsValid = form["weightHeightIsValid"];
                var WeightHeightRemark = form["weightHeightRemark"];

                // ประวัติสุขภาพ
                // ประวัติการแพ้
                var IsAllergyHistory = form["isAllergyHistory"];
                var AllergyHistoryRemark = form["allergyHistoryRemark"];
                // ประวัติการรักษาทั่วไป
                var IsMedicalHistory = form["isMedicalHistory"];
                var MedicalHistoryRemark = form["medicalHistoryRemark"];
                // ประวัติอุบัติเหตุ
                var IsAccidentHistory = form["isAccidentHistory"];
                var AccidentHistoryRemark = form["accidentHistoryRemark"];
                // ประวัติการผ่าตัด
                var IsSurgeryHistory = form["isSurgeryHistory"];
                var SurgeryHistoryRemark = form["surgeryHistoryRemark"];
                // ประวัติการตรวจสุขภาพประจำปี
                var IsHealthCheckHistory = form["isHealthCheckHistory"];
                var HealthCheckHistoryRemark = form["healthCheckHistoryRemark"];
                // ประวัติโรคเรื้อรัง
                var IsChronicDiseaseHistory = form["isChronicDiseaseHistory"];
                var ChronicDiseaseHistoryRemark = form["chronicDiseaseHistoryRemark"];
                // ประวัติการรักษาครั้งล่าสุด
                var IsMedicalLatestHistory = form["isMedicalLatestHistory"];
                var MedicalLatestHistoryRemark = form["medicalLatestHistoryRemark"];
                // ประวัติการสูบบุหรี่
                var IsSmokingHistory = form["isSmokingHistory"];
                var SmokingHistoryRemark = form["smokingHistoryRemark"];
                // ประวิติการดื่มสุรา
                var IsDrinkingHistory = form["isDrinkingHistory"];
                var DrinkingHistoryRemark = form["DrinkingHistoryRemark"];
                // ประวัติการตั้งครรภ์(เพศหญิง)
                var IsPregnantHistory = form["isPregnantHistory"];
                var PregnantHistoryRemark = form["pregnantHistoryRemark"];
                // ประวัติสุขภาพอื่นๆ(เพิ่มเติม)
                var IsOtherHealthHistory = form["isOtherHealthHistory"];
                var OtherHealthHistoryRemark = form["otherHealthHistoryRemark"];

                // ข้อมูลการชำระเบี้ยประกัน
                // อาชีพผู้เอาประกัน
                var PayerOccupationIsValid = form["payerOccupationIsValid"];
                var PayerOccupationRemark = form["payerOccupationRemark"];
                // หน่วยงานผู้ชำระเบี้ย
                var PayerBuildingNameIsValid = form["payerBuildingNameIsValid"];
                var PayerBuildingNameRemark = form["payerBuildingNameRemark"];
                // แผนประกัน
                var ProductIsValid = form["productIsValid"];
                var ProductRemark = form["productRemark"];
                // เบี้ยประกัน
                var PremiumIsValid = form["premiumIsValid"];
                var PremiumRemark = form["premiumRemark"];
                // ประเภทการชำระเบี้ยประกัน
                var PeriodTypeIsValid = form["periodTypeIsValid"];
                var PeriodTypeRemark = form["periodTypeRemark"];
                //ช่องทางการชำระเบี้ยประกัน
                var PayMethodTypeIsValid = form["payMethodTypeIsValid"];
                var PayMethodTypeRemark = form["payMethodTypeRemark"];

                // การตรวจสอบด้านสุขภาพ
                // ผลการตรวจสอบ
                var AuditHealthStatusId = form["auditHealthStatusId"];
                var AuditHealthSpecifyStatusId = form["auditHealthSpecifyStatusId"];
                var AuditHealthRemark = form["auditHealthRemark"];                      // หมายเหตุประวัตการคัดกรอง
                // หมายเหตุอื่นๆ
                var AuditOtherHealthRemark = form["auditOtherHealthRemark"];

                //DECLARE
                int? userId = Convert.ToInt32(user);
                int? queueAuditId = Convert.ToInt32(QueueAuditId);
                int? queueAuditStatusId = Convert.ToInt32(queueAuditStatus);
                int? callStatusId = Convert.ToInt32(CallStatusId);
                int? notAuditCauseId = Convert.ToInt32(NotAuditCauseId);
                int? saleMethodTypeId = Convert.ToInt32(SaleMethodTypeId);
                int? saleServiceTypeId = Convert.ToInt32(SaleServiceTypeId);
                int? underwriteByUserId = Convert.ToInt32(UnderwriteByUserId);
                int? insuranceCardTypeId = Convert.ToInt32(InsuranceCardTypeId);
                int? receivingManualTypeId = 1;
                int? feedbackRate = Convert.ToInt32(FeedbackRate);
                int? underwritingBehaviorTypeId = Convert.ToInt32(UnderwritingBehaviorTypeId);
                int? auditHealthStatusId = 1;
                int? auditHealthSpecifyStatusId = 1;

                bool? cobrandInsuredIsValid = null;
                bool? lINEOAIsValid = null;
                bool? addLINEIsSuccess = null;
                bool? isReceivedManual = null;
                bool? serviceResultIsIssue = null;
                bool? insuredNameIsValid = null;
                bool? payerNameIsValid = null;
                bool? birthDateIsValid = null;
                bool? weightHeightIsValid = null;
                bool? isAllergyHistory = null;
                bool? isMedicalHistory = null;
                bool? isAccidentHistory = null;
                bool? isSurgeryHistory = null;
                bool? isHealthCheckHistory = null;
                bool? isChronicDiseaseHistory = null;
                bool? isMedicalLatestHistory = null;
                bool? isSmokingHistory = null;
                bool? isDrinkingHistory = null;
                bool? isPregnantHistory = null;
                bool? isOtherHealthHistory = null;
                bool? payerOccupationIsValid = null;
                bool? payerBuildingNameIsValid = null;
                bool? productIsValid = null;
                bool? premiumIsValid = null;
                bool? periodTypeIsValid = null;
                bool? payMethodTypeIsValid = null;

                string auditRemark = null;
                string saleMethodTypeRemark = null;
                string saleServiceTypeRemark = null;
                string cobrandInsuredRemark = null;
                string lINEOARemark = null;
                string underwriteByUserRemark = null;
                string feedbackRemark = null;
                string serviceResultRemark = null;
                string insuredNameRemark = null;
                string payerNameRemark = null;
                string birthDateRemark = null;
                string weightHeightRemark = null;
                string allergyHistoryRemark = null;
                string medicalHistoryRemark = null;
                string accidentHistoryRemark = null;
                string surgeryHistoryRemark = null;
                string healthCheckHistoryRemark = null;
                string chronicDiseaseHistoryRemark = null;
                string medicalLatestHistoryRemark = null;
                string smokingHistoryRemark = null;
                string drinkingHistoryRemark = null;
                string pregnantHistoryRemark = null;
                string otherHealthHistoryRemark = null;
                string payerOccupationRemark = null;
                string payerBuildingNameRemark = null;
                string productRemark = null;
                string premiumRemark = null;
                string periodTypeRemark = null;
                string payMethodTypeRemark = null;
                string auditHealthRemark = null;
                string auditOtherHealthRemark = null;
                string underwritingBehaviorRemark = null;

                switch (queueAuditStatusId)
                {
                    //ยังไม่ได้ตรวจสอบ
                    case 2:
                        //สถานะการโทร
                        callStatusId = Convert.ToInt32(CallStatusId);
                        // สาเหตุ
                        // notAuditCauseId = Convert.ToInt32(NotAuditCauseId);
                        // หมายเหตุ
                        auditRemark = AuditRemarkCallStatus.Trim() == "" ? auditRemark : AuditRemarkCallStatus;
                        //ไม่ต้องตรวจสอบ masrter = 1 เพราะไม่ได้เลือก
                        receivingManualTypeId = 1; //n/a
                        saleMethodTypeId = 1; //n/a
                        saleServiceTypeId = 1; //n/a
                        insuranceCardTypeId = 1; //n/a
                        underwritingBehaviorTypeId = 1;
                        feedbackRate = 0;
                        break;

                    //ไม่ต้องตรวจสอบ
                    case 4:
                        //สถานะการโทร
                        //callStatusId = Convert.ToInt32(CallStatusId);
                        // สาเหตุ
                        notAuditCauseId = Convert.ToInt32(NotAuditCauseId);
                        // หมายเหตุ
                        auditRemark = AuditRemarkNotAuditedCause.Trim() == "" ? auditRemark : AuditRemarkNotAuditedCause;

                        //ไม่ต้องตรวจสอบ masrter = 1 เพราะไม่ได้เลือก
                        receivingManualTypeId = 1; //n/a
                        saleMethodTypeId = 1; //n/a
                        saleServiceTypeId = 1; //n/a
                        insuranceCardTypeId = 1; //n/a
                        underwritingBehaviorTypeId = 1;
                        feedbackRate = 0;
                        break;

                    //ตรวจสอบแล้ว
                    case 3:
                        //สถานะการโทร - โทรแล้ว
                        callStatusId = 5;

                        // หมายเหตุเพิ่มเติม
                        saleMethodTypeRemark = SaleMethodTypeRemark.Trim() == "" ? saleMethodTypeRemark : SaleMethodTypeRemark;
                        saleServiceTypeRemark = SaleServiceTypeRemark.Trim() == "" ? saleServiceTypeRemark : SaleServiceTypeRemark;
                        cobrandInsuredRemark = CobrandInsuredRemark.Trim() == "" ? cobrandInsuredRemark : CobrandInsuredRemark;
                        lINEOARemark = LINEOARemark.Trim() == "" ? lINEOARemark : LINEOARemark;
                        underwriteByUserRemark = UnderwriteByUserRemark.Trim() == "" ? underwriteByUserRemark : UnderwriteByUserRemark;
                        feedbackRemark = FeedbackRemark.Trim() == "" ? feedbackRemark : FeedbackRemark;
                        serviceResultRemark = ServiceResultRemark.Trim() == "" ? serviceResultRemark : ServiceResultRemark;
                        insuredNameRemark = InsuredNameRemark.Trim() == "" ? insuredNameRemark : InsuredNameRemark;
                        payerNameRemark = PayerNameRemark.Trim() == "" ? payerNameRemark : PayerNameRemark;
                        birthDateRemark = BirthDateRemark.Trim() == "" ? birthDateRemark : BirthDateRemark;
                        weightHeightRemark = WeightHeightRemark.Trim() == "" ? weightHeightRemark : WeightHeightRemark;
                        allergyHistoryRemark = AllergyHistoryRemark.Trim() == "" ? allergyHistoryRemark : AllergyHistoryRemark;
                        medicalHistoryRemark = MedicalHistoryRemark.Trim() == "" ? medicalHistoryRemark : MedicalHistoryRemark;
                        accidentHistoryRemark = AccidentHistoryRemark.Trim() == "" ? accidentHistoryRemark : AccidentHistoryRemark;
                        surgeryHistoryRemark = SurgeryHistoryRemark.Trim() == "" ? surgeryHistoryRemark : SurgeryHistoryRemark;
                        healthCheckHistoryRemark = HealthCheckHistoryRemark.Trim() == "" ? healthCheckHistoryRemark : HealthCheckHistoryRemark;
                        chronicDiseaseHistoryRemark = ChronicDiseaseHistoryRemark.Trim() == "" ? chronicDiseaseHistoryRemark : ChronicDiseaseHistoryRemark;
                        medicalLatestHistoryRemark = MedicalLatestHistoryRemark.Trim() == "" ? medicalLatestHistoryRemark : MedicalLatestHistoryRemark;
                        smokingHistoryRemark = SmokingHistoryRemark.Trim() == "" ? smokingHistoryRemark : SmokingHistoryRemark;
                        drinkingHistoryRemark = DrinkingHistoryRemark.Trim() == "" ? drinkingHistoryRemark : DrinkingHistoryRemark;
                        pregnantHistoryRemark = PregnantHistoryRemark.Trim() == "" ? pregnantHistoryRemark : PregnantHistoryRemark;
                        otherHealthHistoryRemark = OtherHealthHistoryRemark.Trim() == "" ? otherHealthHistoryRemark : OtherHealthHistoryRemark;
                        payerOccupationRemark = PayerOccupationRemark.Trim() == "" ? payerOccupationRemark : PayerOccupationRemark;
                        payerBuildingNameRemark = PayerBuildingNameRemark.Trim() == "" ? payerBuildingNameRemark : PayerBuildingNameRemark;
                        productRemark = ProductRemark.Trim() == "" ? productRemark : ProductRemark;
                        premiumRemark = PremiumRemark.Trim() == "" ? premiumRemark : PremiumRemark;
                        periodTypeRemark = PeriodTypeRemark.Trim() == "" ? periodTypeRemark : PeriodTypeRemark;
                        payMethodTypeRemark = PayMethodTypeRemark.Trim() == "" ? payMethodTypeRemark : PayMethodTypeRemark;
                        auditOtherHealthRemark = AuditOtherHealthRemark.Trim() == "" ? auditOtherHealthRemark : AuditOtherHealthRemark;
                        underwritingBehaviorRemark = UnderwritingBehaviorRemark.Trim() == "" ? underwritingBehaviorRemark : UnderwritingBehaviorRemark;


                        // Radio box
                        cobrandInsuredIsValid = QuestionAuditUpdateV2CheckBoolType(CobrandInsuredIsValid);
                        lINEOAIsValid = QuestionAuditUpdateV2CheckBoolType(LINEOAIsValid);
                        addLINEIsSuccess = QuestionAuditUpdateV2CheckBoolType(AddLINEIsSuccess);
                        serviceResultIsIssue = QuestionAuditUpdateV2CheckBoolType(ServiceResultIsIssue);
                        insuredNameIsValid = QuestionAuditUpdateV2CheckBoolType(InsuredNameIsValid);
                        payerNameIsValid = QuestionAuditUpdateV2CheckBoolType(PayerNameIsValid);
                        birthDateIsValid = QuestionAuditUpdateV2CheckBoolType(BirthDateIsValid);
                        weightHeightIsValid = QuestionAuditUpdateV2CheckBoolType(WeightHeightIsValid);
                        isAllergyHistory = QuestionAuditUpdateV2CheckBoolType(IsAllergyHistory);
                        isMedicalHistory = QuestionAuditUpdateV2CheckBoolType(IsMedicalHistory);
                        isAccidentHistory = QuestionAuditUpdateV2CheckBoolType(IsAccidentHistory);
                        isSurgeryHistory = QuestionAuditUpdateV2CheckBoolType(IsSurgeryHistory);
                        isHealthCheckHistory = QuestionAuditUpdateV2CheckBoolType(IsHealthCheckHistory);
                        isChronicDiseaseHistory = QuestionAuditUpdateV2CheckBoolType(IsChronicDiseaseHistory);
                        isMedicalLatestHistory = QuestionAuditUpdateV2CheckBoolType(IsMedicalLatestHistory);
                        isSmokingHistory = QuestionAuditUpdateV2CheckBoolType(IsSmokingHistory);
                        isDrinkingHistory = QuestionAuditUpdateV2CheckBoolType(IsDrinkingHistory);
                        isPregnantHistory = QuestionAuditUpdateV2CheckBoolType(IsPregnantHistory);
                        isOtherHealthHistory = QuestionAuditUpdateV2CheckBoolType(IsOtherHealthHistory);
                        payerOccupationIsValid = QuestionAuditUpdateV2CheckBoolType(PayerOccupationIsValid);
                        payerBuildingNameIsValid = QuestionAuditUpdateV2CheckBoolType(PayerBuildingNameIsValid);
                        productIsValid = QuestionAuditUpdateV2CheckBoolType(ProductIsValid);
                        premiumIsValid = QuestionAuditUpdateV2CheckBoolType(PremiumIsValid);
                        periodTypeIsValid = QuestionAuditUpdateV2CheckBoolType(PeriodTypeIsValid);
                        payMethodTypeIsValid = QuestionAuditUpdateV2CheckBoolType(PayMethodTypeIsValid);

                        //// การนำเสนอขาย และบริการหลังการขาย
                        /// คู่มือ
                        isReceivedManual = QuestionAuditUpdateV2CheckBoolType(IsReceivedManual);
                        // ได้รับคู่มือใช่หรือไม่
                        receivingManualTypeId = isReceivedManual != null && (bool)isReceivedManual ? Convert.ToInt32(ReceivingManualTypeId) : receivingManualTypeId; // default receivingManualTypeId = 1

                        //// ผลการตรวจสอบ ด้านสุขภาพ
                        auditHealthStatusId = Convert.ToInt32(AuditHealthStatusId);
                        //เช็ค id ผลการตรวจสอบสุขภาพ
                        switch (auditHealthStatusId)
                        {
                            //ผ่าน
                            case 2:
                                //หมายเหตุ-การตรวจสอบด้านสุขภาพ
                                auditHealthRemark = AuditHealthRemark.Trim() == "" ? auditHealthRemark : AuditHealthRemark;
                                break;
                            //รอพิจารณา
                            case 3:
                                //รอพิจารณา -> ระบุในใบสมัคร-ไม่ระบุ
                                auditHealthSpecifyStatusId = Convert.ToInt32(AuditHealthSpecifyStatusId);

                                //หมายเหตุ-การตรวจสอบด้านสุขภาพ
                                auditHealthRemark = AuditHealthRemark.Trim() == "" ? auditHealthRemark : AuditHealthRemark;
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }

                var result = _context.usp_QueueAudit1ResultV2_Insert(queueAuditId,
                                                                    queueAuditStatusId,
                                                                    notAuditCauseId == -1 ? 1 : notAuditCauseId,
                                                                    callStatusId == -1 ? 1 : callStatusId,
                                                                    auditRemark,
                                                                    saleMethodTypeId,
                                                                    saleMethodTypeRemark,
                                                                    saleServiceTypeId,
                                                                    saleServiceTypeRemark,
                                                                    cobrandInsuredIsValid,
                                                                    cobrandInsuredRemark,
                                                                    lINEOAIsValid,
                                                                    lINEOARemark,
                                                                    underwriteByUserId,
                                                                    underwriteByUserRemark,
                                                                    addLINEIsSuccess,
                                                                    insuranceCardTypeId,
                                                                    isReceivedManual,
                                                                    receivingManualTypeId == -1 ? 1 : receivingManualTypeId,
                                                                    feedbackRate == 6 ? 0 : feedbackRate,
                                                                    feedbackRemark,
                                                                    serviceResultIsIssue,
                                                                    serviceResultRemark,
                                                                    underwritingBehaviorTypeId,
                                                                    underwritingBehaviorRemark,
                                                                    insuredNameIsValid,
                                                                    insuredNameRemark,
                                                                    payerNameIsValid,
                                                                    payerNameRemark,
                                                                    birthDateIsValid,
                                                                    birthDateRemark,
                                                                    weightHeightIsValid,
                                                                    weightHeightRemark,
                                                                    isAllergyHistory,
                                                                    allergyHistoryRemark,
                                                                    isMedicalHistory,
                                                                    medicalHistoryRemark,
                                                                    isAccidentHistory,
                                                                    accidentHistoryRemark,
                                                                    isSurgeryHistory,
                                                                    surgeryHistoryRemark,
                                                                    isHealthCheckHistory,
                                                                    healthCheckHistoryRemark,
                                                                    isChronicDiseaseHistory,
                                                                    chronicDiseaseHistoryRemark,
                                                                    isMedicalLatestHistory,
                                                                    medicalLatestHistoryRemark,
                                                                    isSmokingHistory,
                                                                    smokingHistoryRemark,
                                                                    isDrinkingHistory,
                                                                    drinkingHistoryRemark,
                                                                    isPregnantHistory,
                                                                    pregnantHistoryRemark,
                                                                    isOtherHealthHistory,
                                                                    otherHealthHistoryRemark,
                                                                    payerOccupationIsValid,
                                                                    payerOccupationRemark,
                                                                    payerBuildingNameIsValid,
                                                                    payerBuildingNameRemark,
                                                                    productIsValid,
                                                                    productRemark,
                                                                    premiumIsValid,
                                                                    premiumRemark,
                                                                    periodTypeIsValid,
                                                                    periodTypeRemark,
                                                                    payMethodTypeIsValid,
                                                                    payMethodTypeRemark,
                                                                    auditHealthStatusId,
                                                                    auditHealthSpecifyStatusId,
                                                                    auditHealthRemark,
                                                                    auditOtherHealthRemark,
                                                                    userId).FirstOrDefault();

                //singnal R
                var chat = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                chat.Clients.Group(userId.ToString()).ReceiveGroupNotice("Success");

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //RETURN LIST
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure,UnderwriteAudit_AdminInsure,UnderwriteAudit_ConditionInsure")]
        public ActionResult QuestionAuditInsureUpdate(FormCollection form)
        {
            //queue audit id
            var QueueAuditId = form["QueueAuditId_formInsure"];

            if (!string.IsNullOrEmpty(QueueAuditId))
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                //สถานะพิจารณา
                var AuditInsureStatus = form["AuditInsureStatus"];
                var AuditInsureStatusRemark = form["AuditInsureStatusRemark"];
                //พิจารณาแล้ว --> ผ่าน-ติดเงื่อนไข-ไม่ผ่าน
                var AuditInsureConsiderStatus = form["AuditInsureConsiderStatus"];
                var AuditInsureConsiderStatusRemark = form["AuditInsureConsiderStatusRemark"];

                //DECLARE
                int? userId = Convert.ToInt32(user);
                int? queueAuditId = Convert.ToInt32(QueueAuditId);
                int? auditInsureStatusId = Convert.ToInt32(AuditInsureStatus);

                // 1 = na ค่า ตั้งต้น
                int? auditInsureConsiderStatusId = 1;
                string auditInsureRemark = null;
                string auditInsureConsiderRemark = null;

                switch (auditInsureStatusId)
                {
                    //รอพิจารณา
                    case 2:
                        //หมายเหตุ
                        auditInsureRemark = AuditInsureStatusRemark;
                        break;
                    //พิจารณาแล้ว
                    case 3:
                        //ผลพิจารณา -ผ่าน -ไม่ผ่าน -ผ่านติดเงื่อนไข
                        auditInsureConsiderStatusId = Convert.ToInt32(AuditInsureConsiderStatus);
                        //หมายเหตุ
                        auditInsureConsiderRemark = AuditInsureConsiderStatusRemark;
                        break;

                    default:
                        break;
                }

                var result = _context.usp_QueueAudit2Result_Insert(queueAuditId, auditInsureStatusId, auditInsureRemark, auditInsureConsiderStatusId, auditInsureConsiderRemark, userId).FirstOrDefault();

                //singnal R
                var chat = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                chat.Clients.Group(userId.ToString()).ReceiveGroupNotice("Success");

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //RETURN LIST
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure,UnderwriteAudit_AdminInsure,UnderwriteAudit_ConditionInsure")]
        public ActionResult QuestionAuditInsureUpdateV2(FormCollection form)
        {
            //queue audit id
            var QueueAuditId = form["QueueAuditId_formInsure"];

            if (!string.IsNullOrEmpty(QueueAuditId))
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                //สถานะพิจารณา
                var AuditInsureStatus = form["AuditInsureStatus"];
                //พิจารณาแล้ว --> ผ่าน-ติดเงื่อนไข-ไม่ผ่าน
                var AuditInsureConsiderStatus = form["AuditInsureConsiderStatus"];
                var AuditInsureConsiderStatusRemark = form["AuditInsureConsiderStatusRemark"];
                var AuditInsureClose = form["AuditInsureClose"];
                var AuditInsureCloseRemark = form["AuditInsureCloseRemark"];
                //สถานะพิจารณา
                var AuditInsureStatus_close = form["AuditInsureStatus2"];
                //พิจารณาแล้ว --> ผ่าน-ติดเงื่อนไข-ไม่ผ่าน
                var AuditInsureConsiderStatus_close = form["AuditInsureConsiderStatus2"];
                var AuditInsureConsiderStatusRemark_close = form["AuditInsureConsiderStatusRemark2"];
                var AuditInsureNotConsideredResultStatusId = form["AuditInsureNotConsideredResultStatusId"];
                var AuditInsureNotConsideredVerifiedTypeId = form["AuditInsureNotConsideredVerifiedTypeId"];
                var AuditInsureNotConsideredRemark = form["AuditInsureNotConsideredRemark"];

                var insuredChack = form["insuredChack"];
                bool? InsuredChack = QuestionAuditUpdateV2CheckBoolType(insuredChack);

                //DECLARE
                int? userId = Convert.ToInt32(user);
                int? queueAuditId = Convert.ToInt32(QueueAuditId);
                int? auditInsureStatusId = Convert.ToInt32(AuditInsureStatus);
                int? auditInsureStatus_close = AuditInsureStatus_close != "" ? Convert.ToInt32(AuditInsureStatus_close) : 1;

                // 1 = na ค่า ตั้งต้น
                int? auditInsureConsiderStatusId = 1;
                int? auditInsureConsiderStatus_close = 1;
                string auditInsureConsiderRemark = AuditInsureConsiderStatusRemark;
                bool? auditInsureClose = QuestionAuditUpdateV2CheckBoolType(AuditInsureClose);
                string auditInsureCloseRemark = AuditInsureCloseRemark;
                bool? isSendMemo = null;
                int? auditInsureNotConsideredResultStatusId = null;
                int? auditInsureNotConsideredVerifiedTypeId = null;
                int? auditInsureNotConsideredRemark = null;


                // InsuredChack คือ param ที่เอาไว้เช็คหน้ารับประกัน 1 case พิจารณา กับ case ติดเงื่อนไข
                if (InsuredChack == true && auditInsureClose == true) //หน้าติดเงื่อนไข true && และปิดคิวงานเป็น true
                {
                    isSendMemo = null;
                }
                else if (InsuredChack == true && auditInsureClose == false)
                {
                    isSendMemo = false;
                }
                else
                {
                    isSendMemo = null;
                }

                switch (auditInsureStatusId)
                {
                    //พิจารณาแล้ว
                    case 3:
                        //ผลพิจารณา -ผ่าน -ไม่ผ่าน -ผ่านติดเงื่อนไข
                        auditInsureConsiderStatusId = Convert.ToInt32(AuditInsureConsiderStatus);
                        auditInsureConsiderStatus_close = AuditInsureConsiderStatus_close != "" ? Convert.ToInt32(AuditInsureConsiderStatus_close) : 1;
                        auditInsureNotConsideredResultStatusId = 2;
                        auditInsureNotConsideredVerifiedTypeId = null;
                        //หมายเหตุ
                        // auditInsureConsiderRemark = AuditInsureConsiderStatusRemark;
                        break;
                    //รอพิจารณา

                    default:
                        break;
                }
                if (InsuredChack == true)
                {
                    switch (auditInsureStatus_close)
                    {
                        //พิจารณาแล้ว
                        case 3:
                            //ผลพิจารณา -ผ่าน -ไม่ผ่าน -ผ่านติดเงื่อนไข
                            auditInsureConsiderStatusId = Convert.ToInt32(AuditInsureConsiderStatus);
                            auditInsureConsiderStatus_close = AuditInsureConsiderStatus_close != "" ? Convert.ToInt32(AuditInsureConsiderStatus_close) : 1;
                            auditInsureNotConsideredResultStatusId = Convert.ToInt32(AuditInsureNotConsideredResultStatusId);
                            auditInsureNotConsideredVerifiedTypeId = Convert.ToInt32(AuditInsureNotConsideredVerifiedTypeId);
                            //หมายเหตุ
                            // auditInsureConsiderRemark = AuditInsureConsiderStatusRemark;
                            break;
                        //รอพิจารณา

                        default:
                            break;
                    }
                }


                var result = _context.usp_QueueAudit2ResultV3_Insert(
                           queueAuditId,
                           InsuredChack == true ? auditInsureStatus_close : auditInsureStatusId,
                           InsuredChack == true ? auditInsureConsiderStatus_close : auditInsureConsiderStatusId,
                           InsuredChack == true ? AuditInsureConsiderStatusRemark_close : auditInsureConsiderRemark,
                           auditInsureNotConsideredResultStatusId == 0 ? 2 : auditInsureNotConsideredResultStatusId,
                           auditInsureNotConsideredVerifiedTypeId == 0 ? null : auditInsureNotConsideredVerifiedTypeId,
                           null,
                           auditInsureClose,
                           auditInsureCloseRemark,
                           userId,
                           isSendMemo).FirstOrDefault();

                //singnal R
                var chat = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                chat.Clients.Group(userId.ToString()).ReceiveGroupNotice("Success");

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //RETURN LIST
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpGet]
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure,UnderwriteAudit_AdminInsure,UnderwriteAudit_ConditionInsure")]
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

        [HttpGet]
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure,UnderwriteAudit_AdminInsure,UnderwriteAudit_ConditionInsure")]
        public ActionResult QueueAuditLog(int queueAuditId, int draw, int? indexStart, int? pageSize, string sortField, string orderType, string search)
        {
            var result = _context.usp_QueueAuditLog_Select(queueAuditId, indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                 {"draw", draw },
                 {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                 {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                 {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure,UnderwriteAudit_AdminInsure,UnderwriteAudit_ConditionInsure")]
        public ActionResult ApplicationInsuredRelateByApplicationCodeSelect(string applicationCode, int? indexStart, int? pageSize, string sortField, string orderType)
        {
            var result = _context.usp_ApplicationInsuredRelateByApplicationCode_Select(applicationCode, indexStart, pageSize, sortField, orderType).ToList();

            var dt = new Dictionary<string, object>
            {
                 {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                 {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                 {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion method
    }
}