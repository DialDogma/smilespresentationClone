using SmileSPaymentGateway.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SmileSPaymentGateway.Helper
{
    public class GlobalFunction
    {
        public static LoginDetail GetLoginDetail(HttpContextBase actionContext)
        {
            var auth = Authorization.GetLoginDetail(actionContext);

            var result = GetUserByUserName(auth.UserName);

            return result;
        }

        private static LoginDetail GetUserByUserName(string username)
        {
            using (SqlConnection conn = new SqlConnection(new SSSCashReceiveEntities().Database.Connection.ConnectionString))
            {
                conn.Open();
                // 1.  create a command object identifying the stored procedure
                SqlCommand cmd = new SqlCommand("[DataCenterV1].[Security].[usp_GetUserDetailByUserName]", conn);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // 3. add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new SqlParameter("UserName", username));

                var dt = new DataTable("UserDetail");
                // execute the command to datatable
                dt.Load(cmd.ExecuteReader());

                var login = new LoginDetail()
                {
                    Result = LoginResult.SUCCESS,
                    UserName = dt.Rows[0]["UserName"] != DBNull.Value ? dt.Rows[0]["UserName"].ToString() : "",
                    User_ID = dt.Rows[0]["User_ID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["User_ID"]) : 0,
                    Person_ID = dt.Rows[0]["Person_ID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Person_ID"]) : 0,
                    Employee_ID = dt.Rows[0]["Employee_ID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Employee_ID"]) : 0,
                    FirstName = dt.Rows[0]["FirstName"] != DBNull.Value ? dt.Rows[0]["FirstName"].ToString() : "",
                    LastName = dt.Rows[0]["LastName"] != DBNull.Value ? dt.Rows[0]["LastName"].ToString() : "",
                    EmployeePositionDetail = dt.Rows[0]["EmployeePositionDetail"] != DBNull.Value ? dt.Rows[0]["EmployeePositionDetail"].ToString() : "",
                    EmployeeTeamDetail = dt.Rows[0]["EmployeeTeamDetail"] != DBNull.Value ? dt.Rows[0]["EmployeeTeamDetail"].ToString() : "",
                    BranchDetail = dt.Rows[0]["BranchDetail"] != DBNull.Value ? dt.Rows[0]["BranchDetail"].ToString() : "",
                    DepartmentDetail = dt.Rows[0]["DepartmentDetail"] != DBNull.Value ? dt.Rows[0]["DepartmentDetail"].ToString() : "",
                    EmployeeTeam_ID = dt.Rows[0]["EmployeeTeam_ID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["EmployeeTeam_ID"]) : 0,
                    Department_ID = dt.Rows[0]["Department_ID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Department_ID"]) : 0,
                    Branch_ID = dt.Rows[0]["Branch_ID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Branch_ID"]) : 0,
                    EmployeePosition_ID = dt.Rows[0]["EmployeePosition_ID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["EmployeePosition_ID"]) : 0,
                    FullName = $"{dt.Rows[0]["FirstName"]} {dt.Rows[0]["LastName"]}",
                    Organize_ID = dt.Rows[0]["Organize_ID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Organize_ID"]) : 0,
                    OrganizeCode = dt.Rows[0]["OrganizeCode"] != DBNull.Value ? dt.Rows[0]["OrganizeCode"].ToString() : "",
                    OrganizeDetail = dt.Rows[0]["OrganizeDetail"] != DBNull.Value ? dt.Rows[0]["OrganizeDetail"].ToString() : "",
                    EmployeeCode = dt.Rows[0]["EmployeeCode"] != DBNull.Value ? dt.Rows[0]["EmployeeCode"].ToString() : "",
                };

                return login;
            }
        }
    }
}