using SmileSIncident.DTOs;
using SmileSIncident.Helper;
using SmileSIncident.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SmileSIncident.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        #region dbCon

        private readonly IncidentEntities _context;
        private static readonly CultureInfo _dtEn = new CultureInfo("en-US");
        private static readonly CultureInfo _dtTh = new CultureInfo("th-TH");

        public HomeController()

        {
            _context = new IncidentEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion dbCon

        #region View

        public async Task<ActionResult> Index(int? taskStatus, int? userId)
        {
            //get user id
            var user = GlobalFunction.GetLoginDetail();
            ViewBag.devId = user.User_ID;

            var statusList = await Task.FromResult(_context.usp_IncidentTaskStatus_Select().ToList());
            ViewBag.statusList = statusList;

            //get incident group type list
            var incidentGroupType = await Task.FromResult(_context.usp_IncidentGroupType_Select(null).ToList());
            ViewBag.IncidentTypeList = incidentGroupType;

            //check if pass some parameter
            if (taskStatus != null) ViewBag.jobSelected = taskStatus;

            if (userId != null) ViewBag.userSelected = userId;

            // Check role dev
            var roles = GlobalFunction.GetRoles();
            var lstRoles = roles.Split(',').ToList();
            var lstIsDev = new List<string>() { "Developer", "IncidentDev" };
            var hasRole = lstIsDev.Any(x => lstRoles.Contains(x));

            if (hasRole) // IT or Dev
            {
                ViewBag.BranchList = _context.usp_Branch_Select(null, null, null, null, null, null).ToList();
                ViewBag.Disable = "";
                ViewBag.IsDev = 1;
            }
            else
            {
                ViewBag.BranchList = _context.usp_Branch_Select(user.Branch_ID, null, null, null, null, null).ToList();
                ViewBag.Disable = "disabled";
                ViewBag.IsDev = 2;
            }

            return View();
        }

        public async Task<ActionResult> IncidentInsert()
        {
            //get user id
            var user = GlobalFunction.GetLoginDetail();
            //insert incident task main
            var result = await Task.FromResult(_context.usp_IncidentTask_Insert(user.User_ID).FirstOrDefault());
            //check if incident task cannot insert
            if (!result.IsResult.Value) return RedirectToAction("InternalServerError", "Error");

            //return as viewbag
            ViewBag.IncidentTaskId = result.IncidentTaskId;
            ViewBag.IncidentMessageId = result.IncidentMessageId;

            //get incident group type list
            var incidentGroupType = await Task.FromResult(_context.usp_IncidentGroupType_Select(null).ToList());
            ViewBag.IncidentTypeList = incidentGroupType;

            return View();
        }

        public async Task<ActionResult> IncidentDetail(int incidentTaskId, int? UserId)
        {
            // อ่าน new message
            _ = await Task.FromResult(_context.usp_NoticeReplyRead_Update(incidentTaskId, UserId));

            var detailList = await Task.FromResult(_context.usp_IncidentTask_SelectById(incidentTaskId).FirstOrDefault());
            ViewBag.detailList = detailList;

            var linkList = await Task.FromResult(_context.usp_IncidentAttachment_Select(detailList.IncidentMessageId, null, null, 999, null, null, null).ToList());
            ViewBag.linkList = linkList;

            //get incident group type list
            var incidentGroupType = await Task.FromResult(_context.usp_IncidentGroupType_Select(null).ToList());
            ViewBag.IncidentTypeList = incidentGroupType;

            var statusJobList = await Task.FromResult(_context.usp_IncidentJobStatus_Select().ToList());
            ViewBag.statusJobList = statusJobList;

            var rejectCaseList = await Task.FromResult(_context.usp_IncidentRejectCase_Select().ToList());
            ViewBag.rejectList = rejectCaseList;

            var replyList = await Task.FromResult(_context.usp_IncidentMessage_Select(incidentTaskId, null, 999, null, null, null).ToList());
            var lstAttachment = await Task.FromResult(_context.usp_IncidentAttachment_Select(null, incidentTaskId, null, 999, null, null, null).ToList());

            var replyxAttachments = new List<IncidentMessagexAttachment_Result>();

            foreach (var rpl in replyList)
            {
                var rplxattactments = new IncidentMessagexAttachment_Result
                {
                    ReplyMessage = rpl
                };

                var attachments = lstAttachment.Where(at => at.IncidentMessageId == rpl.MessageId).ToList();
                if (attachments.Count > 0)
                {
                    rplxattactments.Attachments.AddRange(attachments);
                }
                replyxAttachments.Add(rplxattactments);
            }

            ViewBag.ReplyList = replyxAttachments;

            var lstAssignee = await Task.FromResult(_context.usp_Employee_Select(null, null, "6,7").ToList());
            ViewBag.devList = lstAssignee;

            var user = GlobalFunction.GetLoginDetail();
            ViewBag.User = user;

            // check if has feedback
            var feedback = await Task.FromResult(_context.usp_IncidentFeedback_SelectById(incidentTaskId).FirstOrDefault());
            ViewBag.feedback = feedback;

            // Check role dev
            var roles = GlobalFunction.GetRoles();
            var lstRoles = roles.Split(',').ToList();
            var lstIsDev = new List<string>() { "Developer", "IncidentDev" };

            var hasRole = lstIsDev.Any(x => lstRoles.Contains(x));
            ViewBag.IsDevJob = hasRole ? 1 : 2;
            ViewBag.isDev = hasRole;

            return View();
        }

        [Authorization(Roles = "Developer,IncidentDev")]
        public async Task<ActionResult> JobDetail(int? jobStatus, int? devId)
        {
            //get user id
            var user = GlobalFunction.GetLoginDetail();
            var jobStatusList = await Task.FromResult(_context.usp_IncidentJobStatus_Select().ToList());
            ViewBag.jobStatusList = jobStatusList;

            //check if pass some parameter
            if (jobStatus != null)
            {
                //convert nullable int to int
                ViewBag.jobSelected = new[] { jobStatus ?? default(int) };
            }
            else
            {
                ViewBag.jobSelected = new[] { 1, 2 };
            }
            if (devId != null)
            {
                ViewBag.devSelected = devId;
            }
            else
            {
                ViewBag.devSelected = user.User_ID;
            }

            // Get dev list
            var lstAssignee = await Task.FromResult(_context.usp_Employee_Select(null, null, "6,7").ToList());
            ViewBag.devList = lstAssignee;

            return View();
        }

        public ActionResult JobNewMessage()
        {
            var user = GlobalFunction.GetLoginDetail();
            ViewBag.devId = user.User_ID;
            ViewBag.StatusTask = 2;

            // Check role dev
            var roles = GlobalFunction.GetRoles();
            var lstRoles = roles.Split(',').ToList();
            var lstIsDev = new List<string>() { "Developer", "IncidentDev" };
            var hasRole = lstIsDev.Any(x => lstRoles.Contains(x));

            // signalR
            if (hasRole)
            {
                ViewBag.IsDev = 1;
            }
            else
            {
                ViewBag.IsDev = 2;
            }

            return View();
        }

        public ActionResult TestGmailAPI()
        {
            return View();
        }

        #endregion View

        #region api

        //upload file
        [HttpPost]
        public async Task<ActionResult> FileUpload(HttpPostedFileBase file, int hd_IncidentMsgId, int hd_IncidentId)
        {
            try
            {
                var user = GlobalFunction.GetLoginDetail();

                //open memory stream
                var memStream = new MemoryStream();
                file.InputStream.CopyTo(memStream);
                //get filename
                var fileNameX = Path.GetFileName(file.FileName);

                var fileName = Regex.Replace(fileNameX, "[^<>.0-9a-zA-Zก-๙]+", "");

                //insert get attachment id
                Serilog.Log.Information("Attachment insertion in progress – IncidentMsgId: {IncidentMsgId}", hd_IncidentMsgId);
                var attachIdResult = await Task.FromResult(_context.usp_IncidentAttachment_Insert(hd_IncidentMsgId, user.User_ID).FirstOrDefault());
                Serilog.Log.Information("Attachment insertion in progress – result: {@result}", attachIdResult);

                var path = Path.Combine(Properties.Settings.Default.PathStorage + hd_IncidentId + @"\" + hd_IncidentMsgId + @"\" + attachIdResult.Result);

                //check if directory is exist
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //save file
                file.SaveAs(Path.Combine(path, fileName));

                var attachmentId = Convert.ToInt32(attachIdResult.Result);
                Serilog.Log.Information("Attachment {attachmentId} inserted successfully.", attachmentId);

                Serilog.Log.Information("Attachment update in progress – ID: {attachmentId}", attachmentId);
                var attachResult = await Task.FromResult(_context.usp_IncidentAttachment_Update(attachmentId
                    , Properties.Settings.Default.UrlStorage + "/" + hd_IncidentId + "/" + hd_IncidentMsgId + "/" + attachIdResult.Result + "/" + fileName
                    , Path.Combine(path, fileName), fileName, user.User_ID).FirstOrDefault());

                Serilog.Log.Information("Attachment {attachmentId} updated successfully.", attachmentId);
                return Json(new
                {
                    id = attachIdResult.Result,
                    success = true,
                    response = "File uploaded."
                });
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred while inserting or updating attachment.");
                return Json(new
                {
                    success = false,
                    response = "An error occurred while inserting or updating attachment."
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult> File_delete(int attachId)
        {
            var user = GlobalFunction.GetLoginDetail();
            var result = await Task.FromResult(_context.usp_IncidentAttachment_Delete(attachId, user.User_ID).FirstOrDefault());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // IncidentGroupTypeId: 2 คือ IT
        //update Incident
        //ใส่ [ValidateInput](false) เพื่อไม่ให้ทำการเช็ค rich text format เป็น string
        [HttpPost]
        [ValidateInput(false)]
        public async Task<JsonResult> ComposeIncident_Update(int incidentTaskId, int incidentMsgId, int incidentTypeId, string incidentSubject, string msg)
        {
            try
            {
                var user = GlobalFunction.GetLoginDetail();
                var result = await Task.FromResult(_context.usp_IncidentTask_Update(incidentTaskId, incidentMsgId, incidentTypeId, incidentSubject, msg, user.User_ID).FirstOrDefault());

                if ((bool)result.IsResult)
                {
                    var incidentTask = await Task.FromResult(_context.usp_IncidentTask_SelectById(incidentTaskId).FirstOrDefault());
                    if (incidentTask.IncidentGroupTypeId == 2)
                    {
                        var config = _context.ProgramConfig.FirstOrDefault(x => x.ParameterName == "DiscordWebhooks_IT");
                        var userDetail = await Task.FromResult(_context.usp_EmployeeByUserId_Select(incidentTask.CreatedByUserId).FirstOrDefault());

                        // change uRL before PROD
                        var urlIncident = $"https://incident.siamsmile.co.th/Home/IncidentDetail?incidentTaskId={incidentTaskId}&UserId={userDetail.UserId}";
                        await GlobalFunction.SendDiscordMessage(config.ValueString,
                                 "\n" +
                                 "------------------------------------" +
                                  "\n" +
                                 "📅 วันที่แจ้ง: " + incidentTask.CreatedDate?.ToString("dd MMMM yyyy") + " เวลา " + incidentTask.CreatedDate?.ToString("HH:mm") + "\n" +
                                 "\n" +
                                 "👤 ชื่อผู้แจ้ง: " + userDetail.FullName + "\n" +
                                 "\n" +
                                 "🏢 หน่วยงาน / แผนก: " + userDetail.DepartmentDetail + "\n" +
                                 "\n" +
                                 "🏢 สาขา: " + userDetail.BranchDetail + "\n" +
                                 "\n" +
                                 "📞 เบอร์ติดต่อ: " + userDetail.Mobile + "\n" +
                                 "\n" +
                                 "🆔 รหัสงาน: " + incidentTask.TaskCode + "\n" +
                                 "\n" +
                                 "❗ ปัญหา: " + incidentTask.TaskSubject + "\n" +
                                 "\n" +
                                 $"🔗 [🔎 ดูรายละเอียด]({urlIncident})" + "\n" +
                                 "\n" +
                                 "------------------------------------"
                                 , "🆕 แจ้งปัญหาใหม่เข้าระบบ");
                    }
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred.");
                var result = new { IsResult = false, Result = "Error!", Msg = e.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<JsonResult> IncidentTaskType_Update(int incidentTaskId, int incidentTypeId)
        {
            try
            {
                var user = GlobalFunction.GetLoginDetail();
                var result = await Task.FromResult(_context.usp_IncidentTaskType_Update(incidentTaskId, incidentTypeId, user.User_ID).FirstOrDefault());

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred.");
                var result = new { IsResult = false, Result = "Error!", Msg = e.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> CxlJob_Incident(string IncidentTaskId, string IncidentTypeId, string IncidentMessageId, string IncidentSubject, int IncidentTaskStatusId, string message)
        {
            try
            {
                if (IncidentTaskStatusId == 4)
                {
                    var result = new { IsResult = false, Result = "Error!", Msg = "ไม่สามารถยกเลิกงานได้" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var user = GlobalFunction.GetLoginDetail().User_ID;
                    var result = await Task.FromResult(_context.usp_IncidentTask_UpdateById(Convert.ToInt32(IncidentTaskId), Convert.ToInt32(IncidentMessageId), Convert.ToInt32(IncidentTypeId), IncidentSubject, 4, message, user).FirstOrDefault());

                    var incidentTask = await Task.FromResult(_context.usp_IncidentTask_SelectById(Convert.ToInt32(IncidentTaskId)).FirstOrDefault());
                    if (result.IsResult == true && incidentTask.TaskStatusId == 4 && incidentTask.IncidentGroupTypeId == 2) // ถ้ายกเลิก
                    {
                        var userDetail = await Task.FromResult(_context.usp_EmployeeByUserId_Select(incidentTask.UpdatedByUserId).FirstOrDefault());
                        var config = _context.ProgramConfig.FirstOrDefault(x => x.ParameterName == "DiscordWebhooks_IT");
                        // change uRL before PROD
                        var urlIncident = $"https://incident.siamsmile.co.th/Home/IncidentDetail?incidentTaskId={IncidentTaskId}&UserId={userDetail.UserId}";
                        await GlobalFunction.SendDiscordMessage(config.ValueString,
                                     "\n" +
                                     "------------------------------------" +
                                     "\n" +
                                     "🆔 รหัสงาน: " + incidentTask.TaskCode + "\n" +
                                     "\n" +
                                     "📌 สถานะ: " + incidentTask.TaskStatus + "\n" +
                                     "\n" +
                                     "📅 วันที่ยกเลิกงาน: " + incidentTask.UpdatedDate?.ToString("dd MMMM yyyy") + " เวลา " + incidentTask.UpdatedDate?.ToString("HH:mm") + "\n" +
                                     "\n" +
                                     "📝 หัวข้อ: " + incidentTask.TaskSubject + "\n" +
                                     "\n" +
                                     "👤 ยกเลิกโดย: " + userDetail.FullNamexNickName + "\n" +
                                     "\n" +
                                     $"🔗 [🔎 ดูรายละเอียด]({urlIncident})" + "\n" +
                                     "\n" +
                                     "------------------------------------"
                                     , "🗑 งานนี้ถูกยกเลิกแล้ว");
                    }

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred.");
                var result = new { IsResult = false, Result = "Error!", Msg = e.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> InboxTask_Select(int? statusTaskId,
                                            int? incidentGroupTypeId,
                                            int? incidentTypeId,
                                            int? branchId,
                                            int? draw,
                                            int? indexStart = 0,
                                            int? pageSize = null,
                                            string sortField = null,
                                            string orderType = null,
                                            string search = null,
                                            bool isAdvSearch = false)
        {
            var user = GlobalFunction.GetLoginDetail();

            if (statusTaskId == 0)
                statusTaskId = null;

            if (incidentGroupTypeId == 0)
                incidentGroupTypeId = null;

            if (incidentTypeId == 0)
                incidentTypeId = null;

            var result = await Task.FromResult(_context.usp_IncidentTask_Select_V2(user.User_ID, statusTaskId, incidentGroupTypeId, incidentTypeId, branchId, indexStart, pageSize, sortField, orderType, search, isAdvSearch).ToList());
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result?.Count() != 0 ? result?.FirstOrDefault().Total : result?.Count()},
                {"recordsFiltered", result?.Count() != 0 ? result?.FirstOrDefault().Total : result?.Count()},
                {"data", result?.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetAttachment(int? incidentMsgId, int? incidentTaskId)
        {
            var result = await Task.FromResult(_context.usp_IncidentAttachment_Select(incidentMsgId, incidentTaskId, null, 999, null, null, null).ToList());

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> Insert_Reply(int? incidentTaskId)
        {
            var user = GlobalFunction.GetLoginDetail();
            var result = await Task.FromResult(_context.usp_IncidentMessage_Insert(incidentTaskId, user.User_ID).FirstOrDefault());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<JsonResult> Update_Reply(int? msgId, string msg)
        {
            var user = GlobalFunction.GetLoginDetail();
            var result = await Task.FromResult(_context.usp_IncidentMessage_Update(msgId, msg, user.User_ID).FirstOrDefault());

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetJob_dt(int? incidentTaskId, int? draw,
                                            int? indexStart = 0,
                                            int? pageSize = null,
                                            string sortField = null,
                                            string orderType = null,
                                            string search = null)
        {
            var result = await Task.FromResult(_context.usp_IncidentJob_Select(incidentTaskId, null, indexStart, pageSize, sortField, orderType, search).ToList());
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().Total : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().Total : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> Insert_Tag(int jobId, int? tagId)
        {
            var user = GlobalFunction.GetLoginDetail();
            var result = await Task.FromResult(_context.usp_IncidentTag_Insert(jobId, tagId, true, user.User_ID).FirstOrDefault());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetTagById(int jobId)
        {
            var result = await Task.FromResult(_context.usp_IncidentTag_SelectById(jobId).ToList());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetJobDetail(int jobId)
        {
            var result = await Task.FromResult(_context.usp_IncidentJob_SelectById(jobId).FirstOrDefault());

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetProjectList()
        {
            var projectList = await Task.FromResult(_context.usp_IncidentProject_Select().ToList());
            return Json(projectList, JsonRequestBehavior.AllowGet);
        }

        // TaskStatusId: 3 คือสถานะ สำเร็จ
        // IncidentGroupTypeId: 2 คือ IT
        [HttpPost]
        public async Task<JsonResult> Insert_Job(int? incidentTaskId, int? actionByUserId, string description, int? jobStatus, string actionRemark, int? rejectCaseId)
        {
            var user = GlobalFunction.GetLoginDetail();
            var result = await Task.FromResult(_context.usp_IncidentJob_Insert(incidentTaskId, actionByUserId, description, actionRemark, rejectCaseId, jobStatus, user.User_ID).FirstOrDefault());

            if ((bool)result.IsResult)
            {
                var config = _context.ProgramConfig.FirstOrDefault(x => x.ParameterName == "DiscordWebhooks_IT");
                var incidentTask = await Task.FromResult(_context.usp_IncidentTask_SelectById(incidentTaskId).FirstOrDefault());
                var urlIncident = $"https://incident.siamsmile.co.th/Home/IncidentDetail?incidentTaskId={incidentTaskId}";
                if (incidentTask.IncidentGroupTypeId == 2)
                {
                    var userDetail = await Task.FromResult(_context.usp_EmployeeByUserId_Select(actionByUserId).FirstOrDefault());
                    await GlobalFunction.SendDiscordMessage(config.ValueString,
                             "\n" +
                             "------------------------------------" +
                             "\n" +
                             "🆔 รหัสงาน: " + incidentTask.TaskCode + "\n" +
                              "\n" +
                             "📝 หัวข้อ: " + incidentTask.TaskSubject + "\n" +
                             "\n" +
                             "📂 ประเภท: " + incidentTask.IncidentType + "\n" +
                             "\n" +
                             "📌 สถานะ: " + incidentTask.TaskStatus + "\n" +
                             "\n" +
                             "👤 ผู้รับผิดชอบ: " + userDetail.FullNamexNickName + "\n" +
                             "\n" +
                             $"🔗 [🔎 ดูรายละเอียด]({urlIncident})" + "\n" +
                             "\n" +
                             "------------------------------------"
                             , "📤 มีการมอบหมายงานใหม่");
                }

                if (incidentTask.TaskStatusId == 3 && incidentTask.IncidentGroupTypeId == 2)
                {
                    var incidentJob = _context.IncidentJob.Where(x => x.IncidentTaskId == incidentTaskId && x.IncidentJobStatusId == 3).OrderByDescending(x => x.UpdatedDate).FirstOrDefault();
                    var userDetail = await Task.FromResult(_context.usp_EmployeeByUserId_Select(incidentJob.ActionByUserId).FirstOrDefault());

                    await GlobalFunction.SendDiscordMessage(config.ValueString,
                                 "\n" +
                                 "------------------------------------" +
                                 "\n" +
                                 "🆔 รหัสงาน: " + incidentTask.TaskCode + "\n" +
                                  "\n" +
                                 "📌 สถานะ: " + incidentTask.TaskStatus + "\n" +
                                 "\n" +
                                 "📅 วันที่ปิดงาน: " + incidentTask.UpdatedDate?.ToString("dd MMMM yyyy") + " เวลา " + incidentTask.UpdatedDate?.ToString("HH:mm") + "\n" +
                                 "\n" +
                                 "📝 หัวข้อ: " + incidentTask.TaskSubject + "\n" +
                                 "\n" +
                                 "👤 ผู้ดูแล: " + userDetail.FullNamexNickName + "\n" +
                                 "\n" +
                                 $"🔗 [🔎 ดูรายละเอียด]({urlIncident})" + "\n" +
                                 "\n" +
                                 "------------------------------------"
                                 , "✅ งานได้รับการปิดเรียบร้อย");
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // TaskStatusId: 3 คือสถานะ สำเร็จ
        // IncidentGroupTypeId: 2 คือ IT
        [HttpPost]
        public async Task<JsonResult> Job_Update(int? incidentTaskId, int? jobId, int jobStatusId, string description, string actionRemark, int? rejectCaseId, int? trId)
        {
            var user = GlobalFunction.GetLoginDetail();
            usp_IncidentJob_Update_Result result = new usp_IncidentJob_Update_Result();
            if (trId == 0)
                trId = null;

            if (jobStatusId == 4)
                result = await Task.FromResult(_context.usp_IncidentJob_Update(jobId, jobStatusId, description, actionRemark, rejectCaseId, user.User_ID, trId).FirstOrDefault());
            else
                result = await Task.FromResult(_context.usp_IncidentJob_Update(jobId, jobStatusId, description, actionRemark, rejectCaseId, user.User_ID, null).FirstOrDefault());

            var incidentTask = await Task.FromResult(_context.usp_IncidentTask_SelectById(incidentTaskId).FirstOrDefault());
            if ((bool)result.IsResult && incidentTask.IncidentGroupTypeId == 2)
            {
                var config = _context.ProgramConfig.FirstOrDefault(x => x.ParameterName == "DiscordWebhooks_IT");
                var urlIncident = $"https://incident.siamsmile.co.th/Home/IncidentDetail?incidentTaskId={incidentTaskId}";
                if (incidentTask.TaskStatusId == 3)
                {
                    var incidentJob = _context.IncidentJob.Where(x => x.IncidentTaskId == incidentTaskId && x.IncidentJobStatusId == 3).OrderByDescending(x => x.UpdatedDate).FirstOrDefault();
                    var userDetail = await Task.FromResult(_context.usp_EmployeeByUserId_Select(incidentJob.ActionByUserId).FirstOrDefault());
                    await GlobalFunction.SendDiscordMessage(config.ValueString,
                                    "\n" +
                                    "------------------------------------" +
                                    "\n" +
                                    "🆔 รหัสงาน: " + incidentTask.TaskCode + "\n" +
                                    "\n" +
                                    "📌 สถานะ: " + incidentTask.TaskStatus + "\n" +
                                    "\n" +
                                    "📅 วันที่ปิดงาน: " + incidentTask.UpdatedDate?.ToString("dd MMMM yyyy") + "\n" +
                                    "\n" +
                                    "⏰ เวลา: " + incidentTask.UpdatedDate?.ToString("HH:mm") + "\n" +
                                    "\n" +
                                    "📝 หัวข้อ: " + incidentTask.TaskSubject + "\n" +
                                    "\n" +
                                    "👤 ผู้รับผิดชอบ: " + userDetail.FullNamexNickName + "\n" +
                                    "\n" +
                                    $"🔗 [🔎 ดูรายละเอียด]({urlIncident})" + "\n" +
                                    "\n" +
                                    "------------------------------------"
                                    , "✅ งานได้รับการปิดเรียบร้อย");
                }
                else
                {
                    var incidentJob = _context.IncidentJob.Where(x => x.IncidentTaskId == incidentTaskId).OrderByDescending(x => x.UpdatedDate).FirstOrDefault();
                    var jobStatus = _context.usp_IncidentJobStatus_Select().ToList().Where(x => x.JobStatusId == incidentJob.IncidentJobStatusId).FirstOrDefault();
                    var userDetail = await Task.FromResult(_context.usp_EmployeeByUserId_Select(incidentJob.ActionByUserId).FirstOrDefault());
                    await GlobalFunction.SendDiscordMessage(config.ValueString,
                                "\n" +
                                "------------------------------------" +
                                 "\n" +
                                 "📌 สถานะ: " + incidentTask.TaskStatus + "\n" +
                                 "\n" +
                                 "📌 สถานะงานย่อย: " + jobStatus.JobStatus + "\n" +
                                "\n" +
                                "📝 หัวข้อ: " + incidentTask.TaskSubject + "\n" +
                                "\n" +
                                "👤 ผู้รับผิดชอบ: " + userDetail.FullNamexNickName + "\n" +
                                "\n" +
                                $"🔗 [🔎 ดูรายละเอียด]({urlIncident})" + "\n" +
                                "\n" +
                                "------------------------------------"
                                , "✏️ มีการอัปเดตสถานะงาน");
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> Tag_Update(int? jobId, int? tagId, bool isChecked)
        {
            var user = GlobalFunction.GetLoginDetail();
            var result = await Task.FromResult(_context.usp_IncidentTag_Update(jobId, tagId, isChecked, user.User_ID).FirstOrDefault());

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task Read_Update(int incidentTaskId, int? typeId)
        {
            var user = GlobalFunction.GetLoginDetail();
            await Task.FromResult(_context.usp_ReadAndUnRead_Update(incidentTaskId, true, user.User_ID, typeId));
        }

        [HttpGet]
        public async Task UnRead_Update(int incidentTaskId, int? typeId)
        {
            var user = GlobalFunction.GetLoginDetail();
            await Task.FromResult(_context.usp_ReadAndUnRead_Update(incidentTaskId, false, user.User_ID, typeId));
        }

        [HttpPost]
        public async Task<JsonResult> Feedback_Insert(int incidentTaskId, string feedback, float score)
        {
            var user = GlobalFunction.GetLoginDetail();
            var result = await Task.FromResult(_context.usp_IncidentFeedback_Insert(incidentTaskId, feedback, score, user.User_ID).FirstOrDefault());

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> Message_Dt(int? CreatedById,
                                        int? draw,
                                        int? StatusTaskId,
                                        int? IncidentTypeId,
                                        int? IndexStart = 0,
                                        int? PageSize = null,
                                        string SortField = null,
                                        string OrderType = null,
                                        string SearchDetail = null)
        {
            var result = await Task.FromResult(_context.usp_NoticeReplyDetail_Select(CreatedById, StatusTaskId, IncidentTypeId, IndexStart, PageSize, SortField, OrderType, SearchDetail).ToList());

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().Total : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().Total : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> JobTask_Dt(string jobStatusArr,
                                    string devArr,
                                    int? draw,
                                    int? indexStart = 0,
                                    int? pageSize = null,
                                    string sortField = null,
                                    string orderType = null,
                                    string search = null)
        {
            var devArrList = devArr.Split(',');
            int all = Array.IndexOf(devArrList, "ALL");
            if (all >= 0)
            {
                //ส่งค่า null
                devArr = null;
            }

            var result = await Task.FromResult(_context.usp_IncidentJob_Monitor_Select(jobStatusArr, devArr, indexStart, pageSize, sortField, orderType, search).ToList());

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
        public async Task<JsonResult> JobMonitor_DT(string devArr,
                                    int? draw,
                                    int? indexStart = 0,
                                    int? pageSize = null,
                                    string sortField = null,
                                    string orderType = null,
                                    string search = null)
        {
            var devArrList = devArr.Split(',');
            int all = Array.IndexOf(devArrList, "ALL");
            if (all >= 0)
            {
                //ส่งค่า null
                devArr = null;
            }

            var result = await Task.FromResult(_context.usp_IncidentJob_MonitorCount_Select(devArr, indexStart, pageSize, sortField, orderType, search).ToList());

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetIncidentGroupType(int incidentGroupTypeId)
        {
            var rs = await Task.FromResult(_context.usp_IncidentTypeByGroupId_Select(incidentGroupTypeId).ToList());
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        #endregion api
    }
}