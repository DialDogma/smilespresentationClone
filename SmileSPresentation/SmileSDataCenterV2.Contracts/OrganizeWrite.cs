using System;

namespace SmileSDataCenterV2.Contracts
{
    public class OrganizeWrite
    {
        public bool? IsResult { get; set; }
        public string Msg { get; set; }
        public int? OrganizeId { get; set; }
        public string OrganizeDetail { get; set; }
        public int? OrganizeTypeId { get; set; }
        public string OrganizeTypeDetail { get; set; }
        public int? OrganizeSubTypeId { get; set; }
        public string OrganizeSubTypeDetail { get; set; }
        public int? Address_ID { get; set; }
        public string BuildingName { get; set; }
        public string VillageName { get; set; }
        public string No { get; set; }
        public string Moo { get; set; }
        public string Floor { get; set; }
        public string Room { get; set; }
        public string Soi { get; set; }
        public string Road { get; set; }
        public string SubDistrictId { get; set; }
        public string SubDistrictDetail { get; set; }
        public string ProvinceId { get; set; }
        public string ProvinceDetail { get; set; }
        public string ZipCode { get; set; }
        public string AddressDetail1 { get; set; }
        public string AddressDetail2 { get; set; }
        public int? CreateByUserId { get; set; }
        public string CreateByCode { get; set; }
        public string CreateByName { get; set; }
        public int? UpdatedByUserId { get; set; }
        public string UpdatedByCode { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }
        public string DistrictId { get; set; }
        public string DistrictDetail { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}