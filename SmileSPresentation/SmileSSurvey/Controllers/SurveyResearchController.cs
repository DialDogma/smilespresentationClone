using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSSurvey.Models;
using SmileSSurvey.Helper;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Text;
using Serilog;
using ClaimOnlineSurveyorAPI.Contracts;
using MassTransit;
using static SmileSSurvey.Helper.ApiService;
using Newtonsoft.Json;

namespace SmileSSurvey.Controllers
{
    public class SurveyResearchController : Controller
    {
        private readonly string _surveyResearch = "SurveyResearch";
        private readonly string _surveyResearchClaim = "SurveyResearchClaim"; // From ClaimOnLine
        private readonly string _surveyFaxClaim = "SurveyFaxClaim"; // From SSSPH
        private readonly string _surveyBranch = "SurveyBranch";

        #region Constructor

        private readonly SurveyEntities _context;

        public SurveyResearchController() => _context = new SurveyEntities();

        #endregion Constructor

        #region Standard SurveyResearch

        private string StdSurveyResearch(string id, string MethodName)
        {
            //06958 Sahatsawat golf 2022-08-04
            Log.Information("Start [{MethodName}] at {DateTime} [Input] : {ClaimId}", MethodName, DateTime.Now, id);

            var newId = Encoding.UTF8.GetString(Convert.FromBase64String(id));
            int surveyId = Int32.Parse(newId);

            try
            {
                //Check Duplicate
                Log.Information("[{MethodName}] - [Store procedure]: usp_SurveyResultDuplicateCheck_Select [surveyId] : {surveyId}", MethodName, surveyId);
                var duplicate = _context.usp_SurveyResultDuplicateCheck_Select(surveyId: surveyId).FirstOrDefault();
                Log.Debug("[{MethodName}] - usp_SurveyResultDuplicateCheck_Select Result : {@duplicate}", MethodName, duplicate);

                if (duplicate.IsResult.Value)
                {
                    TempData["Msg"] = duplicate.Msg;
                    return "Success";
                }

                //Get Survey Detail
                Log.Information($"[{MethodName}] - [Store procedure]: usp_Survey_Select [Param]: surveyId={surveyId},indexStart={null},pageSize={999},sortField={null},orderType={null},searchDetail={null}");
                var survey = _context.usp_Survey_Select(surveyId: surveyId, indexStart: null, pageSize: 9999, sortField: null, orderType: null, searchDetail: null).FirstOrDefault();
                Log.Debug("[{MethodName}] - usp_Survey_SelequestionIdct Result : {@survey}", MethodName, survey);

                //Check value is null
                if (survey == null)
                {
                    Log.Warning("usp_Survey_SelequestionIdct Result = null");
                    TempData["Msg"] = $"survey id : {surveyId} is null";
                    return "Fail";
                }

                //check isSatisfaction
                if (survey.IsSatisfaction == true)
                {
                    return "Satisfaction";
                }

                Log.Information($"[{MethodName}] - [Store procedure]: usp_SurveyQuestion_Select [Param]: surveyId={surveyId},questionId=null,indexStart=null,pageSize={999},sortField=null,orderType=null,searchDetail=null");
                var SurveyQuestion = _context.usp_SurveyQuestion_Select(surveyId, questionId: null, indexStart: null, pageSize: 9999, sortField: null, orderType: null, searchDetail: null).ToList();
                Log.Debug("[{MethodName}] - usp_SurveyQuestion_Select Result : {@SurveyQuestion}", MethodName, SurveyQuestion);

                if (SurveyQuestion == null) return null;

                var answers = new List<usp_SurveyAnswer_Select_Result>();
                foreach (var item in SurveyQuestion)
                {
                    Log.Information($"[{MethodName}] - [Store procedure]: usp_SurveyAnswer_Select [Param]: SurveyQuestionId={item.SurveyQuestionId},indexStart=null,pageSize={999},sortField=null,orderType=null,searchDetail=null");
                    var answer = _context.usp_SurveyAnswer_Select(item.SurveyQuestionId, indexStart: null, pageSize: 9999, sortField: null, orderType: null, searchDetail: null).ToList();
                    Log.Debug("[{MethodName}] - usp_SurveyQuestion_Select Result : {@answer}", MethodName, answer);
                    if (answer != null) answers.AddRange(answer);
                }

                Log.Information($"[{MethodName}] - [Store procedure]: usp_SurveyViewLog_Insert [Param]: surveyId={surveyId}");
                var viewLog = _context.usp_SurveyViewLog_Insert(surveyId, iPAddress: GlobalFunction.GetIPAddress()).FirstOrDefault();
                Log.Debug("[{MethodName}] - usp_SurveyViewLog_Insert Result viewLog: {@viewLog}", MethodName, viewLog.Result);

                ViewBag.Title = survey.FormName;
                ViewBag.Survey = survey;
                ViewBag.SurveyQuestion = SurveyQuestion;
                ViewBag.SurveyAnswer = answers;
                ViewBag.ViewLog = viewLog.Result;
                ViewBag.SurveyRefId = survey.RefId;
                return "View";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{MethodName} error : " + ex.Message + "at {DateTime} [Input]: {id} ", MethodName, DateTime.Now, id);
                TempData["Msg"] = $"Error : {JsonConvert.SerializeObject(ex)}";
                return "Fail";
            }
        }

        #endregion Standard SurveyResearch

        #region Standard Insert Survey

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
                        AnswerMore = (answerResult == 17) ? form[$"suggest_{questionResult}"] : ""
                    };

                    //Add to List
                    surveyList.Add(survey);
                }

                var suggest_surveyquestion = Convert.ToInt32(form["suggest_surveyquestion_id"]);
                if (!(suggest_surveyquestion == 0))
                {
                    //New object for Suggest
                    var suggest = new SurveyResultDTO
                    {
                        SurveyViewLogId = viewLogId,
                        SurveyId = surveyId,
                        SurveyQuestionId = suggest_surveyquestion,
                        SurveyAnswerId = null,
                        AnswerMore = form["suggest"]
                    };

                    //Add Suggest to List
                    surveyList.Add(suggest);
                }

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
                Log.Information($"[Question] - [Store procedure]: usp_SurveyResult_Insert [Param]: surveyViewLogId={viewLogId},surveyId={surveyId},createdBy=1");
                var result = _context.usp_SurveyResult_Insert(surveyViewLogId: viewLogId, surveyId: surveyId, createdBy: 1).FirstOrDefault();
                Log.Debug("[Question] - usp_SurveyResult_Insert Result : {@result} ", result);

                if (!result.IsResult.Value)
                {
                    TempData["Msg"] = result.Msg;
                    return RedirectToRoute("Fail");
                }

                var thank = _context.usp_Survey_Select(surveyId: surveyId, indexStart: null, pageSize: 9999, sortField: null, orderType: null, searchDetail: null).FirstOrDefault();
                TempData["Msg"] = thank.ThankNotation;
                TempData["Img"] = thank.IsLinkLine;

                Log.Information("Success");
                return RedirectToRoute("Success");
            }
            catch (Exception e)
            {
                TempData["Msg"] = e.Message;
                return RedirectToRoute("Fail");
            }
        }

        #endregion Standard Insert Survey

        #region GetSurvey

        // GET: SurveyResearch
        [Route("sr/{id}")]
        public ActionResult SurveyResearch(string id)
        {
            var result = StdSurveyResearch(id, _surveyResearch);

            switch (result)
            {
                case "Success":
                    return RedirectToRoute("Success");
                    break;

                case "Fail":
                    return RedirectToRoute("Fail");
                    break;

                case "Satisfaction":
                    return RedirectToRoute("Satisfaction");
                    break;

                default:
                    return View();
                    break;
            }
        }

        #endregion GetSurvey

        #region SurveyMonitor

        [Authorization(Roles = "Developer")]
        [Route("sr/monitor")]
        public ActionResult SurveyMonitor()
        {
            ViewBag.FormList = _context.usp_Form_Select(null, null, 9999, null, null, null).ToList();

            return View();
        }

        public JsonResult GetSurveyMonitor(int? formId, int? draw, int? indexStart, int? pageSize, string sortField, string orderType, string searchDetail, string search)
        {
            var FormId = formId == -1 ? null : formId;
            var result = _context.usp_SurveyMonitorResult_Select(FormId, indexStart, pageSize, sortField, orderType, searchDetail, search).ToList().OrderBy(x => x.SurveyId);

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public void ExportSurveyMonitor(int? formId = null, string surveyId = null)
        {
            try
            {
                using (var db = new SurveyEntities())
                {
                    var result = db.usp_SurveyMonitorResult_Select((formId = formId == -1 ? null : formId), 0, 999999, "SurveyId", "asc", null, surveyId)
                        .Select(x => new
                        {
                            SurveyID = x.SurveyId,
                            FormName = x.FormName,
                            CustomerName = x.CustomerName,
                            BranchCode = x.BranchCode,
                            EmployeeCode = x.EmployeeCode,
                            QuestionDetail = x.QuestionDetail,
                            AnswerDetail = x.AnswerDetail,
                            AnswerMore = x.AnswerMore,
                            CreatedDate = x.CreatedDate.Value.Date.ToString("dd/MM/yyyy"),
                            ResultDate = x.ResultDate.Value.Date.ToString("dd/MM/yyyy"),
                        }).ToList();

                    GlobalFunction.ExportToExcel(HttpContext, result, "sheet1", "SurveyMonitor");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        #endregion SurveyMonitor

        #region GetClaimSurvey

        // GET: SurveyResearch
        [Route("srclaim/{id}")]
        public ActionResult SurveyResearchClaim(string id)
        {
            var result = StdSurveyResearch(id, _surveyResearchClaim);

            try
            {
                switch (result)
                {
                    case "Success":
                        return RedirectToRoute("Success");

                    case "Fail":
                        return RedirectToRoute("Fail");

                    case "Satisfaction":
                        return RedirectToRoute("Satisfaction");

                    case "View":
                        var stringValue = ViewBag.SurveyRefId;
                        var intValue = 0;
                        var success = Int32.TryParse(stringValue, out intValue);

                        if (success)
                        {
                            int RefId = Convert.ToInt32(ViewBag.SurveyRefId);

                            var parameter = new List<Parameter>();

                            parameter.Add(new Parameter
                            {
                                Key = "ClaimId",
                                Value = RefId
                            });

                            #region Mass transit

                            //RabbitMq
                            Log.Information("[SurveyResearchClaim] - Start RabbitMq at {DateTime}", DateTime.Now);
                            var message = new ClaimSurveyorRequest()
                            {
                                ClaimId = RefId
                            };

                            var client = MvcApplication.Bus.CreateRequestClient<ClaimSurveyorRequest>();
                            var response = client.GetResponse<ClaimSurveyorResponse, ClaimSurveyorNotFound>(message).Result;

                            #endregion Mass transit

                            if (response.Is<ClaimSurveyorNotFound>(out Response<ClaimSurveyorNotFound> notFound))
                            {
                                TempData["Msg"] = $"Error : Not Found";
                                return RedirectToRoute("Fail");
                            }

                            var messageResponse = response.Message as ClaimSurveyorResponse;

                            ViewBag.ClaimList = messageResponse.Result;

                            decimal? totalAmount = 0;
                            string serviceName = "";

                            foreach (var item in messageResponse.Result)
                            {
                                totalAmount += item.Amount;
                                serviceName = item.ServiceName;
                            }

                            ViewBag.TotalAmount = totalAmount;
                            ViewBag.ServiceName = serviceName;
                        }
                        else
                        {
                            return null;
                        }

                        break;

                    default:
                        // code block
                        break;
                }
                return View();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "SurveyResearchClaim error : " + ex.Message + "at {DateTime} [Input]: {id} ", DateTime.Now, id);
                TempData["Msg"] = $"Error : {JsonConvert.SerializeObject(ex)}";
                return RedirectToRoute("Fail");
            }
        }

        #endregion GetClaimSurvey

        #region GetFaxClaimSurvey

        // GET: Survey FaxClaim
        [Route("surveyfaxclaim/{id}")]
        public ActionResult SurveyFaxClaim(string id)
        {
            var result = StdSurveyResearch(id, _surveyFaxClaim);

            try
            {
                switch (result)
                {
                    case "Success":
                        return RedirectToRoute("Success");

                    case "Fail":
                        return RedirectToRoute("Fail");

                    case "Satisfaction":
                        return RedirectToRoute("Satisfaction");

                    case "View":
                        //เลข Claim
                        var stringValue = ViewBag.SurveyRefId;
                        var intValue = 0;
                        var success = Int32.TryParse(stringValue, out intValue);

                        //RestSharp
                        string apiEndPoint;
                        if (stringValue.Substring(0, 4) == "CLPA")
                        {
                            apiEndPoint = Path.Combine(Properties.Settings.Default.ssspa_api_URL, "api/Claim/");
                        }
                        else
                        {
                            apiEndPoint = Path.Combine(Properties.Settings.Default.sss_api_URL, "api/Claim/");
                        }

                        var parameter = new List<Parameter>();

                        parameter.Add(new Parameter
                        {
                            Key = "ClaimCode",
                            Value = stringValue
                        });
                        var responseApi = RequestApiProvider<CustomerProductResponseDTO>(null, parameter, "CustomerProductByClaim", false, apiEndPoint);

                        if (responseApi.isSuccess == true)
                        {
                            ViewBag.ClaimList = responseApi.data;
                        }

                        break;

                    default:
                        // code block
                        break;
                }
                return View();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "SurveyFaxClaim error : " + ex.Message + "at {DateTime} [Input]: {id} ", DateTime.Now, id);
                TempData["Msg"] = $"Error : {JsonConvert.SerializeObject(ex)}";
                return RedirectToRoute("Fail");
            }
        }

        #endregion GetFaxClaimSurvey

        #region GetSurveyBranch

        // GET: Survey branch
        [Route("surveybranch/{id}")]
        public ActionResult SurveyBranch(string id)
        {
            var result = StdSurveyResearchBranch(id, _surveyBranch);

            try
            {
                switch (result)
                {
                    case "Duplicate":
                        return RedirectToRoute("SurveyDuplicate");

                    case "Success":
                        return RedirectToRoute("SurveySuccess");

                    case "Fail":
                        return RedirectToRoute("Fail");

                    case "Satisfaction":
                        return RedirectToRoute("Satisfaction");

                    case "View":

                    default:
                        // code block
                        break;
                }
                return View();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "SurveyBranch error : " + ex.Message + "at {DateTime} [Input]: {id} ", DateTime.Now, id);
                TempData["Msg"] = $"Error : {JsonConvert.SerializeObject(ex)}";
                return RedirectToRoute("Fail");
            }
        }

        public ActionResult QuestionBranch(FormCollection form)
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
                    return RedirectToRoute("SurveySuccess");
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
                        AnswerMore = (answerResult == 17) ? form[$"suggest_{questionResult}"] : ""
                    };

                    //Add to List
                    surveyList.Add(survey);
                }

                var suggest_surveyquestion = Convert.ToInt32(form["suggest_surveyquestion_id"]);
                if (!(suggest_surveyquestion == 0))
                {
                    //New object for Suggest
                    var suggest = new SurveyResultDTO
                    {
                        SurveyViewLogId = viewLogId,
                        SurveyId = surveyId,
                        SurveyQuestionId = suggest_surveyquestion,
                        SurveyAnswerId = null,
                        AnswerMore = form["suggest"]
                    };

                    //Add Suggest to List
                    surveyList.Add(suggest);
                }

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
                Log.Information($"[Question] - [Store procedure]: usp_SurveyResult_Insert [Param]: surveyViewLogId={viewLogId},surveyId={surveyId},createdBy=1");
                var result = _context.usp_SurveyResult_Insert(surveyViewLogId: viewLogId, surveyId: surveyId, createdBy: 1).FirstOrDefault();
                Log.Debug("[Question] - usp_SurveyResult_Insert Result : {@result} ", result);

                if (!result.IsResult.Value)
                {
                    TempData["Msg"] = result.Msg;
                    return RedirectToRoute("Fail");
                }

                var thank = _context.usp_Survey_Select(surveyId: surveyId, indexStart: null, pageSize: 9999, sortField: null, orderType: null, searchDetail: null).FirstOrDefault();
                TempData["Msg"] = thank.ThankNotation;
                TempData["Img"] = thank.IsLinkLine;

                Log.Information("Success");
                return RedirectToRoute("SurveySuccess");
            }
            catch (Exception e)
            {
                TempData["Msg"] = e.Message;
                return RedirectToRoute("Fail");
            }
        }

        private string StdSurveyResearchBranch(string id, string MethodName)
        {
            //06958 Sahatsawat golf 2022-08-04
            Log.Information("Start [{MethodName}] at {DateTime} [Input] : {ClaimId}", MethodName, DateTime.Now, id);

            var newId = Encoding.UTF8.GetString(Convert.FromBase64String(id));
            int surveyId = Int32.Parse(newId);

            try
            {
                //Check Duplicate
                Log.Information("[{MethodName}] - [Store procedure]: usp_SurveyResultDuplicateCheck_Select [surveyId] : {surveyId}", MethodName, surveyId);
                var duplicate = _context.usp_SurveyResultDuplicateCheck_Select(surveyId: surveyId).FirstOrDefault();
                Log.Debug("[{MethodName}] - usp_SurveyResultDuplicateCheck_Select Result : {@duplicate}", MethodName, duplicate);

                if (duplicate.IsResult.Value)
                {
                    TempData["Msg"] = duplicate.Msg;
                    return "Duplicate";
                }

                //Get Survey Detail
                Log.Information($"[{MethodName}] - [Store procedure]: usp_Survey_Select [Param]: surveyId={surveyId},indexStart={null},pageSize={999},sortField={null},orderType={null},searchDetail={null}");
                var survey = _context.usp_Survey_Select(surveyId: surveyId, indexStart: null, pageSize: 9999, sortField: null, orderType: null, searchDetail: null).FirstOrDefault();
                Log.Debug("[{MethodName}] - usp_Survey_SelequestionIdct Result : {@survey}", MethodName, survey);

                //Check value is null
                if (survey == null)
                {
                    Log.Warning("usp_Survey_SelequestionIdct Result = null");
                    TempData["Msg"] = $"survey id : {surveyId} is null";
                    return "Fail";
                }

                //check isSatisfaction
                if (survey.IsSatisfaction == true)
                {
                    return "Satisfaction";
                }

                Log.Information($"[{MethodName}] - [Store procedure]: usp_SurveyQuestion_Select [Param]: surveyId={surveyId},questionId=null,indexStart=null,pageSize={999},sortField=null,orderType=null,searchDetail=null");
                var SurveyQuestion = _context.usp_SurveyQuestion_Select(surveyId, questionId: null, indexStart: null, pageSize: 9999, sortField: null, orderType: null, searchDetail: null).ToList();
                Log.Debug("[{MethodName}] - usp_SurveyQuestion_Select Result : {@SurveyQuestion}", MethodName, SurveyQuestion);

                if (SurveyQuestion == null) return null;

                var answers = new List<usp_SurveyAnswer_Select_Result>();
                foreach (var item in SurveyQuestion)
                {
                    Log.Information($"[{MethodName}] - [Store procedure]: usp_SurveyAnswer_Select [Param]: SurveyQuestionId={item.SurveyQuestionId},indexStart=null,pageSize={999},sortField=null,orderType=null,searchDetail=null");
                    var answer = _context.usp_SurveyAnswer_Select(item.SurveyQuestionId, indexStart: null, pageSize: 9999, sortField: null, orderType: null, searchDetail: null).ToList();
                    Log.Debug("[{MethodName}] - usp_SurveyQuestion_Select Result : {@answer}", MethodName, answer);
                    if (answer != null) answers.AddRange(answer);
                }

                Log.Information($"[{MethodName}] - [Store procedure]: usp_SurveyViewLog_Insert [Param]: surveyId={surveyId}");
                var viewLog = _context.usp_SurveyViewLog_Insert(surveyId, iPAddress: GlobalFunction.GetIPAddress()).FirstOrDefault();
                Log.Debug("[{MethodName}] - usp_SurveyViewLog_Insert Result viewLog: {@viewLog}", MethodName, viewLog.Result);

                ViewBag.Title = survey.FormName;
                ViewBag.Survey = survey;
                ViewBag.SurveyQuestion = SurveyQuestion;
                ViewBag.SurveyAnswer = answers;
                ViewBag.ViewLog = viewLog.Result;
                ViewBag.SurveyRefId = survey.RefId;
                return "View";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{MethodName} error : " + ex.Message + "at {DateTime} [Input]: {id} ", MethodName, DateTime.Now, id);
                TempData["Msg"] = $"Error : {JsonConvert.SerializeObject(ex)}";
                return "Fail";
            }
        }

        #endregion GetSurveyBranch
    }
}