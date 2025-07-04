using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MassTransit;
using SmileSDataCenterV2.Contracts;
using SmileSDataCenterV2.Helper;
using SmileSDataCenterV2.Models;

namespace SmileSDataCenterV2.Controllers
{
    public class CustomerManagementController : Controller
    {
        // GET: CustomerManagement
        private DataCenterV1Entities _db;
        private DataCenterV1Entities1 _db2;
        private readonly IBusControl _bus;


        // GET: InsuranceManagement
        public CustomerManagementController()
        {
            _db = new DataCenterV1Entities();
            _db2 = new DataCenterV1Entities1();
            _bus = MvcApplication.Bus as IBusControl;
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Supervisor")]
        public ActionResult CustomerManagementMonitor() //หน้า Monitor
        {
            ViewBag.PersonType = _db.usp_PersonType_Select(null).ToList().Where(x => x.PersonType_ID == 2 || x.PersonType_ID == 3);
            return View();
        }

        public JsonResult GetPersonForManagement(int? PersonId = null, int? PersonTypeId = null, string keyword = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            PersonId = PersonId == -1 ? null : PersonId;
            PersonTypeId = PersonTypeId == -1 ? null : PersonTypeId;

            var result = _db.usp_PersonForManagement_Select(PersonId, PersonTypeId, pageStart, pageSize, sortField, orderType, keyword).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerManagementDetail(string personId) // ดู รายละเอียด
        {
            var PersonIdBase64EncodeBytes = Convert.FromBase64String(personId);
            var deCode_personId = System.Text.Encoding.UTF8.GetString(PersonIdBase64EncodeBytes);
            ViewBag.PersonId = deCode_personId;
            return View();
        }

        public ActionResult CustomerManagementNormal(string personId) // หน้าแก้ไข บุคคลธรรมดา
        {
            var PersonIdBase64EncodeBytes = Convert.FromBase64String(personId);
            var deCode_personId = System.Text.Encoding.UTF8.GetString(PersonIdBase64EncodeBytes);
            ViewBag.PersonId = deCode_personId;

            ViewBag.PersonTitle = _db.usp_Title_Select(null, 2).ToList();
            ViewBag.AliveStatus = _db.usp_AliveStatus_Select(null, null, null, null, null, null).ToList();
            ViewBag.Sex = _db.usp_Sex_Select(null).Where(x => x.IsActive == true).ToList();
            ViewBag.MaritalStatus = _db.usp_MaritalStatus_Select(null).ToList();
            ViewBag.BloodType = _db.usp_BloodType_Select().ToList();
            ViewBag.Nationality = _db.usp_Nationality_Select(null, null, null, null, null, null).ToList();
            ViewBag.Race = _db.usp_Race_Select(null, null, null, null, null, null).ToList();
            ViewBag.Religion = _db.usp_Religion_Select(null, null, null, null, null, null).ToList();
            ViewBag.Occupation = _db.usp_Occupation_Select(null).ToList();
            //ViewBag.OccupationLevel = _db.usp_OccupationLevel_Select(null).ToList();

            return View();
        }

        public ActionResult OccupationLevelByOccupationID(int occupation_ID)
        {
            var rs = _db.usp_OccupationLevel_ByOccupationID_Select(occupation_ID).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OccupationRiskByOccupationLevelID(int occupationLevel_ID)
        {
            var rs = _db.usp_OccupationLevel_ByOccID_Select(occupationLevel_ID, null, null, null, null, null, null).FirstOrDefault();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerManagementJuristic() // หน้าแก้ไข นิติบุคคล
        {
            return View();
        }

        public ActionResult CustomerReportMenagement()
        {
            return View();
        }

        public ActionResult CustomerReportMotorMenagement()
        {
            return View();
        }

        public ActionResult CustomerContactManagement(string personId)
        {
            var PersonIdBase64EncodeBytes = Convert.FromBase64String(personId);
            var deCode_personId = System.Text.Encoding.UTF8.GetString(PersonIdBase64EncodeBytes);
            ViewBag.PersonId = deCode_personId;
            return View();
        }

        public ActionResult CustomerManagementDetailJuristic(string personId) // ดู รายละเอียด นิติบุคคล
        {
            var PersonIdBase64EncodeBytes = Convert.FromBase64String(personId);
            var deCode_personId = System.Text.Encoding.UTF8.GetString(PersonIdBase64EncodeBytes);
            int PersonId = Int32.Parse(deCode_personId);

            ViewBag.PersonId = PersonId;
            return View();
        }

        public JsonResult GetCustomerContactById(int personId)
        {
            var PersonContact = _db.usp_PersonForManagementContact_Select(personId, null, null, null, null, null).FirstOrDefault();

            return Json(PersonContact, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerAddressById(int personId, int addressTypeId)
        {
            var PersonAddress = _db.usp_PersonAddressByPersonId_Select(personId, addressTypeId, null, null, null, null, null).FirstOrDefault();

            return Json(PersonAddress, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerPersonById(int personId)
        {
            var Person = _db.usp_PersonForManagementByPersonId_Select(personId, null, null, null, null, null).FirstOrDefault();

            return Json(Person, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerDocumentInsertById(int personId, string documentTypeIdList)
        {
            var updateByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var result = _db.usp_Document_Insert(personId, documentTypeIdList, "", updateByUserId).Single();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerDocumentById(int personId, string documentTypeIdList, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_Document_Select(personId, documentTypeIdList, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProvinceDistrictSubDistrict(int? subDistrictId = null, int? districtId = null, int? provinceId = null, string zipCode = null, string q = null)
        {
            var rs = _db.usp_ProvinceDistrictSubDistrict_Select(subDistrictId, districtId, provinceId, zipCode, 0, 999, null, null, q).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransaction(int personId, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_PersonForManagementTransaction_Select(personId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransactionDetailById(int personId, int? transactionId, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_PersonForManagementTransactionDetail_Select(personId, transactionId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoUpdateNormalCustomer(
            int? personId, int? titleId, string firstName, string lastName, string identityCard, string passport, int? aliveStatusId, string birthDate, int? sexId, int? maritalStatusId,
            int? bloodTypeId, double? weight, double? height, int? nationalityId, int? raceId, int? religionId, int? occupationLevelId, decimal? salary, string medicalConditions, string disability
           , string primaryPhone, string secondaryPhone, string otherPhone, string email, string lineID, string facebook, string twitterID, string instagramID, string youtubeID
           , string h_No, string h_Moo, string h_VillageName, string h_Floor, string h_Soi, string h_Road, string h_SubDistrictCode, string h_ZipCode
            , string c_No, string c_Moo, string c_buildingName, string c_VillageName, string c_Floor, string c_Soi, string c_Road, string c_SubDistrictCode, string c_ZipCode
            , string w_BuildingName, string w_No, string w_Moo, string w_VillageName, string w_Floor, string w_Soi, string w_Road, string w_SubDistrictCode, string w_ZipCode)
        {
            salary = salary == 0 ? null : salary;
            var d_BirthDayCustomer = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(birthDate));
            var updateByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var rs = _db.usp_PersonDetailForDatacenter_Update(
                 personId, titleId, firstName, lastName, null, d_BirthDayCustomer, weight, height, sexId, maritalStatusId, occupationLevelId, null, raceId,
                 nationalityId, religionId, bloodTypeId, salary, medicalConditions, disability, null, null, null,
                 primaryPhone, secondaryPhone, otherPhone, email, facebook, lineID, youtubeID, instagramID, twitterID,
                 c_buildingName, c_VillageName, c_No, c_Moo, c_Floor, null, c_Soi, c_Road, c_SubDistrictCode, c_ZipCode,
                 w_BuildingName, w_VillageName, w_No, w_Moo, w_Floor, null, w_Soi, w_Road, w_SubDistrictCode, w_ZipCode,
                 null, h_VillageName, h_No, h_Moo, h_Floor, null, h_Soi, h_Road, h_SubDistrictCode, h_ZipCode, updateByUserId
            ).Single();

            if (rs.IsResult == true)
            {
                var personUpdated = _db2.usp_GetPersonDetailForUpdate(personId: personId).FirstOrDefault();
                if (personUpdated != null)
                {
                    // Create message 
                    var customerUpdate = new CustomerDetailUpdate
                    {
                        PersonId = personUpdated.PersonGuid.Value,
                        PersonTypeId = personUpdated.PersonTypeId,
                        CardTypeId = personUpdated.CardTypeId,
                        CardDetail = personUpdated.CardDetail,
                        TitleId = personUpdated.TitleId,
                        FirstName = personUpdated.FirstName,
                        LastName = personUpdated.LastName,
                        GenderId = personUpdated.GenderId,
                        BirthDate = personUpdated.BirthDate,
                        OccupationLevelId = personUpdated.OccupationLevelId,
                        MobileNo = personUpdated.PrimaryPhone,
                        PhoneNo = personUpdated.SecondaryPhone,
                        Email = personUpdated.Email,
                        ContactAddressDetail1 = personUpdated.c_AddressDetail1,
                        ContactAddressDetail2 = null,
                        ContactSubDistrictId = Convert.ToInt32(personUpdated.c_SubdistrictId),
                        HomeAddressDetail1 = personUpdated.h_AddressDetail1,
                        HomeAddressDetail2 = null,
                        HomeSubDistrictId = Convert.ToInt32(personUpdated.h_SubdistrictId),
                        WorkWorkplacId = personUpdated.w_WorkplaceId,
                        WorkAddressDetail1 = personUpdated.w_AddressDetail1,
                        WorkAddressDetail2 = null,
                        WorkSubDistrictId = Convert.ToInt32(personUpdated.w_SubdistrictId)
                    };

                    // Publish the message asynchronously (fire-and-forget)
                    //Task.Run(async () => await _bus.Publish(customerUpdate));
                }
            }
            

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInsuranceTransactionById(int? personId, int? draw, int? pageSize, int? indexStart, string sortField, string orderType, string search)
        {
            var result = _db.usp_CustomerApplicationRelatePerson_Select(personId, indexStart, pageSize, sortField, orderType, search).Where(x => x.ProjectId != 26).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServiceTransactionById(int? personId, int? draw, int? pageSize, int? indexStart, string sortField, string orderType, string search)
        {
            var result = _db.usp_CustomerApplicationRelatePerson_Select(personId, indexStart, pageSize, sortField, orderType, search).Where(x => x.ProjectId == 26).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReportTransactionByDete(string dateFrom, string dateTo, int? draw, int? pageSize, int? indexStart, string sortField, string orderType, string search)
        {
            DateTime d_dateStart;
            DateTime d_dateEnd;

            d_dateStart = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
            d_dateEnd = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));

            var result = _db.usp_PersonTransaction_SelectV2(null, d_dateStart, d_dateEnd, indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReportTransactionByDeteMotor(string dateFrom, string dateTo, int? draw, int? pageSize, int? indexStart, string sortField, string orderType, string search)
        {
            DateTime d_dateStart;
            DateTime d_dateEnd;

            d_dateStart = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
            d_dateEnd = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));

            var result = _db.usp_PersonTransactionRelateMotor_Select(null, d_dateStart, d_dateEnd, indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransactionReport(int? transactionId, int? indexStart, int? draw, int? pageSize, string sortField, string orderType, string searchDetail)
        {
            var list = _db.usp_PersonTransactionDetail_SelectV2(transactionId, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public void ExportCustomerReport(string dateStart, string dateEnd)
        {
            DateTime d_dateStart;
            DateTime d_dateEnd;

            d_dateStart = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateStart));
            d_dateEnd = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateEnd));

            var list = _db.usp_Report_PersonTransaction_Select(d_dateStart, d_dateEnd).ToList();

            var Data = list.Select(x => new
            {
                Person_ID = x.PersonId,
                คำนำหน้า = x.Title,
                ชื่ออลูกค้า = x.FirstName,
                นามสกุล = x.LastName,
                เลขประจำตัวประชาชน = x.IdCard,
                Passport = x.PassportCard,
                ประเภทข้อมูลการแก้ไข = x.TransactionType,
                ข้อมูลที่แก้ไข = x.TransactionSubType,
                เปลี่ยนจาก = x.FromDetail,
                เปลี่ยนเป็น = x.ToDetail,
                วันที่ทำรายการ = x.CreatedDate.Value.Date.ToString("dd/MM/yyyy"),
                ผู้ที่แก้ไข = x.CreateByName,
            }).ToList();

            ExcelManager.ExportToExcel(this.HttpContext, Data, "sheet1", "Customer_Manage_Information_Report_" + DateTime.Now.ToString());
        }

        public void ExportCustomerMotorReport(string dateStart, string dateEnd)
        {
            DateTime d_dateStart;
            DateTime d_dateEnd;

            d_dateStart = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateStart));
            d_dateEnd = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateEnd));

            var list = _db.usp_Report_PersonTransactionRelateMotor_Select(d_dateStart, d_dateEnd).ToList();

            var Data = list.Select(x => new
            {
                Person_ID = x.PersonId,
                คำนำหน้า = x.Title,
                ชื่ออลูกค้า = x.FirstName,
                นามสกุล = x.LastName,
                เลขประจำตัวประชาชน = x.IdCard,
                Passport = x.PassportCard,
                ประเภทข้อมูลการแก้ไข = x.TransactionType,
                ข้อมูลที่แก้ไข = x.TransactionSubType,
                เปลี่ยนจาก = x.FromDetail,
                เปลี่ยนเป็น = x.ToDetail,
                วันที่ทำรายการ = x.CreatedDate.Value.Date.ToString("dd/MM/yyyy"),
                ผู้ที่แก้ไข = x.CreateByName,
            }).ToList();

            ExcelManager.ExportToExcel(this.HttpContext, Data, "sheet1", "Customer_Motor_Manage_Information_Report_" + DateTime.Now.ToString());
        }

        public JsonResult GetContactCustomerById(int? personId)
        {
            var Contact = _db.usp_GetPersonContactByPersonId_Select(personId).SingleOrDefault();

            return Json(Contact, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrganizeBySubDistrictId(int? subDistrictId = null, string q = null)
        {
            var organize = _db.usp_OrganizeWorkplaceSearch_Select(subDistrictId, q, null, 99999, null, null, null).ToList();

            return Json(organize, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpsertCustomerContact(int? personId, string primaryPhone, string secordaryPhone = null, string email = null, string lineId = null, string facebookId = null, string twitterId = null, string instagramId = null, string youtubeId = null)
        {
            var createdByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _db.usp_PersonDetailForContact_Update(personId, primaryPhone, secordaryPhone, email, lineId, facebookId, youtubeId, instagramId, twitterId, createdByUserId).Single();

            if (result.IsResult == true)
            {
                var personUpdated = _db2.usp_GetPersonDetailForUpdate(personId: personId).FirstOrDefault();
                if (personUpdated != null)
                {
                    // Create  message 
                    var contactUpdate = new CustomerContactUpdate
                    {
                        PersonId = personUpdated.PersonGuid.Value,
                        MobileNo = personUpdated.PrimaryPhone,
                        PhoneNo = personUpdated.SecondaryPhone,
                        Email = personUpdated.Email
                    };

                    // Publish the message asynchronously (fire-and-forget)
                    //Task.Run(async () => await _bus.Publish(contactUpdate));
                }
            }      

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}