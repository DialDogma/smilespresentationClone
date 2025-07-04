using SmileSSurvey.Helper;
using SmileSSurvey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace SmileSSurvey.Controllers
{
    public class SatisfactionController : Controller
    {
        #region Constructor

        private readonly SurveyEntities _context;

        public SatisfactionController() => _context = new SurveyEntities();

        #endregion Constructor

        #region View

        /// <summary>
        /// แบบประเมินความพึงพอใจ
        /// </summary>
        /// <param name="Id">Survey Id</param>
        /// <returns></returns>
        [Route("st/{id}")]
        public ActionResult Satisfaction(string id)
        {
            var newId = Encoding.UTF8.GetString(Convert.FromBase64String(id));
            int surveyId = Int32.Parse(newId);

            try
            {
                //Check Duplicate
                var duplicate = _context.usp_SurveyResultDuplicateCheck_Select(surveyId: surveyId).FirstOrDefault();
                if (duplicate.IsResult.Value)
                {
                    TempData["Msg"] = duplicate.Msg;
                    return RedirectToRoute("Success");
                }

                //Get Survey Detail
                var survey = _context.usp_Survey_Select(surveyId: surveyId, indexStart: null, pageSize: 9999, sortField: null, orderType: null, searchDetail: null).FirstOrDefault();

                //Check value is null
                if (survey == null)
                {
                    TempData["Msg"] = $"survey id : {surveyId} is null";
                    return RedirectToRoute("Fail"); ;
                }

                //check not isSatisfaction
                if (!survey.IsSatisfaction == true)
                {
                    return RedirectToRoute("SurveyResearch", new { surveyId = surveyId });
                }

                //Get Question by Survey Id
                var surveyQuestion = _context.usp_SurveyQuestion_Select(surveyId: surveyId, questionId: null, indexStart: null, pageSize: 9999, sortField: null, orderType: null, searchDetail: null).ToList();
                //Check value is null
                if (surveyQuestion == null) return null;

                //Declare new list object survey question answer
                var surveyQuestionAnswers = new List<SurveyQuestionAnswerDTO>();
                //Iterate object
                foreach (var item in surveyQuestion)
                {
                    //Declare a SurveyQuestionAnswerDTO by using an object initializer and sending arguments for all properties.
                    var questionAnswer = new SurveyQuestionAnswerDTO
                    {
                        QuestionId = item.QuestionId,
                        QuestionDetail = item.QuestionDetail,
                        SurveyQuestionId = item.SurveyQuestionId,
                        TotalCount = item.TotalCount
                    };

                    //Get SurveyAnswer by SurveyQuestionId
                    var answer = _context.usp_SurveyAnswer_Select(surveyQuestionId: item.SurveyQuestionId, indexStart: null, pageSize: 9999, sortField: null, orderType: null, searchDetail: null).ToList();
                    //Check value is null
                    if (answer != null) questionAnswer.Answers.AddRange(answer);

                    //Add object to list object
                    surveyQuestionAnswers.Add(questionAnswer);
                }

                var viewLog = _context.usp_SurveyViewLog_Insert(surveyId: surveyId, iPAddress: GlobalFunction.GetIPAddress()).FirstOrDefault();

                //Set object to ViewBag
                ViewBag.Title = survey.FormName;
                ViewBag.Survey = survey;
                ViewBag.SurveyQuestionAnswer = surveyQuestionAnswers;
                ViewBag.ViewLog = viewLog.Result;
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }

        #endregion View

        #region Method

        /// <summary>
        /// form question insert
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult Question(FormCollection form)
        {
            try
            {
                var surveyId = Convert.ToInt32(form["survey_id"]);
                if (surveyId == 0) return RedirectToRoute("Fail");

                var viewLogId = Convert.ToInt32(form["viewlog_id"]);
                if (viewLogId == 0) return RedirectToRoute("Fail");

                //Check Duplicate
                var duplicate = _context.usp_SurveyResultDuplicateCheck_Select(surveyId: surveyId).FirstOrDefault();
                if (duplicate.IsResult.Value)
                {
                    TempData["Msg"] = duplicate.Msg;
                    return RedirectToRoute("Success");
                }

                var surveyList = new List<SurveyResultDTO>();
                //Split and Iterate
                var answers = form["answers"];
                var answersArray = answers.Split(',');
                foreach (var item in answersArray)
                {
                    var answer = item.Split(':');
                    //Index 0 = SQ (Survey Question)
                    var questionResult = Convert.ToInt32(Regex.Replace(answer[0], @"\D+", ""));
                    //Index 1 = SA (Survey Answer)
                    var surveyAnswerResult = Convert.ToInt32(Regex.Replace(answer[1], @"\D+", ""));
                    //Index 2 = A (Answer)
                    var answerResult = Convert.ToInt32(Regex.Replace(answer[2], @"\D+", ""));

                    //New object
                    var survey = new SurveyResultDTO
                    {
                        SurveyViewLogId = viewLogId,
                        SurveyId = surveyId,
                        SurveyQuestionId = questionResult,
                        SurveyAnswerId = surveyAnswerResult,
                        AnswerMore = (answerResult == 3) ? form[$"sub_suggest_{questionResult}"] : ""
                    };

                    //Add to List
                    surveyList.Add(survey);
                }

                //New object for Suggest
                var suggest = new SurveyResultDTO
                {
                    SurveyViewLogId = viewLogId,
                    SurveyId = surveyId,
                    SurveyQuestionId = Convert.ToInt32(form["suggest_surveyquestion_id"]),
                    SurveyAnswerId = null,
                    AnswerMore = form["suggest"]
                };

                //Add Suggest to List
                surveyList.Add(suggest);

                //Loop Insert to DB
                foreach (var s in surveyList)
                {
                    var tmp = _context.usp_TmpSurveyResult_Insert(surveyViewLogId: s.SurveyViewLogId, surveyId: s.SurveyId, surveyQuestionId: s.SurveyQuestionId, surveyAnswerId: s.SurveyAnswerId, answerMore: s.AnswerMore, createdBy: 1).FirstOrDefault();

                    if (!tmp.IsResult.Value)
                    {
                        TempData["Msg"] = tmp.Msg;
                        return RedirectToRoute("Fail");
                    }
                }

                //Call Stored Procedure Insert
                var result = _context.usp_SurveyResult_Insert(surveyViewLogId: viewLogId, surveyId: surveyId, createdBy: 1).FirstOrDefault();

                if (!result.IsResult.Value)
                {
                    TempData["Msg"] = result.Msg;
                    return RedirectToRoute("Fail");
                }

                var thank = _context.usp_Survey_Select(surveyId: surveyId, indexStart: null, pageSize: 9999, sortField: null, orderType: null, searchDetail: null).FirstOrDefault();
                TempData["Msg"] = thank.ThankNotation;
                TempData["Img"] = thank.IsLinkLine;

                return RedirectToRoute("Success");
            }
            catch (Exception e)
            {
                TempData["Msg"] = e.Message;
                return RedirectToRoute("Fail");
            }
        }

        #endregion Method
    }
}