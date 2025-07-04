using System;

namespace SmileSDataCenterV2.Contracts
{
    public class CustomerDetailUpdate
    {
        public Guid PersonId { get; set; }

        public int? PersonTypeId { get; set; }

        public int? CardTypeId { get; set; }

        public string CardDetail { get; set; }

        public int? TitleId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? GenderId { get; set; }

        public DateTime? BirthDate { get; set; }

        public int? OccupationLevelId { get; set; }

        public string MobileNo { get; set; }

        public string PhoneNo { get; set; }

        public string Email { get; set; }

        public string ContactAddressDetail1 { get; set; }

        public string ContactAddressDetail2 { get; set; }

        public int? ContactSubDistrictId { get; set; }

        public string HomeAddressDetail1 { get; set; }

        public string HomeAddressDetail2 { get; set; }

        public int? HomeSubDistrictId { get; set; }

        public int? WorkWorkplacId { get; set; }

        public string WorkAddressDetail1 { get; set; }

        public string WorkAddressDetail2 { get; set; }

        public int? WorkSubDistrictId { get; set; }

    }
}
