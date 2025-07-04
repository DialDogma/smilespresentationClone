using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Office2013.Excel;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using SmileSSurvey.Dtos;
using SmileSSurvey.Helper;
using SmileSSurvey.Models;
using SmileSSurvey.ViewModels;

namespace SmileSSurvey.Controllers
{
    public class AgentFeedbackController : Controller
    {
        private readonly SurveyEntities _context;

        public AgentFeedbackController()
        {
            _context = new SurveyEntities();
        }

        [Route("agent-feedback/{surveyIdBase64}")]
        public ActionResult AgentFeedback(string surveyIdBase64)
        {
            try
            {
                var surveyId = 0;
                var validateSurveyId = GlobalFunction.IsValidBase64(surveyIdBase64, out surveyId);
                if (!validateSurveyId)
                {
                    TempData["Msg"] = "เกิดข้อผิดพลาด! รหัสแบบสอบถามไม่ถูกต้อง";
                    return RedirectToAction("FeedbackFail");
                }

                var survey = _context.Survey.Where(x => x.SurveyId == surveyId).FirstOrDefault();
                //ตรวจสอบสถานะ issubmit
                if (survey == null)
                {
                    TempData["Msg"] = "เกิดข้อผิดพลาด! ไม่พบแบบสอบถาม";
                    return RedirectToAction("FeedbackFail");
                }
                ;

                survey.IsRead = true; // แก้ไขค่า
                survey.UpdatedDate = DateTime.Now;
                _context.SaveChanges(); // บันทึกการเปลี่ยนแปลง

                var formMsg = _context.Form.Where(f => f.FormId == survey.FormId).AsNoTracking().Select(f => f.ThankNotation).FirstOrDefault(); // ค้นหา Survey ตาม Id
                if (survey.IsSubmit == true)
                {
                    TempData["Msg"] = "ส่งแบบประเมินเรียบร้อยแล้ว";
                    TempData["formMsg"] = formMsg;
                    return RedirectToAction("FeedbackSuccess");
                }
                ;

                var surveyQuestions = _context.SurveyQuestion.Where(sq => sq.SurveyId == surveyId).ToList();
                var surveyQuestionsId = surveyQuestions.Select(sq => sq.SurveyQuestionId);

                var questionAnswerList = new List<QuestionAnswerViewModelDTO>();
                foreach (var qt in surveyQuestions)
                {
                    var question = _context.Question.Where(q => q.QuestionId == qt.QuestionId).FirstOrDefault();
                    var surveyAnswers = _context.SurveyAnswer.Where(sa => sa.SurveyQuestionId == qt.SurveyQuestionId && sa.QuestionId == question.QuestionId).ToList();

                    var questionAnswer = new QuestionAnswerViewModelDTO();

                    questionAnswer.QuestionDetail = question.QuestionDetail;
                    questionAnswer.SurveyQuestionId = qt.SurveyQuestionId;

                    var anwsList = new List<AnswerViewModelDTO>();
                    foreach (var answers in surveyAnswers)
                    {
                        var answer = _context.Answer.Where(a => a.AnswerId == answers.AnswerId).FirstOrDefault();

                        var anws = new AnswerViewModelDTO();
                        anws.SurveyAnswerId = answers.SurveyAnswerId;
                        anws.AnswerDetail = answer.AnswerDetail;

                        anwsList.Add(anws);
                    }
                    questionAnswer.Answers.AddRange(anwsList);
                    questionAnswerList.Add(questionAnswer);
                }

                var viewLog = new SurveyViewLog { SurveyId = surveyId, IPAddress = GlobalFunction.GetIPAddress(), UserAgent = GlobalFunction.GetUserAgent(), CreatedDate = DateTime.Now };
                _context.SurveyViewLog.Add(viewLog);
                _context.SaveChanges();

                CultureInfo thaiCulture = new CultureInfo("th-TH");
                var date = survey.CreatedDate;
                string thaiDate = date.HasValue ? date.Value.ToString("dd/MM/yyyy", thaiCulture) : "N/A"; // Handle null as appropriate

                string thaiCheckinDate = survey.CheckinDate?.ToString("dd/MM/yyyy", thaiCulture);

                ViewBag.QuestionAnswerViewModel = questionAnswerList;
                ViewBag.Criteria = survey.Criteria;
                ViewBag.CreatedDate = thaiDate;
                ViewBag.CheckinDate = thaiCheckinDate;
                ViewBag.SurveyId = surveyId;
                ViewBag.ViewLogId = viewLog.SurveyViewLogId;

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                Response.Cache.SetSlidingExpiration(false);

                return View();
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "เกิดข้อผิดพลาด! ไม่พบแบบสอบถาม";
                return RedirectToAction("FeedbackFail");
            }
        }

        [HttpPost]
        public ActionResult SubmitFeedback(FeedbackViewModel form)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                // สำหรับ validate คำถามและคำตอบที่ส่งมา
                if (!IsValidSurveyForm(form))
                {
                    TempData["Msg"] = "เกิดข้อผิดพลาด! ไม่พบแบบสอบถาม";
                    return RedirectToAction("FeedbackFail");
                }

                var survey = _context.Survey.Where(x => x.SurveyId == form.SurveyId).FirstOrDefault();
                var formMsg = _context.Form.Where(f => f.FormId == survey.FormId).AsNoTracking().Select(f => f.ThankNotation).FirstOrDefault(); // ค้นหา Survey ตาม Id
                if (survey.IsSubmit == true)
                {
                    TempData["Msg"] = "ส่งแบบประเมินเรียบร้อยแล้ว";
                    TempData["formMsg"] = formMsg;
                    return RedirectToAction("FeedbackSuccess");
                }
                ;

                var surveyResults = new List<SurveyQuestionResultDTO>();

                // Process Survey Question 1
                surveyResults.Add(new SurveyQuestionResultDTO
                {
                    SurveyQuesionId = form.SurveyQuestion1,
                    SurveyAnswers = new List<SurveyAnswerDTO>
                {
                    new SurveyAnswerDTO { SurveyAnswerId = form.SurveyAnswers1 }
                }
                });

                // Process Survey Question 2
                var answers2 = form.SurveyAnswers2
                    ?.Where(answer => !string.IsNullOrEmpty(answer)) // ตรวจสอบว่า answer ไม่เป็น null หรือว่าง
                    .SelectMany(answer =>
                    {
                        try
                        {
                            var numericAnswers = JsonConvert.DeserializeObject<List<int>>(answer) ?? new List<int>(); // ตรวจสอบ null
                            return numericAnswers
                                .Select(numericAnswer => new SurveyAnswerDTO { SurveyAnswerId = numericAnswer })
                                .ToList();
                        }
                        catch (JsonException)
                        {
                            return new List<SurveyAnswerDTO>(); // กรณีเกิดข้อผิดพลาดในการแปลง JSON
                        }
                    })
                    .ToList() ?? new List<SurveyAnswerDTO>(); // ป้องกัน null reference

                if (answers2.Count > 0)
                {
                    surveyResults.Add(new SurveyQuestionResultDTO
                    {
                        SurveyQuesionId = form.SurveyQuestion2,
                        SurveyAnswers = answers2
                    });
                }

                // Process Survey Question 3
                if (!form.SurveyAnswers3.IsNullOrWhiteSpace())
                {
                    surveyResults.Add(new SurveyQuestionResultDTO
                    {
                        SurveyQuesionId = form.SurveyQuestion3,
                        SurveyAnswers = new List<SurveyAnswerDTO>
                {
                    new SurveyAnswerDTO { Text = form.SurveyAnswers3 }
                }
                    });
                }
                var tmpSurveyResultInsertList = new List<TmpSurveyResult>();
                var surveyResultAnswerInsertList = new List<SurveyResultAnswer>();

                foreach (var result in surveyResults)
                {
                    var questionId = _context.SurveyQuestion.Where(sq => sq.SurveyQuestionId == result.SurveyQuesionId).Select(sq => sq.QuestionId).FirstOrDefault();

                    var surveyResult = new SurveyResult
                    {
                        SurveyId = form.SurveyId,
                        SurveyViewLogId = form.ViewLogId,
                        SurveyQuestionId = result.SurveyQuesionId,
                        QuestionId = questionId,
                        IsActive = true,
                        CreatedBy = 1,
                        CreatedDate = DateTime.Now,
                        UpdatedBy = 1,
                        UpdatedDate = DateTime.Now,
                    };

                    _context.SurveyResult.Add(surveyResult);
                    _context.SaveChanges();

                    foreach (var item in result.SurveyAnswers)
                    {
                        var answerId = _context.SurveyAnswer.Where(sa => sa.SurveyAnswerId == item.SurveyAnswerId).Select(sa => sa.AnswerId).FirstOrDefault();
                        var tmpSurveyResultInsert = new TmpSurveyResult
                        {
                            SurveyId = form.SurveyId,
                            SurveyViewLogId = form.ViewLogId,
                            SurveyQuestionId = result.SurveyQuesionId,
                            SurveyAnswerId = item.SurveyAnswerId == 0 ? (int?)null : item.SurveyAnswerId, // ตรวจสอบค่า 0 แล้วกำหนดเป็น null
                            AnswerMore = item.Text,
                            IsActive = true,
                            CreatedBy = 1,
                            CreatedDate = DateTime.Now,
                            UpdatedBy = 1,
                            UpdatedDate = DateTime.Now,
                        };
                        var surveyResultAnswer = new SurveyResultAnswer
                        {
                            SurveyResultId = surveyResult.SurveyResultId,
                            AnswerId = answerId,
                            SurveyAnswerId = item.SurveyAnswerId == 0 ? (int?)null : item.SurveyAnswerId, // ตรวจสอบค่า 0 แล้วกำหนดเป็น null
                            AnswerMore = item.Text,
                            IsActive = true,
                            CreatedBy = 1,
                            CreatedDate = DateTime.Now,
                            UpdatedBy = 1,
                            UpdatedDate = DateTime.Now,
                        };

                        tmpSurveyResultInsertList.Add(tmpSurveyResultInsert);
                        surveyResultAnswerInsertList.Add(surveyResultAnswer);
                    }
                }

                //var survey = _context.Survey.FirstOrDefault(s => s.SurveyId == form.SurveyId); // ค้นหา Survey ตาม Id

                if (survey != null)
                {
                    survey.IsSubmit = true; // แก้ไขค่า
                    survey.UpdatedDate = DateTime.Now; // ปรับค่าหรือเพิ่มฟิลด์อื่น ๆ ตามต้องการ

                    _context.SaveChanges(); // บันทึกการเปลี่ยนแปลง
                }

                _context.TmpSurveyResult.AddRange(tmpSurveyResultInsertList);
                _context.SaveChanges();

                _context.SurveyResultAnswer.AddRange(surveyResultAnswerInsertList);
                _context.SaveChanges();

                // Commit the transaction.
                transaction.Commit();

                //var formMsg = _context.Form.Where(f => f.FormId == survey.FormId).Select(f => f.ThankNotation).FirstOrDefault(); // ค้นหา Survey ตาม Id
                TempData["Msg"] = "ขอขอบคุณ";
                TempData["formMsg"] = formMsg;

                return RedirectToAction("FeedbackSuccess");
            }
            catch (Exception ex)
            {
                // Rollback transaction.
                transaction.Rollback();

                TempData["Msg"] = "เกิดข้อผิดพลาด! ไม่พบแบบสอบถาม";
                return RedirectToAction("FeedbackFail");
            }
        }

        private bool IsValidSurveyForm(FeedbackViewModel form)
        {
            // Validate Survey Question 1
            if (form.SurveyQuestion1 == 0) return false;

            // Validate Survey Question 2
            if (form.SurveyQuestion2 == 0 && (form.SurveyAnswers2 == null || !form.SurveyAnswers2.Any()))
                return false;

            // Validate Survey Question 3
            if (form.SurveyQuestion3 == 0) return false;

            return true;
        }

        #region ResponsePages

        [Route("fb-success")]
        public ActionResult FeedbackSuccess()
        {
            ViewBag.Msg = TempData["Msg"];
            ViewBag.formMsg = TempData["formMsg"];

            return View();
        }

        [Route("fb-fail")]
        public ActionResult FeedbackFail()
        {
            ViewBag.Msg = TempData["Msg"];
            ViewBag.formMsg = TempData["formMsg"];

            return View();
        }

        #endregion ResponsePages
    }
}