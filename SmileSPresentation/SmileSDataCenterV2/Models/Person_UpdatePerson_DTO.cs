using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSDataCenterV2.Models
{
    public class Person_UpdatePerson_DTO
    {
        public string PersonIdList { get; set; }
        public int? ToPersonId { get; set; }
        public int? TitleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentityCard { get; set; }
        public DateTime? Birthdate { get; set; }
        public string PrimaryPhone { get; set; }
        public string SecondaryPhone { get; set; }
        public string Email { get; set; }
        public string LineID { get; set; }
        public string DocumentCode { get; set; }
        public int? UpdatedByUserId { get; set; }
    }
}