using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSPoint.Helper;
using SmileSPoint.Models;
using System.Net;
using System.IO;

namespace SmileSPoint.Controllers
{
    public class ExampleController : Controller
    {
        private SmilePointEntities _context;

        public ExampleController()
        {
            _context = new SmilePointEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Example
        public ActionResult Index()
        {
            return View();
        }

        #region Datatable with checkbox

        public ActionResult DatatableWithCheckbox()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DatatableWithCheckbox(FormCollection form)
        {
            //Get Selected ID In datatable
            var selectedIdDatatable = new List<string>();
            foreach (var item in form.AllKeys)
            {
                var leftString = item.Substring(0, 9);

                if (leftString == "chkdtrow_")
                {
                    var id = item.Replace("chkdtrow_", "");
                    selectedIdDatatable.Add(id);
                }
            }

            return View();
        }

        public JsonResult GetDatatableWithCheckbox(int draw, int pageSize, int pageStart, string sortField, string orderType, string search)
        {
            var list = _context.SP_PointAccount_Datatable(pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion Datatable with checkbox

        public ActionResult PopupWithCheckBox()
        {
            ViewBag.TypeList = _context.PointAccountTypes.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult PopupWithCheckBox(FormCollection form)
        {
            //Get Selected ID In datatable
            var selectedId = new List<string>();
            foreach (var item in form.AllKeys)
            {
                var leftString = item.Substring(0, 8);

                if (leftString == "chktype_")
                {
                    var id = item.Replace("chktype_", "");
                    selectedId.Add(id);
                }
            }

            ViewBag.TypeList = _context.PointAccountTypes.ToList();
            return View();
        }

        public ActionResult Radiobutton(FormCollection form)
        {
            return View();
        }

        public ActionResult DateAndTime()
        {
            ViewBag.InputDate = DateTime.Today;
            return View();
        }

        [HttpPost]
        public ActionResult DateAndTime(FormCollection form)
        {
            var d = form["date_in"];
            var t = form["time_in"];
            var inputDate = GlobalFunction.ConvertDatePickerToDate_BE(d, t);

            ViewBag.InputDate = inputDate.Value;
            return View();
        }

        public ActionResult QRGenerate()
        {
            return View();
        }

        public ActionResult BarcodeGenerate()
        {
            return View();
        }

        public ActionResult DateDiff()
        {
            return View();
        }

        public ActionResult ThaiIDCardValidate()
        {
            ViewBag.PostValue = "";
            return View();
        }

        [HttpPost]
        public ActionResult ThaiIDCardValidate(FormCollection form)
        {
            ViewBag.PostValue = form["id-card"].Replace("-", "");
            return View();
        }

        public ActionResult SignalRMonitor()
        {
            ViewBag.CurrentUser = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID.ToString();
            return View();
        }

        public ActionResult SignalRClient()
        {
            ViewBag.CurrentUser = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID.ToString();
            return View();
        }

        public ActionResult TimeLine()
        {
            //Get data from another services or sp
            ViewBag.TimelineData = new ExampleModel().GetExampleTimeLine();
            return View();
        }

        public ActionResult TEST_BY_BOOM()
        {
            return View();
        }

        public ActionResult ACTION_BOOM()
        {
            var r = "CHERPRANG BNK48 เฌอปราง อารีย์กุล";
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PointOnMobile()
        {
            return View();
        }

        public ActionResult TEST_APISMS()
        {
            //Check Credit
            //var Username = "SiamSmile";
            //var Password = "Admin@1234";

            //var Parameters = String.Format("?User={0}&Password={1}", Username, Password);
            //var API_URL = "https://member.smsmkt.com/SMSLink/GetCredit/index.php";

            //var webc = new WebClient();
            //var Result = webc.DownloadString(API_URL + Parameters);
            //Response.Write(Result);

            //Send SMS
            var Username = "SiamSmile";
            var Password = "Admin@1234";
            var PhoneList = "0922742125";
            var Message = "test API send sms";
            var SenderName = "SiamSmile";

            var ByteMsg = System.Text.Encoding.GetEncoding("TIS-620").GetBytes(Message);
            Message = HttpUtility.UrlEncode(ByteMsg);

            var Parameters = String.Format("?User={0}&Password={1}&Msnlist={2}&Msg={3}&Sender={4}", Username, Password, PhoneList, Message, SenderName);
            var API_URL = "https://member.smsmkt.com/SMSLink/SendMsg/index.php";

            var webc = new WebClient();
            var Result = webc.DownloadString(API_URL + Parameters);
            Response.Write(Result);
            return View();
        }

        public ActionResult TEST_WebServiceSMS()
        {
            var PathFile = Server.MapPath("") + "";
            var sr = new StreamReader(PathFile, System.Text.Encoding.Default);
            var StrData = sr.ReadToEnd();
            sr.Close();

            var ByteData = System.Text.ASCIIEncoding.GetEncoding("TIS-620").GetBytes(StrData);
            StrData = Convert.ToBase64String(ByteData);

            var Username = "";
            var Password = "";
            var FileData = StrData;
            var FileName = "";
            var SenderName = "";
            var SendDate = "";

            //   var ObjSMS = new SMSService();

            return View();
        }
    }
}