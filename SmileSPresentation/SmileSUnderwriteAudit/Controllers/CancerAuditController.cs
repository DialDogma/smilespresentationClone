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
    public class CancerAuditController : Controller
    {
        #region Context

        private readonly UnderwriteCancerAuditEntities _context;
        private readonly UnderwriteCancerBranchV2Entities _contextCancer;

        public CancerAuditController()
        {
            _context = new UnderwriteCancerAuditEntities();
            _contextCancer = new UnderwriteCancerBranchV2Entities();
        }

        #endregion Context

        #region View

        // GET: CancerAudit
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure")]
        public ActionResult CancerAuditIndex(string cancerQueueAuditId)
        {
            var encodeId = cancerQueueAuditId;
            if (encodeId != null && encodeId != string.Empty)
            {
                //decode
                byte[] data = Convert.FromBase64String(encodeId);
                string decodedString = Encoding.UTF8.GetString(data);

                //call stored procedure
                var result = _context.usp_QueueCIAuditFullDetailByQueueAuditId_Select(Convert.ToInt32(decodedString)).FirstOrDefault();
                var OpenGoogleDoc = Properties.Settings.Default.GoogleDocPathCI;
                try
                {
                    var appCode = result.ApplicationCode;
                    var insureName = result.InsuredFirstName + " " + result.InsuredLastName;
                    var payerName = result.PayerFirstName + " " + result.PayerLastName;

                    string appStartCoverDateFormatted = result.Period.HasValue
                          ? result.Period.Value.ToString("d/M/yyyy", new System.Globalization.CultureInfo("th-TH"))
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
                //StartCoverDate
                if (result.StartCoverDate != null)
                {
                    ViewBag.StartCoverDate = string.Format("{0}/{1}/{2} {3}", result.StartCoverDate.Value.Day.ToString("00", null), result.StartCoverDate.Value.Month.ToString("00", null), result.StartCoverDate.Value.Year + 543, result.StartCoverDate.Value.ToString("HH:mm:ss"));
                }
                //วันเกิด เอาประกัน
                if (result.InsuredBirthDate != null)
                {
                    ViewBag.InsuredBirthDate = string.Format("{0}/{1}/{2}", result.InsuredBirthDate.Value.Day.ToString("00", null), result.InsuredBirthDate.Value.Month.ToString("00", null), result.InsuredBirthDate.Value.Year + 543);
                }

                //ถ้า null return notfouond
                if (result is null) return RedirectToAction("NotFound", "Error", new { Msg = "เลข App NotFound" });

                //Get Role
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var accessRole = GlobalFunction.GetAccessRoleUnderWriteAudit(roleList);
                /*ViewBag.AccessRole = roleList.ToList();*/
                var roleList2 = roleListRaw.Split(',').ToList();
                ViewBag.AccessRole = accessRole;
                ViewBag.AccessRole2 = roleList2;


                var OpenSSS = Properties.Settings.Default.SSSpath;
                ViewBag.OpenSSS = OpenSSS;

                //set app code
                ViewBag.AppCode = result.ApplicationCode;

                //1 = na ให้ set default เป็น 2
                ViewBag.QueueAuditStatusId = (result.QueueAuditStatusId == 1 ? 2 : result.QueueAuditStatusId);
                ViewBag.QueueCancerFullDetail = result;
                var applicationCode = result.ApplicationCode;

                var result2 = _context.usp_QueueCIAuditStatus_Select(null).ToList();
                ViewBag.QueueAuditStatus = result2;

                var result3 = _context.usp_CallStatusCI_Select(null).ToList();
                ViewBag.CallStatusListAuditCancer = result3;

                //var result4 = _context.usp_AuditUnderwriteResultStatus_Select(null).ToList();
                //ViewBag.AuditUnderwriteResultStatus = result4;

                var result5 = _contextCancer.usp_UnderwriteTypeCI_Select(null).ToList();
                ViewBag.UnderwriteTypeCI = result5;

                var result6 = _context.usp_AuditCISpecifyStatus_Select(null).ToList();
                ViewBag.AuditCancerSpecifyStatus = result6;

                var result7 = _contextCancer.usp_UnderwriteTypeCI_Select(null).ToList();
                ViewBag.UnderwriteType = result7;

                var result15 = _contextCancer.usp_UnderwritePaymentTypeCI_Select(null).ToList();
                ViewBag.UnderwritePaymentTypeCancer = result15;

                var result8 = _context.usp_AuditCIPaymentStatus_Select(null).ToList();
                ViewBag.AuditCancerPaymentType = result8;

                var result14 = _context.usp_AuditCIStatus_Select(null).ToList();
                ViewBag.AuditCancerStatus = result14;

                //var result9 = _contextCancer.usp_CallStatusCI_Select(null).ToList();
                //ViewBag.CallStatusListUDWBranch = result9;
                var result9 = new List<CallStatusMotorUnderWrite>
                {
                    new CallStatusMotorUnderWrite{CallStatusId = 2, CallStatus = "รับสาย" },
                    new CallStatusMotorUnderWrite{CallStatusId = 3, CallStatus = "ไม่รับสาย"},
                    new CallStatusMotorUnderWrite{CallStatusId = 4, CallStatus = "ไม่สะดวกคุย"},
                    new CallStatusMotorUnderWrite{CallStatusId = 5, CallStatus = "ติดต่อไม่ได้"}
                };
                ViewBag.CallStatusListUDWBranch = result9;

                var result10 = _context.usp_AuditCIInsureStatus_Select(null).ToList();
                ViewBag.AuditInsureStatus = result10;

                var result11 = _context.usp_AuditCIInsureConsiderStatus_Select(null).ToList();
                ViewBag.AuditInsureConsiderStatus = result11;

                var result12 = _context.usp_AuditCIUnderwriteStatus_Select(null).ToList();
                ViewBag.AuditUnderwriteStatus = result12;

                var result13 = _context.usp_AuditCIPoliciesGivenStatus_Select(null).ToList();
                ViewBag.AuditCancerPoliciesGivenStatus = result13;

                //เช็คประวัติ ประวัติการชำระเบี้ยให้กับแอปอื่นๆ
                var count2 = _contextCancer.usp_QueueCIPaymentHistory_Select(result.QueueId, 0, null, null, null).ToList().Count();
                ViewBag.countPaymentHistory = count2;

                #region Documents

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

                #endregion Documents

                return View();
            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
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

        // GET: CancerAudit
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure")]
        public ActionResult CancerAuditIndexV2(string cancerQueueAuditId, int? insuredCheck)
        {
            var encodeId = cancerQueueAuditId;
            if (encodeId != null && encodeId != string.Empty)
            {

                //decode
                byte[] data = Convert.FromBase64String(encodeId);
                string decodedString = Encoding.UTF8.GetString(data);

                //call stored procedure
                var result = _context.usp_QueueCIAuditFullDetailByQueueAuditIdV2_Select(Convert.ToInt32(decodedString)).FirstOrDefault();



                //var period_c = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(b, null));

                if (result is null) return RedirectToAction("CancerAuditindex", new { cancerQueueAuditId = cancerQueueAuditId });

                DateTime? oldStartCoverdate = Convert.ToDateTime(Properties.Settings.Default.StartNewVersionCI);
                if (result.StartCoverDate < oldStartCoverdate) return RedirectToAction("CancerAuditindex", new { cancerQueueAuditId = cancerQueueAuditId });
                if (insuredCheck == 1)
                {
                    ViewBag.insuredCheck = true;
                }
                else
                {
                    ViewBag.insuredCheck = false;
                }


                var OpenGoogleDoc = Properties.Settings.Default.GoogleDocPathCI;
                try
                {
                    var appCode = result.ApplicationCode;
                    var insureName = result.InsuredName;
                    var payerName = result.PayerName;

                    string appStartCoverDateFormatted = result.StartCoverDate.HasValue
                          ? result.StartCoverDate.Value.ToString("d/M/yyyy", new System.Globalization.CultureInfo("th-TH"))
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
                //StartCoverDate
                if (result.StartCoverDate != null)
                {
                    ViewBag.StartCoverDate = string.Format("{0}/{1}/{2} {3}", result.StartCoverDate.Value.Day.ToString("00", null), result.StartCoverDate.Value.Month.ToString("00", null), result.StartCoverDate.Value.Year + 543, result.StartCoverDate.Value.ToString("HH:mm:ss"));
                }
                //วันเกิด เอาประกัน
                if (result.InsuredBirthDate != null)
                {
                    ViewBag.InsuredBirthDate = string.Format("{0}/{1}/{2}", result.InsuredBirthDate.Value.Day.ToString("00", null), result.InsuredBirthDate.Value.Month.ToString("00", null), result.InsuredBirthDate.Value.Year + 543);
                }

                //ถ้า null return notfouond
                // if (result is null) return RedirectToAction("NotFound", "Error", new { Msg = "เลข App NotFound" });

                //Get Role
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var accessRole = GlobalFunction.GetAccessRoleUnderWriteAudit(roleList);
                /*ViewBag.AccessRole = roleList.ToList();*/
                var roleList2 = roleListRaw.Split(',').ToList();
                ViewBag.AccessRole = accessRole;
                ViewBag.AccessRole2 = roleList2;


                var OpenSSS = Properties.Settings.Default.SSSpath;
                ViewBag.OpenSSS = OpenSSS;

                //set app code
                ViewBag.AppCode = result.ApplicationCode;

                //1 = na ให้ set default เป็น 2
                ViewBag.QueueAuditStatusId = (result.QueueAuditStatusId == 1 ? 2 : result.QueueAuditStatusId);
                ViewBag.QueueCancerFullDetail = result;
                var applicationCode = result.ApplicationCode;

                var result2 = _context.usp_QueueCIAuditStatus_Select(null).ToList();
                ViewBag.QueueAuditStatus = result2;

                var result3 = _context.usp_CallStatusCI_Select(null).ToList();
                ViewBag.CallStatusListAuditCancer = result3;

                //var result4 = _context.usp_AuditUnderwriteResultStatus_Select(null).ToList();
                //ViewBag.AuditUnderwriteResultStatus = result4;

                var result5 = _contextCancer.usp_UnderwriteTypeCI_Select(null).ToList();
                ViewBag.UnderwriteTypeCI = result5;

                var result6 = _context.usp_AuditCISpecifyStatus_Select(null).ToList();
                ViewBag.AuditCancerSpecifyStatus = result6;

                var result7 = _contextCancer.usp_UnderwriteTypeCI_Select(null).ToList();
                ViewBag.UnderwriteType = result7;

                var result15 = _contextCancer.usp_UnderwritePaymentTypeCI_Select(null).ToList();
                ViewBag.UnderwritePaymentTypeCancer = result15;

                var result8 = _context.usp_AuditCIPaymentStatus_Select(null).ToList();
                ViewBag.AuditCancerPaymentType = result8;

                var result14 = _context.usp_AuditCIStatus_Select(null).ToList();
                ViewBag.AuditCancerStatus = result14;

                var result19 = _context.usp_NotAuditedCause_Select(null, null).ToList();
                ViewBag.NotAuditedCause = result19;
                var result20 = _context.usp_SaleMethodType_Select(null, null).ToList();
                ViewBag.SaleMethodType = result20;

                var result21 = _context.usp_SaleServiceType_Select(null, null).ToList();
                ViewBag.SaleServiceType = result21;

                var result22 = _context.usp_UnderwritingBehaviorType_Select(null, null).ToList();
                ViewBag.UnderwritingBehaviorType = result22;
                var result23 = _context.usp_CallStatusCI_Select(null).ToList();
                ViewBag.CallStatusList = result23;

                var result24 = _context.usp_AuditCISpecifyStatus_Select(null).ToList();
                ViewBag.AuditHealthSpecifyStatus = result24;

                var result25 = _context.usp_ReceivingInsuranceType_Select(null, null).ToList();
                ViewBag.ReceivingInsuranceType = result25;
                var result26 = _context.usp_AuditInsureNotConsideredVerifiedType_Select(null, null).ToList();
                ViewBag.AuditInsureNotConsideredVerifiedType = result26;
                var result27 = _context.usp_AuditInsureNotConsideredResultStatus_Select(null, null).ToList();
                ViewBag.AuditInsureNotConsideredResultStatus = result27;



                //var result9 = _contextCancer.usp_CallStatusCI_Select(null).ToList();
                //ViewBag.CallStatusListUDWBranch = result9;
                var result9 = new List<CallStatusMotorUnderWrite>
                {
                    new CallStatusMotorUnderWrite{CallStatusId = 2, CallStatus = "รับสาย" },
                    new CallStatusMotorUnderWrite{CallStatusId = 3, CallStatus = "ไม่รับสาย"},
                    new CallStatusMotorUnderWrite{CallStatusId = 4, CallStatus = "ไม่สะดวกคุย"},
                    new CallStatusMotorUnderWrite{CallStatusId = 5, CallStatus = "ติดต่อไม่ได้"}
                };
                ViewBag.CallStatusListUDWBranch = result9;

                var result10 = _context.usp_AuditCIInsureStatus_Select(null).ToList();
                ViewBag.AuditInsureStatus = result10;

                var result11 = _context.usp_AuditCIInsureConsiderStatus_Select(null).ToList();
                ViewBag.AuditInsureConsiderStatus = result11;

                var result12 = _context.usp_AuditCIUnderwriteStatus_Select(null).ToList();
                ViewBag.AuditUnderwriteStatus = result12;

                var result13 = _context.usp_AuditCIPoliciesGivenStatus_Select(null).ToList();
                ViewBag.AuditCancerPoliciesGivenStatus = result13;

                //เช็คประวัติ ประวัติการชำระเบี้ยให้กับแอปอื่นๆ
                var count2 = _contextCancer.usp_QueueCIPaymentHistory_Select(result.QueueAuditId, 0, null, null, null).ToList().Count();
                ViewBag.countPaymentHistory = count2;

                #region Documents

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

                #endregion Documents

                return View();
            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
            }
        }





        [HttpPost]
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure")]
        public ActionResult QuestionCIAuditUpdateV2(FormCollection form)
        {
            //สถานะการตรวจสอบ
            var queueAuditStatus = form["queueAuditStatusId"];

            if (!string.IsNullOrEmpty(queueAuditStatus))
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                var QueueAuditId = form["queueAuditId"];

                /// ยังไม่ได้ตรวจสอบ
                var CallStatusId = form["callStatusId"];
                var NotAuditCauseId = form["notAuditedCauseId"];        //สาเหตุ
                var AuditRemark = form["auditRemark"];
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
                var InsuranceInsuredIsValid = form["insuranceInsuredIsValid"];
                var IsReceivedInsurance = form["isReceivedInsurance"];
                // ประเภทบัตร
                var LINEOAIsValid = form["lineOAIsValid"];
                var LINEOARemark = form["lineOARemark"];
                // ข้อมูล ผอ.บล
                var UnderwriteByUserId = form["underwriteByUserId"];
                //var UnderwriteByUserId = "3";
                var UnderwriteByUserRemark = form["underwriteByUserRemark"];
                // เพิ่มเพื่อนกับสยามสไมล์ทาง line OA
                var AddLINEIsSuccess = form["addLINEIsSuccess"];
                // รูปแนบบัตรประกัน
                var InsuranceCardTypeId = form["insuranceCardType"];

                var InsuranceInsuredRemark = form["insuranceInsuredRemark"];
                var ReceivingInsuranceTypeId = form["receivingInsuranceTypeId"];
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
                var IsCriticalIllnessHistory = form["isCriticalIllnessHistory"];
                var CriticalIllnessHistoryRemark = form["criticalIllnessHistoryRemark"];
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
                // ค่าสายตา
                var IsVisionValue = form["isVisionValue"];
                var VisionValueRemark = form["visionValueRemark"];
                //การได้ยิน
                var IsHearingValue = form["isHearingValue"];
                var HearingValueRemark = form["hearingValueRemark"];

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
                //var AuditHealthStatusId = form["auditHealthStatusId"];
                var AuditCIStatusId = form["auditCIStatusId"];//-----------------

                //var AuditHealthSpecifyStatusId = form["auditHealthSpecifyStatusId"];
                var AuditCISpecifyStatusId = form["auditCISpecifyStatusId"];//-----------------

                //var AuditHealthRemark = form["auditHealthRemark"];
                var AuditCIRemark = form["auditCIRemark"];//-----------------
                // หมายเหตุประวัตการคัดกรอง
                // หมายเหตุอื่นๆ
                //var AuditOtherHealthRemark = form["auditOtherHealthRemark"];
                var AuditOtherCIRemark = form["auditOtherCIRemark"];


                // ผลการตรวจสอบ


                //DECLARE
                //int? insuranceCardTypeId = Convert.ToInt32(InsuranceCardTypeId);

                int? userId = Convert.ToInt32(user);
                int? queueAuditId = Convert.ToInt32(QueueAuditId);
                int? queueAuditStatusId = Convert.ToInt32(queueAuditStatus);
                int? callStatusId = Convert.ToInt32(CallStatusId);
                int? notAuditCauseId = Convert.ToInt32(NotAuditCauseId);
                int? saleMethodTypeId = Convert.ToInt32(SaleMethodTypeId);
                int? saleServiceTypeId = Convert.ToInt32(SaleServiceTypeId);
                int? underwriteByUserId = Convert.ToInt32(UnderwriteByUserId);
                int? receivingInsuranceTypeId = 1;
                int? feedbackRate = Convert.ToInt32(FeedbackRate);
                int? underwritingBehaviorTypeId = Convert.ToInt32(UnderwritingBehaviorTypeId);
                int? auditQCUserId = Convert.ToInt32(user);
                int? auditCIStatusId = 1;
                int? auditCISpecifyStatusId = 1;

                bool? insuranceInsuredIsValid = null;
                bool? addLINEIsSuccess = null;
                bool? isReceivedInsurance = null;
                bool? serviceResultIsIssue = null;
                bool? insuredNameIsValid = null;
                bool? payerNameIsValid = null;
                bool? birthDateIsValid = null;
                bool? weightHeightIsValid = null;
                bool? isAllergyHistory = null;
                bool? isCriticalIllnessHistory = null;
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
                bool? isVisionValue = null;
                bool? isHearingValue = null;
                bool? payMethodTypeIsValid = null;

                string auditRemark = null;
                string saleMethodTypeRemark = null;
                string saleServiceTypeRemark = null;
                string insuranceInsuredRemark = null;
                string visionValueRemark = null;
                string hearingValueRemark = null;
                string underwriteByUserRemark = null;
                string feedbackRemark = null;
                string serviceResultRemark = null;
                string insuredNameRemark = null;
                string payerNameRemark = null;
                string birthDateRemark = null;
                string weightHeightRemark = null;
                string allergyHistoryRemark = null;
                string criticalIllnessHistoryRemark = null;
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
                string auditCIRemark = null;
                string auditOtherCIRemark = null;
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

                        receivingInsuranceTypeId = 1; //n/a
                        notAuditCauseId = null;
                        feedbackRate = 0;
                        auditRemark = AuditRemark.Trim() == "" ? auditRemark : AuditRemark;
                        break;

                    //ไม่ต้องตรวจสอบ
                    case 4:
                        //สถานะการโทร
                        //callStatusId = Convert.ToInt32(CallStatusId);
                        // สาเหตุ
                        receivingInsuranceTypeId = 1; //n/a
                        callStatusId = null;
                        feedbackRate = 0;
                        // หมายเหตุ
                        auditRemark = AuditRemarkNotAuditedCause.Trim() == "" ? auditRemark : AuditRemarkNotAuditedCause;
                        break;

                    //ตรวจสอบแล้ว
                    case 3:
                        //สถานะการโทร - โทรแล้ว
                        callStatusId = 5;

                        // หมายเหตุเพิ่มเติม
                        saleMethodTypeRemark = SaleMethodTypeRemark.Trim() == "" ? saleMethodTypeRemark : SaleMethodTypeRemark;
                        saleServiceTypeRemark = SaleServiceTypeRemark.Trim() == "" ? saleServiceTypeRemark : SaleServiceTypeRemark;
                        insuranceInsuredRemark = InsuranceInsuredRemark.Trim() == "" ? insuranceInsuredRemark : InsuranceInsuredRemark;
                        underwriteByUserRemark = UnderwriteByUserRemark.Trim() == "" ? underwriteByUserRemark : UnderwriteByUserRemark;
                        feedbackRemark = FeedbackRemark.Trim() == "" ? feedbackRemark : FeedbackRemark;
                        serviceResultRemark = ServiceResultRemark.Trim() == "" ? serviceResultRemark : ServiceResultRemark;
                        insuredNameRemark = InsuredNameRemark.Trim() == "" ? insuredNameRemark : InsuredNameRemark;
                        payerNameRemark = PayerNameRemark.Trim() == "" ? payerNameRemark : PayerNameRemark;
                        birthDateRemark = BirthDateRemark.Trim() == "" ? birthDateRemark : BirthDateRemark;
                        weightHeightRemark = WeightHeightRemark.Trim() == "" ? weightHeightRemark : WeightHeightRemark;
                        allergyHistoryRemark = AllergyHistoryRemark.Trim() == "" ? allergyHistoryRemark : AllergyHistoryRemark;
                        criticalIllnessHistoryRemark = CriticalIllnessHistoryRemark.Trim() == "" ? criticalIllnessHistoryRemark : CriticalIllnessHistoryRemark;
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
                        auditOtherCIRemark = AuditOtherCIRemark.Trim() == "" ? auditOtherCIRemark : AuditOtherCIRemark;
                        underwritingBehaviorRemark = UnderwritingBehaviorRemark.Trim() == "" ? underwritingBehaviorRemark : UnderwritingBehaviorRemark;
                        visionValueRemark = VisionValueRemark.Trim() == "" ? visionValueRemark : VisionValueRemark;
                        hearingValueRemark = HearingValueRemark.Trim() == "" ? hearingValueRemark : HearingValueRemark;




                        // Radio box

                        isReceivedInsurance = QuestionAuditUpdateV2CheckBoolType(IsReceivedInsurance);
                        isVisionValue = QuestionAuditUpdateV2CheckBoolType(IsVisionValue);
                        insuranceInsuredIsValid = QuestionAuditUpdateV2CheckBoolType(InsuranceInsuredIsValid);
                        addLINEIsSuccess = QuestionAuditUpdateV2CheckBoolType(AddLINEIsSuccess);
                        serviceResultIsIssue = QuestionAuditUpdateV2CheckBoolType(ServiceResultIsIssue);
                        insuredNameIsValid = QuestionAuditUpdateV2CheckBoolType(InsuredNameIsValid);
                        payerNameIsValid = QuestionAuditUpdateV2CheckBoolType(PayerNameIsValid);
                        birthDateIsValid = QuestionAuditUpdateV2CheckBoolType(BirthDateIsValid);
                        weightHeightIsValid = QuestionAuditUpdateV2CheckBoolType(WeightHeightIsValid);
                        isAllergyHistory = QuestionAuditUpdateV2CheckBoolType(IsAllergyHistory);
                        isCriticalIllnessHistory = QuestionAuditUpdateV2CheckBoolType(IsCriticalIllnessHistory);
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
                        isHearingValue = QuestionAuditUpdateV2CheckBoolType(IsHearingValue);
                        payMethodTypeIsValid = QuestionAuditUpdateV2CheckBoolType(PayMethodTypeIsValid);

                        //// การนำเสนอขาย และบริการหลังการขาย
                        /// คู่มือ

                        // ได้รับคู่มือใช่หรือไม่     receivingInsuranceTypeId = 1; //n/a
                        receivingInsuranceTypeId = isReceivedInsurance != null && (bool)isReceivedInsurance ? Convert.ToInt32(ReceivingInsuranceTypeId) : receivingInsuranceTypeId; // default receivingManualTypeId = 1

                        //// ผลการตรวจสอบ ด้านสุขภาพ
                        auditCIStatusId = Convert.ToInt32(AuditCIStatusId);
                        //เช็ค id ผลการตรวจสอบสุขภาพ
                        switch (auditCIStatusId)
                        {
                            //ผ่าน
                            case 2:
                                //หมายเหตุ-การตรวจสอบด้านสุขภาพ
                                auditCIRemark = AuditCIRemark.Trim() == "" ? auditCIRemark : AuditCIRemark;
                                break;
                            //รอพิจารณา
                            case 3:
                                //รอพิจารณา -> ระบุในใบสมัคร-ไม่ระบุ
                                auditCISpecifyStatusId = Convert.ToInt32(AuditCISpecifyStatusId);

                                //หมายเหตุ-การตรวจสอบด้านสุขภาพ
                                auditCIRemark = AuditCIRemark.Trim() == "" ? auditCIRemark : AuditCIRemark;
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }

                //var a = new AuditDto
                //{
                //    UserId = userId,
                //    //  QueueAuditId
                //    QueueAuditId = queueAuditId,

                //    //สถานะการตรวจสอบ
                //    QueueAuditStatusId = queueAuditStatusId,

                //    //สาเหตุ 
                //    CallStatusId = callStatusId,
                //    NotAuditCauseId = notAuditCauseId,

                //    //สาเหตุ หมายเหตุ *remark รวม
                //    AuditRemark = auditRemark,

                //    //1.1 
                //    SaleMethodTypeId = saleMethodTypeId,
                //    SaleMethodTypeRemark = saleMethodTypeRemark,

                //    //1.2
                //    SaleServiceTypeId = saleServiceTypeId,
                //    SaleServiceTypeRemark = saleServiceTypeRemark,

                //    //1.3
                //    IsReceivedInsurance = isReceivedInsurance,

                //    //1.4
                //    InsuranceInsuredIsValid = insuranceInsuredIsValid,
                //    InsuranceInsuredRemark = insuranceInsuredRemark,

                //    //1.5
                //    UnderwriteByUserId = underwriteByUserId,
                //    UnderwriteByUserRemark = underwriteByUserRemark,

                //    //1.6
                //    AddLINEIsSuccess = addLINEIsSuccess,

                //    //1.7
                //    FeedbackRate = feedbackRate,
                //    FeedbackRemark = feedbackRemark,

                //    //1.8
                //    ServiceResultIsIssue = serviceResultIsIssue,
                //    ServiceResultRemark = serviceResultRemark,

                //    //2.1
                //    InsuredNameIsValid = insuredNameIsValid,
                //    InsuredNameRemark = insuredNameRemark,

                //    //2.2
                //    PayerNameIsValid = payerNameIsValid,
                //    PayerNameRemark = payerNameRemark,

                //    //2.3
                //    BirthDateIsValid = birthDateIsValid,
                //    BirthDateRemark = birthDateRemark,

                //    //2.4
                //    WeightHeightIsValid = weightHeightIsValid,
                //    WeightHeightRemark = weightHeightRemark,

                //    //3.1
                //    IsAllergyHistory = isAllergyHistory,
                //    AllergyHistoryRemark = allergyHistoryRemark,

                //    //3.2
                //    IsCriticalIllnessHistory = isCriticalIllnessHistory,
                //    CriticalIllnessHistoryRemark = criticalIllnessHistoryRemark,

                //    //3.3
                //    IsAccidentHistory = isAccidentHistory,
                //    AccidentHistoryRemark = accidentHistoryRemark,

                //    //3.4
                //    IsSurgeryHistory = isSurgeryHistory,
                //    SurgeryHistoryRemark = surgeryHistoryRemark,

                //    //3.5
                //    IsHealthCheckHistory = isHealthCheckHistory,
                //    HealthCheckHistoryRemark = healthCheckHistoryRemark,

                //    //3.6
                //    IsChronicDiseaseHistory = isChronicDiseaseHistory,
                //    ChronicDiseaseHistoryRemark = chronicDiseaseHistoryRemark,

                //    //3.7
                //    IsMedicalLatestHistory = isMedicalLatestHistory,
                //    MedicalLatestHistoryRemark = medicalLatestHistoryRemark,

                //    //3.8
                //    IsSmokingHistory = isSmokingHistory,
                //    SmokingHistoryRemark = smokingHistoryRemark,

                //    //3.9
                //    IsDrinkingHistory = isDrinkingHistory,
                //    DrinkingHistoryRemark = drinkingHistoryRemark,

                //    //3.10
                //    IsVisionValue = isVisionValue,
                //    VisionValueRemark = visionValueRemark,

                //    //3.11
                //    IsHearingValue = isHearingValue,
                //    HearingValueRemark = hearingValueRemark,

                //    //3.12                    
                //    IsPregnantHistory = isPregnantHistory,
                //    PregnantHistoryRemark = pregnantHistoryRemark,

                //    //3.13
                //    IsOtherHealthHistory = isOtherHealthHistory,
                //    OtherHealthHistoryRemark = otherHealthHistoryRemark,

                //    //4.1
                //    PayerOccupationIsValid = payerOccupationIsValid,
                //    PayerOccupationRemark = payerOccupationRemark,

                //    //4.2
                //    PayerBuildingNameIsValid = payerBuildingNameIsValid,
                //    PayerBuildingNameRemark = payerBuildingNameRemark,

                //    //4.3
                //    ProductIsValid = productIsValid,
                //    ProductRemark = productRemark,

                //    //4.4
                //    PremiumIsValid = premiumIsValid,
                //    PremiumRemark = premiumRemark,

                //    //4.5
                //    PeriodTypeIsValid = periodTypeIsValid,
                //    PeriodTypeRemark = periodTypeRemark,

                //    //4.6
                //    PayMethodTypeIsValid = payMethodTypeIsValid,
                //    PayMethodTypeRemark = payMethodTypeRemark,

                //    //5.1
                //    AuditCIStatusId = auditCIStatusId,
                //    AuditCISpecifyStatusId = auditCISpecifyStatusId,

                //    //หมายเหตุประวัติการคัดกรอง
                //    AuditCIRemark = auditCIRemark,

                //    //5.2
                //    UnderwritingBehaviorTypeId = underwritingBehaviorTypeId,
                //    UnderwritingBehaviorRemark = underwritingBehaviorRemark,

                //    //หมายเหตุอื่นๆ
                //    AuditOtherCIRemark = auditOtherCIRemark,

                //    AuditQCUserId = auditQCUserId,

                //};

                //ReceivingManualTypeId = receivingManualTypeId,


                var result = _context.usp_QueueCIAudit1ResultV2_Insert(queueAuditId,
    queueAuditStatusId,
    notAuditCauseId == -1 ? 1 : notAuditCauseId,
    callStatusId == -1 ? 1 : callStatusId,
    auditRemark,
    saleMethodTypeId,
    saleMethodTypeRemark,
    saleServiceTypeId,
    saleServiceTypeRemark,
    isReceivedInsurance,
    receivingInsuranceTypeId,
    insuranceInsuredIsValid,
    insuranceInsuredRemark,
    underwriteByUserId,
    underwriteByUserRemark,
    addLINEIsSuccess,
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
    isCriticalIllnessHistory,
    criticalIllnessHistoryRemark,
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
    isVisionValue,
    visionValueRemark,
    isHearingValue,
    hearingValueRemark,
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
    auditCIStatusId,
    auditCISpecifyStatusId,
    auditCIRemark,
    auditOtherCIRemark,
    auditQCUserId
    ).FirstOrDefault();

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
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure")]
        public ActionResult ApplicationInsuredRelateByApplicationCodeSelect(string applicationCode, int? indexStart, int? pageSize, string sortField, string orderType)
        {
            var result = _context.usp_ApplicationCIInsuredRelateByApplicationCode_Select(applicationCode, indexStart, pageSize, sortField, orderType).ToList();

            var dt = new Dictionary<string, object>
            {
                 {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                 {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                 {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure")]
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


        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure")]
        public ActionResult CancerAuditView(string cancerQueueAuditId)
        {
            var encodeId = cancerQueueAuditId;
            if (encodeId != null && encodeId != string.Empty)
            {
                //decode
                byte[] data = Convert.FromBase64String(encodeId);
                string decodedString = Encoding.UTF8.GetString(data);

                //call stored procedure
                var result = _context.usp_QueueCIAuditFullDetailByQueueAuditId_Select(Convert.ToInt32(decodedString)).FirstOrDefault();

                //StartCoverDate
                if (result.StartCoverDate != null)
                {
                    ViewBag.StartCoverDate = string.Format("{0}/{1}/{2} {3}", result.StartCoverDate.Value.Day.ToString("00", null), result.StartCoverDate.Value.Month.ToString("00", null), result.StartCoverDate.Value.Year + 543, result.StartCoverDate.Value.ToString("HH:mm:ss"));
                }
                //วันเกิด เอาประกัน
                if (result.InsuredBirthDate != null)
                {
                    ViewBag.InsuredBirthDate = string.Format("{0}/{1}/{2}", result.InsuredBirthDate.Value.Day.ToString("00", null), result.InsuredBirthDate.Value.Month.ToString("00", null), result.InsuredBirthDate.Value.Year + 543);
                }

                //GiveDate
                if (result.GiveDate != null)
                {
                    ViewBag.GiveDate = string.Format("{0}/{1}/{2}", result.GiveDate.Value.Day.ToString("00", null), result.GiveDate.Value.Month.ToString("00", null), result.GiveDate.Value.Year + 543);
                }

                //ถ้า null return notfouond
                if (result is null) return RedirectToAction("NotFound", "Error", new { Msg = "เลข App NotFound" });

                //Get Role
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var accessRole = GlobalFunction.GetAccessRoleUnderWriteAudit(roleList);
                /* ViewBag.AccessRole = roleList.ToList();*/
                var roleList2 = roleListRaw.Split(',').ToList();
                ViewBag.AccessRole = accessRole;
                ViewBag.AccessRole2 = roleList2;

                //set app code
                ViewBag.AppCode = result.ApplicationCode;

                var OpenSSS = Properties.Settings.Default.SSSpath;
                ViewBag.OpenSSS = OpenSSS;

                //1 = na ให้ set default เป็น 2
                ViewBag.QueueAuditStatusId = (result.QueueAuditStatusId == 1 ? 2 : result.QueueAuditStatusId);
                ViewBag.QueueCancerFullDetail = result;
                var applicationCode = result.ApplicationCode;

                var result2 = _context.usp_QueueCIAuditStatus_Select(null).ToList();
                ViewBag.QueueAuditStatus = result2;

                var result3 = _context.usp_CallStatusCI_Select(null).ToList();
                ViewBag.CallStatusListAuditCancer = result3;

                var result4 = _context.usp_AuditCIUnderwriteStatus_Select(null).ToList();
                ViewBag.AuditUnderwriteResultStatus = result4;

                var result5 = _context.usp_AuditCIUnderwriteStatus_Select(null).ToList();
                ViewBag.AuditCancerStatus = result5;

                var result6 = _context.usp_AuditCISpecifyStatus_Select(null).ToList();
                ViewBag.AuditCancerSpecifyStatus = result6;

                var result7 = _contextCancer.usp_UnderwriteTypeCI_Select(null).ToList();
                ViewBag.UnderwriteType = result7;

                var result15 = _contextCancer.usp_UnderwritePaymentTypeCI_Select(null).ToList();
                ViewBag.UnderwritePaymentTypeCancer = result15;

                var result8 = _context.usp_AuditCIPaymentStatus_Select(null).ToList();
                ViewBag.AuditCancerPaymentType = result8;

                var result14 = _context.usp_AuditCIStatus_Select(null).ToList();
                ViewBag.AuditCancerStatus = result14;

                //var result9 = _contextCancer.usp_CallStatusCI_Select(null).ToList();
                //ViewBag.CallStatusListUDWBranch = result9;
                var result9 = new List<CallStatusMotorUnderWrite>
                {
                    new CallStatusMotorUnderWrite{CallStatusId = 2, CallStatus = "รับสาย" },
                    new CallStatusMotorUnderWrite{CallStatusId = 3, CallStatus = "ไม่รับสาย"},
                    new CallStatusMotorUnderWrite{CallStatusId = 4, CallStatus = "ไม่สะดวกคุย"},
                    new CallStatusMotorUnderWrite{CallStatusId = 5, CallStatus = "ติดต่อไม่ได้"}
                };
                ViewBag.CallStatusListUDWBranch = result9;

                var result10 = _context.usp_AuditCIInsureStatus_Select(null).ToList();
                ViewBag.AuditInsureStatus = result10;

                var result11 = _context.usp_AuditCIInsureConsiderStatus_Select(null).ToList();
                ViewBag.AuditInsureConsiderStatus = result11;

                var result12 = _context.usp_AuditCIUnderwriteStatus_Select(null).ToList();
                ViewBag.AuditUnderwriteStatus = result12;

                var result13 = _context.usp_AuditCIPoliciesGivenStatus_Select(null).ToList();
                ViewBag.AuditCancerPoliciesGivenStatus = result13;
                var result22 = _context.usp_UnderwritingBehaviorType_Select(null, null).ToList();
                ViewBag.UnderwritingBehaviorType = result22;
                //เช็คประวัติ ประวัติการชำระเบี้ยให้กับแอปอื่นๆ
                var count2 = _contextCancer.usp_QueueCIPaymentHistory_Select(result.QueueId, 0, null, null, null).ToList().Count();
                ViewBag.countPaymentHistory = count2;

                #region Documents

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

                #endregion Documents

                return View();
            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
            }
        }



        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure")]
        public ActionResult CancerAuditViewV2(string cancerQueueAuditId, int? insuredCheck)
        {
            var encodeId = cancerQueueAuditId;
            if (encodeId != null && encodeId != string.Empty)
            {
                //decode
                byte[] data = Convert.FromBase64String(encodeId);
                string decodedString = Encoding.UTF8.GetString(data);

                //call stored procedure
                var result = _context.usp_QueueCIAuditFullDetailByQueueAuditIdV2_Select(Convert.ToInt32(decodedString)).FirstOrDefault();


                if (insuredCheck == 1)
                {
                    ViewBag.insuredCheck = true;
                }
                else
                {
                    ViewBag.insuredCheck = false;
                }

                //StartCoverDate
                if (result.StartCoverDate != null)
                {
                    ViewBag.StartCoverDate = string.Format("{0}/{1}/{2} {3}", result.StartCoverDate.Value.Day.ToString("00", null), result.StartCoverDate.Value.Month.ToString("00", null), result.StartCoverDate.Value.Year + 543, result.StartCoverDate.Value.ToString("HH:mm:ss"));
                }
                //วันเกิด เอาประกัน
                if (result.InsuredBirthDate != null)
                {
                    ViewBag.InsuredBirthDate = string.Format("{0}/{1}/{2}", result.InsuredBirthDate.Value.Day.ToString("00", null), result.InsuredBirthDate.Value.Month.ToString("00", null), result.InsuredBirthDate.Value.Year + 543);
                }

                //ถ้า null return notfouond

                if (result is null) return RedirectToAction("CancerAuditView", new { cancerQueueAuditId = cancerQueueAuditId });
                DateTime? oldStartCoverdate = Convert.ToDateTime(Properties.Settings.Default.StartNewVersionCI);
                if (result.StartCoverDate < oldStartCoverdate) return RedirectToAction("CancerAuditView", new { cancerQueueAuditId = cancerQueueAuditId });
                if (result.IsPolicies == null)
                {
                    if (cancerQueueAuditId != null && cancerQueueAuditId != "")
                    {
                        return RedirectToAction("CancerAuditView", new { cancerQueueAuditId = cancerQueueAuditId });
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
                /* ViewBag.AccessRole = roleList.ToList();*/
                var roleList2 = roleListRaw.Split(',').ToList();
                ViewBag.AccessRole = accessRole;
                ViewBag.AccessRole2 = roleList2;

                //set app code
                ViewBag.AppCode = result.ApplicationCode;

                var OpenSSS = Properties.Settings.Default.SSSpath;
                ViewBag.OpenSSS = OpenSSS;

                //1 = na ให้ set default เป็น 2
                ViewBag.QueueAuditStatusId = (result.QueueAuditStatusId == 1 ? 2 : result.QueueAuditStatusId);
                ViewBag.QueueCancerFullDetail = result;
                var applicationCode = result.ApplicationCode;

                var result2 = _context.usp_QueueCIAuditStatus_Select(null).ToList();
                ViewBag.QueueAuditStatus = result2;

                var result3 = _context.usp_CallStatusCI_Select(null).ToList();
                ViewBag.CallStatusListAuditCancer = result3;

                var result4 = _context.usp_AuditCIUnderwriteStatus_Select(null).ToList();
                ViewBag.AuditUnderwriteResultStatus = result4;

                var result5 = _context.usp_AuditCIUnderwriteStatus_Select(null).ToList();
                ViewBag.AuditCancerStatus = result5;

                var result6 = _context.usp_AuditCISpecifyStatus_Select(null).ToList();
                ViewBag.AuditCancerSpecifyStatus = result6;

                var result7 = _contextCancer.usp_UnderwriteTypeCI_Select(null).ToList();
                ViewBag.UnderwriteType = result7;

                var result15 = _contextCancer.usp_UnderwritePaymentTypeCI_Select(null).ToList();
                ViewBag.UnderwritePaymentTypeCancer = result15;

                var result8 = _context.usp_AuditCIPaymentStatus_Select(null).ToList();
                ViewBag.AuditCancerPaymentType = result8;

                var result14 = _context.usp_AuditCIStatus_Select(null).ToList();
                ViewBag.AuditCancerStatus = result14;
                var result25 = _context.usp_ReceivingInsuranceType_Select(null, null).ToList();
                ViewBag.ReceivingInsuranceType = result25;
                //var result9 = _contextCancer.usp_CallStatusCI_Select(null).ToList();
                //ViewBag.CallStatusListUDWBranch = result9;
                var result9 = new List<CallStatusMotorUnderWrite>
                {
                    new CallStatusMotorUnderWrite{CallStatusId = 2, CallStatus = "รับสาย" },
                    new CallStatusMotorUnderWrite{CallStatusId = 3, CallStatus = "ไม่รับสาย"},
                    new CallStatusMotorUnderWrite{CallStatusId = 4, CallStatus = "ไม่สะดวกคุย"},
                    new CallStatusMotorUnderWrite{CallStatusId = 5, CallStatus = "ติดต่อไม่ได้"}
                };
                ViewBag.CallStatusListUDWBranch = result9;

                var result10 = _context.usp_AuditCIInsureStatus_Select(null).ToList();
                ViewBag.AuditInsureStatus = result10;

                var result11 = _context.usp_AuditCIInsureConsiderStatus_Select(null).ToList();
                ViewBag.AuditInsureConsiderStatus = result11;

                var result12 = _context.usp_AuditCIUnderwriteStatus_Select(null).ToList();
                ViewBag.AuditUnderwriteStatus = result12;

                var result13 = _context.usp_AuditCIPoliciesGivenStatus_Select(null).ToList();
                ViewBag.AuditCancerPoliciesGivenStatus = result13;
                var result23 = _context.usp_CallStatusCI_Select(null).ToList();
                ViewBag.CallStatusList = result23;
                var result20 = _context.usp_SaleMethodType_Select(null, null).ToList();
                ViewBag.SaleMethodType = result20;
                var result24 = _context.usp_AuditCISpecifyStatus_Select(null).ToList();
                ViewBag.AuditHealthSpecifyStatus = result24;
                var result19 = _context.usp_NotAuditedCause_Select(null, null).ToList();
                ViewBag.NotAuditedCause = result19;
                //เช็คประวัติ ประวัติการชำระเบี้ยให้กับแอปอื่นๆ
                var count2 = _contextCancer.usp_QueueCIPaymentHistory_Select(result.QueueAuditId, 0, null, null, null).ToList().Count();
                ViewBag.countPaymentHistory = count2;
                var result21 = _context.usp_SaleServiceType_Select(null, null).ToList();
                ViewBag.SaleServiceType = result21;
                var result22 = _context.usp_UnderwritingBehaviorType_Select(null, null).ToList();
                ViewBag.UnderwritingBehaviorType = result22;
                var result26 = _context.usp_AuditInsureNotConsideredVerifiedType_Select(null, null).ToList();
                ViewBag.AuditInsureNotConsideredVerifiedType = result26;
                var result27 = _context.usp_AuditInsureNotConsideredResultStatus_Select(null, null).ToList();
                ViewBag.AuditInsureNotConsideredResultStatus = result27;

                #region Documents

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

                #endregion Documents

                return View();
            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
            }
        }


        #endregion View

        #region API

        [HttpPost]
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure")]
        public ActionResult CancerQuestionAuditInsert(FormCollection form)
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
                var CallRemark = form["remark"];

                //ผลการตรวจสอบ ผอ.บล.
                var AuditUnderwriteResultStatus = form["AuditUnderwriteResultStatus"];

                //ผลการตรวจสอบการคัดกรอง
                var AuditUnderwriteStatus = form["AuditUnderwriteStatus"];

                //ผลการตรวจสอบการมอบบัตร
                var auditPoliciesGivenStatus = form["AuditPoliciesGivenStatus"];
                var AuditRemark = form["AuditRemark"];

                //ผลการตรวจสอบการชำระเบี้ย
                var AuditPaymentCIStatus = form["AuditPaymentCancerStatus"];
                var AuditPaymentRemark = form["AuditPaymentCancerRemark"];

                //การตรวจสอบข้อมูลการทำประกัน
                var AuditCIStatus = form["AuditCancerStatus"];
                var AuditCIRemark = form["AuditCancerRemark"];

                //รอพิจารณา -> ระบุในใบสมัคร-ไม่ระบุ
                var AuditCISpecifyStatus = form["AuditCancerSpecifyStatus"];
                var AuditCISpecifyRemark = form["AuditCancerSpecifyRemark"];

                //DECLARE
                int? userId = Convert.ToInt32(user);
                int? queueAuditStatusId = Convert.ToInt32(queueAuditStatus);
                int? queueAuditId = Convert.ToInt32(QueueAuditId);

                // 1 = na ค่า ตั้งต้น
                int? callStatusId = 1;
                int? auditPaymentStatusId = AuditPaymentCIStatus != null ? Convert.ToInt32(AuditPaymentCIStatus) : 1;
                int? auditCIStatusId = 1;
                int? auditCISpecifyStatusId = 1;
                string auditRemark = null;
                string auditPaymentCIRemark = null;
                string auditCIRemark = null;
                int? auditUnderwriteStatusId = 1;
                int? auditPoliciesGivenStatusId = 1;

                switch (queueAuditStatusId)
                {
                    //ยังไม่ได้ตรวจสอบ
                    case 2:
                        //สถานะการโทร
                        callStatusId = Convert.ToInt32(CallStatus);
                        //หมายเหตุ *หลัก
                        auditRemark = CallRemark;
                        break;

                    //ตรวจสอบแล้ว
                    case 3:
                        //สถานะการโทร - โทรแล้ว
                        callStatusId = 5;
                        //ผลการตรวจสอบ ผอ.บล.
                        //auditUnderwriteResultStatusId = Convert.ToInt32(AuditUnderwriteResultStatus);
                        //หมายเหตุ ผลการตรวจสอบ ผอ.บล.
                        auditPaymentCIRemark = AuditPaymentRemark;
                        //หมายเหตุ *หลัก
                        auditRemark = AuditRemark;
                        //ผลการตรวจสอบการคัดกรอง
                        auditUnderwriteStatusId = Convert.ToInt32(AuditUnderwriteStatus);
                        //ผลการตรวจสอบการมอบบัตร
                        auditPoliciesGivenStatusId = Convert.ToInt32(auditPoliciesGivenStatus);

                        //การตรวจสอบข้อมูลการทำประกัน
                        auditCIStatusId = Convert.ToInt32(AuditCIStatus);
                        //เช็ค id ผลการตรวจสอบสุขภาพ
                        switch (auditCIStatusId)
                        {
                            //ผ่าน
                            case 2:
                                //หมายเหตุ-การตรวจสอบด้านสุขภาพ
                                auditCIRemark = AuditCIRemark;
                                break;
                            //รอพิจารณา
                            case 3:
                                //รอพิจารณา -> ระบุในใบสมัคร-ไม่ระบุ
                                auditCISpecifyStatusId = Convert.ToInt32(AuditCISpecifyStatus);
                                //เลือก รอพิจารณา ให้ระบุข้อยกเว้น ใน หมายเหตุ-การตรวจสอบด้านสุขภาพ
                                auditCIRemark = AuditCISpecifyRemark;
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }

                var result = _context.usp_QueueCIAudit1Result_Insert(queueAuditId: queueAuditId,
                                                                        queueAuditStatusId: queueAuditStatusId,
                                                                        callStatusId: callStatusId,
                                                                        auditUnderwriteStatusId: auditUnderwriteStatusId,
                                                                        auditPoliciesGivenStatusId: auditPoliciesGivenStatusId,
                                                                        auditRemark: auditRemark,
                                                                        auditPaymentStatusId: auditPaymentStatusId,
                                                                        auditPaymentRemark: auditPaymentCIRemark,
                                                                        auditCIStatusId: auditCIStatusId,
                                                                        auditCISpecifyStatusId: auditCISpecifyStatusId,
                                                                        auditCIRemark: auditCIRemark,
                                                                        audit1CreatedByUserId: user).FirstOrDefault();

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
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure")]
        public ActionResult QuestionAuditInsureUpdate(FormCollection form)
        {
            //queue audit id
            var QueueAuditId = form["QueueAuditId_formInsure"];

            if (!string.IsNullOrEmpty(QueueAuditId))
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                var AuditInsureStatus = form["AuditInsureStatus"];
                //แถบพิจารณาแล้วจะ disable AuditInsureStatus ทำให้ AuditInsureStatus ส่งมาเป็น null ต้องใช้ status เดิม คือ AuditInsureStatusId_formInsure
                if (AuditInsureStatus == null)
                {
                    AuditInsureStatus = form["AuditInsureStatusId_formInsure"]; //status เดิม
                }

                //สถานะพิจารณา
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

                var result = _context.usp_QueueCIAudit2Result_Insert(queueAuditId, auditInsureStatusId, auditInsureRemark, auditInsureConsiderStatusId, auditInsureConsiderRemark, userId).FirstOrDefault();

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
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure")]
        /*      public ActionResult QuestionAuditInsureUpdateV2(FormCollection form)
              {
                  //queue audit id
                  var QueueAuditId = form["QueueAuditId_formInsure"];
                  var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                  if (!string.IsNullOrEmpty(QueueAuditId))
                  {


                      var AuditInsureStatus = form["AuditInsureStatus"];
                      //แถบพิจารณาแล้วจะ disable AuditInsureStatus ทำให้ AuditInsureStatus ส่งมาเป็น null ต้องใช้ status เดิม คือ AuditInsureStatusId_formInsure
                      if (AuditInsureStatus == null)
                      {
                          AuditInsureStatus = form["AuditInsureStatusId_formInsure"]; //status เดิม
                      }
                      var AuditInsureConsiderStatusRemark_close = form["AuditInsureConsiderStatusRemark2"];
                      var AuditInsureStatus_close = form["AuditInsureStatus2"];
                      //สถานะพิจารณา
                      var AuditInsureStatusRemark = form["AuditInsureStatusRemark"];
                      //พิจารณาแล้ว --> ผ่าน-ติดเงื่อนไข-ไม่ผ่าน
                      var AuditInsureConsiderStatus = form["AuditInsureConsiderStatus"];
                      var AuditInsureConsiderStatus_close = form["AuditInsureConsiderStatus2"];
                      var AuditInsureConsiderStatusRemark = form["AuditInsureConsiderStatusRemark"];
                      var AuditInsureClose = form["AuditInsureClose"];
                      var AuditInsureCloseRemark = form["AuditInsureCloseRemark"];
                      var insuredCheck = form["insuredCheck"];
                      var AuditInsureNotConsideredResultStatusId = form["AuditInsureNotConsideredResultStatusId"];
                      var AuditInsureNotConsideredVerifiedTypeId = form["AuditInsureNotConsideredVerifiedTypeId"];
                      bool? InsuredCheck = QuestionAuditUpdateV2CheckBoolType(insuredCheck);
                      //DECLARE
                      int? auditInsureConsiderStatus_close = 1;
                      int? userId = Convert.ToInt32(user);
                      int? queueAuditId = Convert.ToInt32(QueueAuditId);
                      int? auditInsureStatusId = Convert.ToInt32(AuditInsureStatus);
                      int? auditInsureStatus_close = AuditInsureStatus_close != "" ? Convert.ToInt32(AuditInsureStatus_close) : 1;
                      // 1 = na ค่า ตั้งต้น
                      int? auditInsureConsiderStatusId = 1;
                      string auditInsureRemark = null;
                      string auditInsureConsiderRemark = null;
                      bool? auditInsureClose = QuestionAuditUpdateV2CheckBoolType(AuditInsureClose);
                      string auditInsureCloseRemark = AuditInsureCloseRemark;
                      int? auditInsureNotConsideredResultStatusId = null;
                      int? auditInsureNotConsideredVerifiedTypeId = null;
                      bool? isSendMemo = null;

                      // InsuredChack คือ param ที่เอาไว้เช็คหน้ารับประกัน 1 case พิจารณา กับ case ติดเงื่อนไข
                      if (InsuredCheck == true && auditInsureClose == true) //หน้าติดเงื่อนไข true && และปิดคิวงานเป็น true
                      {
                          isSendMemo = null;
                      }
                      else if (InsuredCheck == true && auditInsureClose == false)
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
                              //หมายเหตุ
                              auditInsureNotConsideredResultStatusId = 2;
                              //auditInsureConsiderRemark = AuditInsureConsiderStatusRemark;
                              break;
                          //รอพิจารณา

                          default:
                              break;
                      }

                      if (InsuredCheck == true)
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


                          }
                      }



                      var result = _context.usp_QueueCIAudit2ResultV3_Insert(
                 queueAuditId,
                 InsuredCheck == true ? auditInsureStatus_close : auditInsureStatusId,
                 InsuredCheck == true ? auditInsureConsiderStatus_close : auditInsureConsiderStatusId,
                 InsuredCheck == true ? AuditInsureConsiderStatusRemark_close : AuditInsureConsiderStatusRemark,
                 auditInsureNotConsideredResultStatusId == 0 ? 2 : auditInsureNotConsideredResultStatusId,
                 auditInsureNotConsideredVerifiedTypeId == 0 ? null : auditInsureNotConsideredVerifiedTypeId, null,
                 auditInsureClose,
                 auditInsureCloseRemark,
                 userId
                 , isSendMemo
             ).FirstOrDefault();



                      //var result = _context.usp_QueueCIAudit2ResultV2_Insert(queueAuditId, auditInsureStatusId, auditInsureRemark, auditInsureConsiderStatusId, auditInsureConsiderRemark, userId).FirstOrDefault();
                      //var result = _context.usp_QueueCIAudit2ResultV2_Insert(queueAuditId,
                      //        auditInsureStatusId,
                      //        auditInsureConsiderStatusId,
                      //        AuditInsureConsiderStatusRemark,
                      //        auditInsureClose,
                      //        auditInsureCloseRemark,
                      //        userId, isSendMemo).FirstOrDefault();

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
              }*/
        public ActionResult QuestionAuditInsureUpdateV2(FormCollection form)
        {
            //queue audit id
            var QueueAuditId = form["QueueAuditId_formInsure"];

            if (!string.IsNullOrEmpty(QueueAuditId))
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                //สถานะพิจารณา
                var AuditInsureStatus = form["AuditInsureStatus"];
                if (AuditInsureStatus == null)
                {
                    AuditInsureStatus = form["AuditInsureStatus2"];
                }
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

                var insuredCheck = form["insuredCheck"];
                bool? InsuredCheck = QuestionAuditUpdateV2CheckBoolType(insuredCheck);

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
                if (InsuredCheck == true && auditInsureClose == true) //หน้าติดเงื่อนไข true && และปิดคิวงานเป็น true
                {
                    isSendMemo = null;
                }
                else if (InsuredCheck == true && auditInsureClose == false)
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
                if (InsuredCheck == true)
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


                var result = _context.usp_QueueCIAudit2ResultV3_Insert(
                           queueAuditId,
                           InsuredCheck == true ? auditInsureStatus_close : auditInsureStatusId,
                           InsuredCheck == true ? auditInsureConsiderStatus_close : auditInsureConsiderStatusId,
                           InsuredCheck == true ? AuditInsureConsiderStatusRemark_close : auditInsureConsiderRemark,
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
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_Insure")]
        public ActionResult QueueCancerAuditLog(int queueAuditId, int draw, int? indexStart, int? pageSize, string sortField, string orderType, string search)
        {
            var result = _context.usp_QueueCIAuditLogByQueueAuditId_Select(queueAuditId, indexStart, pageSize, sortField, orderType, search).ToList();
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
        public ActionResult UnderwritePaymentHistory(int queueId, int draw, int? indexStart, int? pageSize, string sortField, string orderType)
        {
            var result = _contextCancer.usp_QueueCIPaymentHistory_Select(9, indexStart, pageSize, sortField, orderType).ToList();

            var dt = new Dictionary<string, object>
            {
                 {"draw", draw },
                 {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                 {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                 {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion API
    }
    public class AuditDto
    {
        public int? UserId { get; set; }
        public int? QueueAuditId { get; set; }
        public int? QueueAuditStatusId { get; set; }
        public int? CallStatusId { get; set; }
        public int? NotAuditCauseId { get; set; }
        public int? SaleMethodTypeId { get; set; }
        public int? SaleServiceTypeId { get; set; }
        public int? UnderwriteByUserId { get; set; }
        public int? ReceivingManualTypeId { get; set; } = 1;
        public int? FeedbackRate { get; set; }
        public int? UnderwritingBehaviorTypeId { get; set; }
        public int? AuditQCUserId { get; set; }
        public int? AuditCIStatusId { get; set; } = 1;
        public int? AuditCISpecifyStatusId { get; set; } = 1;

        public bool? InsuranceInsuredIsValid { get; set; }
        public bool? AddLINEIsSuccess { get; set; }
        public bool? IsReceivedInsurance { get; set; }
        public bool? ServiceResultIsIssue { get; set; }
        public bool? InsuredNameIsValid { get; set; }
        public bool? PayerNameIsValid { get; set; }
        public bool? BirthDateIsValid { get; set; }
        public bool? WeightHeightIsValid { get; set; }
        public bool? IsAllergyHistory { get; set; }
        public bool? IsCriticalIllnessHistory { get; set; }
        public bool? IsAccidentHistory { get; set; }
        public bool? IsSurgeryHistory { get; set; }
        public bool? IsHealthCheckHistory { get; set; }
        public bool? IsChronicDiseaseHistory { get; set; }
        public bool? IsMedicalLatestHistory { get; set; }
        public bool? IsSmokingHistory { get; set; }
        public bool? IsDrinkingHistory { get; set; }
        public bool? IsPregnantHistory { get; set; }
        public bool? IsOtherHealthHistory { get; set; }
        public bool? PayerOccupationIsValid { get; set; }
        public bool? PayerBuildingNameIsValid { get; set; }
        public bool? ProductIsValid { get; set; }
        public bool? PremiumIsValid { get; set; }
        public bool? PeriodTypeIsValid { get; set; }
        public bool? IsVisionValue { get; set; }
        public bool? IsHearingValue { get; set; }
        public bool? PayMethodTypeIsValid { get; set; }

        public string AuditRemark { get; set; }
        public string SaleMethodTypeRemark { get; set; }
        public string SaleServiceTypeRemark { get; set; }
        public string InsuranceInsuredRemark { get; set; }
        public string VisionValueRemark { get; set; }
        public string HearingValueRemark { get; set; }
        public string UnderwriteByUserRemark { get; set; }
        public string FeedbackRemark { get; set; }
        public string ServiceResultRemark { get; set; }
        public string InsuredNameRemark { get; set; }
        public string PayerNameRemark { get; set; }
        public string BirthDateRemark { get; set; }
        public string WeightHeightRemark { get; set; }
        public string AllergyHistoryRemark { get; set; }
        public string CriticalIllnessHistoryRemark { get; set; }
        public string AccidentHistoryRemark { get; set; }
        public string SurgeryHistoryRemark { get; set; }
        public string HealthCheckHistoryRemark { get; set; }
        public string ChronicDiseaseHistoryRemark { get; set; }
        public string MedicalLatestHistoryRemark { get; set; }
        public string SmokingHistoryRemark { get; set; }
        public string DrinkingHistoryRemark { get; set; }
        public string PregnantHistoryRemark { get; set; }
        public string OtherHealthHistoryRemark { get; set; }
        public string PayerOccupationRemark { get; set; }
        public string PayerBuildingNameRemark { get; set; }
        public string ProductRemark { get; set; }
        public string PremiumRemark { get; set; }
        public string PeriodTypeRemark { get; set; }
        public string PayMethodTypeRemark { get; set; }
        public string AuditCIRemark { get; set; }
        public string AuditOtherCIRemark { get; set; }
        public string UnderwritingBehaviorRemark { get; set; }
    }

}