namespace SmileSLogin.Helper
{
    public enum TokenResult
    {
        SUCCESS,
        ERROR
    }

    public class Token
    {
        public TokenResult Result { get; set; }
        public string ErrorText { get; set; }
        public string UserName { get; set; }
        public int User_ID { get; set; }
        public int Person_ID { get; set; }
        public int Employee_ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeePositionDetail { get; set; }
        public string EmployeeTeamDetail { get; set; }
        public string BranchDetail { get; set; }
        public string DepartmentDetail { get; set; }
        public int EmployeeTeam_ID { get; set; }
        public int Department_ID { get; set; }
        public int Branch_ID { get; set; }
        public int EmployeePosition_ID { get; set; }
        public string FullName { get; set; }
        public string OrganizeCode { get; set; }
        public string OrganizeDetail { get; set; }
        public string EmployeeCode { get; set; }
    }
}