namespace RoadsideAssistance.Models
{
    public class ImportPA
    {
        public string sourceTypeId { get; set; }
        public string sourceType { get; set; }
        public string referenceCode { get; set; }
        public string title { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string mobileNumber { get; set; }
        public string cardDetailType { get; set; }
        public string cardDetail { get; set; }
        public string passportId { get; set; }
        public string cardId { get; set; }

        public string Product { get; set; }
        public int TmpCode { get; set; }
    }
}