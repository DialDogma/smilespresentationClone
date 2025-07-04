using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmileSDataCenterV2.Models;

namespace SmileSDataCenterV2.API
{
    [RoutePrefix("api/Employee")]
    public class EmployeeController : ApiController

    {
        private readonly DataCenterV1Entities _context;
        public EmployeeController() => _context = new DataCenterV1Entities();

        [HttpGet]
        [Route("GetEmployee")]
        public IHttpActionResult GetEmployee(int? branchId = null,string searchValue = null)
        {
            try
            {
                var result = _context.usp_API_Employee_Select(branchId, searchValue).ToList();
                return Json(result);
            }
            catch (Exception e)
            {

                return Json("Select from usp_API_PersonByPersonGUID_Select fail. Message : " + e.Message); ;
            }
            return Json("");
        }
    }
}
