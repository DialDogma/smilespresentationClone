using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSPettyCash.Helper;
using SmileSPettyCash.Models;
using System.Text;
using Microsoft.Ajax.Utilities;
using SmileSPettyCash.DataCenterAddressService;

namespace SmileSPettyCash.Controllers
{
    [Authorization(Roles = "Developer,PettyCash_MO")]
    public class PettyCashController:Controller
    {
        #region dbCon

        private readonly PettyCashEntities _context;
        private static readonly CultureInfo _dtEn = new CultureInfo("en-US");
        private static readonly CultureInfo _dtTh = new CultureInfo("th-TH");

        public PettyCashController()

        {
            _context = new PettyCashEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion dbCon

        #region View

        public ActionResult ManagePettyCashTransaction()
        {
            //get user id
            var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            var branch = GlobalFunction.GetLoginDetail(HttpContext).Branch_ID;
            //generate petty cash id(fix petty cash type)
            //var user = 1;
            //var branch = 70;
            var pettyCash = _context.usp_PettyCash_Select(branch,2).FirstOrDefault();
            ViewBag.pettyCashId = pettyCash.PettyCashId;
            ViewBag.updatedDate = pettyCash.UpdatedDate?.ToString("dd/MM/yyyy HH:mm:ss");
            ViewBag.balance = pettyCash.Balance?.ToString("N2");
            ViewBag.balanceInt = pettyCash.Balance;
            ViewBag.UserId = user;

            return View();
        }

        public ActionResult AddTransaction(int? ptcTransactionId)
        {
            //get user id real
            var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            //var user = 1;
            //get branch real
            var branch = GlobalFunction.GetLoginDetail(HttpContext).Branch_ID;
            //var branch = 70;
            //generate petty cash id(fix petty cash type)
            var pettyCash = _context.usp_PettyCash_Select(branch,2).FirstOrDefault();
            ViewBag.pettyCashId = pettyCash.PettyCashId;

            //check if not edit
            if(ptcTransactionId == null)
            {
                //get doc data
                var docResult = _context.usp_Document_Insert(10,user).ToList();
                ViewBag.docId = docResult[0].DocumentId;
                ViewBag.docLink = docResult[0].UrlLinkScanSSSDoc;
                ViewBag.docCode = docResult[0].DocumentCode;
                ViewBag.amount = 0.00;
                ViewBag.isUpdate = "false";
            }
            else
            {
                //for edit
                var paymentResult = _context.usp_PettyCashTransactionById_Select(ptcTransactionId).FirstOrDefault();
                ViewBag.ptcTranId = ptcTransactionId;
                ViewBag.ttSelected = paymentResult.PettyCashTransactionTypeId;
                ViewBag.ttDescription = paymentResult.PettyCashTransactionTypeDescription;
                var convertTime = DateTime.ParseExact(paymentResult.BillDate.ToString("dd/MM/yyyy HH:mm:ss"),"dd/MM/yyyy HH:mm:ss",_dtTh);
                ViewBag.billDate = convertTime.ToString("yyyy,MM,dd",_dtTh);
                ViewBag.billTime = convertTime.ToString("HH,mm,ss",_dtTh);
                ViewBag.billBook = paymentResult.BillBook;
                ViewBag.amount = paymentResult.Amount;
                ViewBag.remark = paymentResult.Remark;
                ViewBag.docId = paymentResult.DocumentId;
                ViewBag.docLink = paymentResult.UrlLinkOpenSSSDoc;
                ViewBag.docCode = paymentResult.DocumentCode;
                ViewBag.organizeId = paymentResult.OrganizeId;
                ViewBag.isGroup = paymentResult.PettyCashXPettyCashDailyId;
                ViewBag.isUpdate = "true";
                //for edit
            }

            //get payment type
            var paymentType = _context.usp_PettyCashTransactionType_Select(2).ToList();
            var branchList = _context.usp_Branch_Select(null,null,null,9999,null,null,null).ToList();
            ViewBag.PaymentType = paymentType;
            ViewBag.branchList = branchList;
            ViewBag.branchCurrentUser = branch;
            ViewBag.UserId = user;

            return View();
        }

        public ActionResult AddPayment(int? ptcTransactionId)
        {
            //get user id
            var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            var branch = GlobalFunction.GetLoginDetail(HttpContext).Branch_ID;
            //generate petty cash id(fix petty cash type)
            //var user = 1;
            //var branch = 70;
            var pettyCash = _context.usp_PettyCash_Select(branch,2).ToList();
            ViewBag.pettyCashId = pettyCash[0].PettyCashId;
            //get company list
            ViewBag.CompanyList = _context.usp_Organize_Select(6,0,99,null,null,null).ToList();
            //get paymentType
            ViewBag.paymentTypeList = _context.usp_PaymentType_Select(null,null,999,null,null,null).ToList();

            //check if not for edit
            if(ptcTransactionId == null)
            {
                //create doc data
                var docResult = _context.usp_Document_Insert(10,user).ToList();
                ViewBag.docId = docResult[0].DocumentId;
                ViewBag.docLink = docResult[0].UrlLinkScanSSSDoc;
                ViewBag.docCode = docResult[0].DocumentCode;
                ViewBag.amount = 0.00;
                ViewBag.isUpdate = "false";
            }
            else
            {
                //for edit
                var paymentResult = _context.usp_PettyCashTransactionById_Select(ptcTransactionId).FirstOrDefault();
                ViewBag.ptcTranId = ptcTransactionId;
                ViewBag.ttSelected = paymentResult.PettyCashTransactionTypeId;
                ViewBag.ttDescription = paymentResult.PettyCashTransactionTypeDescription;
                var convertTime = DateTime.ParseExact(paymentResult.BillDate.ToString("dd/MM/yyyy HH:mm:ss"),"dd/MM/yyyy HH:mm:ss",_dtTh);
                ViewBag.billDate = convertTime.ToString("MM/dd/yyyy",_dtEn);
                ViewBag.billTime = convertTime.ToString("HH:mm:ss");
                ViewBag.billBook = paymentResult.BillBook;
                ViewBag.amount = paymentResult.Amount;
                ViewBag.remark = paymentResult.Remark;
                ViewBag.docId = paymentResult.DocumentId;
                ViewBag.docLink = paymentResult.UrlLinkOpenSSSDoc;
                ViewBag.docCode = paymentResult.DocumentCode;
                ViewBag.organizeId = paymentResult.OrganizeId;
                ViewBag.isGroup = paymentResult.PettyCashXPettyCashDailyId;
                ViewBag.isUpdate = "true";
            }

            //get payment type
            var paymentType = _context.usp_PettyCashTransactionType_Select(3).ToList();
            var branchList = _context.usp_Branch_Select(null,null,null,9999,null,null,null).ToList();
            ViewBag.PaymentType = paymentType;
            ViewBag.branchList = branchList;
            ViewBag.branchCurrentUser = branch;
            ViewBag.UserId = user;

            return View();
        }

        public ActionResult EditNoPassPaymentManage()
        {
            //get user id
            var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            //get user id
            //var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            var branch = GlobalFunction.GetLoginDetail(HttpContext).Branch_ID;
            //generate petty cash id(fix petty cash type)
            // var user = 1;
            //var branch = 70;
            var pettyCash = _context.usp_PettyCash_Select(branch,2).ToList();
            ViewBag.pettyCashId = pettyCash[0].PettyCashId;
            ViewBag.UserId = user;
            return View();
        }

        public ActionResult CloseDayTransaction()
        {
            //get user id
            var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            var branch = GlobalFunction.GetLoginDetail(HttpContext).Branch_ID;
            //generate petty cash id(fix petty cash type)
            //var user = 1;
            //var branch = 70;
            var pettyCash = _context.usp_PettyCash_Select(branch,2).FirstOrDefault();
            ViewBag.pettyCashId = pettyCash.PettyCashId;
            ViewBag.updatedDate = pettyCash.UpdatedDate?.ToString("dd/MM/yyyy HH:mm:ss");
            ViewBag.balance = pettyCash.Balance?.ToString("N2");
            ViewBag.balanceInt = pettyCash.Balance;

            return View();
        }

        //หน้าจอตั้งเบิกเงินสดย่อย
        public ActionResult Withdrawal()
        {
            var loginDetail = GlobalFunction.GetLoginDetail(HttpContext);
            var pettyCashType = 2;// 2 = เงินสดย่อย
            var pettyCash = _context.usp_PettyCash_Select(loginDetail.Branch_ID,pettyCashType).FirstOrDefault();

            var withdrawAmount = _context.usp_ProgramConfig_PettyCash_WithdrawAmount_Select().FirstOrDefault();

            ViewBag.WithdrawAmount = withdrawAmount.Value;

            ViewBag.PettyCashId = pettyCash.PettyCashId;

            return View();
        }

        //หน้าจอเช็คสถานะตั้งเบิก
        public ActionResult WithdrawalNoPass()
        {
            var loginDetail = GlobalFunction.GetLoginDetail(HttpContext);
            var pettyCashType = 2;// 2 = เงินสดย่อย
            var pettyCash = _context.usp_PettyCash_Select(loginDetail.Branch_ID,pettyCashType).FirstOrDefault();

            ViewBag.PettyCashId = pettyCash.PettyCashId;

            return View();
        }

        //หน้าจอพิมพ์ตั้งเบิก
        public ActionResult CashWithdraw(int transferId)
        {
            var trResult = _context.usp_Payment_Select(transferId,null,null,9999,null,null,null).ToList();
            var printResult = _context.usp_TransferForPrintHeader_ById_Select(transferId).FirstOrDefault();
            var empDetail = GlobalFunction.GetLoginDetail(HttpContext);

            //company header
            ViewBag.CompanyHeader = printResult.Company;
            ViewBag.CompanyDetail_1 = printResult.CompanyAddressDetail1_1;
            ViewBag.CompanyDetail_2 = printResult.CompanyAddressDetail1_2;
            ViewBag.CompanyDetail_3 = "เลขที่ผู้เสียภาษี " + printResult.CompanyTaxNumber;
            //Voucher Header
            ViewBag.Voucher_branch = empDetail.BranchDetail;
            ViewBag.Voucher_department = empDetail.DepartmentDetail;
            ViewBag.Voucher_date = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.Voucher_No = printResult.TransferCode;
            //Voucher owner
            ViewBag.Owner_name = empDetail.FirstName + " " + empDetail.LastName;
            ViewBag.Owner_code = empDetail.UserName;

            //List 1
            ViewBag.List01_detail = trResult[0].PettyCashTransactionType + '(' + trResult[0].PettyCashTransactionTypeDescription + ')';
            ViewBag.List01_debit = trResult[0].PaymentAmount;
            ViewBag.List01_credit = "";
            if(trResult.Count >= 2)
            {
                //List 2
                ViewBag.List02_detail = trResult[1].PettyCashTransactionType + '(' + trResult[1].PettyCashTransactionTypeDescription + ')';
                ViewBag.List02_debit = trResult[1].PaymentAmount;
                ViewBag.List02_credit = "";
            }
            if(trResult.Count >= 3)
            {
                //List 3
                ViewBag.List03_detail = trResult[2].PettyCashTransactionType + '(' + trResult[2].PettyCashTransactionTypeDescription + ')';
                ViewBag.List03_debit = trResult[2].PaymentAmount;
                ViewBag.List03_credit = "";
            }
            if(trResult.Count >= 4)
            {
                //List 4
                ViewBag.List04_detail = trResult[3].PettyCashTransactionType + '(' + trResult[3].PettyCashTransactionTypeDescription + ')';
                ViewBag.List04_debit = trResult[3].PaymentAmount;
                ViewBag.List04_credit = "";
            }
            if(trResult.Count >= 5)
            {
                //List 5
                ViewBag.List05_detail = trResult[4].PettyCashTransactionType + '(' + trResult[4].PettyCashTransactionTypeDescription + ')';
                ViewBag.List05_debit = trResult[4].PaymentAmount;
                ViewBag.List05_credit = "";
            }

            //List result
            var sumAmount = trResult.Sum(x => x.PaymentAmount);
            ViewBag.ListResult_debit = "0";
            ViewBag.ListResult_credit = sumAmount;
            ViewBag.ListResult_thai = GlobalFunction.ThaiBahtText(sumAmount.ToString());
            ViewBag.ListResult_total_debit = sumAmount;
            ViewBag.ListResult_total_credit = sumAmount;
            //payment
            ViewBag.Payment_bank = printResult.Bank;
            ViewBag.Payment_no = printResult.BankAccountNo;
            ViewBag.Payment_name = printResult.BankAccountName;
            ViewBag.Payment_branch = "";
            ViewBag.Payment_cash = printResult.Amount;

            return View();
        }

        public ActionResult WaitScanWithdraw()
        {
            var branch = GlobalFunction.GetLoginDetail(HttpContext).Branch_ID;
            ViewBag.branch = branch;

            return View();
        }

        #endregion View

        #region API

        [HttpPost]
        public JsonResult InsertTransaction(int pctId,
                                            int pctTypeId,
                                            string pctDescription,
                                            int? organizeId,
                                            string amount,
                                            string billBook,
                                            string billDate,
                                            string remark,
                                            int documentId)
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            var newAmount = amount.Replace(",","");
            var newDecimal = Convert.ToDecimal(newAmount);
            //var user = 1;

            DateTime billDateConvert = DateTime.ParseExact(billDate,"dd/MM/yyyy HH:mm:ss",_dtTh);

            var dbresult = _context.usp_PettyCashTransaction_Insert(pctId,
                                                                    pctTypeId,
                                                                    pctDescription,
                                                                    organizeId,
                                                                    newDecimal,
                                                                    billBook,
                                                                    billDateConvert,
                                                                    remark,
                                                                    documentId,
                                                                    user).ToList();

            var result = new { boolResult = dbresult[0].IsResult,textResult = dbresult[0].Msg };

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateRejectRequisition(int? pettyCashTransactionId)
        {
            var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

            var result = _context.usp_RejectRequisition_Update(pettyCashTransactionId,userId).FirstOrDefault();
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertRequisitionDetail(int? pettyCashId,string[] pettyCashTransactionIdList)
        {
            var newStr = string.Join(",",pettyCashTransactionIdList);
            var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

            var result = _context.usp_RequisitionDetail_Insert(pettyCashId,newStr,userId).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update Payment
        /// </summary>
        /// <param name="paymentId">Id</param>
        /// <param name="bankId">ไอดี ธนาคาร</param>
        /// <param name="bankAccountNo">เลขบัญชี</param>
        /// <param name="bankAccountName">ชื่อบัญชี</param>
        /// <param name="amount">ยอดโอน</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdatePayment(int? paymentId,int? bankId,string bankAccountNo,string bankAccountName,decimal amount)
        {
            var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            var paymentType = 2;  // 2 = ธนาคาร

            var result = _context.usp_Payment_Update(paymentId,bankId,bankAccountNo,bankAccountName,amount,userId,paymentType).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateTransaction(int pctId,
                                            int pctTypeId,
                                            string pctDescription,
                                            int? organizeId,
                                            string amount,
                                            string billBook,
                                            string billDate,
                                            string remark)
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            var newAmount = amount.Replace(",","");
            var newDecimal = Convert.ToDecimal(newAmount);
            //var user = 1;
            DateTime billDateConvert = DateTime.ParseExact(billDate,"dd/MM/yyyy HH:mm:ss",_dtTh);

            var dbresult = _context.usp_PettyCashTransaction_Update(pctId,
                pctTypeId,
                pctDescription,
                organizeId,
                newDecimal,
                billBook,
                billDateConvert,
                remark,
                user).ToList();

            var result = new { boolResult = dbresult[0].IsResult,textResult = dbresult[0].Msg };

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPettyCashDT(int pettyCashId,
                                        int? draw = null,
                                        int? indexStart = null,
                                        int? pettyCashTransactionTypeStatusId = null,
                                        int? pageSize = null,
                                        string sortField = null,
                                        string orderType = null,
                                        string search = null)
        {
            var result = _context.usp_PettyCashTransaction_Select(pettyCashId,
                                                                  pettyCashTransactionTypeStatusId,
                                                                  indexStart,
                                                                  pageSize,
                                                                  sortField,
                                                                  orderType,
                                                                  search).ToList();

            //var balanceResult = result.Sum(x => Convert.ToInt32(x.MultiplierResult));
            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        //get branch book from sub branch id
        [HttpGet]
        public JsonResult GetBank()
        {
            var result = _context.usp_Organize_Select(5,null,9999,null,null,null).ToList();
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        //get branch book from sub branch id
        [HttpGet]
        public JsonResult GetBranchBook(int subBranchId)
        {
            var result = _context.usp_BranchXBankAccount_Select(null,subBranchId,null,null,9999,null,null,null).ToList();
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBranchBookDetail(int branchXbookId)
        {
            var result = _context.usp_BranchXBankAccount_Select(branchXbookId,null,null,null,null,null,null,null).FirstOrDefault();
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDocCount(string docCode)
        {
            var result = _context.usp_CountDocument(docCode).FirstOrDefault();
            if(result == null)
            {
                result = 0;
            }
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertCloseDayTransaction(int pettyCashId,int pettyCashTransactionStatusId,int c_1000_00,
            int c_0500_00,int c_0100_00,int c_0050_00,int c_0020_00,int c_0010_00,int c_0005_00,int c_0002_00,
            int c_0001_00,int c_0000_50,int c_0000_25)
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            //var user = 1;

            var result = _context.usp_PettyCashXPettyCashDaily_Insert(pettyCashId,pettyCashTransactionStatusId,c_1000_00,
                c_0500_00,c_0100_00,c_0050_00,c_0020_00,c_0010_00,c_0005_00,c_0002_00,c_0001_00,c_0000_50,c_0000_25,user).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteTransaction(int pctTranId)
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            //var user = 1;
            var result = _context.usp_PettyCashTransaction_Delete(pctTranId,user).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetPettyCashTransactionForRequisition(int? draw,
                                                                            int? pettyCashId,
                                                                            int? organizeCompanyId,
                                                                            int? indexStart,
                                                                            int? pageSize,
                                                                            string sortField,
                                                                            string orderType,
                                                                            string search)
        {
            var result = _context.usp_PettyCashTransaction_ForRequisition_Select(pettyCashId,
                                                                                 organizeCompanyId,
                                                                                 indexStart,
                                                                                 pageSize,
                                                                                 sortField,
                                                                                 orderType,
                                                                                 search).ToList();

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
        public ActionResult GetRejectRequisition(int? draw,int? pettyCashId,int? indexStart,int? pageSize,string sortField,string orderType,string search)
        {
            var result = _context.usp_RejectRequisition_Select(pettyCashId,indexStart,pageSize,sortField,orderType,search).ToList();
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
        public ActionResult GetPettyCashTransaction(int? pettyCashTransactionId)
        {
            var result = _context.usp_PettyCashTransactionById_Select(pettyCashTransactionId).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetTransferDetailById(int transferId)
        {
            var result = _context.usp_Transfer_ById_Select(transferId).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateScanDoc(int transferId)
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            var result = _context.usp_Transfer_ScanDoc_Update(transferId,user).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        //get transfer datatable
        [HttpGet]
        public JsonResult GetTransfer_DT(int? branchId,bool? isScanDoc,bool? isTransfer,int? draw = null,int? indexStart = null,
            int? pageSize = null,string sortField = null,string orderType = null,string search = null)
        {
            if(branchId == 1)
            {
                branchId = null;
            }

            var result = _context.usp_Transfer_Select(branchId,isScanDoc,isTransfer,indexStart,pageSize,sortField,orderType,search).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
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

        #endregion API
    }
}