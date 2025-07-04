using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSDataCenterV2.DTOs
{
    public class PersonUpsertDTO
    {
        public int? PersonTypeId { get; set; }
        public int? CardTypeId { get; set; }
        public string CardDetail { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? TitleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? OccupationLevelId { get; set; }
        public string PrimaryPhone { get; set; }
        public string SecondaryPhone { get; set; }

        public int? H_WorkPlaceId { get; set; }
        public string H_BuildingName { get; set; }
        public string H_No { get; set; }
        public string H_AddressDetail1 { get; set; }
        public string H_AddressDetail2 { get; set; }
        public string H_SubDistrictId { get; set; }

        public int? C_WorkPlaceId { get; set; }
        public string C_BuildingName { get; set; }
        public string C_No { get; set; }
        public string C_AddressDetail1 { get; set; }
        public string C_AddressDetail2 { get; set; }
        public string C_SubDistrictId { get; set; }

        public int? W_WorkPlaceId { get; set; }
        public string W_BuildingName { get; set; }
        public string W_No { get; set; }
        public string W_AddressDetail1 { get; set; }
        public string W_AddressDetail2 { get; set; }
        public string W_SubDistrictId { get; set; }

        public int? CreatedByUserId { get; set; }
        public string PolicyCode { get; set; }
        public int? CustomerTypeId { get; set; }
        public string Condition1 { get; set; }
    }
}