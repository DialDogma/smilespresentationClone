using Amazon.Runtime.Internal.Transform;
using Amazon.S3;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Elmah;
using MailKit.Net.Smtp;
using MailKit.Security;
using MassTransit;
using MassTransit.Futures.Contracts;
using MassTransit.Internals.GraphValidation;
using MimeKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.Style;
using PayTransferAPI.Contract;
using Renci.SshNet;
using RestSharp;
using RestSharp.Authenticators;
using Rotativa;
using Serilog;
using SmileSClaimPayBack.Helper;
using SmileSClaimPayBack.Models;
using SmileSClaimPayBack.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Drawing;
using System.EnterpriseServices;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Windows.Interop;

namespace SmileSClaimPayBack.Controllers
{
    [Authorization]
    public class ClaimPayBackController : Controller
    {
        private readonly ClaimPayBackEntities _context;
        private const string _controllerName = nameof(ClaimPayBackController);
        private const string _invalidClaimInClaimHeaderGroup = "ไม่มีรายการ Claim อยู่ใน บส นี้";
        public ClaimPayBackController()

        {
            _context = new ClaimPayBackEntities();
        }

        public ActionResult ClaimPayBackMonitor()
        {
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();

            var roleMO = new[] { "SmileClaimPayBack_Operation" }; //Operator

            var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;

            if (rolelist.Intersect(roleMO).Count() > 0)
            {
                ViewBag.Branch = _context.usp_Branch_Select(branchId, null, 0, 9999, null, null, null).ToList();
                ViewBag.checkBranch = false;
            }
            else
            {
                ViewBag.Branch = _context.usp_Branch_Select(null, null, 0, 9999, null, null, null).ToList();
                ViewBag.checkBranch = true;
            }

            ViewBag.Status = _context.usp_ClaimPayBackStatus_Select(null, 0, 9999, null, null, null).Where(x => x.ClaimPayBackStatusId != 1).ToList();
            ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            return View();
        }

        public ActionResult ClaimPayBackCreate()
        {
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();

            var roleMO = new[] { "SmileClaimPayBack_Operation" }; //Operator

            var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;

            if (rolelist.Intersect(roleMO).Count() > 0)
            {
                ViewBag.Branch = _context.usp_Branch_Select(branchId, null, 0, 9999, null, null, null).ToList();
                ViewBag.checkBranch = false;
            }
            else
            {
                ViewBag.Branch = _context.usp_Branch_Select(null, null, 0, 9999, null, null, null).ToList();
                ViewBag.checkBranch = true;
            }

            ViewBag.ProductGroup = _context.usp_ProductGroup_Select(null, 0, 9999, null, null, null).Where(x => x.ProductGroup_ID != 1).ToList();
            ViewBag.ClaimGroupType = _context.usp_ClaimGroupType_Select(null, 0, 9999, null, null, null).Where(x => x.ClaimGroupTypeId != 1).ToList();
            ViewBag.Insurance = _context.usp_OrganizeInsuranceForGenerateGroup_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.EmployeeByBranch = _context.usp_GetEmployeesByBranch_Select(null, branchId, 0, 9999, null, null, null).ToList();
            ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            return View();
        }
        public JsonResult GetUserByBranchId(int? branchId = null)
        {
            var rs = _context.usp_GetEmployeesByBranch_Select(null, branchId == -1 ? null : branchId, 0, 9999, null, null, null).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ClaimPayBackDetail(String claimPayBackId)
        {
            byte[] idByte = Convert.FromBase64String(claimPayBackId);
            string id = System.Text.Encoding.UTF8.GetString(idByte);

            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();
            var roleMO = new[] { "SmileClaimPayBack_Operation" };

            if (rolelist.Intersect(roleMO).Count() > 0)
                ViewBag.RoleOperation = true;
            else
                ViewBag.RoleOperation = false;

            var detail = _context.usp_ClaimPayBackDetailByHeader_Select(Int32.Parse(id), 0, 9999, null, null, null).FirstOrDefault();

            if (detail != null)
            {
                ViewBag.Code = detail.ClaimPayBackCode;
                ViewBag.StatusId = detail.ClaimPayBackStatusId;
                ViewBag.ClaimGroupTypeId = detail.ClaimGroupTypeId;
            }
            else
            {
                ViewBag.Code = null;
            }

            ViewBag.Id = id;

            return View();
        }

        public ActionResult ClaimPayBackTransferPayment()
        {
            ViewBag.ClaimGroupType = _context.usp_ClaimGroupType_Select(null, 0, 99999, null, null, null).Where(x => x.ClaimGroupTypeId != 1).ToList();
            return View();
        }

        //[Authorization(Roles = "SmileClaimPayBack_Payer,SmileClaimPayBack_FundManage,Developer")]
        public ActionResult ClaimPayBackTransferPaymentMonitor()
        {
            //// check role
            //var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            //var role = OAuth2Helper.GetRoles();
            //var rolelist = role.Split(',').ToList();
            //var rolePermisstion = new List<string> { "Developer" };
            //if (rolelist.Intersect(rolePermisstion).Count() == 0)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            ViewBag.Status = _context.usp_ClaimPayBackTransferStatus_Select(null, 0, 9999, null, null, null).Where(x => x.ClaimPayBackTransferStatusId != 1 && x.ClaimPayBackTransferStatusId != 5).ToList();
            ViewBag.ClaimGroupType = _context.usp_ClaimGroupType_Select(null, 0, 99999, null, null, null).Where(x => x.ClaimGroupTypeId != 1).ToList();
            return View();
        }

        public ActionResult ClaimPayBackDetailXClaim(string claimPayBackDetailId)
        {
            byte[] idByte = Convert.FromBase64String(claimPayBackDetailId);
            string id = System.Text.Encoding.UTF8.GetString(idByte);

            ViewBag.ClaimPayBackDetailId = id;
            return View();
        }

        public ActionResult ClaimPayBackPDF(String claimPayBackId)
        {
            try
            {
                //// Debug
                //return View(LoadClaimPayBackMonitorPDF(claimPayBackId));

                // Use
                var data = LoadClaimPayBackMonitorPDF(claimPayBackId);

                string footerOption = "--footer-center \"Page: [page] of [toPage]\" --footer-font-size 8 --footer-spacing 5 ";
                var pdf = new ViewAsPdf
                {
                    Model = data,
                    ViewName = "ClaimPayBackPDF",
                    FileName = "ClaimPayBack_" + DateTime.Now.ToString("dd/MM/yyyy") + ".pdf",
                    CustomSwitches = footerOption,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageSize = Rotativa.Options.Size.A4,
                    MinimumFontSize = 10,
                };
                return pdf;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ClaimPayBackPDF:" + ex.Message);
                ViewBag.ErrorMessage = ex.Message;
                return View("ErrorMessage");
            }
        }

        public ActionResult ClaimPayBackBillingRequestGroupExport()
        {
            try
            {
                ViewBag.InsuranceCompany = _context.usp_OrganizeInsuranceForGenerateGroup_Select(null, 0, 9999, null, null, null).ToList();
                ViewBag.BillingRequestGroupStatus = _context.usp_BillingRequestGroupStatus_Select(null, null, null, null, null, null).ToList();

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ClaimPayBackExportBOHO:" + ex.Message);
                ViewBag.ErrorMessage = ex.Message;
                return View("ErrorMessage");
            }
        }

        public ActionResult ClaimPayBackBillingRequestGroupImport()
        {
            try
            {
                ViewBag.ClaimHeaderGroupStatusId = _context.usp_ClaimHeaderGroupImportStatus_Select(null, 0, 9999, null, null, null).ToList();
                ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ClaimPayBackImportBOHO:" + ex.Message);
                ViewBag.ErrorMessage = ex.Message;
                return View("ErrorMessage");
            }
        }

        public ActionResult ClaimPayBackBillingRequestGroupImportDetail()
        {
            try
            {
                ViewBag.ClaimHeaderGroupTypeId = _context.usp_ClaimHeaderGroupType_Select(null, 0, 9999, null, null, null).ToList();
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ClaimPayBackImportBOHODetail:" + ex.Message);
                ViewBag.ErrorMessage = ex.Message;
                return View("ErrorMessage");
            }
        }

        public ActionResult ClaimPayBackSaveBalance()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ClaimPayBackSaveBalance:" + ex.Message);
                ViewBag.ErrorMessage = ex.Message;
                return View("ErrorMessage");
            }
        }

        public ActionResult ClaimPayBackImportSaveBalance()
        {
            try
            {
                ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ClaimPayBackImportSaveBalance:" + ex.Message);
                ViewBag.ErrorMessage = ex.Message;
                return View("ErrorMessage");
            }
        }

        public ActionResult MedicalPaymentPDF(int? claimPayBackSubGroupId)
        {
            try
            {
                Log.Debug("{_controllerName} Start MedicalPaymentPDF [claimPayBackSubGroupId = {claimPayBackSubGroupId}]", _controllerName, claimPayBackSubGroupId);

                System.Globalization.CultureInfo _cultureTHInfo = new System.Globalization.CultureInfo("th-TH");

                // Set the Thai Buddhist calendar
                _cultureTHInfo.DateTimeFormat.Calendar = new ThaiBuddhistCalendar();

                // Set the culture for the current thread
                CultureInfo.CurrentCulture = _cultureTHInfo;

                Log.Debug($"{_controllerName} - MedicalPaymentPDF Call Store procedure: usp_MedicalBillsReport_Select [claimPayBackSubGroupId = {claimPayBackSubGroupId}]");
                var list = _context.usp_MedicalBillsReport_Select(claimPayBackSubGroupId).ToList();

                var createdDate = list.FirstOrDefault().CreatedDate;
                var transferDate = list.FirstOrDefault().TransferDate;
                var hospitalName = list.FirstOrDefault().HospitalName;
                //function change date to datethai
                DateTime createdDateThai = Convert.ToDateTime(createdDate, _cultureTHInfo);
                string createdDateThaiText = createdDateThai.ToString("d MMMM yyyy", _cultureTHInfo);
                ViewBag.CreatedDate = createdDateThaiText;
                DateTime transferDateThai = Convert.ToDateTime(transferDate, _cultureTHInfo);
                string transferDateThaiText = transferDateThai.ToString("d MMMM yyyy", _cultureTHInfo);
                ViewBag.TransferDate = transferDateThaiText;

                var sumAmount = list.Sum(_ => _.Amount);
                string sumAmountString = sumAmount?.ToString("#,###.00");
                ViewBag.SumAmount = sumAmountString;

                //using function ThaiBaht from GlobalFunction
                ViewBag.SumAmountTaxt = GlobalFunction.ThaiBahtText(sumAmount.ToString());

                //header && footer pdf 
                var tpaHeaderUrl = Properties.Settings.Default.LocalUrl + "/Report/TPAReportHeader";
                var tpaFooterUrl = Properties.Settings.Default.LocalUrl + "/Report/TPAReportFooter";
                var customSwitches =
                    string.Format("--print-media-type --allow {0} --header-html {0} --allow {1} --footer-html {1}", tpaHeaderUrl, tpaFooterUrl);

                //using WebConfig Call Medical Data
                ViewBag.ClaimPayBackMedicalManagerName = Properties.Settings.Default.ClaimPayBackMedicalManagerName;
                ViewBag.ClaimPayBackMedicalPosition = Properties.Settings.Default.ClaimPayBackMedicalPosition;

                //pdf function
                var pdf = new ViewAsPdf
                {
                    Model = list,
                    ViewName = "MedicalPaymentPDF",
                    FileName = hospitalName + "-" + sumAmountString + ".pdf",
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageSize = Rotativa.Options.Size.A4,
                    MinimumFontSize = 10,
                    PageMargins = new Rotativa.Options.Margins(35, 0, 29, 0),
                    CustomSwitches = customSwitches,
                };

                return pdf;
            }
            catch (Exception ex)
            {
                Console.WriteLine("MedicalPaymentPDF:" + ex.Message);
                ViewBag.ErrorMessage = ex.Message;
                return View("ErrorMessage");
            }
        }

        #region ตัดรับชำระสินไหม Online 

        [HttpGet]
        public ActionResult NonPerformingloanMonitor()
        {
            // get PaymentType .ClaimPaybackPaymentType.Where(w => w.IsActive == true && w.ClaimPaybackPaymentTypeId != 1).ToList();
            ViewBag.DifferentPaymentTypeId = _context.vw_ClaimOnlineReceiveType.Where(w => w.IsActive == true && w.ReceiveTypeId != 1 && w.ReceiveTypeId != 5).ToList();
            return View("NonPerformingloanMonitor");
        }

        [HttpPost]
        public ActionResult NonPerformingloanImportExcelFile(HttpPostedFileBase ImportFile, int DifferentPaymentTypeId)
        {
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;
            decimal? Total = null;
            var createHeader = new ClaimPaybackPaymentHeader();
            var detailModels = new List<ClaimPaybackPaymentDetail>();
            try
            {
                Log.Debug("{_controllerName} Start NonPerformingloanImportExcelFile ", _controllerName);
                if (ImportFile.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    Log.Debug("{_controllerName} File import ContentType invalid", _controllerName);
                    return Json(new { valid = false, message = "กรุณาเลือกเอกสารนามสกุล .xlsx " });
                }

                using (var package = new ExcelPackage(ImportFile.InputStream))
                {
                    var cs = package.Workbook.Worksheets;
                    var ws = cs.First();
                    var lastRecord = ws.Dimension.End.Row;

                    try
                    {
                        var claimCodeValidate = new List<string>();
                        var differenceValidate = new List<decimal>();
                        var NPLDetailsValidate = new List<Guid?>();
                        for (var record = 2; record <= lastRecord; record++)
                        {
                            if (DifferentPaymentTypeId != 4)
                            {
                                // บันทึกโอนเพิ่ม /บันทึกคืนเงิน
                                Guid claimOnlineItemId = new Guid(ws.Cells[record, 1].Value.ToString());
                                string CLNumber = ws.Cells[record, 2].Value.ToString();
                                decimal differenceAmount = Convert.ToDecimal(ws.Cells[record, 9].Value.ToString());

                                claimCodeValidate.Add(CLNumber);
                                differenceValidate.Add(differenceAmount);
                                NPLDetailsValidate.Add(claimOnlineItemId);
                                detailModels.Add(new ClaimPaybackPaymentDetail
                                {
                                    ClaimOnlineItemId = claimOnlineItemId,
                                    ClaimCode = CLNumber,
                                    DifferentAmount = differenceAmount,
                                    IsActive = true,
                                    CreatedByUserId = userId,
                                    CreatedDate = DateTime.Now,
                                    UpdatedByUserId = userId,
                                    UpdatedDate = DateTime.Now,
                                });
                            }
                            else
                            {
                                // NPL
                                Guid NPLDetailId = new Guid(ws.Cells[record, 1].Value.ToString());
                                string CLNumber = ws.Cells[record, 6].Value.ToString();
                                decimal differenceAmount = (ws.Cells[record, 16].Value == null ? 0 : Convert.ToDecimal(ws.Cells[record, 16].Value.ToString()));

                                claimCodeValidate.Add(CLNumber);
                                differenceValidate.Add(differenceAmount);
                                NPLDetailsValidate.Add(NPLDetailId);
                                detailModels.Add(new ClaimPaybackPaymentDetail
                                {
                                    ClaimOnlineItemId = _context.vw_ClaimOnline_npl.FirstOrDefault(f => f.NPLDetailId == NPLDetailId).ClaimOnLineItemId,
                                    ClaimCode = CLNumber,
                                    DifferentAmount = differenceAmount,
                                    IsActive = true,
                                    CreatedByUserId = userId,
                                    CreatedDate = DateTime.Now,
                                    UpdatedByUserId = userId,
                                    UpdatedDate = DateTime.Now,
                                    NPLDetailId = NPLDetailId,
                                });
                            }
                        }

                        // บันทึกการโอน
                        if (DifferentPaymentTypeId == 2)
                        {
                            if (cs.FirstOrDefault().Dimension.Columns != 9)
                            {
                                return Json(new { valid = false, message = "กรุณาตรวจสอบเอกสารนำเข้าข้อมูล \n\r ชื่อเอกสารที่นำเข้า (" + ImportFile.FileName + ")" });
                            }

                            // validate  
                            var checkValidate = _context.vw_ClaimOnlineItem.Where(w => NPLDetailsValidate.Contains(w.ClaimOnLineItemId)).ToList();
                            if (checkValidate.Count() != NPLDetailsValidate.Count())
                            {
                                return Json(new { valid = false, message = "ข้อมูลนำเข้าไม่ถูกต้อง" });
                            }

                            if (!ValidateClaimPayBackXClaim(claimCodeValidate))
                            {
                                Log.Debug("{_controllerName} check Claim Code invalid", _controllerName);
                                return Json(new { valid = false, message = "กรุณาตรวจสอบ Claim Code" });
                            }

                            bool validateAmount = differenceValidate.Any(a => a <= 0);
                            if (validateAmount)
                            {
                                Log.Debug("{_controllerName} Difference amount invalid. DifferentPaymentTypeId is {DifferentPaymentTypeId}", _controllerName, DifferentPaymentTypeId);
                                return Json(new { valid = false, message = "กรุณาตรวจสอบผลต่างการชำระ" });
                            }
                        }

                        // บันทึกคืนเงิน
                        if (DifferentPaymentTypeId == 3)
                        {
                            if (!ValidateClaimPayBackXClaim(claimCodeValidate))
                            {
                                Log.Debug("{_controllerName} check Claim Code invalid", _controllerName);
                                return Json(new { valid = false, message = "กรุณาตรวจสอบ Claim Code" });
                            }

                            // validate  
                            var checkValidate = _context.vw_ClaimOnlineItem.Where(w => NPLDetailsValidate.Contains(w.ClaimOnLineItemId)).ToList();
                            if (checkValidate.Count() != NPLDetailsValidate.Count())
                            {
                                return Json(new { valid = false, message = "ข้อมูลนำเข้าไม่ถูกต้อง" });
                            }

                            bool validateAmount = differenceValidate.Any(a => a > 0);
                            if (validateAmount)
                            {
                                return Json(new { valid = false, message = "กรุณาตรวจสอบผลต่างการชำระ" });
                            }
                        }

                        // บันทึกรับเงิน NPL
                        if (DifferentPaymentTypeId == 4)
                        {
                            // validate  
                            var checkValidate = _context.vw_ClaimOnline_npl.Where(w => NPLDetailsValidate.Contains(w.NPLDetailId)).ToList();
                            if (checkValidate.Count() != NPLDetailsValidate.Count())
                            {
                                return Json(new { valid = false, message = "ข้อมูลนำเข้าไม่ถูกต้อง" });
                            }

                            bool validateAmount = differenceValidate.Any(a => a <= 0);
                            if (validateAmount)
                            {
                                return Json(new { valid = false, message = "กรุณาตรวจสอบยอดเงิน NPL" });
                            }

                            if (cs.FirstOrDefault().Dimension.Columns != 22)
                            {
                                return Json(new { valid = false, message = "กรุณาตรวจสอบเอกสารนำเข้าข้อมูล \n\r ชื่อเอกสารที่นำเข้า (" + ImportFile.FileName + ")" });
                            }
                        }


                        // create header
                        createHeader.CreatedByUserId = userId;
                        createHeader.CreatedDate = DateTime.Now;
                        createHeader.UpdatedByUserId = userId;
                        createHeader.UpdatedDate = DateTime.Now;
                        createHeader.IsActive = true;
                        createHeader.ClaimPaybackPaymentTypeId = DifferentPaymentTypeId;
                        createHeader.ClaimPaybackPaymentHeaderId = new Guid(Guid.NewGuid().ToString());

                        // sum total
                        Total = detailModels.Sum(s => s.DifferentAmount);
                    }
                    catch (Exception ex)
                    {
                        return Json(new { valid = false, message = "ข้อมูลนำเข้าไม่ถูกต้อง" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { valid = false, message = ex.InnerException != null ? ex.InnerException.Message : ex.Message });
            }

            return Json(new { valid = true, differentAmount = Total, header = JsonConvert.SerializeObject(createHeader), details = JsonConvert.SerializeObject(detailModels) });
        }

        [HttpPost]
        public async Task<ActionResult> NonPerformingloanConfirm(decimal amount, DateTime TransferDate, string jsonHeader, string jsonDetail)
        {
            Log.Debug("{_controllerName} NonPerformingloanConfirm is Start.", _controllerName);
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;
            try
            {

                decimal _amount = 0;
                var paymentHeader = JsonConvert.DeserializeObject<ClaimPaybackPaymentHeader>(jsonHeader);
                if (paymentHeader.ClaimPaybackPaymentTypeId == 3 && amount > 0)
                {
                    _amount = (amount * -1);
                }
                else
                {
                    _amount = amount;
                }

                // insert header
                var createHeader = new ClaimPaybackPaymentHeader
                {
                    CreatedByUserId = userId,
                    CreatedDate = DateTime.Now,
                    UpdatedByUserId = userId,
                    UpdatedDate = DateTime.Now,
                    IsActive = true,
                    ClaimPaybackPaymentTypeId = paymentHeader.ClaimPaybackPaymentTypeId,
                    ClaimPaybackPaymentHeaderId = new Guid(Guid.NewGuid().ToString()),
                    ActiveDate = TransferDate,
                    TotalAmount = _amount
                };
                _context.ClaimPaybackPaymentHeader.Add(createHeader);
                await _context.SaveChangesAsync();

                Log.Debug("{_controllerName} NonPerformingloanConfirm create DifferentPaymentHeader {DifferentPaymentHeaderId} is done.", _controllerName, createHeader.ClaimPaybackPaymentHeaderId);

                // insert details
                var receiveItems = new List<ClaimOnLine_Api.Contract.ClaimReceiveItemList>();
                var paymentDetails = JsonConvert.DeserializeObject<List<ClaimPaybackPaymentDetail>>(jsonDetail);
                foreach (var Get in paymentDetails)
                {
                    var _ClaimOnlineId = _context.vw_ClaimOnlineItem.Where(w => w.ClaimOnLineItemId == Get.ClaimOnlineItemId).Select(s => s.ClaimOnLineItemId).FirstOrDefault();
                    var _NPLDetailId = _context.vw_ClaimOnline_npl.Where(w => w.ClaimOnLineItemId == Get.NPLDetailId).Select(s => s.NPLDetailId).FirstOrDefault();

                    var InsertDetail = new ClaimPaybackPaymentDetail
                    {
                        ClaimPaybackPaymentDetailId = new Guid(Guid.NewGuid().ToString()),
                        ClaimPaybackPaymentHeaderId = createHeader.ClaimPaybackPaymentHeaderId,
                        ClaimOnlineItemId = Get.ClaimOnlineItemId,
                        ClaimCode = Get.ClaimCode,
                        DifferentAmount = Get.DifferentAmount,
                        ClaimOnlineId = _ClaimOnlineId,
                        NPLDetailId = Get.NPLDetailId,
                        IsActive = true,
                        CreatedByUserId = userId,
                        CreatedDate = DateTime.Now,
                        UpdatedByUserId = userId,
                        UpdatedDate = DateTime.Now,

                    };
                    _context.ClaimPaybackPaymentDetail.Add(InsertDetail);

                    // set detail data for publish 
                    receiveItems.Add(new ClaimOnLine_Api.Contract.ClaimReceiveItemList
                    {
                        Amount = (decimal)InsertDetail.DifferentAmount,
                        ClaimCode = InsertDetail.ClaimCode,
                        ClaimPaybackPaymentDetailId = InsertDetail.ClaimPaybackPaymentDetailId,
                        NPLDetailId = (InsertDetail.NPLDetailId == null ? new Guid("00000000-0000-0000-0000-000000000000") : InsertDetail.NPLDetailId.Value),
                        ClaimGroupCode = "",
                        ClaimOnlineItemId = (InsertDetail.ClaimOnlineItemId == null ? new Guid("00000000-0000-0000-0000-000000000000") : InsertDetail.ClaimOnlineItemId.Value),
                        ClaimPayBackCode = "",
                        ClaimPayBackXClaimId = 1
                    });
                }

                await _context.SaveChangesAsync();

                /*
                 * 2 === 4
                 * 3 === 7
                 * 4 === 8
                 */

                int _transferTypeId = 0;
                switch (createHeader.ClaimPaybackPaymentTypeId)
                {
                    case 2:
                        _transferTypeId = 4;
                        break;
                    case 3:
                        _transferTypeId = 7;
                        break;
                    case 4:
                        _transferTypeId = 8;
                        break;
                }

                // set header data for publish 
                var publishMq = new ClaimOnLine_Api.Contract.ClaimReceiveGroupRequest()
                {
                    ClaimOnLineId = new Guid(),
                    ClaimPaybackPaymentHeaderId = createHeader.ClaimPaybackPaymentHeaderId,
                    CreatedByUserId = userId,
                    CreatedDate = DateTime.Now,
                    UpdatedByUserId = userId,
                    UpdatedDate = DateTime.Now,
                    IsActive = true,
                    ReceiveTypeId = (int)createHeader.ClaimPaybackPaymentTypeId,
                    Remark = "",
                    TransferTypeId = _transferTypeId,
                    ClaimReceiveItem = receiveItems
                };

                // publish Mq
                PublishToReceiveClaimOnline(publishMq);

                Log.Debug("{_controllerName} NonPerformingloanConfirm create DifferentPaymentDetail {DifferentPaymentHeaderId} is done", _controllerName, createHeader.ClaimPaybackPaymentHeaderId);
                Log.Information("{_controllerName} NonPerformingloanConfirm is done", _controllerName);

            }
            catch (Exception ex)
            {
                Log.Debug("{_controllerName} NonPerformingloanConfirm is errro {message}", _controllerName, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                return Json(new { valid = false, message = ex.InnerException != null ? ex.InnerException.Message : ex.Message });
            }

            return Json(new { valid = true });
        }

        private bool ValidateClaimPayBackXClaim(List<string> claimCodes)
        {
            bool result = true;
            var checkValidate = _context.ClaimPayBackXClaim.Where(w => claimCodes.Contains(w.ClaimCode) && w.IsActive == true).ToList();
            if (checkValidate.Count() != claimCodes.Count())
            {
                result = false;
            }


            return result;
        }

        private void PublishToReceiveClaimOnline(ClaimOnLine_Api.Contract.ClaimReceiveGroupRequest model)
        {
            Log.Debug("{_controllerName} Publish to rabbitMq headerId {HeaderId} is start.", _controllerName, model.ClaimPaybackPaymentHeaderId);

            var client = MvcApplication.bus.CreateRequestClient<ClaimOnLine_Api.Contract.ClaimReceiveGroupRequest>();
            var result = client.GetResponse<ClaimOnLine_Api.Contract.ClaimReceiveGroupResponse>(model);
            if (result.IsCompleted)
            {
                Log.Debug("{_controllerName} Publish to rabbitMq headerId  {headerId} is successfully.", _controllerName, model.ClaimPaybackPaymentHeaderId);
            }
            else
            {
                Log.Debug("{_controllerName} Publish to rabbitMq headerId  {headerId} is unsuccessfully.", _controllerName, model.ClaimPaybackPaymentHeaderId);
            }
        }

        #endregion

        #region "Report View"

        public ActionResult TransferPaymentReport()
        {
            int? branchid = null;

            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();

            var roleMO = new[] { "SmileClaimPayBack_Operation" }; //Operator

            if (rolelist.Intersect(roleMO).Count() > 0)
            {
                branchid = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
            }

            ViewBag.BranchId = branchid;
            ViewBag.ProductGroupId = _context.usp_ProductGroup_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.InsuranceCompany = _context.usp_OrganizeInsuranceForGenerateGroup_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.ClaimGroupType = _context.usp_ClaimGroupType_Select(null, 0, 9999, null, null, null).Where(x => x.ClaimGroupTypeId != 1).ToList();
            return View();
        }

        public ActionResult ClaimPayBackCancelledReport()
        {
            int? branchId = null;

            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();

            var roleLogin = new[] { "SmileClaimPayBack_Operation" }; // Login By Operator

            if (rolelist.Intersect(roleLogin).Count() > 0)
            {
                branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
            }
            ViewBag.BranchIdLogin = branchId;

            return View();
        }

        public ActionResult BillingReport()
        {
            ViewBag.ClaimHeaderGroupTypeId = _context.usp_ClaimHeaderGroupType_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.InsuranceCompany = _context.usp_OrganizeInsuranceForGenerateGroup_Select(null, 0, 9999, null, null, null).ToList();
            return View();
        }

        public ActionResult BillingRequestReport()
        {
            ViewBag.InsuranceCompany = _context.usp_OrganizeInsuranceForGenerateGroup_Select(null, 0, 9999, null, null, null).ToList();
            return View();
        }

        public ActionResult PaymentDifferenceResultReport()
        {
            ViewBag.InsuranceCompany = _context.usp_OrganizeInsuranceForGenerateGroup_Select(null, 0, 9999, null, null, null).ToList();
            return View();
        }

        public ActionResult EstimatePaymentReport()
        {

            //ViewBag.ProductGroupId = _context.usp_ProductGroup_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.ClaimHeaderGroupTypeId = _context.usp_ClaimHeaderGroupType_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.InsuranceCompany = _context.usp_OrganizeInsuranceForGenerateGroup_Select(null, 0, 9999, null, null, null).ToList();
            return View();
        }

        public ActionResult AmountPaymentReport()
        {
            ViewBag.ClaimHeaderGroupTypeId = _context.usp_ClaimHeaderGroupType_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.InsuranceCompany = _context.usp_OrganizeInsuranceForGenerateGroup_Select(null, 0, 9999, null, null, null).ToList();
            return View();
        }

        public ActionResult OutStandingBalanceDetailReport()
        {
            ViewBag.InsuranceCompany = _context.usp_OrganizeInsuranceForGenerateGroup_Select(null, 0, 9999, null, null, null).ToList();
            return View();
        }

        public ActionResult BillingRejectClaimReport()
        {
            ViewBag.InsuranceCompany = _context.usp_OrganizeInsuranceForGenerateGroup_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.DecisionStatus = _context.usp_DecisionStatus_Select().ToList();
            return View();
        }

        [HttpGet]
        [Authorization(Roles = "SmileClaimPayBack_FundManage,Developer")]
        public ActionResult ReconcileMonitor()
        {
            ViewBag.ClaimGroupType = _context.usp_ClaimGroupType_Select(null, 0, 9999, null, null, null).Where(x => x.ClaimGroupTypeId != 1).ToList();
            return View("ReconcileMonitor");
        }

        [HttpPost]
        public async Task<ActionResult> ExportExcelReconciles(int searchFrom, string StartDate, string endDate, int claimTypeId)
        {
            await Task.Yield();
            Session.Remove("ExportExcelReconcilesDownload");
            try
            {
                DateTime searcDatehFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(StartDate));
                DateTime searcDatehTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(endDate));

                /*
                 * searcDatehFrom set default date start 00.00.00
                 * searcDatehTo set default date start 23.59.00
                 */

                searcDatehTo = searcDatehTo.Add(new TimeSpan(23, 59, 59));

                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    var getTransfers = _context.ClaimPayBackTransfer.Where(w => w.IsActive == 1 && w.ClaimPayBackTransferStatusId == 3 && w.ClaimGroupTypeId == claimTypeId);
                    if (searchFrom != 1)
                    {
                        getTransfers = getTransfers.Where(w => w.CreatedDate >= searcDatehFrom && w.CreatedDate <= searcDatehTo);
                    }
                    else
                    {
                        getTransfers = getTransfers.Where(w => w.TransferDate >= searcDatehFrom && w.TransferDate <= searcDatehTo);
                    }

                    #region backup
                    //if (claimTypeId != 0)
                    //{
                    //    if (searchFrom != 1)
                    //    {
                    //        getTransfers = getTransfers.Where(w => w.ClaimGroupTypeId == claimTypeId && (w.CreatedDate >= searcDatehFrom && w.CreatedDate <= searcDatehTo));
                    //    }
                    //    else
                    //    {
                    //        getTransfers = getTransfers.Where(w => w.ClaimGroupTypeId == claimTypeId && (w.TransferDate >= searcDatehFrom && w.TransferDate <= searcDatehTo));
                    //    }
                    //}
                    //else
                    //{
                    //    if (searchFrom != 1)
                    //    {
                    //        getTransfers = getTransfers.Where(w => (w.CreatedDate >= searcDatehFrom && w.CreatedDate <= searcDatehTo));
                    //    }
                    //    else
                    //    {
                    //        getTransfers = getTransfers.Where(w => (w.TransferDate >= searcDatehFrom && w.TransferDate <= searcDatehTo));
                    //    }
                    //}
                    #endregion

                    // ตรวจสอบผลการค้นหา
                    if (getTransfers.Count() == 0)
                    {
                        return Json(new { isError = false, message = "ไม่พบข้อมูล" }, JsonRequestBehavior.AllowGet);
                    }

                    // สรุปการจ่ายเงิน 
                    var workSheet1 = package.Workbook.Worksheets.Add("สรุปการจ่ายเงิน");
                    workSheet1.Cells["A1"].Value = "รหัสการโอน";
                    workSheet1.Cells["B1"].Value = "วันที่โอนเงิน";
                    workSheet1.Cells["C1"].Value = "จำนวนสถานพยาบาล";
                    workSheet1.Cells["D1"].Value = "จำนวนเงินรวมทั้งหมด";
                    workSheet1.Cells["E1"].Value = "จำนวน บส ทั้งหมด";

                    var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                    headerCells1.Style.Font.Bold = true;
                    headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);
                    headerCells1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    var getSubGroupAll = _context.ClaimPayBackSubGroup.Where(w => getTransfers.Select(s => s.ClaimPayBackTransferId.ToString()).Contains(w.ClaimPayBackTransferId.ToString()) && w.IsActive == true);
                    int countTransfers = 2;
                    foreach (var getTransfer in getTransfers)
                    {
                        workSheet1.Cells["A" + countTransfers].Value = getTransfer.ClaimPayBackTransferCode;
                        workSheet1.Cells["B" + countTransfers].Value = getTransfer.TransferDate != null ? getTransfer.TransferDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : "-";
                        workSheet1.Cells["C" + countTransfers].Value = getSubGroupAll.Where(w => w.ClaimPayBackTransferId == getTransfer.ClaimPayBackTransferId).Count();
                        workSheet1.Cells["C" + countTransfers].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet1.Cells["D" + countTransfers].Value = Convert.ToDecimal(getTransfer.Amount).ToString("N");
                        workSheet1.Cells["D" + countTransfers].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        workSheet1.Cells["E" + countTransfers].Value = getSubGroupAll.Where(w => w.ClaimPayBackTransferId == getTransfer.ClaimPayBackTransferId).Sum(r => r.ItemCount) == null ? 0 : getSubGroupAll.Where(w => w.ClaimPayBackTransferId == getTransfer.ClaimPayBackTransferId).Sum(r => r.ItemCount);
                        workSheet1.Cells["E" + countTransfers].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        countTransfers++;
                    }

                    var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                    allCells1.AutoFilter = true;
                    allCells1.AutoFitColumns();

                    // กระทบยอด
                    var workSheet2 = package.Workbook.Worksheets.Add("กระทบยอด");
                    workSheet2.Cells["A1"].Value = "รหัสรายการ";
                    //workSheet2.Cells["B1"].Value = "ยอดเงินรวมCPB";
                    workSheet2.Cells["B1"].Value = "จำนวนเงินโอน";
                    workSheet2.Cells["C1"].Value = "สถานพยาบาล";
                    workSheet2.Cells["D1"].Value = "รหัสอ้างอิงธนาคาร";
                    workSheet2.Cells["E1"].Value = "เลขบัญชีผู้รับ";
                    workSheet2.Cells["F1"].Value = "ชื่อบัญชีผู้รับ";
                    workSheet2.Cells["G1"].Value = "วันที่สร้างรายการ";
                    workSheet2.Cells["H1"].Value = "วันที่โอนเงินผ่านธนาคาร";
                    workSheet2.Cells["I1"].Value = "ยืนยันยอดการโอน";

                    var headerCells2 = workSheet2.Cells[1, 1, 1, workSheet2.Dimension.Columns];
                    headerCells2.Style.Font.Bold = true;
                    headerCells2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);
                    headerCells2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int countSubGroups = 2;
                    var getSubGroups = getSubGroupAll;
                    var getTransactionByGroups = _context.ClaimPayBackSubGroupTransaction.Where(w => w.ClaimPayBackSubGroupTransactionStatusId == 5 && getSubGroups.Select(s => s.ClaimPayBackSubGroupId.ToString()).Contains(w.ClaimPayBackSubGroupId.ToString()) && w.IsActive == true).ToList();
                    foreach (var reconcile in getSubGroups)
                    {
                        var getTransactionByGroup = getTransactionByGroups.Where(w => w.ClaimPayBackSubGroupId == reconcile.ClaimPayBackSubGroupId).FirstOrDefault();
                        var getHospitalDetails = _context.usp_HospitalAccountDetails_Select(reconcile.HospitalCode).FirstOrDefault();

                        workSheet2.Cells["A" + countSubGroups].Value = reconcile.ClaimPayBackSubGroupCode;
                        //workSheet2.Cells["B" + countSubGroups].Value = "รอดำเนินการต่อ";
                        workSheet2.Cells["B" + countSubGroups].Value = Convert.ToDecimal(reconcile.Amount).ToString("N");
                        workSheet2.Cells["B" + countSubGroups].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        workSheet2.Cells["C" + countSubGroups].Value = reconcile.HospitalName;
                        workSheet2.Cells["D" + countSubGroups].Value = getTransactionByGroup != null ? getTransactionByGroup.TransRefNo : "-"; // reconcile.ReferenceNo;
                        workSheet2.Cells["E" + countSubGroups].Value = reconcile.HospitalCode != null ? getHospitalDetails.ReceivingBankAccountNo : "-";
                        workSheet2.Cells["F" + countSubGroups].Value = reconcile.HospitalCode != null ? getHospitalDetails.ReceivingName : "-";
                        workSheet2.Cells["G" + countSubGroups].Value = reconcile.CreatedDate != null ? reconcile.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : "-";
                        workSheet2.Cells["H" + countSubGroups].Value = reconcile.BillingTransferDate != null ? reconcile.BillingTransferDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : "-";
                        workSheet2.Cells["I" + countSubGroups].Value = reconcile.IsReconcile != true ? "รอยืนยัน" : "ยืนยันแล้ว";
                        workSheet2.Cells["I" + countSubGroups].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        countSubGroups++;
                    }

                    var allCells2 = workSheet2.Cells[workSheet2.Dimension.Address];
                    allCells2.AutoFilter = true;
                    allCells2.AutoFitColumns();

                    //รายละเอียด
                    var workSheet3 = package.Workbook.Worksheets.Add("รายละเอียด");
                    workSheet3.Cells["A1"].Value = "ลำดับ";
                    workSheet3.Cells["B1"].Value = "รหัสรายการ";
                    workSheet3.Cells["C1"].Value = "จำนวนราย";
                    workSheet3.Cells["D1"].Value = "ชื่อสถานพยาบาล";
                    workSheet3.Cells["E1"].Value = "จำนวนเงินโอน";
                    workSheet3.Cells["F1"].Value = "เลข บ.ส.";
                    workSheet3.Cells["G1"].Value = "ยอดเงินตาม บ.ส.";

                    var headerCells3 = workSheet3.Cells[1, 1, 1, workSheet3.Dimension.Columns];
                    headerCells3.Style.Font.Bold = true;
                    headerCells3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);
                    headerCells3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    // สร้างหัวตาราง
                    int countHeader = 1;
                    int countDataHeader = 2;
                    var getDetails = await _context.ClaimPayBackDetail.Where(w => w.IsActive == true && getSubGroups.Select(s => s.ClaimPayBackSubGroupId.ToString()).Contains(w.ClaimPayBackSubGroupId.ToString())).ToListAsync();
                    foreach (var getHead in getSubGroups)
                    {
                        workSheet3.Cells["A" + countDataHeader].Value = countHeader.ToString() + ".";
                        workSheet3.Cells["A" + countDataHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet3.Cells["B" + countDataHeader].Value = getHead.ClaimPayBackSubGroupCode;
                        workSheet3.Cells["C" + countDataHeader].Value = (int)getHead.ItemCount;
                        workSheet3.Cells["C" + countDataHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet3.Cells["D" + countDataHeader].Value = getHead.HospitalName;
                        workSheet3.Cells["E" + countDataHeader].Value = Convert.ToDecimal(getHead.Amount).ToString("N");
                        workSheet3.Cells["E" + countDataHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        workSheet3.Cells["F" + countDataHeader].Value = "";
                        workSheet3.Cells["G" + countDataHeader].Value = "";

                        int subCountHeader = 1;
                        var getDetail = getDetails.Where(w => w.ClaimPayBackSubGroupId == getHead.ClaimPayBackSubGroupId);
                        if (getDetail.Count() > 0)
                        {
                            foreach (var item in getDetail)
                            {
                                countDataHeader++;

                                workSheet3.Cells["A" + countDataHeader].Value = countHeader.ToString() + "." + subCountHeader;
                                workSheet3.Cells["A" + countDataHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                workSheet3.Cells["B" + countDataHeader].Value = "";
                                workSheet3.Cells["C" + countDataHeader].Value = "";
                                workSheet3.Cells["D" + countDataHeader].Value = "";
                                workSheet3.Cells["E" + countDataHeader].Value = "";
                                workSheet3.Cells["F" + countDataHeader].Value = item.ClaimGroupCode;
                                workSheet3.Cells["G" + countDataHeader].Value = Convert.ToDecimal(item.Amount).ToString("N");
                                workSheet3.Cells["G" + countDataHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                subCountHeader++;
                            }
                        }
                        else
                        {
                            countDataHeader++;
                        }

                        countDataHeader++;
                        countHeader++;
                    }

                    var allCells3 = workSheet3.Cells[workSheet3.Dimension.Address];
                    allCells3.AutoFilter = true;
                    allCells3.AutoFitColumns();
                    package.Save();


                    stream.Position = 0;
                    Session["ExportExcelReconcilesDownload"] = package.GetAsByteArray();

                }

                return Json(new { isError = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                Session.Remove("ExportExcelReconcilesDownload");
                return Json(new { isError = false, message = Ex.InnerException != null ? Ex.InnerException.Message : Ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportExcelReconcilesDownload(string claimTypeName, string startDate, string endDate)
        {
            //var dataKey = TempData["keyReport"].ToString();
            if (Session["ExportExcelReconcilesDownload"] != null)
            {
                //var period = TempData[dataKey] as string;

                byte[] data = Session["ExportExcelReconcilesDownload"] as byte[];
                string excelName = $"รายงานกระทบยอดเคลม_{claimTypeName}_{startDate}_{endDate}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        #endregion "Report View"

        #region "Method Report"

        [HttpPost]
        public async Task<ActionResult> ExportTransferPayment(string dateFrom, string dateTo, int searchTypeId, int? productGroupId, int? insuranceCompanyId, int? branchId = null, int? claimGroupTypeId = null)
        {
            await Task.Yield();
            Session.Remove("ExportTransferPaymentDownload");
            using (var db = new ClaimPayBackEntities())
            {
                DateTime d_DateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
                DateTime d_DateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));
                productGroupId = productGroupId == -1 ? null : productGroupId;
                insuranceCompanyId = insuranceCompanyId == -1 ? null : insuranceCompanyId;
                try
                {
                    //เคลมโรงพยาบาล
                    if (claimGroupTypeId == 4)
                    {
                        var dm = db.usp_Report_ClaimPayBackTransfer_Select(searchTypeId, d_DateFrom, d_DateTo, insuranceCompanyId, productGroupId, branchId, claimGroupTypeId).Select(x => new
                        {
                            บริษัทประกันภัย = x.InsuranceCompany
                         ,
                            สาขา = x.Branch
                         ,
                            กลุ่มแผนประกัน = x.ProductGroupDetail
                         ,
                            จังหวัด = x.HospitalProvince
                         ,
                            สถานพยาบาล = x.HospitalName
                         ,
                            เลข_บส = x.ClaimGroupCode
                         ,
                            จำนวนราย = x.ItemCount
                         ,
                            ยอดเงินตาม_บส = x.Amount
                         ,
                            ประเภทงานเคลม = x.ClaimGroupType
                         ,
                            วันที่ส่งการเงิน = x.CreatedDate != null ? x.CreatedDate.Value.Date.ToString("dd/MM/yyyy") : null
                         ,
                            เวลาที่ส่งการเงิน = x.CreatedDate != null ? x.CreatedDate.Value.ToString("HH:mm:ss") : null
                         ,
                            วันที่การเงินจ่าย = x.TransferDate != null ? x.TransferDate.Value.Date.ToString("dd/MM/yyyy") : null
                         ,
                            เวลาที่การเงินจ่าย = x.TransferDate != null ? x.TransferDate.Value.ToString("HH:mm:ss") : null
                         ,
                            ธนาคาร = x.Bank
                         ,
                            ชื่อบัญชี = x.BankAccountName
                         ,
                            เลขที่บัญชี = x.BankAccountNo
                         ,
                            ผู้ส่งอนุมัติ = x.empClaimApproved
                         ,
                            ผู้ส่งเบิก = x.empClaimPayBackCreated
                         ,
                        }).ToList();

                        var dmCL = db.usp_Report_ClaimPayBackTransferCL_Select(searchTypeId, d_DateFrom, d_DateTo, insuranceCompanyId, productGroupId, branchId, claimGroupTypeId).Select(x => new
                        {
                            บริษัทประกันภัย = x.InsuranceCompany_Name
                         ,
                            สาขา = x.Branch
                         ,
                            กลุ่มแผนประกัน = x.ProductGroupDetail
                         ,
                            จังหวัด = x.HospitalProvince
                         ,
                            สถานพยาบาล = x.HospitalName
                         ,
                            เลข_บส = x.ClaimGroupCode
                         ,
                            เลข_CL = x.ClaimCode
                         ,
                            x.CustomerName
                         ,
                            ยอดเงิน = x.Amount
                         ,
                            ประเภทการรักษา = x.ClaimAdmitType
                        }).ToList();

                        if (dm.Count > 0)
                        {
                            var stream = new MemoryStream();
                            using (var package = new ExcelPackage(stream))
                            {
                                var workSheet1 = package.Workbook.Worksheets.Add("Detail-เคลมรพ.");
                                workSheet1.Cells.LoadFromCollection(dm, true);

                                var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                                headerCells1.Style.Font.Bold = true;
                                headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);
                                var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                                allCells1.AutoFilter = true;
                                allCells1.AutoFitColumns();

                                var workSheet2 = package.Workbook.Worksheets.Add("Detial-เคลมรพ._แยกCL");
                                workSheet2.Cells.LoadFromCollection(dmCL, true);
                                workSheet2.Cells[1, 8].Value = "ชื่อ-นามสกุล";

                                var headerCells2 = workSheet2.Cells[1, 1, 1, workSheet2.Dimension.Columns];
                                headerCells2.Style.Font.Bold = true;
                                headerCells2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                headerCells2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);
                                var allCells2 = workSheet2.Cells[workSheet2.Dimension.Address];
                                allCells2.AutoFilter = true;
                                allCells2.AutoFitColumns();

                                package.Save();

                                stream.Position = 0;
                                Session["ExportTransferPaymentDownload"] = package.GetAsByteArray();
                            }
                        }
                        else
                        {
                            return Json(new { isError = true, Msg = "ไม่มีข้อมูล!!!" });
                        }
                    }
                    //เคลมโอนแยก
                    else if (claimGroupTypeId == 5)
                    {
                        var dc = db.usp_Report_ClaimPayBackTransfer_Select(searchTypeId, d_DateFrom, d_DateTo, insuranceCompanyId, productGroupId, branchId, claimGroupTypeId).Select(x => new
                        {
                            บริษัทประกันภัย = x.InsuranceCompany
                          ,
                            สาขา = x.Branch
                          ,
                            กลุ่มแผนประกัน = x.ProductGroupDetail
                          ,
                            เลข_บส = x.ClaimGroupCode
                          ,
                            เลขที่เคลมโอนแยก = x.ClaimCompensateCode
                          ,
                            เลขที่เคลม = x.ClaimCode
                          ,
                            ชื่อผู้เอาประกัน = x.CustName
                          ,
                            ยอดเงินตาม_บส = x.Amount
                          ,
                            ธนาคาร = x.Bank
                          ,
                            ชื่อบัญชี = x.BankAccountName
                          ,
                            เลขที่บัญชี = x.BankAccountNo
                          ,
                            เบอร์โทร = x.PhoneNo
                          ,
                            ประเภทงานเคลม = x.ClaimGroupType
                          ,
                            วันที่ส่งการเงิน = x.CreatedDate != null ? x.CreatedDate.Value.Date.ToString("dd/MM/yyyy") : null
                          ,
                            เวลาที่ส่งการเงิน = x.CreatedDate != null ? x.CreatedDate.Value.ToString("HH:mm:ss") : null
                          ,
                            วันที่การเงินจ่าย = x.TransferDate != null ? x.TransferDate.Value.Date.ToString("dd/MM/yyyy") : null
                          ,
                            เวลาที่การเงินจ่าย = x.TransferDate != null ? x.TransferDate.Value.ToString("HH:mm:ss") : null
                          ,
                            ผู้ส่งอนุมัติ = x.empClaimApproved
                            ,
                            ผู้ส่งเบิก = x.empClaimPayBackCreated
                            ,
                        }).ToList();

                        if (dc.Count > 0)
                        {
                            var stream = new MemoryStream();
                            using (var package = new ExcelPackage(stream))
                            {
                                var workSheet1 = package.Workbook.Worksheets.Add("Detail");
                                workSheet1.Cells.LoadFromCollection(dc, true);

                                var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                                headerCells1.Style.Font.Bold = true;
                                headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);
                                var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                                allCells1.AutoFilter = true;
                                allCells1.AutoFitColumns();

                                package.Save();

                                stream.Position = 0;
                                Session["ExportTransferPaymentDownload"] = package.GetAsByteArray();
                            }
                        }
                        else
                        {
                            return Json(new { isError = true, Msg = "ไม่มีข้อมูล!!!" });
                        }
                    }
                    else
                    {
                        var dt = db.usp_Report_ClaimPayBackTransfer_Select(searchTypeId, d_DateFrom, d_DateTo, insuranceCompanyId, productGroupId, branchId, claimGroupTypeId).Select(x => new
                        {
                            สาขา = x.Branch
                          ,
                            ภาค = x.Area
                          ,
                            เลข_บส = x.ClaimGroupCode
                          ,
                            COL = x.ClaimOnLineCode
                          ,
                            Product = x.ProductGroupDetail
                          ,
                            จำนวนราย = x.ItemCount
                          ,
                            ยอดเงินตาม_บส = x.Amount
                          ,
                            ประเภทงานเคลม = x.ClaimGroupType
                          ,
                            วันที่ส่งการเงิน = x.CreatedDate != null ? x.CreatedDate.Value.Date.ToString("dd/MM/yyyy") : null
                          ,
                            เวลาที่ส่งการเงิน = x.CreatedDate != null ? x.CreatedDate.Value.ToString("HH:mm:ss") : null
                          ,
                            วันที่การเงินจ่าย = x.TransferDate != null ? x.TransferDate.Value.Date.ToString("dd/MM/yyyy") : null
                          ,
                            เวลาที่การเงินจ่าย = x.TransferDate != null ? x.TransferDate.Value.ToString("HH:mm:ss") : null
                          ,
                            บริษัทประกันภัย = x.InsuranceCompany
                          ,
                            ผู้อนุมัติเคลมออนไลน์ = x.empClaimOnLineApprove
                          ,
                        }).ToList();

                        if (dt.Count > 0)
                        {
                            var stream = new MemoryStream();
                            using (var package = new ExcelPackage(stream))
                            {
                                var workSheet1 = package.Workbook.Worksheets.Add("Detail");
                                workSheet1.Cells.LoadFromCollection(dt, true);

                                var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                                headerCells1.Style.Font.Bold = true;
                                headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);
                                var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                                allCells1.AutoFilter = true;
                                allCells1.AutoFitColumns();

                                package.Save();

                                stream.Position = 0;
                                Session["ExportTransferPaymentDownload"] = package.GetAsByteArray();
                            }
                        }
                        else
                        {
                            return Json(new { isError = true, Msg = "ไม่มีข้อมูล!!!" });
                        }
                    }

                    return Json(new { isError = false }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception Ex)
                {
                    Session.Remove("ExportTransferPaymentDownload");
                    return Json(new { isError = true, Msg = Ex.Message });
                }
            }
        }

        public ActionResult ExportTransferPaymentDownload()
        {
            //var dataKey = TempData["keyReport"].ToString();
            if (Session["ExportTransferPaymentDownload"] != null)
            {
                //var period = TempData[dataKey] as string;

                byte[] data = Session["ExportTransferPaymentDownload"] as byte[];
                string excelName = $"ReportTransferPayment_{DateTime.Now}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public async Task<ActionResult> ExportCancelledClaimPayBack(string dateFrom, string dateTo, int? branchId)
        {
            await Task.Yield();
            Session.Remove("DownloadExcelCancelled");

            //var datePeriod = Convert.ToDateTime(period);
            using (var db = new ClaimPayBackEntities())
            {
                DateTime d_DateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
                DateTime d_DateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));


                try
                {
                    var dt = db.usp_Report_ClaimPayBackDetailCancelled_Select(d_DateFrom, d_DateTo, branchId).Select(x => new
                    {
                        สาขา = x.Branch
                     ,
                        ภาค = x.Area
                     ,
                        เลข_บส = x.ClaimGroupCode
                     ,
                        Product = x.ProductGroupDetail
                     ,
                        จำนวนราย = x.ItemCount
                     ,
                        ยอดเงินตาม_บส = x.Amount
                     ,
                        ประเภทงานเคลม = x.ClaimGroupType
                     ,
                        วันที่ส่งการเงิน = x.CreatedDate != null ? x.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : null
                     ,
                        บริษัทประกันภัย = x.InsuranceCompany
                     ,
                        เหตุผลการยกเลิก = x.CancelRemark
                     ,
                        ผู้ทำรายการ = x.UpdatedByName
                     ,
                        วันที่ทำรายการ = x.UpdatedDate != null ? x.UpdatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : null
                    }).ToList();

                    if (dt.Count > 0)
                    {
                        var stream = new MemoryStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            var workSheet1 = package.Workbook.Worksheets.Add("Detail");
                            workSheet1.Cells.LoadFromCollection(dt, true);

                            var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                            headerCells1.Style.Font.Bold = true;
                            headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);
                            var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                            allCells1.AutoFilter = true;
                            allCells1.AutoFitColumns();

                            package.Save();

                            stream.Position = 0;
                            Session["DownloadExcelCancelled"] = package.GetAsByteArray();
                        }
                    }
                    else
                    {
                        return Json(new { isError = true, Msg = "ไม่มีข้อมูล!!!" });
                    }

                    return Json(new { isError = false }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception Ex)
                {
                    Session.Remove("DownloadExcelCancelled");
                    return Json(new { isError = true, Msg = Ex.Message });
                }


            }
        }

        public ActionResult ExportCancelledDownload()
        {
            //var dataKey = TempData["keyReport"].ToString();
            if (Session["DownloadExcelCancelled"] != null)
            {
                //var period = TempData[dataKey] as string;

                byte[] data = Session["DownloadExcelCancelled"] as byte[];
                string excelName = $"ReportCancelledClaimPayBack_{DateTime.Now}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public async Task<ActionResult> ExportBillingRequest(string dateFrom, string dateTo, int? searchTypeId, int? insuranceCompanyId)
        {
            await Task.Yield();
            Session.Remove("ExportBillingRequestDownload");

            using (var db = new ClaimPayBackEntities())
            {
                DateTime d_DateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
                DateTime d_DateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo)); ;
                insuranceCompanyId = insuranceCompanyId == -1 ? null : insuranceCompanyId;

                try
                {
                    var dt = db.usp_Report_BillingRequest_Select(searchTypeId, d_DateFrom, d_DateTo, insuranceCompanyId).Select(x => new
                    {
                        วันที่นำเข้า = x.CreatedDate != null ? x.CreatedDate.Value.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null
                   ,
                        วันที่วางบิลบริษัทประกัน = x.BillingDate != null ? x.BillingDate.Value.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null
                   ,
                        บริษัทประกัน = x.InsuranceCompany
                   ,
                        กลุ่มเคลม = x.ClaimHeaderGroupTypeName
                   ,
                        เลขที่ชุด = x.ClaimHeaderGroupCode
                   ,
                        เลขที่เคลม = x.ClaimCode
                   ,
                        จำนวนราย = x.ItemCount
                   ,
                        x.ManualNPLAmount
                   ,
                        x.NPLTotalAmount
                   ,
                        x.AutoTotalAmount
                   ,
                        จำนวนเงินตั้งเบิกคงค้าง = x.TotalAmount
                   ,
                        ประเภทเคลม = x.Detail
                    }).ToList();

                    if (dt.Count > 0)
                    {
                        var stream = new MemoryStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            var workSheet1 = package.Workbook.Worksheets.Add("Detail");
                            workSheet1.Cells.LoadFromCollection(dt, true);
                            workSheet1.Cells["H1"].Value = "จำนวนเงินตั้งเบิก NPL";
                            workSheet1.Cells["I1"].Value = "จำนวนเงินตั้งเบิก NPL(ทั้งหมด)";
                            workSheet1.Cells["J1"].Value = "จำนวนเงินตั้งเบิก (Auto)";

                            var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                            headerCells1.Style.Font.Bold = true;
                            headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCells1.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#bdd7ee"));
                            var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                            allCells1.AutoFilter = true;
                            allCells1.AutoFitColumns();
                            package.Save();

                            stream.Position = 0;
                            Session["ExportBillingRequestDownload"] = package.GetAsByteArray();
                        }
                    }
                    else
                    {
                        return Json(new { isError = true, Msg = "ไม่มีข้อมูล!!!" });
                    }
                    return Json(new { isError = false }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception Ex)
                {
                    Session.Remove("ExportBillingRequestDownload");
                    return Json(new { isError = true, Msg = Ex.Message });
                }
            }
        }

        public ActionResult ExportBillingRequestDownload()
        {
            //var dataKey = TempData["keyReport"].ToString();
            if (Session["ExportBillingRequestDownload"] != null)
            {
                //var period = TempData[dataKey] as string;

                byte[] data = Session["ExportBillingRequestDownload"] as byte[];
                string excelName = $"รายงานกระทบยอดส่งบริษัทประกัน-{DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture)}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public async Task<ActionResult> ExportBilling(int? searchTypeId, int? claimHeaderGroupTypeId, string dateFrom, string dateTo, int? productTypeId, int? insuranceCompanyId)
        {
            await Task.Yield();

            Session.Remove("DownloadExcelBilling");
            using (var db = new ClaimPayBackEntities())
            {
                DateTime d_DateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
                DateTime d_DateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));

                var ClaimType = "";
                if (productTypeId == 1)
                {
                    ClaimType = "1000";
                }
                else if (productTypeId == 2)
                {
                    ClaimType = "2000";
                }
                else
                {
                    ClaimType = null;
                }

                try
                {
                    if (claimHeaderGroupTypeId == -1)
                    {
                        var responePH = db.usp_ReportBillingRequestResultConfirm_Select(searchTypeId, null, d_DateFrom, d_DateTo, ClaimType, insuranceCompanyId, 0, 9999, null, null, null).Select((x, index) => new
                        {
                            No = index + 1,
                            ImportDate = x.ImportDate != null ? x.ImportDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            BillingDate = x.BillingDate != null ? x.BillingDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            DueDate = x.BillingDueDate != null ? x.BillingDueDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            x.Branch,
                            x.InsuranceCompany,
                            x.ClaimHeaderGroupCode,
                            x.PolicyNo,
                            App_id = x.ApplicationCode,
                            x.Product,
                            x.Province,
                            Code = x.ClaimCode,
                            ZcardId = x.IdentityCard,
                            CustomerName = x.CustName,
                            StartCoverDate = x.StartCoverDate != null ? x.StartCoverDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            x.ClaimAdmitType,
                            x.HospitalName,
                            x.ICD10_1Code,
                            x.ICD10,
                            DateHappen = x.DateHappen != null ? x.DateHappen.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            DateIn = x.DateIn != null ? x.DateIn.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            DateOut = x.DateOut != null ? x.DateOut.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            net = x.Net, //**** ค่ารักษา
                            x.Compensate_Include,
                            Pay = x.Pay, //*** ยอดเบิก
                            Pay_Total = x.Pay_Total, //**** รวมจ่าย
                            x.IPDCount,
                            x.ICUCount,
                            x.ClaimType,
                            x.BillingRequestGroupCode,
                            x.BillingRequestItemCode,
                            DocumentLink = "",

                            PaymentReferenceID = x.PaymentReferenceId,
                            CoverAmount = x.CoverAmount,
                            x.UncoverAmount,
                            x.UnCoverRemark,
                            x.DecisionStatus,
                            x.RejectResult,
                            DecisionDate = x.DecisionDate != null ? x.DecisionDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            EstimatePaymentDate = x.EstimatePaymentDate != null ? x.EstimatePaymentDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            Remark2 = x.Remark, //**** หมายเหตุ

                            PaymentDate = x.PaymentDate != null ? x.PaymentDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            x.AmountPayment,
                            x.BankName,
                            x.BankAccountName,
                            x.BankAccountNumber,
                            x.Remark3,

                        }).ToList();


                        var resPA = db.usp_ReportBillingRequestResultConfirm_Select(searchTypeId, null, d_DateFrom, d_DateTo, ClaimType, insuranceCompanyId, 0, 9999, null, null, null).Select((x, index) => new
                        {
                            No = index + 1,
                            ImportDate = x.ImportDate != null ? x.ImportDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            BillingDate = x.BillingDate != null ? x.BillingDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            DueDate = x.BillingDueDate != null ? x.BillingDueDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            x.Branch,
                            x.InsuranceCompany,
                            x.ClaimHeaderGroupCode,
                            x.PolicyNo,
                            App_ID = x.ApplicationCode,
                            Code = x.ClaimCode,
                            x.Province,
                            x.SchoolName,
                            x.CustomerDetailCode,
                            ZcardId = x.IdentityCard,
                            CustomerName = x.CustName,
                            x.SchoolLevel,
                            DateHappen = x.DateHappen != null ? x.DateHappen.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            x.Accident,
                            x.ChiefComplain,
                            x.Orgen,
                            x.Pay, //**** ยอดเบิก
                            Compensate_Include = x.Amount_Compensate_in,
                            Compensate_Out = x.Amount_Compensate_out,
                            Amount_Pay = x.Amount_Pay,///****ค่ารักษา
                            x.Amount_Dead,
                            Pay_Total = x.Pay_Total, //**** รวมจ่าย
                            x.ClaimAdmitType,
                            x.HospitalName,
                            x.ICD10_1Code,
                            x.ICD10,
                            Remark = x.PaRemark,
                            DateIn = x.DateIn != null ? x.DateIn.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            DateOut = x.DateOut != null ? x.DateOut.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            x.ClaimType,
                            x.BillingRequestGroupCode,
                            x.BillingRequestItemCode,
                            DocumentLink = "",

                            PaymentReferenceID = x.PaymentReferenceId,
                            CoverAmount = x.CoverAmount,
                            x.UncoverAmount,
                            x.UnCoverRemark,
                            x.DecisionStatus,
                            x.RejectResult,
                            DecisionDate = x.DecisionDate != null ? x.DecisionDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            EstimatePaymentDate = x.EstimatePaymentDate != null ? x.EstimatePaymentDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            Remark2 = x.Remark,

                            PaymentDate = x.PaymentDate != null ? x.PaymentDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            x.AmountPayment,
                            x.BankName,
                            x.BankAccountName,
                            x.BankAccountNumber,
                            x.Remark3,

                        }).ToList();

                        var stream = new MemoryStream();
                        using (var package = new ExcelPackage(stream))
                        {

                            //PH Sheet
                            var workSheet1 = package.Workbook.Worksheets.Add("PH");
                            workSheet1.Cells.LoadFromCollection(responePH.Where(w => w.BillingRequestItemCode.Substring(3, 2) == "PH"), true);

                            var headerCells1 = workSheet1.Cells["A1:AF1"];
                            headerCells1.Style.Font.Bold = true;
                            headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;

                            headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);

                            var headerCells2 = workSheet1.Cells["AG1:AO1"];
                            headerCells2.Style.Font.Bold = true;
                            headerCells2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCells2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

                            var headerCells3 = workSheet1.Cells["AP1:AU1"];
                            headerCells3.Style.Font.Bold = true;
                            headerCells3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCells3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);

                            var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                            allCells1.AutoFilter = true;
                            allCells1.AutoFitColumns();




                            //PA Sheet
                            var workSheet2 = package.Workbook.Worksheets.Add("PA");
                            workSheet2.Cells.LoadFromCollection(resPA.Where(w => w.BillingRequestItemCode.Substring(3, 2) == "PA"), true);

                            var headerCellsPA1 = workSheet2.Cells["A1:AK1"];
                            headerCellsPA1.Style.Font.Bold = true;
                            headerCellsPA1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCellsPA1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);

                            var headerCellsPA2 = workSheet2.Cells["AL1:AT1"];
                            headerCellsPA2.Style.Font.Bold = true;
                            headerCellsPA2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCellsPA2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

                            var headerCellsPA3 = workSheet2.Cells["AU1:AZ1"];
                            headerCellsPA3.Style.Font.Bold = true;
                            headerCellsPA3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCellsPA3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);

                            var allCellsPA1 = workSheet2.Cells[workSheet2.Dimension.Address];
                            allCellsPA1.AutoFilter = true;
                            allCellsPA1.AutoFitColumns();

                            package.Save();

                            stream.Position = 0;
                            Session["DownloadExcelBilling"] = package.GetAsByteArray();
                        }
                    }

                    if (claimHeaderGroupTypeId == 2 || claimHeaderGroupTypeId == 4 || claimHeaderGroupTypeId == 5)//PH
                    {
                        var dt = db.usp_ReportBillingRequestResultConfirm_Select(searchTypeId, claimHeaderGroupTypeId, d_DateFrom, d_DateTo, ClaimType, insuranceCompanyId, 0, 9999, null, null, null).Select((x, index) => new
                        {
                            No = index + 1,
                            ImportDate = x.ImportDate != null ? x.ImportDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            BillingDate = x.BillingDate != null ? x.BillingDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            DueDate = x.BillingDueDate != null ? x.BillingDueDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            x.Branch,
                            x.InsuranceCompany,
                            x.ClaimHeaderGroupCode,
                            x.PolicyNo,
                            App_id = x.ApplicationCode,
                            x.Product,
                            x.Province,
                            Code = x.ClaimCode,
                            ZcardId = x.IdentityCard,
                            CustomerName = x.CustName,
                            StartCoverDate = x.StartCoverDate != null ? x.StartCoverDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            x.ClaimAdmitType,
                            x.HospitalName,
                            x.ICD10_1Code,
                            x.ICD10,
                            DateHappen = x.DateHappen != null ? x.DateHappen.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            DateIn = x.DateIn != null ? x.DateIn.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            DateOut = x.DateOut != null ? x.DateOut.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            net = x.Net, //**** ค่ารักษา
                            x.Compensate_Include,
                            Pay = x.Pay, //*** ยอดเบิก
                            Pay_Total = x.Pay_Total, //**** รวมจ่าย
                            x.IPDCount,
                            x.ICUCount,
                            x.ClaimType,
                            x.BillingRequestGroupCode,
                            x.BillingRequestItemCode,
                            DocumentLink = "",

                            PaymentReferenceID = x.PaymentReferenceId,
                            CoverAmount = x.CoverAmount,
                            x.UncoverAmount,
                            x.UnCoverRemark,
                            x.DecisionStatus,
                            x.RejectResult,
                            DecisionDate = x.DecisionDate != null ? x.DecisionDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            EstimatePaymentDate = x.EstimatePaymentDate != null ? x.EstimatePaymentDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            Remark2 = x.Remark, //**** หมายเหตุ

                            PaymentDate = x.PaymentDate != null ? x.PaymentDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            x.AmountPayment,
                            x.BankName,
                            x.BankAccountName,
                            x.BankAccountNumber,
                            x.Remark3,

                        }).ToList();

                        if (dt.Count > 0)
                        {
                            var stream = new MemoryStream();
                            using (var package = new ExcelPackage(stream))
                            {
                                var workSheet1 = package.Workbook.Worksheets.Add("PH");
                                workSheet1.Cells.LoadFromCollection(dt, true);

                                var headerCells1 = workSheet1.Cells["A1:AF1"];
                                headerCells1.Style.Font.Bold = true;
                                headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;

                                headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);

                                var headerCells2 = workSheet1.Cells["AG1:AO1"];
                                headerCells2.Style.Font.Bold = true;
                                headerCells2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                headerCells2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

                                var headerCells3 = workSheet1.Cells["AP1:AU1"];
                                headerCells3.Style.Font.Bold = true;
                                headerCells3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                headerCells3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);

                                var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                                allCells1.AutoFilter = true;
                                allCells1.AutoFitColumns();

                                package.Save();

                                stream.Position = 0;
                                Session["DownloadExcelBilling"] = package.GetAsByteArray();
                            }
                        }
                        else
                        {
                            return Json(new { isError = true, Msg = "ไม่มีข้อมูล!!!" });
                        }
                    }
                    else if (claimHeaderGroupTypeId == 3) //PA
                    {
                        var dt = db.usp_ReportBillingRequestResultConfirm_Select(searchTypeId, claimHeaderGroupTypeId, d_DateFrom, d_DateTo, ClaimType, insuranceCompanyId, 0, 9999, null, null, null).Select((x, index) => new
                        {
                            No = index + 1,
                            ImportDate = x.ImportDate != null ? x.ImportDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            BillingDate = x.BillingDate != null ? x.BillingDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            DueDate = x.BillingDueDate != null ? x.BillingDueDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            x.Branch,
                            x.InsuranceCompany,
                            x.ClaimHeaderGroupCode,
                            x.PolicyNo,
                            App_ID = x.ApplicationCode,
                            Code = x.ClaimCode,
                            x.Province,
                            x.SchoolName,
                            x.CustomerDetailCode,
                            ZcardId = x.IdentityCard,
                            CustomerName = x.CustName,
                            x.SchoolLevel,
                            DateHappen = x.DateHappen != null ? x.DateHappen.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            x.Accident,
                            x.ChiefComplain,
                            x.Orgen,
                            x.Pay, //**** ยอดเบิก
                            Compensate_Include = x.Amount_Compensate_in,
                            Compensate_Out = x.Amount_Compensate_out,
                            Amount_Pay = x.Amount_Pay,///****ค่ารักษา
                            x.Amount_Dead,
                            Pay_Total = x.Pay_Total, //**** รวมจ่าย
                            x.ClaimAdmitType,
                            x.HospitalName,
                            x.ICD10_1Code,
                            x.ICD10,
                            Remark = x.PaRemark,
                            DateIn = x.DateIn != null ? x.DateIn.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            DateOut = x.DateOut != null ? x.DateOut.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            x.ClaimType,
                            x.BillingRequestGroupCode,
                            x.BillingRequestItemCode,
                            DocumentLink = "",

                            PaymentReferenceID = x.PaymentReferenceId,
                            CoverAmount = x.CoverAmount,
                            x.UncoverAmount,
                            x.UnCoverRemark,
                            x.DecisionStatus,
                            x.RejectResult,
                            DecisionDate = x.DecisionDate != null ? x.DecisionDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            EstimatePaymentDate = x.EstimatePaymentDate != null ? x.EstimatePaymentDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            Remark2 = x.Remark,

                            PaymentDate = x.PaymentDate != null ? x.PaymentDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                            x.AmountPayment,
                            x.BankName,
                            x.BankAccountName,
                            x.BankAccountNumber,
                            x.Remark3,

                        }).ToList();

                        if (dt.Count > 0)
                        {
                            var stream = new MemoryStream();
                            using (var package = new ExcelPackage(stream))
                            {
                                var workSheet1 = package.Workbook.Worksheets.Add("PA");
                                workSheet1.Cells.LoadFromCollection(dt, true);

                                var headerCells1 = workSheet1.Cells["A1:AK1"];
                                headerCells1.Style.Font.Bold = true;
                                headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;

                                headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);

                                var headerCells2 = workSheet1.Cells["AL1:AT1"];
                                headerCells2.Style.Font.Bold = true;
                                headerCells2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                headerCells2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

                                var headerCells3 = workSheet1.Cells["AU1:AZ1"];
                                headerCells3.Style.Font.Bold = true;
                                headerCells3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                headerCells3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);

                                var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                                allCells1.AutoFilter = true;
                                allCells1.AutoFitColumns();

                                package.Save();

                                stream.Position = 0;
                                Session["DownloadExcelBilling"] = package.GetAsByteArray();
                            }
                        }
                        else
                        {
                            return Json(new { isError = true, Msg = "ไม่มีข้อมูล!!!" });
                        }

                    }
                    return Json(new { isError = false }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception Ex)
                {
                    Session.Remove("DownloadExcelBilling");
                    return Json(new { isError = true, Msg = Ex.Message });
                }
            }
        }

        public ActionResult ExportBillingDownload()
        {
            if (Session["DownloadExcelBilling"] != null)
            {
                //var period = TempData[dataKey] as string;

                byte[] data = Session["DownloadExcelBilling"] as byte[];
                string excelName = $"รายงานการวางบิลบริษัทประกัน-{DateTime.Now}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }


        [HttpPost]
        public async Task<ActionResult> ExportPaymentDifferenceResult(string dateFrom, string dateTo, int? searchTypeId, int? insuranceCompanyId)
        {
            await Task.Yield();
            Session.Remove("ExportPaymentDifferenceResultDownload");

            using (var db = new ClaimPayBackEntities())
            {
                DateTime d_DateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
                DateTime d_DateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));
                insuranceCompanyId = insuranceCompanyId == -1 ? null : insuranceCompanyId;

                try
                {
                    var dt = db.usp_Report_PaymentDifferenceResult_Select(searchTypeId, d_DateFrom, d_DateTo, insuranceCompanyId).Select(x => new
                    {

                        BillingDate = x.BillingDate != null ? x.BillingDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                        x.BillingRequestGroupCode,
                        x.InsuranceCompany,
                        x.Branch,
                        ClaimHeaderGroup_id = x.ClaimHeaderGroupCode,
                        Code = x.ClaimCode,
                        Customer_Name = x.CustName,
                        x.Pay_Total,
                        x.ClaimType,
                        x.ClaimHeaderGroupType,
                        x.CoverAmount,
                        PaymentDate = x.PaymentDate != null ? x.PaymentDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                        x.AmountPayment,
                        x.UncoverAmount,
                        x.UnCoverRemark,
                        Difference = x.SUMDifference,
                        x.DecisionStatus,
                        x.RejectResult

                    }).ToList();

                    if (dt.Count > 0)
                    {
                        var stream = new MemoryStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            var workSheet1 = package.Workbook.Worksheets.Add("Detail");
                            workSheet1.Cells.LoadFromCollection(dt, true);

                            var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                            headerCells1.Style.Font.Bold = true;
                            headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);
                            var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                            allCells1.AutoFilter = true;
                            allCells1.AutoFitColumns();
                            package.Save();

                            stream.Position = 0;
                            Session["ExportPaymentDifferenceResultDownload"] = package.GetAsByteArray();
                        }
                    }
                    else
                    {
                        return Json(new { isError = true, Msg = "ไม่มีข้อมูล!!!" });
                    }
                    return Json(new { isError = false }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception Ex)
                {
                    Session.Remove("ExportPaymentDifferenceResultDownload");
                    return Json(new { isError = true, Msg = Ex.Message });
                }
            }
        }

        public ActionResult ExportPaymentDifferenceResultDownload()
        {
            //var dataKey = TempData["keyReport"].ToString();
            if (Session["ExportPaymentDifferenceResultDownload"] != null)
            {
                //var period = TempData[dataKey] as string;

                byte[] data = Session["ExportPaymentDifferenceResultDownload"] as byte[];
                string excelName = $"รายงานผลต่างการชำระเงิน-{DateTime.Now.ToString("ddMMyyyy-HHMM", CultureInfo.InvariantCulture)}.xlsx"; //รายงานผลต่างการชำระเงิน - DDMMYYYY - HHMM

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }


        [HttpPost]
        public async Task<ActionResult> ExportEstimatePaymentReport(string dateFrom, string dateTo, int? searchTypeId, int? productGroupId, int? insuranceCompanyId)
        {
            await Task.Yield();
            Session.Remove("ExportEstimatePaymentReportDownload");

            using (var db = new ClaimPayBackEntities())
            {
                DateTime d_DateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
                DateTime d_DateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));
                productGroupId = productGroupId == -1 ? null : productGroupId;
                insuranceCompanyId = insuranceCompanyId == -1 ? null : insuranceCompanyId;

                try
                {
                    var dt = db.usp_Report_EstimatePaymentReport_Select(searchTypeId, d_DateFrom, d_DateTo, insuranceCompanyId, productGroupId).Select(x => new
                    {

                        BillingDate = x.BillingDate != null ? x.BillingDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                        DueDate = x.Duedate != null ? x.Duedate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                        x.BillingRequestGroup,
                        x.InsuranceCompany,
                        x.ClaimHeaderGroup_id,
                        x.Code,
                        //x.ClaimCount,
                        x.Pay_Total,
                        x.ClaimType,
                        x.ClaimHeaderGroupType,
                        x.CoverAmount,
                        EstimatePaymentDate = x.EstimatePaymentDate != null ? x.EstimatePaymentDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                        x.Remark2
                    }).ToList();

                    if (dt.Count > 0)
                    {
                        var stream = new MemoryStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            var workSheet1 = package.Workbook.Worksheets.Add("Detail");
                            workSheet1.Cells["A3"].LoadFromCollection(dt, true);

                            workSheet1.Cells["A1:B1"].Merge = true;
                            workSheet1.Cells["A1:C1"].Merge = true;
                            workSheet1.Cells["A1"].Value = "รายงานยอดเงินที่คาดว่าจะได้รับ";
                            workSheet1.Cells["A1"].Style.Font.Bold = true;
                            workSheet1.Cells["K1:L1"].Merge = true;
                            workSheet1.Cells["K1"].Value = $"ประจำวันที่  {DateTime.Now.ToString("dd/MM/yyyy")}";
                            workSheet1.Cells["K1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            workSheet1.Cells["K1"].Style.Font.Bold = true;

                            var headerCells1 = workSheet1.Cells[3, 1, 3, workSheet1.Dimension.Columns];
                            headerCells1.Style.Font.Bold = true;
                            headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCells1.Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFF"));
                            headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#4471C4"));

                            var allCells1 = workSheet1.Cells["A3:K3"];
                            var allCells2 = workSheet1.Cells[workSheet1.Dimension.Address];

                            allCells1.AutoFilter = true;
                            allCells2.AutoFitColumns();
                            package.Save();

                            stream.Position = 0;
                            Session["ExportEstimatePaymentReportDownload"] = package.GetAsByteArray();
                        }
                    }
                    else
                    {
                        return Json(new { isError = true, Msg = "ไม่มีข้อมูล!!!" });
                    }
                    return Json(new { isError = false }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception Ex)
                {
                    Session.Remove("ExportEstimatePaymentReportDownload");
                    return Json(new { isError = true, Msg = Ex.Message });
                }
            }
        }

        public ActionResult ExportEstimatePaymentReportDownload()
        {
            //var dataKey = TempData["keyReport"].ToString();
            if (Session["ExportEstimatePaymentReportDownload"] != null)
            {
                //var period = TempData[dataKey] as string;

                byte[] data = Session["ExportEstimatePaymentReportDownload"] as byte[];
                string excelName = $"รายงานยอดเงินที่คาดว่าจะได้รับ-{DateTime.Now.ToString("ddMMyyyy-HHMM", CultureInfo.InvariantCulture)}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public async Task<ActionResult> ExportAmountPaymentReport(string dateFrom, string dateTo, int? searchTypeId, int? productGroupId, int? insuranceCompanyId)
        {
            await Task.Yield();
            Session.Remove("ExportAmountPaymentReportDownload");

            using (var db = new ClaimPayBackEntities())
            {
                DateTime d_DateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
                DateTime d_DateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));
                productGroupId = productGroupId == -1 ? null : productGroupId;
                insuranceCompanyId = insuranceCompanyId == -1 ? null : insuranceCompanyId;

                try
                {
                    var dt = db.usp_Report_AmountPaymentReport_Select(searchTypeId, d_DateFrom, d_DateTo, insuranceCompanyId, productGroupId).Select(x => new
                    {

                        BillingDate = x.BillingDate != null ? x.BillingDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                        DueDate = x.Duedate != null ? x.Duedate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                        x.BillingRequestGroup,
                        x.ClaimHeaderGroup_id,
                        x.Branch,
                        x.InsuranceCompany,
                        x.Code,
                        //x.ClaimCount,
                        x.Pay_Total,
                        x.ClaimType,
                        x.ClaimHeaderGroupType,
                        PaymentDate = x.PaymentDate != null ? x.PaymentDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                        x.AmountPayment,
                        x.Pay_Period
                    }).ToList();

                    if (dt.Count > 0)
                    {
                        var stream = new MemoryStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            var workSheet1 = package.Workbook.Worksheets.Add("Detail");
                            workSheet1.Cells["A3"].LoadFromCollection(dt, true);

                            workSheet1.Cells["A1:K1"].Merge = true;
                            workSheet1.Cells["A1"].Value = "รายงานรับชำระจากบริษัทประกัน";
                            workSheet1.Cells["A1"].Style.Font.Bold = true;
                            workSheet1.Cells["L1:M1"].Merge = true;
                            workSheet1.Cells["L1"].Value = $"ประจำวันที่ : {DateTime.Now.ToString("dd/MM/yyyy")}";
                            workSheet1.Cells["L1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            workSheet1.Cells["L1"].Style.Font.Bold = true;

                            var headerCells1 = workSheet1.Cells[3, 1, 3, workSheet1.Dimension.Columns];
                            headerCells1.Style.Font.Bold = true;
                            headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCells1.Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFF"));
                            headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#4471C4"));

                            var allCells1 = workSheet1.Cells["A3:M3"];
                            var allCells2 = workSheet1.Cells[workSheet1.Dimension.Address];

                            allCells1.AutoFilter = true;
                            allCells2.AutoFitColumns();
                            package.Save();

                            stream.Position = 0;
                            Session["ExportAmountPaymentReportDownload"] = package.GetAsByteArray();
                        }
                    }
                    else
                    {
                        return Json(new { isError = true, Msg = "ไม่มีข้อมูล!!!" });
                    }
                    return Json(new { isError = false }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception Ex)
                {
                    Session.Remove("ExportAmountPaymentReportDownload");
                    return Json(new { isError = true, Msg = Ex.Message });
                }
            }
        }

        public ActionResult ExportAmountPaymentReportDownload()
        {
            //var dataKey = TempData["keyReport"].ToString();
            if (Session["ExportAmountPaymentReportDownload"] != null)
            {
                //var period = TempData[dataKey] as string;

                byte[] data = Session["ExportAmountPaymentReportDownload"] as byte[];
                string excelName = $"รายงานรับชำระจากบ.ประกัน-{DateTime.Now.ToString("ddMMyyyy-HHMM", CultureInfo.InvariantCulture)}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public async Task<ActionResult> ExportOutStandingBalanceDetailReport(string dateFrom, string dateTo, int? insuranceCompanyId)
        {
            await Task.Yield();
            Session.Remove("ExportOutStandingBalanceDetailReportDownload");

            using (var db = new ClaimPayBackEntities())
            {
                DateTime d_DateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
                DateTime d_DateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));
                insuranceCompanyId = insuranceCompanyId == -1 ? null : insuranceCompanyId;

                try
                {
                    var dt = db.usp_Report_OutStandingBalanceDetail_Select(d_DateFrom, d_DateTo, insuranceCompanyId).Select(x => new
                    {

                        BillingDate = x.BillingDate != null ? x.BillingDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                        x.BillingRequestGroup,
                        x.ClaimHeaderGroupType,
                        x.InsuranceCompany,
                        x.Branch,
                        x.ClaimHeaderGroup_id,
                        x.Code,
                        x.CustomerName,
                        x.Pay_Total,
                        x.ClaimType,
                        x.OverDue_Period
                    }).ToList();

                    if (dt.Count > 0)
                    {
                        var stream = new MemoryStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            var workSheet1 = package.Workbook.Worksheets.Add("Detail");
                            workSheet1.Cells["A3"].LoadFromCollection(dt, true);

                            workSheet1.Cells["A1:I1"].Merge = true;
                            workSheet1.Cells["A1"].Value = "รายงานรายละเอียดยอดคงค้าง";
                            workSheet1.Cells["A1"].Style.Font.Bold = true;
                            workSheet1.Cells["J1:K1"].Merge = true;
                            workSheet1.Cells["J1"].Value = $"ประจำวันที่ : {DateTime.Now.ToString("dd/MM/yyyy")}";
                            workSheet1.Cells["J1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            workSheet1.Cells["J1"].Style.Font.Bold = true;

                            var headerCells1 = workSheet1.Cells[3, 1, 3, workSheet1.Dimension.Columns];
                            headerCells1.Style.Font.Bold = true;
                            headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCells1.Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFF"));
                            headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#4471C4"));

                            var allCells1 = workSheet1.Cells["A3:K3"];
                            var allCells2 = workSheet1.Cells[workSheet1.Dimension.Address];
                            allCells1.AutoFilter = true;
                            allCells2.AutoFitColumns();
                            package.Save();

                            stream.Position = 0;
                            Session["ExportOutStandingBalanceDetailReportDownload"] = package.GetAsByteArray();
                        }
                    }
                    else
                    {
                        return Json(new { isError = true, Msg = "ไม่มีข้อมูล!!!" });
                    }
                    return Json(new { isError = false }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception Ex)
                {
                    Session.Remove("ExportOutStandingBalanceDetailReportDownload");
                    return Json(new { isError = true, Msg = Ex.Message });
                }
            }
        }

        public ActionResult ExportOutStandingBalanceDetailReportDownload()
        {
            //var dataKey = TempData["keyReport"].ToString();
            if (Session["ExportOutStandingBalanceDetailReportDownload"] != null)
            {
                //var period = TempData[dataKey] as string;

                byte[] data = Session["ExportOutStandingBalanceDetailReportDownload"] as byte[];
                string excelName = $"รายงานรายละเอียดยอดคงค้าง-{DateTime.Now.ToString("ddMMyyyy-HHMM", CultureInfo.InvariantCulture)}.xlsx"; //รายงานผลต่างการชำระเงิน - DDMMYYYY - HHMM

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public async Task<ActionResult> ExportBillingRejectClaimReport(string dateFrom, string dateTo, int? insuranceCompanyId, int? DecisionStatusId)
        {
            await Task.Yield();
            Session.Remove("ExportBillingRejectClaimReportDownload");

            using (var db = new ClaimPayBackEntities())
            {
                DateTime d_DateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
                DateTime d_DateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));
                insuranceCompanyId = insuranceCompanyId == -1 ? null : insuranceCompanyId;
                DecisionStatusId = DecisionStatusId == -1 ? null : DecisionStatusId;

                try
                {
                    var dt = db.usp_Report_BillingRejectClaim_select(DecisionStatusId, d_DateFrom, d_DateTo, insuranceCompanyId).Select(x => new
                    {

                        BillingDate = x.BillingDate != null ? x.BillingDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                        x.BillingRequestGroup,
                        x.ClaimAdmit_Type,
                        x.InsuranceCompany,
                        x.Branch,
                        x.ClaimHeaderGroup_id,
                        x.Code,
                        x.Customer_Name,
                        x.Pay_Total,
                        x.UncoverAmount,
                        x.UnCoverRemark,
                        x.DecisionStatus,
                        DecisionDate = x.DecisionDate != null ? x.DecisionDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                        x.RejectResult,
                        x.ClaimType
                    }).ToList();

                    if (dt.Count > 0)
                    {
                        var stream = new MemoryStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            var workSheet1 = package.Workbook.Worksheets.Add("Detail");
                            workSheet1.Cells["A3"].LoadFromCollection(dt, true);

                            workSheet1.Cells["A1:M1"].Merge = true;
                            workSheet1.Cells["A1"].Value = "รายงานเคลมปฏิเสธ";
                            workSheet1.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet1.Cells["A1"].Style.Font.Bold = true;

                            workSheet1.Cells["N1:O1"].Merge = true;
                            workSheet1.Cells["N1"].Value = $"ประจำวันที่ : {DateTime.Now.ToString("dd/MM/yyyy")}";
                            workSheet1.Cells["N1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            workSheet1.Cells["N1"].Style.Font.Bold = true;

                            var headerCells1 = workSheet1.Cells[3, 1, 3, workSheet1.Dimension.Columns];
                            headerCells1.Style.Font.Bold = true;
                            headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCells1.Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFF"));
                            headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#4471C4"));

                            var allCells1 = workSheet1.Cells["A3:K3"];
                            var allCells2 = workSheet1.Cells[workSheet1.Dimension.Address];
                            allCells1.AutoFilter = true;
                            allCells2.AutoFitColumns();
                            package.Save();

                            stream.Position = 0;
                            Session["ExportBillingRejectClaimReportDownload"] = package.GetAsByteArray();
                        }
                    }
                    else
                    {
                        return Json(new { isError = true, Msg = "ไม่มีข้อมูล!!!" });
                    }
                    return Json(new { isError = false }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception Ex)
                {
                    Session.Remove("ExportBillingRejectClaimReportDownload");
                    return Json(new { isError = true, Msg = Ex.Message });
                }
            }
        }

        public ActionResult ExportBillingRejectClaimReportDownload()
        {
            //var dataKey = TempData["keyReport"].ToString();
            if (Session["ExportBillingRejectClaimReportDownload"] != null)
            {
                //var period = TempData[dataKey] as string;

                byte[] data = Session["ExportBillingRejectClaimReportDownload"] as byte[];
                string excelName = $"รายงานเคลมปฏิเสธ-{DateTime.Now.ToString("ddMMyyyy-HHMM", CultureInfo.InvariantCulture)}.xlsx"; //รายงานผลต่างการชำระเงิน - DDMMYYYY - HHMM

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }


        #endregion "Method Report"

        #region "Method Branch"

        public ActionResult GetClaimPayBackMonitor(int? draw, int? pageSize, int? indexStart, string sortField, string orderType, string searchDetail,
                                                   string dateFrom, string dateTo, int? branchId, int? statusId)
        {
            var CreatedDateFrom = Convert.ToDateTime(dateFrom);
            var CreatedDateTo = Convert.ToDateTime(dateTo);
            branchId = branchId == -1 ? null : branchId;
            statusId = statusId == -1 ? null : statusId;
            var list = _context.usp_ClaimPayBack_Select(CreatedDateFrom, CreatedDateTo, statusId, branchId, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                { "draw", draw },
                { "recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                { "recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                { "data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClaimPayBackDetail(int? draw, int? pageSize, int? indexStart, string sortField, string orderType, string searchDetail,
                                                   int? claimPayBackId)
        {
            var list = _context.usp_ClaimPayBackDetailByHeader_Select(claimPayBackId, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                { "draw", draw },
                { "recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                { "recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                { "data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBSDetail(int? draw, int? pageSize, int? indexStart, string sortField, string orderType, string searchDetail, int? claimPayBackDetailId)
        {
            var list = _context.usp_ClaimPayBackDetailXClaim_Select(claimPayBackDetailId, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                { "draw", draw },
                { "recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                { "recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                { "data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBSForCreate(int? draw, int? pageSize, int? indexStart, string sortField, string orderType, int? productGroupId, int? claimGroupTypeId, int? insuranceId, int? branchId, string searchDetail, bool? isShowDocument = false, string createByUserCode = null)
        {
            productGroupId = productGroupId == -1 ? null : productGroupId;
            claimGroupTypeId = claimGroupTypeId == -1 ? null : claimGroupTypeId;
            insuranceId = insuranceId == -1 ? null : insuranceId;
            branchId = branchId == -1 ? null : branchId;
            createByUserCode = createByUserCode == "-1" ? null : createByUserCode;
            isShowDocument = isShowDocument == false ? null : isShowDocument;

            var list = _context.usp_ClaimHeaderGroupDetail_SelectV3(productGroupId, insuranceId, claimGroupTypeId, branchId, createByUserCode, indexStart, pageSize, sortField, orderType, searchDetail, isShowDocument).ToList();

            var dt = new Dictionary<string, object>
            {
                { "draw", draw },
                { "recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                { "recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                { "data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBSDetailForCreate(int? draw, int? pageSize, int? indexStart, string sortField, string orderType, string searchDetail, string claimGroupCode, int productGroupId, int claimGroupTypeId)
        {
            var list = _context.usp_ClaimHeaderGroupItem_Select(claimGroupCode, productGroupId, claimGroupTypeId, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                { "draw", draw },
                { "recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                { "recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                { "data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InsertClaimPayBack(string[] claimGroupCodeListArray, int productGroupId, int claimGroupTypeId)
        {
            try {
                string claimGroupCodeList = "";

                if (claimGroupCodeListArray != null) {
                    claimGroupCodeList = string.Join(",", claimGroupCodeListArray);
                }
                int userId = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;
                var creardByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

                Log.Information("{_controllerName} usp_ClaimPayBackDetail_InsertV4 is start. Parameters: claimGroupCodeList:{claimGroupCodeList},productGroupId:{productGroupId},claimGroupTypeId:{claimGroupTypeId},creardByUserId{creardByUserId}", _controllerName, claimGroupCodeList, productGroupId, claimGroupTypeId, creardByUserId);

                var rs = _context.usp_ClaimPayBackDetail_InsertV4(claimGroupCodeList, productGroupId, claimGroupTypeId, creardByUserId).ToList();

                Log.Information("{_controllerName} usp_ClaimPayBackDetail_InsertV4 finished result: {rs} ", _controllerName,rs);
                var result = rs.FirstOrDefault();
                if (rs.Count() > 0 && result.IsResult == true && result.ClaimOnLineId != null) {
                    Log.Debug("{_controllerName} InsertClaimPayBack is start.", _controllerName);

                    //var getClaimPayBacks = _context.ClaimPayBack.FirstOrDefault(f => f.ClaimPayBackTransferId == claimPayBackTransferId);
                    var receiveItems = new List<ClaimOnLine_Api.Contract.ClaimReceiveItemList>();
                    //int _transferTypeId = 0;
                    //foreach (var getDetail in rs.Where(w=>w.InsuranceCompanyId == 699804).ToList()) {
                    foreach (var getDetail in rs.ToList()) {
                        //var getXClaims = _context.ClaimPayBackXClaim.FirstOrDefault(f => f.ClaimCode == getDetail.ClaimCode);
                        //var getClaimGroupCode = _context.ClaimPayBackDetail.Where(w => w.ClaimPayBackDetailId == getXClaims.ClaimPayBackDetailId).Select(s => s.ClaimGroupCode).FirstOrDefault();
                        // set detail data for publish 
                        receiveItems.Add(new ClaimOnLine_Api.Contract.ClaimReceiveItemList
                        {
                            Amount = (decimal)getDetail.ClaimPay,
                            ClaimCode = getDetail.ClaimCode,
                            ClaimPaybackPaymentDetailId = new Guid("00000000-0000-0000-0000-000000000000"),
                            NPLDetailId = new Guid("00000000-0000-0000-0000-000000000000"),
                            ClaimGroupCode = getDetail.ClaimGroupCode,
                            ClaimOnlineItemId = (getDetail.ClaimOnLineItemId == null ? new Guid("00000000-0000-0000-0000-000000000000") : getDetail.ClaimOnLineItemId.Value),
                            ClaimPayBackCode = getDetail.ClaimPayBackCode,
                            ClaimPayBackXClaimId = (int)getDetail.ClaimPayBackXClaimId
                        });
                    }
                    // set header data for publish 
                    var publishMq = new ClaimOnLine_Api.Contract.ClaimReceiveGroupRequest()
                    {
                        ClaimOnLineId = new Guid(),
                        ClaimPaybackPaymentHeaderId = new Guid("00000000-0000-0000-0000-000000000000"),
                        CreatedByUserId = userId,
                        CreatedDate = DateTime.Now,
                        UpdatedByUserId = userId,
                        UpdatedDate = DateTime.Now,
                        IsActive = true,
                        ReceiveTypeId = (int)(rs.FirstOrDefault().ReceiveTypeId),
                        Remark = "",
                        TransferTypeId = (int)(rs.FirstOrDefault().TransferTypeId),
                        ClaimReceiveItem = receiveItems
                    };

                    Log.Debug("{_controllerName} Publish rabbitMq is start.", _controllerName);

                    // publish Mq
                    PublishToReceiveClaimOnline(publishMq);

                    Log.Information("{_controllerName} Publish rabbitMq is done.", _controllerName);
                    
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                Log.Error(ex, "{_controllerName} InsertClaimPayBack Error",_controllerName);
                return Json(new { IsResult = false, Message = "เกิดข้อผิดพลาดขณะบันทึกข้อมูล" }, JsonRequestBehavior.AllowGet);
            }
            
            
        }

        public ActionResult RemoveBS(int? claimPayBackDetailId, string remark)
        {
            var creardByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _context.usp_ClaimPayBackDetail_Update(claimPayBackDetailId, remark, creardByUserId).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion "Method Branch"

        #region "Method Transfer"

        public JsonResult GetClaimPaymentReceiptGroup(string dateStart, string dateEnd, int? claimGroupTypeId, int indexStart, int draw, int pageSize, string sortField, string orderType, string searchDetail)
        {
            DateTime d_dateStart;
            DateTime d_dateEnd;

            d_dateStart = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateStart));
            d_dateEnd = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateEnd));

            var list = _context.usp_ClaimPayBackForGroupTransfer_Select(d_dateStart, d_dateEnd, claimGroupTypeId, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClaimPaymentReceipGroupByTransfer(int? claimPayBackTransferid, int indexStart, int draw, int pageSize, string sortField, string orderType, string searchDetail)
        {
            var list = _context.usp_ClaimPayBackByTransfer_Select(claimPayBackTransferid, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClaimPayBackTransfer(string createdDateFrom, string createdDateTo, int? transferStatusId, int draw, int indexStart, int pageSize, string sortField, string orderType, string searchDetail)
        {
            DateTime d_DateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(createdDateFrom));
            DateTime d_DateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(createdDateTo));

            transferStatusId = transferStatusId == -1 ? null : transferStatusId;

            var list = _context.usp_ClaimPayBackTransfer_Select(d_DateFrom, d_DateTo, transferStatusId, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClaimPayBackTransferMonitor(string createdDateFrom, string createdDateTo, int? transferStatusId, int? claimGroupType, int draw, int indexStart, int pageSize, string sortField, string orderType, string searchDetail)
        {
            DateTime d_DateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(createdDateFrom));
            DateTime d_DateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(createdDateTo));

            transferStatusId = transferStatusId == -1 ? null : transferStatusId;
            claimGroupType = claimGroupType == -1 ? null : claimGroupType;

            var list = _context.usp_ClaimPayBackTransferMonitor_Select(d_DateFrom, d_DateTo, transferStatusId, claimGroupType, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateTransfer(int claimPayBackTransferId, string transferAmount, string transferDate, string transferTimeDate)
        {
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;
            transferTimeDate = transferTimeDate + ":00";
            var d_TransferDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(transferDate, transferTimeDate));
            Decimal d_TransferAmount = Convert.ToDecimal(transferAmount);

            var rs = _context.usp_ClaimPayBackTransfer_Update(claimPayBackTransferId, d_TransferAmount, d_TransferDate, userId).ToList();
            if (rs.Count() > 0 && rs.FirstOrDefault().IsResult == true)
            {
                Log.Debug("{_controllerName} UpdateTransfer is start.", _controllerName);

                var getClaimPayBacks = _context.ClaimPayBack.FirstOrDefault(f => f.ClaimPayBackTransferId == claimPayBackTransferId);
                var receiveItems = new List<ClaimOnLine_Api.Contract.ClaimReceiveItemList>();
                foreach (var getDetail in rs.ToList())
                {
                    var getXClaims = _context.ClaimPayBackXClaim.FirstOrDefault(f => f.ClaimCode == getDetail.ClaimCode);
                    var getClaimGroupCode = _context.ClaimPayBackDetail.Where(w => w.ClaimPayBackDetailId == getXClaims.ClaimPayBackDetailId).Select(s => s.ClaimGroupCode).FirstOrDefault();
                    // set detail data for publish 
                    receiveItems.Add(new ClaimOnLine_Api.Contract.ClaimReceiveItemList
                    {
                        Amount = (decimal)getDetail.ClaimPay,
                        ClaimCode = getDetail.ClaimCode,
                        ClaimPaybackPaymentDetailId = new Guid("00000000-0000-0000-0000-000000000000"),
                        NPLDetailId = new Guid("00000000-0000-0000-0000-000000000000"),
                        ClaimGroupCode = getClaimGroupCode,
                        ClaimOnlineItemId = (getDetail.ClaimOnLineItemId == null ? new Guid("00000000-0000-0000-0000-000000000000") : getDetail.ClaimOnLineItemId.Value),
                        ClaimPayBackCode = getClaimPayBacks.ClaimPayBackCode,
                        ClaimPayBackXClaimId = getXClaims.ClaimPayBackXClaimId
                    });
                }

                // ตั้งรายการ transferTypeId ตาม ReceiveTypeId
                int _transferTypeId = 0;
                switch (rs.FirstOrDefault().ReceiveTypeId)
                {
                    case 2:
                        _transferTypeId = 4;
                        break;
                    case 3:
                        _transferTypeId = 7;
                        break;
                    case 4:
                        _transferTypeId = 8;
                        break;
                    case 5:
                        _transferTypeId = 4;
                        break;
                }

                // set header data for publish 
                var publishMq = new ClaimOnLine_Api.Contract.ClaimReceiveGroupRequest()
                {
                    ClaimOnLineId = new Guid(),
                    ClaimPaybackPaymentHeaderId = new Guid("00000000-0000-0000-0000-000000000000"),
                    CreatedByUserId = userId,
                    CreatedDate = DateTime.Now,
                    UpdatedByUserId = userId,
                    UpdatedDate = DateTime.Now,
                    IsActive = true,
                    ReceiveTypeId = rs.FirstOrDefault().ReceiveTypeId,
                    Remark = "",
                    TransferTypeId = _transferTypeId,
                    ClaimReceiveItem = receiveItems
                };

                Log.Debug("{_controllerName} Publish rabbitMq is start.", _controllerName);

                // publish Mq
                PublishToReceiveClaimOnline(publishMq);

                Log.Information("{_controllerName} Publish rabbitMq is done.", _controllerName);
            }

            return Json(new
            {
                IsResult = rs.Count() > 0 ? rs.FirstOrDefault().IsResult : true,
                Msg = rs.Count() > 0 ? rs.FirstOrDefault().Msg : "บันทึกข้อมูล สำเร็จ",
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InsertTransfer(string[] claimPayBackIdArray, int claimGroupTypeId)
        {
            var claimPaybackIdList = "";
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            if (claimPayBackIdArray != null)
            {
                claimPaybackIdList = string.Join(",", claimPayBackIdArray);
            }

            var rs = _context.usp_ClaimPayBackTransfer_Insert(claimPaybackIdList, claimGroupTypeId, userId).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveChangeTranferGroup(int? ClaimPayBackTransferId)
        {
            var creardByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            //Validate Cancel
            var getTransfer = _context.ClaimPayBackTransfer.Where(w => w.ClaimPayBackTransferId == ClaimPayBackTransferId && w.IsActive == 1).FirstOrDefault();

            if (getTransfer == null)
            {
                return Json(new { IsResult = false, Result = "Failure", Msg = "ไม่พบข้อมูล" }, JsonRequestBehavior.AllowGet);
            }

            if (getTransfer.ClaimPayBackTransferStatusId != 5)
            {
                return Json(new { IsResult = false, Result = "Failure", Msg = "ยกเลิกได้เฉพาะสถานะรอสร้างรายการ" }, JsonRequestBehavior.AllowGet);
            }

            var result = _context.usp_ClaimPayBackTranfer_Cancel(ClaimPayBackTransferId, creardByUserId).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion "Method Transfer"

        #region "Method ClaimPayBackMonitorPDF"

        private List<ClaimPayBackMonitorModel> LoadClaimPayBackMonitorPDF(string claimPayBackId)
        {
            try
            {
                ViewBag.claimPayBackId = claimPayBackId;
                byte[] idByte = Convert.FromBase64String(claimPayBackId);
                string id = System.Text.Encoding.UTF8.GetString(idByte);
                var ClaimPayBackMonitor = _context.usp_ClaimPayBackDetailByHeader_Select(Int32.Parse(id), 0, 9999, "true", string.Empty, string.Empty).Select(x => new ClaimPayBackMonitorModel
                {
                    ClaimGroupCode = x.ClaimGroupCode,
                    ItemCount = x.ItemCount,
                    Amount = x.Amount,
                    ClaimGroupType = x.ClaimGroupType,
                    CreatedDate = x.CreatedDate,
                    ProductGroup = x.ProductGroup,
                    TotalCount = x.TotalCount,
                    InsuranceCompany = x.InsuranceCompany,
                    BranchDetail = x.BranchDetail,
                    HospitalName = x.HospitalName,
                    ClaimGroupTypeId = x.ClaimGroupTypeId,
                }).ToList();
                return ClaimPayBackMonitor;
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoadClaimPayBackMonitorPDF:" + ex.Message);
                return null;
            }
        }

        #endregion "Method ClaimPayBackMonitorPDF"

        #region ไฟล์นำส่ง บ.ประกัน

        public ActionResult ClaimPayBackBillingGroupDetail()
        {
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();

            var roleMO = new[] { "SmileClaimPayBack_Operation" }; //Operator

            var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;

            if (rolelist.Intersect(roleMO).Count() > 0)
            {
                ViewBag.Branch = _context.usp_Branch_Select(branchId, null, 0, 9999, null, null, null).ToList();
                ViewBag.checkBranch = false;
            }
            else
            {
                ViewBag.Branch = _context.usp_Branch_Select(null, null, 0, 9999, null, null, null).ToList();
                ViewBag.checkBranch = true;
            }

            ViewBag.Status = _context.usp_ClaimPayBackStatus_Select(null, 0, 9999, null, null, null).Where(x => x.ClaimPayBackStatusId != 1).ToList();
            ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            return View();
        }

        #endregion ไฟล์นำส่ง บ.ประกัน

        #region Module Export ไฟล์นำส่ง บ.ประกัน

        public ActionResult ClaimPayBackBillingRequestItem(string BillingRequestGroupId, string BillingRequestGroupCode)
        {
            try
            {
                //decode
                byte[] _byteBillingRequestGroupId = Convert.FromBase64String(BillingRequestGroupId);
                string _strBillingRequestGroupId = System.Text.Encoding.UTF8.GetString(_byteBillingRequestGroupId);

                byte[] _byteBillingRequestGroupCode = Convert.FromBase64String(BillingRequestGroupCode);
                string _strBillingRequestGroupCode = System.Text.Encoding.UTF8.GetString(_byteBillingRequestGroupCode);

                ViewBag.BillingRequestGroupId = _strBillingRequestGroupId;
                ViewBag.BillingRequestGroupCode = _strBillingRequestGroupCode;

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ClaimPayBackBillingRequestItem:" + ex.Message);
                ViewBag.ErrorMessage = ex.Message;
                return View("ErrorMessage");
            }
        }

        public JsonResult GetBillingRequestMonitor(int? insuranceCompanyId, string billingDate, int statusId, int draw, int indexStart, int pageSize, string sortField, string orderType, string searchDetail)
        {
            Log.Debug("{_controllerName} Start GetBillingRequestMonitor ", _controllerName);
            DateTime d_BillingDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(billingDate));

            string _search1 = Regex.Replace(searchDetail, @"-", "");    //ตัด -
            string _search2 = Regex.Replace(_search1, @" ", "");        //ตัด space bar

            var _sortField = sortField == "" ? null : sortField;

            Log.Debug($"{_controllerName} - GetBillingRequestMonitor Call Store procedure: usp_BillingRequestGroup_Select [insuranceCompanyId = {insuranceCompanyId}, billingDate = {d_BillingDate},billingRequestGroupStatusId = {statusId}, indexStart = {indexStart}, pageSize = {pageSize},sortField = {_sortField}, orderType = {orderType}, searchDetail = {_search2} ]");
            var list = _context.usp_BillingRequestGroup_Select(insuranceCompanyId == -1 ? null : insuranceCompanyId, d_BillingDate, statusId, indexStart, pageSize, _sortField, orderType, _search2).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            Log.Information("{_controllerName} - GetBillingRequestMonitor Successful", _controllerName);

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBillingRequestDetail(int? billingRequestGroupId = null, int? draw = null, int? indexStart = null, int? pageSize = null, string sortField = null, string orderType = null, string searchDetail = null)
        {
            Log.Debug("{_controllerName} Start GetBillingRequestDetail ", _controllerName);
            Log.Debug($"{_controllerName} - GetBillingRequestDetail Call Store procedure: usp_BillingRequestItem_Select [billingRequestGroupId = {billingRequestGroupId}, indexStart = {indexStart}, pageSize = {pageSize}, searchDetail = {searchDetail} ]");

            var list = _context.usp_BillingRequestItem_Select(billingRequestGroupId, indexStart, pageSize, sortField == "" ? null : sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            Log.Information("{_controllerName} - GetBillingRequestDetail Successful", _controllerName);

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> ExportExcelClaimPayBackBillingRequestGroup(int? billingRequestGroupId = null, string billingRequestGroupCode = null, int? actionId = null, string insuranceCompanyCode = null, string insuranceCompanyName = null)
        {
            try
            {
                Log.Debug("{_controllerName} Start ExportExcelClaimPayBackBillingRequestGroup [billingRequestGroupId = {billingRequestGroupId}, billingRequestGroupCode = {billingRequestGroupCode}, actionId = {actionId}]", _controllerName, billingRequestGroupId, billingRequestGroupCode, actionId);

                bool IsPH = false;
                string ClaimHeaderGroupCodeIndex3 = "";
                //substring เพื่อนำ 5 ตัวแรกมาเช็คว่าเป็น PH หรือ PA
                string billingRequestGroupCodeType = billingRequestGroupCode.Substring(0, 5);

                if (billingRequestGroupCodeType == "BQGPH")
                    IsPH = true;

                string fileName = null;

                //Check Directory 
                //actionId == 1 is for SFTP

                string tempPath = Properties.Settings.Default.TempBillingPath;
                var getBillingRequestGroup = _context.BillingRequestGroup.FirstOrDefault(f => f.BillingRequestGroupCode == billingRequestGroupCode);

                if (actionId == 1 && !Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                    Log.Debug("{_controllerName} - CreateDirectory in server project {tempPath = {tempPath}}", _controllerName, tempPath);
                }

                if (IsPH)
                {
                    Log.Debug($"{_controllerName} - ExportExcelClaimPayBackBillingRequestGroup Call Store procedure: usp_BillingRequestItem_Select [billingRequestGroupId = {billingRequestGroupId}, pageSize = 10000]");
                    var resultPH = _context.usp_BillingRequestItem_Select(billingRequestGroupId, null, 10000, null, null, null).Select((x, index) => new DocumentLinkPHModel
                    {
                        No = index + 1,
                        BillingDate = x.BillingDate != null ? x.BillingDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                        DueDate = x.BillingDueDate != null ? x.BillingDueDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                        Branch = x.Branch,
                        ClaimHeaderGroupCode = x.ClaimHeaderGroupCode,
                        PolicyNo = x.PolicyNo,
                        App_id = x.ApplicationCode,
                        Product = x.Product,
                        Province = x.Province,
                        Code = x.ClaimCode,
                        ZcardId = x.IdentityCard,
                        CustomerName = x.CustName,
                        StartCoverDate = x.StartCoverDate != null ? x.StartCoverDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                        ClaimAdmitType = x.ClaimAdmitType,
                        HospitalName = x.HospitalName,
                        ICD10_1Code = x.ICD10_1Code,
                        ICD10 = x.ICD10,
                        DateHappen = x.DateHappen != null ? x.DateHappen.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                        DateIn = x.DateIn != null ? x.DateIn.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                        DateOut = x.DateOut != null ? x.DateOut.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                        Net = x.Net,
                        Compensate_Include = x.Compensate_Include,
                        Pay = x.Pay,
                        Pay_Total = x.Pay_Total,
                        IPDCount = x.IPDCount,
                        ICUCount = x.ICUCount,
                        ClaimType = x.ClaimType,
                        BillingRequestGroupCode = x.BillingRequestGroupCode,
                        BillingRequestItemCode = x.BillingRequestItemCode,
                        DocumentLink = x.DocumentLink,
                        PaymentReferenceID = "",
                        CoverAmount = "",
                        UncoverAmount = "",
                        UnCoverRemark = "",
                        DecisionStatus = "",
                        RejectResult = "",
                        DecisionDate = "",
                        EstimatePaymentDate = "",
                        Remark2 = "",
                        PaymentDate = "",
                        AmountPayment = "",
                        BankName = "",
                        BankAccountName = "",
                        BankAccountNumber = "",
                        Remark3 = ""
                    }).ToList();

                    //เช็ครายการเคลม
                    if (resultPH.Count < 1)
                    {
                        return Json(ResponseResult.Failure<string>(_invalidClaimInClaimHeaderGroup), JsonRequestBehavior.AllowGet);
                    }

                    var resultPHWithLink = GetS3URL(billingRequestGroupId, resultPH, null);

                    var stream = new MemoryStream();
                    using (var package = new ExcelPackage(stream))
                    {
                        var IsPA30 = resultPHWithLink.Item1[0].ClaimHeaderGroupCode.Substring(2, 1) == "P";
                        ClaimHeaderGroupCodeIndex3 = resultPHWithLink.Item1[0].ClaimHeaderGroupCode.Substring(3, 1);
                        BillingSummarySheet(package, billingRequestGroupCode, ClaimHeaderGroupCodeIndex3, IsPH, IsPA30);

                        var workSheet2 = package.Workbook.Worksheets.Add(IsPA30 ? "รายงาน ไฟล์ส่ง บ.ประกัน PA30" : "รายงาน ไฟล์ส่ง บ.ประกัน PH");
                        workSheet2.Cells.LoadFromCollection(resultPHWithLink.Item1, true);
                        workSheet2.Cells["G1"].Value = "App_id";
                        workSheet2.Cells["V1"].Value = "Compensate_Include";
                        workSheet2.Cells["X1"].Value = "Pay_Total";

                        var headerCells1 = workSheet2.Cells["A1:AD1"];
                        headerCells1.Style.Font.Bold = true;
                        headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);

                        var headerCells2 = workSheet2.Cells["AE1:AM1"];
                        headerCells2.Style.Font.Bold = true;
                        headerCells2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

                        var headerCells3 = workSheet2.Cells["AN1:AS1"];
                        headerCells3.Style.Font.Bold = true;
                        headerCells3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);

                        var allCells1 = workSheet2.Cells[workSheet2.Dimension.Address];
                        allCells1.AutoFilter = true;
                        allCells1.AutoFitColumns();

                        //GenerateBillingFileName
                        // string resultReplaceBillingRequestGroupCode = (getBillingRequestGroup.ClaimHeaderGroupTypeId == 4 ? ReplaceBillingRequestGroupCode(billingRequestGroupCode) : billingRequestGroupCode);
                        fileName = GenerateBillingFileName(billingRequestGroupCode);
                        //fileName = GenerateBillingFileName(billingRequestGroupCode);
                        //Combine local path file
                        var tempPathFile = Path.Combine(tempPath, fileName);

                        if (actionId == 1) // for SFTP
                        {
                            ////get file in local
                            //string[] files = Directory.GetFiles(tempPath);
                            //foreach (string file in files)
                            //{
                            //    //Delete all file in local
                            //    System.IO.File.Delete(file);
                            //}
                            //save excel file in local
                            FileInfo excelFile = new FileInfo(tempPathFile);
                            package.SaveAs(excelFile);
                        }
                        else if (actionId == 2) // for export excel
                        {
                            package.Save();

                            stream.Position = 0;
                            Session["DownloadExcel"] = package.GetAsByteArray();
                        }
                    }
                }
                else if (IsPH == false)
                {
                    Log.Debug($"{_controllerName} - ExportExcelClaimPayBackBillingRequestGroup Call Store procedure: usp_BillingRequestItem_Select [billingRequestGroupId = {billingRequestGroupId}, pageSize = 10000]");
                    var resultPA = _context.usp_BillingRequestItem_Select(billingRequestGroupId, null, 10000, null, null, null).Select((x, index) => new DocumentLinkPAModel
                    {
                        No = index + 1,
                        BillingDate = x.BillingDate != null ? x.BillingDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                        DueDate = x.BillingDueDate != null ? x.BillingDueDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                        Branch = x.Branch,
                        ClaimHeaderGroupCode = x.ClaimHeaderGroupCode,
                        PolicyNo = x.PolicyNo,
                        App_ID = x.ApplicationCode,
                        Code = x.ClaimCode,
                        Province = x.Province,
                        SchoolName = x.SchoolName,
                        CustomerDetailCode = x.CustomerDetailCode,
                        ZcardId = x.IdentityCard,
                        CustomerName = x.CustName,
                        SchoolLevel = x.SchoolLevel,
                        DateHappen = x.DateHappen != null ? x.DateHappen.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                        Accident = x.Accident,
                        ChiefComplain = x.ChiefComplain,
                        Orgen = x.Orgen,
                        Pay = x.Pay,
                        Compensate_Include = x.Amount_Compensate_in,
                        Compensate_Out = x.Amount_Compensate_out,
                        Amount_Pay = x.Amount_Pay,
                        Amount_Dead = x.Amount_Dead,
                        Pay_Total = x.Pay_Total,
                        ClaimAdmitType = x.ClaimAdmitType,
                        HospitalName = x.HospitalName,
                        ICD10_1Code = x.ICD10_1Code,
                        ICD10 = x.ICD10,
                        Remark = x.Remark,
                        DateIn = x.DateIn != null ? x.DateIn.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                        DateOut = x.DateOut != null ? x.DateOut.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                        ClaimType = x.ClaimType,
                        BillingRequestGroupCode = x.BillingRequestGroupCode,
                        BillingRequestItemCode = x.BillingRequestItemCode,
                        DocumentLink = x.DocumentLink,
                        PaymentReferenceID = "",
                        CoverAmount = "",
                        UncoverAmount = "",
                        UnCoverRemark = "",
                        DecisionStatus = "",
                        RejectResult = "",
                        DecisionDate = "",
                        EstimatePaymentDate = "",
                        Remark2 = "",
                        PaymentDate = "",
                        AmountPayment = "",
                        BankName = "",
                        BankAccountName = "",
                        BankAccountNumber = "",
                        Remark3 = ""
                    }).ToList();

                    //เช็ครายการเคลม
                    if (resultPA.Count < 1)
                    {
                        return Json(ResponseResult.Failure<string>(_invalidClaimInClaimHeaderGroup), JsonRequestBehavior.AllowGet);
                    }

                    var resultPAWithLink = GetS3URL(billingRequestGroupId, null, resultPA);

                    var stream = new MemoryStream();
                    using (var package = new ExcelPackage(stream))
                    {
                        ClaimHeaderGroupCodeIndex3 = resultPAWithLink.Item2[0].ClaimHeaderGroupCode.Substring(3, 1);
                        BillingSummarySheet(package, billingRequestGroupCode, ClaimHeaderGroupCodeIndex3, IsPH, false);

                        var workSheet2 = package.Workbook.Worksheets.Add("รายงาน ไฟล์ส่ง บ.ประกัน PA");
                        workSheet2.Cells.LoadFromCollection(resultPAWithLink.Item2, true);
                        workSheet2.Cells["G1"].Value = "App_ID";
                        workSheet2.Cells["T1"].Value = "Compensate_Include";
                        workSheet2.Cells["U1"].Value = "Compensate_Out";
                        workSheet2.Cells["V1"].Value = "Amount_Pay";
                        workSheet2.Cells["W1"].Value = "Amount_Dead";
                        workSheet2.Cells["X1"].Value = "Pay_Total";

                        var headerCells1 = workSheet2.Cells["A1:AI1"];
                        headerCells1.Style.Font.Bold = true;
                        headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);

                        var headerCells2 = workSheet2.Cells["AJ1:AR1"];
                        headerCells2.Style.Font.Bold = true;
                        headerCells2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

                        var headerCells3 = workSheet2.Cells["AS1:AX1"];
                        headerCells3.Style.Font.Bold = true;
                        headerCells3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells3.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);

                        var allCells1 = workSheet2.Cells[workSheet2.Dimension.Address];
                        allCells1.AutoFilter = true;
                        allCells1.AutoFitColumns();

                        //GenerateBillingFileName
                        //string resultReplaceBillingRequestGroupCode = (getBillingRequestGroup.ClaimHeaderGroupTypeId == 4 ? ReplaceBillingRequestGroupCode(billingRequestGroupCode) : billingRequestGroupCode);
                        fileName = GenerateBillingFileName(billingRequestGroupCode);
                        // fileName = GenerateBillingFileName(billingRequestGroupCode);
                        //Combine local path file
                        var tempPathFile = Path.Combine(tempPath, fileName);

                        if (actionId == 1) // for SFTP
                        {
                            ////get file in local
                            //string[] files = Directory.GetFiles(tempPath);
                            //foreach (string file in files)
                            //{
                            //    //Delete all file in local
                            //    System.IO.File.Delete(file);
                            //}
                            //save excel file in local
                            FileInfo excelFile = new FileInfo(tempPathFile);
                            package.SaveAs(excelFile);
                        }
                        else if (actionId == 2) // for export excel
                        {
                            package.Save();
                            stream.Position = 0;
                            Session["DownloadExcel"] = package.GetAsByteArray();
                        }
                    }
                }

                if (actionId == 1) // for SFTP
                {
                    var billingGroupCodeIndex8 = billingRequestGroupCode.Substring(7, 1);
                    var checkBillingBankLine = (ClaimHeaderGroupCodeIndex3 == "H" && billingGroupCodeIndex8 == "H") || (ClaimHeaderGroupCodeIndex3 == "N" && billingGroupCodeIndex8 == "H") ? true : false;
                    return UploadFilesToSFTP(fileName, billingRequestGroupCode, checkBillingBankLine, tempPath, insuranceCompanyCode);
                }

                // update
                //var getgroup = _context.BillingRequestGroup.FirstOrDefault(f => f.BillingRequestGroupCode == billingRequestGroupCode.Trim());
                //getgroup.BillingRequestGroupCode = (getBillingRequestGroup.ClaimHeaderGroupTypeId == 4 ? ReplaceBillingRequestGroupCode(billingRequestGroupCode) : billingRequestGroupCode);
                //_context.BillingRequestGroup.AddOrUpdate(getgroup);
                //_context.SaveChanges();

                Log.Information("{_controllerName} - ExportExcelClaimPayBackBillingRequestGroup Successful", _controllerName);
                return Json(ResponseResult.Success<string>(data: fileName), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - ExportExcelClaimPayBackBillingRequestGroup Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        private async void BillingSummarySheet(ExcelPackage package, string billingRequestGroupCode, string claimHeaderGroupCodeIndex3, bool IsPH, bool IsPA30)
        {
            var result = _context.usp_BillingRequestGroup_Select(null, null, null, null, null, null, null, billingRequestGroupCode).AsQueryable();
            var resultBillingRequestGroup = result.FirstOrDefault();

            // todo issue number 1 new version
            var billingGroupCodeIndex8 = billingRequestGroupCode.Substring(7, 1);
            var checkBillingBankLine = (claimHeaderGroupCodeIndex3 == "H" && billingGroupCodeIndex8 == "H") || (claimHeaderGroupCodeIndex3 == "N" && billingGroupCodeIndex8 == "H") ? "BB1" : "BB2";
            var resultBillingBank = _context.BillingBank.Where(x => x.BillingBankCode == checkBillingBankLine).FirstOrDefault();

            string claimType = null;
            if (IsPA30 && IsPH)
            {
                claimType = "PA30";
            }
            else if (IsPH)
            {
                claimType = "สุขภาพ";
            }
            else
            {
                claimType = "อุบัติเหตุ";
            }

            //set header
            var workSheet1 = package.Workbook.Worksheets.Add("ใบสรุปการวางบิล");
            var headerCells1 = workSheet1.Cells["A1:L1"];
            headerCells1.Style.Font.Bold = true;
            headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#BFBFBF"));

            // set detail
            workSheet1.Cells["A1"].Value = "ลำดับ";
            workSheet1.Cells["A2"].Value = 1;
            workSheet1.Cells["B1"].Value = "บริษัท";
            workSheet1.Cells["B2"].Value = resultBillingRequestGroup.InsuranceCompanyName;
            workSheet1.Cells["C1"].Value = "ประเภทเคลม";
            workSheet1.Cells["C2"].Value = claimType;
            workSheet1.Cells["D1"].Value = "เลขที่ชุดวางบิล";
            workSheet1.Cells["D2"].Value = resultBillingRequestGroup.BillingRequestGroupCode;
            workSheet1.Cells["E1"].Value = "จำนวนราย";
            workSheet1.Cells["E2"].Value = resultBillingRequestGroup.ItemCount;
            workSheet1.Cells["F1"].Value = "จำนวนเงิน";
            workSheet1.Cells["F2"].Value = resultBillingRequestGroup.TotalAmount;
            workSheet1.Cells["G1"].Value = "ธนาคาร";
            workSheet1.Cells["G2"].Value = resultBillingBank.BankName;
            workSheet1.Cells["H1"].Value = "สาขา";
            workSheet1.Cells["H2"].Value = resultBillingBank.BankBranch;
            workSheet1.Cells["I1"].Value = "เลขที่บัญชี";
            workSheet1.Cells["I2"].Value = resultBillingBank.BankAccountNumber;
            workSheet1.Cells["J1"].Value = "ชื่อบัญชี";
            workSheet1.Cells["J2"].Value = resultBillingBank.BankAccountName;
            workSheet1.Cells["K1"].Value = "วันที่วางบิล";
            workSheet1.Cells["K2"].Value = resultBillingRequestGroup.BillingDate != null ? resultBillingRequestGroup.BillingDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "";
            workSheet1.Cells["L1"].Value = "วันครบกำหนด";
            workSheet1.Cells["L2"].Value = resultBillingRequestGroup.BillingDueDate != null ? resultBillingRequestGroup.BillingDueDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "";

            var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
            allCells1.AutoFitColumns();
        }

        private string ReplaceBillingRequestGroupCode(string originalString)
        {
            string result = "";
            int positionToReplace = 7;
            char newChar = 'H';

            if (positionToReplace >= 0 && positionToReplace < originalString.Length)
            {
                char[] chars = originalString.ToCharArray();
                chars[positionToReplace] = newChar;
                string modifiedString = new string(chars);

                result = modifiedString;
            }

            return result;
        }

        private string GenerateBillingFileName(string billingRequestGroupCode)
        {
            return $"{billingRequestGroupCode}-{DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture)}.xlsx";
        }

        public ActionResult ExportExcelClaimPayBackBillingRequestGroupDownload(string fileName)
        {
            try
            {
                Log.Debug("{_controllerName} Start ExportExcelClaimPayBackBillingRequestGroupDownload", _controllerName);

                if (Session["DownloadExcel"] != null)
                {
                    byte[] data = Session["DownloadExcel"] as byte[];
                    Log.Information("{_controllerName} - ExportExcelClaimPayBackBillingRequestGroupDownload Successful", _controllerName);
                    return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - ExportExcelClaimPayBackBillingRequestGroupDownload Error: {Message}", _controllerName, ex.Message);
                return null;
            }
        }

        public JsonResult UploadFilesToSFTP(string excelName = null, string billingRequestGroupCode = null, bool IsHospitalClaim = false, string tempPath = null, string insuranceCompanyCode = null)
        {
            try
            {
                Log.Debug("{_controllerName} Start UploadFilesToSFTP [excelName = {excelName}, billingRequestGroupCode = {billingRequestGroupCode}, IsHospitalClaim = {IsHospitalClaim}, tempPath = {tempPath}]", _controllerName, excelName, billingRequestGroupCode, IsHospitalClaim, tempPath);

                Log.Debug($"{_controllerName} - UploadFilesToSFTP Get SFTPConfig [SFTPConfigCode = {insuranceCompanyCode}]");
                var SFTPConfig = _context.SFTPConfig.Where(x => x.InsuranceCompanyCode.Equals(insuranceCompanyCode) && x.IsActive == true).FirstOrDefault();

                //update 2023-05-30 Chanadol
                //ถ้ามีบ.ประกันที่ไม่ Active ให้ออกทันที
                if (SFTPConfig == null)
                {
                    Log.Debug("{_controllerName} [UploadFilesToSFTP] - Get SFTPConfig not found", _controllerName, SFTPConfig);
                    //return Json(false, JsonRequestBehavior.AllowGet);
                    return Json(ResponseResult.Failure<string>("ไม่พบบริษัทประกันในระบบ"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Log.Debug("{_controllerName} [Get SFTPConfig Result] = {@SFTPConfig}", _controllerName, SFTPConfig);
                }

                //Preparations
                //Declare a variable and assigning the value
                var host = SFTPConfig.Host;
                var port = SFTPConfig.Port;
                var username = SFTPConfig.Username;
                var password = SFTPConfig.Password;
                var input = Properties.Settings.Default.Input;
                var output = Properties.Settings.Default.Output;

                //Connect to Sftp Server
                SftpClient sftp = new SftpClient(host: host, port: int.Parse(port), username: username, password: password);
                sftp.Connect();

                //Check connected sftp
                if (!sftp.IsConnected)
                {
                    Log.Warning("{_controllerName} [UploadFilesToSFTP] - Connected SFTP is fail", _controllerName);
                    return Json(ResponseResult.Failure<string>("Connected SFTP is fail"), JsonRequestBehavior.AllowGet);

                }

                string DirectoryResult1 = "1";
                string DirectoryResult2 = "2";
                string StrHospital = "Hospital";
                string StrBranch = "Branch";
                //string StrBackUp2 = Properties.Settings.Default.BackUpPath;
                //string StrOther = "Other";
                //string DirectoryInsurance = SFTPConfig.ShotInsuranceCompanyName;
                //string DirectoryInsuranceOutput = SFTPConfig == null ? $"{StrOther}/{output}" : SFTPConfig.PathOUT;
                string DirectoryInsuranceOutput = SFTPConfig.PathOUT;
                string DirectoryInsuranceOutput1 = $"{DirectoryInsuranceOutput}/{DirectoryResult1}";
                string DirectoryInsuranceOutput2 = $"{DirectoryInsuranceOutput}/{DirectoryResult2}";
                string DirectoryInsuranceInput = SFTPConfig.PathIN;
                string DirectoryInsuranceInputHospital = $"{DirectoryInsuranceInput}/{StrHospital}";
                string DirectoryInsuranceInputBranch = $"{DirectoryInsuranceInput}/{StrBranch}";
                string _fullDirectoryInputPath = "";

                #region Check Directory from current date
                //-----------------------------------------------สร้าง Directory สำหรับวางไฟล์ส่งบ.ประกัน--------------------------------------------------------
                _fullDirectoryInputPath = IsHospitalClaim ? DirectoryInsuranceInputHospital : DirectoryInsuranceInputBranch;
                //Check Insurance
                //if (!sftp.Exists(DirectoryInsurance))
                //{
                //    sftp.CreateDirectory(DirectoryInsurance);
                //}
                //Check Insurance/IN
                if (!sftp.Exists(DirectoryInsuranceInput))
                {
                    sftp.CreateDirectory(DirectoryInsuranceInput);
                }
                //Check Insurance/IN/Branch or Hospital
                if (!sftp.Exists(_fullDirectoryInputPath))
                {
                    sftp.CreateDirectory(_fullDirectoryInputPath);
                }

                //Check Insurance/OUT
                if (!sftp.Exists(DirectoryInsuranceOutput))
                {
                    sftp.CreateDirectory(DirectoryInsuranceOutput);
                }
                //Check Insurance/OUT/1
                if (!sftp.Exists(DirectoryInsuranceOutput1))
                {
                    sftp.CreateDirectory(DirectoryInsuranceOutput1);
                }
                //Check Insurance/OUT/2
                if (!sftp.Exists(DirectoryInsuranceOutput2))
                {
                    sftp.CreateDirectory(DirectoryInsuranceOutput2);
                }
                #endregion

                //Get all files from directory
                IEnumerable<string> files = Directory.GetFiles(tempPath, excelName);

                if (!files.Any())
                {
                    Log.Information("{_controllerName} [UploadFilesToSFTP] - file not found", _controllerName);
                    return Json(ResponseResult.Failure<string>("file not found"), JsonRequestBehavior.AllowGet);
                }

                foreach (var file in files)
                {
                    string fileName = Path.GetFileName(file);
                    //New FileStream
                    using (Stream fs = new FileStream(file, FileMode.Open))
                    {
                        //Upload to Sftp
                        Log.Information("{_controllerName} - [UploadFilesToSFTP] - Upload file to directory: [{_directory}]", _controllerName, _fullDirectoryInputPath);
                        sftp.UploadFile(fs, $"{_fullDirectoryInputPath}/{fileName}", (uploaded) =>
                        {
                            if (fs.Length == (long)uploaded)
                            {
                                Log.Information("{_controllerName} - [UploadFilesToSFTP] - Upload file success", _controllerName);
                            }
                            else
                            {
                                Log.Information("{_controllerName} - [UploadFilesToSFTP] - Upload file fail or number byte of file is not equal (file stream = {fs}, uploaded file lenth = {uploaded})", _controllerName, fs.Length, (long)uploaded);
                            }
                        });

                        //If uploaded when update status
                        ClaimPayBackEntities dataBase = new ClaimPayBackEntities();

                        var c = (from x in dataBase.BillingRequestGroup
                                 where x.BillingRequestGroupCode == billingRequestGroupCode
                                 select x).First();
                        c.BillingRequestGroupStatusId = 3;
                        dataBase.SaveChangesAsync();
                    }
                }

                //Disconnect
                sftp.Disconnect();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - [UploadFilesToSFTP] - UploadFiles Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(ex.Message), JsonRequestBehavior.AllowGet);
            }

            return Json(ResponseResult.Success<string>("Export To SFTP สำเร็จ"), JsonRequestBehavior.AllowGet);
        }

        #endregion Module Export ไฟล์นำส่ง บ.ประกัน

        #region "Module ImportClaimHeaderGroup"

        public JsonResult GetClaimPayBackImportBOHO(string ClaimHeaderGroupImportId = null,
            int? draw = null,
            string billingDateFrom = null,
            string billingDateTo = null,
            int? statusId = null,
            int? indexStart = null,
            int? pageSize = null,
            string sortField = null,
            string orderType = null,
            string searchDetail = null)
        {
            Log.Debug("{_controllerName} Start GetClaimPayBackImportBOHO ", _controllerName);

            DateTime cBillingDateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(billingDateFrom));
            DateTime cBillingDateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(billingDateTo));

            Log.Debug($"{_controllerName} - GetClaimPayBackImportBOHO Call Store procedure: usp_ClaimHeaderGroupImport_Select [cBillingDateFrom = {cBillingDateFrom}, cBillingDateTo = {cBillingDateTo}, indexStart = {indexStart}, pageSize = {pageSize},sortField = {sortField}, orderType = {orderType}, searchDetail = {searchDetail} ]");

            var result = _context.usp_ClaimHeaderGroupImport_Select(cBillingDateFrom,
                                                    cBillingDateTo,
                                                    statusId,
                                                    indexStart,
                                                    pageSize,
                                                    sortField,
                                                    orderType,
                                                    searchDetail.Trim()).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            Log.Information("{_controllerName} - GetClaimPayBackImportBOHO Successful", _controllerName);
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ImportClaimHeaderGroupFile(HttpPostedFileBase file, int? claimHeaderGroupType = null, string billingDate = null)
        {
            try
            {
                Log.Debug("{_controllerName} Start ImportClaimHeaderGroupFile [file = {file}, claimHeaderGroupType={claimHeaderGroupType}, billingDate={billingDate}]", _controllerName, file, claimHeaderGroupType, billingDate);

                var lstValidate = new object[4];

                var tmCode = new ObjectParameter("Result", typeof(string));

                DateTime cBillingDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(billingDate));

                using (var db = new ClaimPayBackEntities())
                {
                    //Gen Code Tmp
                    Log.Debug($"{_controllerName} - ImportClaimHeaderGroupFile Call Store procedure: usp_GenerateCode [transactionCodeControlTypeDetail = {"IMCHG"}, runningLenght = {6}, result = {tmCode} ]");
                    db.usp_GenerateCode("IMCHG", 6, tmCode);
                }

                var tmp_Code = tmCode.Value.ToString();

                var lst = GetExcelImportClaimHeaderGroup(file, claimHeaderGroupType, cBillingDate, tmp_Code);
                if (lst.Item2 != "")
                {
                    lstValidate[0] = false;
                    lstValidate[1] = "0";
                    lstValidate[2] = lst.Item2;
                    lstValidate[3] = "";
                    return Json(lstValidate, JsonRequestBehavior.AllowGet);
                }

                //if (claimHeaderGroupType == 4)
                //{
                //    foreach (var item in lst.Item2)
                //    {
                //        item.
                //        _context.asdasd.where(w => w.asdfasd == item)
                //    }
                //}

                Log.Debug($"{_controllerName} - Insert TmpClaimHeaderGroupImport Start");

                using (SqlConnection connection = new SqlConnection(_context.Database.Connection.ConnectionString))
                {
                    connection.Open();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {

                        bulkCopy.DestinationTableName = "TmpClaimHeaderGroupImport"; // Replace with your table name
                        System.Data.DataTable table = ConvertToDataTable(lst.Item1);

                        bulkCopy.ColumnMappings.Add("TmpCode", "TmpCode");
                        bulkCopy.ColumnMappings.Add("ClaimHeaderGroupCode", "ClaimHeaderGroupCode");
                        bulkCopy.ColumnMappings.Add("ItemCount", "ItemCount");
                        bulkCopy.ColumnMappings.Add("TotalAmount", "TotalAmount");
                        bulkCopy.ColumnMappings.Add("BillingDate", "BillingDate");
                        bulkCopy.ColumnMappings.Add("IsValid", "IsValid");
                        bulkCopy.ColumnMappings.Add("ValidateResult", "ValidateResult");
                        bulkCopy.ColumnMappings.Add("ClaimHeaderGroupTypeId", "ClaimHeaderGroupTypeId");
                        bulkCopy.ColumnMappings.Add("ClaimTypeCode", "ClaimTypeCode");
                        bulkCopy.WriteToServer(table);
                        Log.Debug($"{_controllerName} - Insert TmpClaimHeaderGroupImport Success");
                    }
                }

                //Validate After Insert Tmp Success
                Log.Debug($"{_controllerName} - ImportClaimHeaderGroupFile Call Store procedure: usp_TmpClaimHeaderGroupImport_Validate_V2 [tmp_Code = {tmp_Code}]");
                //Insert Validate
                _context.Database.CommandTimeout = 60;
                var rs_validate = await Task.FromResult(_context.usp_TmpClaimHeaderGroupImport_Validate_V2(tmp_Code).FirstOrDefault());
                lstValidate[0] = rs_validate.IsResult;
                lstValidate[1] = rs_validate.Result;
                lstValidate[2] = rs_validate.Msg;
                lstValidate[3] = tmp_Code;
                Log.Debug("{_controllerName} - ImportClaimHeaderGroupFile [Result]: {@rs_validate}", _controllerName, rs_validate);

                Log.Information("{_controllerName} - ImportClaimHeaderGroupFile Successful", _controllerName);
                return Json(lstValidate, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - ImportClaimHeaderGroupFile Error: {Message}", _controllerName, ex.Message);
                var lstValidate = new object[4];
                lstValidate[0] = 1;
                lstValidate[1] = "3";
                lstValidate[2] = ex.Message;
                return Json(lstValidate, JsonRequestBehavior.AllowGet);
            }
        }

        private System.Data.DataTable ConvertToDataTable(List<TmpClaimHeaderGroupImport> data)
        {
            try
            {
                Log.Debug("{_controllerName} Start ConvertToDataTable ", _controllerName);

                System.Data.DataTable table = new System.Data.DataTable();
                table.Columns.Add("TmpCode", typeof(string));
                table.Columns.Add("ClaimHeaderGroupCode", typeof(string));
                table.Columns.Add("ItemCount", typeof(int));
                table.Columns.Add("TotalAmount", typeof(decimal));
                table.Columns.Add("BillingDate", typeof(DateTime));
                table.Columns.Add("IsValid", typeof(bool));
                table.Columns.Add("ValidateResult", typeof(string));
                table.Columns.Add("ClaimHeaderGroupTypeId", typeof(int));
                table.Columns.Add("ClaimTypeCode", typeof(string));

                foreach (var item in data)
                {
                    System.Data.DataRow row = table.NewRow();
                    table.Rows.Add(item.TmpCode
                                    , item.ClaimHeaderGroupCode
                                    , item.ItemCount
                                    , item.TotalAmount
                                    , item.BillingDate
                                    , item.IsValid
                                    , item.ValidateResult
                                    , item.ClaimHeaderGroupTypeId
                                    , item.ClaimTypeCode
                                  );
                }

                Log.Debug("{_controllerName} ConvertToDataTable Success ", _controllerName);
                return table;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - ConvertToDataTable Error: {Message}", _controllerName, ex.Message);
                return null;
            }
        }

        public Tuple<List<TmpClaimHeaderGroupImport>, string> GetExcelImportClaimHeaderGroup(HttpPostedFileBase file, int? claimHeaderGroupType, DateTime billingDate, string tmp_Code)
        {
            Log.Debug("{_controllerName} Start GetExcelImportClaimHeaderGroup ", _controllerName);

            List<TmpClaimHeaderGroupImport> claimHeaderGroupList = new List<TmpClaimHeaderGroupImport>();

            using (var package = new ExcelPackage(file.InputStream))
            {
                var cs = package.Workbook.Worksheets;
                var ws = cs.First();
                var nr = ws.Dimension.End.Row;

                try
                {
                    for (var ri = 2; ri <= nr; ri++)
                    {
                        if (ws.Cells[ri, 1].Value == null)
                        {
                            ws.DeleteRow(nr);
                        }
                        else
                        {
                            var claimHeaderGroupCode = ws.Cells[ri, 1]?.Value?.ToString().Trim() ?? "";
                            var ItemCount = ws.Cells[ri, 2].Value ?? 0;
                            //ถ้าไม่สามารถ Convert เป็น int ได้ ให้แจ้งตรวจสอบไฟล์
                            if (!Int32.TryParse(ItemCount.ToString(), out int itemCount))
                            {
                                return System.Tuple.Create(claimHeaderGroupList, "กรุณาตวรจสอบคอลัมน์จำนวนราย");
                            }
                            var Total_Amount = ws.Cells[ri, 3].Value ?? 0;
                            //ถ้าไม่สามารถ Convert เป็น Decimal ได้ ให้แจ้งตรวจสอบไฟล์
                            if (!Decimal.TryParse(Total_Amount.ToString(), out decimal amount))
                            {
                                return System.Tuple.Create(claimHeaderGroupList, "กรุณาตรวจสอบคอลัมน์จำนวนเงิน");
                            }

                            TmpClaimHeaderGroupImport item = new TmpClaimHeaderGroupImport();
                            item.TmpCode = tmp_Code;
                            item.ClaimHeaderGroupCode = claimHeaderGroupCode;
                            item.ItemCount = Convert.ToInt32(ItemCount);
                            item.TotalAmount = Convert.ToDecimal(Total_Amount);
                            item.BillingDate = billingDate;
                            item.IsValid = false;
                            item.ValidateResult = "wait validate";
                            //item.InsuranceCompanyId = null;
                            item.ClaimHeaderGroupTypeId = claimHeaderGroupType;
                            item.ClaimTypeCode = "";
                            claimHeaderGroupList.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "{_controllerName} - GetExcelImportClaimHeaderGroup Error: {Message}", _controllerName, ex.Message);
                    return System.Tuple.Create(claimHeaderGroupList, ex.Message);
                }
            }

            Log.Information("{_controllerName} - GetExcelImportClaimHeaderGroup Successful", _controllerName);
            return System.Tuple.Create(claimHeaderGroupList, "");
        }

        public JsonResult GetTmpClaimHeaderGroupImportOverView(string tmpCode,
            int? draw = null,
            int? pageSize = null,
            int? pageStart = null,
            string sortField = null,
            string orderType = null,
            string search = null)
        {
            Log.Debug("{_controllerName} Start GetTmpClaimHeaderGroupImportOverView ", _controllerName);

            Log.Debug($"{_controllerName} - GetTmpClaimHeaderGroupImportOverView Call Store procedure: usp_TmpClaimHeaderGroupImport_Preview [tmp_Code = {tmpCode}, pageStart = {pageStart}, pageSize = {pageSize}, sortField = {sortField}, orderType = {orderType}, search = {search} ]");

            //var result = _context.usp_TmpClaimHeaderGroupImportDetail_Select(tmpCode, null, pageStart, pageSize, sortField, orderType, search).ToList();

            var result = _context.usp_TmpClaimHeaderGroupImport_Preview(tmpCode, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            Log.Information("{_controllerName} - GetTmpClaimHeaderGroupImportOverView Successful", _controllerName);
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GenarateGroup(string billingDateTo = null)
        {
            try
            {
                /*
                 * get current token from cookie
                 * check token expiry
                 */

                var nameToken = Properties.Settings.Default.TokenName;
                var token = Request.Cookies[nameToken].Value;
                bool checkExpiryToken = OAuth2Helper.ValidateTokenExpired(token, -10);
                if (checkExpiryToken)
                {
                    OAuth2Helper.ClearCookies();
                    return Json(new
                    {
                        IsResult = false,
                        IsReload = true,
                        Msg = "Token หมดอายุกรุณาลองใหม่อีกครั้ง"
                    }, JsonRequestBehavior.AllowGet);
                }

                Log.Debug("{_controllerName} Start GenarateGroup ", _controllerName);

                var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

                DateTime cBillingDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(billingDateTo));

                Log.Debug($"{_controllerName} - GenarateGroup Call Store procedure: usp_BillingRequest_Insert [userID = {userID}, biilingDate = {cBillingDate} ]");
                var result = _context.usp_BillingRequest_Insert(userID, cBillingDate).FirstOrDefault();
                Log.Debug("{_controllerName} - GenarateGroup [Result]: {@result}", _controllerName, result);

                Log.Information("{_controllerName} - GenarateGroup Successful", _controllerName);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - GenarateGroup Error: {Message}", _controllerName, ex.Message);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult InsertDocumentLink()
        {
            Log.Debug("{_controllerName} Start InsertDocumentLink ", _controllerName);
            try
            {

                Log.Debug($"{_controllerName} - InsertDocumentLink Call Store procedure: usp_S3UploaderConfig_Select [ParameterName = {null}]");
                var S3Config = _context.usp_S3UploaderConfig_Select(null).Where(x =>
                    x.ParameterName == "S3AccesskeyRead" ||
                    x.ParameterName == "S3SecretkeyRead" ||
                    x.ParameterName == "S3RegionName" ||
                    x.ParameterName == "ClaimFileForInsuranceReadTimeout"
                ).ToList();


                var S3_Acc = S3Config.Where(x => x.ParameterName == "S3AccesskeyRead").Select(y => y.ValueString).FirstOrDefault();
                var S3_Secr = S3Config.Where(x => x.ParameterName == "S3SecretkeyRead").Select(y => y.ValueString).FirstOrDefault();
                var S3_RegString = S3Config.Where(x => x.ParameterName == "S3RegionName").Select(y => y.ValueString).FirstOrDefault();
                var dayleftNum = Convert.ToInt32(S3Config.Where(x => x.ParameterName == "ClaimFileForInsuranceReadTimeout").Select(y => y.ValueNumber).FirstOrDefault());

                if (S3_RegString == "Asia Pacific (Singapore)")
                {
                    S3_RegString = "ap-southeast-1";
                }

                var S3Reg = Amazon.RegionEndpoint.GetBySystemName(S3_RegString);

                AmazonS3Config AWSS3Config = new AmazonS3Config
                {
                    RegionEndpoint = S3Reg
                };

                AmazonS3Client AWSS3Client = new AmazonS3Client(S3_Acc, S3_Secr, AWSS3Config);
                DateTime CurrentDate = DateTime.Parse(DateTime.Now.ToShortDateString());

                var query = from be in _context.BillingExport
                            join bg in _context.BillingRequestGroup
                            on be.BillingRequestGroupCode equals bg.BillingRequestGroupCode
                            where bg.BillingRequestGroupStatusId == 2 && be.CreatedDate >= CurrentDate && string.IsNullOrEmpty(be.DocumentLink)
                            group bg by bg.BillingRequestGroupId into grouped
                            select grouped.Key;

                var billingRequestGroupIds = query.ToList();

                //ส่ง BillingRequestId เป็นแบบ 1,2,3,4,5 ไป Get ข้อมูลที่ store usp_GetS3ByBillingRequestGroupId_Select(update 2023-09-29)
                string billingRequestGroupListId = string.Join(",", billingRequestGroupIds);

                Log.Debug($"{_controllerName} - InsertDocumentLink Call Store procedure: usp_GetS3ByBillingRequestGroupId_Select [billingRequestGroupId = {null}, billingRequestGroupListId = {billingRequestGroupListId}, IsCheck = {2}]");

                //usp_GetS3ByBillingRequestGroupId_Select(Id 1 ตัว, Id หลายตัว ,2 คือตัวเช็คว่าเป็นข้อมูลที่ใช้ Update BillingExport)
                var lstDocument = _context.usp_GetS3ByBillingRequestGroupId_Select(null, billingRequestGroupListId, 2).ToList();

                Log.Debug("{_controllerName} Start GetLink And AddLink To DocumentLinkPAModel", _controllerName);
                if (lstDocument.Count > 0)
                {
                    string docURL = "";

                    // get current token from cookie
                    var nameToken = Properties.Settings.Default.TokenName;
                    var token = Request.Cookies[nameToken].Value;
                    string endpoint = Properties.Settings.Default.EndpointGenerateShortUrl;

                    // Restsharp client
                    var client = new RestClient(endpoint)
                    {
                        Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(token, "Bearer")
                    };


                    foreach (var item in lstDocument)
                    {
                        docURL = "";
                        var c = lstDocument.Where(x => x.ClaimCode == item.ClaimCode).ToList();

                        var shortLinks = new List<string>();

                        for (int i = 0; i < c.Count; i++)
                        {
                            DateTime readFileTimeoutDT = DateTime.Now.AddDays(dayleftNum);

                            //เปลี่ยน docURL += เป็น docURL =
                            docURL = AWSS3Client.GetPreSignedURL(new Amazon.S3.Model.GetPreSignedUrlRequest
                            {
                                BucketName = c[i].S3Bucket,
                                Key = c[i].S3Key,
                                Expires = readFileTimeoutDT
                            });

                            shortLinks.Add(GenerateShortUrl(docURL, readFileTimeoutDT, client));
                            //docURL += " , ";
                        }

                        var billingExportToUpdate = _context.BillingExport.FirstOrDefault(obj => obj.ClaimCode == item.ClaimCode);
                        var shortLinkComma = String.Join(",", shortLinks);
                        if (billingExportToUpdate != null)
                        {
                            // แก้ไขค่า DocumentLink ของ BillingExport
                            //billingExportToUpdate.DocumentLink = docURL.Substring(0, docURL.Length - 3);
                            billingExportToUpdate.DocumentLink = shortLinkComma;

                            // บันทึกการเปลี่ยนแปลงลงในฐานข้อมูล
                            _context.SaveChanges();
                        }
                    }
                    Log.Debug("{_controllerName} GetLink And UpdateLink To DocumentLink In BillingExport Successful ", _controllerName);
                }
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - InsertDocumentLink Error: {Message}", _controllerName, ex.Message);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        private string GenerateShortUrl(string url, DateTime expiredDate, RestClient client)
        {
            try
            {
                Log.Debug("{_controllerName} Function GenerateShortUrl Start with parameter url = {url} and expiredDate = {expiredDate}", _controllerName, url, expiredDate);

                // generateShortLink request body
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json; charset=utf-8");
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(new GenerateShortLinkRequestDto
                {
                    url = url,
                    projectId = 35,
                    expiredDate = expiredDate,
                    subtypeId = 0,
                    usePassword = true
                });

                // Get response and deserialize to Token object
                Log.Debug("{_controllerName} Function GenerateShortUrl Execute {RequestBody} to {endpoint}", _controllerName, request.Parameters.FirstOrDefault(p => p.Type == ParameterType.RequestBody), client.BaseUrl);
                var response = client.Execute(request);

                if (!response.IsSuccessful)
                {
                    Log.Error("{_controllerName} Function GenerateShortUrl is not success. Error : {StatusDescription}", _controllerName, response.StatusDescription);
                    return null;
                }

                var json = JsonConvert.DeserializeObject<GenerateShortLinkResponseDto>(response.Content);
                if (!json.isSuccess)
                {
                    Log.Error("{_controllerName} Function GenerateShortUrl is not success. response Content : {json}", _controllerName, response.Content);
                    return null;
                }

                Log.Debug("{_controllerName} Function GenerateShortUrl is success. shortUrl is {shortUrl}", _controllerName, json.data.shortUrl);

                return json.data.shortUrl;
            }
            catch (Exception error)
            {

                var msg = error.InnerException?.InnerException?.Message ?? error.InnerException?.Message ?? error.Message;

                Log.Error("{_controllerName} Function GenerateShortUrl is error {msg}", _controllerName, msg);

                return null;
            }
        }

        public async Task<JsonResult> ImportClaimHeaderGroupConfirm(HttpPostedFileBase file, string tmpCode = null)
        {
            try
            {
                Log.Debug("{_controllerName} Start ImportClaimHeaderGroupConfirm [file = {file}, tmpCode={tmpCode}]", _controllerName, file, tmpCode);

                var lstArr = new string[3];
                var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

                Log.Debug($"{_controllerName} - ImportClaimHeaderGroupConfirm Call Store procedure: usp_ClaimHeaderGroupImport_Insert [tmpCode = {tmpCode}, fileName = {file.FileName} createdByUser = {userID}]");
                _context.Database.CommandTimeout = 60;
                var result = await Task.FromResult(_context.usp_ClaimHeaderGroupImport_Insert(tmpCode, file.FileName, userID).FirstOrDefault());
                lstArr[0] = result.IsResult.Value.ToString();
                lstArr[1] = result.Result;
                lstArr[2] = result.Msg;

                Log.Debug("{_controllerName} - ImportClaimHeaderGroupConfirm [Result]: {@result}", _controllerName, result);

                Log.Information("{_controllerName} - ImportClaimHeaderGroupConfirm Successful", _controllerName);
                return Json(lstArr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - ImportClaimHeaderGroupConfirm Error: {Message}", _controllerName, ex.Message);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetImportClaimHeaderGroupTmpCodeCount(string tmpCode)
        {
            var rs = _context.usp_TmpClaimHeaderGroupImportCount_Select(tmpCode).FirstOrDefault();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportClaimHeaderGroupImportValidateToExcel(string tmpID)
        {
            //await Task.Yield();
            var data_sheet1 = _context.usp_TmpClaimHeaderGroupImport_Preview(tmpID, null, 9999, null, null, null).Select(x => new
            {
                รายการ = x.ClaimHeaderGroupCode,
                จำนวนราย = x.ItemCount,
                จำนวนเงิน = x.TotalAmount,
                Error_Message = x.ValidateResult,
            }).ToList();

            var data_sheet2 = _context.usp_TmpClaimHeaderGroupImportDetail_Select(tmpID, null, null, 9999, null, null, null).Select(x => new
            {
                รายการ = x.ClaimHeaderGroupCode,
                เลขที่เคลม = x.ClaimHeaderCode,
                Error_Message = x.ValidateDetailResult,
            }).ToList();

            //update 2023-08-08 chanadol
            //var data_sheet2 = _context.usp_CountDocumentClaimByClaimHeaderGroupCode_Preview(, null, 9999, null, null, null).Select(x => new 
            //{
            //    รายการ = x.ClaimHeaderGroupCode,
            //    เลขที่เคลม = x.ClaimHeaderCode,
            //    //จำนวนเงิน = x.TotalAmount,
            //    Error_Message = x.CountDoc != null ? "": "ไม่พบเอกสารแนบ",
            //}).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet1 = package.Workbook.Worksheets.Add("บ.ส Error");
                workSheet1.Cells.LoadFromCollection(data_sheet1, true);

                // Select only the header cells
                var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                // Set their text to bold.
                headerCells1.Style.Font.Bold = true;
                // Set their background color
                headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGoldenrodYellow);
                // Apply the auto-filter to all the columns
                var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                allCells1.AutoFilter = true;
                // Auto-fit all the columns
                allCells1.AutoFitColumns();

                var workSheet2 = package.Workbook.Worksheets.Add("รายละเอียด Error");
                workSheet2.Cells.LoadFromCollection(data_sheet2, true);
                var headerCells2 = workSheet2.Cells[1, 1, 1, workSheet2.Dimension.Columns];
                headerCells2.Style.Font.Bold = true;
                headerCells2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerCells2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGoldenrodYellow);
                var allCells2 = workSheet2.Cells[workSheet2.Dimension.Address];
                allCells2.AutoFilter = true;
                // Auto-fit all the columns
                allCells2.AutoFitColumns();


                package.Save();

                stream.Position = 0;
                Session["DownloadExcel_FileClaimHeaderGroupImportValidateReport"] = package.GetAsByteArray();
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadClaimHeaderGroupImportValidate()
        {
            if (Session["DownloadExcel_FileClaimHeaderGroupImportValidateReport"] != null)
            {
                byte[] data = Session["DownloadExcel_FileClaimHeaderGroupImportValidateReport"] as byte[];
                string excelName = $"รายงานตรวจสอบไฟล์นำเข้า-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public JsonResult GetClaimHeaderGroupImportOverViewDetail(int? TmpClaimHeaderGroupImportId,
            int? draw = null,
            int? pageSize = null,
            int? pageStart = null,
            string sortField = null,
            string orderType = null,
            string search = null)
        {
            Log.Debug("{_controllerName} Start GetClaimHeaderGroupImportOverViewDetail ", _controllerName);

            Log.Debug($"{_controllerName} - GetClaimHeaderGroupImportOverViewDetail Call Store procedure: usp_TmpClaimHeaderGroupImportErrorMessage_Select [claimHeaderGroupCode = {TmpClaimHeaderGroupImportId}, pageStart = {pageStart}, pageSize = {pageSize}, sortField = {sortField}, orderType = {orderType}, search = {search} ]");

            var result = _context.usp_TmpClaimHeaderGroupImportDetail_Select(null, TmpClaimHeaderGroupImportId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            Log.Information("{_controllerName} - GetClaimHeaderGroupImportOverViewDetail Successful", _controllerName);
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion "Module ImportClaimHeaderGroup"

        #region "บันทึกยอดเงิน"

        //Get Table History
        public JsonResult GetClaimPayBackSaveBalanceHistory(int? claimHeaderGroupImportDetailId = null,
            int? draw = null,
            int? indexStart = null,
            int? pageSize = null,
            string sortField = null,
            string orderType = null,
            string searchDetail = null)
        {

            var result = _context.usp_BillingRequestResultDetailLog_Select(claimHeaderGroupImportDetailId,
                                                    indexStart,
                                                    pageSize,
                                                    sortField,
                                                    orderType,
                                                    searchDetail.Trim()).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        //Get Table ClaimHeaderGroup
        public ActionResult GetClaimHeaderGroupImportDetail(int SearchTypeId, string SearchDetail, int? draw)
        {
            var result = _context.usp_ClaimHeaderGroupImportDetailSearch_Select(SearchTypeId, SearchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }


        //Get Detail Claim
        public ActionResult GetClaimDetailSelect(int ClaimHeaderCode)
        {
            var result = _context.usp_GetClaimHeaderGroupImportDetailById_Select(ClaimHeaderCode).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Save Amount 
        public ActionResult SaveClaimPayBackAmountBalance(int ClaimHeaderGroupImportDetailId,
                                                          string coverAmount,
                                                          int IsCheckManual)
        {
            try
            {
                Log.Debug("{_controllerName} Start SaveClaimPayBackAmountBalance ", _controllerName);
                Decimal d_CoverAmount = Convert.ToDecimal(coverAmount);
                var userID = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;

                Log.Debug($"{_controllerName} - GenarateGroup Call Store procedure: usp_BillingRequestResultManual_Insert [claimHeaderGroupImportDetailId = {ClaimHeaderGroupImportDetailId}, coverAmount = {d_CoverAmount}, isCheckManual = {IsCheckManual}, createdByUserId = {userID} ]");
                var result = _context.usp_BillingRequestResultManual_Insert(ClaimHeaderGroupImportDetailId,
                                                                            d_CoverAmount,
                                                                            IsCheckManual,
                                                                            userID).FirstOrDefault();
                Log.Debug("{_controllerName} - SaveClaimPayBackAmountBalance [Result]: {@result}", _controllerName, result);
                Log.Information("{_controllerName} - SaveClaimPayBackAmountBalance Successful", _controllerName);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - SaveClaimPayBackAmountBalance Error: {Message}", _controllerName, ex.Message);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ImportClaimPayBackSaveBalanceFile(HttpPostedFileBase file)
        {
            Log.Debug("{_controllerName} Start ImportClaimPayBackSaveBalanceFile ", _controllerName);

            var lstDetailArr = new string[3];
            var lstValidate = new string[4];

            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var lst = GetExcelImportSaveBalance(file);

            if (lst == null)
            {
                lstValidate[0] = "False";
                lstValidate[1] = "0";
                lstValidate[2] = "ข้อมูลไม่ถูกต้องกรุณาตรวจสอบไฟล์";
                lstValidate[3] = "";
                return Json(lstValidate, JsonRequestBehavior.AllowGet);
            }

            var lst_ClaimHeaderGroup = lst.Item1;

            Log.Debug($"{_controllerName} - ImportClaimPayBackSaveBalanceFile Call Store procedure: usp_TmpBillingRequestResultManualImportHeader_Insert [userID = {userID}]");

            var resualt = _context.usp_TmpBillingRequestResultManualImportHeader_Insert(userID).FirstOrDefault();
            var HeaderId = Convert.ToInt32(resualt.Result);

            //Loop Insert Data
            foreach (var item in lst_ClaimHeaderGroup)
            {
                try
                {

                    Log.Debug($"{_controllerName} - ImportClaimPayBackSaveBalanceFile Call Store procedure: usp_TmpBillingRequestResultManualImportDetail_Insert [HeaderId = {HeaderId}, ClaimHeaderGroupCode = {item.ClaimHeaderGroupCode}, ClaimHeader = {item.ClaimHeader}, Total_Amout = {item.Total_Amount}, Cover_Amount = {item.Cover_Amount}]");

                    var rs = _context.usp_TmpBillingRequestResultManualImportDetail_Insert(HeaderId,
                                                                                            item.ClaimHeaderGroupCode,
                                                                                            item.ClaimHeader,
                                                                                            Decimal.Parse(item.Total_Amount),
                                                                                            Decimal.Parse(item.Cover_Amount)).FirstOrDefault();

                    string messageDetail = rs.Msg.ToString();
                    lstDetailArr[0] = rs.IsResult.Value.ToString();
                    lstDetailArr[1] = rs.Result.ToString();
                    lstDetailArr[2] = messageDetail;

                    Log.Debug("{_controllerName} - ImportClaimPayBackSaveBalanceFile [Result]: {@rs}", _controllerName, rs);

                    if (lstDetailArr[0] == "False")
                    {
                        Log.Warning(lstDetailArr[0], "{_controllerName} - ImportClaimPayBackSaveBalanceFile Error: {Message}", _controllerName, lstDetailArr[2]);
                        return Json(lstDetailArr, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "{_controllerName} - ImportClaimPayBackSaveBalanceFile Error: {Message}", _controllerName, ex.Message);
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }

            Log.Debug($"{_controllerName} - ImportClaimHeaderGroupFile Call Store procedure: usp_TmpClaimHeaderGroupImport_Validate_V2 [tmp_Code = {HeaderId}]");
            //Insert Validate
            var rs_validate = _context.usp_TmpBillingRequestResultManual_Validate(HeaderId).FirstOrDefault();
            lstValidate[0] = rs_validate.IsResult.Value.ToString();
            lstValidate[1] = rs_validate.Result.ToString();
            lstValidate[2] = rs_validate.Msg.ToString();
            lstValidate[3] = HeaderId.ToString();

            Log.Debug("{_controllerName} - ImportClaimPayBackSaveBalanceFile [Result]: {@rs_validate}", _controllerName, rs_validate);

            Log.Information("{_controllerName} - ImportClaimPayBackSaveBalanceFile Successful", _controllerName);
            return Json(lstValidate, JsonRequestBehavior.AllowGet);
        }

        public Tuple<List<ImportClaimPayBackSaveBalanceRequestModel>> GetExcelImportSaveBalance(HttpPostedFileBase file)
        {
            Log.Debug("{_controllerName} Start GetExcelImportSaveBalance ", _controllerName);
            List<ImportClaimPayBackSaveBalanceRequestModel> claimHeaderGroupList = new List<ImportClaimPayBackSaveBalanceRequestModel>();

            using (var package = new ExcelPackage(file.InputStream))
            {
                var cs = package.Workbook.Worksheets;
                var ws = cs.First();
                var nr = ws.Dimension.End.Row;

                try
                {
                    for (var ri = 2; ri <= nr; ri++)
                    {
                        if (ws.Cells[ri, 1].Value == null)
                        {
                            ws.DeleteRow(nr);
                        }
                        else
                        {
                            var ClaimHeaderGroupCode = ws.Cells[ri, 1].Value;
                            string claimHeaderGroup_str = "";
                            if (ClaimHeaderGroupCode != null) { claimHeaderGroup_str = ws.Cells[ri, 1].Value.ToString(); }
                            var ClaimHeader = ws.Cells[ri, 2].Value;
                            string claimHeader_str = "";
                            if (ClaimHeader != null) { claimHeader_str = ws.Cells[ri, 2].Value.ToString(); }
                            var Total_Amount = ws.Cells[ri, 3].Value;
                            string totalAmount_str = "";
                            if (Total_Amount != null) { totalAmount_str = ws.Cells[ri, 3].Value.ToString(); }
                            var Cover_Amount = ws.Cells[ri, 4].Value;
                            string coverAmount_str = "";
                            if (Cover_Amount != null) { coverAmount_str = ws.Cells[ri, 4].Value.ToString(); }
                            //var BillingDate = ws.Cells[ri, 5].Value;
                            //string billingdate_str = "";
                            //if (BillingDate != null) { billingdate_str = ws.Cells[ri, 5].Value.ToString(); }

                            ImportClaimPayBackSaveBalanceRequestModel item = new ImportClaimPayBackSaveBalanceRequestModel();
                            item.ClaimHeaderGroupCode = claimHeaderGroup_str;
                            item.ClaimHeader = claimHeader_str;
                            item.Total_Amount = totalAmount_str;
                            item.Cover_Amount = coverAmount_str;
                            //item.BillingDate = billingdate_str;
                            claimHeaderGroupList.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "{_controllerName} - GetExcelImportSaveBalance Error: {Message}", _controllerName, ex.Message);
                    throw;
                }
            }

            Log.Information("{_controllerName} - GetExcelImportSaveBalance Successful", _controllerName);
            return System.Tuple.Create(claimHeaderGroupList);
        }

        public JsonResult GetTmpSaveBalanceImportOverView(int? headerId,
                                                                int? draw = null,
                                                                int? pageSize = null,
                                                                int? pageStart = null,
                                                                string sortField = null,
                                                                string orderType = null,
                                                                string search = null)
        {
            Log.Debug("{_controllerName} Start GetTmpSaveBalanceImportOverView ", _controllerName);

            Log.Debug($"{_controllerName} - GetTmpSaveBalanceImportOverView Call Store procedure: usp_TmpClaimHeaderGroupImport_Preview [headerId = {headerId}, pageStart = {pageStart}, pageSize = {pageSize}, sortField = {sortField}, orderType = {orderType}, search = {search} ]");

            var result = _context.usp_TmpBillingRequestResultManual_Preview(headerId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            Log.Information("{_controllerName} - GetTmpSaveBalanceImportOverView Successful", _controllerName);
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetImportSaveBalanceHeaderIdCount(int? headerId)
        {
            var rs = _context.usp_TmpBillingRequestResultManualCount_Select(headerId).FirstOrDefault();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ImportSaveBlanceConfirm(int? headerId)
        {
            try
            {
                Log.Debug("{_controllerName} Start ImportSaveBlanceConfirm [headerId = {headerId}, tmpCode={tmpCode}]  ", _controllerName, headerId);
                var lstArr = new string[3];
                var userID = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;

                Log.Debug($"{_controllerName} - ImportSaveBlanceConfirm Call Store procedure: usp_TmpBillingRequestResultManual_Insert [HeaderId = {headerId}, UserID = {userID}]");

                var result = _context.usp_TmpBillingRequestResultManual_Insert(headerId, userID).FirstOrDefault();
                lstArr[0] = result.IsResult.Value.ToString();
                lstArr[1] = result.Result;
                lstArr[2] = result.Msg;

                Log.Debug("{_controllerName} - ImportSaveBlanceConfirm [Result]: {@result}", _controllerName, result);

                Log.Information("{_controllerName} - ImportSaveBlanceConfirm Successful", _controllerName);
                return Json(lstArr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - ImportSaveBlanceConfirm Error: {Message}", _controllerName, ex.Message);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportSaveBalanceImportValidateToExcel(int? headerId)
        {
            //await Task.Yield();
            var data_sheet1 = _context.usp_TmpBillingRequestResultManual_Preview(headerId, null, 9999, null, null, null).Select(x => new
            {
                รายการ = x.ClaimHeaderGroupCode,
                CL = x.ClaimCode,
                จำนวนเงิน = x.Amount,
                ยอดบริษัทประกัน = x.CoverAmount,
                Error_Message = x.Result,
            }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet1 = package.Workbook.Worksheets.Add("Sheet1");
                workSheet1.Cells.LoadFromCollection(data_sheet1, true);

                // Select only the header cells
                var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                // Set their text to bold.
                headerCells1.Style.Font.Bold = true;
                // Set their background color
                headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGoldenrodYellow);
                // Apply the auto-filter to all the columns
                var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                allCells1.AutoFilter = true;
                // Auto-fit all the columns
                allCells1.AutoFitColumns();

                package.Save();

                stream.Position = 0;
                Session["DownloadExcel_FileSaveBalanceImportValidateReport"] = package.GetAsByteArray();
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadSaveBalanceImportValidate()
        {
            if (Session["DownloadExcel_FileSaveBalanceImportValidateReport"] != null)
            {
                byte[] data = Session["DownloadExcel_FileSaveBalanceImportValidateReport"] as byte[];
                string excelName = $"รายงานตรวจสอบไฟล์นำเข้า-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        #endregion

        #region Dashboard ผลอนุมัติเคลมจาก บ.ประกัน

        //update 2023-05-05 Chanadol
        [Authorization(Roles = "Developer,SmileClaimPayBack_Finance")]
        //[Authorization(Roles = "SmileClaimPayBack_Operation,SmileClaimPayBack_Finance,Developer")]
        public ActionResult DashboardClaimApproveResult()
        {
            ViewBag.ClaimHeaderGroupTypeId = _context.usp_ClaimHeaderGroupType_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.ClaimType = _context.usp_ClaimType_Select().ToList();
            ViewBag.InsuranceCompany = _context.usp_OrganizeInsuranceForGenerateGroup_Select(null, 0, 9999, null, null, null).ToList();
            return View();
        }

        public JsonResult GetDashboardClaimApproveResult(string billingDateFrom = null, string billingDateTo = null, string claimTypeId = null, int? claimHeaderGroupType = null, int? draw = null, int? indexStart = null, int? pageSize = null, string sortField = null, string orderType = null, string searchDetail = null, int? organizeId = null)
        {
            Log.Debug("{_controllerName} Start GetDashboardClaimApproveResult [billingDateFrom = {billingDateFrom}, billingDateTo = {billingDateTo}]", _controllerName, billingDateFrom, billingDateTo, claimTypeId, claimHeaderGroupType);
            DateTime d_BillingDateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(billingDateFrom));
            DateTime d_BillingDateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(billingDateTo));

            var _claimTypeId = claimTypeId == "-1" ? null : claimTypeId;
            var _claimHeaderGroupType = claimHeaderGroupType == -1 ? null : claimHeaderGroupType;
            var _sortField = sortField == "" ? null : sortField;
            var _organizeId = organizeId == -1 ? null : organizeId;

            Log.Debug($"{_controllerName} - GetDashboardClaimApproveResult Call Store procedure: usp_DashboardClaimApproveBillingCountAndAmount_Select [d_BillingDateFrom = {d_BillingDateFrom}, d_BillingDateTo = {d_BillingDateTo},claimTypeId = {_claimTypeId}, claimHeaderGroupType = {_claimHeaderGroupType}, sortField = {sortField},orderType = {orderType} ]");
            var list = _context.usp_DashboardClaimApproveBillingCountAndAmount_Select(d_BillingDateFrom, d_BillingDateTo, _claimTypeId, _claimHeaderGroupType, _organizeId, null, null, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.Count() : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.Count()  : list.Count()},
                {"data", list.ToList()}
            };

            Log.Information("{_controllerName} - GetDashboardClaimApproveResult Successful", _controllerName);
            return Json(dt, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetDashboardBillingAmountTotal_Detail(string billingDateFrom, string billingDateTo, string claimTypeId, int? claimHeaderGroupType, int? organizeId)
        {
            Log.Debug("{_controllerName} Start GetDashboardBillingAmountTotal_Detail [billingDateFrom = {billingDateFrom}, billingDateTo = {billingDateTo}, claimTypeId = {claimTypeId}, claimHeaderGroupType = {claimHeaderGroupType}]", _controllerName, billingDateFrom, billingDateTo, claimTypeId, claimHeaderGroupType);
            DateTime d_BillingDateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(billingDateFrom));
            DateTime d_BillingDateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(billingDateTo));
            var _claimType = claimTypeId == "-1" ? null : claimTypeId;
            int? _claimHeaderGroupType = claimHeaderGroupType == -1 ? null : claimHeaderGroupType;
            var _organizeId = organizeId == -1 ? null : organizeId;

            Log.Debug($"{_controllerName} - GetDashboardBillingAmountTotal_Detail Call Store procedure: usp_DashboardBillingAmountTotal_Detail_Select [d_BillingDateFrom = {d_BillingDateFrom}, d_BillingDateTo = {d_BillingDateTo}, claimTypeId = {_claimType}, claimHeaderGroupType = {_claimHeaderGroupType}]");
            var billingAmountList = _context.usp_DashboardBillingAmountTotal_Detail_Select(d_BillingDateFrom, d_BillingDateTo, _claimType, _claimHeaderGroupType, _organizeId).ToList();
            var paymentAmountList = _context.usp_DashboardPaymentAmountTotal_Detail_Select(d_BillingDateFrom, d_BillingDateTo, _claimType, _claimHeaderGroupType, _organizeId).ToList();
            var outstandingBalanceList = _context.usp_DashboardOutstandingBalanceTotal_Detail_Select(d_BillingDateFrom, d_BillingDateTo, _claimType, _claimHeaderGroupType, _organizeId).ToList();

            var model = new DashboardBillingModel();
            model.BillingAmountTotal = billingAmountList.Sum(x => x.TotalAmount);
            model.PaymentAmountTotal = paymentAmountList.Sum(x => x.TotalAmount);
            model.OutstandingBalanceTotal = outstandingBalanceList.Sum(x => x.TotalAmount);
            model.BillingAmountList = billingAmountList;
            model.PaymentAmountList = paymentAmountList;
            model.OutstandingBalanceList = outstandingBalanceList;

            Log.Information("{_controllerName} - GetDashboardBillingAmountTotal_Detail Successful", _controllerName);

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Dashboard ยอดคงค้าง บ. ประกัน
        //update 2023-05-05 Chanadol
        [Authorization(Roles = "Developer,SmileClaimPayBack_Finance")]
        //[Authorization(Roles = "SmileClaimPayBack_Operation,SmileClaimPayBack_Finance,Developer")]

        public ActionResult DashboardOutStandingBalance()
        {
            ViewBag.ClaimType = _context.usp_ClaimType_Select().ToList();
            ViewBag.InsuranceCompany = _context.usp_OrganizeInsuranceForGenerateGroup_Select(null, 0, 9999, null, null, null).ToList();
            return View();
        }


        public JsonResult GetDashboardOutstandingBalanceTotal(string claimTypeId, int? organizeId)

        {
            Log.Debug("{_controllerName} Start GetDashboardOutstandingBalanceResult_Detail [claimTypeId = {claimTypeId}]", _controllerName, claimTypeId);

            var _claimType = claimTypeId == "-1" ? null : claimTypeId;
            var _organizeId = organizeId == -1 ? null : organizeId;

            Log.Debug($"{_controllerName} - GetDashboardOutstandingBalanceResult_Detail Call Store procedure: usp_DashboardBillingAmountTotal_Detail_Select [claimTypeId = {_claimType}]");

            var result = _context.usp_DashboardOutstandingBalanceTotal_Select(_claimType, _organizeId).FirstOrDefault();


            //var AmountTotal = result.AmountTotal == null ? 0 : result.AmountTotal;
            //result.ConsideredAmountTotal = 0;
            //result.ConsiderAmountTotal = 0;

            Log.Information("{_controllerName} - GetDashboardOutstandingBalanceResult_Detail Successful", _controllerName);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetDashboardOutstandingBalanceInDueDateResult(string claimTypeId = null, int? draw = null, int? indexStart = null, int? pageSize = null, string sortField = null, string orderType = null, string searchDetail = null, int? organizeId = null)
        {
            Log.Debug("{_controllerName} Start GetDashboardOutstandingBalanceResult [claimTypeId = {claimTypeId}]", _controllerName, claimTypeId);

            var _claimType = claimTypeId == "-1" ? null : claimTypeId;
            var _sortField = sortField == "" ? null : sortField;
            var _organizeId = organizeId == -1 ? null : organizeId;


            Log.Debug($"{_controllerName} - GetDashboardOutstandingBalanceResult Call Store procedure: usp_DashboardBillingAmountTotal_Detail_Select [claimTypeId = {_claimType}, indexStart = {indexStart}, pageSize = {pageSize}, _sortField = {_sortField}, orderType = {orderType}, searchDetail = {searchDetail}]");

            var result = _context.usp_DashboardOutStandingDuedateAmount_Select(_claimType, indexStart, pageSize, _sortField, orderType, searchDetail, _organizeId).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.ToList().Count : result.ToList().Count()},
                {"recordsFiltered", result.Count() != 0 ? result.ToList().Count : result.ToList().Count()},
                {"data", result.ToList()}
            };

            Log.Information("{_controllerName} - GetDashboardOutstandingBalanceResult Successful", _controllerName);
            return Json(dt, JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetDashboardOutstandingBalanceOverDueDateResult(string claimTypeId = null, string dueDateLength = null, int? draw = null, int? indexStart = null, int? pageSize = null, string sortField = null, string orderType = null, string searchDetail = null, int? organizeId = null)
        {
            Log.Debug("{_controllerName} Start GetDashboardOutstandingBalanceOverDueDateResult [claimTypeId = {claimTypeId}]", _controllerName, claimTypeId);

            var _claimType = claimTypeId == "-1" ? null : claimTypeId;

            var _dueDateLength = dueDateLength == "-1" ? null : dueDateLength;

            var _sortField = sortField == "" ? null : sortField;
            var _organizeId = organizeId == -1 ? null : organizeId;

            Log.Debug($"{_controllerName} - GetDashboardOutstandingBalanceOverDueDateResult Call Store procedure: usp_DashboardOutStandingOverDuedateAmount_Select [claimTypeId = {_claimType}, indexStart = {indexStart}, pageSize = {pageSize}, _sortField = {_sortField}, orderType = {orderType}, searchDetail = {searchDetail}]");

            var result = _context.usp_DashboardOutStandingOverDuedateAmount_Select(_claimType, _dueDateLength, indexStart, pageSize, _sortField, orderType, searchDetail, _organizeId).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.ToList().Count : result.ToList().Count()},
                {"recordsFiltered", result.Count() != 0 ? result.ToList().Count : result.ToList().Count()},
                {"data", result.ToList()}
            };

            Log.Information("{_controllerName} - GetDashboardOutstandingBalanceOverDueDateResult Successful", _controllerName);
            return Json(dt, JsonRequestBehavior.AllowGet);

        }

        //------------------------------------------------- Export To Excel -------------------------------------------------//


        [HttpPost]
        public async Task<ActionResult> ExportOutStandingBalance(string claimTypeId = null, int? organizeId = null)
        {
            await Task.Yield();
            Session.Remove("ExportOutStandingBalanceDownload");

            var _claimType = claimTypeId == "-1" ? null : claimTypeId;
            var _organizeId = organizeId == -1 ? null : organizeId;
            string IsClaimType;

            if (_claimType == "1000")
            {
                IsClaimType = "เคลมโรงพยาบาล";
            }
            else if (_claimType == "2000")
            {
                IsClaimType = "เคลมลูกค้า";
            }
            else
            {
                IsClaimType = "ทั้งหมด";
            }

            using (var db = new ClaimPayBackEntities())
            {
                try
                {
                    var dt = db.usp_Report_OutStandingBalance_Select(_claimType, _organizeId).Select(x => new
                    {
                        บริษัทประกัน = x.InsuranceCompany,
                        ผลิตภัณฑ์ = x.Product, //ClaimHeaderGroupType
                        x.Datelength0,
                        x.Datelength16,
                        x.Datelength31,
                        x.Datelength61,
                        x.Datelength90,
                        รวมยอดคงค้าง = x.DatelengthTotal,

                    }).ToList();

                    if (dt.Count > 0)
                    {
                        var stream = new MemoryStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            var workSheet1 = package.Workbook.Worksheets.Add(IsClaimType);
                            workSheet1.Cells["A3"].LoadFromCollection(dt, true);

                            workSheet1.Cells["A1:B1"].Merge = true;
                            workSheet1.Cells["A1:B1"].Value = "รายงานยอดลูกหนี้คงค้าง";
                            workSheet1.Cells["A1:B1"].Style.Font.Bold = true;
                            workSheet1.Cells["G1:H1"].Merge = true;
                            workSheet1.Cells["G1:H1"].Value = $"ประจำวันที่  {DateTime.Now.ToString("dd/MM/yyyy")}";
                            workSheet1.Cells["G1:H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            workSheet1.Cells["A1:H1"].Style.Font.Bold = true;
                            workSheet1.Cells["C3"].Value = "0-15 วัน";
                            workSheet1.Cells["D3"].Value = "16-30 วัน";
                            workSheet1.Cells["E3"].Value = "31-60 วัน";
                            workSheet1.Cells["F3"].Value = "61-90 วัน";
                            workSheet1.Cells["G3"].Value = "มากกว่า 90 วัน";

                            var headerCells1 = workSheet1.Cells[3, 1, 3, workSheet1.Dimension.Columns];
                            headerCells1.Style.Font.Bold = true;
                            headerCells1.Style.Font.Color.SetColor(System.Drawing.Color.White);
                            headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#4471C4"));

                            var allCells1 = workSheet1.Cells["A3:H3"];
                            var allCells2 = workSheet1.Cells[workSheet1.Dimension.Address];
                            ExcelRange dataRange = workSheet1.Cells[workSheet1.Dimension.Address];
                            int lastRow = dataRange.End.Row;

                            workSheet1.Cells[$"A{lastRow}:B{lastRow}"].Merge = true;
                            workSheet1.Cells[$"A{lastRow}:B{lastRow}"].Value = "รวมยอดคงค้าง";
                            workSheet1.Cells[$"A{lastRow}:B{lastRow}"].Style.Font.Bold = true;
                            workSheet1.Cells[$"A{lastRow}:B{lastRow}"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                            workSheet1.Cells[$"A{lastRow}:B{lastRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet1.Cells[$"A{lastRow}:B{lastRow}"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#4471C4"));
                            workSheet1.Cells[$"A{lastRow}:B{lastRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet1.Cells[$"A{lastRow}:B{lastRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            workSheet1.Cells[$"C{lastRow}:H{lastRow}"].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                            workSheet1.Cells[$"C{lastRow}:H{lastRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet1.Cells[$"C{lastRow}:H{lastRow}"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#fce4d6"));

                            allCells1.AutoFilter = true;
                            allCells2.AutoFitColumns();
                            package.Save();

                            stream.Position = 0;
                            Session["ExportOutStandingBalanceDownload"] = package.GetAsByteArray();
                        }
                    }
                    else
                    {
                        return Json(new { isError = true, Msg = "ไม่มีข้อมูล!!!" });
                    }
                    return Json(new { isError = false }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception Ex)
                {
                    Session.Remove("ExportOutStandingBalanceDownload");
                    return Json(new { isError = true, Msg = Ex.Message });
                }
            }
        }

        public ActionResult ExportOutStandingBalanceDownload()
        {
            //var dataKey = TempData["keyReport"].ToString();
            if (Session["ExportOutStandingBalanceDownload"] != null)
            {
                //var period = TempData[dataKey] as string;

                byte[] data = Session["ExportOutStandingBalanceDownload"] as byte[];
                string excelName = $"รายงานยอดลูกหนี้คงค้าง-{DateTime.Now.ToString("ddMMyyyy-HHMM", CultureInfo.InvariantCulture)}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        #endregion

        #region Dashboard เคลมปฏิเสธ

        [Authorization(Roles = "Developer,SmileClaimPayBack_Finance")]
        public ActionResult DashboardRejectClaim()
        {
            ViewBag.ClaimType = _context.usp_ClaimType_Select().ToList();
            ViewBag.ClaimHeaderGroupTypeId = _context.usp_ClaimHeaderGroupType_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.InsuranceCompany = _context.usp_OrganizeInsuranceForGenerateGroup_Select(null, 0, 9999, null, null, null).ToList();
            return View();
        }

        public JsonResult GetDashboardClaimRejectTotal(string billingDateFrom = null, string billingDateTo = null, string claimTypeId = null, int? claimHeaderGroupType = null, int? organizeId = null)
        {
            Log.Debug("{_controllerName} Start GetDashboardClaimRejectTotal [billingDateFrom = {billingDateFrom}, billingDateTo = {billingDateTo}]", _controllerName, claimTypeId);

            DateTime d_BillingDateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(billingDateFrom));
            DateTime d_BillingDateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(billingDateTo));
            var _claimType = claimTypeId == "-1" ? null : claimTypeId;
            var _claimHeaderGroupType = claimHeaderGroupType == -1 ? null : claimHeaderGroupType;
            var _organizeId = organizeId == -1 ? null : organizeId;

            Log.Debug($"{_controllerName} - GetDashboardClaimRejectTotal Call Store procedure: usp_DashboardBillingAmountTotal_Detail_Select [d_BillingDateFrom = {d_BillingDateFrom}, d_BillingDateTo = {d_BillingDateTo},claimTypeId = {_claimType}, claimHeaderGroupType = {_claimHeaderGroupType}");

            var result = _context.usp_DashboardClaimRejectCountAndAmountTotal_Select(d_BillingDateFrom, d_BillingDateTo, _claimType, _claimHeaderGroupType, _organizeId).FirstOrDefault();


            Log.Information("{_controllerName} - GetDashboardClaimRejectTotal Successful", _controllerName);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDashboardClaimRejectResult(string billingDateFrom = null, string billingDateTo = null, string claimTypeId = null, int? claimHeaderGroupType = null, int? organizeId = null)
        {
            Log.Debug("{_controllerName} Start GetDashboardClaimRejectResult [billingDateFrom = {billingDateFrom}, billingDateTo = {billingDateTo}]", _controllerName, billingDateFrom, billingDateTo, claimTypeId, claimHeaderGroupType);
            DateTime d_BillingDateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(billingDateFrom));
            DateTime d_BillingDateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(billingDateTo));

            var _claimTypeId = claimTypeId == "-1" ? null : claimTypeId;
            var _claimHeaderGroupType = claimHeaderGroupType == -1 ? null : claimHeaderGroupType;
            var _organizeId = organizeId == -1 ? null : organizeId;

            Log.Debug($"{_controllerName} - GetDashboardClaimRejectResult Call Store procedure: usp_DashboardClaimRejectBillingCountAndAmount_Select [d_BillingDateFrom = {d_BillingDateFrom}, d_BillingDateTo = {d_BillingDateTo},claimTypeId = {_claimTypeId}, claimHeaderGroupType = {_claimHeaderGroupType}]");

            var list = _context.usp_DashboardClaimRejectCountAndAmountTotalChart_Select(d_BillingDateFrom, d_BillingDateTo, _claimTypeId, _claimHeaderGroupType, _organizeId).ToList();



            Log.Information("{_controllerName} - GetDashboardClaimRejectResult Successful", _controllerName);
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region DocumentLink

        public Tuple<List<DocumentLinkPHModel>, List<DocumentLinkPAModel>> GetS3URL(int? billingRequestGroupId, List<DocumentLinkPHModel> resultPH, List<DocumentLinkPAModel> resultPA)
        {
            Log.Debug("{_controllerName} Start GetS3URL ", _controllerName);
            try
            {
                Log.Debug($"{_controllerName} - GetS3URL Call Store procedure: usp_S3UploaderConfig_Select [ParameterName = {null}]");
                var S3Config = _context.usp_S3UploaderConfig_Select(null).Where(x =>
                    x.ParameterName == "S3AccesskeyRead" ||
                    x.ParameterName == "S3SecretkeyRead" ||
                    x.ParameterName == "S3RegionName" ||
                    x.ParameterName == "ClaimFileForInsuranceReadTimeout"
                ).ToList();


                var S3_Acc = S3Config.Where(x => x.ParameterName == "S3AccesskeyRead").Select(y => y.ValueString).FirstOrDefault();
                var S3_Secr = S3Config.Where(x => x.ParameterName == "S3SecretkeyRead").Select(y => y.ValueString).FirstOrDefault();
                var S3_RegString = S3Config.Where(x => x.ParameterName == "S3RegionName").Select(y => y.ValueString).FirstOrDefault();
                var dayleftNum = Convert.ToInt32(S3Config.Where(x => x.ParameterName == "ClaimFileForInsuranceReadTimeout").Select(y => y.ValueNumber).FirstOrDefault());

                if (S3_RegString == "Asia Pacific (Singapore)")
                {
                    S3_RegString = "ap-southeast-1";
                }

                var S3Reg = Amazon.RegionEndpoint.GetBySystemName(S3_RegString);

                AmazonS3Config AWSS3Config = new AmazonS3Config
                {
                    RegionEndpoint = S3Reg
                };

                AmazonS3Client AWSS3Client = new AmazonS3Client(S3_Acc, S3_Secr, AWSS3Config);

                Log.Debug($"{_controllerName} - GetS3URL Call Store procedure: usp_GetS3ByBillingRequestGroupId_Select [billingRequestGroupId = {billingRequestGroupId}, billingRequestGroupListId = {null}, IsCheck = {1}]");
                var lstDocument = _context.usp_GetS3ByBillingRequestGroupId_Select(billingRequestGroupId, null, 1).ToList();

                var nameToken = Properties.Settings.Default.TokenName;
                var token = Request.Cookies[nameToken].Value;
                string endpoint = Properties.Settings.Default.EndpointGenerateShortUrl;

                // Restsharp client
                var client = new RestClient(endpoint)
                {
                    Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(token, "Bearer")
                };

                if (resultPH != null)
                {
                    Log.Debug("{_controllerName} Start GetLink And AddLink To DocumentLinkPHModel", _controllerName);
                    if (lstDocument.Count > 0)
                    {
                        string docURL = "";

                        foreach (var item in resultPH)
                        {
                            docURL = "";

                            var c = lstDocument.Where(x => x.ClaimCode == item.Code).ToList();
                            var shortLinks = new List<string>();
                            for (int i = 0; i < c.Count; i++)
                            {
                                DateTime readFileTimeoutDT = DateTime.Now.AddDays(dayleftNum);

                                //เปลี่ยน docURL += เป็น docURL =
                                docURL = AWSS3Client.GetPreSignedURL(new Amazon.S3.Model.GetPreSignedUrlRequest
                                {
                                    BucketName = c[i].S3Bucket,
                                    Key = c[i].S3Key,
                                    Expires = readFileTimeoutDT
                                });

                                shortLinks.Add(GenerateShortUrl(docURL, readFileTimeoutDT, client));
                                //docURL += " , ";
                            }

                            var shortLinkComma = String.Join(",", shortLinks);
                            int index = resultPH.FindIndex(obj => obj.Code == item.Code);
                            resultPH[index].DocumentLink = shortLinkComma; //docURL.Substring(0, docURL.Length - 3);

                        }
                        Log.Debug("{_controllerName} GetLink And AddLink To DocumentLinkPHModel Successful ", _controllerName);
                    }
                }
                else
                {
                    Log.Debug("{_controllerName} Start GetLink And AddLink To DocumentLinkPAModel", _controllerName);
                    if (lstDocument.Count > 0)
                    {
                        string docURL = "";
                        foreach (var item in resultPA)
                        {
                            docURL = "";
                            var c = lstDocument.Where(x => x.ClaimCode == item.Code).ToList();

                            var shortLinks = new List<string>();
                            for (int i = 0; i < c.Count; i++)
                            {
                                DateTime readFileTimeoutDT = DateTime.Now.AddDays(dayleftNum);

                                //เปลี่ยน docURL += เป็น docURL =
                                docURL = AWSS3Client.GetPreSignedURL(new Amazon.S3.Model.GetPreSignedUrlRequest
                                {
                                    BucketName = c[i].S3Bucket,
                                    Key = c[i].S3Key,
                                    Expires = readFileTimeoutDT
                                });

                                shortLinks.Add(GenerateShortUrl(docURL, readFileTimeoutDT, client));
                                //docURL += " , ";
                            }

                            var shortLinkComma = String.Join(",", shortLinks);
                            int index = resultPA.FindIndex(obj => obj.Code == item.Code);
                            resultPA[index].DocumentLink = shortLinkComma; //docURL.Substring(0, docURL.Length - 3);
                        }
                        Log.Debug("{_controllerName} GetLink And AddLink To DocumentLinkPAModel Successful ", _controllerName);
                    }
                }
                Log.Information("{_controllerName} - GetS3URL Successful", _controllerName);
                return System.Tuple.Create(resultPH, resultPA);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - GetS3URL Error: {Message}", _controllerName, ex.Message);
                return System.Tuple.Create(resultPH, resultPA);
            }
        }
        #endregion

        #region  Group เคลม ร.พ. และ เคลมโอนแยก
        public ActionResult ClaimPayBackSubGroup(string ClaimPayBackTransferId = null, string ClaimPayBackTransferCode = null)
        {
            //DeCode
            if (ClaimPayBackTransferId != null)
            {
                var Base64EncodedBytes = Convert.FromBase64String(ClaimPayBackTransferId);
                ClaimPayBackTransferId = System.Text.Encoding.UTF8.GetString(Base64EncodedBytes);
            }

            //DeCode
            if (ClaimPayBackTransferCode != null)
            {
                var Base64EncodedBytes = Convert.FromBase64String(ClaimPayBackTransferCode);
                ClaimPayBackTransferCode = System.Text.Encoding.UTF8.GetString(Base64EncodedBytes);
            }

            ViewBag.ClaimPayBackTransferId = ClaimPayBackTransferId;
            ViewBag.ClaimPayBackTransferCode = ClaimPayBackTransferCode;

            // check transaction for disable btn
            int? _ClaimPayBackTransferId = Convert.ToInt32(ClaimPayBackTransferId);
            var getSubGroups = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackTransferId == _ClaimPayBackTransferId).Select(s => s.ClaimPayBackSubGroupId).ToList();


            ViewBag.DisableTransferBtn = _context.ClaimPayBackSubGroupTransaction.Where(w => getSubGroups.Any(a => a.Equals(w.ClaimPayBackSubGroupId))).Count();
            ViewBag.countSubGroupByTransferId = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackTransferId == _ClaimPayBackTransferId).Sum(s => s.ItemCount);
            ViewBag.TransferStatus = _context.ClaimPayBackTransfer.FirstOrDefault(w => w.ClaimPayBackTransferId == _ClaimPayBackTransferId).ClaimPayBackTransferStatusId;

            var empCode = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            var env = Properties.Settings.Default.Environment;
            ViewBag.IsRolePermision = _context.RoleFundPayConfig.Where(w => w.IsActive == true && empCode.Contains(w.EmployeeCode) && w.Environment == env).Count() > 0 ? true : false;

            ViewBag.Province = _context.usp_Province_Select(0, 999, null, null, null).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult TransferClaimPayBackSubGroup(string ClaimPayBackTransferId = null, string ClaimPayBackTransferCode = null)
        {
            //DeCode
            if (ClaimPayBackTransferId != null)
            {
                var Base64EncodedBytes = Convert.FromBase64String(ClaimPayBackTransferId);
                ClaimPayBackTransferId = System.Text.Encoding.UTF8.GetString(Base64EncodedBytes);
            }

            //DeCode
            if (ClaimPayBackTransferCode != null)
            {
                var Base64EncodedBytes = Convert.FromBase64String(ClaimPayBackTransferCode);
                ClaimPayBackTransferCode = System.Text.Encoding.UTF8.GetString(Base64EncodedBytes);
            }

            ViewBag.ClaimPayBackTransferId = ClaimPayBackTransferId;
            ViewBag.ClaimPayBackTransferCode = ClaimPayBackTransferCode;

            var empCode = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            var env = Properties.Settings.Default.Environment;
            ViewBag.IsRolePermision = _context.RoleFundPayConfig.Where(w => w.IsActive == true && empCode.Contains(w.EmployeeCode) && w.Environment == env).Count() > 0 ? true : false;

            return View("TransferClaimPayBackSubGroup");
        }

        [HttpGet]
        public ActionResult TransferClaimPayBackSubGroupDashBord(int claimPayBackTransferId)
        {

            //จำนวนเงินทั้งหมด
            var totalAmount = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackTransferId == claimPayBackTransferId && w.IsActive == true).Sum(s => s.Amount) ?? 0;
            //จำนวนเงินโอนแล้ว
            var transferedAmount = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackTransferId == claimPayBackTransferId && w.IsActive == true && w.IsPayTransfer == true).Sum(s => s.Amount) ?? 0;
            //จำนวนเงินค้างชำระ
            var remainAmount = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackTransferId == claimPayBackTransferId && w.IsActive == true && w.IsPayTransfer == false).Sum(s => s.Amount) ?? 0;

            //จำนวนสถานพยาบาลทั้งหมด
            var totalHospital = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackTransferId == claimPayBackTransferId && w.IsActive == true).Count();
            //จำนวนสถานพยาบาลโอนแล้ว
            var transferedHospital = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackTransferId == claimPayBackTransferId && w.IsActive == true && w.IsPayTransfer == true).Count();
            //จำนวนสถานพยาบาลคงเหลือโอน
            var remainferedHospital = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackTransferId == claimPayBackTransferId && w.IsActive == true && w.IsPayTransfer == false).Count();

            ViewBag.totalAmount = totalAmount.ToString("N");
            ViewBag.transferedAmount = transferedAmount.ToString("N");
            ViewBag.remain = remainAmount.ToString("N");

            ViewBag.totalHospital = totalHospital;
            ViewBag.transferedHospital = transferedHospital;
            ViewBag.remainHospital = remainferedHospital;

            return PartialView("TransferClaimPayBackSubGroupDashBord");
        }

        [HttpPost]
        public JsonResult TransferClaimPayBackSubGroupTransfer(int[] ClaimPayBackSubGroupId)
        {
            Log.Information("{_controllerName} PayTransferHeaderCreateBySubGroup is start.", _controllerName);
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;

            try
            {

                if (ClaimPayBackSubGroupId.Count() == 0)
                {
                    Log.Information("{_controllerName} TransferClaimPayBackSubGroupTransfer int[] ClaimPayBackSubGroupId ไม่มีข้อมูล ", _controllerName);
                    return Json(new { valid = false, message = "กรุณาตรวจสอบการทำรายการ" }, JsonRequestBehavior.AllowGet);
                }
                // ตรวจสอบ duplicate data
                foreach (var claimPayBackSubGroupId in ClaimPayBackSubGroupId)
                {
                    var getSubGroups = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackSubGroupId == claimPayBackSubGroupId).Select(s => s.ClaimPayBackSubGroupId).ToList();
                    Log.Information("{_controllerName} TransferClaimPayBackSubGroupTransfer getSubGroups is [{getSubGroups}]", _controllerName, String.Join(",", getSubGroups));
                    if (_context.ClaimPayBackSubGroupTransaction.Where(w => getSubGroups.Any(a => a.Equals(w.ClaimPayBackSubGroupId)) && w.IsActive == true).Count() > 0)
                    {
                        Log.Information("{_controllerName} TransferClaimPayBackSubGroupTransfer ตรวจสอบการทำรายการซ้ำ user มีการกดข้อมูลรายการเดียวกันระบบจะทำการตรวจสอบก่อนทำรายการ", _controllerName);
                        return Json(new { valid = false, message = "รายการนี้อยู่ระหว่างการดำเนินการ" }, JsonRequestBehavior.AllowGet);
                    }

                    var getSubGroup = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackSubGroupId == claimPayBackSubGroupId && w.IsActive == true).FirstOrDefault();
                    if (getSubGroup != null)
                    {
                        var getHotpitalDetail = _context.usp_HospitalAccountDetails_Select(getSubGroup.HospitalCode).FirstOrDefault();
                        if (getHotpitalDetail.ReceivingBankId == null)
                        {
                            return Json(new { valid = false, message = "รหัสธนาคารไม่ถูกต้อง (" + getSubGroup.HospitalName + ")" }, JsonRequestBehavior.AllowGet);
                        }

                        if (getHotpitalDetail.ReceivingBankAccountNo == "" || getHotpitalDetail.ReceivingBankAccountNo == null)
                        {
                            return Json(new { valid = false, message = "หมายเลขบัญชีธนาคารไม่ถูกต้อง (" + getSubGroup.HospitalName + ")" }, JsonRequestBehavior.AllowGet);
                        }

                        if (getHotpitalDetail.ReceivingName == "" || getHotpitalDetail.ReceivingName == null)
                        {
                            return Json(new { valid = false, message = "ชื่อบัญชีธนาคารไม่ถูกต้อง (" + getSubGroup.HospitalName + ")" }, JsonRequestBehavior.AllowGet);
                        }

                        // gernerate PayListHeaderId
                        Guid generatePayListHeaderId = new Guid(Guid.NewGuid().ToString());

                        // update sub group
                        getSubGroup.PayListHeaderId = generatePayListHeaderId;
                        getSubGroup.UpdatedByUserId = userId;
                        getSubGroup.UpdatedDate = DateTime.Now;
                        _context.Entry(getSubGroup).State = EntityState.Modified;
                        _context.SaveChanges();
                        Log.Information("{_controllerName} TransferClaimPayBackSubGroupTransfer Update 'ClaimPayBackSubGroup' PayListHeaderId = {PayListHeaderId} ", _controllerName, generatePayListHeaderId);

                        // create sub transaction step ที่ 1
                        var getLastRoundNumber = _context.ClaimPayBackSubGroupTransaction.Where(w => w.ClaimPayBackSubGroupId == getSubGroup.ClaimPayBackSubGroupId).OrderByDescending(O => O.RoundNumber).FirstOrDefault();
                        var createSubGroupTransaction = new ClaimPayBackSubGroupTransaction()
                        {
                            ClaimPayBackSubGroupId = getSubGroup.ClaimPayBackSubGroupId,
                            ClaimPayBackSubGroupTransactionStatusId = 2,
                            RefCode = null,
                            TransRefNo = null,
                            StatusBank = null,
                            DescriptionEN = null,
                            DescriptionTH = null,
                            TransferDate = null,
                            PayResultStatusId = 0,
                            PayResultDescription = null,
                            TransType = null,
                            RoundNumber = (getLastRoundNumber != null ? getLastRoundNumber.RoundNumber + 1 : 1),
                            PayListHeaderId = generatePayListHeaderId,
                            IsActive = true,
                            CreatedByUserId = userId,
                            CreatedDate = DateTime.Now,
                            UpdatedByUserId = userId,
                            UpdatedDate = DateTime.Now,
                            ReceiverAccountDetail = getHotpitalDetail.BankName + " " + getHotpitalDetail.ReceivingBankAccountNo + " " + getHotpitalDetail.ReceivingName
                        };
                        _context.ClaimPayBackSubGroupTransaction.Add(createSubGroupTransaction);
                        _context.SaveChanges();

                        Log.Information("{_controllerName} TransferClaimPayBackSubGroupTransfer Add ClaimPayBackSubGroupTransaction is successfully.", _controllerName);

                        // publish to paymentTransfer
                        var setModels = new PayTransferAPI.Contract.TempPayListHeaderCreate()
                        {
                            PayListHeaderId = generatePayListHeaderId,
                            PayListHeaderCode = null,
                            SendingBankId = Convert.ToInt32(Properties.Settings.Default.SendingBankId),
                            SendingBankAccountNo = Properties.Settings.Default.SendingBankAccountNo,
                            SendingName = Properties.Settings.Default.SendingName,
                            ReceivingBankId = getHotpitalDetail.ReceivingBankId,
                            ReceivingBankAccountNo = getHotpitalDetail.ReceivingBankAccountNo,
                            ReceivingName = getHotpitalDetail.ReceivingName,
                            TempPayListStatusId = 3,
                            Amount = getSubGroup.Amount,
                            PayListHeaderSourceTypeId = 4,
                            CreatedByUserId = userId,
                            CreatedDate = DateTime.Now,
                            UpdatedByUserId = userId,
                            UpdatedDate = DateTime.Now,
                            IsSentSMS = false,
                            PhoneNo = null,
                            TempPayListDetailCreate = new List<TempPayListDetailCreate>
                                    {
                                        new TempPayListDetailCreate
                                        {
                                            PayListDetailId = new Guid(Guid.NewGuid().ToString()),
                                            Amount = getSubGroup.Amount,
                                            PayListDetailCode = getSubGroup.ClaimPayBackSubGroupCode,
                                            PayListHeaderId = generatePayListHeaderId,
                                            RefDetail01 = "",
                                            RefDetail02 = "",
                                            RefDate = DateTime.Now,
                                        }
                                    }
                        };
                        MvcApplication.bus.Publish<PayTransferAPI.Contract.TempPayListHeaderCreate>(setModels);

                        Log.Information("{_controllerName} Function TransferClaimPayBackSubGroupTransfer Publish to contract name {contractName} PayListHeaderId={PayListHeaderId}", _controllerName, "PayTransferAPI.Contract.TempPayListHeaderCreate", generatePayListHeaderId);

                    }
                    else
                    {
                        Log.Information("{_controllerName} TransferClaimPayBackSubGroupTransfer getSubGroup ไม่มีข้อมูล", _controllerName);
                    }
                }


            }
            catch (Exception error)
            {
                string errorMessage = error.InnerException == null ? error.Message : error.InnerException.Message;
                Log.Error("{_controllerName} TransferClaimPayBackSubGroupTransfer is error {message}.", _controllerName, errorMessage);
                return Json(new { valid = false, message = errorMessage }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { valid = true, message = "ทำรายการสำเร็จ" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorization(Roles = "SmileClaimPayBack_FundManage,Developer")]
        public ActionResult ClaimPayBackSubGroupUpload(string ClaimPayBackTransferId = null, string ClaimPayBackTransferCode = null)
        {
            //DeCode
            if (ClaimPayBackTransferId != null)
            {
                var Base64EncodedBytes = Convert.FromBase64String(ClaimPayBackTransferId);
                ClaimPayBackTransferId = System.Text.Encoding.UTF8.GetString(Base64EncodedBytes);
            }

            //DeCode
            if (ClaimPayBackTransferCode != null)
            {
                var Base64EncodedBytes = Convert.FromBase64String(ClaimPayBackTransferCode);
                ClaimPayBackTransferCode = System.Text.Encoding.UTF8.GetString(Base64EncodedBytes);
            }

            ViewBag.ClaimPayBackTransferId = ClaimPayBackTransferId;
            ViewBag.ClaimPayBackTransferCode = ClaimPayBackTransferCode;

            // check transaction for disable btn
            int? _ClaimPayBackTransferId = Convert.ToInt32(ClaimPayBackTransferId);
            var getSubGroups = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackTransferId == _ClaimPayBackTransferId).Select(s => s.ClaimPayBackSubGroupId).ToList();


            ViewBag.DisableTransferBtn = _context.ClaimPayBackSubGroupTransaction.Where(w => getSubGroups.Any(a => a.Equals(w.ClaimPayBackSubGroupId))).Count();
            ViewBag.countSubGroupByTransferId = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackTransferId == _ClaimPayBackTransferId).Sum(s => s.ItemCount);
            ViewBag.TransferStatus = _context.ClaimPayBackTransfer.FirstOrDefault(w => w.ClaimPayBackTransferId == _ClaimPayBackTransferId).ClaimPayBackTransferStatusId;

            var empCode = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            var env = Properties.Settings.Default.Environment;
            ViewBag.IsRolePermision = _context.RoleFundPayConfig.Where(w => w.IsActive == true && empCode.Contains(w.EmployeeCode) && w.Environment == env).Count() > 0 ? true : false;

            ViewBag.Province = _context.usp_Province_Select(0, 999, null, null, null).ToList();
            return View();
        }

        [HttpGet]
        [Authorization(Roles = "SmileClaimPayBack_FundManage,Developer")]
        public ActionResult CreateClaimPayBackSubGroup()
        {

            ViewBag.Status = _context.usp_ClaimPayBackTransferStatus_Select(null, 0, 9999, null, null, null)
                .Where(x => x.ClaimPayBackTransferStatusId != 1)
                .OrderByDescending(o => o.ClaimPayBackTransferStatusId == 5)
                .ToList();

            return View("CreateClaimPayBackSubGroup");
        }

        [HttpGet]
        public JsonResult GetClaimPayBackSubGroupDashbord(int claimPayBackTransferId)
        {
            var getSubGroupsDashBoard = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackTransferId == claimPayBackTransferId).ToList();

            float countAll = getSubGroupsDashBoard.Count();
            float countAllDocs = (float)getSubGroupsDashBoard.Sum(s => s.ItemCount);
            float coustIsPay = getSubGroupsDashBoard.Where(w => w.IsPayTransfer == true).Count();
            float coustIsSendEmail = getSubGroupsDashBoard.Where(w => w.IsSendEmail == true).Count();
            float coustIsDoc = (float)getSubGroupsDashBoard.Where(w => w.IsUploadDoc == true).Sum(s => s.ItemCount);

            var DashBoard = new
            {
                isPay = $"{coustIsPay}/{countAll}",
                isSendDoc = $"{coustIsDoc}/{countAllDocs}",
                isSendEmail = $"{coustIsSendEmail}/{countAll}",
                currProgressIsPay = ((coustIsPay / countAll) * 100).ToString() + '%',
                currProgressSendDoc = ((coustIsDoc / countAllDocs) * 100).ToString() + '%',
                currProgressSendEmail = ((coustIsSendEmail / countAll) * 100).ToString() + '%'
            };

            return Json(DashBoard, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GenerateClaimPayBackSubGroup(int ClaimPayBackTransferId)
        {
            try
            {
                Log.Debug("{_controllerName} Start GenerateClaimPayBackSubGroup [ClaimPayBackTransferId = {ClaimPayBackTransferId}]", _controllerName, ClaimPayBackTransferId);
                int createByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

                Log.Debug($"{_controllerName} - GenerateClaimPayBackSubGroup [Store procedure]: usp_ClaimPayBackSubGroup_Insert [ClaimPayBackTransferId = {ClaimPayBackTransferId}, createByUserId = {createByUserId}]");
                var result = _context.usp_ClaimPayBackSubGroup_Insert(ClaimPayBackTransferId, createByUserId).FirstOrDefault();
                Log.Debug("{_controllerName} - GenerateClaimPayBackSubGroup [Store procedure]: usp_ClaimPayBackSubGroup_Insert [Result]: {@result}", _controllerName, result);

                if (result.IsResult.Value)
                {
                    Log.Information("{_controllerName} - GenerateClaimPayBackSubGroup Successful", _controllerName);

                    var getTransfer = _context.ClaimPayBackTransfer.Where(w => w.ClaimPayBackTransferId == ClaimPayBackTransferId && w.IsActive == 1).FirstOrDefault();
                    if (getTransfer != null)
                    {
                        getTransfer.ClaimPayBackTransferStatusId = 2;
                        getTransfer.UpdatedDate = DateTime.Now;
                        getTransfer.UpdatedByUserId = createByUserId;

                        _context.Entry(getTransfer).State = EntityState.Modified;
                        _context.SaveChanges();

                        Log.Information("{_controllerName} - GenerateClaimPayBackSubGroup อัปเดต status ในตาราง ClaimPayBackTransfer ClaimPayBackTransferStatusId = 2 ในรายการ ClaimPayBackTransferId = {ClaimPayBackTransferId} ", _controllerName, ClaimPayBackTransferId);

                    }

                    return Json(ResponseResult.Success<string>(message: result.Msg), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Log.Information("{_controllerName} - GenerateClaimPayBackSubGroup failed", _controllerName);
                    return Json(ResponseResult.Failure<string>(message: result.Msg), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - GenerateClaimPayBackSubGroup Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetClaimPayBackSubGroup(int? ClaimPayBackTransferId = null, int? draw = null, int? indexStart = null, int? pageSize = null, string sortField = null, string orderType = null, string search = null, string mailStatus = null, string docStatus = null, string IsPayTransfer = null)
        {
            Log.Debug("{_controllerName} Start GetClaimPayBackSubGroup ", _controllerName);
            Log.Debug($"{_controllerName} - GetClaimPayBackSubGroup Call Store procedure: usp_ClaimPayBackSubGroup_Select [ClaimPayBackTransferId = {ClaimPayBackTransferId}, indexStart = {indexStart}, pageSize = {pageSize}, searchDetail = {search} ]");

            bool? mail = (mailStatus == "-1") ? (bool?)null : (mailStatus == "1") ? true : false;
            bool? doc = (docStatus == "-1") ? (bool?)null : (docStatus == "1") ? true : false;
            bool? _IsPayTransfer = (IsPayTransfer == "-1") ? (bool?)null : (IsPayTransfer == "true") ? true : false;

            var isAction = new List<bool>();
            var list = _context.usp_ClaimPayBackSubGroup_Select(ClaimPayBackTransferId, indexStart, pageSize, sortField == "" ? null : sortField, orderType, search == "" ? null : search, mail, doc, _IsPayTransfer)
                .OrderByDescending(o => o.IsPayTransfer == false)
                .ToList();

            foreach (var item in list)
            {
                isAction.Add(_context.ClaimPayBackSubGroupTransaction.Any(w => w.ClaimPayBackSubGroupId == item.ClaimPayBackSubGroupId));
            }

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()},
                {"totalAmount", list.Count() != 0 ? list.FirstOrDefault().TotalAmount : 0},
                { "isActionList" , isAction}
            };

            Log.Information("{_controllerName} - GetClaimPayBackSubGroup Successful", _controllerName);

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHospitalNameForPHDetail(string q, string provinceId)
        {
            var rs = _context.usp_CompanyGroupHospital_Select(provinceId, 0, 10, null, null, q).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerateClaimPayBackSubGroupRefCode(int claimPayBackTransferId, string transferDate)
        {
            try
            {
                Log.Debug("{_controllerName} Start GenerateClaimPayBackSubGroupRefCode [ClaimPayBackTransferId = {claimPayBackTransferId}] ", _controllerName, claimPayBackTransferId);
                var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

                Log.Debug($"{_controllerName} - GetClaimPayBackSubGroup Call Store procedure: usp_ClaimPayBackSubGroupReferenceNo_Update [claimPayBackTransferId = {claimPayBackTransferId}, userId = {userId}]");
                var result = _context.usp_ClaimPayBackSubGroupReferenceNo_Update(claimPayBackTransferId, userId).FirstOrDefault();
                Log.Debug("{_controllerName} - GenerateClaimPayBackSubGroupRefCode [Result]: {@result}", _controllerName, result);

                Log.Information("{_controllerName} - GenerateClaimPayBackSubGroupRefCode Successful ClaimPayBackTransferId = {claimPayBackTransferId}", _controllerName);
                return Json(ResponseResult.Success<string>(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - GenerateClaimPayBackSubGroupRefCode Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        private bool GenerateMedicalBillsPDF(int claimPayBackTransferId, ClaimPayBackSubGroup objSubGroup, IEnumerable<int?> distinctProductGroupId)
        {
            var success = true;
            try
            {
                Log.Debug("{_controllerName} Start GenerateMedicalBillsPDF [claimPayBackTransferId = {claimPayBackTransferId}, objSubGroup = {@objSubGroup}, distinctProductGroupId = {@distinctProductGroupId}]", _controllerName, claimPayBackTransferId, objSubGroup, distinctProductGroupId);

                #region Check Directory in ISC_SmileDoc
                string documentPath = Properties.Settings.Default.MedicalBillsSmileDocPath;
                var environment = Properties.Settings.Default.Environment;
                DateTime transferDate = (DateTime)objSubGroup.BillingTransferDate;

                if (environment == "UAT")
                {
                    for (int DocumentId = 134; DocumentId < 136; DocumentId++)
                    {
                        var DocumentType = DocumentId == 134 ? "5" : "6";
                        var checkDocumentPath = $"{documentPath}{transferDate.Year + 543}\\{DocumentType}\\{DocumentId}\\{transferDate.Month}\\";
                        // Check Exist Directory
                        if (!Directory.Exists(checkDocumentPath))
                        {
                            Log.Debug($"{_controllerName} CreateDirectory [Path = {checkDocumentPath}]");
                            Directory.CreateDirectory(checkDocumentPath);
                        }
                    }
                }
                else
                {
                    for (int DocumentId = 137; DocumentId < 139; DocumentId++)
                    {
                        var DocumentType = DocumentId == 137 ? "5" : "6";
                        var checkDocumentPath = $"{documentPath}{transferDate.Year + 543}\\{DocumentType}\\{DocumentId}\\{transferDate.Month}\\";
                        // Check Exist Directory
                        if (!Directory.Exists(checkDocumentPath))
                        {
                            Log.Debug($"{_controllerName} CreateDirectory [Path = {checkDocumentPath}]");
                            Directory.CreateDirectory(checkDocumentPath);
                        }
                    }
                }
                #endregion

                var claimPayBackSubGroup = objSubGroup.ClaimPayBackSubGroupCode;

                Log.Debug($"{_controllerName} - Start BuildPdf : [claimPayBackSubGroupCode = {claimPayBackSubGroup}]");
                var pdf = (ViewAsPdf)MedicalPaymentPDF(objSubGroup.ClaimPayBackSubGroupId);

                foreach (var productGroupId in distinctProductGroupId)
                {
                    string DocumentListId = "";
                    var DocumentTypeID = "";
                    if (environment == "UAT")
                    {
                        DocumentListId = productGroupId == 2 ? "134" : "135";
                        DocumentTypeID = DocumentListId == "134" ? "5" : "6";
                        Log.Debug($"{_controllerName} Environment UAT DocumentListId = {DocumentListId}, DocumentTypeID = {DocumentTypeID}");

                    }
                    else
                    {
                        DocumentListId = productGroupId == 2 ? "137" : "138";
                        DocumentTypeID = DocumentListId == "137" ? "5" : "6";
                        Log.Debug($"{_controllerName} Environment Prod DocumentListId = {DocumentListId}, DocumentTypeID = {DocumentTypeID}");
                    }

                    var insertPath = $"{documentPath}{transferDate.Year + 543}\\{DocumentTypeID}\\{DocumentListId}\\{transferDate.Month}\\{claimPayBackSubGroup}.pdf";

                    var byteArray = pdf.BuildPdf(ControllerContext);
                    var fileStream = new FileStream(insertPath, FileMode.Create, FileAccess.Write);
                    fileStream.Write(byteArray, 0, byteArray.Length);
                    fileStream.Close();

                    // Create a new object to insert
                    Log.Debug($"{_controllerName} - GenerateMedicalBilllsPDF ClaimPayBackTransferId = {claimPayBackTransferId} Insert ClaimPayBackSubGroupPathFilePDF [claimPayBackSubGroupId = {objSubGroup.ClaimPayBackSubGroupId}, PathFilePDF = {insertPath}]");
                    var newPDFPath = new ClaimPayBackSubGroupPathFilePDF
                    {
                        ClamPayBackSubGroupId = objSubGroup.ClaimPayBackSubGroupId,
                        PathFilePDF = insertPath,
                        ProductGroupId = productGroupId,
                        IsActive = true
                    };

                    // Add the new object to the DbSet (table) and save changes
                    _context.ClaimPayBackSubGroupPathFilePDF.Add(newPDFPath);
                    _context.SaveChanges();
                }

                Log.Information("{_controllerName} - GenerateMedicalBilllsPDF Successful [ClaimPayBackTransferId = {claimPayBackTransferId}]", _controllerName, claimPayBackTransferId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - GenerateMedicalBilllsPDF [ClaimPayBackTransferId = {claimPayBackTransferId}] Error: {Message}", _controllerName, claimPayBackTransferId, ex.Message);
                success = false;
            }

            return success;
        }

        public ActionResult MedicalPaymentPrintAllPDF(string claimPayBackTransferId, string claimPayBackTransferCode)
        {
            try
            {
                Log.Debug("{_controllerName} Start MedicalPaymentPrintAllPDF [ClaimPayBackTransferId = {claimPayBackTransferId}, ClaimPayBackTransferCode = {claimPayBackTransferCode}] ", _controllerName, claimPayBackTransferId, claimPayBackTransferCode);

                var Base64EncodedBytes = Convert.FromBase64String(claimPayBackTransferId);
                claimPayBackTransferId = System.Text.Encoding.UTF8.GetString(Base64EncodedBytes);

                var Base64EncodedBytesX = Convert.FromBase64String(claimPayBackTransferCode);
                claimPayBackTransferCode = System.Text.Encoding.UTF8.GetString(Base64EncodedBytesX);

                int xClaimPayBackTransferId = Convert.ToInt32(claimPayBackTransferId);

                var result = _context.ClaimPayBackSubGroup.Where(x => x.ClaimPayBackTransferId == xClaimPayBackTransferId).Select(x => new
                {
                    ClaimPayBackSubGroupId = x.ClaimPayBackSubGroupId
                    ,
                    Amount = x.Amount
                }).ToList();

                MedicalBillAllDataListModel listAll = new MedicalBillAllDataListModel();
                foreach (var item in result)
                {
                    var list = _context.usp_MedicalBillsReport_Select(item.ClaimPayBackSubGroupId).AsEnumerable();
                    listAll.MedicalBillReports.Add(item.ClaimPayBackSubGroupId, list);
                }

                var tpaHeaderUrl = Properties.Settings.Default.LocalUrl + "/Report/TPAReportHeader";
                var tpaFooterUrl = Properties.Settings.Default.LocalUrl + "/Report/TPAReportFooter";
                var customSwitches =
                    string.Format("--print-media-type --allow {0} --header-html {0} --allow {1} --footer-html {1}", tpaHeaderUrl, tpaFooterUrl);

                //using WebConfig Call Medical Data
                ViewBag.ClaimPayBackMedicalManagerName = Properties.Settings.Default.ClaimPayBackMedicalManagerName;
                ViewBag.ClaimPayBackMedicalPosition = Properties.Settings.Default.ClaimPayBackMedicalPosition;

                var pdf = new ViewAsPdf
                {
                    Model = listAll,
                    ViewName = "MedicalPaymentPrintAllPDF",
                    FileName = claimPayBackTransferCode + "-" + result.Sum(x => x.Amount)?.ToString("#,###.00") + ".pdf",
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageSize = Rotativa.Options.Size.A4,
                    MinimumFontSize = 10,
                    PageMargins = new Rotativa.Options.Margins(35, 0, 29, 0),
                    CustomSwitches = customSwitches,
                };

                Log.Information("{_controllerName} - MedicalPaymentPrintAllPDF Successful [ClaimPayBackTransferId = {claimPayBackTransferId}]", _controllerName, claimPayBackTransferId);
                return pdf;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - MedicalPaymentPrintAllPDF [ClaimPayBackTransferId = {claimPayBackTransferId}] Error: {Message}", _controllerName, claimPayBackTransferId, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ValidateMedicalBillls(int claimPayBackTransferId, int claimPayBackSubGroupId, string hospitalCode)
        {
            try
            {
                Log.Debug("{_controllerName} Start ValidateMedicalBillls [ClaimPayBackTransferId = {claimPayBackTransferId}, ClaimPayBackSubGroupId = {claimPayBackSubGroupId}]", _controllerName, claimPayBackTransferId, claimPayBackSubGroupId);
                string StrErrorGenerate = "เกิดข้อผิดพลาดในการ Generate ไฟล์";
                var objSubGroup = _context.ClaimPayBackSubGroup.Where(x => x.ClaimPayBackSubGroupId == claimPayBackSubGroupId).FirstOrDefault();

                if (objSubGroup == null)
                {
                    Log.Warning("{_controllerName} Data not fould in ClaimPayBackSubGroup [ClaimPayBackSubGroupId = {ClaimPayBackSubGroupId}]", _controllerName, claimPayBackSubGroupId);
                    return Json(ResponseResult.Failure<string>(string.Format("ไม่พบข้อมูล: {0}", objSubGroup)), JsonRequestBehavior.AllowGet);
                }

                var MedicalBillsReport = _context.usp_MedicalBillsReportByClaimPayBackSubGroupId_Select(claimPayBackSubGroupId).ToList();
                var distinctProductGroupId = MedicalBillsReport.Select(x => x.ProductGroupId).Distinct();
                var lstClaimPayBackSubGroupPathFilePDF = _context.ClaimPayBackSubGroupPathFilePDF.Where(x => x.ClamPayBackSubGroupId == claimPayBackSubGroupId && x.IsActive == true).ToList();

                if (lstClaimPayBackSubGroupPathFilePDF.Count == 0)
                {
                    if (!GenerateMedicalBillsPDF(claimPayBackTransferId, objSubGroup, distinctProductGroupId))
                        return Json(ResponseResult.Failure<string>(StrErrorGenerate), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    foreach (var item in lstClaimPayBackSubGroupPathFilePDF)
                    {
                        if (!System.IO.File.Exists(item.PathFilePDF))
                        {
                            if (!GenerateMedicalBillsPDF(claimPayBackTransferId, objSubGroup, new int?[] { item.ProductGroupId }))
                                return Json(ResponseResult.Failure<string>(StrErrorGenerate), JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                //Validate Data from Isc-SmileDoc
                if (objSubGroup.IsUploadDoc == false)
                    UploadMedicalBilllsReportToSmileDoc(claimPayBackTransferId, claimPayBackSubGroupId, objSubGroup);

                var objClaimPayBackSubGroupPathFilePDF = _context.ClaimPayBackSubGroupPathFilePDF.Where(x => x.ClamPayBackSubGroupId == claimPayBackSubGroupId && x.IsActive == true).FirstOrDefault();
                if (objClaimPayBackSubGroupPathFilePDF == null)
                    return Json(ResponseResult.Failure<string>(StrErrorGenerate), JsonRequestBehavior.AllowGet);

                //Validate MedicalBillls from Isc-SmileDoc path
                if (objSubGroup.IsSendEmail == false)
                {
                    //Send Email
                    if (!SendEmail(objSubGroup, objClaimPayBackSubGroupPathFilePDF.PathFilePDF))
                        return Json(ResponseResult.Failure<string>(objSubGroup.HospitalName, 1), JsonRequestBehavior.AllowGet);
                }

                Log.Information("{_controllerName} - ValidateMedicalBillls Successful [ClaimPayBackTransferId = {claimPayBackTransferId}, ClaimPayBackSubGroupId = {claimPayBackSubGroupId}]", _controllerName, claimPayBackTransferId, claimPayBackSubGroupId);
                return Json(ResponseResult.Success<string>(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - ValidateMedicalBillls [ClaimPayBackTransferId = {claimPayBackTransferId}, ClaimPayBackSubGroupId = {claimPayBackSubGroupId}] Error: {Message}", _controllerName, claimPayBackTransferId, claimPayBackSubGroupId, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UploadMedicalBilllsReportToSmileDoc(int? claimPayBackTransferId, int? claimPayBackSubGroupId, ClaimPayBackSubGroup objSubGroup)
        {
            try
            {
                Log.Debug("{_controllerName} Start UploadMedicalBilllsReportToSmileDoc [ClaimPayBackTransferId = {claimPayBackTransferId}, ClaimPayBackSubGroupId = {claimPayBackSubGroupId}]", _controllerName, claimPayBackTransferId, claimPayBackSubGroupId);

                var employeeCode = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
                string StrErrorUpload = "เกิดข้อผิดพลาดในการอัปโหลดไฟล์";
                var MedicalBillsReport = _context.usp_MedicalBillsReportByClaimPayBackSubGroupId_Select(claimPayBackSubGroupId).ToList();

                foreach (var i in MedicalBillsReport)
                {
                    // PrepareData
                    DateTime documentDateRef = Convert.ToDateTime(i.TransferDate);

                    int documentListId;
                    int documentIndexIdClaimHeaderGroupId;
                    int documentIndexIdClaimHeaderId;

                    string environment = Properties.Settings.Default.Environment;
                    if (environment == "UAT")
                    {
                        if (i.ProductGroupId == 3) // PA
                        {
                            documentListId = 135;
                            documentIndexIdClaimHeaderGroupId = 560;
                            documentIndexIdClaimHeaderId = 561;
                        }
                        else
                        {
                            documentListId = 134;
                            documentIndexIdClaimHeaderGroupId = 558;
                            documentIndexIdClaimHeaderId = 559;
                        }
                    }
                    else
                    {
                        if (i.ProductGroupId == 3) // PA
                        {
                            documentListId = 138;
                            documentIndexIdClaimHeaderGroupId = 578;
                            documentIndexIdClaimHeaderId = 579;
                        }
                        else
                        {
                            documentListId = 137;
                            documentIndexIdClaimHeaderGroupId = 576;
                            documentIndexIdClaimHeaderId = 577;
                        }
                    }

                    Log.Debug($"{_controllerName} - UploadMedicalBilllsReportToSmileDoc [Store procedure]: usp_MedicalBillDocument_Insert [documentListId = {documentListId}, documentDate = {documentDateRef}, employeeCode = {employeeCode}]");
                    var documentInsert = _context.usp_MedicalBillDocument_Insert(documentListId, documentDateRef, employeeCode).FirstOrDefault();
                    Log.Debug("{_controllerName} - UploadMedicalBilllsReportToSmileDoc ClaimPayBackTransferId = {claimPayBackTransferId} [Result]: {@documentInsert}", _controllerName, claimPayBackTransferId, documentInsert);

                    if (documentInsert.IsResult.Value == false)
                    {
                        Log.Warning("{_controllerName} - UploadMedicalBilllsReportToSmileDoc {StrErrorUpload} [claimPayBackTransferId = {claimPayBackTransferId}]", _controllerName, StrErrorUpload, claimPayBackTransferId);
                        return Json(ResponseResult.Failure<string>(StrErrorUpload), JsonRequestBehavior.AllowGet);
                    }

                    int documentId = Convert.ToInt32(documentInsert.Result);
                    // Insert DocumentData
                    Log.Debug($"{_controllerName} - UploadMedicalBilllsReportToSmileDoc [Store procedure]: usp_MedicalBillDocumentData_Insert [documentIndexId = {documentIndexIdClaimHeaderGroupId}, documentId = {documentId}, documentIndexData = {i.ClaimGroupCode}, employeeCode = {employeeCode}]");
                    var documentInsertDataClaimHeaderGroupId = _context.usp_MedicalBillDocumentData_Insert(documentIndexIdClaimHeaderGroupId, documentId, i.ClaimGroupCode, employeeCode).FirstOrDefault();
                    Log.Debug("{_controllerName} - UploadMedicalBilllsReportToSmileDoc ClaimPayBackTransferId = {claimPayBackTransferId} [Result]: {@documentInsertDataClaimHeaderGroupId}", _controllerName, claimPayBackTransferId, documentInsertDataClaimHeaderGroupId);

                    if (documentInsertDataClaimHeaderGroupId.IsResult.Value == false)
                    {
                        Log.Warning("{_controllerName} - UploadMedicalBilllsReportToSmileDoc {StrErrorUpload} [claimPayBackTransferId = {claimPayBackTransferId}]", _controllerName, StrErrorUpload, claimPayBackTransferId);
                        return Json(ResponseResult.Failure<string>(StrErrorUpload), JsonRequestBehavior.AllowGet);
                    }

                    Log.Debug($"{_controllerName} - UploadMedicalBilllsReportToSmileDoc [Store procedure]: usp_MedicalBillDocumentData_Insert [documentIndexId = {documentIndexIdClaimHeaderId}, documentId = {documentId}, documentIndexData = {i.ClaimHeader_id}, employeeCode = {employeeCode}]");
                    var documentInsertDataClaimHeaderId = _context.usp_MedicalBillDocumentData_Insert(documentIndexIdClaimHeaderId, documentId, i.ClaimHeader_id, employeeCode).FirstOrDefault();
                    Log.Debug("{_controllerName} - UploadMedicalBilllsReportToSmileDoc ClaimPayBackTransferId = {claimPayBackTransferId} [Result]: {@documentInsertDataClaimHeaderId}", _controllerName, claimPayBackTransferId, documentInsertDataClaimHeaderId);

                    if (documentInsertDataClaimHeaderId.IsResult.Value == false)
                    {
                        Log.Warning("{_controllerName} - UploadMedicalBilllsReportToSmileDoc {StrErrorUpload} [claimPayBackTransferId = {claimPayBackTransferId}]", _controllerName, StrErrorUpload, claimPayBackTransferId);
                        return Json(ResponseResult.Failure<string>(StrErrorUpload), JsonRequestBehavior.AllowGet);
                    }

                    // PrepareDocument & Upload
                    string Filename = i.ClaimPayBackSubGroupCode + ".pdf";

                    // Insert Attachment
                    Log.Debug($"{_controllerName} - UploadMedicalBilllsReportToSmileDoc [Store procedure]: usp_MedicalBillAttachment_Insert [documentId = {documentId}, attachmentName = {Filename}, employeeCode = {employeeCode}]");
                    var attachmentInsert = _context.usp_MedicalBillAttachment_Insert(documentId, Filename, employeeCode).FirstOrDefault();
                    Log.Debug("{_controllerName} - UploadMedicalBilllsReportToSmileDoc ClaimPayBackTransferId = {claimPayBackTransferId} [Result]: {@attachmentInsert}", _controllerName, claimPayBackTransferId, attachmentInsert);

                    if (attachmentInsert.IsResult.Value == false)
                    {
                        Log.Warning("{_controllerName} - UploadMedicalBilllsReportToSmileDoc {StrErrorUpload} [claimPayBackTransferId = {claimPayBackTransferId}]", _controllerName, StrErrorUpload, claimPayBackTransferId);
                        return Json(ResponseResult.Failure<string>(StrErrorUpload), JsonRequestBehavior.AllowGet);
                    }
                }

                //Update IsUploadDoc = 1 (ส่ง ISC_SmileDoc แล้ว)
                Log.Debug($"{_controllerName} - UploadMedicalBilllsReportToSmileDoc Update IsUploadDoc Column [ClaimPayBackSubGroupId = {claimPayBackSubGroupId}, IsUploadDoc = {objSubGroup.IsUploadDoc}]", _controllerName, claimPayBackSubGroupId, objSubGroup.IsUploadDoc);
                objSubGroup.IsUploadDoc = true;
                _context.SaveChanges();

                Log.Information("{_controllerName} - UploadMedicalBilllsReportToSmileDoc Successful [ClaimPayBackTransferId = {claimPayBackTransferId}, ClaimPayBackSubGroupId = {claimPayBackSubGroupId}]", _controllerName, claimPayBackTransferId, claimPayBackSubGroupId);
                return Json(ResponseResult.Success<string>(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - UploadMedicalBilllsReportToSmileDoc [ClaimPayBackTransferId = {claimPayBackTransferId}, ClaimPayBackSubGroupId = {claimPayBackSubGroupId}] Error: {Message}", _controllerName, claimPayBackTransferId, claimPayBackSubGroupId, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        private bool SendEmail(ClaimPayBackSubGroup objSubGroup, string pathFilePDF)
        {
            Log.Information("[SendEmail] - Start Send Email [objSubGroup = {@objSubGroup}]", objSubGroup);

            string emailAddress = "";
            string ccAddresses = "";

            //ชื่อไฟล์เอกสารสำหรับแนบใน Email
            string sumAmountString = objSubGroup.Amount?.ToString("#,###.00");
            string FilePDFName = objSubGroup.HospitalName + "-" + sumAmountString + ".pdf";

            //MailKit Setting
            string hostMailKit = Properties.Settings.Default.MailKitHost;
            int portMailKit = Properties.Settings.Default.MailKitPort;
            string userMailKit = Properties.Settings.Default.MailKitUser;
            string passwordMailKit = Properties.Settings.Default.MailKitPassword;
            string env = Properties.Settings.Default.Environment;

            //Convert BillingTransferDate to Format Thai Date
            var _cultureTHInfo = new System.Globalization.CultureInfo("th-TH");
            DateTime createdDateThai = Convert.ToDateTime(objSubGroup.BillingTransferDate, _cultureTHInfo);
            string convertTransferDate = createdDateThai.ToString("d MMMM yyyy", _cultureTHInfo);

            //Create Mail Message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("กองทุนสินไหม สไมล์ ทีพีเอ ( โรงพยาบาล )", userMailKit));

            if (env == "UAT")
            {
                if (!string.IsNullOrEmpty(objSubGroup.ContactEmail))
                {
                    emailAddress = _context.MailConfig.Where(x => x.IsUse == true && x.MailConfigCode == 2).Select(x => x.MailTo).First();
                    foreach (string toAddresses in emailAddress.Split(','))
                    {
                        message.To.Add(new MailboxAddress(toAddresses, toAddresses));
                    }
                }
                else
                {
                    Log.Information("[SendEmail] [ClaimPayBackSubGroupId = {claimPayBackSubGroupId}] - Not ContactEmail", objSubGroup.ClaimPayBackSubGroupId);
                    return false;
                }

                ccAddresses = _context.MailConfig.Where(x => x.IsUse == true && x.MailConfigCode == 2).Select(x => x.MailCc).First();
                foreach (string ccAddress in ccAddresses.Split(','))
                {
                    message.Cc.Add(new MailboxAddress(ccAddress, ccAddress));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(objSubGroup.ContactEmail))
                {
                    emailAddress = objSubGroup.ContactEmail.Trim();
                    foreach (string toAddresses in emailAddress.Split(','))
                    {
                        message.To.Add(new MailboxAddress(toAddresses, toAddresses));
                    }

                    ccAddresses = _context.MailConfig.Where(x => x.IsUse == true && x.MailConfigCode == 2).Select(x => x.MailCc).First();
                    foreach (string ccAddress in ccAddresses.Split(','))
                    {
                        message.Cc.Add(new MailboxAddress(ccAddress, ccAddress));
                    }
                }
                else
                {
                    Log.Information("[SendEmail] [ClaimPayBackSubGroupId = {claimPayBackSubGroupId}] - Not ContactEmail", objSubGroup.ClaimPayBackSubGroupId);
                    return false;
                }
            }

            //Create Header Email
            message.Subject = $"แจ้งการโอนเงินค่ารักษาพยาบาล ประจำวันที่ {convertTransferDate} ";

            //Create Body Email
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "<html><body style='color: black;'>" +
                "<table><tr><td>" +
                $"<p>Subject: แจ้งการโอนเงินค่ารักษาพยาบาล ประจำวันที่ {convertTransferDate} <br />To: {objSubGroup.HospitalName} " + "<" + $"<a href = '{emailAddress}'>{emailAddress}</a>" + ">" + "</p>" +
                "</td></tr>" +
                "<tr><td>" +
                $"<p>แจ้งการโอนเงินค่ารักษาพยาบาล ประจำวันที่ {convertTransferDate} รายละเอียดตามเอกสารแนบ<br /><b> (หากได้รับเอกสารแล้วรบกวนตอบกลับด้วยค่ะ) </b></p>" +
                "</td></tr>" +
                "<tr><td>" +
                "<p style='text-align: center; padding-left: 50%;'>ขอแสดงความนับถือ</p>" +
                "</td></tr>" +
                "<tr><td>" +
                "<p style='text-align: center; padding-left: 50%;'>นางสาวบูรณา เคียงข้าง<br />เจ้าหน้าที่แผนกงานกองทุนสินไหม</p>" +
                "</td></tr>" +
                "<tr><td>" +
                "<p>แผนกกองทุนสินไหม<br>Tel. 081 - 9527996 <br>บริษัท สไมล์ ทีพีเอ จำกัด(ในเครือ สยามสไมล์) <br>89 / 6 - 10 ถ.เฉลิมพงษ์ แขวงสายไหม เขตสายไหม<br>กรุงเทพมหานคร  10220 www.siamsmile.co.th<br>Email Accclaim.isc@gmail.com<br>Tel.Call Center 1434 , 081 - 9527996<p/> " +
                "</td></tr></table>" +
                "</body></html>";

            //เตรียม Path File สำหรับแนบเอกสาร
            byte[] pdfData = System.IO.File.ReadAllBytes(pathFilePDF);

            var pdfAttachment = new MimePart()
            {
                Content = new MimeContent(new MemoryStream(pdfData), ContentEncoding.Default),
                ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName(FilePDFName),
            };

            //Add Document in Email
            bodyBuilder.Attachments.Add(pdfAttachment);

            //Set Body Email
            message.Body = bodyBuilder.ToMessageBody();

            //Connect MailKit For Send Email
            using (var client = new SmtpClient())
            {
                client.Connect(hostMailKit, portMailKit, SecureSocketOptions.StartTls); // Use your SMTP server and port
                client.Authenticate(userMailKit, passwordMailKit); // Your email credentials
                client.Send(message);


                client.Disconnect(true);
            }

            //Update IsSendEmail = 1 (ส่ง Email แล้ว)
            Log.Debug($"{_controllerName} - UploadMedicalBilllsReportToSmileDoc Update IsSendEmail Column [ClaimPayBackSubGroupId = {objSubGroup.ClaimPayBackSubGroupId}, IsSendEmail = {objSubGroup.IsSendEmail}]", _controllerName, objSubGroup.ClaimPayBackSubGroupId, objSubGroup.IsSendEmail);
            objSubGroup.IsSendEmail = true;
            _context.SaveChanges();

            //status success
            Log.Information("{_controllerName} [SendEmail] - Send Successfully - [ClaimPayBackSubGroupId = {claimPayBackSubGroupId}, MailAddress = {emailAddress}, MailCcAddress = {ccAddresses} ]", _controllerName, objSubGroup.ClaimPayBackSubGroupId, emailAddress, ccAddresses);
            return true;
        }

        public ActionResult CheckHospitalBankAccount(string[] claimGroupCodeListArray, int ClaimGroupTypeId)
        {
            try
            {
                string claimGroupCodeList = "";

                if (claimGroupCodeListArray != null)
                {
                    claimGroupCodeList = string.Join(",", claimGroupCodeListArray);
                }

                Log.Debug("{_controllerName} Start CheckHospitalBankAccount [ClaimGroupCodeList = {ClaimGroupCodeList}, ClaimGroupTypeId = {ClaimGroupTypeId}]", _controllerName, claimGroupCodeList, ClaimGroupTypeId);

                Log.Debug($"{_controllerName} - CheckHospitalBankAccount [Store procedure]: usp_CheckCompanyBankAccount_Select [claimGroupCodeList = {claimGroupCodeList}, ClaimGroupTypeId = {ClaimGroupTypeId}]");
                var result = _context.usp_CheckCompanyBankAccount_Select(claimGroupCodeList, ClaimGroupTypeId).First();
                Log.Debug("{_controllerName} - CheckHospitalBankAccount [Result]: {@rs}", _controllerName, result);

                Log.Information("{_controllerName} - CheckHospitalBankAccount Successful", _controllerName);

                if (result.IsResult == true)
                    return Json(ResponseResult.Success<string>(), JsonRequestBehavior.AllowGet);
                else
                    return Json(ResponseResult.Failure<string>(message: result.Msg), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[SendEmail] - CheckHospitalBankAccount Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ViewDetailTransaction(int ClaimPayBackSubGroupId, string ClaimPayBackSubGroupCode)
        {

            //get Data from ClaimPayBackSubGroupTransaction
            var getSubGroupTranstion = _context.usp_ClaimPayBackSubGroupTransactionDetails_Select(ClaimPayBackSubGroupId).ToList();

            ViewBag.ClaimPayBackSubGroupId = ClaimPayBackSubGroupId;
            ViewBag.ClaimPayBackSubGroupCode = ClaimPayBackSubGroupCode;
            ViewBag.Environment = Properties.Settings.Default.Environment;
            return PartialView("ViewDetailTransaction", getSubGroupTranstion);
        }

        [HttpGet]
        public ActionResult PayTransferHeaderCreate(int ClaimPayBackTransferId)
        {
            Log.Information("{_controllerName} PayTransferHeaderCreate is start.", _controllerName);
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;

            try
            {
                // ตรวจสอบ duplicate data
                var getSubGroups = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackTransferId == ClaimPayBackTransferId).Select(s => s.ClaimPayBackSubGroupId).ToList();
                if (_context.ClaimPayBackSubGroupTransaction.Where(w => getSubGroups.Any(a => a.Equals(w.ClaimPayBackSubGroupId)) && w.IsActive == true).Count() > 0)
                {
                    Log.Information("{_controllerName} PayTransferHeaderCreate ตรวจสอบการทำรายการซ่ำ user มีการกดข้อมูลรายการเดียวกันระบบจะทำการตรวจสอบก่อนทำรายการ", _controllerName);
                    return Json(new { valid = false, message = "รายการนี้อยู่ระหว่างการดำเนินการ" }, JsonRequestBehavior.AllowGet);
                }

                // get list  
                var getClaimSubGroups = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackTransferId == ClaimPayBackTransferId && w.IsActive == true).ToList();
                foreach (var get in getClaimSubGroups)
                {
                    var getHotpitalDetail = _context.usp_HospitalAccountDetails_Select(get.HospitalCode).FirstOrDefault();
                    if (getHotpitalDetail.ReceivingBankId == null)
                    {
                        return Json(new { valid = false, message = "รหัสธนาคารไม่ถูกต้อง (" + get.HospitalName + ")" }, JsonRequestBehavior.AllowGet);
                    }

                    if (getHotpitalDetail.ReceivingBankAccountNo == "" || getHotpitalDetail.ReceivingBankAccountNo == null)
                    {
                        return Json(new { valid = false, message = "หมายเลขบัญชีธนาคารไม่ถูกต้อง (" + get.HospitalName + ")" }, JsonRequestBehavior.AllowGet);
                    }

                    if (getHotpitalDetail.ReceivingName == "" || getHotpitalDetail.ReceivingName == null)
                    {
                        return Json(new { valid = false, message = "ชื่อบัญชีธนาคารไม่ถูกต้อง (" + get.HospitalName + ")" }, JsonRequestBehavior.AllowGet);
                    }

                    // gernerate PayListHeaderId
                    Guid generatePayListHeaderId = new Guid(Guid.NewGuid().ToString());

                    // update sub group
                    get.PayListHeaderId = generatePayListHeaderId;
                    get.UpdatedByUserId = userId;
                    get.UpdatedDate = DateTime.Now;
                    _context.Entry(get).State = EntityState.Modified;
                    _context.SaveChanges();
                    Log.Information("{_controllerName} Update ", _controllerName);

                    // create sub transaction step ที่ 1
                    var getLastRoundNumber = _context.ClaimPayBackSubGroupTransaction.Where(w => w.ClaimPayBackSubGroupId == get.ClaimPayBackSubGroupId).OrderByDescending(O => O.RoundNumber).FirstOrDefault();
                    var createSubGroupTransaction = new ClaimPayBackSubGroupTransaction()
                    {
                        ClaimPayBackSubGroupId = get.ClaimPayBackSubGroupId,
                        ClaimPayBackSubGroupTransactionStatusId = 2,
                        RefCode = null,
                        TransRefNo = null,
                        StatusBank = null,
                        DescriptionEN = null,
                        DescriptionTH = null,
                        TransferDate = null,
                        PayResultStatusId = 0,
                        PayResultDescription = null,
                        TransType = null,
                        RoundNumber = (getLastRoundNumber != null ? getLastRoundNumber.RoundNumber + 1 : 1),
                        PayListHeaderId = generatePayListHeaderId,
                        IsActive = true,
                        CreatedByUserId = userId,
                        CreatedDate = DateTime.Now,
                        UpdatedByUserId = userId,
                        UpdatedDate = DateTime.Now,
                        ReceiverAccountDetail = getHotpitalDetail.BankName + " " + getHotpitalDetail.ReceivingBankAccountNo + " " + getHotpitalDetail.ReceivingName
                    };
                    _context.ClaimPayBackSubGroupTransaction.Add(createSubGroupTransaction);
                    _context.SaveChanges();

                    Log.Information("TempPayListHeaderCreate is successfully.", _controllerName);

                    // publish to paymentTransfer
                    var setModels = new PayTransferAPI.Contract.TempPayListHeaderCreate()
                    {
                        PayListHeaderId = generatePayListHeaderId,
                        PayListHeaderCode = null,
                        SendingBankId = Convert.ToInt32(Properties.Settings.Default.SendingBankId),
                        SendingBankAccountNo = Properties.Settings.Default.SendingBankAccountNo,
                        SendingName = Properties.Settings.Default.SendingName,
                        ReceivingBankId = getHotpitalDetail.ReceivingBankId,
                        ReceivingBankAccountNo = getHotpitalDetail.ReceivingBankAccountNo,
                        ReceivingName = getHotpitalDetail.ReceivingName,
                        TempPayListStatusId = 3,
                        Amount = get.Amount,
                        PayListHeaderSourceTypeId = 4,
                        CreatedByUserId = userId,
                        CreatedDate = DateTime.Now,
                        UpdatedByUserId = userId,
                        UpdatedDate = DateTime.Now,
                        IsSentSMS = false,
                        PhoneNo = null,
                        TempPayListDetailCreate = new List<TempPayListDetailCreate>
                        {
                            new TempPayListDetailCreate
                            {
                                PayListDetailId = new Guid(Guid.NewGuid().ToString()),
                                Amount = get.Amount,
                                PayListDetailCode = get.ClaimPayBackSubGroupCode,
                                PayListHeaderId = generatePayListHeaderId,
                                RefDetail01 = "",
                                RefDetail02 = "",
                                RefDate = DateTime.Now,
                            }
                        }
                    };
                    MvcApplication.bus.Publish<PayTransferAPI.Contract.TempPayListHeaderCreate>(setModels);

                    Log.Information("{_controllerName} Function PayTransferHeaderCreate Publish to contract name {contractName} PayListHeaderId={PayListHeaderId}", _controllerName, "PayTransferAPI.Contract.TempPayListHeaderCreate", generatePayListHeaderId);

                }
            }
            catch (Exception error)
            {
                string errorMessage = error.InnerException == null ? error.Message : error.InnerException.Message;
                Log.Error("{_controllerName} PayTransferHeaderCreate is error {message}.", _controllerName, errorMessage);
                return Json(new { valid = false, message = errorMessage }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { valid = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ฟังก์ชันโอนเงินทีละรายการ
        /// </summary>
        /// <param name="claimPayBackSubGroupId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PayTransferHeaderCreateBySubGroup(int claimPayBackSubGroupId)
        {
            Log.Information("{_controllerName} PayTransferHeaderCreateBySubGroup is start.", _controllerName);
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;

            try
            {
                // ตรวจสอบ duplicate data
                var getSubGroups = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackSubGroupId == claimPayBackSubGroupId).Select(s => s.ClaimPayBackSubGroupId).ToList();
                if (_context.ClaimPayBackSubGroupTransaction.Where(w => getSubGroups.Any(a => a.Equals(w.ClaimPayBackSubGroupId)) && w.IsActive == true).Count() > 0)
                {
                    Log.Information("{_controllerName} PayTransferHeaderCreateBySubGroup ตรวจสอบการทำรายการซ้ำ user มีการกดข้อมูลรายการเดียวกันระบบจะทำการตรวจสอบก่อนทำรายการ", _controllerName);
                    return Json(new { valid = false, message = "รายการนี้อยู่ระหว่างการดำเนินการ" }, JsonRequestBehavior.AllowGet);
                }

                // get list  
                var getClaimSubGroups = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackSubGroupId == claimPayBackSubGroupId && w.IsActive == true).ToList();
                foreach (var get in getClaimSubGroups)
                {
                    var getHotpitalDetail = _context.usp_HospitalAccountDetails_Select(get.HospitalCode).FirstOrDefault();
                    if (getHotpitalDetail.ReceivingBankId == null)
                    {
                        return Json(new { valid = false, message = "รหัสธนาคารไม่ถูกต้อง (" + get.HospitalName + ")" }, JsonRequestBehavior.AllowGet);
                    }

                    if (getHotpitalDetail.ReceivingBankAccountNo == "" || getHotpitalDetail.ReceivingBankAccountNo == null)
                    {
                        return Json(new { valid = false, message = "หมายเลขบัญชีธนาคารไม่ถูกต้อง (" + get.HospitalName + ")" }, JsonRequestBehavior.AllowGet);
                    }

                    if (getHotpitalDetail.ReceivingName == "" || getHotpitalDetail.ReceivingName == null)
                    {
                        return Json(new { valid = false, message = "ชื่อบัญชีธนาคารไม่ถูกต้อง (" + get.HospitalName + ")" }, JsonRequestBehavior.AllowGet);
                    }

                    // gernerate PayListHeaderId
                    Guid generatePayListHeaderId = new Guid(Guid.NewGuid().ToString());

                    // update sub group
                    get.PayListHeaderId = generatePayListHeaderId;
                    get.UpdatedByUserId = userId;
                    get.UpdatedDate = DateTime.Now;
                    _context.Entry(get).State = EntityState.Modified;
                    _context.SaveChanges();
                    Log.Information("{_controllerName} Update 'ClaimPayBackSubGroup' PayListHeaderId = {PayListHeaderId} ", _controllerName, generatePayListHeaderId);

                    // create sub transaction step ที่ 1
                    var getLastRoundNumber = _context.ClaimPayBackSubGroupTransaction.Where(w => w.ClaimPayBackSubGroupId == get.ClaimPayBackSubGroupId).OrderByDescending(O => O.RoundNumber).FirstOrDefault();
                    var createSubGroupTransaction = new ClaimPayBackSubGroupTransaction()
                    {
                        ClaimPayBackSubGroupId = get.ClaimPayBackSubGroupId,
                        ClaimPayBackSubGroupTransactionStatusId = 2,
                        RefCode = null,
                        TransRefNo = null,
                        StatusBank = null,
                        DescriptionEN = null,
                        DescriptionTH = null,
                        TransferDate = null,
                        PayResultStatusId = 0,
                        PayResultDescription = null,
                        TransType = null,
                        RoundNumber = (getLastRoundNumber != null ? getLastRoundNumber.RoundNumber + 1 : 1),
                        PayListHeaderId = generatePayListHeaderId,
                        IsActive = true,
                        CreatedByUserId = userId,
                        CreatedDate = DateTime.Now,
                        UpdatedByUserId = userId,
                        UpdatedDate = DateTime.Now,
                        ReceiverAccountDetail = getHotpitalDetail.BankName + " " + getHotpitalDetail.ReceivingBankAccountNo + " " + getHotpitalDetail.ReceivingName
                    };
                    _context.ClaimPayBackSubGroupTransaction.Add(createSubGroupTransaction);
                    _context.SaveChanges();

                    Log.Information("{_controllerName} ClaimPayBackSubGroupTransaction is successfully.", _controllerName);

                    // publish to paymentTransfer
                    var setModels = new PayTransferAPI.Contract.TempPayListHeaderCreate()
                    {
                        PayListHeaderId = generatePayListHeaderId,
                        PayListHeaderCode = null,
                        SendingBankId = Convert.ToInt32(Properties.Settings.Default.SendingBankId),
                        SendingBankAccountNo = Properties.Settings.Default.SendingBankAccountNo,
                        SendingName = Properties.Settings.Default.SendingName,
                        ReceivingBankId = getHotpitalDetail.ReceivingBankId,
                        ReceivingBankAccountNo = getHotpitalDetail.ReceivingBankAccountNo,
                        ReceivingName = getHotpitalDetail.ReceivingName,
                        TempPayListStatusId = 3,
                        Amount = get.Amount,
                        PayListHeaderSourceTypeId = 4,
                        CreatedByUserId = userId,
                        CreatedDate = DateTime.Now,
                        UpdatedByUserId = userId,
                        UpdatedDate = DateTime.Now,
                        IsSentSMS = false,
                        PhoneNo = null,
                        TempPayListDetailCreate = new List<TempPayListDetailCreate>
                        {
                            new TempPayListDetailCreate
                            {
                                PayListDetailId = new Guid(Guid.NewGuid().ToString()),
                                Amount = get.Amount,
                                PayListDetailCode = get.ClaimPayBackSubGroupCode,
                                PayListHeaderId = generatePayListHeaderId,
                                RefDetail01 = "",
                                RefDetail02 = "",
                                RefDate = DateTime.Now,
                            }
                        }
                    };
                    MvcApplication.bus.Publish<PayTransferAPI.Contract.TempPayListHeaderCreate>(setModels);

                    Log.Information("{_controllerName} Function PayTransferHeaderCreateBySubGroup Publish to contract name {contractName} PayListHeaderId={PayListHeaderId}", _controllerName, "PayTransferAPI.Contract.TempPayListHeaderCreate", generatePayListHeaderId);

                }

            }
            catch (Exception error)
            {
                string errorMessage = error.InnerException == null ? error.Message : error.InnerException.Message;
                Log.Error("{_controllerName} PayTransferHeaderCreateBySubGroup is error {message}.", _controllerName, errorMessage);
                return Json(new { valid = false, message = errorMessage }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { valid = true }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ClaimPayBackRePayTransferMonitor()
        {
            ViewBag.Province = _context.usp_Province_Select(0, 999, null, null, null).ToList();
            ViewBag.dataJsonIds = JsonConvert.SerializeObject(_context.usp_ClaimPayBackSubGroupRePayTransfer_Select(null, 0, 999, null, null, null).Select(s => new { s.ClaimPayBackSubGroupId, s.ClaimPayBackSubGroupTransactionId }).ToList());

            var empCode = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            var env = Properties.Settings.Default.Environment;
            ViewBag.IsRolePermision = _context.RoleFundPayConfig.Where(w => w.IsActive == true && empCode.Contains(w.EmployeeCode) && w.Environment == env).Count() > 0 ? true : false;
            return View();
        }

        [HttpPost]
        public ActionResult GetListRePayTransfer(int? draw = null, int? indexStart = null, int? pageSize = null, string sortField = null, string orderType = null, string search = null)
        {
            var _search = search == "" ? null : search;
            var list = _context.usp_ClaimPayBackSubGroupRePayTransfer_Select(_search, indexStart, pageSize, sortField, orderType, null).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> RePayTransferHeaderCreate(FormRePayTransfersDTO models)
        {
            Log.Information("{_controllerName} PayTransferHeaderCreate is start.", _controllerName);
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;

            try
            {
                // ตรวจสอบข้อมูล ซ่ำกับ กรณี user กดรายการเดียวกัน
                string[] subGroupId = models.dataPublish.Split(',');
                foreach (var getSubGroupId in subGroupId)
                {
                    var converSubGroupId = Convert.ToInt32(getSubGroupId);
                    var getLastRound = await _context.ClaimPayBackSubGroupTransaction.Where(w => w.ClaimPayBackSubGroupId == converSubGroupId && w.IsActive == true)
                                        .OrderByDescending(o => o.ClaimPayBackSubGroupTransactionId)
                                        .FirstOrDefaultAsync();

                    int[] statusValidate = { 2, 3, 5 };
                    if (statusValidate.Any(a => a == getLastRound.ClaimPayBackSubGroupTransactionStatusId))
                    {
                        Log.Information("{_controllerName} PayTransferHeaderCreate ตรวจสอบการทำรายการซ้ำ user มีการกดข้อมูลรายการเดียวกันระบบจะทำการตรวจสอบก่อนทำรายการ", _controllerName);
                        return Json(new { valid = false, message = "รายการนี้อยู่ระหว่างการดำเนินการ" }, JsonRequestBehavior.AllowGet);
                    }
                }

                // set ข้อมูลสำหรับสร้างรายการโอนเงิน
                foreach (var get in subGroupId)
                {
                    int _subGroupId = Convert.ToInt32(get);
                    var getClaimSubGroups = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackSubGroupId == _subGroupId && w.IsActive == true).FirstOrDefault();
                    var getHotpitalDetail = _context.usp_HospitalAccountDetails_Select(getClaimSubGroups.HospitalCode).FirstOrDefault();

                    // generate UUID 
                    Guid generatePayListHeaderId = new Guid(Guid.NewGuid().ToString());

                    // create sub transaction step ที่ 1 เปลี่ยน IsActive เป็น False ของรายการในรอบล่าสุด
                    var getLastRoundNumber = _context.ClaimPayBackSubGroupTransaction.Where(w => w.ClaimPayBackSubGroupId == getClaimSubGroups.ClaimPayBackSubGroupId).OrderByDescending(O => O.RoundNumber).FirstOrDefault();
                    var getLastTransactionInRound = _context.ClaimPayBackSubGroupTransaction
                        .Where(w => w.ClaimPayBackSubGroupId == getClaimSubGroups.ClaimPayBackSubGroupId
                        && w.RoundNumber == getLastRoundNumber.RoundNumber)
                        .OrderByDescending(o => o.ClaimPayBackSubGroupTransactionId)
                        .FirstOrDefault();

                    getLastTransactionInRound.IsActive = false;
                    getLastTransactionInRound.UpdatedByUserId = 1;
                    getLastTransactionInRound.UpdatedDate = DateTime.Now;
                    _context.ClaimPayBackSubGroupTransaction.AddOrUpdate(getLastTransactionInRound);
                    await _context.SaveChangesAsync();

                    var createSubGroupTransaction = new ClaimPayBackSubGroupTransaction()
                    {
                        ClaimPayBackSubGroupId = getClaimSubGroups.ClaimPayBackSubGroupId,
                        ClaimPayBackSubGroupTransactionStatusId = 2,
                        RefCode = null,
                        TransRefNo = null,
                        StatusBank = null,
                        DescriptionEN = null,
                        DescriptionTH = null,
                        TransferDate = null,
                        PayResultStatusId = 0,
                        PayResultDescription = null,
                        TransType = null,
                        RoundNumber = (getLastRoundNumber != null ? getLastRoundNumber.RoundNumber + 1 : 1),
                        PayListHeaderId = generatePayListHeaderId,
                        IsActive = true,
                        CreatedByUserId = userId,
                        CreatedDate = DateTime.Now,
                        UpdatedByUserId = userId,
                        UpdatedDate = DateTime.Now,
                        ReceiverAccountDetail = getHotpitalDetail.BankName + " " + getHotpitalDetail.ReceivingBankAccountNo + " " + getHotpitalDetail.ReceivingName
                    };
                    _context.ClaimPayBackSubGroupTransaction.Add(createSubGroupTransaction);
                    await _context.SaveChangesAsync();

                    // update sub group
                    getClaimSubGroups.PayListHeaderId = generatePayListHeaderId;
                    getClaimSubGroups.UpdatedByUserId = userId;
                    getClaimSubGroups.UpdatedDate = DateTime.Now;
                    _context.ClaimPayBackSubGroup.AddOrUpdate(getClaimSubGroups);
                    await _context.SaveChangesAsync();

                    var setModels = new PayTransferAPI.Contract.TempPayListHeaderCreate()
                    {
                        PayListHeaderId = generatePayListHeaderId,
                        PayListHeaderCode = null, // add new
                        SendingBankId = Convert.ToInt32(Properties.Settings.Default.SendingBankId),
                        SendingBankAccountNo = Properties.Settings.Default.SendingBankAccountNo,
                        SendingName = Properties.Settings.Default.SendingName,
                        ReceivingBankId = getHotpitalDetail.ReceivingBankId,
                        ReceivingBankAccountNo = getHotpitalDetail.ReceivingBankAccountNo,
                        ReceivingName = getHotpitalDetail.ReceivingName,
                        TempPayListStatusId = 3,
                        Amount = getClaimSubGroups.Amount,
                        PayListHeaderSourceTypeId = 4,
                        CreatedByUserId = userId,
                        CreatedDate = DateTime.Now,
                        UpdatedByUserId = userId,
                        UpdatedDate = DateTime.Now,
                        IsSentSMS = false,
                        PhoneNo = null,
                        TempPayListDetailCreate = new List<TempPayListDetailCreate>
                        {
                            new TempPayListDetailCreate
                            {
                                PayListDetailId = new Guid(Guid.NewGuid().ToString()),
                                Amount = getClaimSubGroups.Amount,
                                PayListDetailCode = getClaimSubGroups.ClaimPayBackSubGroupCode,
                                PayListHeaderId = generatePayListHeaderId,
                                RefDetail01 = "",
                                RefDetail02 = "",
                                RefDate = DateTime.Now,
                            }
                        }
                    };

                    // publish to paymentTransfer
                    await MvcApplication.bus.Publish<PayTransferAPI.Contract.TempPayListHeaderCreate>(setModels);

                    Log.Information("{_controllerName} Function PayTransferHeaderCreate Publish to contract name {contractName} PayListHeaderId={PayListHeaderId}", _controllerName, "PayTransferAPI.Contract.TempPayListHeaderCreate", generatePayListHeaderId);
                    Log.Information("TempPayListHeaderCreate is successfully.", _controllerName);
                }
            }
            catch (Exception error)
            {
                string errorMessage = error.InnerException == null ? error.Message : error.InnerException.Message;
                Log.Error("{_controllerName} PayTransferHeaderCreate is error {message}.", _controllerName, errorMessage);
                return Json(new { valid = false, message = errorMessage }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { valid = true }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "อนุมัติไฟล์ตอบกลับบริษัทประกัน"

        [HttpGet]
        public ActionResult Out2ApproveMonitor()
        {
            Log.Debug("{_controllerName} Start Out2ApproveMonitor", _controllerName);
            ViewBag.InsuranceCompany = _context.usp_OrganizeInsurance_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.ClaimGroupType = _context.usp_ClaimType_Select().ToList();
            ViewBag.BillingReceiveStatus = _context.BillingReceiveStatus.Where(w => w.IsActive == true && w.BillingReceiveStatusId != 3).ToList();
            return View("Out2ApproveMonitor");
        }

        [HttpPost]
        public ActionResult GetOut2Approves(int? dateTypeId, DateTime startDate, DateTime endDate, int decisionStatus, int? insuranceCompany, int? cliamType, int draw, int indexStart, int pageSize, string sortField, string orderType, string searchDetail)
        {
            int? _insuranceCompany = (Convert.ToInt32(insuranceCompany) == -1 ? null : insuranceCompany);
            int? _cliamType = (Convert.ToInt32(cliamType) == -1 ? null : cliamType);

            var dataResponse = new List<ApproveMonitor>();
            var getDatas = _context.usp_Out2ApproveMonitor_Select(dateTypeId, startDate, endDate, decisionStatus, _insuranceCompany, _cliamType, indexStart, pageSize, sortField, orderType, searchDetail).ToList();
            foreach (var get in getDatas)
            {
                dataResponse.Add(new ApproveMonitor
                {
                    tempCode = get.tempCode,
                    billingRequestGroup = get.billingRequestGroup,
                    insuranceName = get.insuranceName,
                    amount = (int)get.amount,
                    amountTotal = Convert.ToDecimal(get.amountTotal).ToString("N"),
                    statusName = get.StatusName,
                    receiveDate = get.ReceiveDate,
                    billingDate = Convert.ToDateTime(get.BillingDate),
                    claimType = get.ClaimType,
                    StatusId = (int)get.StatusId
                });
            }

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", 1},
                {"recordsFiltered",1},
                {"data", dataResponse}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Out2ApproveDetail(string tempCode, string billingRequestGroup, int statusId)
        {
            Log.Debug("{_controllerName} Start Out2ApproveDetail", _controllerName);

            var getReasonDetails = _context.ClaimPayBackVerifyReason.Where(w => w.TmpCode == tempCode).ToList();
            var getReasonMaster = _context.ClaimPayBackVerifyInsuranceMaster.Where(w => w.IsActive == true).ToList();
            var getTmpHeader = _context.TmpBillingReceiveResultHeader.FirstOrDefault(f => f.BillingRequestGroupCode == billingRequestGroup && f.IsActive == (statusId != 2 ? false : true));

            var respnseResults = new List<ApproveMonitorDetail>();
            var queryString = from tmp in _context.TmpBillingRequestResult
                              join brrd in _context.BillingRequestResultDetail on tmp.BillingRequestItemCode equals brrd.BillingRequestItemCode
                              join cgd in _context.ClaimHeaderGroupImportDetail on brrd.ClaimHeaderGroupImportDetailId equals cgd.ClaimHeaderGroupImportDetailId
                              where tmp.TmpCode == tempCode && tmp.IsValid == true
                              select new
                              {
                                  TmpBillingRequestResultId = tmp.TmpBillingRequestResultId,
                                  ClaimHeaderGroupCode = cgd.ClaimHeaderGroupCode,
                                  BillingRequestItem = tmp.BillingRequestItemCode,
                                  ClaimCode = cgd.ClaimCode,
                                  Remark = tmp.Remark3,
                                  Amount = tmp.CoverAmount,
                                  PaymentDate = tmp.PaymentDate,
                                  AccountDetail = tmp.BankAccountName + " ธนาคาร" + tmp.BankName + " (" + tmp.BankAccountNumber + ") "
                              };

            foreach (var item in queryString)
            {
                var GetReasonId = getReasonDetails.Where(w => w.TmpBillingRequestResultId == item.TmpBillingRequestResultId).Select(s => s.ClaimPayBackReasonId).ToList();
                var GetReasonString = getReasonMaster.Where(w => GetReasonId.Contains(w.ClaimPayBackReasonId)).Select(s => s.ClaimPayBackReasonName).ToList();
                respnseResults.Add(new ApproveMonitorDetail
                {
                    TmpBillingRequestResultId = item.TmpBillingRequestResultId,
                    ClaimHeaderGroupCode = item.ClaimHeaderGroupCode,
                    BillingRequestItem = item.BillingRequestItem,
                    ClaimCode = item.ClaimCode,
                    Remark = item.Remark,
                    Amount = (decimal)item.Amount,
                    PaymentDate = Convert.ToDateTime(item.PaymentDate).ToString("dd/MM/yyyy"),
                    AccountDetail = item.AccountDetail,
                    StatusId = getTmpHeader.BillingReceiveStatusId,
                    ReasonDetail = string.Join(",", GetReasonString)
                });
            }

            ViewBag.Reason = _context.ClaimPayBackVerifyInsuranceMaster.ToList();
            ViewBag.tempCode = tempCode;
            ViewBag.BillingRequestGroupCode = billingRequestGroup;

            Log.Information("{_controllerName} Out2ApproveDetail done.", _controllerName);
            return View("Out2ApproveDetail", respnseResults);
        }

        [HttpPost]
        public ActionResult Out2ApproveDetail(ApproveDetailForm model)
        {
            Log.Debug("{_controllerName} Start Out2ApproveDetail.", _controllerName);
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;

            try
            {
                // get BillingRequestResultHeader สำหรับชื่อเอกสาร และอื่นๆ
                var getTempResult = _context.TmpBillingRequestResult.FirstOrDefault(w => w.TmpCode == model.tempCode);
                var getResultDetail = _context.BillingRequestResultDetail.FirstOrDefault(a => a.BillingRequestItemCode == getTempResult.BillingRequestItemCode);
                var getResultDetailHeader = _context.BillingRequestResultHeader.FirstOrDefault(f => f.BillingRequestResultHeaderId == getResultDetail.BillingRequestResultHeaderId);

                var VerifyReasonModels = new List<ClaimPayBackVerifyReason>();

                int BillingReceiveStatusId = 0;
                bool tmpHeaderIsActive = false;

                // รายการที่เลือกทั้งหมด
                if (model.isAll)
                {
                    var queryString = _context.TmpBillingRequestResult.Where(w => w.TmpCode == model.tempCode).ToList();
                    foreach (var item in queryString)
                    {
                        string[] reasonMasters = model.reasonMaster[0].Split(',');
                        foreach (var reason in reasonMasters)
                        {
                            VerifyReasonModels.Add(new ClaimPayBackVerifyReason
                            {
                                CreatedByUserId = userId,
                                CreatedDate = DateTime.Now,
                                UpdatedByUserId = userId,
                                UpdatedDate = DateTime.Now,
                                TmpBillingRequestResultId = item.TmpBillingRequestResultId,
                                IsActive = true,
                                TmpCode = model.tempCode,
                                ClaimPayBackReasonId = Convert.ToInt32(reason)
                            });
                            _context.ClaimPayBackVerifyReason.AddRange(VerifyReasonModels);
                        }
                    }

                    _context.SaveChanges();

                    Log.Information("{_controllerName} Function Out2ApproveDetail save data in ClaimPayBackVerifyReason table", _controllerName);

                    Log.Debug("{_controllerName} Function Out2ApproveDetail send email.", _controllerName);

                    // send email ปฏิเสธ
                    bool resultSendMail = Out2ApproveSendEmail(model.tempCode, model.isAll, model.BillingRequestGroupCode, getResultDetailHeader.FileName, false);
                    BillingReceiveStatusId = 4;
                }
                else
                {
                    // รายการอนุมัติ
                    if (model.reasonMaster.All(a => a == null || a == ""))
                    {
                        /*
                         * call store
                         * send email สำหรับรายการที่อนุมัติสำเร็จ
                         * อนุมัติ
                        */

                        bool resultSendMail = Out2ApproveSendEmail(model.tempCode, model.isAll, model.BillingRequestGroupCode, getResultDetailHeader.FileName, true);
                        var resultApprove = _context.usp_BillingRequestResultConfirm_Insert(model.tempCode, getResultDetailHeader.FileName, userId).FirstOrDefault();
                        if (Convert.ToBoolean(resultApprove.IsResult))
                        {
                            BillingReceiveStatusId = 3;
                            tmpHeaderIsActive = true;
                        }
                    }
                    else
                    {
                        // รายการที่ไม่อนุมัติ
                        int count = 0;
                        var queryString = _context.TmpBillingRequestResult.Where(w => w.TmpCode == model.tempCode).ToList();
                        foreach (var item in model.id)
                        {
                            string[] reasonMasters = model.reasonMaster[count].Split(',');
                            foreach (var reason in reasonMasters)
                            {
                                VerifyReasonModels.Add(new ClaimPayBackVerifyReason
                                {
                                    CreatedByUserId = userId,
                                    CreatedDate = DateTime.Now,
                                    UpdatedByUserId = userId,
                                    UpdatedDate = DateTime.Now,
                                    TmpBillingRequestResultId = item,
                                    IsActive = true,
                                    TmpCode = model.tempCode,
                                    ClaimPayBackReasonId = reason == "" ? 0 : Convert.ToInt32(reason)
                                });
                                _context.ClaimPayBackVerifyReason.AddRange(VerifyReasonModels);
                            }

                            count++;
                        }

                        _context.SaveChanges();
                        Log.Information("{_controllerName} Function Out2ApproveDetail save data in ClaimPayBackVerifyReason table", _controllerName);

                        Log.Debug("{_controllerName} Function Out2ApproveDetail send email.", _controllerName);

                        // send email ปฏิเสธ
                        bool resultSendMail = Out2ApproveSendEmail(model.tempCode, model.isAll, model.BillingRequestGroupCode, getResultDetailHeader.FileName, false);
                        if (resultSendMail)
                        {
                            BillingReceiveStatusId = 4;
                        }
                    }
                }

                // update status tmp header
                var getTmpHeaders = _context.TmpBillingReceiveResultHeader.Where(w => w.BillingRequestGroupCode == model.BillingRequestGroupCode && w.TmpCode == model.tempCode).ToList();
                if (getTmpHeaders.Count() > 0)
                {
                    foreach (var getTmpHeader in getTmpHeaders)
                    {
                        getTmpHeader.UpdatedDate = DateTime.Now;
                        getTmpHeader.UpdatedByUserId = userId;
                        getTmpHeader.BillingReceiveStatusId = BillingReceiveStatusId;
                        getTmpHeader.IsActive = tmpHeaderIsActive;
                        //_context.TmpBillingReceiveResultHeader.AddOrUpdate(getTmpHeader);
                        _context.Entry(getTmpHeader).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception error)
            {
                return Json(new { valid = false, message = error.InnerException == null ? error.Message : error.InnerException.Message });
            }

            return Json(new { valid = true });
        }

        private bool Out2ApproveSendEmail(string TmpCode, bool isAll, string billingRequestGroupCode, string fileName, bool IsSuccess)
        {
            Log.Debug("{_controllerName} Function Out2ApproveSendEmail is start.", _controllerName);
            bool isSuccess = true;
            try
            {
                var getTmpresultData = _context.TmpBillingRequestResult.FirstOrDefault(f => f.TmpCode == TmpCode);
                var getBillingGroup = _context.BillingRequestGroup.FirstOrDefault(f => f.BillingRequestGroupCode == billingRequestGroupCode);

                //MailKit Setting
                string hostMailKit = Properties.Settings.Default.MailKitHost;
                int portMailKit = Properties.Settings.Default.MailKitPort;
                string userMailKit = Properties.Settings.Default.MailKitUser;
                string passwordMailKit = Properties.Settings.Default.MailKitPassword;
                string env = Properties.Settings.Default.Environment;

                //Create Mail Message
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("บริษัท สไมล์ ทีพีเอ จำกัด(ในเครือสยามสไมล์) แผนกกองทุนสินไหม", userMailKit));
                message.Subject = "ตรวจสอบรายละเอียดการแจ้งจ่ายค่าสินไหม รอบวางบิลวันที่ " + Convert.ToDateTime(getBillingGroup.BillingDate).ToString("dd/MM/yyyy");

                var getInsuranceCompany = _context.usp_OrganizeInsurance_Select(null, null, 999, null, null, null).Where(w => w.OrganizeId == getBillingGroup.InsuranceCompanyId).FirstOrDefault();
                var getEmailCC = _context.SFTPConfig.Where(x => x.IsActive == true && x.InsuranceCompanyCode == getInsuranceCompany.OrganizeCode).Select(x => x.MailCc).First();
                foreach (string ccAddress in getEmailCC.Split(','))
                {
                    message.Cc.Add(new MailboxAddress(ccAddress, ccAddress));
                }

                string setMessage = string.Empty;
                setMessage += "เรียน " + getBillingGroup.InsuranceCompanyName + " <br/>";
                setMessage += "<br/>";
                setMessage +=
                      "ไฟล์: " + fileName
                    + " BillingRequestGroup: "
                    + billingRequestGroupCode
                    + (IsSuccess == true ? "<br/>" : " โปรดตรวจสอบ และดำเนินการแก้ไข<br/>");

                setMessage += "<br/>";
                setMessage += " รายละเอียด <br/>";

                string reasonMessage = "";
                if (!IsSuccess)
                {
                    if (isAll)
                    {
                        var getReasonMasters = _context.ClaimPayBackVerifyInsuranceMaster.ToList();
                        var getReasonResults = _context.ClaimPayBackVerifyReason.Where(f => f.TmpCode == TmpCode).Select(s => s.ClaimPayBackReasonId).Distinct().ToList();
                        foreach (var claimPayBackReasonId in getReasonResults)
                        {

                            var setStringResult = getReasonMasters.FirstOrDefault(f => f.ClaimPayBackReasonId == claimPayBackReasonId);
                            reasonMessage += "-" + setStringResult.ClaimPayBackReasonName + " " + setStringResult.ClaimPayBackReasonField + ", <br/>";
                        }

                        setMessage += "<span style='color:red'>" + reasonMessage + "</span><br/>";
                    }
                    else
                    {
                        var getReasonMasters = _context.ClaimPayBackVerifyInsuranceMaster.ToList();
                        var getReasonEmail = _context.ClaimPayBackVerifyReason.Where(w => w.TmpCode == TmpCode).ToList();
                        foreach (var get in getReasonEmail.Where(f => f.TmpCode == TmpCode && f.ClaimPayBackReasonId != 0).Select(s => new { s.TmpBillingRequestResultId }).Distinct().ToList())
                        {
                            reasonMessage += "-" + _context.TmpBillingRequestResult.FirstOrDefault(f => f.TmpBillingRequestResultId == get.TmpBillingRequestResultId).BillingRequestItemCode;
                            var getReasonResults = getReasonEmail.Where(f => f.TmpBillingRequestResultId == get.TmpBillingRequestResultId).ToList();
                            int countReason = 1;
                            foreach (var getReasonResult in getReasonResults)
                            {
                                var setStringResult = getReasonMasters.FirstOrDefault(f => f.ClaimPayBackReasonId == getReasonResult.ClaimPayBackReasonId);
                                reasonMessage += " " + setStringResult.ClaimPayBackReasonName + " " + setStringResult.ClaimPayBackReasonField + ", " + (getReasonResults.Count() == countReason ? "<br/>" : "");
                                countReason++;
                            }
                        }

                        setMessage += "<span style='color:red'>" + reasonMessage + "</span><br/>";
                    }
                }
                else
                {
                    setMessage += "ผลตอบกลับใบนำส่ง บ.ประกัน สำเร็จ<br/>";
                }

                setMessage += "<br/>";
                setMessage += "ขอแสดงความนับถือ<br>";
                setMessage += "แผนกกองทุนสินไหม<br> Tel.081-8839597<br>";
                setMessage += "บริษัท สไมล์ ทีพีเอ จำกัด(ในเครือสยามสไมล์)<br>";
                setMessage += "89/6-10 ถ.เฉลิมพงษ์ แขวงสายไหม เขตสายไหม<br> กรุงเทพมหานคร 10220 www.siamsmile.co.th <br>";
                setMessage += "E-mail: claimsfund.tpa@gmail.com <br>";
                setMessage += "Tel. 1434";

                //Create Body Email
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = setMessage;

                //Set Body Email
                message.Body = bodyBuilder.ToMessageBody();

                //Connect MailKit For Send Email
                using (var client = new SmtpClient())
                {
                    client.Connect(hostMailKit, portMailKit, SecureSocketOptions.StartTls); // Use your SMTP server and port
                    client.Authenticate(userMailKit, passwordMailKit); // Your email credentials
                    client.Send(message);
                    client.Disconnect(true);
                }

                Log.Debug("{_controllerName} Function Out2ApproveSendEmail is done.", _controllerName);
            }
            catch (Exception)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        #endregion

        #region Financial transaction report

        [HttpGet]
        public ActionResult FinancialTransactionReport()
        {
            int? branchid = null;

            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();

            var roleMO = new[] { "SmileClaimPayBack_Operation" }; //Operator

            if (rolelist.Intersect(roleMO).Count() > 0)
            {
                branchid = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
            }

            ViewBag.BranchId = branchid;
            ViewBag.ProductGroupId = _context.usp_ProductGroup_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.InsuranceCompany = _context.usp_OrganizeInsuranceForGenerateGroup_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.ClaimGroupType = _context.usp_ClaimGroupType_Select(null, 0, 9999, null, null, null).Where(x => x.ClaimGroupTypeId != 1).ToList();
            return View("FinancialTransactionReport");
        }

        [HttpPost]
        public async Task<ActionResult> ExportFinancialTransaction(string dateFrom, string dateTo, int? productGroupId, int? insuranceCompanyId, int? claimGroupTypeId = null)
        {
            await Task.Yield();
            Session.Remove("ExportFinancialTransactionDownload");
            using (var db = new ClaimPayBackEntities())
            {
                DateTime d_DateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
                DateTime d_DateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));
                productGroupId = productGroupId == -1 ? null : productGroupId;
                insuranceCompanyId = insuranceCompanyId == -1 ? null : insuranceCompanyId;
                claimGroupTypeId = claimGroupTypeId == -1 ? null : claimGroupTypeId;
                try
                {
                        var getReports = db.usp_Report_ClaimPayBackFinancialTransaction_Select(d_DateFrom, d_DateTo, insuranceCompanyId, productGroupId, claimGroupTypeId).Select(s =>
                    new
                    {
                        บริษัทประกันภัย = s.InsuranceCompany_Name,
                        สาขา = s.Branch,
                        กลุ่มแผนประกัน = s.ProductGroupDetailName,
                        จังหวัด = s.Province,
                        สถานพยาบาล = s.Hospital,
                        วันที่ธุรการคีย์รับเคลมออนไลน์ = s.RecordedDate != null ? s.RecordedDate.Value.Date.ToString("dd/MM/yyyy") : null,
                        เลขที่เคลมโอนแยก = s.ClaimCompensate,
                        เลขที่เคลม = s.ClaimNo,
                        COL = s.COL,
                        เลข_บส = s.ClaimGroupCode,
                        จำนวนราย = s.ItemCount,
                        ยอดเงินตาม_บส = s.Amount,
                        ชื่อผู้เอาประกัน = s.CustomerName,
                        ธนาคาร = s.BankName,
                        ชื่อบัญชี = s.BankAccountName,
                        เลขที่บัญชี = s.BankAccountNo,
                        เบอร์โทร = s.PhoneNo,
                        วันที่ส่งการเงิน = s.CreatedDate != null ? s.CreatedDate.Value.Date.ToString("dd/MM/yyyy") : null,
                        เวลาที่ส่งการเงิน = s.CreatedDate != null ? s.CreatedDate.Value.ToString("HH:mm:ss") : null,
                        ประเภทงานเคลม = s.ClaimGroupType,
                        ผู้ส่งอนุมัติ = s.ApprovedUser,
                        ผู้ส่งเบิก = s.CteatedUser,
                        ประเภทการรักษา = s.ClaimAdmitType,
                    }).ToList();

                    if (getReports.Count() > 0)
                    {
                        var stream = new MemoryStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            var workSheet1 = package.Workbook.Worksheets.Add("รายงานส่งการเงิน");
                            workSheet1.Cells.LoadFromCollection(getReports, true);

                            var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                            headerCells1.Style.Font.Bold = true;
                            headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);
                            var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                            allCells1.AutoFilter = true;
                            allCells1.AutoFitColumns();

                            package.Save();

                            stream.Position = 0;
                            Session["ExportFinancialTransactionDownload"] = package.GetAsByteArray();
                        }
                    }
                    else
                    {
                        return Json(new { isError = true, Msg = "ไม่มีข้อมูล" }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { isError = false }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception Ex)
                {
                    Session.Remove("ExportFinancialTransactionDownload");
                    return Json(new { isError = true, Msg = Ex.Message });
                }
            }
        }

        [HttpGet]
        public ActionResult ExportFinancialTransactionDownload()
        {
            //var dataKey = TempData["keyReport"].ToString();
            if (Session["ExportFinancialTransactionDownload"] != null)
            {
                //var period = TempData[dataKey] as string;

                byte[] data = Session["ExportFinancialTransactionDownload"] as byte[];
                string excelName = $"รายงานส่งการเงิน_{DateTime.Now}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpGet]
        public ActionResult PaymentTransferReport()
        {
            int? branchid = null;

            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();

            var roleMO = new[] { "SmileClaimPayBack_Operation" }; //Operator

            if (rolelist.Intersect(roleMO).Count() > 0)
            {
                branchid = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
            }

            ViewBag.BranchId = branchid;
            ViewBag.ProductGroupId = _context.usp_ProductGroup_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.InsuranceCompany = _context.usp_OrganizeInsuranceForGenerateGroup_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.ClaimGroupType = _context.usp_ClaimGroupType_Select(null, 0, 9999, null, null, null).Where(x => x.ClaimGroupTypeId != 1).ToList();
            return View("PaymentTransferReport");
        }

        [HttpPost]
        public async Task<ActionResult> ExportPaymentTransfer(string dateFrom, string dateTo, int? productGroupId, int? insuranceCompanyId, int? claimGroupTypeId = null)
        {
            await Task.Yield();
            Session.Remove("ExportPaymentTransferDownload");
            using (var db = new ClaimPayBackEntities())
            {
                DateTime d_DateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
                DateTime d_DateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));
                productGroupId = productGroupId == -1 ? null : productGroupId;
                insuranceCompanyId = insuranceCompanyId == -1 ? null : insuranceCompanyId;
                claimGroupTypeId = claimGroupTypeId == -1 ? null : claimGroupTypeId;
                try
                {
                    var getReports = db.usp_Report_ClaimPayBackTransfersFinancialTransaction_Select(d_DateFrom, d_DateTo, insuranceCompanyId, productGroupId, claimGroupTypeId).Select(s =>
                    new
                    {
                        บริษัทประกันภัย = s.InsuranceCompany_Name,
                        สาขา = s.Branch,
                        กลุ่มแผนประกัน = s.ProductGroupDetailName,
                        จังหวัด = s.Province,
                        สถานพยาบาล = s.Hospital,
                        เลขที่เคลมโอนแยก = s.ClaimCompensate,
                        เลขที่เคลม = s.ClaimNo,
                        COL = s.COL,
                        เลข_บส = s.ClaimGroupCode,
                        จำนวนราย = s.ItemCount,
                        ยอดเงินตาม_บส = s.Amount,
                        ชื่อผู้เอาประกัน = s.CustomerName,
                        ธนาคาร = s.BankName,
                        ชื่อบัญชี = s.BankAccountName,
                        เลขที่บัญชี = s.BankAccountNo,
                        เบอร์โทร = s.PhoneNo,
                        วันที่ส่งการเงิน = s.SendDate != null ? s.SendDate.Value.Date.ToString("dd/MM/yyyy") : null,
                        วันที่การเงินจ่าย = s.CreatedDate != null ? s.CreatedDate.Value.Date.ToString("dd/MM/yyyy") : null,
                        เวลาที่การเงินจ่าย = s.CreatedDate != null ? s.CreatedDate.Value.ToString("HH:mm:ss") : null,
                        ประเภทงานเคลม = s.ClaimGroupType,
                        ผู้ส่งอนุมัติ = s.ApprovedUser,
                        ผู้ส่งเบิก = s.CteatedUser,
                        ประเภทการรักษา = s.ClaimAdmitType,
                    }).ToList();

                    if (getReports.Count() > 0)
                    {
                        var stream = new MemoryStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            var workSheet1 = package.Workbook.Worksheets.Add("รายงานการโอนเงิน");
                            workSheet1.Cells.LoadFromCollection(getReports, true);

                            var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                            headerCells1.Style.Font.Bold = true;
                            headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);
                            var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                            allCells1.AutoFilter = true;
                            allCells1.AutoFitColumns();

                            package.Save();

                            stream.Position = 0;
                            Session["ExportPaymentTransferDownload"] = package.GetAsByteArray();
                        }
                    }
                    else
                    {
                        return Json(new { isError = true, Msg = "ไม่มีข้อมูล" }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { isError = false }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception Ex)
                {
                    Session.Remove("ExportPaymentTransferDownload");
                    return Json(new { isError = true, Msg = Ex.Message });
                }
            }
        }

        [HttpGet]
        public ActionResult ExportPaymentTransferDownload()
        {
            //var dataKey = TempData["keyReport"].ToString();
            if (Session["ExportPaymentTransferDownload"] != null)
            {
                //var period = TempData[dataKey] as string;

                byte[] data = Session["ExportPaymentTransferDownload"] as byte[];
                string excelName = $"รายงานโอนเงิน_{DateTime.Now}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpGet]
        public ActionResult PaymentTransferDetailReport()
        {
            int? branchid = null;

            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();

            var roleMO = new[] { "SmileClaimPayBack_Operation" }; //Operator

            if (rolelist.Intersect(roleMO).Count() > 0)
            {
                branchid = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
            }

            ViewBag.ClaimGroupType = _context.usp_ClaimGroupType_Select(null, 0, 9999, null, null, null).Where(x => x.ClaimGroupTypeId == 4).ToList();
            return View("PaymentTransferDetailReport");
        }

        [HttpPost]
        public async Task<ActionResult> ExportPaymentTransferDetail(string dateTo, int? claimGroupTypeId = null)
        {
            await Task.Yield();
            Session.Remove("ExportPaymentTransferDetailDownload");
            using (var db = new ClaimPayBackEntities())
            {
                DateTime d_DateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));
                DateTime d_DateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo)).AddDays(1);
                claimGroupTypeId = claimGroupTypeId == -1 ? null : claimGroupTypeId;
                try
                {
                    var getReports = db.usp_Report_ClaimPayBackFinancialTransactionDetail_Select(d_DateFrom, d_DateTo, null, null, claimGroupTypeId).Select(s =>
                    new
                    {
                        สถานพยาบาล = s.Hospital,
                        เลข_บส = s.ClaimGroupCode,
                        เลข_CL = s.ClaimNo,
                        ชื่อ_นามสกุล = s.CustomerName,
                        ยอดเงิน = s.Amount,
                        วันที่ส่งการเงิน = s.SendDate != null ? s.SendDate.Value.Date.ToString("dd/MM/yyyy") : null,
                        เวลาที่ส่งการเงิน = s.SendDate != null ? s.SendDate.Value.ToString("HH:mm:ss") : null,
                        วันที่การเงินจ่าย = s.CreatedDate != null ? s.CreatedDate.Value.Date.ToString("dd/MM/yyyy") : null,
                        เวลาที่การเงินจ่าย = s.CreatedDate != null ? s.CreatedDate.Value.ToString("HH:mm:ss") : null,

                    }).ToList();

                    if (getReports.Count() > 0)
                    {
                        var stream = new MemoryStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            var workSheet1 = package.Workbook.Worksheets.Add("รายงานรายละเอียดการโอนเงิน");
                            workSheet1.Cells.LoadFromCollection(getReports, true);

                            var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                            headerCells1.Style.Font.Bold = true;
                            headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);
                            var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                            allCells1.AutoFilter = true;
                            allCells1.AutoFitColumns();

                            package.Save();

                            stream.Position = 0;
                            Session["ExportPaymentTransferDetailDownload"] = package.GetAsByteArray();
                        }
                    }
                    else
                    {
                        return Json(new { isError = true, Msg = "ไม่มีข้อมูล" }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { isError = false }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception Ex)
                {
                    Session.Remove("ExportPaymentTransferDetailDownload");
                    return Json(new { isError = true, Msg = Ex.Message });
                }
            }
        }

        [HttpGet]
        public ActionResult ExportPaymentTransferDetailDownload()
        {
            //var dataKey = TempData["keyReport"].ToString();
            if (Session["ExportPaymentTransferDetailDownload"] != null)
            {
                //var period = TempData[dataKey] as string;

                byte[] data = Session["ExportPaymentTransferDetailDownload"] as byte[];
                string excelName = $"รายงานรายละเอียดการโอนเงิน_{DateTime.Now}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        #endregion

        #region Financial SMI Transaction Report
        [HttpGet]
        public ActionResult FinancialSMITransactionReport() {
            int? branchid = null;

            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();

            var roleMO = new[] { "SmileClaimPayBack_Operation" }; //Operator

            if (rolelist.Intersect(roleMO).Count() > 0) {
                branchid = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
            }

            ViewBag.BranchId = branchid;
            ViewBag.ProductGroupId = _context.usp_ProductGroup_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.ClaimGroupType = _context.usp_ClaimGroupType_Select(null, 0, 9999, null, null, null).Where(x => x.ClaimGroupTypeId == 2).ToList();
            return View("FinancialSMITransactionReport");
        }
        [HttpPost]
        public async Task<ActionResult> ExportFinancialSMITransaction(string dateFrom, string dateTo, int? productGroupId, int? insuranceCompanyId, int? claimGroupTypeId = null) {
            await Task.Yield();
            Session.Remove("ExportFinancialSMITransactionDownload");
            using (var db = new ClaimPayBackEntities()) {
                DateTime d_DateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
                DateTime d_DateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));
                productGroupId = productGroupId == -1 ? null : productGroupId;
                insuranceCompanyId = 699804; //699804 บริษัทสยามสไมล์ ประกันภัย จำกัด (มหาชน)
                claimGroupTypeId = claimGroupTypeId == -1 ? null : claimGroupTypeId;
                try {
                    var getReports = db.usp_Report_ClaimPayBackFinancialSMITransaction_Select(d_DateFrom, d_DateTo, insuranceCompanyId, productGroupId, claimGroupTypeId).Select(s =>
                    new {
                        บริษัทประกันภัย = s.InsuranceCompany_Name,
                        สาขา = s.Branch,
                        สถานพยาบาล = s.Hospital,
                        กลุ่มแผนประกัน = s.ProductGroupDetailName,
                        ประเภทงานเคลม = s.ClaimGroupType,
                        เลข_บส = s.ClaimGroupCode,
                        จำนวนราย = s.ItemCount,
                        ยอดเงินตาม_บส = s.Amount,
                        เลขที่เคลมโอนแยก = s.ClaimCompensate,
                        เลขที่เคลม = s.ClaimNo,
                        COL = s.COL,
                        จังหวัด = s.Province,
                        ชื่อผู้เอาประกัน = s.CustomerName,
                        ธนาคาร = s.BankName,
                        ชื่อบัญชี = s.BankAccountName,
                        เลขที่บัญชี = s.BankAccountNo,
                        เบอร์โทร = s.PhoneNo,
                        วันที่ส่งการเงิน = s.CreatedDate != null ? s.CreatedDate.Value.Date.ToString("dd/MM/yyyy") : null,
                        เวลาที่ส่งการเงิน = s.CreatedDate != null ? s.CreatedDate.Value.ToString("HH:mm:ss") : null,
                        วันที่_SMI_จ่าย = s.SMIPaid != null ? s.SMIPaid.Value.Date.ToString("dd/MM/yyyy") : "SMI ยังไม่จ่าย",
                        ผู้ส่งอนุมัติ = s.ApprovedUser,
                        ผู้ส่งเบิก = s.CteatedUser,
                        ประเภทการรักษา = s.ClaimAdmitType,
                    }).ToList();

                    if (getReports.Count() > 0) {
                        var stream = new MemoryStream();
                        using (var package = new ExcelPackage(stream)) {
                            var workSheet1 = package.Workbook.Worksheets.Add("รายงานส่งการเงินSMI");
                            workSheet1.Cells.LoadFromCollection(getReports, true);

                            var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                            headerCells1.Style.Font.Bold = true;
                            headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);
                            var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                            allCells1.AutoFilter = true;
                            allCells1.AutoFitColumns();

                            package.Save();

                            stream.Position = 0;
                            Session["ExportFinancialSMITransactionDownload"] = package.GetAsByteArray();
                        }
                    }
                    else {
                        return Json(new { isError = true, Msg = "ไม่มีข้อมูล" }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { isError = false }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception Ex) {
                    Session.Remove("ExportFinancialSMITransactionDownload");
                    return Json(new { isError = true, Msg = Ex.Message });
                }
            }
        }
        [HttpGet]
        public ActionResult ExportFinancialSMITransactionDownload() {
            //var dataKey = TempData["keyReport"].ToString();
            if (Session["ExportFinancialSMITransactionDownload"] != null) {
                //var period = TempData[dataKey] as string;

                byte[] data = Session["ExportFinancialSMITransactionDownload"] as byte[];
                string excelName = $"รายงานส่งการเงิน_SMI_{DateTime.Now}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else {
                return new EmptyResult();
            }
        }
        #endregion

        #region Payment Transfer Detail SMI Report
        [HttpGet]
        public ActionResult PaymentTransferDetailSMIReport() {
            int? branchid = null;

            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();

            var roleMO = new[] { "SmileClaimPayBack_Operation" }; //Operator

            if (rolelist.Intersect(roleMO).Count() > 0) {
                branchid = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
            }

            ViewBag.ClaimGroupType = _context.usp_ClaimGroupType_Select(null, 0, 9999, null, null, null).Where(x => x.ClaimGroupTypeId == 4).ToList();
            return View("PaymentTransferDetailSMIReport");
        }

        [HttpPost]
        public async Task<ActionResult> ExportPaymentTransferDetailSMI(string dateTo, int? claimGroupTypeId = null) {
            await Task.Yield();
            Session.Remove("ExportPaymentTransferDetailSMIDownload");
            using (var db = new ClaimPayBackEntities()) {
                DateTime d_DateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));
                DateTime d_DateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo)).AddDays(1);
                claimGroupTypeId = claimGroupTypeId == -1 ? null : claimGroupTypeId;
                try {
                    var getReports = db.usp_Report_ClaimPayBackFinancialTransactionDetailSMI_Select(d_DateFrom, d_DateTo, null, null, claimGroupTypeId).Select(s =>
                    new {
                        สถานพยาบาล = s.Hospital,
                        เลข_บส = s.ClaimGroupCode,
                        เลข_CL = s.ClaimNo,
                        ชื่อ_นามสกุล = s.CustomerName,
                        ยอดเงิน = s.Amount,
                        วันที่ส่งการเงิน = s.SendDate != null ? s.SendDate.Value.Date.ToString("dd/MM/yyyy") : null,
                        เวลาที่ส่งการเงิน = s.SendDate != null ? s.SendDate.Value.ToString("HH:mm:ss") : null,
                        วันที่การเงินจ่าย = s.CreatedDate != null ? s.CreatedDate.Value.Date.ToString("dd/MM/yyyy") : null,
                        เวลาที่การเงินจ่าย = s.CreatedDate != null ? s.CreatedDate.Value.ToString("HH:mm:ss") : null,

                    }).ToList();

                    if (getReports.Count() > 0) {
                        var stream = new MemoryStream();
                        using (var package = new ExcelPackage(stream)) {
                            var workSheet1 = package.Workbook.Worksheets.Add("รายงานรายละเอียดการโอนเงิน SMI");
                            workSheet1.Cells.LoadFromCollection(getReports, true);

                            var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                            headerCells1.Style.Font.Bold = true;
                            headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);
                            var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                            allCells1.AutoFilter = true;
                            allCells1.AutoFitColumns();

                            package.Save();

                            stream.Position = 0;
                            Session["ExportPaymentTransferDetailSMIDownload"] = package.GetAsByteArray();
                        }
                    }
                    else {
                        return Json(new { isError = true, Msg = "ไม่มีข้อมูล" }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { isError = false }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception Ex) {
                    Session.Remove("ExportPaymentTransferDetailSMIDownload");
                    return Json(new { isError = true, Msg = Ex.Message });
                }
            }
        }

        [HttpGet]
        public ActionResult ExportPaymentTransferDetailSMIDownload() {
            //var dataKey = TempData["keyReport"].ToString();
            if (Session["ExportPaymentTransferDetailSMIDownload"] != null) {
                //var period = TempData[dataKey] as string;

                byte[] data = Session["ExportPaymentTransferDetailSMIDownload"] as byte[];
                string excelName = $"รายงานรายละเอียดการโอนเงิน_SMI_{DateTime.Now}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else {
                return new EmptyResult();
            }
        }
        #endregion
    }
}