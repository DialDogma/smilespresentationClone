using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSDuration.Helper;
using SmileSDuration.Models;
using System.Net;
using System.Text;

namespace SmileSDuration.Controllers
{
    public class LetterController : Controller
    {
        private readonly DurationV1Entities _context;

        #region Declare

        public LetterController()
        {
            _context = new DurationV1Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Declare

        // GET: Letter
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Lapsed(string id)
        {
            var comID = Convert.ToInt32(id);
            ViewBag.Comunicate = _context.Comunicate.First(x => x.ComunicateID == comID);

            try
            {
                var myip = Request.UserHostAddress;
                //String myip = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                //String myip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];

                //if (string.IsNullOrEmpty(myip))
                //{
                //myip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];

                //   if (!myip.Equals("192.168.100.33"))
                //  {
                var logIP_Lapsed = _context.usp_ComunicateAnalytic_Insert(comID, myip);
                //  }
                //}
                //else
                //{
                //}
            }
            catch (Exception)
            {
                var logIP_Lapsed = _context.usp_ComunicateAnalytic_Insert(comID, "Error IP");
            }

            //var logIP = _context.usp_ComunicateAnalytic_Insert(comID, myip);

            return View();
        }

        [HttpGet]
        public ActionResult InsuranceStatus(string id)
        {
            var comID = Convert.ToInt32(id);
            ViewBag.Comunicate = _context.Comunicate.First(x => x.ComunicateID == comID);

            try
            {
                var myip = Request.UserHostAddress;
                //String myip = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                //if (string.IsNullOrEmpty(myip))
                //{
                //myip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];

                //   if (!myip.Equals("192.168.100.33"))
                //  {
                var logIP_InsStatus = _context.usp_ComunicateAnalytic_Insert(comID, myip);
                //  }
                //}
            }
            catch (Exception)
            {
                var logIP_InsStatus = _context.usp_ComunicateAnalytic_Insert(comID, "Error IP");
            }

            //var logIP = _context.usp_ComunicateAnalytic_Insert(comID, myip);

            return View();
        }

        public ActionResult TestShortURL(string fullURL)
        {
            ViewBag.ShortURL = ShortURL.GetShortURL(fullURL);
            return View();
        }

        /// <summary>
        /// appId == application code
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ThankYou(string Id)
        {
            using (var db = new DurationV1Entities())
            {
                var base64EncodedBytes = Convert.FromBase64String(Id);
                var n_Id = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                var ip = GlobalFunction.GetIPAddress();

                //GET DETAIL TEMP PA TO VIEWBAG
                var tempPA_Select = db.usp_TempPA_SelectV2(int.Parse(n_Id), ip).FirstOrDefault();
                //SET PA  TO VIEWBAG
                ViewBag.TempPA_Select = tempPA_Select;
                //GET PA PAYMENT
                var tempPAPayment_Select = db.usp_TempPAPayment_Select(tempPA_Select.ApplicattionCode).ToList();
                //SET PA PAYMENT TO VIEWBAG
                ViewBag.TempPAPayment_Select = tempPAPayment_Select;
                //SUM VALUE
                ViewBag.SumPremiumAmount = tempPAPayment_Select.Sum(i => i.PremiumAmount);
            }
            return View();
        }

        public ActionResult PaySlip(string code)
        {
            using (var db = new DurationV1Entities())
            {
                var base64EncodedBytes = Convert.FromBase64String(code);
                var DurationMsgCode = Encoding.UTF8.GetString(base64EncodedBytes);

                //GET PaySlipHeader  TO VIEWBAG
                var PaySlipHeader = db.usp_PaySlipHeader_Select(DurationMsgCode).FirstOrDefault();
                //SET  TO VIEWBAG
                ViewBag.PaySlipHeader = PaySlipHeader;
                ViewBag.BahtText = GlobalFunction.ThaiBahtText(PaySlipHeader.SumPremium.ToString());
                //GET PaySlipDetail
                var PaySlipDetail = db.usp_PaySlipDetail_Select(PaySlipHeader.DurationMessageCode).ToList();
                //SET  TO VIEWBAG
                ViewBag.PaySlipDetail = PaySlipDetail;
            }
            return View();
        }
    }
}