using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSPACommunity.Models;
using SmileSPACommunity.Helper;

namespace SmileSPACommunity.Controllers
{
    public class PartialViewController : Controller
    {
        private PACommunityEntities _db;

        public PartialViewController()
        {
            _db = new PACommunityEntities();
        }

        // GET: PartailView
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetBenefitList(int? proId = null, int? beneTypeid = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_ProductBenefit_Select(proId, beneTypeid, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            //var dt = new Dictionary<string, object>();
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetApplicationDetail(int applicationID)
        {
            usp_Application_Select_Result rs = new usp_Application_Select_Result();

            var c = _db.usp_Application_Select(applicationID, null, null, 0, 1, null, null, null).Count();

            if (c == 0)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            else
            {
                rs = _db.usp_Application_Select(applicationID, null, null, 0, 1, null, null, null).Single();

                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProductDetail(int proid)
        {
            usp_Product_Select_Result rs = new usp_Product_Select_Result();

            var c = _db.usp_Product_Select(proid, 0, 1, null, null, null).Count();

            if (c == 0)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            else
            {
                rs = _db.usp_Product_Select(proid, 0, 1, null, null, null).Single();

                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetPersonnelProductByPlanId(int productId)
        {   
            var result = _db.usp_PersonnelProduct_Select(productId, 0, 99, null, null, null).Single();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCustomerDetail(int applicatonId, int? applicatonRoundId = null, int? CustomerDetailId = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_PersonnelCustomerDetail_Select(applicatonId, null, applicatonRoundId,null, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTask(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, int? applicationID = null)
        {
            var result = _db.usp_PersonnelApplicationTransactionHeader_Select(applicationID, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetImportTask(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, int? applicationID = null)
        {
            var result = _db.usp_PersonnelApplicationRoundByApplicationId_Select(applicationID, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPersonnelDocument(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, int? applicationID = null,string documentTypelistId = null)
        {
            var result = _db.usp_PersonnelDocument_Select(applicationID, documentTypelistId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

    }
}