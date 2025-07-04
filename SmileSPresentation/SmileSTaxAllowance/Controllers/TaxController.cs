using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSTaxAllowance.Helper;
using SmileSTaxAllowance.Models;

namespace SmileSTaxAllowance.Controllers
{
    [Authorization]
    public class TaxController : Controller
    {
        #region dbContext

        //private TaxAllowanceEntities _context;

        private TaxAllowanceEntities1 _db;

        public TaxController()

        {
            //_context = new TaxAllowanceEntities();
            _db = new TaxAllowanceEntities1();
        }

        protected override void Dispose(bool disposing)
        {
            //_context.Dispose();
            _db.Dispose();
        }

        #endregion dbContext

        // GET: Tax
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TaxAllowanceMonitor()
        {
            return View();
        }

        public ActionResult TaxAllowance2561Monitor()
        {
            return View();
        }

        public ActionResult TaxAllowance2562Monitor()
        {
            return View();
        }

        public ActionResult TaxAllowance2563Monitor()
        {
            return View();
        }

        public ActionResult TaxAllowance2564Monitor()
        {
            return View();
        }
        public ActionResult TaxAllowance2565Monitor()
        {
            return View();
        }

        public ActionResult TaxAllowance2566Monitor()
        {
            return View();
        }

        public ActionResult TaxAllowance2567Monitor()
        {
            return View();
        }

        #region "Method"

        //public JsonResult GetTaxAllowance2560Monitor(string cardId,string firstName,string lastName, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        //{
        //    //int userID;

        //    //userID = Convert.ToInt32(Session["User_ID"]);

        //    if (cardId != "")
        //    {
        //        cardId = cardId.Replace("-", "");
        //    }
        //    else
        //    {
        //        cardId = null;
        //    }

        //    if (firstName == "")
        //    {
        //        firstName = null;
        //    }

        //    if (lastName == "")
        //    {
        //        lastName = null;
        //    }

        //    var result = _context.usp_TaxData_2560_select(cardId, firstName, lastName, pageStart, pageSize, sortField, orderType).ToList();

        //    var dt = new Dictionary<string, object>
        //    {
        //        {"draw", draw },
        //        {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
        //        {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
        //        {"data", result.ToList()}
        //    };

        //    return Json(dt, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetTaxAllowance2561Monitor(string cardId, string firstName, string lastName, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        //{
        //    //int userID;

        //    //userID = Convert.ToInt32(Session["User_ID"]);

        //    if (cardId != "")
        //    {
        //        cardId = cardId.Replace("-", "");
        //    }
        //    else
        //    {
        //        cardId = null;
        //    }

        //    if (firstName == "")
        //    {
        //        firstName = null;
        //    }

        //    if (lastName == "")
        //    {
        //        lastName = null;
        //    }

        //    var result = _context.usp_TaxData_2561_select(cardId, firstName, lastName, pageStart, pageSize, sortField, orderType).ToList();

        //    var dt = new Dictionary<string, object>
        //    {
        //        {"draw", draw },
        //        {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
        //        {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
        //        {"data", result.ToList()}
        //    };

        //    return Json(dt, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetTaxAllowance2567Monitor(string cardId, string firstName, string lastName, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            //int userID;

            //userID = Convert.ToInt32(Session["User_ID"]);

            int year = 2568;

            if (cardId != "")
            {
                cardId = cardId.Replace("-", "");
            }
            else
            {
                cardId = null;
            }

            if (firstName == "")
            {
                firstName = null;
            }

            if (lastName == "")
            {
                lastName = null;
            }

            var result = _db.usp_TaxAllowance_Select(cardId, firstName, lastName, year, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxAllowance2566Monitor(string cardId, string firstName, string lastName, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            //int userID;

            //userID = Convert.ToInt32(Session["User_ID"]);

            int year = 2567;

            if (cardId != "")
            {
                cardId = cardId.Replace("-", "");
            }
            else
            {
                cardId = null;
            }

            if (firstName == "")
            {
                firstName = null;
            }

            if (lastName == "")
            {
                lastName = null;
            }

            var result = _db.usp_TaxAllowance_Select(cardId, firstName, lastName, year, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTaxAllowance2565Monitor(string cardId, string firstName, string lastName, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            //int userID;

            //userID = Convert.ToInt32(Session["User_ID"]);

            int year = 2566;

            if (cardId != "")
            {
                cardId = cardId.Replace("-", "");
            }
            else
            {
                cardId = null;
            }

            if (firstName == "")
            {
                firstName = null;
            }

            if (lastName == "")
            {
                lastName = null;
            }

            var result = _db.usp_TaxAllowance_Select(cardId, firstName, lastName, year, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxAllowance2564Monitor(string cardId, string firstName, string lastName, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            //int userID;

            //userID = Convert.ToInt32(Session["User_ID"]);

            int year = 2565;

            if (cardId != "")
            {
                cardId = cardId.Replace("-", "");
            }
            else
            {
                cardId = null;
            }

            if (firstName == "")
            {
                firstName = null;
            }

            if (lastName == "")
            {
                lastName = null;
            }

            var result = _db.usp_TaxAllowance_Select(cardId, firstName, lastName, year, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxAllowance2563Monitor(string cardId, string firstName, string lastName, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            //int userID;

            //userID = Convert.ToInt32(Session["User_ID"]);

            int year = 2564;

            if (cardId != "")
            {
                cardId = cardId.Replace("-", "");
            }
            else
            {
                cardId = null;
            }

            if (firstName == "")
            {
                firstName = null;
            }

            if (lastName == "")
            {
                lastName = null;
            }

            var result = _db.usp_TaxAllowance_Select(cardId, firstName, lastName, year, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxAllowance2562Monitor(string cardId, string firstName, string lastName, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            //int userID;

            //userID = Convert.ToInt32(Session["User_ID"]);

            int year = 2563;

            if (cardId != "")
            {
                cardId = cardId.Replace("-", "");
            }
            else
            {
                cardId = null;
            }

            if (firstName == "")
            {
                firstName = null;
            }

            if (lastName == "")
            {
                lastName = null;
            }

            var result = _db.usp_TaxAllowance_Select(cardId, firstName, lastName, year, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveTaxPrintLog(int allowanceId)
        {
            int createUserId;
            createUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var rs = _db.usp_TaxPrintLog_Insert(allowanceId, createUserId).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult InsertLog(string url)
        //{
        //    int userID;

        //    userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

        //    int JSONLogId;

        //    try
        //    {
        //        JSONLogId = _context.usp_JSONLog_insert(url, null, null, null, userID).SingleOrDefault().JSONLogId;

        //        return Json(JSONLogId, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(false, JsonRequestBehavior.AllowGet);
        //    }
        //}

        #endregion "Method"
    }
}