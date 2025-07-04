using System;
using System.Collections.Generic;
using System.Linq;
using SmileSSurvey.Models;
using SmileSSurvey.Helper;
using System.Text;
using System.Web.Mvc;
using System.Web.Http;
using RouteAttribute = System.Web.Mvc.RouteAttribute;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace SmileSSurvey.API
{
    public class ReportSurveyController : ApiController
    {
        private readonly SurveyEntities _context;

        public ReportSurveyController() => _context = new SurveyEntities();

        [Route("smilecrmreport")]
        public IHttpActionResult GetSmileCRMReport(string dateFrom, string dateTo, int? branchId = null, string serviceTypeName = null,int? createdByEmployeeId = null, int? indexStart = 0, int? pageSize = 10, string sortField = null, string orderType = null, string searchDetail = null)
        {
            // dateFrom= "04-10-2566 "   dateTo= "04-10-2566 "  branchId =null  serviceTypeName=null indexStart=1   pageSize=10  sortField=null   orderType="false" searchDetail=null


            if (serviceTypeName== "null")serviceTypeName = null;
            DateTime? c_dateFrom = null;
            DateTime? c_dateTo = null;
            if (dateFrom != null && dateFrom != "")
            {
                c_dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dateFrom);
            }
            if (dateTo != null && dateTo != "")
            {
                c_dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dateTo);
            }
            if (orderType == "false")
            {
                orderType = "desc";
            }
            else if (orderType == "true") { orderType = "asc"; }
            else if (orderType == "undefined") { orderType = "desc"; } 
            if(sortField == "undefined") { sortField = "createdDate"; }
            int c_indexStart = (int)indexStart;
            if (indexStart > 0 ) { 
                c_indexStart = ((int)indexStart -1) * (int)pageSize;
            };
             

            var result = _context.usp_SmileCRMSurveyResult_Select(c_dateFrom, c_dateTo, branchId, serviceTypeName, createdByEmployeeId, c_indexStart, pageSize, sortField, orderType, searchDetail).ToList();


            decimal totalAmountRecords = 0;
            decimal totalAmountPages = 0;
            int pageIndex = 0;

            if (result.FirstOrDefault()?.TotalCount != null)
            {
                totalAmountRecords = (decimal)result.FirstOrDefault()?.TotalCount;
            }
            totalAmountPages = Math.Ceiling(totalAmountRecords / (decimal)pageSize);

            if (indexStart != null)
            {
                pageIndex = (int)indexStart;
            }

            var resultDto = new Dictionary<string, object>
            {
            { "TotalAmountRecords" , totalAmountRecords },
            { "TotalAmountPages" , totalAmountPages },
            { "CurrentPage" , indexStart },
            { "RecordsPerPage" , pageSize },
            { "PageIndex" , pageIndex },
            {"data", result.ToList()}
            };

            return Json(resultDto, GlobalObject.carmelSetting());
        }
    }
}