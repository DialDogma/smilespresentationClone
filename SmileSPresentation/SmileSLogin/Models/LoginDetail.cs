using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSLogin.Models
{
    public class LoginDetail : usp_GetUserDetailByUserName_Result
    {
        public string Roles { get; set; }

        public LoginDetail()
        {
        }

        public LoginDetail(usp_GetUserDetailByUserName_Result detail)
        {
            var context = new DataCenterV1Entities();

            BranchDetail = detail.BranchDetail;
            DepartmentDetail = detail.DepartmentDetail;
            EmployeePositionDetail = detail.EmployeePositionDetail;
            EmployeeTeamDetail = detail.EmployeeTeamDetail;
            Employee_ID = detail.Employee_ID;
            FirstName = detail.FirstName;
            Id = detail.Id;
            LastName = detail.LastName;
            Person_ID = detail.Person_ID;
            UserName = detail.UserName;
            User_ID = detail.User_ID;
            EmployeeTeam_ID = detail.EmployeeTeam_ID;
            Branch_ID = detail.Branch_ID;
            Department_ID = detail.Department_ID;
            EmployeePosition_ID = detail.EmployeePosition_ID;
            Organize_ID = detail.Organize_ID;
            OrganizeCode = detail.OrganizeCode;
            OrganizeDetail = detail.OrganizeDetail;
            EmployeeCode = detail.EmployeeCode;
            EmployeeStatus_ID = detail.EmployeeStatus_ID;
            Roles = string.Join(",", context.usp_GetUserRoles(detail.UserName).Select(x => x.Name).ToList());
        }
    }

    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}