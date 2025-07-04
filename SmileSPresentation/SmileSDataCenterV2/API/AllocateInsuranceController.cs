using SmileSDataCenterV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using Microsoft.AspNetCore.Mvc;
using MassTransit;

namespace SmileSDataCenterV2.API
{
    [RoutePrefix("api/AllocateInsurance")]
    public class AllocateInsuranceController : ApiController
    {
        private readonly DataCenterV1Entities _context;
        private readonly DataCenterV1Entities1 _context2;
        private readonly IBusControl _bus;

        public AllocateInsuranceController()
        {
            _context = new DataCenterV1Entities();
            _context2 = new DataCenterV1Entities1();
            // Assuming the _bus is already configured and running
            _bus = MvcApplication.Bus as IBusControl;
        }




        [HttpGet]
        [Route("GetAllocateInsurance")]
        public IHttpActionResult AllocateInsurance(int periodTypeId, int productPackageId ,Guid? personGuid =null,  int? cardTypeId = null, string cardDetail = null, string firstName = null, string lastName = null)
        {

            var rs = _context2.usp_API_AllocateInsuranceCompany_Select(personGuid, productPackageId, periodTypeId, cardTypeId, cardDetail, firstName,lastName);

            return Json(rs);
        }
    }
}