namespace SmileSCommunicate.DTOs
{
    public class SendSMSWithRefRequestDto
    {
        public string Reference { get; set; }
        public string PhoneNo { get; set; }
        public string Message { get; set; }
        public int SMSTypeId { get; set; }
        public int CreatedById { get; set; }
    }
}