using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SmileSPersonUpdate.Helper;
using SmileSPersonUpdate.Models;
using SmileSPersonUpdate.PersonService;

namespace SmileSPersonUpdate.Controllers
{
    public class CustomersController : Controller
    {
        #region dbContext

        private CustomerBaseEntities _context;

        public CustomersController()

        {
            _context = new CustomerBaseEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion dbContext

        // GET: Customers
        public ActionResult DataAndAgreement(string code)
        {
            //get customer information
            string ipAddress = Request.UserHostAddress;
            string browserPlatform = Request.Browser.Platform;
            string browserVersion = Request.Browser.Browser;
            try
            {
                //select title
                ViewBag.CusTitle = _context.usp_DisclosureSMS_Title_Select().ToList();

                if (code != null)
                {
                    //encode
                    byte[] data = Convert.FromBase64String(code);
                    string appId = Encoding.UTF8.GetString(data);

                    //select cus data
                    var result = _context.usp_DisclosureSMS_Select(appId, ipAddress, browserPlatform, browserVersion).FirstOrDefault();
                    ViewBag.CusId = result.To_ZcardId;
                    ViewBag.FullName = result.To_FirstName + ' ' + result.To_LastName;
                    ViewBag.Product = result.Product;
                    ViewBag.Fname = result.To_FirstName;
                    ViewBag.Lname = result.To_LastName;
                    ViewBag.Bday = Convert.ToDateTime(result.To_BirthDate).ToShortDateString();
                    ViewBag.Tel = result.To_MobileNumber;
                    ViewBag.Email = result.To_Email;
                    ViewBag.CleanID = result.DisclosureSMSId;
                    ViewBag.AppId = result.ApplicationCode;
                    ViewBag.TitleCode = result.To_TitleCode;
                    ViewBag.CusCheck = result.DisclosureSMSStatusId;
                }
                else
                {
                    return RedirectToAction("NotFound", "Error");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("InternalServerError", "Error");
            }

            return View();
        }

        [HttpPost]
        public ActionResult DataAndAgreement(FormCollection form)
        {
            try
            {
                var disclosureId = Convert.ToInt32(form["hd_CleanId"]);
                var zCardId = form["txtIdCard"];
                var titleCode = form["select_title"];
                var fName = form["txtFName"];
                var lName = form["txtLName"];
                var bDay = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(form["BDate"]));
                var mobileNo = form["txtPhone"];
                var email = form["txtEmail"];
                string ipAddress = Request.UserHostAddress;
                var acceptAgreement = Convert.ToInt32(form["chkAccept"]);

                var result = _context.usp_DisclosureSMS_Update(disclosureId, zCardId, titleCode, fName, lName, bDay,
                    mobileNo, email, acceptAgreement, ipAddress).FirstOrDefault();

                if (result.IsResult == true)
                {
                    return RedirectToAction("SuccessResult");
                }
                else
                {
                    return RedirectToAction("FailResult");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult DataAndAgreement2(string disclosureId, string zCardId, string titleCode, string fName, string lName, string bDay, string mobileNo, string email, string acceptAgreement)
        {
            try
            {
                var _disclosureId = Convert.ToInt32(disclosureId);
                var _zCardId = zCardId;
                var _titleCode = titleCode;
                var _fName = fName;
                var _lName = lName;
                var _bDay = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(bDay));
                var _mobileNo = mobileNo;
                var _email = email;
                var ipAddress = Request.UserHostAddress;
                var _acceptAgreement = Convert.ToInt32(acceptAgreement);

                var result = _context.usp_DisclosureSMS_Update(_disclosureId, _zCardId, _titleCode, _fName, _lName, _bDay,
                    _mobileNo, _email, _acceptAgreement, ipAddress).FirstOrDefault();

                return Json(result.IsResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SuccessResult()
        {
            return View();
        }

        public ActionResult FailResult()
        {
            return View();
        }
    }
}