using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmileSPoint.Helper;
using SmileSPoint.Models;

namespace SmileSPoint.Controllers
{
    [APIAuthorization]
    [RoutePrefix("api")]
    public class APIController : ApiController
    {
        /// <summary>
        /// เรียกดู Account Detail จาก referenceCode
        /// </summary>
        /// <param name="accountTypeId">10 = ตัวแทน,20 = สาขา</param>
        /// <param name="referenceCode">รหัสตัวแทน , รหัสสาขา</param>
        /// <returns></returns>
        [HttpGet]
        [Route("AccountDetail")]
        public AccountDetailResult GetAccountDetail(int accountTypeId, string referenceCode)
        {
            var db = new SmilePointEntities();

            AccountDetailResult result = db.usp_GetAccountDetail(accountTypeId, referenceCode).FirstOrDefault();

            db.Dispose();

            return result;
        }

        /// <summary>
        /// Get Point Account List
        /// </summary>
        /// <param name="pageStart"> Start from 0</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPointAccountList")]
        public PointAccountResult GetPointAccountList(int pageStart)
        {
            PointAccountResult result = new PointAccountResult();
            var db = new SmilePointEntities();
            var itemsPerPage = 20;
            var items = db.usp_PointAccount_List(pageStart).ToList();

            result.CurrentPage = pageStart;

            if (items.Count > 0)
            {
                result.TotalCount = items.FirstOrDefault().TotalCount.Value;
                result.TotalPages = result.TotalCount / itemsPerPage;
                var mod = result.TotalCount % itemsPerPage;

                if (mod > 0)
                {
                    result.TotalPages += 1;
                }

                foreach (usp_PointAccount_List_Result item in items)
                {
                    result.Items.Add(new PointAccountItem(item));
                }
            }

            db.Dispose();
            return result;
        }

        /// <summary>
        /// Get Point Account ID
        /// </summary>
        /// <param name="pointAccountId">point account ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("AccountTransaction")]
        public List<AccountTransactionResult> GetAccountTransaction(int pointAccountId)
        {
            var db = new SmilePointEntities();

            List<AccountTransactionResult> result = db.usp_GetAccountTransaction(pointAccountId.ToString()).ToList();

            db.Dispose();

            return result;
        }

        /// <summary>
        /// Use Point
        /// </summary>
        /// <param name="pointAccountId"></param>
        /// <param name="merchantOrderId"></param>
        /// <param name="merchantEmpId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UsePoint")]
        public UsePointResult SetUsePoin(int pointAccountId, string merchantOrderId, string merchantEmpId, int amount)
        {
            var db = new SmilePointEntities();
            var transTypeId = 1001;

            UsePointResult result = new UsePointResult();

            //Validate PointAccountId
            var account = db.PointAccounts.Where(x => x.PointAccountId == pointAccountId).FirstOrDefault();

            if (amount < 0)
            {
                result.isSuccess = false;
                result.remark = "Error: Amount not valid";
                return result;
            }

            //Get userId
            var user = Convert.ToInt32(GlobalFunction.GetAPIUserFromJWTKey(this.Request.Headers.GetValues("Authorization").First()));

            if (account != null)
            {
                if (account.PointRemain >= amount)
                {
                    //Validate merchant id not exsist
                    var merchantOrderExsist = db.Transactions.Where(x => x.CreatedById == user && x.MerchantOrderId == merchantOrderId).FirstOrDefault();
                    if (merchantOrderExsist == null)
                    {
                        //create transaction
                        var transDetail = db.usp_CreateTransactionForAPI(pointAccountId, transTypeId, amount, "", user, merchantOrderId, merchantEmpId).FirstOrDefault();
                        if (transDetail != null)
                        {
                            result.isSuccess = true;
                            result.remark = "";
                            result.pointAccountId = pointAccountId.ToString();
                            result.transactionId = transDetail.TransactionId.ToString();
                            result.balanceBefore = Convert.ToInt32(transDetail.AmountBalance + transDetail.AmountDecrease);
                            result.amount = Convert.ToInt32(transDetail.AmountDecrease);
                            result.balanceAfter = Convert.ToInt32(transDetail.AmountBalance);
                            result.createdDate = transDetail.CreatedDate.Value;
                            result.merchantOrderId = merchantOrderId;
                            result.merchantEmpId = merchantEmpId;
                        }
                        else
                        {
                            //Error
                            result.isSuccess = false;
                            result.remark = "Error: Cant create transaction";
                        }
                    }
                    else
                    {
                        //Error Merchant order exsist
                        result.isSuccess = false;
                        result.remark = "Error: OrderId for this merchant already exsist";
                    }
                }
                else
                {
                    //Error not enough point
                    result.isSuccess = false;
                    result.remark = "Error: Not enought point.";
                }
            }
            else
            {
                //Error pointAccount not found
                result.isSuccess = false;
                result.remark = "Error: PointAccount not found";
            }

            db.Dispose();
            return result;
        }

        public struct UsePointResult
        {
            public bool isSuccess { get; set; }
            public string remark { get; set; }
            public string transactionId { get; set; }
            public string pointAccountId { get; set; }
            public int balanceBefore { get; set; }
            public int amount { get; set; }
            public int balanceAfter { get; set; }
            public DateTime createdDate { get; set; }
            public string merchantOrderId { get; set; }
            public string merchantEmpId { get; set; }
        }

        public class PointAccountResult
        {
            public int TotalCount { get; set; }
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }
            public List<PointAccountItem> Items { get; set; }

            public PointAccountResult()
            {
                TotalCount = 0;
                TotalPages = 0;
                Items = new List<PointAccountItem>();
            }
        }

        public class PointAccountItem
        {
            public int PointAccountId { get; set; }
            public string PointAccountName { get; set; }
            public int? PointAccountTypeId { get; set; }
            public string PointAccountTypeName { get; set; }
            public string ReferenceCode { get; set; }
            public string ReferenceDetail { get; set; }
            public Double? PointRemain { get; set; }
            public DateTime? CreatedDate { get; set; }
            public bool IsActive { get; set; }

            public PointAccountItem(usp_PointAccount_List_Result toCopy)
            {
                this.PointAccountId = toCopy.PointAccountId;
                this.PointAccountName = toCopy.PointAccountName;
                this.PointAccountTypeId = toCopy.PointAccountTypeId;
                this.PointAccountTypeName = toCopy.PointAccountTypeName;
                this.ReferenceCode = toCopy.ReferenceCode;
                this.ReferenceDetail = toCopy.ReferenceDetail;
                this.PointRemain = toCopy.PointRemain;
                this.CreatedDate = toCopy.CreatedDate;
                this.IsActive = toCopy.IsActive;
            }
        }
    }
}