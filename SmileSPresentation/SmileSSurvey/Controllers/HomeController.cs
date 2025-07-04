using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Office2013.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using SmileSSurvey.Helper;
using SmileSSurvey.Models;
using SmileSSurvey.ViewModels;

namespace SmileSSurvey.Controllers
{
    public class HomeController : Controller
    {
        #region Constructor

        private readonly SurveyEntities _context;

        public HomeController() => _context = new SurveyEntities();

        #endregion Constructor

        /// <summary>
        /// index
        /// </summary>
        /// <param name="Id">surveyId</param>
        /// <returns></returns>
        ///
        [HttpGet]
        [Route("survey/{Id}")]
        public ActionResult Index(string id)
        {
            var newId = Encoding.UTF8.GetString(Convert.FromBase64String(id));
            int Id = Int32.Parse(newId);

            try
            {
                //Get Survey Detail
                var survey = _context.usp_Survey_Select(surveyId: Id, indexStart: null, pageSize: 9999, sortField: null, orderType: null, searchDetail: null).FirstOrDefault();
                //Check value is null
                if (survey == null)
                {
                    TempData["Msg"] = $"survey id : {Id} is null";
                    return RedirectToRoute("Fail");
                };

                //Satisfaction
                if (survey.IsSatisfaction == true)
                {
                    //แบบประเมิน
                    return RedirectToRoute("Satisfaction", new { surveyId = id });
                }
                else
                {
                    //แบบสอบถาม
                    return RedirectToRoute("SurveyResearch", new { surveyId = id });
                }
            }
            catch (System.Exception e)
            {
                TempData["Msg"] = e.Message;
                return RedirectToRoute("Fail");
            }
        }

        /// <summary>
        /// Success
        /// </summary>
        /// <param name="msg">ข้อความ</param>
        /// <returns></returns>
        [HttpGet]
        [Route("success")]
        public ActionResult Success()
        {
            ViewBag.Msg = TempData["Msg"];
            ViewBag.Img = TempData["Img"];
            return View();
        }

        [HttpGet]
        [Route("surveysuccess")]
        public ActionResult SurveySuccess()
        {
            ViewBag.Msg = TempData["Msg"];
            ViewBag.Img = TempData["Img"];
            return View();
        }

        [HttpGet]
        [Route("surveyduplicate")]
        public ActionResult SurveyDuplicate()
        {
            ViewBag.Msg = TempData["Msg"];
            ViewBag.Img = TempData["Img"];
            return View();
        }

        /// <summary>
        /// Fail
        /// </summary>
        /// <param name="msg">ข้อความ</param>
        /// <returns></returns>
        [HttpGet]
        [Route("fail")]
        public ActionResult Fail()
        {
            ViewBag.Msg = TempData["Msg"];
            return View();
        }
    }
}