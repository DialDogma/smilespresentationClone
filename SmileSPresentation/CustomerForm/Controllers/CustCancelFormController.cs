using CustomerForm.Models;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CustomerForm.Controllers
{
    [RouteArea("ccf")]
    public class CustCancelFormController : Controller
    {
        private readonly CustomerFromEntities _context;

        public CustCancelFormController()
        {
            _context = new CustomerFromEntities();
        }

/*        [Route]
        [Route("{code}")]
        public ActionResult Index(string code)
        {

            try
            {
                ViewBag.lineId = "@siamsmile (24 ชม.)";
                if (!string.IsNullOrEmpty(code))
                {
                    byte[] codeCon = Convert.FromBase64String(code);
                    var newCode = Encoding.UTF8.GetString(codeCon);

                    ViewBag.Logo = Properties.Settings.Default.LogoPath;
                    ViewBag.Signature = Properties.Settings.Default.SignaturePath;
                    ViewBag.Footer = Properties.Settings.Default.FooterPath;

                    var detailList = _context.usp_SMSDetail_Select(newCode, 0, 10, null, null, null).ToList();
                    ViewBag.detail = detailList.FirstOrDefault();
                    if (detailList.Count() > 0)
                    {
                        var data = detailList.FirstOrDefault();
                        var subDetailList = _context.usp_SMSSubDetail_Select(data.Code, 0, 10, null, null, null).ToList();
                        ViewBag.list = subDetailList;


                        var pdf = new ViewAsPdf("Index", newCode)
                        {
                            ViewName = "Index",
                            FileName = newCode + ".pdf",
                            PageOrientation = Orientation.Portrait,
                            PageSize = Size.A4,
                            PageMargins = new Margins { Bottom = 0, Left = 0, Right = 0, Top = 0 },
                        };
                        return pdf;
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.NotFound, "SMS Detail Is Null");
                    }
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Code is Null");
                }
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid Code");
            }
        }
*/

        [Route]
        [Route("{code}")]
        public ActionResult Index2(string code)
        {

            try
            {
                ViewBag.lineId = "@siamsmile (24 ชม.)";
                if (!string.IsNullOrEmpty(code))
                {
                    byte[] codeCon = Convert.FromBase64String(code);
                    var newCode = Encoding.UTF8.GetString(codeCon);

                    ViewBag.Logo = Properties.Settings.Default.LogoPath;
                    ViewBag.Signature = Properties.Settings.Default.SignaturePath;
                    ViewBag.Footer = Properties.Settings.Default.FooterPath;

                    var detailList = _context.usp_SMSDetail_Select(newCode, 0, 20, null, null, null).ToList();
                    ViewBag.detail = detailList.FirstOrDefault();
                    if (detailList.Count() > 0)
                    {
                        var data = detailList.FirstOrDefault();
                        var subDetailList = _context.usp_SMSSubDetail_Select(data.Code, 0, 20, null, null, null).ToList();
                        ViewBag.list = subDetailList;


                        var pdf = new ViewAsPdf("Index2", newCode)
                        {
                            ViewName = "Index2",
                            FileName = newCode + ".pdf",
                            PageOrientation = Orientation.Portrait,
                            PageSize = Size.A4,
                            PageMargins = new Margins { Bottom = 0, Left = 0, Right = 0, Top = 0 },
                        };
                        return pdf;
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.NotFound, "SMS Detail Is Null");
                    }
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Code is Null");
                }
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid Code");
            }
        }
    }
}