using SmileSDataCenterV2.Helper;
using SmileSDataCenterV2.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SmileSDataCenterV2.Controllers
{
    public class InfomationHospitalManagementController : Controller
    {
        private DataCenterV1Entities _db;

        public InfomationHospitalManagementController()
        {
            _db = new DataCenterV1Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        // GET: InfomationHospitalManagement
        public ActionResult Index()
        {
            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Supervisor")]
        public ActionResult InfomationManagementMonitor()
        {
            //Hospital Type List
            ViewBag.HospitalTypeList = _db.usp_OrganizeType_Select(null, 5, 0, 9999999, null, null, "").ToList();

            //Province List
            ViewBag.ProvinceList = _db.usp_Province_Select(null).ToList();
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.userID = userID;

            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Supervisor")]
        public ActionResult InfomationHospitalManagement(string id = "", int mode = 3)
        {
            string organId = "";
            ViewBag.OrganizeId = "";
            //Tab show and update data
            if (!(string.IsNullOrEmpty(id)))
            {
                //Decode
                var base64EncodedBytes = Convert.FromBase64String(id);
                var decode = Encoding.UTF8.GetString(base64EncodedBytes);
                organId = decode;
                ViewBag.OrganizeId = organId;

                var data1 = _db.usp_OrganizeInformationByOrganizeId_Select(Convert.ToInt32(organId)).FirstOrDefault();
                var data2 = _db.usp_OrganizeContractDetailByOrganizeId_Select(Convert.ToInt32(organId)).FirstOrDefault();
                var data3 = _db.usp_OrganizeBankAccountByOrganizeId_Select(Convert.ToInt32(organId)).FirstOrDefault();
                //if (data3.Count > 1)
                //{
                //    var data3Tmp = data3.Where(x => x.Bank_ID != 2).ToList();
                //    if (data3Tmp.Count != 0) data3 = data3Tmp;
                //}

                var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                ViewBag.userID = userID;

                //////////////////load data//////////////////////
                //ประเภทสถานพยาบาล
                ViewBag.HospitalType = data1.OrganizeTypeDetail;
                //ชื่อสถานพยาบาล
                ViewBag.HospitalName = data1.OrganizeDetail;
                //Is active
                ViewBag.IsActive = data1.IsActive;

                ViewBag.VillageName = data1.VillageName;
                ViewBag.No = data1.No;
                ViewBag.Moo = data1.Moo;
                ViewBag.Floor = data1.Floor;
                ViewBag.Soi = data1.Soi;
                ViewBag.Road = data1.Road;

                //Lattitude
                ViewBag.Lattitude = data1.Latitude;
                //Logitude
                ViewBag.Longitude = data1.Longitude;
                //ตำบล
                ViewBag.SubDistrictData = data1.SubDistrictDetail;
                //อำเภอ
                ViewBag.DistrictData = data1.DistrictDetail;
                //จังหวัด
                ViewBag.ProvinceData = data1.ProvinceDetail;
                //รหัสไปรษณีย์
                ViewBag.PostCode = data1.ZipCode;
                //เบอร์ติดต่อ
                ViewBag.HospitalTel = data1.PrimaryPhone;
                //เบอร์แฟกซ์
                ViewBag.HospitalFax = data1.Fax;
                //อีเมล
                ViewBag.HospitalEmail = data1.Email;
                //สถานที่ใกล้เคียง
                ViewBag.NearByPlace = data1.NearByPlace;

                //ชื่อผู้ติดต่อฝ่ายการเงิน
                ViewBag.FinanceContact = data1.FinanceContact;
                //เบอร์ติดต่อ
                ViewBag.FinanceTel = data1.FinanceTel;
                //เบอร์แฟกซ์
                ViewBag.FinanceEmail = data1.FinanceEmail;
                //อีเมล
                ViewBag.FinanceFax = data1.FinanceFax;

                //ชื่อผู้ติดต่อฝ่ายการเงินสำรอง
                ViewBag.AlternateFinanceContact = data1.AlternateFinanceContact;
                //เบอร์ติดต่อ
                ViewBag.AlternateFinanceTel = data1.AlternateFinanceTel;
                //เบอร์แฟกซ์
                ViewBag.AlternateFinanceEmail = data1.AlternateFinanceEmail;
                //อีเมล
                ViewBag.AlternateFinancefax = data1.AlternateFinanceFax;

                //ชื่อผู้ติดต่อฝ่ายบัญชี
                ViewBag.AccountingContact = data1.AccountingContact;
                //เบอร์ติดต่อ
                ViewBag.AccountingTel = data1.AccountingTel;
                //เบอร์แฟกซ์
                ViewBag.AccountingFax = data1.AccountingFax;
                //อีเมล
                ViewBag.AccountingEmail = data1.AccountingEmail;

                //ชื่อผู้ติดต่อฝ่ายMarketing
                ViewBag.MarketingContact = data1.MarketingContact;
                //เบอร์ติดต่อ
                ViewBag.MarketingTel = data1.MarketingTel;
                //เบอร์แฟกซ์
                ViewBag.MarketingFax = data1.MarketingFax;
                //อีเมล
                ViewBag.MarketingEmail = data1.MarketingEmail;

                //ชื่อผู้ติดต่อพยาบาล
                ViewBag.NurseContact = data1.NurseContact;
                //เบอร์ติดต่อ
                ViewBag.NurseTel = data1.NurseTel;
                //เบอร์แฟกซ์
                ViewBag.NurseFax = data1.NurseFax;
                //อีเมล
                ViewBag.NurseEmail = data1.NurseEmail;

                ViewBag.Iscontract = data2.IsContract;

                ViewBag.IscontractStartDT = data2.StartContractDate == null ? "" : data2.StartContractDate.Value.ToString();
                ViewBag.IscontractEndDT = data2.EndContractDate == null ? "" : data2.EndContractDate.Value.ToString();
                ViewBag.IsnotcontractOnProcessStatus = data2.PendingStatusId;
                ViewBag.Remark = data2.Remark;

                ViewBag.PhIpdIsContract = data2.IPD_PH;
                ViewBag.PhOpdIsContract = data2.OPD_PH;
                ViewBag.PAIpdIsContract = data2.IPD_PA;
                ViewBag.PAOpdIsContract = data2.OPD_PA;

                if (data3 != null)
                {
                    ViewBag.ChequeName = data3.OrganizeBankAccountChequeName;
                    ViewBag.BankId = data3.Bank_ID;
                    //ViewBag.BankName = data3[0].BankDetail;
                    ViewBag.BankBranch = data3.OrganizeBranchBank;
                    ViewBag.BankAccountNo = data3.OrganizeBankAccountNO;
                    ViewBag.BankAccountName = data3.OrganizeBankAccountName;
                }

                //Hospital Type List
                //List<usp_OrganizeType_Select_Result> organizeTypeLst = _db.usp_OrganizeType_Select(null, 5, 0, 9999999, null, null, "").ToList();
                //if (organizeTypeLst.Count > 0) ViewBag.HospitalTypeList = organizeTypeLst.Where(x => x.OrganizeTypeId == 8 || x.OrganizeTypeId == 10).ToList();
                //else ViewBag.HospitalTypeList = new List<usp_OrganizeType_Select_Result>();

                //ViewBag.Province = _db.usp_Province_Select(null).ToList();
                //ViewBag.District = _db.usp_District_Select(888888).ToList();
                //ViewBag.SubDistrict = _db.usp_SubDistrict_Select(8888888).ToList();
                ViewBag.BankName = _db.usp_Bank_Select(null).ToList().Where(x => x.BankDetail != "รอข้อมูล");
                ViewBag.Mode = mode;
                ViewBag.OnProcessStatus = _db.usp_OrganizeStatus_Select(0, 100000, null, null, null).ToList();

                ViewBag.AllowEditInfo = true;
                ViewBag.AllowEditContract = true;
                ViewBag.AllowEditBanking = true;
                //if (mode == 1) //view
                //{
                //}
                //else//update
                //{
                //}
            }
            else // for insert
            {
                ViewBag.Province = _db.usp_Province_Select(null).ToList();
                ViewBag.District = _db.usp_District_Select(888888).ToList();
                ViewBag.SubDistrict = _db.usp_SubDistrict_Select(8888888).ToList();

                //Hospital Type List
                List<usp_OrganizeType_Select_Result> organizeTypeLst = _db.usp_OrganizeType_Select(null, 5, 0, 9999999, null, null, "").ToList();
                if (organizeTypeLst.Count > 0) ViewBag.HospitalTypeList = organizeTypeLst.Where(x => x.OrganizeTypeId == 8 || x.OrganizeTypeId == 10).ToList();
                else ViewBag.HospitalTypeList = new List<usp_OrganizeType_Select_Result>();

                ViewBag.BankName = _db.usp_Bank_Select(null).ToList();
            }

            //get data tab 1

            //get data tab 2
            //get data tab 3

            return View();
        }

        public JsonResult GetHospitalInfoList(int? organizeId, int? organizeTypeId, int? provinceId, string keyWord, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_HospitalOrganize_Select(organizeId,
                                                                                            organizeTypeId == -1 ? null : organizeTypeId,
                                                                                            provinceId == -1 ? null : provinceId,
                                                                                            keyWord,
                                                                                            pageStart,
                                                                                            pageSize,
                                                                                            sortField,
                                                                                            orderType,
                                                                                            search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
            // return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDistrictByProvinceId(string provinceId)
        {
            var result = _db.usp_District_Select(null).ToList();
            result = result.Where(x => x.Province_ID == provinceId).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void HospitalInfoListExport(int? organizeId, int? organizeTypeId, int? provinceId, string keyWord)
        {
            var rs = _db.usp_HospitalOrganize_Select(organizeId,
                                                                                   organizeTypeId == -1 ? null : organizeTypeId,
                                                                                   provinceId == -1 ? null : provinceId,
                                                                                   keyWord,
                                                                                   null,
                                                                                   1000000,
                                                                                   "Organize_ID",
                                                                                   "desc",
                                                                                   "").Select(x => new
                                                                                   {
                                                                                       รหัสรายการ = x.Organize_ID,
                                                                                       ประเภท = x.OrganizeTypeDetail,
                                                                                       ชื่อสถานพยาบาล = x.OrganizeDetail,
                                                                                       ตำบล = x.SubDistrictDetail,
                                                                                       อำเภอ = x.DistrictDetail,
                                                                                       จังหวัด = x.ProvinceDetail,
                                                                                       คู่สัญญา = x.IsContract == true ? "เป็น" : x.IsContract == false ? "ไม่เป็น" : "ไม่ระบุ",
                                                                                       วันที่ทำรายการ = x.OrganizeCreateDate.Value.Date.ToString("dd/MM/yyyy"),
                                                                                       สถานะ = x.IsActive == true ? "ใช้งาน" : "ไม่ใช้งาน"
                                                                                   }).ToList();

            var dt = GlobalFunction.ToDataTable(rs);

            ExcelManager.ExportToExcel(this.HttpContext, "รายการสถานพยาบาล", dt, "รายการสถานพยาบาล");
        }

        public JsonResult ContractInsertOrUpdate(int organizeId, bool isContract, string remark = null, int? iPD_PH = null, int? oPD_PH = null, int? iPD_PA = null, int? oPD_PA = null, string startDate = null, string endDate = null, int? onProcessId = null)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            DateTime? d_startdate = null;
            DateTime? d_enddate = null;

            if (startDate != null && startDate != "")
            {
                d_startdate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(startDate));
            }

            if (endDate != null && endDate != "")
            {
                d_enddate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(endDate));
            }

            var rs = _db.usp_OrganizeContractDetail_Insert(organizeId, isContract, iPD_PH, oPD_PH, iPD_PA, oPD_PA, userID, d_startdate, d_enddate, onProcessId, null, remark).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BankAccountInsertOrUpdate(int organizeId, string ChequeName = null, int? bankId = null, string bankBranch = null, string bankAccountNo = null, string bankAccountName = null)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            //var rs = _db.usp_OrganizeContractDetail_Insert(organizeId,)

            ChequeName = ChequeName == "" ? null : ChequeName;
            bankId = bankId == -1 ? null : bankId;
            bankBranch = bankBranch == "" ? null : bankBranch;
            bankAccountNo = bankAccountNo == "" ? null : bankAccountNo;
            bankAccountName = bankAccountName == "" ? null : bankAccountName;

            var rs = _db.usp_OrganizeBankAccount_Insert(organizeId, ChequeName, bankId, bankBranch, bankAccountNo, bankAccountName, userID, true).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InformationInsertOrUpdate(int organizeId, string nearByPlace = null, string organizeContactTel = null, string organizeContactFax = null
                                                    , string organizeContactEmail = null, string latitude = null, string longitude = null, string financeContact = null,
                                                    string financeTel = null, string financeEmail = null, string financeFax = null, string alternateFinanceContact = null,
                                                    string alternateFinanceTel = null, string alternateFinanceEmail = null, string alternateFinanceFax = null,
                                                    string accountingContact = null, string accountingTel = null, string accountingEmail = null, string accountingFax = null,
                                                    string marketingContact = null, string marketingTel = null, string marketingEmail = null, string marketingFax = null, string nurseContact = null,
                                                    string nurseTel = null, string nurseEmail = null, string nurseFax = null)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var contactType_Tel = 3;
            var contactType_Email = 4;
            var contactType_Fax = 12;

            var rs = _db.usp_OrganizeInformation_Insert(organizeId, nearByPlace, organizeContactTel, contactType_Tel, organizeContactFax, contactType_Fax, organizeContactEmail, contactType_Email,
                                                        latitude, longitude, financeContact, financeTel, financeEmail, financeFax, alternateFinanceContact, alternateFinanceTel, alternateFinanceEmail,
                                                        alternateFinanceFax, accountingContact, accountingTel, accountingEmail, accountingFax, marketingContact, marketingTel, marketingEmail, marketingFax, nurseContact,
                                                        nurseTel, nurseEmail, nurseFax, userID).Single();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrganizeTransaction(int organizeId, int? draw, int? pageSize, int? indexStart, string sortField, string orderType, string search)
        {
            var result = _db.usp_OrganizeTransaction_Select(organizeId, indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrganizeTransactionDetailById(int? transactionId, int? draw, int? pageSize, int? indexStart, string sortField, string orderType, string search)
        {
            var result = _db.usp_OrganizeTransactionDetail_Select(transactionId, indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }
    }
}