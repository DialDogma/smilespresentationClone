using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using SmileSPACommunity.Models;

namespace SmileSPACommunity.API
{
    [RoutePrefix("api/PAPersonnel")]
    public class PAPersonnelController : ApiController
    {

        private readonly PACommunityEntities _context;

        public PAPersonnelController() => _context = new PACommunityEntities();

        // GET: PersonnelCustomerDetail
        [HttpGet]
        [Route("GetCustomerDetailByCode")]
        public IHttpActionResult GetCustomerDetailByCode(string code)
        {
            try
            {
                var result = _context.usp_GetPersonnelCustomerDetail(code).ToList();
                return Json(result);
            }
            catch (Exception e)
            {
                return Json("Select API GetCustomerDetailByCode error: " + e.Message);
            }
        }

        // GET : GetBenefit
        [HttpGet]
        [Route("GetBenefitByProductId")]
        public IHttpActionResult GetBenefitByProductId(int productId, int benefitTypeId)
        {
            try
            {
                var result = _context.usp_ProductBenefit_Select(productId, benefitTypeId, 0, 99, null, null, null).ToList();
                return Json(result);
            }
            catch (Exception e)
            {
                return Json("Select API GetBenefitByProductId error: " + e.Message);
            }
        }
    }
}