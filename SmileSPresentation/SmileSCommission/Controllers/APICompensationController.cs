using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmileSCommission.Helper;
using SmileSCommission.Models;

namespace SmileSCommission.Controllers
{
    [Helper.Authorization]
    [RoutePrefix("APICompensation")]
    public class APICompensationController:ApiController
    {
        #region Context

        private readonly CommissionCalculateEntities _context;

        public APICompensationController()

        {
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        [HttpPost]
        [Route("GetPeriodDCR_Datatable")]
        public IHttpActionResult GetPeriodDCR_Datatable(DateTime payPeriod,bool isWaitProcess,bool isSentPayroll,
            bool isSucess,bool isCancel,int? draw = null,int? indexStart = null,int? pageSize = null,
            string sortField = null,string orderType = null,string search = null)
        {
            var result = _context.usp_CommissionPeriodSearch_Select(payPeriod,isWaitProcess,isSentPayroll,isSucess,
                isCancel,indexStart,pageSize,sortField,orderType,search).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt,GlobalFunction.carmelSetting());
        }
    }
}