using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSHTMLRanking.Helper;
using SmileSHTMLRanking.Models;

namespace SmileSHTMLRanking.Controllers
{
    [Authorization]
    public class HTMLController:Controller
    {
        #region dbCon

        private readonly HtmlRankingEntities _context;
        private static readonly CultureInfo _dtEn = new CultureInfo("en-US");
        private static readonly CultureInfo _dtTh = new CultureInfo("th-TH");

        public HTMLController()

        {
            _context = new HtmlRankingEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion dbCon

        #region View

        // GET: HTML
        [Authorization(Roles = "Developer,RankingEditor")]
        public ActionResult Index()
        {
            var rankingGroupList = _context.usp_RankingGroup_Select(null).ToList();
            ViewBag.rankingGroupList = rankingGroupList;

            return View();
        }

        public ActionResult See()
        {
            var rankingGroupList = _context.usp_RankingGroup_Select(null).ToList();
            ViewBag.rankingGroupList = rankingGroupList;
            return View();
        }

        #endregion View

        #region api

        [HttpPost]
        public JsonResult Select(string period,int rankingTypeId)
        {
            try
            {
                DateTime periodConvert = DateTime.ParseExact(period,"dd-MM-yyyy",new CultureInfo("en-Us"));

                var result = _context.usp_Ranking_Select(periodConvert,rankingTypeId).FirstOrDefault();

                if(result == null)
                {
                    return Json("false",JsonRequestBehavior.AllowGet);
                }
                return Json(result,JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(e.Message,JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Submit(string period,int rankingTypid,string rankingDetail)
        {
            try
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                DateTime periodConvert = DateTime.ParseExact(period,"dd-MM-yyyy",new CultureInfo("en-Us"));
                var result = _context.usp_Ranking_Insert(periodConvert,rankingTypid,rankingDetail,user);

                return Json(true,JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(e.Message,JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ddlTypeLoad(int groupTypeId)
        {
            try
            {
                var listResult = _context.usp_RankingTypeByRankingGroupId_Select(groupTypeId).ToList();

                return Json(listResult,JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(e.Message,JsonRequestBehavior.AllowGet);
            }
        }

        #endregion api
    }
}