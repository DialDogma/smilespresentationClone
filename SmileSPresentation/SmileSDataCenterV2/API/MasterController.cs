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
using static SmileSDataCenterV2.DTOs.Contracts;
using System.Threading.Tasks;

namespace SmileSDataCenterV2.API
{

    [RoutePrefix("api/Master")]
    public class MasterController : ApiController
    {

        private readonly DataCenterV1Entities _context;
        private readonly DataCenterV1Entities1 _context2;
        private readonly IBusControl _bus;

        public MasterController() {
            _context = new DataCenterV1Entities();
            _context2 = new DataCenterV1Entities1();
            // Assuming the _bus is already configured and running
            _bus = MvcApplication.Bus as IBusControl;
        }

        [HttpGet]
        [Route("TestContract")]
        public async Task<IHttpActionResult> TestContract()
        {
            try
            {
                // Arrange (เตรียมข้อมูล)
                int? personTypeId = 1;
                int? cardTypeId = 2;
                string cardDetail = "1234567890";
                DateTime? birthDate = new DateTime(1990, 1, 1);
                int? titleId = 1;
                string firstName = "John";
                string lastName = "Doe";
                int? occupationLevelId = 3;
                string primaryPhone = "0812345678";
                string secondaryPhone = "021234567";
                int? h_WorkPlaceId = 1001;
                string h_BuildingName = "Home Building";
                string h_No = "123/1";
                string h_AddressDetail1 = "Main St.";
                string h_AddressDetail2 = "Floor 5";
                string h_SubDistrictId = "11000";
                int? c_WorkPlaceId = 2002;
                string c_BuildingName = "Condo A";
                string c_No = "99/9";
                string c_AddressDetail1 = "Sukhumvit";
                string c_AddressDetail2 = "Near BTS";
                string c_SubDistrictId = "12000";
                int? w_WorkPlaceId = 3003;
                string w_BuildingName = "Office B";
                string w_No = "456";
                string w_AddressDetail1 = "Silom Rd.";
                string w_AddressDetail2 = "Tower 2";
                string w_SubDistrictId = "13000";
                int? createdByUserId = 999;
                string policyCode = "POL123456";
                int? customerTypeId = 1;
                string condition1 = "VIP";

                // Act (เรียกใช้งานจริง)
                var result =     _context2.usp_Person_Upsert(personTypeId, cardTypeId, cardDetail, birthDate, titleId, firstName, lastName, occupationLevelId, primaryPhone, secondaryPhone,
                                               h_WorkPlaceId, h_BuildingName, h_No, h_AddressDetail1, h_AddressDetail2, h_SubDistrictId,
                                               c_WorkPlaceId, c_BuildingName, c_No, c_AddressDetail1, c_AddressDetail2, c_SubDistrictId,
                                               w_WorkPlaceId, w_BuildingName, w_No, w_AddressDetail1, w_AddressDetail2, w_SubDistrictId,
                                               createdByUserId, policyCode, customerTypeId, condition1);

                // Create your message (OrderCreated in this case)
                var obj = new OrderCreated
                {
                    CustomerId = Guid.NewGuid(),
                    OrderId = "123456789"
                };

                // Publish the message through the bus
                await _bus.Publish(obj);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetBranch")]
        public IHttpActionResult GetBranch(int? branchId = null,int? areaId = null, int? indexStart = null,int? pageSize = null, string sortField = null,string orderType = null,string searchDetail = null)
        {

            var rs = _context.usp_API_Branch_Select(branchId, areaId, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

            return Json(rs);
        }

        [HttpGet]
        [Route("GetBranchAddress")]
        public IHttpActionResult GetBranchAddress(int? branchId = null, int? indexStart = null, int? pageSize = null, string sortField = null, string orderType = null, string searchDetail = null)
        {

            var rs = _context.usp_API_BranchAddress_Select(branchId, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

            return Json(rs);
        }

        [HttpGet]
        [Route("GetZebra")]
        public IHttpActionResult GetZebra(string searchValues = null)
        {
            var rs = _context.usp_API_Zebra_Select(searchValues).ToList();

            return Json(rs);
        }

        [HttpGet]
        [Route("GetOrganize")]
        public IHttpActionResult GetOrganize(int organizeTypeId, string searchValues = null)
        {
            var rs = _context.usp_API_Organize_Select(organizeTypeId, searchValues).ToList();
            return Json(rs);
        }

        [HttpGet]
        [Route("GetWorkplaceSearch")]
        public IHttpActionResult GetWorkplaceSearch(int subDisTrictId, string organizeDetail = null ,int ? indexStart = null, int? pageSize = null, string sortField = null, string orderType = null, string searchDetail = null)
        {
            var rs = _context.usp_API_OrganizeWorkplaceSearch_Select (subDisTrictId, organizeDetail, indexStart, pageSize, sortField, orderType, searchDetail).ToList();
            return Json(rs);
        }

        [HttpGet]
        [Route("GetOccupation")]
        public IHttpActionResult GetOccupation()
        {
            var rs = _context.usp_API_Occupation_Select();
            return Json(rs);
        }

        [HttpGet]
        [Route("GetOccupationLevel")]
        public IHttpActionResult GetOccupationLevel(int? occupationId = null)
        {
            var rs = _context.usp_API_OccupationLevel_Select(occupationId);
            return Json(rs);
        }

        [HttpGet]
        [Route("GetJobDescriptionType")]
        public IHttpActionResult GetJobDescriptionType()
        {
            var rs = _context.usp_API_JobDescriptionType_Select();
            return Json(rs);
        }

        [HttpGet]
        [Route("GetPayMethodType")]
        public IHttpActionResult GetPayMethodType(int? paymethodTypeId = null)
        {
            var rs = _context.usp_API_PayMethodType_Select(paymethodTypeId);
            return Json(rs);
        }

        [HttpGet]
        [Route("GetPeriodType")]
        public IHttpActionResult GetPeriodType(int? periodTypeId = null)
        {
            var rs = _context.usp_API_PeriodType_Select(periodTypeId);
            return Json(rs);
        }

    }
}