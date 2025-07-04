using SmileSPaymentGateway.Helper;
using SmileSPaymentGateway.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SmileSMobileAppToken;

namespace SmileSPaymentGateway.Controllers
{
    [Authorization]
    public class SendPremiumController : Controller
    {
        private readonly SSSCashReceiveEntities _sSSCashReceiveEntities;
        private readonly CultureInfo dateTH = new CultureInfo("th-TH");

        #region Declare

        public SendPremiumController()
        {
            _sSSCashReceiveEntities = new SSSCashReceiveEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _sSSCashReceiveEntities.Dispose();
        }

        #endregion Declare

        // GET: SendPremium

        #region Action

        public ActionResult Index()
        {
            var empCode = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;

            var list = _sSSCashReceiveEntities.usp_GetPremiumDeliverToCenter_Select("03754", 1000000000, 0, null, null).ToList();
            var mobilekey = new AlignToken().GetAlignToken();

            //การรวมค่า คำสั่ง linq
            var premiumTotal = list.Aggregate(0, (current, t) => current + Convert.ToInt32(t.Premium));

            //SET DATA TO VIEWBAG
            ViewBag.Token = mobilekey;
            ViewBag.EmpCode = empCode;
            ViewBag.URL_API = Properties.Settings.Default.URL_API;
            ViewBag.PremiumTotal = premiumTotal;
            ViewBag.CurrentDate = DateTime.Now.ToString("d MMMM yyyy", dateTH);
            ViewBag.CurrentTime = DateTime.Now.ToString("HH:mm");
            return View();
        }

        public ActionResult DeliverToCenterHistory()
        {
            var mobilekey = new AlignToken().GetAlignToken();
            var empCode = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;

            //SET DATA TO VIEWBAG
            ViewBag.Token = mobilekey;
            ViewBag.EmpCode = "03754"; //TODO:BOOM MOCK ID
            ViewBag.URL_API = Properties.Settings.Default.URL_API;
            return View();
        }

        #endregion Action

        #region GetData

        /// <summary>
        /// GET DATATABLE
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageStart"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public JsonResult GetDatatablePremiumDeliverToCenter(int draw, int pageSize, int pageStart, string sortField, string orderType)
        {
            var empCode = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;

            var list = _sSSCashReceiveEntities.usp_GetPremiumDeliverToCenter_Select("03754", pageSize, pageStart, sortField, orderType).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// CALL API
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetHistory(string empCode)
        {
            //SET Value Valiable
            const string method = "9010";

            //Hosted web API REST Service base url
            var Baseurl = Properties.Settings.Default.URL_API;
            var RequestUri = "mobileapi/Premiums/PremiumDeliverToCenterHistory?" + "DeliverMethod=" + method + "&EmpCode=" + empCode;
            var key = new AlignToken().GetAlignToken();

            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource  using HttpClient
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(key);
                var Res = await client.PostAsync(RequestUri, new StringContent(""));

                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //returning the Json Result
                    return Res.Content.ReadAsStringAsync().Result;
                }

                return null;
                //returning the employee list to view
            }
        }

        #endregion GetData
    }
}