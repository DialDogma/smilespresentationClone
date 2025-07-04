using RoadsideAssistance.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoadsideAssistance.Models
{
    public class MenuRoles
    {
        private static List<Menu> GetAllMenu()
        {
            var result = new List<Menu>
            {
                //insert all menu here
                new Menu(1, 0, 0, "เช็คสิทธิ์ Roadside Assist", "RoadsideAssistance", "Index", "fa fa-check-square-o", "", "Developer,RoadsideAssistance_Callcenter,RoadsideAssistance_Branch"),
                new Menu(2, 0, 0, "ติดตามการดำเนินการ", "RoadsideAssistance", "FollowRoadsideAssis", "fa fa-car", "", "Developer,RoadsideAssistance_Callcenter,RoadsideAssistance_CallcenterAdmin"),
                new Menu(3, 0, 0, "นำเข้าเพิ่มรายชื่อลูกค้า", "RoadsideAssistance", "ImportRoadsideAssis", "fa fa-user-plus", "", "Developer,RoadsideAssistance_Callcenter,RoadsideAssistance_Branch"),
                new Menu(4, 0, 0, "นำเข้ารายชื่อยกเลิกสิทธิ์", "RoadsideAssistance", "ImporrtCxlRoadsideAssis", "fa fa-user-times", "", "Developer,RoadsideAssistance_Callcenter"),
                //new Menu(5, 0, 0, "รายงาน","","", "fa fa-pie-chart", "", "Developer,RoadsideAssistance_Callcenter,RoadsideAssistance_Branch,RoadsideAssistance_CallcenterAdmin"),
                new Menu(5, 0, 0, "รายงานการใช้สิทธิ์ Roadside","RoadsideAssistance","Report", "fa fa-file-text-o", "", "Developer,RoadsideAssistance_Callcenter"),
                new Menu(6, 0, 0, "รายงานการนำเข้าโปรบุคลากร PA","RoadsideAssistance","ReportPAImport", "fa fa-file-text-o", "", "Developer,RoadsideAssistance_CallcenterAdmin,RoadsideAssistance_Callcenter"),
                new Menu(7, 0, 0, "รายงานรายชื่อโปรบุคลากร PA","RoadsideAssistance","ReportPABranchImport", "fa fa-file-text-o", "", "Developer,RoadsideAssistance_Branch"),
                new Menu(8, 0, 0, "รายงานนำเข้า Motor","RoadsideAssistance","ReportMotorPayerImport", "fa fa-file-text-o", "", "Developer,RoadsideAssistance_CallcenterAdmin,RoadsideAssistance_Callcenter"),
                new Menu(9, 0, 0, "รายงานยกเลิกสิทธิ์ Motor","RoadsideAssistance","ReportMotorCancelPayerImport", "fa fa-file-text-o", "", "Developer,RoadsideAssistance_CallcenterAdmin,RoadsideAssistance_Callcenter"),
                new Menu(10, 0, 0, "ล็อคข้อมูลหักแต้มพนักงาน","RoadsideAssistance","EmployeePointLocked", "fa fa-lock", "", "Developer,RoadsideAssistance_CallcenterAdmin"),
            };

            return result;
        }

        /// <summary>
        /// Get Menu By username
        /// </summary>
        /// <param name="userName">username</param>
        /// <returns></returns>
        public static List<Menu> GetMenuByUserName(string userName, int branchId)
        {
            var roles = OAuth2Helper.GetRoles();

            var result = new List<Menu>();

            var lstRoles = roles.Split(',').ToList();

            foreach (var item in GetAllMenu())
            {
                var intersecCount = lstRoles.Intersect(item.AllowRoles).Count();
                if (intersecCount != 0 || item.AllowRoles.Contains("*"))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Get menu by role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static List<Menu> GetMenuByRole(string role, int branchId)
        {
            var result = new List<Menu>();

            foreach (var item in GetAllMenu())
            {
                if (item.AllowRoles.Contains(role) || item.AllowRoles.Contains("*"))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// get role names
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllRoles(int branchId)
        {
            var roles = new List<String>();
            foreach (var mnu in GetAllMenu())
            {
                foreach (var role in mnu.AllowRoles)
                {
                    if (!roles.Contains(role))
                    {
                        roles.Add(role);
                    }
                }
            }

            return roles;
        }
    }

    public class Menu
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="menuId">Menu Id ห้ามซ้ำ</param>
        /// <param name="MainMenuId">ใส่ 0 ถ้าเป็น Main Menu Level</param>
        /// <param name="sortOrder">ลำดับที่เรียง</param>
        /// <param name="detail">detail menu</param>
        /// <param name="controller">controller name</param>
        /// <param name="action">action</param>
        /// <param name="iconClassMenu">font awesome เช่น fa-pie-chart  https://adminlte.io/themes/AdminLTE/pages/UI/icons.html</param>
        /// <param name="queryString">ถ้ามี เช่น ?action=readonly</param>
        /// <param name="allowRoleNames">ชื่อ role ที่อนุญาต เช่น "Admin,User,Manager" ใส่ * เพื่ออนุญาตทั้งหมด</param>
        public Menu(int menuId, int mainMenuId, int sortOrder, string detail, string controller, string action, string iconClassMenu, string queryString, string allowRoleNames)
        {
            MenuId = menuId;
            MainMenuId = mainMenuId;
            SortOrder = sortOrder;
            Detail = detail;
            Controller = controller;
            Action = action;
            IconClassMenu = iconClassMenu;
            QueryString = queryString;
            AllowRoles = allowRoleNames.Split(',').ToList();
        }

        public int MenuId { get; set; }
        public int MainMenuId { get; set; }
        public int SortOrder { get; set; }
        public string Detail { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string IconClassMenu { get; set; }
        public string QueryString { get; set; }
        public List<string> AllowRoles { get; set; }
    }
}