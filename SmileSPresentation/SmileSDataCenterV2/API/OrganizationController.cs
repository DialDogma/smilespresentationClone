using AutoMapper;
using Newtonsoft.Json;
using SmileSDataCenterV2.DTOs;
using SmileSDataCenterV2.Helper;
using SmileSDataCenterV2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Authorization = SmileSDataCenterV2.Helper.Authorization;

namespace SmileSDataCenterV2.API
{
    [RoutePrefix("api")]
    public class OrganizationController : ApiController
    {
        private readonly DataCenterV1Entities _context;

        public OrganizationController()
        {
            _context = new DataCenterV1Entities();
        }

        //[Authorize]
        [HttpPost]
        [Route("organization")]
        [ApiExplorerSettings(IgnoreApi = true)] //ignoreApi
        public async Task<IHttpActionResult> Organization([FromBody] OrganizeAddDto org)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(org);
            var validation = new List<ValidationResult>();
            if (!Validator.TryValidateObject(org, validationContext, validation))
                return Content(HttpStatusCode.BadRequest, new { IsResult = false, Msg = validation });

            var organize = _context.usp_OrganizeFromApiBM_Insert(organizeTypeId: org.organizeTypeId,
                                                    organizeDetail: org.organizeDetail,
                                                    buildingName: org.buildingName,
                                                    villageName: org.villageName,
                                                    no: org.no,
                                                    moo: org.moo,
                                                    floor: org.floor,
                                                    soi: org.soi,
                                                    road: org.road,
                                                    subDistrictCode: org.subDistrictCode,
                                                    zipCode: org.zipCode,
                                                    createByUserCode: org.createByUserCode,
                                                    organizeSubTypeId: org.organizeSubTypeId,
                                                    taxNumber: org.taxNumber).FirstOrDefault();

            if (organize.IsResult.Value)
            {
                var configuration = new MapperConfiguration(cfg =>
                {
                    //cfg.CreateMap<usp_OrganizeFromApiBM_Insert_Result, OrganizeWrite>();
                });

                //mapping contract
                //var message = configuration.CreateMapper().Map<OrganizeWrite>(organize);

                //publisher msg
                //await MvcApplication.Bus.Publish(message);
            }

            //response  id to bm
            return Json(new { IsResult = organize.IsResult, Result = organize.OrganizeId ?? null, Msg = organize.Msg });
        }
    }
}