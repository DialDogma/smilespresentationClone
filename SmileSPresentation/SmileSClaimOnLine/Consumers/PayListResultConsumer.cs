using MassTransit;
using Newtonsoft.Json;
using PayTransferAPI.Contract;
using RestSharp;
using Serilog;
using SmileSClaimOnLine.Controllers;
using SmileSClaimOnLine.DTOs;
using SmileSClaimOnLine.Helper;
using SmileSClaimOnLine.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Interop;
using static SmileSClaimOnLine.Models.SmsModel;

namespace SmileSClaimOnLine.Consumers
{
    public class PayListResultConsumer : IConsumer<PayListResult>
    {
        private const string _controllerName = nameof(PayListResultConsumer);
        public Task Consume(ConsumeContext<PayListResult> context)
        {
            int? claimOnLineId = null;
            try
            {
                var message = context.Message;
                Log.Information("{_controllerName} - Consume [message: {@message}]", _controllerName, message);

                ClaimOnLineDBContext dataBase = new ClaimOnLineDBContext();
                Guid? PayListHeaderId = message.RefCode;
                var jSonContext = JsonConvert.SerializeObject(message);
                //Select TmpClaimOnLineTransfer
                var col = dataBase.usp_TmpClaimOnLineTransfer_Select(PayListHeaderId).FirstOrDefault();

                DateTime? transferDate = message.TransferDate;
                int? userId = col.CreateByUserId;
                claimOnLineId = col.ClaimOnLineId;
                double? transferAmountTotal = col.Amount;
                int? ClaimOnLineTransferId = col.ClaimOnLineTransferId;
                int claimOnLineStatusId = message.IsSucceed ? 3 : 6; //3 ส่วนกลางโอนเงินแล้ว, 6 ไม่สำเร็จ
                var statusBank = message.StatusBank;

                //--------------------Mokup Test--------------------//
                //col.SMSPhoneNo = "0929103290";
                //message.IsSucceed = true;

                //if (message.IsSucceed)
                //{
                //    statusBank = "000";
                //    claimOnLineStatusId = 3;
                //}
                //else
                //{
                //    statusBank = "012";
                //    claimOnLineStatusId = 6;
                //}
                //--------------------------------------------------//

                Log.Information("{_controllerName} - Consume claimOnLineId = {claimOnLineId} [message: {@message}]", _controllerName, claimOnLineId, message);

                if (col.ClaimOnLineStatusId != 6) //อยู่ระหว่างดำเนินการโอนเงิน
                {
                    Log.Debug($"{_controllerName} Consume - 1");
                    var TmpPayAutoTypeId = dataBase.TmpClaimOnLinePayAuto_Transaction.Where(x => x.PayListHeaderId == PayListHeaderId).Select(x => x.PayAutoTypeId).FirstOrDefault();

                    //----------------------------------------------------------- บันทึกประวัติการทำรายการ ----------------------------------------------------------------
                    //เป็นได้ทั้ง PayAutoType = อนุมัติ (2), โอนเพิ่ม (4)
                    var payAutoType = TmpPayAutoTypeId;
                    Log.Information($"{_controllerName} Consume - บันทึก ประวัติการทำรายการ สถานะ: {payAutoType} claimOnLineId = {claimOnLineId}");
                    ClaimOnLinePayAuto_Transaction_Insert(1, payAutoType, userId, null, (decimal)transferAmountTotal, null, null, claimOnLineId, PayListHeaderId, statusBank);

                    var survey = GetLinkSMSMesssageSurvey(col, true);

                    if (message.IsSucceed)
                    {
                        //ส่ง SMS แจ้งโอนเงิน
                        var resultSMS = SendSMS(col, survey.msg);

                        //----------------------------------------------------------- บันทึกประวัติการทำรายการ ----------------------------------------------------------------
                        Log.Information($"{_controllerName} Consume - บันทึก ประวัติการทำรายการ สถานะ: โอนเงินสำเร็จ claimOnLineId = {claimOnLineId}");
                        ClaimOnLinePayAuto_Transaction_Insert(1, 3, userId, transferDate, (decimal)transferAmountTotal, message.TransRefNo, jSonContext, claimOnLineId, PayListHeaderId, statusBank);

                        Log.Debug($"{_controllerName} Consume - บันทึก ClaimOnLineTransfer claimOnLineId = {claimOnLineId}");
                        var result = ClaimOnLineTransfer_PayAuto_Insert(claimOnLineId, transferDate, transferAmountTotal, userId, resultSMS, col.SMSPhoneNo, survey.MsgSurvey, PayListHeaderId, claimOnLineStatusId, col.TransferFromMenu, statusBank);

                        if (result.IsResult == true)
                            DeleteTmp(PayListHeaderId, claimOnLineId);
                    }
                    else
                    {
                        Log.Information($"{_controllerName} Consume - บันทึก ประวัติการทำรายการ สถานะ: โอนเงินไม่สำเร็จ claimOnLineId = {claimOnLineId}");
                        ClaimOnLinePayAuto_Transaction_Insert(1, 6, userId, null, (decimal)transferAmountTotal, null, null, claimOnLineId, PayListHeaderId, statusBank);
                        ClaimOnLineTransfer_PayAuto_Insert(claimOnLineId, transferDate, transferAmountTotal, userId, null, col.SMSPhoneNo, survey.MsgSurvey, PayListHeaderId, claimOnLineStatusId, col.TransferFromMenu, statusBank);
                    }
                }
                else if (message.IsSucceed) //ไม่สำเร็จมาก่อน
                {
                    var survey = GetLinkSMSMesssageSurvey(col, false);
                    //ส่ง SMS แจ้งโอนเงิน
                    var resultSMS = SendSMS(col, survey.msg);

                    //Insert PayAuto_Transaction เก็บประวัติการทำรายการ
                    if (col.TransferFromMenu == 2)
                    {
                        Log.Information($"{_controllerName} Consume - บันทึก ประวัติการทำรายการ สถานะ: โอนเพิ่ม claimOnLineId = {claimOnLineId}");
                        ClaimOnLinePayAuto_Transaction_Insert(1, 4, userId, null, (decimal)transferAmountTotal, null, null, claimOnLineId, PayListHeaderId, statusBank);
                    }

                    Log.Information($"{_controllerName} Consume - บันทึก ประวัติการทำรายการ สถานะ: โอนเงินสำเร็จ claimOnLineId = {claimOnLineId}");
                    ClaimOnLinePayAuto_Transaction_Insert(1, 3, userId, transferDate, (decimal)transferAmountTotal, message.TransRefNo, jSonContext, claimOnLineId, PayListHeaderId, statusBank);

                    //Update ClaimOnLineTransfer
                    DateTime UpdateDate = DateTime.Now;
                    bool IsSendSMS = resultSMS.Item1.IsSuccessful;
                    var SMSReferenceId = resultSMS.Item2.ReferenceId;
                    Log.Debug($"{_controllerName} Consume - อัพเดต ClaimOnLineTransfer claimOnLineId = {claimOnLineId}");
                    Log.Debug($"{_controllerName} - Consume [Store procedure]: usp_ClaimOnLineTransfer_PayAuto_Update [ClaimOnLineTransferId = {ClaimOnLineTransferId}, ClaimOnLineId = {col.ClaimOnLineId}, PayListHeaderId = {PayListHeaderId}, createByUserId = {userId}, UpdateDate = {UpdateDate}, IsSendSMS = {IsSendSMS}, SMSReferenceId = {SMSReferenceId}, statusBank = {statusBank}, transferDate = {transferDate}]");
                    var result = dataBase.usp_ClaimOnLineTransfer_PayAuto_Update(ClaimOnLineTransferId, col.ClaimOnLineId, PayListHeaderId, UpdateDate, IsSendSMS, SMSReferenceId, statusBank, transferDate).FirstOrDefault();
                    Log.Information("{_controllerName} - Consume [Result: {@result}]", _controllerName, result);

                    if (result.IsResult == true)
                        DeleteTmp(PayListHeaderId, claimOnLineId);
                }
                else
                {
                    Log.Information($"{_controllerName} Consume - บันทึก ประวัติการทำรายการ สถานะ: ไม่สำเร็จ claimOnLineId = {claimOnLineId}");
                    ClaimOnLinePayAuto_Transaction_Insert(1, 6, userId, null, (decimal)transferAmountTotal, null, null, claimOnLineId, PayListHeaderId, statusBank);

                    //update status
                    var PremiumSourceStatusId = dataBase.PremiumSourceStatus.Where(x => x.MapperStatus_BBL == statusBank).ToList();
                    if (PremiumSourceStatusId.Any())
                    {
                        bool? IsAllowReTransfer = true;
                        if (PremiumSourceStatusId[0].BlockReTransfer.Value)
                        {
                            IsAllowReTransfer = false;
                        }
                        ClaimOnLineDBContext dbCT = new ClaimOnLineDBContext();
                        var d = dbCT.ClaimOnLineTransfer.Where(x => x.ClaimOnLineTransferId == ClaimOnLineTransferId).FirstOrDefault();
                        d.PremiumSourceStatusId = PremiumSourceStatusId[0].PremiumSourceStatusId;
                        d.IsAllowReTransfer = IsAllowReTransfer;
                        dbCT.SaveChanges();
                    }

                    Log.Information("UpdateClaimOnLineStatusId:6 claimOnLineId = {claimOnLineId}", claimOnLineId);
                    new PayAutoController().UpdateClaimOnLineStatusId(claimOnLineId, 6);//6 ไม่สำเร็จ
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - Consume Error claimOnLineId = {claimOnLineId} : {Message}", _controllerName, claimOnLineId, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Tuple<IRestResponse, SMS_Result> SendSMS(usp_TmpClaimOnLineTransfer_Select_Result c, string SMSMessage)
        {
            IRestResponse response = null;
            SMS_Result sms = new SMS_Result();
            try
            {
                if (SMSMessage == "")
                    return Tuple.Create(response, sms);

                //---------------------------------------------------------- ส่ง SMS -------------------------------------------------------------------------------
                //Call Service Send SMS
                var client = new RestClient(Properties.Settings.Default.SMSURL.Trim());
                //24 จ่ายสินไหม เคลมออนไลน์
                var param = new { Reference = c.ClaimOnLineCode, SMSTypeId = 24, Message = SMSMessage, PhoneNo = c.SMSPhoneNo, CreatedById = c.CreateByUserId };
                //Add Json Body to Request
                var request = new RestRequest().AddJsonBody(param);
                //Add Header Token
                request.AddHeader("Authorization", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwcm9qZWN0aWQiOjl9.XX7R_Ik8pydv2e04ZvrDew8tlszSDrjvTEYKdgO4t7A");
                //Post Request
                Log.Debug("{_controllerName} - SendSMS claimOnLineId = {claimOnLineId} Start to Send SMS [request = {param}]", _controllerName, c.ClaimOnLineId, param);
                response = client.Post(request);
                Log.Debug("{_controllerName} - SendSMS claimOnLineId = {claimOnLineId} SMS Json response [response = {@response}]", _controllerName, c.ClaimOnLineId, response);

                //Deserialize JSON
                sms = JsonConvert.DeserializeObject<SMS_Result>(response.Content);
                Log.Information("{_controllerName} - SendSMS SMS DeserializeObject Response [response = {@sms}]", _controllerName, sms);
                return Tuple.Create(response, sms);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - SendSMS Error claimOnLineId = {claimOnLineId} : {Message}", _controllerName, c.ClaimOnLineId, ex.Message);
                return Tuple.Create(response, sms);
            }
        }

        public usp_ClaimOnLineTransfer_PayAuto_Insert_Result ClaimOnLineTransfer_PayAuto_Insert(int? claimOnLineId, DateTime? transferDate, double? transferAmountTotal, int? userId, Tuple<IRestResponse, SMS_Result> resultSMS, string sMSPhoneNo, string sMSMessageSurvey, Guid? payListHeaderId, int claimOnLineStatusId, int? transferFormMenu, string statusBank = null, bool? isSuccessTmpPayList = null)
        {
            ClaimOnLineDBContext dataBase = new ClaimOnLineDBContext();
            //Insert value TmpClaimOnLinePayAuto_Transaction to ClaimOnLinePayAuto_Transaction
            bool isSendSMS = resultSMS == null ? false : resultSMS.Item1.IsSuccessful;
            string smsReferanceId = resultSMS == null ? null : resultSMS.Item2.ReferenceId;
            isSuccessTmpPayList = isSuccessTmpPayList == null ? true : false;
            Log.Debug($"{_controllerName} - ClaimOnLineTransfer_PayAuto_Insert [Store procedure]: usp_ClaimOnLineTransfer_PayAuto_Insert [claimOnLineId = {claimOnLineId}, transferDate = {transferDate}, transferAmountTotal = {transferAmountTotal}, createByUserId = {userId}, isSendSMS = {isSendSMS}, SMSReferenceId = {smsReferanceId}, SMSPhoneNo = {sMSPhoneNo}, msgSurvey = {sMSMessageSurvey}, payListHeaderId = {payListHeaderId}, claimOnLineStatusId = {claimOnLineStatusId}, transferFormMenu = {transferFormMenu}, premiumSourceStatusId = {statusBank}, isSuccessTmpPayList = {isSuccessTmpPayList}]");
            var result = dataBase.usp_ClaimOnLineTransfer_PayAuto_Insert(claimOnLineId, transferDate, transferAmountTotal, userId, isSendSMS, smsReferanceId, sMSPhoneNo, sMSMessageSurvey, payListHeaderId, claimOnLineStatusId, statusBank, transferFormMenu, isSuccessTmpPayList).FirstOrDefault();
            Log.Information("{_controllerName} - ClaimOnLineTransfer_PayAuto_Insert claimOnLineId = {claimOnLineId} [Result]: {@result}", _controllerName, claimOnLineId, result);
            return result;
        }

        private void DeleteTmp(Guid? PayListHeaderId, int? ClaimOnLineId)
        {
            Log.Debug("{_controllerName} - DeleteTmp - Start DeleteTmp [PayListHeaderId = {PayListHeaderId}, ClaimOnLineId = {ClaimOnLineId}]", _controllerName, PayListHeaderId, ClaimOnLineId);
            ClaimOnLineDBContext dataBase = new ClaimOnLineDBContext();
            var recordsToDelete1 = dataBase.TmpClaimOnLinePayAuto_Transaction.Where(tmp => tmp.PayListHeaderId == PayListHeaderId);
            var recordsToDelete2 = dataBase.TmpClaimOnLineTransfer.Where(tmp => tmp.PayListHeaderId == PayListHeaderId);
            dataBase.TmpClaimOnLinePayAuto_Transaction.RemoveRange(recordsToDelete1);
            dataBase.TmpClaimOnLineTransfer.RemoveRange(recordsToDelete2);
            dataBase.SaveChanges();
            Log.Debug("{_controllerName} - DeleteTmp - DeleteTmp Success [PayListHeaderId = {PayListHeaderId}, ClaimOnLineId = {ClaimOnLineId}] ", _controllerName, PayListHeaderId, ClaimOnLineId);
        }

        public void ClaimOnLinePayAuto_Transaction_Insert(int type, int? payAutoTypeId, int? createdByUserId, DateTime? paymentDate, decimal paymentAmount, string transRefNo, string payTransferResponse, int? claimOnLineId, Guid? payListHeaderId, string statusBank)
        {
            try
            {
                ClaimOnLineDBContext dataBase = new ClaimOnLineDBContext();
                var Code = new PayAutoController().GenerateCode("CT");
                dataBase.usp_ClaimOnLinePayAuto_Transaction_Insert(type, Code, payAutoTypeId, createdByUserId, paymentDate, paymentAmount, transRefNo, payTransferResponse, claimOnLineId, payListHeaderId, statusBank);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - ClaimOnLinePayAuto_Transaction_Insert Error claimOnLineId = {claimOnLineId} : {Message}", _controllerName, claimOnLineId, ex.Message);
            }
        }

        private static string SelectMessageTemplate(usp_PersonContactPhoneNoById_Select_Result contact, string surveyLink = "", double? transferAmountTotal = null, int? claimOnLineId = null)
        {
            try
            {
                string tranferAmount = transferAmountTotal != null ? "จำนวน " + transferAmountTotal.ToString() + ".- " : "";

                string template;
                bool checkPayeeAccountNo = contact.PayeeAccountNo.Trim().Length == 10;

                string payeeAccountNo = checkPayeeAccountNo ? contact.PayeeAccountNo.Substring(contact.PayeeAccountNo.Length - 5, 4)
                                : contact.PayeeAccountNo.Substring(contact.PayeeAccountNo.Length - 8, 4);

                surveyLink = surveyLink.Trim();

                if (!string.IsNullOrWhiteSpace(surveyLink))
                {
                    template = Properties.Settings.Default.SMS_Template_AccountOtherWithSurvey;

                    template = template.Trim() + " " + surveyLink;
                    return String.Format(template);
                }
                else
                {
                    template = checkPayeeAccountNo ? Properties.Settings.Default.SMS_Template_AccountEqual10
                        : Properties.Settings.Default.SMS_Template_AccountOther;
                    return String.Format(template, payeeAccountNo, tranferAmount);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - SelectMessageTemplate Error claimOnLineId = {claimOnLineId} : {Message}", _controllerName, claimOnLineId, ex.Message);
                return "";
            }
        }

        private SurveyDto GetLinkSMSMesssageSurvey(usp_TmpClaimOnLineTransfer_Select_Result c, bool flagGenerateSurvey)
        {
            ClaimOnLineDBContext dataBase = new ClaimOnLineDBContext();
            var contact = new usp_PersonContactPhoneNoById_Select_Result
            {
                PersonContactPhoneNo = c.SMSPhoneNo,
                PayeeAccountName = c.ToAccountName,
                PayeeAccountNo = c.ToAccountNo
            };

            // เลือกข้อความ แบบไม่ต้องส่ง Link Survey ไว้ก่อน
            var survey = new SurveyDto();
            survey.msg = SelectMessageTemplate(contact, "", c.Amount, c.ClaimOnLineId);

            try
            {
                if (c.TransferFromMenu == 1 && flagGenerateSurvey && survey.msg != "")
                {
                    //1. Get CustomerGUID for Send to Survey.
                    Log.Debug("{_controllerName} - GetLinkSMSMesssageSurvey - Get CustomerGUID by ClaimOnlineId: {ClaimOnlineId}", _controllerName, c.ClaimOnLineId);
                    var customerDetail = dataBase.usp_GetCustomerGUIDbyClaimOnLineId(c.ClaimOnLineId).FirstOrDefault();

                    var customerGuid = customerDetail != null ? customerDetail.Customer_guid : null;

                    //2. If cannot get customerGUID change to send normal SMS.
                    Log.Debug("{_controllerName} - GetLinkSMSMesssageSurvey - Prepare message sms for ClaimOnlineId: {ClaimOnlineId}", _controllerName, c.ClaimOnLineId);

                    if (customerGuid != null)
                    {
                        Log.Debug("{_controllerName} - GetLinkSMSMesssageSurvey - Found CustomerGUID: {@Customer_Guid} by ClaimOnlineId: {claimonLineId} from: {sp}", _controllerName, customerGuid, c.ClaimOnLineId, "usp_GetCustomerGUIDbyClaimOnLineId");

                        // Get Link Survey from API. 
                        var surveyLink = RequestSurveyLink(c.ClaimOnLineId, c.CreatedByCode, c.SMSPhoneNo, customerGuid);

                        if (surveyLink != null && surveyLink.IsResult == true)
                        {
                            survey.flgSendSMSWithSurvey = true;

                            // Prepare Survey SMS Message.
                            survey.MsgSurvey = SelectMessageTemplate(contact, surveyLink.Msg, claimOnLineId: c.ClaimOnLineId);

                            Log.Debug("{_controllerName} - GetLinkSMSMesssageSurvey - Get Survey Message With Link Success: {msg} ClaimOnlineId: {claimonLineId} CustomerGuid: {@Customer_Guid} Result: {@SurveyLink}", _controllerName, survey.msg, c.ClaimOnLineId, customerGuid, surveyLink);
                        }
                    }
                    else
                    {
                        Log.Error("{_controllerName} - GetLinkSMSMesssageSurvey - Not Found CustomerGUID by ClaimonLineId: {claimonLineId} from: {sp}", _controllerName, c.ClaimOnLineId, "usp_GetCustomerGUIDbyClaimOnLineId");
                    }

                    Log.Debug("{_controllerName} - GetLinkSMSMesssageSurvey - Send Message: {msg} for ClaimOnlineId : {claimonLineId}", _controllerName, survey.msg, c.ClaimOnLineId);
                }

                return survey;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - GetLinkSMSMesssageSurvey Error claimOnLineId = {claimOnLineId} : {Message}", _controllerName, c.ClaimOnLineId, ex.Message);
                return new SurveyDto();
            }

        }

        private GetResponseSurveyDTO RequestSurveyLink(int? claimonLineId, string employeeCode, string phoneNumber, Guid? customerGuid)
        {
            // Send ClaimId into reference.
            var refValue = new PostTemplateSurveyDTO.Reference
            {
                refId = claimonLineId.ToString()
            };

            var refList = new List<PostTemplateSurveyDTO.Reference>() { };
            refList.Add(refValue);

            var objSurvey = new PostTemplateSurveyDTO
            {
                formId = 7,
                sFCaseId = "",
                customerId = customerGuid,
                phoneNo = phoneNumber,
                employeeCode = employeeCode,
                reference = refList,
                isSendNow = false,
                sendBy = employeeCode,
                sendDate = DateTime.Now
            };

            Log.Debug("{_controllerName} - RequestSurveyLink ClaimOnlineId: {claimonLineId} [Request = {@objSurvey}]", _controllerName, claimonLineId, objSurvey);

            try
            {
                return ApiService.RequestApiProvider<GetResponseSurveyDTO>(objSurvey, "sendsurveyclaim", true);
            }
            catch (Exception e)
            {
                // Keep Log but system must go on.
                Log.Error(e, "{_controllerName} - RequestSurveyLink Cannot Get Link From Survey: {@objServeyor} ClaimOnlineId: {claimonLineId}", objSurvey, claimonLineId);
                return null;
            }
        }


    }
}