using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RoadsideAssistance.Helper;
using RoadsideAssistance.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace RoadsideAssistance.Controllers
{
    [Authorization(Roles = "Developer,RoadsideAssistance_Callcenter,RoadsideAssistance_Branch,RoadsideAssistance_CallcenterAdmin")]
    public class RoadsideAssistanceController : Controller
    {
        private readonly RoadsideAssistanceEntities _context;

        public RoadsideAssistanceController()
        {
            _context = new RoadsideAssistanceEntities();
        }

        // GET: RoadsideAssistance
        public ActionResult Index()
        {
            var getYear = DateTime.Now.Year;
            var getMonth = DateTime.Now.Month;
            var getDay = DateTime.Now.Day;
            var YearTH = getYear;
            ViewBag.today = String.Format("{0}/{1}/{2}", getDay, getMonth, YearTH + 543);

            //Get Login Detail
            var userDetail = RoadsideAssistance.Helper.OAuth2Helper.GetLoginDetail();
            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();

            //เช็คสิทธดูได้อย่างเดียว
            ViewBag.SeeOnly = false;
            var roleBranch = new[] { "RoadsideAssistance_Branch" };
            if (rolelist.Intersect(roleBranch).Count() > 0) ViewBag.SeeOnly = true;

            ViewBag.Fristname = userDetail.FirstName;
            ViewBag.Lastname = userDetail.LastName;
            ViewBag.EmpCode = userDetail.UserName;
            ViewBag.Department = userDetail.DepartmentDetail;
            ViewBag.BranchDetail = userDetail.BranchDetail;
            ViewBag.EmpId = userDetail.User_ID;

            ViewBag.Title_Select = _context.usp_Title_Select(0, 99, null, null, null).ToList();
            ViewBag.PersonCardType_Select = _context.usp_PersonCardType_Select(0, 2, null, null, null).ToList();
            ViewBag.SourceType = _context.usp_SourceType_Select(null, 0, 999, null, null, null).ToList();
            return View();
        }

        [Authorization(Roles = "Developer,RoadsideAssistance_Callcenter")]
        public ActionResult UseRoadsideAssis(string memberid)
        {
            var memberIdBase64EncodedBytes = Convert.FromBase64String(memberid);
            var memberId = System.Text.Encoding.UTF8.GetString(memberIdBase64EncodedBytes);

            var Member_Id = Convert.ToInt32(memberId);

            var result = _context.usp_Member_Select(Member_Id, null, null, null, null, null).ToList();
            ViewBag.useRoadsside_result = result;

            //Json  Serialize

            var js = new JavaScriptSerializer();
            var sourceType = js.Deserialize<SourceTypeObject[]>(result[0].SourceTypeIdList);
            ViewBag.SourceTypeList = sourceType;

            ViewBag.Province = _context.usp_Province_Select(0, 999, null, null, null);
            ViewBag.Service = _context.usp_RoadsideAssistanceServiceType_Select(0, 50, null, null, null);

            var getYear = DateTime.Now.Year;
            var getMonth = DateTime.Now.Month;
            var getDay = DateTime.Now.Day;
            var YearTH = getYear + 543;
            ViewBag.today = getDay + "-" + getMonth + "-" + YearTH;

            var time = DateTime.Now.ToString("HH:mm:ss");
            ViewBag.time = time;

            return View();
        }

        public ActionResult FollowRoadsideAssis(int? RoadsideAssistanceId, int? roadsideStatusId, string sortField, string orderType)
        {
            var result = _context.usp_RoadsideAssistance_Select(RoadsideAssistanceId, roadsideStatusId, null, null, null, sortField, orderType, null).ToList();
            ViewBag.RoadsideAssisresult = result;

            return View();
        }

        public ActionResult HistoryAssistance(string memberid)
        {
            var memberIdBase64EncodedBytes = Convert.FromBase64String(memberid);
            var memberId = System.Text.Encoding.UTF8.GetString(memberIdBase64EncodedBytes);

            var Member_Id = Convert.ToInt32(memberId);

            var result = _context.usp_Member_Select(Member_Id, null, null, null, null, null).ToList();
            ViewBag.Member_result = result;

            var MemberRoadside_result = _context.usp_RoadsideAssistance_Select(null, null, Member_Id, null, null, null, null, null).ToList();
            ViewBag.MemberRoadside_result = MemberRoadside_result;
            var resultCloseJob = _context.usp_RoadsideAssistance_Select(null, null, Member_Id, null, null, null, null, null).FirstOrDefault();
            ViewBag.CancelServiceRemark = resultCloseJob.CancelServiceRemark.Replace(" ", "");

            //วัน-เวลา Created
            if (MemberRoadside_result.Count > 0)
            {
                var MemberRoadside = MemberRoadside_result[0].CreatedDate;
                if (MemberRoadside != null)
                {
                    var CreatedDate = string.Format("{0}/{1}/{2}", MemberRoadside.Value.Day, MemberRoadside.Value.Month, MemberRoadside.Value.Year + 543);
                    var CreatedTime = string.Format("{0}:{1}:{2}", MemberRoadside.Value.Hour, MemberRoadside.Value.Minute, MemberRoadside.Value.Second);
                    ViewBag.CreatedDate = CreatedDate;
                    ViewBag.CreatedTime = CreatedTime;
                }
                else
                {
                    ViewBag.CreatedDate = null;
                    ViewBag.CreatedTime = null;
                }
            }

            //วัน-เวลา ปิดงาน
            if (MemberRoadside_result.Count > 0)
            {
                var MemberRoadside = MemberRoadside_result[0].ReferanceClosedDate;
                if (MemberRoadside != null)
                {
                    var CreatedDate = string.Format("{0}/{1}/{2}", MemberRoadside.Value.Day, MemberRoadside.Value.Month, MemberRoadside.Value.Year + 543);
                    var CreatedTime = string.Format("{0}:{1}:{2}", MemberRoadside.Value.Hour, MemberRoadside.Value.Minute, MemberRoadside.Value.Second);
                    ViewBag.ClosedDate = CreatedDate;
                    ViewBag.ClosedTime = CreatedTime;
                }
                else
                {
                    ViewBag.ClosedDate = null;
                    ViewBag.ClosedTime = null;
                }
            }

            //วันที่แจ้งเหตุ
            if (MemberRoadside_result.Count > 0)
            {
                var dateHappen = MemberRoadside_result[0].DateHappen;
                if (dateHappen != null)
                {
                    var strDate = string.Format("{0}/{1}/{2}", dateHappen.Value.Day, dateHappen.Value.Month, dateHappen.Value.Year + 543);
                    var strTime = string.Format("{0}:{1}:{2}", dateHappen.Value.Hour, dateHappen.Value.Minute, dateHappen.Value.Second);
                    ViewBag.dateHappen = strDate;
                    ViewBag.timeHappen = strTime;
                }
                else
                {
                    ViewBag.dateHappen = null;
                    ViewBag.timeHappen = null;
                }
            }

            var js = new JavaScriptSerializer();
            if (result[0].SourceTypeIdList != null)
            {
                var sourceType = js.Deserialize<SourceTypeObject[]>(result[0].SourceTypeIdList);
                ViewBag.SourceTypeList = sourceType;
            }
            else
            {
                ViewBag.SourceTypeList = "";
            }

            return View();
        }

        [Authorization(Roles = "Developer,RoadsideAssistance_Callcenter,RoadsideAssistance_CallcenterAdmin")]
        public ActionResult TakeActionCloseJob(string roadsideId, string memberId, string viewOnly)
        {
            var memberIdBase64EncodedBytes = Convert.FromBase64String(Convert.ToString(memberId));
            var memberid = System.Text.Encoding.UTF8.GetString(memberIdBase64EncodedBytes);
            var Member_Id = Convert.ToInt32(memberid);

            var roadsideIdBase64EncodedBytes = Convert.FromBase64String(Convert.ToString(roadsideId));
            var roadsideid = System.Text.Encoding.UTF8.GetString(roadsideIdBase64EncodedBytes);
            var roadside_id = Convert.ToInt32(roadsideid);

            ViewBag.Service = _context.usp_RoadsideAssistanceServiceType_Select(0, 50, null, null, null).ToList();
            ViewBag.Province = _context.usp_Province_Select(0, 999, null, null, null);

            var resultMemberSelect = _context.usp_Member_Select(Member_Id, null, null, null, null, null).ToList();
            ViewBag.MemberSelect = resultMemberSelect;
            var ServiceCancelCause = _context.usp_ServiceCancelCause_Select(null).ToList();
            ViewBag.ServiceCancelCause = ServiceCancelCause;

            var userDetail = RoadsideAssistance.Helper.OAuth2Helper.GetLoginDetail();
            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();
            ViewBag.rolelist = rolelist;

            ViewBag.DueDateEdite = Properties.Settings.Default.DueDate;

            //date time Cover Date
            var StartCoverDate = resultMemberSelect[0].StartCoverDate;
            if (StartCoverDate != null)
            {
                var strCoverDate = string.Format("{0}-{1}-{2}", StartCoverDate.Value.Day, StartCoverDate.Value.Month, StartCoverDate.Value.Year + 543);
                ViewBag.strCoverDate = strCoverDate;
            }

            var resultCloseJob = _context.usp_RoadsideAssistance_Select(roadside_id, null, null, null, null, null, null, null).FirstOrDefault();
            ViewBag.resultCloseJob = resultCloseJob;
            ViewBag.ServiceCancelCauseDetail = resultCloseJob.ServiceCancelCauseDetail;
            ViewBag.ServiceCancelCauseId = resultCloseJob.ServiceCancelCauseId;
            ViewBag.CancelServiceRemark = resultCloseJob.CancelServiceRemark.Replace(" ", "");
            ViewBag.TypeUse = resultCloseJob.UseService;

            if (resultCloseJob.IsEmployeePointLocked == null)
            {
                ViewBag.IsEmployeePointLocked = false;
            }
            else
            {
                ViewBag.IsEmployeePointLocked = true;
            }

            //date time Created job
            var CreatedDate = resultCloseJob.CreatedDate;
            if (CreatedDate != null)
            {
                var strCreatedDate = string.Format("{0}-{1}-{2}", CreatedDate.Value.Day, CreatedDate.Value.Month, CreatedDate.Value.Year + 543);
                ViewBag.strCreatedDate = strCreatedDate;

                //เช็ควันที่แก้ไข dropdown ประเภทการใช้สิทธิ์ ได้  ไม่ได้ สถานะสำเร็จ ปุ่มแก้ไข
                var CreatedDatecheck = string.Format("{0}-{1}-{2}", CreatedDate.Value.Day, CreatedDate.Value.Month, CreatedDate.Value.Year);
                ViewBag.CreatedDatecheck = CreatedDatecheck;
                ViewBag.CreatedDatecheckDay = string.Format("{0}", CreatedDate.Value.Day);
                ViewBag.CreatedDatecheckMonth = string.Format("{0}", CreatedDate.Value.Month);
                ViewBag.CreatedDatecheckYear = string.Format("{0}", CreatedDate.Value.Year);
            }

            //date time Close job
            var refCloseDate = resultCloseJob.ReferanceClosedDate;
            if (refCloseDate != null)
            {
                var strDate = string.Format("{0}/{1}/{2}", refCloseDate.Value.Day, refCloseDate.Value.Month, refCloseDate.Value.Year + 543);
                var strTime = string.Format("{0}:{1}:{2}", refCloseDate.Value.Hour, refCloseDate.Value.Minute, refCloseDate.Value.Second);
                ViewBag.refcloseDate = strDate;
                ViewBag.refcloseTime = strTime;
            }

            //วันที่แจ้งเหตุ
            var dateHappen = resultCloseJob.DateHappen;
            if (dateHappen != null)
            {
                var strDate = string.Format("{0}/{1}/{2}", dateHappen.Value.Day, dateHappen.Value.Month, dateHappen.Value.Year + 543);
                var strTime = string.Format("{0}:{1}:{2}", dateHappen.Value.Hour, dateHappen.Value.Minute, dateHappen.Value.Second);
                ViewBag.dateHappen = strDate;
                ViewBag.timeHappen = strTime;
            }
            else
            {
                ViewBag.dateHappen = null;
                ViewBag.timeHappen = null;
            }

            ViewBag.IsEmployee = false;
            var js = new JavaScriptSerializer();
            if (resultMemberSelect[0].SourceTypeIdList != null)
            {
                var sourceType = js.Deserialize<SourceTypeObject[]>(resultMemberSelect[0].SourceTypeIdList);
                ViewBag.SourceTypeList = sourceType;

                foreach (var item in sourceType)
                {
                    if (item.SourceTypeId.Contains("11") || item.SourceTypeId.Contains("12"))
                    {
                        ViewBag.IsEmployee = true;
                    }
                }
            }
            else
            {
                ViewBag.SourceTypeList = "";
            }

            if (viewOnly != "")
            {
                ViewBag.viewOnly = "Y";
            }
            else
            {
                ViewBag.viewOnly = "";
            }

            return View();
        }

        //Import customer
        public ActionResult ImportRoadsideAssis()
        {
            ViewBag.excelData = new List<MemberImport>();
            //Get Login Detail
            var userDetail = RoadsideAssistance.Helper.OAuth2Helper.GetLoginDetail();
            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();
            ViewBag.Roles = rolelist;
            ViewBag.SourceType = _context.usp_SourceType_Select(null, 0, 999, null, null, null).ToList();
            return View();
        }

        //import customer CXL
        public ActionResult ImporrtCxlRoadsideAssis()
        {
            ViewBag.excelData = new List<MemberImport>();

            ViewBag.SourceType = _context.usp_SourceType_Select(null, 0, 999, null, null, null).ToList();

            return View();
        }

        [Authorization(Roles = "Developer,RoadsideAssistance_CallcenterAdmin")]
        public ActionResult EmployeePointLocked()
        {
            return View();
        }

        #region funtion

        //DT หักแต้มพนักงาน
        [HttpPost]
        public JsonResult EmployeePointLocked_DT(string fromDate, string toDate, int? draw, int? indexStart = 0, int? pageSize = null, string sortField = null, string orderType = null)
        {
            var fdate = Convert.ToDateTime(fromDate);
            var lasttdateDayOfMonth = DateTime.DaysInMonth(fdate.Year, fdate.Month);
            var formathdate = new DateTime(fdate.Year, fdate.Month, lasttdateDayOfMonth);

            var result = _context.usp_EmployeePointLocked_Select(
                                                                fdate,
                                                                formathdate,
                                                                indexStart,
                                                                pageSize,
                                                                sortField,
                                                                orderType
                                                                ).ToList();

            var count = result.Where(A => A.IsEmployeePointLocked != true).Count();

            var dt = new Dictionary<string, object>
                    {
                        {"draw", draw },
                        {"count",count },
                        {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"data", result.ToList()}
                    };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        //Lock หักแต้มพนักงาน
        [HttpPost]
        public ActionResult UpdateEmployeePointLockedAll(string fromDate, string toDate)
        {
            try
            {
                var fdate = Convert.ToDateTime(fromDate);
                var lasttdateDayOfMonth = DateTime.DaysInMonth(fdate.Year, fdate.Month);
                var formathdate = new DateTime(fdate.Year, fdate.Month, lasttdateDayOfMonth);

                int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var result = _context.usp_EmployeePointLockedAll_Update(fdate, formathdate, userId).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpPost]
        public JsonResult GetDataMember(string firstName,
                                        string lastName,
                                        string cardDetail,
                                        string buildingName,
                                        int? cardDetailTypeId,
                                        int? draw,
                                        int? indexStart = 0,
                                        int? pageSize = null,
                                        string sortField = null,
                                        string orderType = null,
                                        string searchDetail = null)
        {
            var result = _context.usp_MemberSearch_Select(firstName == "" ? null : firstName, lastName == "" ? null : lastName, cardDetail, cardDetailTypeId, buildingName == "" ? null : buildingName, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

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
        public JsonResult GetDataRoadsideSatatus(string roadsideStatusId,
                                                    int? draw,
                                                    int? indexStart = 0,
                                                    int? pageSize = null,
                                                    string sortField = null,
                                                    string orderType = null,
                                                    string searchDetail = null)
        {
            var result = _context.usp_RoadsideAssistance_Select(null,
                                                                Convert.ToInt32(roadsideStatusId),
                                                                null,
                                                                indexStart,
                                                                pageSize,
                                                                sortField,
                                                                orderType,
                                                                searchDetail).ToList();

            var dt = new Dictionary<string, object>
                    {
                        {"draw", draw },
                        {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"data", result.ToList()}
                    };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TakeActionCloseJobInsert(string roadsideId,
                                                   string contactNumber,
                                                   string vehicleRegistrationDetail,
                                                   string vehicleRegistrationNumber,
                                                   string vehicleRegistrationProvinceId,
                                                   string locationDetail,
                                                   string remark,
                                                   string roadsideAssistanceSeviceTypeId,
                                                   string roadsideAssistanceStatusId,
                                                   string referanceCode,
                                                   string totalAmount,
                                                   string customerAmount,
                                                   string employeePointAmount,
                                                   string referanceName,
                                                   string createdByUserId,
                                                   string referanceClosedDate,
                                                   string referanceClosedTime,
                                                   string isService,
                                                   string cancelServiceRemark,
                                                   string cancelRemark,
                                                   string dateHappent,
                                                   string timeHappent,
                                                   string serviceCancelCauseId,
                                                   string otherServiceCancelRemark)
        {
            try
            {
                //วัน-เวลา ที่แจ้งเหตุ
                DateTime? date_timeHappent = null;
                if (dateHappent.Trim() != "" && timeHappent.Trim() != "") date_timeHappent = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateHappent, timeHappent));

                DateTime? refClosedDate = null;
                decimal total;
                decimal customertotal;
                if (referanceClosedDate.Trim() != "" && referanceClosedTime.Trim() != "") refClosedDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(referanceClosedDate, referanceClosedTime));
                total = Convert.ToDecimal(totalAmount);
                customertotal = Convert.ToDecimal(customerAmount);
                //var serviceCancelId = (!String.IsNullOrEmpty(serviceCancelCauseId) ? Convert.ToInt32(serviceCancelCauseId) : );
                //if (total < customertotal)
                //{
                //    return Json(new { IsResult = false, Msg = $"Error Message : ค่าบริการทั้งหมดน้อยกว่ายอดที่ลูกค่าชำระเอง" }, JsonRequestBehavior.AllowGet);
                //}
                var rs = _context.usp_RoadsideAssistance_Update(Convert.ToInt32(roadsideId),
                                                                       contactNumber,
                                                                       vehicleRegistrationDetail,
                                                                       vehicleRegistrationNumber,
                                                                       Convert.ToInt32(vehicleRegistrationProvinceId),
                                                                       locationDetail,
                                                                       remark,
                                                                       Convert.ToInt32(roadsideAssistanceSeviceTypeId),
                                                                       Convert.ToInt32(roadsideAssistanceStatusId),
                                                                       referanceCode,
                                                                       referanceName,
                                                                       total,
                                                                       customertotal,
                                                                       Convert.ToDecimal(employeePointAmount),
                                                                       Convert.ToInt32(createdByUserId),
                                                                       refClosedDate, Convert.ToBoolean(Convert.ToInt32(isService)), cancelServiceRemark, cancelRemark, date_timeHappent,
                                                                       Convert.ToInt32(serviceCancelCauseId), otherServiceCancelRemark).Single();
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsResult = false, Msg = $"Error Message : {e.Message} :{e.InnerException}" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UseRoadsideAssisInsert(string memberid,
                                                    string contactNumber,
                                                    string vehicleRegistrationDetail,
                                                    string vehicleRegistrationNumber,
                                                    string vehicleRegistrationProvinceId,
                                                    string locationDetail,
                                                    string remark,
                                                    string roadsideAssistanceSeviceTypeId,
                                                    string payTypeId,
                                                    string createdByUserId,
                                                    string dateHappent,
                                                    string timeHappent,
                                                    int? serviceCancelCauseId,
                                                    string otherServiceCancelRemark)
        {
            try
            {
                //วัน-เวลา ที่แจ้งเหตุ
                DateTime? date_timeHappent = null;
                if (dateHappent.Trim() != "" && timeHappent.Trim() != "") date_timeHappent = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateHappent, timeHappent));

                var rs = _context.usp_RoadsideAssistance_Insert(Convert.ToInt32(memberid),
                                                                contactNumber,
                                                                vehicleRegistrationDetail,
                                                                vehicleRegistrationNumber,
                                                                Convert.ToInt32(vehicleRegistrationProvinceId),
                                                                locationDetail,
                                                                remark,
                                                                Convert.ToInt32(roadsideAssistanceSeviceTypeId),
                                                                Convert.ToInt32(payTypeId),
                                                                date_timeHappent,
                                                                null,
                                                                null,
                                                                null,
                                                                Convert.ToInt32(createdByUserId), Convert.ToInt32(serviceCancelCauseId), otherServiceCancelRemark).SingleOrDefault();

                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsResult = false, Msg = $"Error Message : {e.Message} :{e.InnerException}" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult MemberRoadsideAssistanceInsert(string sourceTypeId,
                                                           string referenceCode,
                                                           string cardDetailTypeId,
                                                           string cardDetail,
                                                           string titleId,
                                                           string firstName,
                                                           string lastName,
                                                           string mobileNumber,
                                                           string buildingName,
                                                           string startCoverDate,
                                                           string endCoverDate)
        {
            try
            {
                //Get Login Detail
                var userDetail = RoadsideAssistance.Helper.OAuth2Helper.GetLoginDetail();

                //Declare cVariable *c=convert
                DateTime? cStartCoverDate = null;
                DateTime? cEndCoverDate = null;

                //Check String.Empty
                if (startCoverDate.Trim() != "") cStartCoverDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(startCoverDate, null));
                if (endCoverDate.Trim() != "") cEndCoverDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(endCoverDate, null));

                var rs = _context.usp_StagingSourceDirect_Insert(Convert.ToInt32(sourceTypeId),
                                                               referenceCode,
                                                               Convert.ToInt32(cardDetailTypeId),
                                                               cardDetail,
                                                               Convert.ToInt32(titleId),
                                                               firstName,
                                                               lastName,
                                                               mobileNumber,
                                                               buildingName,
                                                               cStartCoverDate,
                                                               cEndCoverDate,
                                                               userDetail.User_ID).SingleOrDefault();

                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsResult = false, Msg = $"Error Message : {e.Message} :{e.InnerException}" }, JsonRequestBehavior.AllowGet);
            }
        }

        //import customer
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase[] file, string[] txtSourceTypeId, string actionType)
        {
            //Get Login Detail
            var userDetail = RoadsideAssistance.Helper.OAuth2Helper.GetLoginDetail();
            var excelList = new List<MemberImport>();
            var excelListPA = new List<ImportPA>();
            if (txtSourceTypeId == null)
            {
                return Json(new { IsResult = false, Msg = "ประเภทลูกค้าไม่ถูกต้อง" }, JsonRequestBehavior.AllowGet);
            }

            if (actionType == null)
            {
                return Json(new { IsResult = false, Msg = "ประเภทการอัพโหลดไม่ถูกต้อง" }, JsonRequestBehavior.AllowGet);
            }

            for (int f = 0; f < file.Count(); f++)
            {
                if (file[f] == null || file[f].ContentLength <= 0)
                {
                    return Json(new { IsResult = false, Msg = "กรุณาเลือกไฟล์ก่อนอัพโหลด" }, JsonRequestBehavior.AllowGet);
                }
                if (!Path.GetExtension(file[f].FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return Json(new { IsResult = false, Msg = "ประเภทของไฟล์ไม่ถูกต้อง" }, JsonRequestBehavior.AllowGet);
                }
                var fileDoc = Request.Files[f].FileName;
                var getSourceTypeId = txtSourceTypeId[0] == null ? "" : txtSourceTypeId[0];

                Stream fileContent = file[f].InputStream;
                int? column = null;
                int? row = null;
                var columnErr = "";

                using (ExcelPackage excelPackage = new ExcelPackage(fileContent))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var colCount = worksheet.Dimension.Columns;

                    int headerRow = 1; // Assuming the headers are in the first row

                    int columnCount = worksheet.Dimension.End.Column;
                    if (getSourceTypeId != "14")
                    {
                        for (int col = 1; col <= columnCount; col++)
                        {
                            string columnHeader = worksheet.Cells[headerRow, col].Text;
                            switch (col)
                            {
                                case 1:
                                    if (columnHeader != "หมายเลขอ้างอิง")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบหมายเลขอ้างอิง file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 2:
                                    if (columnHeader != "ประเภทบัตร")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบประเภทบัตร file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 3:
                                    if (columnHeader != "หมายเลขบัตร")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบหมายเลขบัตร file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 4:
                                    if (columnHeader != "คำนำหน้า")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบคำนำหน้า file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 5:
                                    if (columnHeader != "ชื่อ")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบหมายเลขอ้างอิง file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 6:
                                    if (columnHeader != "นามสกุล")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบชื่อ file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 7:
                                    if (columnHeader != "เบอร์โทรศัพท์")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบเบอร์โทรศัพท์ file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 8:
                                    if (columnHeader != "หน่วยงาน")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบหน่วยงาน file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 9:
                                    if (columnHeader != "ประเภทการชำระเบี้ย")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบประเภทการชำระเบี้ย file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 10:
                                    if (columnHeader != "วันคุ้มครอง")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบวันคุ้มครอง file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 11:
                                    if (columnHeader != "เดือนคุ้มครอง")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบเดือนคุ้มครอง file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 12:
                                    if (columnHeader != "ปีคุ้มครอง")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบปีคุ้มครอง file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 13:
                                    if (columnHeader != "วันสิ้นสุด")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบวันสิ้นสุด file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 14:
                                    if (columnHeader != "เดือนสิ้นสุด")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบเดือนสิ้นสุด file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 15:
                                    if (columnHeader != "ปีสิ้นสุด")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบปีสิ้นสุด file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }


                    try
                    {
                        for (int i = 2; i <= rowCount; i++)
                        {
                            row = i;//เช็ค valid ค้าว่าง
                            //โปรบุคลากรPA
                            if (getSourceTypeId == "14")
                            {
                                if (colCount == 7)
                                {
                                    for (int j = 1; j <= colCount; j++)
                                    {
                                        column = j;
                                        switch (j)
                                        {
                                            case 1:
                                                if (worksheet.Cells[i, j].Value.ToString().Trim().Length == 8)
                                                {
                                                    worksheet.Cells[i, j].Value.ToString();
                                                }
                                                else
                                                {
                                                    var columnErrName = (worksheet.Cells[1, column.Value].Value != null) ? worksheet.Cells[1, column.Value].Value.ToString() : $"Column Name = Null;Error Column :{column.Value}";
                                                    var MsgErr = $"Error Message : กรุณาตรวจสอบหมายเลขอ้างอิง file:{fileDoc} column:{columnErrName} row:{row}";

                                                    //return RedirectToAction("BadReques", "Error", new { ErrMsg = Msg });
                                                    return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                                }
                                                break;

                                            case 5:
                                                worksheet.Cells[i, j].Value.ToString();
                                                break;

                                            case 6:
                                                if (worksheet.Cells[i, j].Value != null)
                                                {
                                                    if (worksheet.Cells[i, 5].Value.ToString().Trim() == "เลขบัตรประจำตัวประชาชน")
                                                    {
                                                        if (worksheet.Cells[i, j].Value.ToString().Trim().Length == 13)
                                                        {
                                                            worksheet.Cells[i, j].Value.ToString();
                                                        }
                                                        else
                                                        {
                                                            var columnErrName = (worksheet.Cells[1, column.Value].Value != null) ? worksheet.Cells[1, column.Value].Value.ToString() : $"Column Name = Null;Error Column :{column.Value}";
                                                            var MsgErr = $"Error Message : กรุณาตรวจสอบเลขบัตรประจำตัวประชาชน file:{fileDoc} column:{columnErrName} row:{row}";

                                                            //return RedirectToAction("BadReques", "Error", new { ErrMsg = Msg });
                                                            return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                                        }
                                                    }
                                                    else if (worksheet.Cells[i, 5].Value.ToString().Trim() == "หมายเลข Passport")
                                                    {
                                                        worksheet.Cells[i, j].Value.ToString();
                                                    }
                                                }
                                                else
                                                {
                                                    if (worksheet.Cells[i, 5].Value.ToString().Trim() == "หมายเลข Passport")
                                                    {
                                                        var columnErrName = (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString());
                                                    }
                                                    else
                                                    {
                                                        var columnErrName = (worksheet.Cells[1, column.Value].Value != null) ? worksheet.Cells[1, column.Value].Value.ToString() : $"Column Name = Null;Error Column :{column.Value}";
                                                        var MsgErr = $"Error Message : กรุณาตรวจสอบ file:{fileDoc} column:{columnErrName} row:{row}";

                                                        //return RedirectToAction("BadReques", "Error", new { ErrMsg = Msg });
                                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                                    }
                                                }
                                                break;

                                            case 7:
                                                columnErr = (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString());
                                                if (worksheet.Cells[i, j].Value != null)
                                                {
                                                    if (worksheet.Cells[i, j].Value.ToString().Trim().Length == 10)
                                                    {
                                                        worksheet.Cells[i, j].Value.ToString();
                                                    }
                                                    else
                                                    {
                                                        var columnErrName = (worksheet.Cells[1, column.Value].Value != null) ? worksheet.Cells[1, column.Value].Value.ToString() : $"Column Name = Null;Error Column :{column.Value}";
                                                        var MsgErr = $"Error Message : กรุณาตรวจสอบเบอร์โทรศัพท์ file:{fileDoc} column:{columnErrName} row:{row}";

                                                        //return RedirectToAction("BadReques", "Error", new { ErrMsg = Msg });
                                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                                    }
                                                }
                                                else
                                                {
                                                    worksheet.Cells[i, j].Value.ToString();
                                                }
                                                break;

                                            case 8:
                                            //case 9:
                                            //case 10:
                                            //case 11:
                                            //case 12:
                                            case 13:
                                            case 14:
                                            case 15:
                                                columnErr = (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString());
                                                break;

                                            default:
                                                worksheet.Cells[i, j].Value.ToString();
                                                break;
                                        }
                                    }
                                    excelListPA.Add(new ImportPA
                                    {
                                        title = worksheet.Cells[i, 2].Value.ToString(),
                                        firstName = worksheet.Cells[i, 3].Value.ToString(),
                                        lastName = worksheet.Cells[i, 4].Value.ToString(),
                                        cardDetail = (worksheet.Cells[i, 5].Value.ToString().Trim() == "หมายเลข Passport" && worksheet.Cells[i, 6].Value == null ? "" : worksheet.Cells[i, 6].Value.ToString()),
                                        mobileNumber = (worksheet.Cells[i, 7].Value != null ? worksheet.Cells[i, 7].Value.ToString() : ""),
                                        cardDetailType = worksheet.Cells[i, 5].Value.ToString(),
                                        referenceCode = worksheet.Cells[i, 1].Value.ToString(),
                                        sourceTypeId = txtSourceTypeId[f]
                                    });
                                }
                                else
                                {
                                    return Json(new { IsResult = false, Msg = "รูปแบบไม่ถูกต้อง" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                if (colCount == 15)
                                {
                                    for (int j = 1; j <= colCount; j++)
                                    {
                                        column = j;
                                        switch (j)
                                        {
                                            case 1:
                                                if (getSourceTypeId == "17")
                                                {
                                                    if (worksheet.Cells[i, j].Value.ToString().Trim().Length == 5)
                                                    {
                                                        worksheet.Cells[i, j].Value.ToString();
                                                    }
                                                    else
                                                    {
                                                        var columnErrName = (worksheet.Cells[1, column.Value].Value != null) ? worksheet.Cells[1, column.Value].Value.ToString() : $"Column Name = Null;Error Column :{column.Value}";
                                                        var MsgErr = $"Error Message : กรุณาตรวจสอบหมายเลขอ้างอิง file:{fileDoc} column:{columnErrName} row:{row}";

                                                        //return RedirectToAction("BadReques", "Error", new { ErrMsg = Msg });
                                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                                    }
                                                }
                                                else
                                                {
                                                    columnErr = (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString());
                                                }
                                                break;
                                            case 3:
                                            case 5:
                                            case 7:
                                            case 8:
                                            case 9:
                                            //case 10:
                                            //case 11:
                                            //case 12:
                                            case 13:
                                            case 14:
                                            case 15:
                                                columnErr = (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString());
                                                break;

                                            default:
                                                worksheet.Cells[i, j].Value.ToString();
                                                break;
                                        }
                                    }

                                    excelList.Add(new MemberImport
                                    {
                                        title = worksheet.Cells[i, 4].Value.ToString(),
                                        firstName = worksheet.Cells[i, 5].Value.ToString(),
                                        lastName = worksheet.Cells[i, 6].Value.ToString(),
                                        cardDetail = worksheet.Cells[i, 3].Value.ToString(),
                                        mobileNumber = (worksheet.Cells[i, 7].Value != null ? worksheet.Cells[i, 7].Value.ToString() : ""),
                                        MonthOrYear = (worksheet.Cells[i, 9].Value != null ? worksheet.Cells[i, 9].Value.ToString() : ""),
                                        buildingName = (worksheet.Cells[i, 8].Value != null ? worksheet.Cells[i, 8].Value.ToString() : ""),
                                        CoverDay = (worksheet.Cells[i, 10].Value != null ? worksheet.Cells[i, 10].Value.ToString() : ""),
                                        CoverMonth = (worksheet.Cells[i, 11].Value != null ? worksheet.Cells[i, 11].Value.ToString() : ""),
                                        CoverYear = (worksheet.Cells[i, 12].Value != null ? worksheet.Cells[i, 12].Value.ToString() : ""),
                                        EndDay = (worksheet.Cells[i, 13].Value != null ? worksheet.Cells[i, 13].Value.ToString() : ""),
                                        EndMonth = (worksheet.Cells[i, 14].Value != null ? worksheet.Cells[i, 14].Value.ToString() : ""),
                                        EndYear = (worksheet.Cells[i, 15].Value != null ? worksheet.Cells[i, 15].Value.ToString() : ""),
                                        cardDetailType = worksheet.Cells[i, 2].Value.ToString(),
                                        referenceCode = worksheet.Cells[i, 1].Value.ToString(),
                                        sourceTypeId = txtSourceTypeId[f]
                                    });
                                }
                                else
                                {
                                    return Json(new { IsResult = false, Msg = "รูปแบบไม่ถูกต้อง" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        var columnErrName = (worksheet.Cells[1, column.Value].Value != null) ? worksheet.Cells[1, column.Value].Value.ToString() : $"Column Name = Null;Error Column :{column.Value}";
                        var MsgErr = $"Error Message : กรุณาตรวจสอบ file:{fileDoc} column:{columnErrName} row:{row}";

                        //return RedirectToAction("BadReques", "Error", new { ErrMsg = Msg });
                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            ViewBag.excelData = excelList;
            ViewBag.excelDataJson = JsonConvert.SerializeObject(excelList);

            var currentCalendaTyper = Thread.CurrentThread.CurrentCulture.DateTimeFormat.Calendar.GetType();

            try
            {
                var getSourceTypeId = txtSourceTypeId[0] == null ? "" : txtSourceTypeId[0];
                var tempCode = new ObjectParameter("Result", typeof(string));
                _context.usp_GenerateCode("temp", 8, tempCode);

                if (getSourceTypeId == "14")
                {
                    foreach (var item in excelListPA)
                    {
                        var cardDetail = item.cardDetail;
                        var cardDetailType = item.cardDetailType;
                        var sourceType = item.sourceTypeId;
                        var referenceCode = item.referenceCode;
                        var title = item.title;
                        var firstName = item.firstName;
                        var lastName = item.lastName;
                        var mobileNumber = item.mobileNumber;

                        var xxx = _context.usp_StagingSourceTempDetail_Insert(tempCode.Value.ToString(), Convert.ToInt32(sourceType), referenceCode, cardDetailType, cardDetail, title, firstName, lastName, mobileNumber, null, null, null, null, userDetail.User_ID, actionType).SingleOrDefault();
                    }
                }
                else
                {
                    foreach (var item in excelList)
                    {
                        var cardDetail = item.cardDetail;
                        var cardDetailType = item.cardDetailType;
                        var sourceType = item.sourceTypeId;
                        var referenceCode = item.referenceCode;
                        var MonthorYear = item.MonthOrYear;
                        var title = item.title;
                        var firstName = item.firstName;
                        var lastName = item.lastName;
                        var mobileNumber = item.mobileNumber;
                        var buildingName = item.buildingName;
                        var cDay = item.CoverDay;
                        var cMonth = item.CoverMonth;
                        var cYear = item.CoverYear;
                        var eDay = item.EndDay;
                        var eMonth = item.EndMonth;
                        var eYear = item.EndYear;

                        DateTime? startCoverDate = null;
                        //Type GregorianCalendar ค.ศ
                        if (currentCalendaTyper == typeof(GregorianCalendar))
                        {
                            var strCDate = string.Format("{0}-{1}-{2}", cDay, cMonth, cYear);
                            startCoverDate = Convert.ToDateTime(strCDate);
                        }
                        // พ.ศ.
                        else if (currentCalendaTyper == typeof(ThaiBuddhistCalendar))
                        {
                            var strCDate = string.Format("{0}-{1}-{2}", cDay, cMonth, Convert.ToInt32(cYear) + 543);
                            startCoverDate = Convert.ToDateTime(strCDate);
                        }

                        DateTime? endCoverDate = null;
                        if (eDay != "" && eMonth != "" && eYear != "")
                        {
                            //Type GregorianCalendar ค.ศ
                            if (currentCalendaTyper == typeof(GregorianCalendar))
                            {
                                var strCDate = string.Format("{0}-{1}-{2}", eDay, eMonth, eYear);
                                endCoverDate = Convert.ToDateTime(strCDate);
                            }
                            // พ.ศ.
                            else if (currentCalendaTyper == typeof(ThaiBuddhistCalendar))
                            {
                                var strEDate = string.Format("{0}-{1}-{2}", eDay, eMonth, Convert.ToInt32(eYear) + 543);
                                endCoverDate = Convert.ToDateTime(strEDate);
                            }
                        }

                        var xxx = _context.usp_StagingSourceTempDetail_Insert(
                            tempCode.Value.ToString(),
                            Convert.ToInt32(sourceType),
                            referenceCode,
                            cardDetailType,
                            cardDetail,
                            title,
                            firstName,
                            lastName,
                            mobileNumber,
                            buildingName,
                            MonthorYear,
                            startCoverDate,
                            endCoverDate,
                            userDetail.User_ID,
                            actionType).SingleOrDefault();
                    }
                }

                var sourceTypeID = txtSourceTypeId[0].ToString();
                ViewBag.sourceTypeID = sourceTypeID;

                var txttempCode = tempCode.Value.ToString();
                ViewBag.tempCode = txttempCode;

                ViewBag.SourceType = _context.usp_SourceType_Select(null, 0, 999, null, null, null).ToList();

                var rs = _context.usp_StagingSourceTempDetail_Validate_V2(tempCode.Value.ToString());
                return Json(new { txttempCode = txttempCode, IsResult = true, sourceTypeID = sourceTypeID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsResult = false, Msg = $"Error Message : {e.Message}", JsonRequestBehavior.AllowGet });
            }
        }

        [HttpPost]
        public ActionResult UploadCxl(HttpPostedFileBase[] file, string[] txtSourceTypeId, string actionType)
        {
            //Get Login Detail
            var userDetail = RoadsideAssistance.Helper.OAuth2Helper.GetLoginDetail();
            var excelList = new List<MemberImport>();
            var excelListPA = new List<ImportPA>();
            if (txtSourceTypeId == null)
            {
                return Json(new { IsResult = false, Msg = "ประเภทลูกค้าไม่ถูกต้อง" }, JsonRequestBehavior.AllowGet);
            }

            if (actionType == null)
            {
                return Json(new { IsResult = false, Msg = "ประเภทการอัพโหลดไม่ถูกต้อง" }, JsonRequestBehavior.AllowGet);
            }

            for (int f = 0; f < file.Count(); f++)
            {
                if (file[f] == null || file[f].ContentLength <= 0)
                {
                    return Json(new { IsResult = false, Msg = "กรุณาเลือกไฟล์ก่อนอัพโหลด" }, JsonRequestBehavior.AllowGet);
                }
                if (!Path.GetExtension(file[f].FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return Json(new { IsResult = false, Msg = "ประเภทของไฟล์ไม่ถูกต้อง" }, JsonRequestBehavior.AllowGet);
                }
                var fileDoc = Request.Files[f].FileName;
                var getSourceTypeId = txtSourceTypeId[0] == null ? "" : txtSourceTypeId[0];

                Stream fileContent = file[f].InputStream;
                int? column = null;
                int? row = null;
                var columnErr = "";

                using (ExcelPackage excelPackage = new ExcelPackage(fileContent))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var colCount = worksheet.Dimension.Columns;

                    int headerRow = 1; // Assuming the headers are in the first row

                    int columnCount = worksheet.Dimension.End.Column;

                    if (getSourceTypeId != "14")
                    {
                        for (int col = 1; col <= columnCount; col++)
                        {
                            string columnHeader = worksheet.Cells[headerRow, col].Text;
                            switch (col)
                            {
                                case 1:
                                    if (columnHeader != "หมายเลขอ้างอิง")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบหมายเลขอ้างอิง file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 2:
                                    if (columnHeader != "ประเภทบัตร")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบประเภทบัตร file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 3:
                                    if (columnHeader != "หมายเลขบัตร")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบหมายเลขบัตร file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 4:
                                    if (columnHeader != "คำนำหน้า")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบคำนำหน้า file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 5:
                                    if (columnHeader != "ชื่อ")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบหมายเลขอ้างอิง file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 6:
                                    if (columnHeader != "นามสกุล")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบชื่อ file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 7:
                                    if (columnHeader != "เบอร์โทรศัพท์")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบเบอร์โทรศัพท์ file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 8:
                                    if (columnHeader != "หน่วยงาน")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบหน่วยงาน file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 9:
                                    if (columnHeader != "ประเภทการชำระเบี้ย")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบประเภทการชำระเบี้ย file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 10:
                                    if (columnHeader != "วันคุ้มครอง")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบวันคุ้มครอง file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 11:
                                    if (columnHeader != "เดือนคุ้มครอง")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบเดือนคุ้มครอง file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 12:
                                    if (columnHeader != "ปีคุ้มครอง")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบปีคุ้มครอง file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 13:
                                    if (columnHeader != "วันสิ้นสุด")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบวันสิ้นสุด file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 14:
                                    if (columnHeader != "เดือนสิ้นสุด")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบเดือนสิ้นสุด file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                case 15:
                                    if (columnHeader != "ปีสิ้นสุด")
                                    {
                                        var MsgErr = $"Error Message : กรุณาตรวจสอบปีสิ้นสุด file:{fileDoc} column:{columnHeader} row:{headerRow}";
                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    try
                    {
                        for (int i = 2; i <= rowCount; i++)
                        {
                            row = i;//เช็ค valid ค่าว่าง
                            //โปรบุคลากรPA
                            if (getSourceTypeId == "14")
                            {
                                //Format ใหม่
                                if (colCount == 4)
                                {
                                    for (int j = 1; j <= colCount; j++)
                                    {
                                        column = j;
                                        switch (j)
                                        {
                                            case 1:
                                                worksheet.Cells[i, j].Value.ToString();
                                                break;

                                            default:
                                                worksheet.Cells[i, j].Value.ToString();
                                                break;
                                        }
                                    }
                                    excelListPA.Add(new ImportPA
                                    {
                                        referenceCode = worksheet.Cells[i, 1].Value.ToString(),
                                        title = worksheet.Cells[i, 2].Value.ToString(),
                                        firstName = worksheet.Cells[i, 3].Value.ToString(),
                                        lastName = worksheet.Cells[i, 4].Value.ToString(),
                                        sourceTypeId = txtSourceTypeId[f]
                                    });
                                }
                                else
                                {
                                    return Json(new { IsResult = false, Msg = "รูปแบบไม่ถูกต้อง" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                if (colCount == 15)
                                {
                                    for (int j = 1; j <= colCount; j++)
                                    {
                                        column = j;
                                        switch (j)
                                        {
                                            case 1:
                                                if (getSourceTypeId == "17")
                                                {
                                                    if (worksheet.Cells[i, j].Value.ToString().Trim().Length == 5)
                                                    {
                                                        worksheet.Cells[i, j].Value.ToString();
                                                    }
                                                    else
                                                    {
                                                        var columnErrName = (worksheet.Cells[1, column.Value].Value != null) ? worksheet.Cells[1, column.Value].Value.ToString() : $"Column Name = Null;Error Column :{column.Value}";
                                                        var MsgErr = $"Error Message : กรุณาตรวจสอบหมายเลขอ้างอิง file:{fileDoc} column:{columnErrName} row:{row}";

                                                        //return RedirectToAction("BadReques", "Error", new { ErrMsg = Msg });
                                                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                                                    }
                                                }
                                                else
                                                {
                                                    columnErr = (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString());
                                                }
                                                break;
                                            case 3:
                                            case 5:
                                            case 7:
                                            case 8:
                                            case 9:
                                            //case 10:
                                            //case 11:
                                            //case 12:
                                            case 13:
                                            case 14:
                                            case 15:
                                                columnErr = (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString());
                                                break;

                                            default:
                                                worksheet.Cells[i, j].Value.ToString();
                                                break;
                                        }
                                    }

                                    excelList.Add(new MemberImport
                                    {
                                        title = worksheet.Cells[i, 4].Value.ToString(),
                                        firstName = worksheet.Cells[i, 5].Value.ToString(),
                                        lastName = worksheet.Cells[i, 6].Value.ToString(),
                                        cardDetail = worksheet.Cells[i, 3].Value.ToString(),
                                        mobileNumber = (worksheet.Cells[i, 7].Value != null ? worksheet.Cells[i, 7].Value.ToString() : ""),
                                        MonthOrYear = (worksheet.Cells[i, 9].Value != null ? worksheet.Cells[i, 9].Value.ToString() : ""),
                                        buildingName = (worksheet.Cells[i, 8].Value != null ? worksheet.Cells[i, 8].Value.ToString() : ""),
                                        CoverDay = (worksheet.Cells[i, 10].Value != null ? worksheet.Cells[i, 10].Value.ToString() : ""),
                                        CoverMonth = (worksheet.Cells[i, 11].Value != null ? worksheet.Cells[i, 11].Value.ToString() : ""),
                                        CoverYear = (worksheet.Cells[i, 12].Value != null ? worksheet.Cells[i, 12].Value.ToString() : ""),
                                        EndDay = (worksheet.Cells[i, 13].Value != null ? worksheet.Cells[i, 13].Value.ToString() : ""),
                                        EndMonth = (worksheet.Cells[i, 14].Value != null ? worksheet.Cells[i, 14].Value.ToString() : ""),
                                        EndYear = (worksheet.Cells[i, 15].Value != null ? worksheet.Cells[i, 15].Value.ToString() : ""),
                                        cardDetailType = worksheet.Cells[i, 2].Value.ToString(),
                                        referenceCode = worksheet.Cells[i, 1].Value.ToString(),
                                        sourceTypeId = txtSourceTypeId[f]
                                    });
                                }
                                else
                                {
                                    return Json(new { IsResult = false, Msg = "รูปแบบไม่ถูกต้อง" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        var columnErrName = (worksheet.Cells[1, column.Value].Value != null) ? worksheet.Cells[1, column.Value].Value.ToString() : $"Column Name = Null;Error Column :{column.Value}";
                        var MsgErr = $"Error Message : กรุณาตรวจสอบ file:{fileDoc} column:{columnErrName} row:{row}";

                        //return RedirectToAction("BadReques", "Error", new { ErrMsg = Msg });
                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            ViewBag.excelData = excelList;
            ViewBag.excelDataJson = JsonConvert.SerializeObject(excelList);

            var currentCalendaTyper = Thread.CurrentThread.CurrentCulture.DateTimeFormat.Calendar.GetType();

            try
            {
                var getSourceTypeId = txtSourceTypeId[0] == null ? "" : txtSourceTypeId[0];
                var tempCode = new ObjectParameter("Result", typeof(string));
                _context.usp_GenerateCode("temp", 8, tempCode);
                if (getSourceTypeId == "14")
                {
                    foreach (var item in excelListPA)
                    {
                        var cardDetail = item.cardDetail;
                        var cardDetailType = item.cardDetailType;
                        var sourceType = item.sourceTypeId;
                        var referenceCode = item.referenceCode;
                        var title = item.title;
                        var firstName = item.firstName;
                        var lastName = item.lastName;
                        var mobileNumber = item.mobileNumber;

                        var xxx = _context.usp_StagingSourceTempDetailPACancel_Insert(tempCode.Value.ToString(), Convert.ToInt32(sourceType), referenceCode, cardDetailType, cardDetail, title, firstName, lastName, mobileNumber, null, null, null, null, userDetail.User_ID, actionType).SingleOrDefault();
                    }
                }
                else
                {
                    foreach (var item in excelList)
                    {
                        var cardDetail = item.cardDetail;
                        var cardDetailType = item.cardDetailType;
                        var sourceType = item.sourceTypeId;
                        var referenceCode = item.referenceCode;
                        var MonthorYear = item.MonthOrYear;
                        var title = item.title;
                        var firstName = item.firstName;
                        var lastName = item.lastName;
                        var mobileNumber = item.mobileNumber;
                        var buildingName = item.buildingName;
                        var cDay = item.CoverDay;
                        var cMonth = item.CoverMonth;
                        var cYear = item.CoverYear;
                        var eDay = item.EndDay;
                        var eMonth = item.EndMonth;
                        var eYear = item.EndYear;

                        DateTime? startCoverDate = null;
                        if (cDay != "" && cMonth != "" && cYear != "")
                        {
                            //Type GregorianCalendar ค.ศ
                            if (currentCalendaTyper == typeof(GregorianCalendar))
                            {
                                var strCDate = string.Format("{0}-{1}-{2}", cDay, cMonth, cYear);
                                startCoverDate = Convert.ToDateTime(strCDate);
                            }
                            // พ.ศ.
                            else if (currentCalendaTyper == typeof(ThaiBuddhistCalendar))
                            {
                                var strCDate = string.Format("{0}-{1}-{2}", cDay, cMonth, Convert.ToInt32(cYear) + 543);
                                startCoverDate = Convert.ToDateTime(strCDate);
                            }
                        }

                        DateTime? endCoverDate = null;
                        if (eDay != "" && eMonth != "" && eYear != "")
                        {
                            //Type GregorianCalendar ค.ศ
                            if (currentCalendaTyper == typeof(GregorianCalendar))
                            {
                                var strCDate = string.Format("{0}-{1}-{2}", eDay, eMonth, eYear);
                                endCoverDate = Convert.ToDateTime(strCDate);
                            }
                            // พ.ศ.
                            else if (currentCalendaTyper == typeof(ThaiBuddhistCalendar))
                            {
                                var strEDate = string.Format("{0}-{1}-{2}", eDay, eMonth, Convert.ToInt32(eYear) + 543);
                                endCoverDate = Convert.ToDateTime(strEDate);
                            }
                        }
                        var xxx = _context.usp_StagingSourceTempDetail_Insert(tempCode.Value.ToString(), Convert.ToInt32(sourceType), referenceCode, cardDetailType, cardDetail, title, firstName, lastName, mobileNumber, buildingName, MonthorYear, startCoverDate, endCoverDate, userDetail.User_ID, actionType).SingleOrDefault();
                    }
                }

                var sourceTypeID = txtSourceTypeId[0].ToString();
                ViewBag.sourceTypeID = sourceTypeID;

                var txttempCode = tempCode.Value.ToString();
                ViewBag.tempCode = txttempCode;

                ViewBag.SourceType = _context.usp_SourceType_Select(null, 0, 999, null, null, null).ToList();

                var rs = _context.usp_StagingSourceTempDetail_Validate_V2(tempCode.Value.ToString());
                return Json(new { txttempCode = txttempCode, IsResult = true, sourceTypeID = sourceTypeID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsResult = false, Msg = $"Error Message : {e.Message}", JsonRequestBehavior.AllowGet });
            }
        }

        //Preview
        public ActionResult Preview(string draw, string tempCode, int indexStart, int pageSize, string sortField, string orderType, string searchDetail)
        {
            if (tempCode == null)
            {
                return Json(new { IsResult = false, Msg = "ไม่พบข้อมูล", JsonRequestBehavior.AllowGet });
            }
            try
            {
                var result = _context.usp_StagingSourceTempDetail_Preview_Select(tempCode, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

                // select data ValidateDescription fix pagesize
                var result_countValid = _context.usp_StagingSourceTempDetail_Preview_Select(tempCode, 0, 99999, sortField, orderType, searchDetail).Select(x => x.ValidateDescription).ToList();

                var countValidate = result_countValid.Count(x => x.Length > 0);

                var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()},
                {"errorCountField", countValidate} //รายชื้อที่มี Err
            };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var r = new List<usp_StagingSourceTempDetail_Preview_Select_Result>();
                var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", 0},
                {"recordsFiltered", 0},
                {"data", r},
                {"errorCountField", 0} ,
                {"errMsg",e.Message }
            };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportReportValidate(string tempCode)
        {
            var result = _context.usp_StagingSourceTempDetail_Preview_Select(tempCode, 0, 99999, null, null, null).ToList();

            // data empty
            if (result.Count == 0)
            {
                return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);
            }

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Detail");
                {
                    var Roadside = _context.usp_StagingSourceTempDetail_Preview_Select(tempCode, 0, 99999, null, null, null).Select(g => new
                    {
                        หมายเลขอ้างอิง = g.ReferenceCode,
                        ประเภทบัตร = g.CardTypeDetail,
                        หมายเลขบัตร = g.CardDetail,
                        คำนำหน้า = g.Title,
                        ชื่อ = g.FirstName,
                        นามสกุล = g.LastName,
                        เบอร์โทรศัพท์ = g.MobileNumber,
                        หน่วยงาน = g.BuildingName,
                        ประเภทการชำระเบี้ย = g.PaymentType,
                        วันคุ้มครอง = g.StartCoverDateDay,
                        เดือนคุ้มครอง = g.StartCoverDateMonth,
                        ปีคุ้มครอง = g.StartCoverDateYear,
                        วันสิ้นสุด = g.EndCoverDateDay,
                        เดือนสิ้นสุด = g.EndCoverDateMonth,
                        ปีสิ้นสุด = g.EndCoverDateYear,
                        หมายเหตุ = g.ValidateDescription
                    }).ToList();
                    workSheet.Cells.LoadFromCollection(Roadside, true);
                }
                var rowCount = workSheet.Dimension.Rows;
                var colCount = workSheet.Dimension.Columns;

                // Select only the header cells
                var headerCells = workSheet.Cells[1, 1, 1, workSheet.Dimension.Columns];

                // Set their text to bold.
                headerCells.Style.Font.Bold = true;
                // Set their background color
                headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerCells.Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                // Apply the auto-filter to all the columns
                var allCells = workSheet.Cells[workSheet.Dimension.Address];
                allCells.AutoFilter = true;
                // Auto-fit all the columns
                allCells.AutoFitColumns();

                package.Save();

                stream.Position = 0;
                //get new GUID
                var dataSessionKey = Guid.NewGuid().ToString();
                //tempData GUID
                TempData["keyReport"] = dataSessionKey;
                //Session Data
                Session[dataSessionKey] = package.GetAsByteArray();
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadValidate()
        {
            var dataKey = TempData["keyReport"].ToString();

            if (Session[dataKey] != null)
            {
                byte[] data = Session[dataKey] as byte[];
                string excelName = $"Roadside Assistance Validate-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ActionResult DownloadValidateCxl()
        {
            var dataKey = TempData["keyReport"].ToString();

            if (Session[dataKey] != null)
            {
                byte[] data = Session[dataKey] as byte[];
                string excelName = $"Roadside Assistance Validate Cxl-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        //save import
        public ActionResult SaveUpload(string tmpCode, string sourceTypeId, string createdByUserId)
        {
            try
            {
                var result = _context.usp_StagingSourceTempDetail_Save(tmpCode, Convert.ToInt32(sourceTypeId), Convert.ToInt32(createdByUserId)).SingleOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsResult = false, Msg = $"Error Message : {e.Message} :{e.InnerException}" }, JsonRequestBehavior.AllowGet);
            }
        }

        //report
        public ActionResult Report()
        {
            return View();
        }

        /// <summary>
        /// Report PAImport
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportPAImport()
        {
            return View();
        }

        public ActionResult ReportPABranchImport()
        {
            return View();
        }

        public ActionResult ReportMotorPayerImport()
        {
            return View();
        }

        public ActionResult ReportMotorCancelPayerImport()
        {
            return View();
        }


        public ActionResult ExportReport(string startDate, string endDate)
        {
            var nStartDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(startDate, null));
            var nEndDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(endDate, null));

            var js = new JavaScriptSerializer();

            var result = _context.usp_ReportRoadsideAssistance_Select(nStartDate, nEndDate, null).ToList();

            // data empty
            if (result.Count == 0)
            {
                return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);
            }

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Detail");
                {
                    var Roadside = _context.usp_ReportRoadsideAssistance_Select(nStartDate, nEndDate, null).Select(o => new
                    {
                        รหัสทำรายการ = o.RoadsideAssistanceCode,
                        วันที่บันทึกเหตุ = o.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                        คำนำหน้า = o.Title,
                        ชื่อ = o.FirstName,
                        นามสกุล = o.LastName,
                        ประเภทบัตร = (o.CardDetailType != null ? o.CardDetailType : ""),
                        หมายเลขบัตร = (o.CardDetail != null ? o.CardDetail : ""),
                        เบอร์โทรศัพท์ = (o.MobileNumber != null ? o.MobileNumber : ""),
                        หน่วยงาน = (o.BuildingName != null ? o.BuildingName : ""),
                        ประเภทลูกค้า = (o.SourceTypeIdList != null ? o.SourceTypeIdList : ""),
                        วันที่คุ้มครอง = (o.StartCoverDate != null ? o.StartCoverDate.Value.ToString("dd/MM/yyyy HH:mm") : ""),
                        วันที่สิ้นสุด = (o.EndCoverDate != null ? o.EndCoverDate.Value.ToString("dd/MM/yyyy HH:mm") : ""),
                        สิทธิ์ในการใช้ = o.FreeServiceTotal,
                        วันที่เกิดเหตุ = (o.DateHappen != null ? o.DateHappen.Value.ToString("dd/MM/yyyy") : "-"),
                        เวลาเกิดเหตุ = (o.DateHappen != null ? o.DateHappen.Value.ToString("HH:mm:ss") : "-"),
                        ทะเบียนรถ = o.VehicleRegistrationDetail + o.VehicleRegistrationNumber,
                        จังหวัด = o.VehicleRegistrationProvince,
                        สถานที่เกิดเหตุ = o.LocationDetail,
                        หมายเหตุ = (o.Remark != null ? o.Remark : ""),
                        ประเภทบริการ = o.RoadsideAssistanceServiceType,
                        ประเภทการใช้สิทธิ์ = o.PayType,
                        ค่าบริการสยามสไมล์จ่าย = o.TotalAmount,
                        ค่าบริการลูกค้าจ่าย = o.CustomerAmount,
                        หักแต้มพนักงาน = o.EmployeePointAmount,
                        วันที่Assistปิดงาน = (o.ReferanceClosedDate != null ? o.ReferanceClosedDate.Value.ToString("dd/MM/yyyy") : ""),
                        เวลาAssistปิดงาน = (o.ReferanceClosedDate != null ? o.ReferanceClosedDate.Value.ToString("HH:mm") : ""),
                        ประเภทการใช้สิทธิ์1 = o.UseService,
                        สาเหตุที่ไม่ใช้สิทธิ์ = (o.ServiceCancelCauseDetail != null ? o.ServiceCancelCauseDetail : ""),
                        หมายเหตุที่ไม่ใช้สิทธิ์ = (o.OtherServiceCancelRemark != null ? o.OtherServiceCancelRemark : ""),
                        ผู้รับเรื่องRoadSide = (o.ReferanceName != null ? o.ReferanceName : ""),
                        หมายเลขอ้างอิง = (o.ReferanceCode != null ? o.ReferanceCode : ""),
                        รหัสผู้แจ้งเรื่อง = o.CreatedByCode,
                        ผู้แจ้งเรื่องCallCenter = o.CreatedByName,
                        รหัสผู้ปิดงาน = o.ClosedByCode,
                        ผู้ปิดงานCallCenter = o.ClosedByName,
                        สาเหตุที่ยกเลิกรายการ = (o.CancelRemark != null ? o.CancelRemark : ""),
                    }).ToList();
                    workSheet.Cells.LoadFromCollection(Roadside, true);
                }
                var rowCount = workSheet.Dimension.Rows;
                var colCount = workSheet.Dimension.Columns; //10

                //add sourceTypeNames
                for (int i = 0; i < result.Count; i++)
                {
                    for (int r = 1; r == 1; r++)
                    {
                        List<String> sourceTypeNames = new List<String>();

                        if (result[i].SourceTypeIdList != null)
                        {
                            var sourceType = js.Deserialize<SourceTypeObject[]>(result[i].SourceTypeIdList);
                            for (int s = 0; s < sourceType.Length; s++)
                            {
                                sourceTypeNames.Add(sourceType[s].SourceType);
                            }
                        }
                        else
                        {
                            sourceTypeNames = null;
                        }
                        if (sourceTypeNames != null)
                        {
                            StringBuilder sourceTypeD = new StringBuilder();
                            foreach (string value in sourceTypeNames)
                            {
                                sourceTypeD.Append(value);
                                sourceTypeD.Append(',');
                            }
                            workSheet.Cells[i + 2, 10].Value = sourceTypeD;
                        }
                        else
                        {
                            workSheet.Cells[i + 2, 10].Value = "";
                        }
                    }
                }

                // Select only the header cells
                var headerCells = workSheet.Cells[1, 1, 1, workSheet.Dimension.Columns];

                // Set their text to bold.
                headerCells.Style.Font.Bold = true;
                // Set their background color
                headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerCells.Style.Fill.BackgroundColor.SetColor(Color.Orange);

                // Apply the auto-filter to all the columns
                var allCells = workSheet.Cells[workSheet.Dimension.Address];
                allCells.AutoFilter = true;
                // Auto-fit all the columns
                allCells.AutoFitColumns();

                package.Save();

                stream.Position = 0;
                //get new GUID
                var dataSessionKey = Guid.NewGuid().ToString();
                //tempData GUID
                TempData["keyReport"] = dataSessionKey;
                //Session Data
                Session[dataSessionKey] = package.GetAsByteArray();
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// รายงานสรุปการนำเข้าโปรบุคลากร PA
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ActionResult ExportPAImportReport(string startDate, string endDate)
        {
            DateTime? nStartDate = GlobalFunction.ConvertDatePickerToDate_BE(startDate);
            DateTime? nEndDate = GlobalFunction.ConvertDatePickerToDate_BE(endDate);

            var js = new JavaScriptSerializer();

            var result = _context.usp_ReportImportPAHeader_Select(nStartDate, nEndDate, 0, 999999, null, null, null).ToList();

            // data empty
            if (result.Count == 0)
            {
                return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet1 = package.Workbook.Worksheets.Add("สรุป");
                {
                    var data_sheet1 = _context.usp_ReportImportPAHeader_Select(nStartDate, nEndDate, 0, 999999, null, null, null).Select(o => new
                    {
                        สาขา = o.Branch,
                        App_ID = o.AppId,
                        หน่วยงาน = o.BuildingName,
                        วันเริ่มคุ้มครอง = (o.StartCoverDate != null ? o.StartCoverDate.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) : ""),
                        วันสิ้นสุดความคุ้มครอง = (o.EndCoverDate != null ? o.EndCoverDate.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) : ""),
                        จำนวนการนำเข้า = o.CountImport
                    }
                    ).ToList();

                    workSheet1.Cells.LoadFromCollection(data_sheet1, true);

                    // Select only the header cells
                    var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];

                    // Set their text to bold.
                    headerCells1.Style.Font.Bold = true;

                    // Set their background color
                    headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells1.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

                    // Apply the auto-filter to all the columns
                    var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];

                    allCells1.AutoFilter = true;

                    // Auto-fit all the columns
                    allCells1.AutoFitColumns();
                }

                var workSheet2 = package.Workbook.Worksheets.Add("รายชื่อ");
                {
                    var data_sheet2 = _context.usp_ReportImportPADetail_Select(nStartDate, nEndDate, null, 0, 999999, null, null, null).Select(o => new
                    {
                        App_ID = o.AppId,
                        หมายเลขอ้างอิง = o.ReferenceCode,
                        คำนำหน้า = o.Title,
                        ชื่อ = o.FirstName,
                        นามสกุล = o.LastName,
                        หน่วยงาน = o.BuildingName,
                        วันเริ่มคุ้มครอง = (o.StartCoverDate != null ? o.StartCoverDate.Value.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) : ""),
                        วันสิ้นสุดความคุ้มครอง = (o.EndCoverDate != null ? o.EndCoverDate.Value.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) : ""),
                        ผู้ทำรายการ = o.EmpName,
                        สาขาทึ่ทำรายการ = o.Branch,
                        วันที่ทำรายการ = (o.StagingDate != null ? o.StagingDate.Value.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("th-TH")) : "")
                    }).ToList();

                    workSheet2.Cells.LoadFromCollection(data_sheet2, true);
                    // Select only the header cells

                    var headerCells2 = workSheet2.Cells[1, 1, 1, 32];
                    // Set their text to bold.
                    headerCells2.Style.Font.Bold = true;
                    // Set their background color
                    headerCells2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells2.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    // Apply the auto-filter to all the columns
                    var allCells2 = workSheet2.Cells[workSheet2.Dimension.Address];
                    allCells2.AutoFilter = true;
                    // Auto-fit all the columns
                    allCells2.AutoFitColumns();
                }

                package.Save();
                stream.Position = 0;
                //get new GUID
                var dataSessionKey = Guid.NewGuid().ToString();
                //tempData GUID
                TempData["keyReport"] = dataSessionKey;
                //Session Data
                Session[dataSessionKey] = package.GetAsByteArray();
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///รายงานรายชื่อโปรบุคลากร PA(ตามสาขา)
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ActionResult ExportPABranchImportReport(string startDate, string endDate)
        {
            DateTime? nStartDate = GlobalFunction.ConvertDatePickerToDate_BE(startDate);
            DateTime? nEndDate = GlobalFunction.ConvertDatePickerToDate_BE(endDate);
            var userDetail = RoadsideAssistance.Helper.OAuth2Helper.GetLoginDetail();
            int? userId = userDetail.User_ID;

            var js = new JavaScriptSerializer();

            var result = _context.usp_ReportImportPADetail_Select(nStartDate, nEndDate, userId, 0, 999999, null, null, null).ToList();

            // data empty
            if (result.Count == 0)
            {
                return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet1 = package.Workbook.Worksheets.Add("รายชื่อ");
                {
                    var data_sheet1 = _context.usp_ReportImportPADetail_Select(nStartDate, nEndDate, userId, 0, 999999, null, null, null).Select(o => new
                    {
                        หมายเลขอ้างอิง = o.ReferenceCode,
                        คำนำหน้า = o.Title,
                        ชื่อ = o.FirstName,
                        นามสกุล = o.LastName,
                        หน่วยงาน = o.BuildingName,
                        วันเริ่มคุ้มครอง = (o.StartCoverDate != null ? o.StartCoverDate.Value.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) : ""),
                        วันสิ้นสุดความคุ้มครอง = (o.EndCoverDate != null ? o.EndCoverDate.Value.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) : ""),
                        ผู้ทำรายการ = o.EmpName,
                        สาขาทึ่ทำรายการ = o.Branch,
                        วันที่ทำรายการ = (o.StagingDate != null ? o.StagingDate.Value.ToString("dd/MM/yyyy HH:MM:ss", new CultureInfo("th-TH")) : "")
                    }).ToList();

                    workSheet1.Cells.LoadFromCollection(data_sheet1, true);

                    // Select only the header cells
                    var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];

                    // Set their text to bold.
                    headerCells1.Style.Font.Bold = true;

                    // Set their background color
                    headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells1.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

                    // Apply the auto-filter to all the columns
                    var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];

                    allCells1.AutoFilter = true;

                    // Auto-fit all the columns
                    allCells1.AutoFitColumns();
                }

                package.Save();
                stream.Position = 0;
                //get new GUID
                var dataSessionKey = Guid.NewGuid().ToString();
                //tempData GUID
                TempData["keyReport"] = dataSessionKey;
                //Session Data
                Session[dataSessionKey] = package.GetAsByteArray();
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportMotorPayerImport(string startDate, string endDate)
        {
            DateTime? nStartDate = GlobalFunction.ConvertDatePickerToDate_BE(startDate);
            DateTime? nEndDate = GlobalFunction.ConvertDatePickerToDate_BE(endDate);
            var userDetail = RoadsideAssistance.Helper.OAuth2Helper.GetLoginDetail();
            int? userId = userDetail.User_ID;

            var js = new JavaScriptSerializer();

            var sourceTypeId = 7; //motor
            var actionType = "A";

            var result = _context.usp_ReportStagingSourceLog_Select(nStartDate, nEndDate, sourceTypeId, actionType).ToList();

            // data empty
            if (result.Count == 0)
            {
                return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet1 = package.Workbook.Worksheets.Add("รายชื่อที่นำเข้าสิทธ์");
                {
                    var data_sheet1 = _context.usp_ReportStagingSourceLog_Select(nStartDate, nEndDate, sourceTypeId, actionType).Select(o => new
                    {
                        หมายเลขอ้างอิง = o.ReferenceCode,
                        คำนำหน้า = o.Title,
                        ชื่อ = o.FirstName,
                        นามสกุล = o.LastName,
                        หน่วยงาน = o.BuildingName,
                        วันเริ่มคุ้มครอง = (o.StartCoverDate != null ? o.StartCoverDate.Value.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) : ""),
                        วันสิ้นสุดความคุ้มครอง = (o.EndCoverDate != null ? o.EndCoverDate.Value.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) : ""),
                        ผู้ทำรายการ = o.PersonName,
                        วันที่ทำรายการ = (o.CreatedDate != null ? o.CreatedDate.Value.ToString("dd/MM/yyyy HH:MM:ss", new CultureInfo("th-TH")) : "")
                    }).ToList();

                    workSheet1.Cells.LoadFromCollection(data_sheet1, true);

                    // Select only the header cells
                    var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];

                    // Set their text to bold.
                    headerCells1.Style.Font.Bold = true;

                    // Set their background color
                    headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells1.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

                    // Apply the auto-filter to all the columns
                    var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];

                    allCells1.AutoFilter = true;

                    // Auto-fit all the columns
                    allCells1.AutoFitColumns();
                }

                package.Save();
                stream.Position = 0;
                //get new GUID
                var dataSessionKey = Guid.NewGuid().ToString();
                //tempData GUID
                TempData["keyReport"] = dataSessionKey;
                //Session Data
                Session[dataSessionKey] = package.GetAsByteArray();
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportMotorCancelPayerImport(string startDate, string endDate)
        {
            DateTime? nStartDate = GlobalFunction.ConvertDatePickerToDate_BE(startDate);
            DateTime? nEndDate = GlobalFunction.ConvertDatePickerToDate_BE(endDate);
            var userDetail = RoadsideAssistance.Helper.OAuth2Helper.GetLoginDetail();
            int? userId = userDetail.User_ID;

            var js = new JavaScriptSerializer();

            var sourceTypeId = 7; //motor
            var actionType = "D";

            var result = _context.usp_ReportStagingSourceLog_Select(nStartDate, nEndDate, sourceTypeId, actionType).ToList();

            // data empty
            if (result.Count == 0)
            {
                return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลที่ค้นหา" }, JsonRequestBehavior.AllowGet);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet1 = package.Workbook.Worksheets.Add("รายชื่อยกเลิกสิทธ์");
                {
                    var data_sheet1 = _context.usp_ReportStagingSourceLog_Select(nStartDate, nEndDate, sourceTypeId, actionType).Select(o => new
                    {
                        หมายเลขอ้างอิง = o.ReferenceCode,
                        คำนำหน้า = o.Title,
                        ชื่อ = o.FirstName,
                        นามสกุล = o.LastName,
                        หน่วยงาน = o.BuildingName,
                        วันเริ่มคุ้มครอง = (o.StartCoverDate != null ? o.StartCoverDate.Value.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) : ""),
                        วันสิ้นสุดความคุ้มครอง = (o.EndCoverDate != null ? o.EndCoverDate.Value.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) : ""),
                        ผู้ทำรายการ = o.PersonName,
                        วันที่ทำรายการ = (o.CreatedDate != null ? o.CreatedDate.Value.ToString("dd/MM/yyyy HH:MM:ss", new CultureInfo("th-TH")) : "")
                    }).ToList();

                    workSheet1.Cells.LoadFromCollection(data_sheet1, true);

                    // Select only the header cells
                    var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];

                    // Set their text to bold.
                    headerCells1.Style.Font.Bold = true;

                    // Set their background color
                    headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells1.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

                    // Apply the auto-filter to all the columns
                    var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];

                    allCells1.AutoFilter = true;

                    // Auto-fit all the columns
                    allCells1.AutoFitColumns();
                }

                package.Save();
                stream.Position = 0;
                //get new GUID
                var dataSessionKey = Guid.NewGuid().ToString();
                //tempData GUID
                TempData["keyReport"] = dataSessionKey;
                //Session Data
                Session[dataSessionKey] = package.GetAsByteArray();
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Download()
        {
            var dataKey = TempData["keyReport"].ToString();

            if (Session[dataKey] != null)
            {
                byte[] data = Session[dataKey] as byte[];
                string excelName = $"Roadside Assistance Report-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        #endregion funtion
    }
}