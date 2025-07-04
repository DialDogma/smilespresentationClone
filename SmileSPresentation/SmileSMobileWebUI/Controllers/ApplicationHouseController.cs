using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Http.Results;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using SmileSMobileWebUI.PersonService;
using SmileSMobileWebUI.PHApplicationService;
using SmileSMobileWebUI.FinancialDataCenterService;
using SmileSMobileWebUI.MobileDataCenterService;
using SmileSMobileWebUI.AddressPHService;
using SmileSMobileWebUI.AddressDataCenterService;

namespace SmileSMobileWebUI.Controllers
{
    public class ApplicationHouseController : Controller
    {
        // GET: ApplicationHouse
        public ActionResult Application()
        {
            var appid = Request.QueryString["appid"];

            if (!string.IsNullOrEmpty(appid))
            {
                using (var applicationService = new ApplicationServiceClient())
                {
                    ViewBag.App = applicationService.GetApplicationHeader(appid);
                    if (ViewBag.App != null)//if (!string.IsNullOrEmpty(ViewBag.App.App_id))
                    {
                        var trCustomerId = applicationService.GetTrCustomerTransaction(appid, "2000").FirstOrDefault();
                        ViewBag.CusTranDetail = applicationService.GetTrCustomerTransactionDetail(trCustomerId?.CustomerTransaction_id, "2023").FirstOrDefault();

                        //addressPHService
                        using (var addressService = new AddressPHService.AddressServiceClient())
                        {
                            //ที่อยู่ตามบัตร
                            if (!string.IsNullOrEmpty(ViewBag.App.HomeAddress_id))
                            {
                                ViewBag.HomeAddress = addressService.GetAddressSelect(ViewBag.App.HomeAddress_id);
                            }
                            //ที่อยู่ผู้ชำระเบี้ย
                            if (!string.IsNullOrEmpty(ViewBag.App.PayerAddress_id))
                            {
                                ViewBag.PayerAddress = addressService.GetAddressSelect(ViewBag.App.PayerAddress_id);
                            }
                        }
                        //addressDataCenter
                        using (var addressDatacenter = new AddressDataCenterService.AddressServiceClient())
                        {
                            ViewBag.ProvinceList = addressDatacenter.GetProvince(null).ToList();
                        }
                        //Person
                        using (var personService = new PersonServiceClient())
                        {
                            ViewBag.TitleName = personService.GetTitle(null, null).ToList();
                        }
                        //mobileService
                        using (var mobileService = new MobileServiceClient())
                        {
                            {
                                ViewBag.Occupation = mobileService.GetOccupation().ToList();
                                ViewBag.ZebraCar = mobileService.GetZebraCar().ToList();
                                ViewBag.PayMethodType = mobileService.GetPayMethodType().ToList();
                                ViewBag.CollectSource = mobileService.GetCollectSource().ToList();
                                ViewBag.Bank = mobileService.GetFinancialBank(false, true).ToList();
                                ViewBag.EmpDetail1 = mobileService.GetEmployee(ViewBag.App.Sale_id1).CodeEmployeeName;
                                ViewBag.EmpDetail2 = ViewBag.App.Sale_id2 != null ? mobileService.GetEmployee(ViewBag.App.Sale_id2).CodeEmployeeName : "";
                                //get tr customertran (Appid,2000).first'customertransactionid'   CT1802000172
                                //get tr customerdetail(customertransactionid(CT1802000172),2023).list      ///// ToDetail
                            }
                            if (!string.IsNullOrEmpty(ViewBag.App.Occupation_id))
                            {
                                ViewBag.OccupationLevel = mobileService.GetOccupationLevel(ViewBag.App.Occupation_id);
                            }
                            if (!string.IsNullOrEmpty(ViewBag.App.PayerOccupationLevel_id))
                            {
                                ViewBag.PayerOccupationLevel = mobileService.GetOccupationLevel(ViewBag.App.PayerOccupation_id);
                            }
                        }
                    }
                    else
                    {
                        Response.Write("n/a");
                        Response.End();
                    }
                }

                //using (var applicationService = new ApplicationServiceClient())
                //{
                //    ViewBag.App = applicationService.GetApplicationHeader(appid);
                //}
                //using (var financialService = new FinancialClient())
                //{
                //    ViewBag.paymethodtype = financialService.GetPayMethodType().ToList();
                //}

                //using (var mobileService = new MobileServiceClient())
                //{
                //    ViewBag.empDetail1 = mobileService.GetEmployee(ViewBag.App.Sale_id1).CodeEmployeeName;
                //    ViewBag.collectSource = mobileService.GetCollectSource();

                //    ViewBag.empDetail2 = ViewBag.App.Sale_id2 != null ? mobileService.GetEmployee(ViewBag.App.Sale_id2).CodeEmployeeName : "";
                //}

                //ViewBag.APIUrlApplication = Properties.Settings.Default["APIUrlApplication"].ToString();
                //ViewBag.AppId = appId;
            }
            else
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad Request");
                //throw new HttpException(404, "appId not found.");
                Response.Write("n/a");
                Response.End();
            }

            ViewBag.APIUrlApplication = Properties.Settings.Default.APIUrlApplication;
            return View();
        }

        [HttpPost]
        public ActionResult Application(FormCollection form)
        {
            var appId = form["applicationId"];
            var cardNo = form["cardNo"];
            var startCoverDate = Convert.ToDateTime(form["startCoverDate"]);
            var productCode = form["hidden_product"];
            var yearPaymentMode = Convert.ToInt32(form["hidden_yearPaymentMode"]);
            var payMethodType = form["payMethodType"];
            var collectSource = form["collectSource"];
            var bank = form["bank"];
            var bankAccountNo = form["bankAccountNo"];
            var empCode = form["empId1"];//form["empCode"];
            var zebraCar = form["zebraCar"];
            var houseCardId = form["houseCardID"];
            var no = form["no"];
            var buildingName = form["buildingName"];
            var villageName = form["villageName"];
            var moo = form["moo"];
            var floor = form["floor"];
            var soi = form["soi"];
            var road = form["road"];
            var subDistrict = form["subDistrict"];
            var phoneNo1 = form["phoneNo1"];
            var phoneNo2 = form["phoneNo2"];
            var ownerCardId = form["ownerCardID"];
            var ownerTitleCode = form["ownerTitleName"];
            var ownerFirstName = form["ownerFirstName"];
            var ownerLastName = form["ownerLastName"];
            var ownerOccupationLevel = form["ownerOccupationLevel"];
            var ownerPhoneNo = form["ownerPhoneNo"];
            var ownerMobileNo = form["ownerMobileNo"];
            var payerCardId = form["payerCardID"];
            var payerTitleCode = form["payerTitleName"];
            var payerFirstName = form["payerFirstName"];
            var payerLastName = form["payerLastName"];
            var payerOccupationLevel = form["payerOccupationLevel"];
            var payerMobileNo = form["payerMobileNo"];
            var payerNo = form["payerNo"];
            var payerBuildingName = form["payerBuildingName"];
            var payerVillageName = form["payerVillageName"];
            var payerMoo = form["payerMoo"];
            var payerFloor = form["payerFloor"];
            var payerSoi = form["payerSoi"];
            var payerRoad = form["payerRoad"];
            var payerSubDistrict = form["payerSubDistrict"];
            var customerAddressId = form["hidden_customerAddressId"];
            var payerAddressId = form["hidden_payerAddressId"];
            var birthDate = Convert.ToDateTime(form["hidden_birthDate"]);
            var billDate = DateTime.Now;//Convert.ToDateTime(form["hidden_billDate"]);
            var toDetail = form["hidden_todetail"];
            var createdByid = form["hidden_createdByid"];
            using (var application = new ApplicationServiceClient())
            {
                //Response.Write(ownerMobileNo + " xxxx " + ownerPhoneNo);
                //ownerMobileNo = ", ownerMobileNo";
                //ownerPhoneNo = "2";

                //Response.End();
                //update Application
                var resultAppId = application.SetApplicationHouse(appId, startCoverDate, cardNo, houseCardId, ownerTitleCode, ownerFirstName, ownerLastName, ownerCardId, birthDate, ownerOccupationLevel, phoneNo2, phoneNo1, payerTitleCode, payerFirstName, payerLastName, payerCardId, payerOccupationLevel, payerMobileNo, productCode, yearPaymentMode, billDate, toDetail, payMethodType, collectSource, bank, bankAccountNo, empCode, zebraCar, createdByid);
                var addressService = new AddressPHService.AddressServiceClient();
                //update Customer Address
                var resultCusAddress = addressService.SetAddress(customerAddressId, buildingName, villageName, no, moo, floor,
                    soi, road, subDistrict, createdByid);
                //update Payer Address
                //var resultPayerAddress = addressService.SetAddress(payerAddressId, payerBuildingName, payerVillageName, payerNo, payerMoo, payerFloor,
                //    payerSoi, payerRoad, payerSubDistrict, createdByid);
                if (!string.IsNullOrEmpty(resultAppId))
                {
                    //p Jack
                    return Redirect(Url.Action("Success", "UserAccount"));
                }
            }
            //P Jack
            return Redirect(Url.Action("ErrorPage", "UserAccount"));
        }

        #region Json

        //DataCenter_Get_Province
        public ActionResult GetProvince()
        {
            using (var addressDatacenter = new AddressDataCenterService.AddressServiceClient())
            {
                var result = addressDatacenter.GetProvince(null).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //DataCenter_Get_District
        public ActionResult GetDistrict(int provinceId)
        {
            using (var addressDatacenter = new AddressDataCenterService.AddressServiceClient())
            {
                var result = addressDatacenter.GetDistrict(provinceId).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //DataCenter_Get_SubDistrict
        public ActionResult GetSubDistrict(int districtId)
        {
            using (var addressDatacenter = new AddressDataCenterService.AddressServiceClient())
            {
                var result = addressDatacenter.GetSubDistrict(districtId).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //DataCenter_Get_Occupation
        public ActionResult GetOccupation()
        {
            using (var mobileService = new MobileServiceClient())
            {
                var result = mobileService.GetOccupation().ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //DataCenter_Get_OccupationLevel
        public ActionResult GetOccupationLevel(string occupationId)
        {
            using (var mobileService = new MobileServiceClient())
            {
                var result = mobileService.GetOccupationLevel(occupationId).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Json
    }
}