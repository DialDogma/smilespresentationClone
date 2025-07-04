using Microsoft.AspNetCore.Mvc;
using SmileSClaimOnLine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serilog;
using RestSharp;
using static SmileSClaimOnLine.Models.SmsModel;
using Newtonsoft.Json;

namespace SmileSClaimOnLine.Controllers
{
    [ApiController]
    [System.Web.Mvc.Route("[controller]")]
    public class BankingController : Controller
    {
        private const string _controllerName = nameof(BankingController);
        private ClaimOnLineDBContext _context;

        public BankingController()
        {
            _context = new ClaimOnLineDBContext();
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult UpdateAccountBalance(string AccountNo, string BankId, string BankName, decimal Balance, string CreatedByUserId)
        {
            Log.Debug($"{_controllerName} Start Update Account Balance at:", DateTime.Now);
            var minBalance = decimal.Parse(Properties.Settings.Default.MinBalance);
            if (Balance < minBalance)
            {
                Log.Debug("Send Message Balance notification. BankId: {BankId} Balance : {Balance}", BankId, Balance);
                try
                {
                    var url = Properties.Settings.Default.SendSMSList;
                    var Message = Properties.Settings.Default.SMS_Template_BalanceNotification;
                    Message = String.Format(Message, minBalance.ToString("#,##0"));
                    var PhoneNumbers = Properties.Settings.Default.PhonNoSMSMinBalance;
                    string[] PhoneNumberList = PhoneNumbers.Split(',');

                    List<SendListDetail> Data = new List<SendListDetail>();

                    if (PhoneNumberList.Length > 0)
                    {
                        foreach (string PhoneNo in PhoneNumberList)
                        {
                            Data.Add(new SendListDetail() { Ref= PhoneNo, PhoneNo= PhoneNo, Message= Message });
                        }
                    }

                    var client = new RestClient(url);

                    //45 แจ้งเตือนยอดเงินต่ำกว่าที่กำหนด
                    var param = new SMSSendListRequest { ProjectId = 22, SMSTypeId = 45, Remark = "ClaimOnlineแจ้งเตือนยอดเงินในบัญชีต่ำ", Total = Data.Count, SendDate = DateTime.Now, Data = Data };
                    //Add Json Body to Request
                    var request = new RestRequest().AddJsonBody(param);
                    //Add Header Token
                    request.AddHeader("Authorization", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwcm9qZWN0aWQiOjl9.XX7R_Ik8pydv2e04ZvrDew8tlszSDrjvTEYKdgO4t7A");

                    //Log
                    Log.Debug("Start to Send SMS Balance notification : at: {DATETIME}", DateTime.Now);
                    //Post Request
                    var response = client.Post(request);

                    //Deserialize JSON
                    SMS_Result sms = JsonConvert.DeserializeObject<SMS_Result>(response.Content);

                    Log.Information("Send Message Balance notification Successful. at: {DATETIME}", DateTime.Now);
                    //return Json(rs, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    //return exception insert claimonline transfer failed
                    Log.Error(ex, "Send Message Balance notification Error.: {ex}  at: {DATETIME}", ex, DateTime.Now);
                    //return Json(new { IsResult = false, Msg = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
       
            Log.Debug($"{_controllerName} - Update Account Balance Call Store procedure: usp_BankAccountBalance_Insert [AccountNo = {AccountNo}, BankId = {BankId},BankName = {BankName},Balance = {Balance},CreatedByUserId = {CreatedByUserId}]");
            string[] rs = new string[3];

            var result = _context.usp_BankAccountBalance_Insert(AccountNo, BankId, BankName, Balance, CreatedByUserId).SingleOrDefault();
            rs[0] = result.IsResult.Value.ToString();
            rs[1] = result.Result;
            rs[2] = result.Msg;
            if (result.IsResult == true)
            {
                Log.Information($"Update Account Balance Successful AccountNo : {AccountNo.ToString()} Balance : {Balance}");
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Log.Error($"Update Account Balance FAIL!!! {rs[2]} !!! AccountNo : {AccountNo.ToString()} Balance : {Balance}");
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }
    }
}