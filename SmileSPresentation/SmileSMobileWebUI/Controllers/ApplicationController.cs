using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Http.Results;
using System.Web.Mvc;
using SmileSMobileWebUI.PersonService;
using SmileSMobileWebUI.PHApplicationService;
using SmileSMobileWebUI.FinancialDataCenterService;
using SmileSMobileWebUI.MobileDataCenterService;
using SmileSMobileWebUI.AddressPHService;
using SmileSMobileWebUI.AddressDataCenterService;

namespace SmileSMobileWebUI.Controllers
{
    public class ApplicationController : Controller
    {
        // GET: Application

        #region application

        public ActionResult Application()
        {
            //Session["step"] = "Application/";
            var appId = Request.QueryString["appid"];

            if (!string.IsNullOrEmpty(appId))
            {
                using (var applicationService = new ApplicationServiceClient())
                {
                    ViewBag.obj = applicationService.GetApplicationHeader(appId);
                    var trCustomerId = applicationService.GetTrCustomerTransaction(appId, "2000").FirstOrDefault(); //2000 = transaction new app
                    var lstTrCustomerDetail = applicationService.GetTrCustomerTransactionDetail(trCustomerId?.CustomerTransaction_id, null);
                    foreach (var item in lstTrCustomerDetail)
                    {
                        switch (item.TransactionTypeDetail_id)
                        {
                            case "2022":
                                ViewBag.trPeriod = item.To_Detail;
                                break;

                            case "2021":
                                ViewBag.trPremiumSum = item.To_Detail;
                                break;
                        }
                    }
                }

                using (var financialService = new FinancialClient())
                {
                    ViewBag.paymethodtype = financialService.GetPayMethodType().ToList();
                }

                using (var mobileService = new MobileServiceClient())
                {
                    ViewBag.empDetail1 = mobileService.GetEmployee(ViewBag.obj.Sale_id1).CodeEmployeeName;
                    ViewBag.collectSource = mobileService.GetCollectSource();

                    ViewBag.empDetail2 = ViewBag.obj.Sale_id2 != null ? mobileService.GetEmployee(ViewBag.obj.Sale_id2).CodeEmployeeName : "";
                }
                ViewBag.APIUrlApplication = Properties.Settings.Default.APIUrlApplication;
                ViewBag.AppId = appId;
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "not found");
                //throw new HttpException(404, "appId not found.");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Application(FormCollection form)
        {
            var appId = form["applicationId"];
            var bankAdvance = form["bankAdvance"];
            var signaturePass = form["signaturePass"];
            var oldCustomer = form["oldCustomer"];
            var paymethodId = form["payMethodHidden"];
            var collectSourceId = form["collectSource"];
            var bankId = form["bank"];
            var bankAccountId = form["bankAccount"];
            var udwTimePeriodId = form["udwPeriod"];
            var saleId1 = form["empId1"];
            var saleId2 = form["empId2"];
            var zebraId = form["zebraCar"];
            var updateById = form["hidden_CreateById"];
            bool isBankDeductNextMonth = false;
            bool isCheckSignature = false;
            bool isAdditionalCustomer = false;

            if (bankAdvance == "1")
            {
                isBankDeductNextMonth = true;
            }
            if (signaturePass == "1")
            {
                isCheckSignature = true;
            }
            if (oldCustomer == "1")
            {
                isAdditionalCustomer = true;
            }

            ////Validate
            //if (appId != "xxxx205")
            //{
            //    ViewBag.ErrorText = "ทดสอบไม่ผ่าน";
            //    return Redirect(Url.Action("Application",new { appid = appId }));
            //}

            using (var application = new ApplicationServiceClient())
            {
                var resultAppId = application.SetApplicationPh01(appId, isBankDeductNextMonth, isCheckSignature, isAdditionalCustomer,
                      paymethodId, collectSourceId, bankId, bankAccountId, udwTimePeriodId, saleId1, saleId2, zebraId,
                      updateById);

                if (!string.IsNullOrEmpty(resultAppId))
                {
                    //GOTO : Customer
                    return Redirect(Url.Action("Customer2", new { appid = resultAppId }));
                }
            }
            return Redirect(Url.Action("Application", new { appid = appId }));
            //return View();
        }

        public ActionResult ApplicationDetail()
        {
            var appId = Request.QueryString["appid"];

            if (!string.IsNullOrEmpty(appId))
            {
                var client = new ApplicationServiceClient();
                var obj = client.GetApplicationHeader(appId);

                ViewBag.obj = obj;
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "not found querystring(appid)");
                //throw new HttpException(404, "appId not found.");
            }

            return View();
        }

        #endregion application

        #region Customer

        public ActionResult Customer2()
        {
            //if (Session["step"] == "Application/" || Session["step"] == "Application/Customer2")
            //{
            //    Session["step"] = "Application/Customer2";
            //}
            var appId = Request.QueryString["appid"];

            //set viewbag appId
            ViewBag.AppId = appId;

            if (!string.IsNullOrEmpty(appId))
            {
                //applicationService
                using (var applicationService = new ApplicationServiceClient())
                {
                    ViewBag.obj = applicationService.GetApplicationHeader(appId);
                }

                //addressPHService
                using (var addressService = new AddressPHService.AddressServiceClient())
                {
                    if (!string.IsNullOrEmpty(ViewBag.obj.HomeAddress_id))
                    {
                        //ที่อยู่ตามบัตร
                        ViewBag.HomeAddress = addressService.GetAddressSelect(ViewBag.obj.HomeAddress_id);
                    }
                    if (!string.IsNullOrEmpty(ViewBag.obj.WorkAddress_id))
                    {
                        //ที่อยู่ที่ทำงาน
                        ViewBag.WorkAddress = addressService.GetAddressSelect(ViewBag.obj.WorkAddress_id);
                    }
                    if (!string.IsNullOrEmpty(ViewBag.obj.ContactAddress_id))
                    {
                        //ที่อยู่ปัจจุบัน
                        ViewBag.ContactAddress = addressService.GetAddressSelect(ViewBag.obj.ContactAddress_id);
                    }
                }
                //AddressDataCenter
                using (var addressDatacenter = new AddressDataCenterService.AddressServiceClient())
                {
                    ViewBag.ProvinceList = addressDatacenter.GetProvince(null).ToList();
                }
                //personService
                using (var personService = new PersonServiceClient())
                {
                    ViewBag.TitleName = personService.GetTitle(null, null).ToList();
                }

                //mobileService
                using (var mobileService = new MobileServiceClient())
                {
                    ViewBag.Occupation = mobileService.GetOccupation().ToList();
                    ViewBag.Sex = mobileService.GetSex().ToList();
                    ViewBag.MaritalStatus = mobileService.GetMaritalStatus().ToList();
                    ViewBag.BloodType = mobileService.GetBloodType().ToList();
                    ViewBag.AddressType = mobileService.GetAddressType().ToList();
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "not found querystring(appid)");
            }

            ViewBag.APIUrlApplication = Properties.Settings.Default.APIUrlApplication;
            return View();
        }

        [HttpPost]
        public ActionResult Customer2(FormCollection form)
        {
            var appId = form["hidden_AppId"];
            var cusTitleId = form["hidden_titleName"];
            var firstName = form["firstName"];
            var lastName = form["lastName"];
            var birthDate = form["birthdate"];
            var sexId = form["sex"];
            var maritalStatusId = form["maritalstatus"];
            var bloodType = form["bloodtype"];
            var weight = form["weight"];
            var height = form["height"];
            var zcardId = form["id_card"];
            var passports = form["passport"];
            var officerCardNo = form["officercard"];
            var occupationId = form["occupation"];
            var occupationLevel = form["occupationLevel"];
            var tel = form["home_phone"];
            var workTel = form["work_phone"];
            var mobile = form["mobilePhone"];
            var email = form["-"];
            var updateById = form["hidden_CreateById"];

            using (var customer = new ApplicationServiceClient())
            {
                var resultAppId = customer.SetApplicationPh02(appId, cusTitleId, firstName, lastName, Convert.ToDateTime(birthDate), sexId, maritalStatusId, bloodType, float.Parse(weight), float.Parse(height), zcardId, passports, officerCardNo, occupationId, occupationLevel, tel, workTel, mobile, email, updateById);

                if (!string.IsNullOrEmpty(resultAppId))
                {
                    //GOTO : Payer
                    return Redirect(Url.Action("Payer", new { appid = resultAppId }));
                }
            }
            return Redirect(Url.Action("Customer2", new { appid = appId }));
            //return View();
        }

        [HttpPost]
        public ActionResult Address(FormCollection form)
        {
            string addresId = "";
            if (form["select_typeaddress"] == "2")
            {
                addresId = form["cur_AddressID"];
            }
            if (form["select_typeaddress"] == "3")
            {
                addresId = form["card_AddressID"];
            }
            if (form["select_typeaddress"] == "4")
            {
                addresId = form["work_AddressID"];
            }

            var division = form["modal_division"];
            var villageName = form["modal_village"];
            var no = form["modal_no"];
            var moo = form["modal_moo"];
            var floor = form["modal_floor"];
            var road = form["modal_road"];
            var soi = form["modal_alley"];
            var subdistrictId = form["hidden_subdistrictId"];
            var appId = form["hidden_AppId"];
            var updateById = form["hidden_CreateById"];

            using (var address = new AddressPHService.AddressServiceClient())
            {
                var resultAddressId = address.SetAddress(addresId, division, villageName, no, moo, floor, soi, road,
                    subdistrictId, updateById);

                if (!string.IsNullOrEmpty(resultAddressId.Address_ID))
                {
                    //GOTO : Customer
                    return Redirect(Url.Action("Customer2", new { appid = appId }));
                }
            }
            return Redirect(Url.Action("Customer2", new { appid = appId }));
            //return View();
        }

        #endregion Customer

        #region Payer

        public ActionResult Payer()
        {
            var appId = Request.QueryString["appid"];

            //set viewbag appId
            ViewBag.AppId = appId;

            if (!string.IsNullOrEmpty(appId))
            {
                //applicationService
                using (var applicationService = new ApplicationServiceClient())
                {
                    ViewBag.obj = applicationService.GetApplicationHeader(appId);
                }

                using (var addressService = new AddressPHService.AddressServiceClient())
                {
                    if (!string.IsNullOrEmpty(ViewBag.obj.PayerWorkAddress_id))
                    {
                        //ที่อยู่ที่ทำงาน
                        ViewBag.WorkAddress = addressService.GetAddressSelect(ViewBag.obj.PayerWorkAddress_id);
                    }
                    if (!string.IsNullOrEmpty(ViewBag.obj.PayerAddress_id))
                    {
                        //ที่อยู่ปัจจุบัน
                        ViewBag.ContactAddress = addressService.GetAddressSelect(ViewBag.obj.PayerAddress_id);
                    }

                    if (!string.IsNullOrEmpty(ViewBag.obj.WorkAddress_id))
                    {
                        //ที่อยู่ที่ทำงาน ผู้เอาประกัน
                        ViewBag.WorkAddressCustomer = addressService.GetAddressSelect(ViewBag.obj.WorkAddress_id);
                    }
                    if (!string.IsNullOrEmpty(ViewBag.obj.ContactAddress_id))
                    {
                        //ที่อยู่ปัจจุบัน ผู้เอาประกัน
                        ViewBag.ContactAddressCustomer = addressService.GetAddressSelect(ViewBag.obj.ContactAddress_id);
                    }
                }

                //AddressDataCenter
                using (var addressDatacenter = new AddressDataCenterService.AddressServiceClient())
                {
                    ViewBag.ProvinceList = addressDatacenter.GetProvince(null).ToList();
                }

                //personService
                using (var personService = new PersonServiceClient())
                {
                    ViewBag.TitleName = personService.GetTitle(null, null).ToList();
                    ViewBag.RelationType = personService.GetRelation().ToList();
                }

                //mobileService
                using (var mobileService = new MobileServiceClient())
                {
                    ViewBag.Occupation = mobileService.GetOccupation().ToList();
                    ViewBag.AddressType = mobileService.GetAddressType().ToList();
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "not found querystring(appid)");
            }

            ViewBag.APIUrlApplication = Properties.Settings.Default.APIUrlApplication;
            return View();
        }

        [HttpPost]
        public ActionResult Payer(FormCollection form)
        {
            var appId = form["hidden_AppId"];
            var relationId = form["relationship"];
            var titleId = form["hidden_titleName"];
            var firstName = form["firstName"];
            var lastName = form["lastName"];
            var zCardId = form["id_card"];
            var occupationId = form["hidden_occupationId"];
            var occupationLevelId = form["hidden_occupationLevelId"];
            var phoneNumber = form["mobilePhone"];
            var email = form["email"];
            var updateById = form["hidden_CreateById"];

            using (var payer = new ApplicationServiceClient())
            {
                var resultAppId = payer.SetApplicationPh03(appId, relationId, titleId, firstName, lastName, zCardId, occupationId, occupationLevelId, phoneNumber, email, updateById);
                if (!string.IsNullOrEmpty(resultAppId))
                {
                    //GOTO : Payer
                    return Redirect(Url.Action("HeirAndEmergencyContact", new { appid = resultAppId }));
                }
            }
            return Redirect(Url.Action("Payer", new { appid = appId }));
        }

        #endregion Payer

        #region HeirAndEmergencyContact

        public ActionResult HeirAndEmergencyContact()
        {
            var appId = Request.QueryString["appid"];
            ViewBag.AppId = appId;
            if (!string.IsNullOrEmpty(appId))
            {
                var client = new ApplicationServiceClient();
                var obj = client.GetApplicationHeader(appId);

                ViewBag.obj = obj;
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "not found querystring(appid)");
                //throw new HttpException(404, "appId not found.");
            }

            using (
                var personClient = new PersonServiceClient())
            {
                ViewBag.TitleContact = personClient.GetTitle(null, null).ToList();
                ViewBag.RelationType = personClient.GetRelation().ToList();
            }

            using (var sssClient = new ApplicationServiceClient())
            {
                ViewBag.CusDetail = sssClient.GetApplicationHeader(appId);
                var Heirlist = sssClient.GetHeirByAppId(appId);
                ViewBag.HeirList = Heirlist;
                double? sumTotalPerCent = 0;
                foreach (var item in Heirlist)
                {
                    sumTotalPerCent += item.PercentShare;
                }
                ViewBag.SumPercent = sumTotalPerCent;
                ViewBag.dividePercent = 100 - sumTotalPerCent;
                var sss = sssClient.GetEmergencyContactByAppId(appId);
                ViewBag.ContactList = sss;
            }
            @ViewBag.UrlPage = Properties.Settings.Default.UrlPage;
            @ViewBag.APIUrlApplication = Properties.Settings.Default.APIUrlApplication;
            return View();
        }

        [HttpPost]
        public ActionResult HeirAndEmergencyContact(FormCollection form)
        {
            var heirId = form["RelationCode"];
            return View();
        }

        #endregion HeirAndEmergencyContact

        #region UnderWrite

        public ActionResult UnderWrite()
        {
            var appId = Request.QueryString["appid"];
            var underWiteTypeId = "1000";

            ViewBag.AppId = appId;
            if (!string.IsNullOrEmpty(appId))
            {
                var client = new ApplicationServiceClient();
                ViewBag.UnderWriteList = client.GetUnderWriteByAppID(appId, underWiteTypeId);

                using (var applicationService = new ApplicationServiceClient())
                {
                    ViewBag.obj = applicationService.GetApplicationHeader(appId);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "not found querystring(appid)");
                //throw new HttpException(404, "appId not found.");
            }

            return View();
        }

        [HttpPost]
        public ActionResult UnderWrite(FormCollection form)
        {
            var appId = form["applicationId"];
            var valueUdw1001 = form["udw_1001"];
            var valueUdw1002 = form["udw_1002"];
            var valueUdw1003 = form["udw_1003"];
            var valueUdw1004 = form["udw_1004"];
            var valueUdw1005 = form["udw_1005"];
            var valueUdw1006 = form["udw_1006"];
            var valueUdw1007 = form["udw_1007"];
            var valueUdw1101 = form["udw_1101"];
            var valueUdw1102 = form["udw_1102"];
            var valueUdw1103 = form["udw_1103"];
            var valueUdw1201 = form["udw_1201"];
            var valueUdw1202 = form["udw_1202"];
            var valueUdw1203 = form["udw_1203"];
            var valueUdw1301 = form["udw_1301"];
            var valueUdw1302 = form["udw_1302"];
            var valueUdw1303 = form["udw_1303"];
            var valueUdw1304 = form["udw_1304"];
            var valueUdw1305 = form["udw_1305"];
            var valueUdw1306 = form["udw_1306"];
            var valueUdw1307 = form["udw_1307"];
            var valueUdw1308 = form["udw_1308"];
            var valueUdw1309 = form["udw_1309"];
            var valueUdw1310 = form["udw_1310"];
            var valueUdw1311 = form["udw_1311"];
            var valueUdw1312 = form["udw_1312"];
            var valueUdw1313 = form["udw_1313"];
            var valueUdw1314 = form["udw_1314"];
            var valueUdw1315 = form["udw_1315"];
            var updateById = form["hidden_CreateById"];

            using (var ph05 = new ApplicationServiceClient())
            {
                //var  result2 = application.SetApplicationPh05(appId,valueUdw1001,valueUdw1002,valueUdw1003,valueUdw1004,valueUdw1005,valueUdw1006,valueUdw1007,valueUdw1101,valueUdw1102,valueUdw1103,valueUdw1201,valueUdw1202),
                var resultAppId = ph05.SetApplicationPh05(appId
                    , valueUdw1001
                    , valueUdw1002
                    , valueUdw1003
                    , valueUdw1004
                    , valueUdw1005
                    , valueUdw1006
                    , valueUdw1007
                    , valueUdw1101
                    , valueUdw1102
                    , valueUdw1103
                    , valueUdw1201
                    , valueUdw1202
                    , valueUdw1203
                    , valueUdw1301
                    , valueUdw1302
                    , valueUdw1303
                    , valueUdw1304
                    , valueUdw1305
                    , valueUdw1306
                    , valueUdw1307
                    , valueUdw1308
                    , valueUdw1309
                    , valueUdw1310
                    , valueUdw1311
                    , valueUdw1312
                    , valueUdw1313
                    , valueUdw1314
                    , valueUdw1315
                    , updateById);

                if (!string.IsNullOrEmpty(resultAppId))
                {
                    //GOTO : Customer
                    return Redirect(Url.Action("Memo", new { appid = resultAppId }));
                }
            }
            return Redirect(Url.Action("UnderWrite", new { appid = appId }));
            //return View();
        }

        #endregion UnderWrite

        public ActionResult Customer()
        {
            var client = new PersonServiceClient();
            ViewBag.RelationshipList = client.GetTitle(null, null).ToList();
            return View();
        }

        #region Memo

        public ActionResult Memo()
        {
            var appId = Request.QueryString["appid"];

            if (!string.IsNullOrEmpty(appId))
            {
                using (var applicationService = new ApplicationServiceClient())
                {
                    ViewBag.obj = applicationService.GetApplicationHeader(appId);
                }

                var client = new MobileServiceClient();
                ViewBag.BranchList = client.GetBranch();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "not found querystring(appid)");
                //throw new HttpException(404, "appId not found.");
            }

            ViewBag.APIUrlApplication = Properties.Settings.Default.APIUrlApplication;
            return View();
        }

        //[HttpPost]
        //public ActionResult Memo(FormCollection form)
        //{
        //    var appId = form["hidden_AppId"];
        //    var memoType = form["ddlMemoType"];
        //    var memoTxt = form["txtMemo"];
        //    var createById = form["hidden_CreateById"];

        //    using (var memo = new ApplicationServiceClient())
        //    {
        //        var resultMemoId = memo.SetCustomerMemo(appId, createById, memoType, memoTxt, "").ToString();

        //        if (!string.IsNullOrEmpty(resultMemoId))
        //        {
        //            //GOTO : Memo
        //            return Redirect(Url.Action("Memo", new { appid = appId }));
        //        }
        //    }
        //    return Redirect(Url.Action("Memo", new { appid = appId }));
        //    //return View();
        //}

        [HttpPost]
        public ActionResult Memo(FormCollection form)
        {
            var appId = form["applicationId"];
            var sentBranch = form["sentBranch"];
            var createById = form["hidden_CreateById"];

            using (var memo = new ApplicationServiceClient())
            {
                var result = memo.SetApplicationPh06(appId, sentBranch, createById);

                if (!string.IsNullOrEmpty(result))
                {
                    //GOTO : Memo
                    // return Redirect(Url.Action("Memo", new { appid = appId }));
                    return RedirectToAction("Success", "Community");
                }
            }
            //return Redirect(Url.Action("Memo", new { appid = appId }));
            //return View();
            ViewBag.message = "ไม่สามารถบันทึกได้ กรุณาลองใหม่อีกครั้ง!";
            return View();
        }

        #endregion Memo

        #region DataCenter

        public ActionResult GetZebraCar()
        {
            using (var mobileService = new MobileServiceClient())
            {
                var result = mobileService.GetZebraCar().ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TestJqueryValid(string value)
        {
            using (var application = new ApplicationServiceClient())
            {
                var result = application.TESTJquery(value).IsResult;

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion DataCenter
    }
}