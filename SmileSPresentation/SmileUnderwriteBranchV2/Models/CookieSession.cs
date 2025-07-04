namespace SmileUnderwriteBranchV2.Models
{
    public class CookieSession
    {
        public CookieSession(bool status)
        {
            cookieStatus = status;
        }

        public bool cookieStatus { get; set; }
    }
}