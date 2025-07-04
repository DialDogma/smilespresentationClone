using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office2013.Excel;
using DocumentFormat.OpenXml.Vml;
using MassTransit.Saga.Pipeline.Filters;
using Newtonsoft.Json;
using Serilog;
using SmileSSurvey.Dtos;
using SmileSSurvey.Helper;
using SmileSSurvey.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using WebGrease.Css.Ast;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace SmileSSurvey.Controllers
{
    public class SurveyController : ApiController
    {
        private readonly SurveyEntities _context;

        public SurveyController()
        {
            _context = new SurveyEntities();
        }

        //Add Authorize
        //[Authorize]
        [HttpPost]
        [Route("sendsurvey/agent-feedback")]
        public IHttpActionResult SendSurveyAgentFeedback([FromBody] SurveyRequestDTO request)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                // ถ้าต้องการ ส่ง SMS Validate หมายเลขโทรศัพท์ที่ส่งมา
                if (request.IsSendSMS)
                {
                    var (IsValid, Message) = GlobalFunction.ValidatePhoneNumber(request.PhoneNumber);
                    if (!IsValid)
                    {
                        Log.Error("[{SendSurveyAgentFeedback}] - {message} : '{PhoneNumber}'.", Message, request.PhoneNumber);
                        throw new ArgumentException(Message);
                    }
                }
                var dateTimeNow = DateTime.Now;
                var userId = request.CreatedByUserId ?? 1;

                var form = _context.Form.FirstOrDefault(fo => fo.FormId == request.FormId && fo.IsActive == true);
                if (form is null)
                {
                    Log.Error("[{SendSurveyAgentFeedback}] - FormId '{FormId}' was not found.", request.FormId);
                    throw new ArgumentException("FormId  was not found.");
                }
                var survey = new Models.Survey()
                {
                    FormId = request.FormId,
                    PhoneNo = request.PhoneNumber.ToString(),
                    EmployeeId = userId,
                    IsRead = false,
                    IsSubmit = false,
                    IsSendNow = request.IsSendSMS,
                    SendDate = dateTimeNow,
                    Criteria = request.Criteria,
                    IsActive = true,
                    CreatedBy = userId,
                    CreatedDate = dateTimeNow,
                    UpdatedBy = userId,
                    UpdatedDate = dateTimeNow,
                    CheckinDate = request.CheckinDate ?? dateTimeNow
                };

                _context.Survey.Add(survey);
                _context.SaveChanges();

                var questions = _context.Question.AsNoTracking().Where(q => q.FormId == request.FormId && q.IsActive == true).ToList();
                if (questions.Count == 0)
                {
                    Log.Error("[{SendSurveyAgentFeedback}] - Question 'FormId :{FormId}'was not found.", request.FormId);
                    throw new ArgumentException("Question was not found.");
                }

                var surveyAnswers = new List<Models.SurveyAnswer>();
                foreach (var question in questions)
                {
                    var surveyQuestion = new Models.SurveyQuestion()
                    {
                        SurveyId = survey.SurveyId,
                        QuestionId = question.QuestionId,
                        IsActive = true,
                        CreatedBy = userId,
                        CreatedDate = dateTimeNow,
                        UpdatedBy = userId,
                        UpdatedDate = dateTimeNow
                    };
                    _context.SurveyQuestion.Add(surveyQuestion);
                    _context.SaveChanges();

                    var questionXanswer = _context.QuestionXAnswer.Where(qa => qa.QuestionId == question.QuestionId && qa.IsActive == true).ToList();
                    foreach (var qa in questionXanswer)
                    {
                        var answer = _context.Answer.AsNoTracking().FirstOrDefault(a => a.AnswerId == qa.AnswerId && a.IsActive == true);

                        var surveyAnswer = new SurveyAnswer()
                        {
                            AnswerId = answer.AnswerId,
                            SurveyQuestionId = surveyQuestion.SurveyQuestionId,
                            QuestionId = qa.QuestionId,
                            IsActive = true,
                            CreatedBy = userId,
                            CreatedDate = dateTimeNow,
                            UpdatedBy = userId,
                            UpdatedDate = dateTimeNow
                        };

                        surveyAnswers.Add(surveyAnswer);
                    }
                }

                _context.SurveyAnswer.AddRange(surveyAnswers);
                _context.SaveChanges();

                string surveyIdBase64Safe = Base64UrlExtensions.ToBase64Url(survey.SurveyId.ToString());
                // ตั้งค่า Culture เป็น Thai (Thailand)
                CultureInfo thaiCulture = new CultureInfo("th-TH");
                string thaiDate = survey.CheckinDate.Value.ToString("dd/MM/yyyy", thaiCulture);

                var sendmessage = string.Format(form.MessageTemplate.ToString(), thaiDate, surveyIdBase64Safe);

                if (request.IsSendSMS == true)
                {
                    // SMSType 58 = ระบบริหารงานพื้นที่ (BM) แบบประเมินความพึงพอใจ
                    var sms = GlobalFunction.SendSMS(sendmessage, request.PhoneNumber, 58, survey.CreatedBy.ToString());
                    //ถ้า return กลับมา OK
                    if (sms.Status == "000")
                    {
                        //update ผลการส่ง sms
                        var smsId = Int32.Parse(sms.SmsId);
                        survey.SmsId = smsId;
                        survey.UpdatedBy = userId;
                        survey.UpdatedDate = DateTime.Now;
                    }
                    else
                    {
                        //update ผลการส่ง sms fail
                        survey.Remark = sms.Detail;
                        survey.UpdatedBy = userId;
                        survey.UpdatedDate = DateTime.Now;
                    }

                    _context.Entry(survey).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                // ดึงเฉพาะ URL จากข้อความใน sendmessage
                string extractedUrl = Regex.Match(sendmessage, @"http[s]?:\/\/[^\s]+").Value;
                // Commit the transaction.
                transaction.Commit();
                var response = new ResultDtoResponse()
                {
                    IsResult = true,
                    Result = $"{survey.SurveyId}",
                    Msg = "Success",
                    Url = request.IsSendSMS == false ? extractedUrl : null
                };

                return Json(response, GlobalObject.carmelSetting());
            }
            catch (Exception ex)
            {
                // Rollback transaction.
                transaction.Rollback();
                var response = new ResultDtoResponse()
                {
                    IsResult = false,
                    Result = null,
                    Msg = ex.Message
                };

                Log.Error(ex, "[{SendSurveyAgentFeedback}] - An error occurred.");
                return Json(response, GlobalObject.carmelSetting());
            }
        }

        [HttpGet]
        [Route("survey/result/{surveyId}")]
        public IHttpActionResult SurveyResult(int surveyId)
        {
            try
            {
                var result = from survey in _context.Survey
                             join form in _context.Form on survey.FormId equals form.FormId
                             join results in _context.SurveyResult on survey.SurveyId equals results.SurveyId
                             join question in _context.Question on results.QuestionId equals question.QuestionId
                             join resultans in _context.SurveyResultAnswer on results.SurveyResultId equals resultans.SurveyResultId
                             join answer in _context.Answer on resultans.AnswerId equals answer.AnswerId into answerGroup
                             from answer in answerGroup.DefaultIfEmpty() // Handles null values for answers
                             where survey.SurveyId == surveyId
                             group new { resultans, answer, question } by new
                             {
                                 survey.SurveyId,
                                 form.FormId,
                                 form.FormName,
                                 survey.EmployeeId,
                                 survey.Criteria,
                                 survey.IsSendNow,
                                 survey.IsRead,
                                 survey.IsSubmit,
                             } into grouped
                             select new SurveyResultReportDTO
                             {
                                 SurveyId = grouped.Key.SurveyId,
                                 FormId = grouped.Key.FormId,
                                 FormName = grouped.Key.FormName,
                                 EmployeeId = grouped.Key.EmployeeId,
                                 Criteria = grouped.Key.Criteria,
                                 IsSendSMS = grouped.Key.IsSendNow,
                                 IsRead = grouped.Key.IsRead,
                                 IsSubmit = grouped.Key.IsSubmit,
                                 Questions = grouped.GroupBy(q => new { q.question.QuestionId, q.question.QuestionDetail })
                                 .Select(g => new SurveyResultQuestionReportDTO
                                 {
                                     QuestionId = g.Key.QuestionId,
                                     QuestionDetail = g.Key.QuestionDetail,
                                     Answers = g.Select(a => new SurveyResultAnswerReportDTO
                                     {
                                         AnswerId = a.answer.AnswerId,
                                         AnswerDetail = a.answer.AnswerDetail,
                                         AnswerMore = a.resultans.AnswerMore
                                     }).ToList()
                                 }).ToList()
                             };

                if (result.Count() == 0)
                {
                    Log.Error("[{SurveyResult}] -  SurveyId :{SurveyId} was not found.", surveyId);
                    throw new ArgumentException("Survey was not found.");
                }

                return Json(result, GlobalObject.carmelSetting());
            }
            catch (Exception ex)
            {
                var response = new ResultDtoResponse()
                {
                    IsResult = false,
                    Result = null,
                    Msg = ex.Message
                };

                Log.Error(ex, "[{SurveyResult}] - An error occurred.");
                return Json(response, GlobalObject.carmelSetting());
            }
        }
    }
}