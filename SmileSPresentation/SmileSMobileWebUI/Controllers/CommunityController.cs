using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using SmileSMobileWebUI.MobileDataCenterService;
using SmileSMobileWebUI.PersonService;
using SmileSMobileWebUI.PHApplicationService;

namespace SmileSMobileWebUI.Controllers
{
    public class CommunityController : Controller
    {
        #region New App PA Community

        [HttpGet]
        public ActionResult AddPACommunity()
        {
            ViewBag.APIUrlApplication = Properties.Settings.Default.APIUrlApplication;
            var appId = Request.QueryString["appid"];
            //set viewbag appId
            ViewBag.AppId = appId;

            if (string.IsNullOrEmpty(appId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "not found querystring(appid)");
            }

            //applicationService
            using (var applicationService = new ApplicationServiceClient())
            {
                var result = applicationService.GetApplicationHeader(appId);
                ViewBag.obj = result;
                ViewBag.StartCoverDate = Convert.ToDateTime(result.StartCoverDate).ToShortDateString();
                ViewBag.zCardID = result.ZCard_id;
                ViewBag.passport = result.PassportCardID;
                ViewBag.cardNo = result.Card_no;
                ViewBag.firstname = result.FirstName;
                ViewBag.lastname = result.LastName;
                ViewBag.birthDate = Convert.ToDateTime(result.BirthDate).ToShortDateString();
                ViewBag.mobileNo = Regex.Replace(result.MobilePhoneNumber, "[^0-9]", "");
                ViewBag.contactAddressId = result.ContactAddress_id;
                ViewBag.createby = result.CreatedBy_id;
                ViewBag.titleId = result.Title_id;
                ViewBag.sexId = result.Sex_id;
                ViewBag.agentId = result.Sale_id1;
                ViewBag.OccupationLevel = result.OccupationLevel_id;
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
                //ViewBag.empDetail1 = mobileService.GetEmployee(ViewBag.agentId).CodeEmployeeName;
                ViewBag.Occupation = mobileService.GetOccupation().ToList();
                ViewBag.Sex = mobileService.GetSex().ToList();
                ViewBag.AddressType = mobileService.GetAddressType().ToList();
            }

            return View();
        }

        [HttpPost]
        public ActionResult AddPACommunity(FormCollection form)
        {
            var appId = form["input_AppID"];
            var agentId = form["input_AgentID"];
            var startCoverDate = Convert.ToDateTime(form["input_coverDate"]);
            var CardNo = form["input_AppCardID"];
            var titleID = form["hidden_titleName"];
            var firstname = form["input_firstname"];
            var lastname = form["input_surename"];
            var sexID = form["select_sex"];
            var zCardID = form["input_zcardID"];
            var passport = form["input_passport"];
            var birthDate = Convert.ToDateTime(form["input_birthDate"]);
            //var occupation = form["select_occupation"];
            var addressID = form["cur_AddressID"];
            var occupationLevel = form["selec_occupationlevel"];
            var mobileNo = form["input_mobile"];
            var createById = form["hidden_CreateById"];
            var contactAddressId = form["hidden_contactAddressId"];

            using (var client = new ApplicationServiceClient())
            {
                var spResult = client.SetDetailPACommunity(appId, titleID, firstname, lastname, sexID, zCardID,
                    passport, birthDate, startCoverDate, occupationLevel, mobileNo, addressID, agentId, createById,
                    CardNo, contactAddressId);
                if (!spResult.IsResult ?? false)
                {
                    TempData["Message"] = "เกิดข้อผิดพลาด ไม่สามารถบันทึกข้อมูลได้!!";
                    return RedirectToAction("AddPACommunity", new { appid = appId });
                }
                else
                {
                    return RedirectToAction("Success");
                }
            }
        }

        public JsonResult GetAddress(string addressId)
        {
            using (var addressService = new AddressPHService.AddressServiceClient())
            {
                var result = addressService.GetAddressSelect(addressId);
                var shortaddress = result.AddressFullDetail;
                return Json(shortaddress, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Success()
        {
            return View();
        }

        #endregion New App PA Community

        #region New Community

        [HttpGet]
        public ActionResult AddNewCommunity()
        {
            ViewBag.APIUrlApplication = Properties.Settings.Default.APIUrlApplication;
            var empCode = Request.QueryString["empCode"];
            //set viewbag empCode
            ViewBag.empCode = empCode;

            if (string.IsNullOrEmpty(empCode))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "not found querystring(appid)");
            }

            using (var addressDatacenter = new AddressDataCenterService.AddressServiceClient())
            {
                ViewBag.ProvinceList = addressDatacenter.GetProvince(null).ToList();
            }

            return View();
        }

        [HttpPost]
        public ActionResult AddNewCommunity(FormCollection form)
        {
            var communityDetail = form["input_community"];
            var contactName = form["input_name"];
            var contactPhone = form["input_phone"];
            var contactEmail = form["input_email"];

            var division = form["HomeCompany"];
            var villageName = form["HomeVillageName"];
            var no = form["HomeNumber"];
            var moo = form["HomeVillageNo"];
            var floor = form["HomeFloor"];
            var soi = form["HomeAlley"];
            var road = form["HomeRoad"];
            var tambolId = form["HomeTambol"].ToString();
            var createdBy = form["createby"];

            //call sp and set new community
            using (var applicationClient = new ApplicationServiceClient())
            {
                try
                {
                    var spResult = applicationClient.SetNewCommunity(communityDetail, contactName,
                        contactPhone, contactEmail, division, villageName, no, moo, floor, soi,
                        road, tambolId, createdBy);

                    return RedirectToAction("Success");
                }
                catch (Exception e)
                {
                    ViewBag.message = "ทำรายการไม่สำเร็จ!!";
                    return View();
                }
            }
        }

        #endregion New Community
    }
}