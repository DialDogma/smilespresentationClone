namespace SmileSCommunicate.DTOs
{
    public class SMSSendV2RequestDto
    {
        public string PhoneNo { get; set; }
        public string Message { get; set; }
        public int SMSTypeId { get; set; }
        public int CreatedById { get; set; }
    }
}