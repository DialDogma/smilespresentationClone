using System.Web.Http;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using SmileSDataCenterV2.Models;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;
using System.Linq;
using System;
using SmileSDataCenterV2.DTOs;
using System.Collections.Generic;

namespace SmileSDataCenterV2.API
{
    [RoutePrefix("api/Person")]
    public class PersonController : ApiController
    {
        private readonly DataCenterV1Entities _context;
        private readonly DataCenterV1Entities1 _context2;

        public PersonController()
        {
            _context = new DataCenterV1Entities();
            _context2 = new DataCenterV1Entities1();
        }

        #region New

        [HttpPost]
        [Route("UpsertPerson")]
        public IHttpActionResult UpsertPerson([FromBody] PersonUpsertDTO request)
        {
            try
            {
                //throw new NotImplementedException();
                var result = _context2.usp_Person_Upsert(
                       request.PersonTypeId,
                       request.CardTypeId,
                       request.CardDetail,
                       request.BirthDate,
                       request.TitleId,
                       request.FirstName,
                       request.LastName,
                       request.OccupationLevelId,
                       request.PrimaryPhone,
                       request.SecondaryPhone,
                       request.H_WorkPlaceId,
                       request.H_BuildingName,
                       request.H_No,
                       request.H_AddressDetail1,
                       request.H_AddressDetail2,
                       request.H_SubDistrictId,
                       request.C_WorkPlaceId,
                       request.C_BuildingName,
                       request.C_No,
                       request.C_AddressDetail1,
                       request.C_AddressDetail2,
                       request.C_SubDistrictId,
                       request.W_WorkPlaceId,
                       request.W_BuildingName,
                       request.W_No,
                       request.W_AddressDetail1,
                       request.W_AddressDetail2,
                       request.W_SubDistrictId,
                       request.CreatedByUserId,
                       request.PolicyCode,
                       request.CustomerTypeId,
                       request.Condition1
                   ).FirstOrDefault();

                return Json(result);
            }
            catch (Exception e)
            {
                return Json("Upsert usp_Person_Upsert fail. Message : " + e.Message);
            }
        }

        #endregion New

        [HttpPost]
        [Route("UpdatePerson")]
        public IHttpActionResult UpdatePerson([FromBody] Person_UpdatePerson_DTO person)
        {
            var rsFail = new usp_PersonDetailForSMSProfile_Update_Result()
            {
                IsResult = false,
                Result = "Fail",
                Msg = ""
            };

            if (person.TitleId == 0 || person.TitleId == 1)
            {
                rsFail.Msg = "TitleId can not be 0 or 1";
                return Json(rsFail);
            }
            else if (person.UpdatedByUserId == null)
            {
                rsFail.Msg = "Field UpdatedByUserId can not be null";
                return Json(rsFail);
            }
            else
            {
                try
                {
                    var result = _context.usp_PersonDetailForSMSProfile_Update(person.PersonIdList, person.ToPersonId,
                                 person.TitleId, person.FirstName, person.LastName, person.IdentityCard, person.Birthdate,
                                 person.PrimaryPhone, person.SecondaryPhone, person.Email, person.LineID, person.DocumentCode,
                                 person.UpdatedByUserId).FirstOrDefault();

                    return Json(result);
                }
                catch (Exception e)
                {
                    rsFail.Msg = e.Message;
                    return Json(rsFail);
                }
            }
        }

        [HttpGet]
        [Route("GetPersonByCardDetail")]
        public IHttpActionResult GetPersonByCardDetail(int? CardTypeId, string CardDetail)
        {
            try
            {
                var result = _context.usp_API_PersonByCardDetail_Select(CardTypeId, CardDetail).ToList();

                return Json(result);
            }
            catch (Exception e)
            {
                return Json("Select from usp_API_PersonByCardDetail_Select fail. Message : " + e.Message);
            }
        }

        [HttpGet]
        [Route("GetPersonByPersonGUID")]
        public IHttpActionResult GetPersonByPersonGUID(Guid Person_guid)
        {
            try
            {
                var result = _context.usp_API_PersonByPersonGUID_Select(Person_guid).FirstOrDefault();

                return Json(result);
            }
            catch (Exception e)
            {
                return Json("Select from usp_API_PersonByPersonGUID_Select fail. Message : " + e.Message);
            }
        }

        [HttpGet]
        [Route("GetPersonBankAccount")]
        public IHttpActionResult GetPersonBankAccount(Guid Person_guid, int? BankId = null, string AccountNo = null, bool? IsBankDirectDebit = null)
        {
            try
            {
                var result = _context.usp_API_PersonBankAccount_Select(Person_guid, BankId, AccountNo, IsBankDirectDebit).ToList();
                return Json(result);
            }
            catch (Exception e)
            {
                return Json("Select from usp_API_PersonBankAccount_Select fail. Message : " + e.Message);
            }
        }

        [HttpGet]
        [Route("GetProductPHByPersonGUID")]
        public IHttpActionResult GetProductPHByPersonGUID(Guid Person_guid)
        {
            try
            {
                var result = _context.usp_API_ProductPHByPersonGUID_Select(Person_guid).ToList();
                return Json(result);
            }
            catch (Exception e)
            {
                return Json("Select from usp_API_ProductPHByPersonGUID_Select fail. Message : " + e.Message);
            }
        }

        [HttpGet]
        [Route("GetIDCardByUserCode")]
        public IHttpActionResult GetIDCardByUserCode(int userType, string userCode)
        {
            try
            {
                var value = _context.usp_API_GetIDCardByPersonId_Select(userType, userCode).FirstOrDefault();
                var result = new IDCardReturnDTO
                {
                    IDCard = value
                };
                return Json(result);
            }
            catch (Exception e)
            {
                return Json("Select from usp_API_GetIDCardByPersonId_Select fail. Message : " + e.Message);
            }
        }

        [HttpGet]
        [Route("GetPersonDetailByPersonId")]
        public IHttpActionResult GetPersonDetailByPersonId(int personId)
        {
            try
            {
                var result = _context.usp_API_PersonDetailByPersonId_Select(personId).FirstOrDefault();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json("Select from usp_API_PersonDetailByPersonId_Select fail. Message : " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPersonRelationType")]
        public IHttpActionResult GetPersonRelationType()
        {
            try
            {
                var result = _context.usp_API_PersonRelationType_Select().ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json("Select from usp_API_PersonRelationType_Select fail. Message : " + ex.Message);
            }
        }

        [HttpGet]
        [Route("getprospectcustomer")]
        public IHttpActionResult GetProspectCustomer(int? draw = null, int? searchIndex = null, string search = null, int? indexStart = null, int? pageSize = null, string sortField = null, string orderType = null)
        {
            try
            {
                var result = _context.usp_ProspectCustomer_Select(searchIndex, search, indexStart, pageSize, sortField, orderType).ToArray();

                var dt = new Dictionary<string, object>
                {
                    {"draw", draw },
                    {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : 0},
                    {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : 0},
                    {"data", result.ToList()}
                };

                return Json(dt);
            }
            catch (Exception ex)
            {
                return Json("Select from usp_ProspectCustomer_Select fail. Message : " + ex.Message);
            }
        }

        [HttpGet]
        [Route("getprospectcustomer/{personId}/details")]
        public IHttpActionResult GetProspectCustomerDetail(string personId)
        {
            try
            {
                var result = _context.usp_ProspectCustomer_Select(99, personId, null, null, null, null).FirstOrDefault();

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json("Select from usp_ProspectCustomer_Select fail. Message : " + ex.Message);
            }
        }

        [HttpGet]
        [Route("getprospectcustomer/{id}/product")]
        public IHttpActionResult GetProspectCustomerProduct(Guid id)
        {
            try
            {
                var result = _context.usp_ProspectCustomerProduct_Select(id).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json("Select from usp_ProspectCustomerProduct_Select fail. Message : " + ex.Message);
            }
        }
    }
}