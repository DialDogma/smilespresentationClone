using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSMobileWebUI.Helper;
using SmileSMobileWebUI.Models;

namespace SmileSMobileWebUI.Controllers
{
    [Authorization]
    public class PortfolioController : Controller
    {
        // GET: Portfolio
        public ActionResult Index()
        {
            using (var db = new MobileV1Entities())
            {
                //LAST UPDATE
                var lastUpdate = db.usp_UpdatedDate_Select().FirstOrDefault();
                ViewBag.LastUpdate = lastUpdate;
                ViewBag.LoginDetail = Authorization.GetLoginDetail();
                ViewBag.BranchCode = db.usp_PersonUser_Select(Authorization.GetLoginDetail().User_ID).First().BranchCode;

                return View();
            }
        }

        // GET: ProductGroup List
        public ActionResult GetProductGroupList(int? typeId)
        {
            using (var db = new MobileV1Entities())
            {
                var productGroupList = db.usp_Dropdown_Select(typeId).ToList();
                return Json(productGroupList, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Area List
        public ActionResult GetAreaList()
        {
            using (var db = new MobileV1Entities())
            {
                var areaList = db.usp_StudyArea_Select(null, null, null, 500, null, null, null).ToList();

                return Json(areaList, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Period List
        public ActionResult GetPeriodList()
        {
            using (var db = new MobileV1Entities())
            {
                var periodList = db.usp_Period_Select().ToList();

                return Json(periodList, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: App Detail Count
        public ActionResult GetAppDetailCount(string period, string productGroupCode, int? referanceTypeId, string referanceCode)
        {
            using (var db = new MobileV1Entities())
            {
                var nPeriod = DateTime.ParseExact(period, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var lst = db.usp_AppDetailCount_Select(nPeriod, productGroupCode, referanceTypeId, referanceCode).ToList();

                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }

        //GET: App Detail for Datatables
        public ActionResult GetAppDetail(string period, string productGroupCode, int? referanceTypeId, string referanceCode, int? typeId, int? indexStart, int? pageSize, string sortField, string orderType, string search)
        {
            using (var db = new MobileV1Entities())
            {
                var nPeriod = DateTime.ParseExact(period, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var lst = db.usp_AppDetail_Select(nPeriod,
                                                 productGroupCode,
                                                 referanceTypeId,
                                                 referanceCode,
                                                 typeId,
                                                 indexStart,
                                                 pageSize,
                                                 sortField,
                                                 orderType,
                                                 search).ToList();
                var dt = new Dictionary<string, object>
                {
                    //{"draw",draw },
                    {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"data", lst.ToList()}
                };
                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        //GET: RankNewAppSale for Datatables
        public ActionResult GetRankNewAppSalePage(string period, string productGroupCode, string referanceCode, int? indexStart, int? pageSize)
        {
            using (var db = new MobileV1Entities())
            {
                var nPeriod = DateTime.ParseExact(period, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var lst = db.usp_RankNewAppSalePage_Select(nPeriod,
                                                            productGroupCode,
                                                            referanceCode,
                                                            indexStart,
                                                            pageSize,
                                                            null,
                                                            null,
                                                            null).ToList();

                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }

        //GET: Ranking newapp for Datatables
        public ActionResult GetRankNewAppSale(string period, string productGroupCode, string referanceCode, int? indexStart, int? pageSize, string search)
        {
            using (var db = new MobileV1Entities())
            {
                var nPeriod = DateTime.ParseExact(period, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var lst = db.usp_RankNewAppSale_Select(nPeriod,
                                                        productGroupCode,
                                                        referanceCode,
                                                        indexStart,
                                                        pageSize,
                                                        null,
                                                        null,
                                                        search).ToList();
                var dt = new Dictionary<string, object>
                {
                    //{"draw",draw },
                    {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"data", lst.ToList()}
                };
                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        //GET: Rank Payer Area for Datatables
        public ActionResult GetRankPayerStudyAreaPage(string period, string productGroupCode, string referanceCode, int? gradeTypeId, int? referanceTypeId, int? indexStart, int? pageSize)
        {
            using (var db = new MobileV1Entities())
            {
                var nPeriod = DateTime.ParseExact(period, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var lst = db.usp_RankPayerStudyAreaPage_Select(nPeriod,
                                                            productGroupCode,
                                                            gradeTypeId,
                                                            referanceTypeId,
                                                            referanceCode,
                                                            indexStart,
                                                            pageSize,
                                                            null,
                                                            null,
                                                            null).ToList();

                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }

        //GET:RankNewAppSale for Datatables
        public ActionResult GetRankPayerStudyArea(string period, string productGroupCode, string referanceCode, int? gradeTypeId, int? referanceTypeId, int? indexStart, int? pageSize, string search)
        {
            using (var db = new MobileV1Entities())
            {
                var nPeriod = DateTime.ParseExact(period, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var lst = db.usp_RankPayerStudyArea_Select(nPeriod,
                                                        productGroupCode,
                                                        gradeTypeId,
                                                        referanceTypeId,
                                                        referanceCode,
                                                        indexStart,
                                                        pageSize,
                                                        null,
                                                        null,
                                                        search).ToList();
                var dt = new Dictionary<string, object>
                {
                    //{"draw",draw },
                    {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"data", lst.ToList()}
                };
                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        //GET: RankPayerBranch Page for Datatables
        public ActionResult GetRankPayerBranchPage(string period, string productGroupCode, string referanceCode, int? gradeTypeId, int? referanceTypeId, int? indexStart, int? pageSize)
        {
            using (var db = new MobileV1Entities())
            {
                var nPeriod = DateTime.ParseExact(period, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var lst = db.usp_RankPayerBranchPage_Select(nPeriod,
                                                            productGroupCode,
                                                            gradeTypeId,
                                                            referanceTypeId,
                                                            referanceCode,
                                                            indexStart,
                                                            pageSize,
                                                            null,
                                                            null,
                                                            null).ToList();

                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }

        //GET:RankPayerBranch for Datatables
        public ActionResult GetRankPayerBranch(string period, string productGroupCode, string referanceCode, int? gradeTypeId, int? referanceTypeId, int? indexStart, int? pageSize, string search)
        {
            using (var db = new MobileV1Entities())
            {
                var nPeriod = DateTime.ParseExact(period, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var lst = db.usp_RankPayerBranch_Select(nPeriod,
                                                        productGroupCode,
                                                        gradeTypeId,
                                                        referanceTypeId,
                                                        referanceCode,
                                                        indexStart,
                                                        pageSize,
                                                        null,
                                                        null,
                                                        search).ToList();
                var dt = new Dictionary<string, object>
                {
                    //{"draw",draw },
                    {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"data", lst.ToList()}
                };
                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GET BRANCH GRAPH TITLE
        /// </summary>
        /// <param name="branchCode"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public ActionResult GetBranchGraph_Title(string branchCode, string period)
        {
            using (var db = new MobileV1Entities())
            {
                var nPeriod = DateTime.ParseExact(period, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var obj = db.usp_BranchGraph_Title(branchCode, nPeriod).FirstOrDefault();

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///  GET BRANCH GRAPH FM FO
        /// </summary>
        /// <param name="branchCode"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public ActionResult GetBranchGraph_FM(string branchCode, string period)
        {
            using (var db = new MobileV1Entities())
            {
                var nPeriod = DateTime.ParseExact(period, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var obj = db.usp_BranchGraph_FM(branchCode, nPeriod).FirstOrDefault();

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get BranchGraph
        /// </summary>
        /// <param name="branchCode"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public ActionResult GetBranchGraph(string branchCode, string period)
        {
            using (var db = new MobileV1Entities())
            {
                var nPeriod = DateTime.ParseExact(period, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var lst = db.usp_BranchGraph(branchCode, nPeriod).ToList();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetAppTarget
        /// </summary>
        /// <param name="period"></param>
        /// <param name="productGroupCode"></param>
        /// <param name="refTypeId">person or branch</param>
        /// <returns></returns>
        public ActionResult GetAppTarget(string period, string productGroupCode, int? refTypeId)
        {
            using (var db = new MobileV1Entities())
            {
                var refCode = "";
                var nPeriod = DateTime.ParseExact(period, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                switch (refTypeId)
                {
                    case 1: //type ส่วนตัว
                        refCode = Authorization.GetLoginDetail().UserName;
                        break;

                    case 2: //type สาขา
                        refCode = db.usp_PersonUser_Select(Authorization.GetLoginDetail().User_ID).First().BranchCode;
                        break;

                    default:
                        break;
                }

                var lst = db.usp_AppTarget_Select(nPeriod, productGroupCode, refTypeId, refCode).ToList();

                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
    }
}