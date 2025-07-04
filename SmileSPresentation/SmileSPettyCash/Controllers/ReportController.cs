using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSPettyCash.DataCenterAddressService;
using SmileSPettyCash.Helper;
using SmileSPettyCash.Models;

namespace SmileSPettyCash.Controllers
{
    [Authorization(Roles = "Developer,PettyCash_MO,PettyCash_ACC_01,PettyCash_ACC_02")]
    public class ReportController:Controller
    {
        #region dbCon

        private readonly PettyCashEntities _context;
        private readonly AddressServiceClient _DCService;
        private static readonly CultureInfo _dtEn = new CultureInfo("en-US");
        private static readonly CultureInfo _dtTh = new CultureInfo("th-TH");

        public ReportController()

        {
            _context = new PettyCashEntities();
            _DCService = new AddressServiceClient();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion dbCon

        #region View

        public ActionResult TransactionReport()
        {
            //check branch and roles
            var userName = GlobalFunction.GetLoginDetail(HttpContext).UserName;
            var branchId = GlobalFunction.GetLoginDetail(HttpContext).Branch_ID;
            var roles = new SSOService.SSOServiceClient().GetRoleByUserName(userName);
            var lstRoles = roles.Split(',').ToList();
            var isSelectBranch = false;
            //loop check roles
            foreach(var itm in lstRoles)
            {
                if(itm == "PettyCash_ACC_01" || itm == "PettyCash_ACC_02" || itm == "Developer")
                {
                    isSelectBranch = true;
                    ViewBag.isSelectBranch = true;
                    break;
                }
                else
                {
                    isSelectBranch = false;
                    ViewBag.isSelectBranch = false;
                }
            }

            //return result
            if(isSelectBranch)
            {
                //get branch list
                var branchList = _DCService.GetBranch(null).ToList();
                var itemToRemove = branchList.Single(x => x.Branch_ID == 1);
                branchList.Remove(itemToRemove);

                branchList.Add(new DataCenterAddressService.usp_Branch_Select_Result() { Branch_ID = 1,BranchDetail = "---ทั้งหมด---" });
                ViewBag.branchList = branchList;
            }
            else
            {
                //get branch list
                var branchList = _DCService.GetBranch(branchId).ToList();
                ViewBag.branchList = branchList;
            }

            return View();
        }

        public ActionResult CloseDayReport()
        {
            //check branch and roles
            var userName = GlobalFunction.GetLoginDetail(HttpContext).UserName;
            var branchId = GlobalFunction.GetLoginDetail(HttpContext).Branch_ID;
            var roles = new SSOService.SSOServiceClient().GetRoleByUserName(userName);
            var lstRoles = roles.Split(',').ToList();
            var isSelectBranch = false;
            //loop check roles
            foreach(var itm in lstRoles)
            {
                if(itm == "PettyCash_ACC_01" || itm == "PettyCash_ACC_02" || itm == "Developer")
                {
                    isSelectBranch = true;
                    ViewBag.isSelectBranch = true;
                    break;
                }
                else
                {
                    isSelectBranch = false;
                    ViewBag.isSelectBranch = false;
                }
            }

            //return result
            if(isSelectBranch)
            {
                //get branch list
                var branchList = _DCService.GetBranch(null).ToList();
                var itemToRemove = branchList.Single(x => x.Branch_ID == 1);
                branchList.Remove(itemToRemove);

                branchList.Add(new DataCenterAddressService.usp_Branch_Select_Result() { Branch_ID = 1,BranchDetail = "---ทั้งหมด---" });
                ViewBag.branchList = branchList;
            }
            else
            {
                //get branch list
                var branchList = _DCService.GetBranch(branchId).ToList();
                ViewBag.branchList = branchList;
            }

            return View();
        }

        public ActionResult PettycashTransactionReport()
        {
            //check branch and roles
            var userName = GlobalFunction.GetLoginDetail(HttpContext).UserName;
            var branchId = GlobalFunction.GetLoginDetail(HttpContext).Branch_ID;
            var roles = new SSOService.SSOServiceClient().GetRoleByUserName(userName);
            var lstRoles = roles.Split(',').ToList();
            var isSelectBranch = false;
            //loop check roles
            foreach(var itm in lstRoles)
            {
                if(itm == "PettyCash_ACC_01" || itm == "PettyCash_ACC_02" || itm == "Developer")
                {
                    isSelectBranch = true;
                    ViewBag.isSelectBranch = true;
                    break;
                }
                else
                {
                    isSelectBranch = false;
                    ViewBag.isSelectBranch = false;
                }
            }

            //return result
            if(isSelectBranch)
            {
                //get branch list
                var branchList = _DCService.GetBranch(null).ToList();
                var itemToRemove = branchList.Single(x => x.Branch_ID == 1);
                branchList.Remove(itemToRemove);

                branchList.Add(new DataCenterAddressService.usp_Branch_Select_Result() { Branch_ID = 1,BranchDetail = "---ทั้งหมด---" });
                ViewBag.branchList = branchList;
            }
            else
            {
                //get branch list
                var branchList = _DCService.GetBranch(branchId).ToList();
                ViewBag.branchList = branchList;
            }

            return View();
        }

        #endregion View

        #region api

        [HttpPost]
        public JsonResult GetTransactionReport_DT(int? branchId,
                                        string dateTo,
                                        string dateForm,
                                        int? draw = null,
                                        int? indexStart = null,
                                        int? pettyCashTransactionTypeStatusId = null,
                                        int? pageSize = null,
                                        string sortField = null,
                                        string orderType = null,
                                        string search = null)
        {
            if(branchId == 1)
            {
                branchId = null;
            }

            var dateToConvert = DateTime.ParseExact(dateTo,"dd/MM/yyyy",_dtTh);
            var dateFormConvert = DateTime.ParseExact(dateForm,"dd/MM/yyyy",_dtTh);
            //get result
            var result = _context.usp_Report_Transfer_Select(branchId,dateFormConvert,dateToConvert,indexStart,pageSize,sortField,orderType,search).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCloseDayHeader_DT(int? branchId,
                                        string dateTo,
                                        string dateForm,
                                        int? draw = null,
                                        int? indexStart = null,
                                        int? pettyCashTransactionTypeStatusId = null,
                                        int? pageSize = null,
                                        string sortField = null,
                                        string orderType = null,
                                        string search = null)
        {
            if(branchId == 1)
            {
                branchId = null;
            }

            var dateToConvert = DateTime.ParseExact(dateTo,"dd/MM/yyyy",_dtTh);
            var dateFormConvert = DateTime.ParseExact(dateForm,"dd/MM/yyyy",_dtTh);

            var result = _context.usp_Report_PettyCashXPettyCashDaily_Select(branchId,null,dateFormConvert,dateToConvert,
                indexStart,pageSize,sortField,orderType,search).ToList();
            var dt = new Dictionary<string,object>
             {
                 {"draw", draw },
                 {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                 {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                 {"data", result.ToList()}
             };
            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCloseDayDetail_DT(int pcDailyId,int? draw = null,int? indexStart = null,int? pageSize = null,
            string sortField = null,string orderType = null,string search = null)
        {
            var result = _context.usp_PettyCashXPettyCashDailyDetail_Select(pcDailyId,indexStart,pageSize,sortField,
                orderType,search).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void ExportExcelFile(int? branchId,
                                        string dateForm,
                                        string dateTo
                                        )
        {
            if(branchId == 1)
            {
                branchId = null;
            }
            var dateToConvert = DateTime.ParseExact(dateTo,"dd/MM/yyyy",_dtTh);
            var dateFormConvert = DateTime.ParseExact(dateForm,"dd/MM/yyyy",_dtTh);
            var result = _context.usp_Report_PettyCashXPettyCashDailyDetail_Select(branchId,null,
                dateFormConvert,dateToConvert).Select(x => new
                {
                    วันที่ปิดยอด = x.PettyCashDailyDate_String,
                    Code = x.PettyCashTransactionCode,
                    สาขา = x.Branch,
                    ประเภท = x.PettyCashTransactionGroup,
                    เลขที่เอกสาร = x.DocumentCode,
                    เลขที่ใบเสร็จ = x.BillBook,
                    ประเภทค่าใช้จ่าย = x.PettyCashTransactionType,
                    รายละเอียดเพิ่มเติม = x.PettyCashTransactionTypeDescription,
                    จำนวนเงิน = x.Amount,
                    คงเหลือ = x.Balance,
                    หมายเหตุ = x.Remark
                }).ToList();

            GlobalFunction.ExportDatatableToExcel(HttpContext,result,"รายงานปิดยอดสาขา","รายงานปิดยอดสาขา");
        }

        [HttpGet]
        public ActionResult GetTransferDetailById(int transferId)
        {
            var result = _context.usp_Transfer_ById_Select(transferId).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        //get transfer detail
        [HttpGet]
        public JsonResult GetTransferDetail_DT(int transferId,int? draw = null,int? indexStart = null,
            int? pageSize = null,string sortField = null,string orderType = null,string search = null)
        {
            var result = _context.usp_Payment_Select(transferId,null,indexStart,pageSize,sortField,orderType,search).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void ExportExcel_TransferDetail(int? branchId,
                                        string dateForm,
                                        string dateTo)
        {
            if(branchId == 1)
            {
                branchId = null;
            }
            var dateToConvert = DateTime.ParseExact(dateTo,"dd/MM/yyyy",_dtTh);
            var dateFormConvert = DateTime.ParseExact(dateForm,"dd/MM/yyyy",_dtTh);
            var result = _context.usp_Report_TransferDetail_Select(branchId,
                dateFormConvert,dateToConvert).Select(x => new
                {
                    รหัสการโอน = x.TransferCode,
                    สาขา = x.Branch,
                    ชื่อบัญชีผู้รับโอน = x.TransferBankAccountName,
                    เลขที่บัญชีผู้รับโอน = x.TransferBankAccountNo,
                    จำนวนเงินที่โอน = x.TransferAmount,
                    วันที่โอน = x.TransferDate_String,
                    วันที่ตั้งเบิก = x.RequisitionHeaderCreatedDate_String,
                    รหัสรายการ = x.PettyCashTransactionCode,
                    ประเภท = x.PettyCashTransactionGroup,
                    ประเภทค่าใช้จ่าย = x.PettyCashTransactionType,
                    รายละเอียดเพิ่มเติม = x.PettyCashTransactionTypeDescription,
                    จำนวนเงิน = x.PettyCashTransactionAmount,
                }).ToList();

            GlobalFunction.ExportDatatableToExcel(HttpContext,result,"รายงานการแจ้งโอน","รายงานการแจ้งโอน");
        }

        [HttpPost]
        public JsonResult GetPettycashTransferDetail_DT(int? branchId,
                                        string dateForm,
                                        string dateTo,
                                        int? draw = null,
                                        int? indexStart = null,
                                        int? pageSize = null,
                                        string sortField = null,
                                        string orderType = null,
                                        string search = null)
        {
            if(branchId == 1)
            {
                branchId = null;
            }

            var dateToConvert = DateTime.ParseExact(dateTo,"dd/MM/yyyy",_dtTh);
            var dateFormConvert = DateTime.ParseExact(dateForm,"dd/MM/yyyy",_dtTh);
            var result = _context.usp_Report_PettyCashTransaction_Select(
                branchId,dateFormConvert,dateToConvert,indexStart,pageSize,sortField,orderType,search).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void ExportExcel_PettycashTransaction(int? branchId,
                                        string dateForm,
                                        string dateTo)
        {
            if(branchId == 1)
            {
                branchId = null;
            }
            var dateToConvert = DateTime.ParseExact(dateTo,"dd/MM/yyyy",_dtTh);
            var dateFormConvert = DateTime.ParseExact(dateForm,"dd/MM/yyyy",_dtTh);
            var result = _context.usp_Report_PettyCashTransaction_Select(branchId,
                dateFormConvert,dateToConvert,null,9999,null,null,null).Select(x => new
                {
                    วันที่สร้างรายการ = x.PettyCashTransactionCreatedDate_String,
                    สาขา = x.Branch,
                    เลขที่เอกสาร = x.PettyCashTransactionCode,
                    ประเภทค่าใช้จ่าย = x.PettyCashTransactionType,
                    รายละเอียด = x.PettyCashTransactionTypeDescription,
                    จำนวนเงิน = x.PettyCashTransactionAmount,
                    เลขที่ใบเสร็จ = x.PettyCashTransactionBillBook,
                    วันที่ใบเสร็จ = x.PettyCashTransactionBillDate_String,
                    หมายเหตุ = x.PettyCashTransactionRemark,
                    สถานะ = x.PettyCashXPettyCashDailyStatus
                }).ToList();

            GlobalFunction.ExportDatatableToExcel(HttpContext,result,"รายงานการเบิกจ่าย","รายงานการเบิกจ่าย");
        }

        #endregion api
    }
}