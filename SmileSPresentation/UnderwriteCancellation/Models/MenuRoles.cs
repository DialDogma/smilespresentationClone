using System;
using System.Collections.Generic;
using System.Linq;
using UnderwriteCancellation.Helper;

namespace UnderwriteCancellation.Models
{
    public class MenuRoles
    {
        private static List<Menu> GetAllMenu()
        {
            var result = new List<Menu>();

            //insert all menu here

            result.Add(new Menu(1, 0, 0, "หน้าแรก", "Home", "ManagerQC", "fa fa-home", "", "Developer,UWCancellation_QCManager,UWCancellation_QC"));
            result.Add(new Menu(2, 0, 0, "สร้างคิวงาน", "QueueCreate", "QueueCreateCancellation", "fa fa-plus-circle", "", "Developer,UWCancellation_QCManager"));
            result.Add(new Menu(3, 0, 0, "ตรวจสอบข้อมูลลูกค้ายกเลิก", "", "WaitCloseQueue", "fa fa-check-square-o", "", "Developer,UWCancellation_QC"));
            result.Add(new Menu(4, 3, 0, "รอปิดคิวงาน", "QueueManageAssign", "WaitCloseQueue", "fa fa-hourglass-2", "", "Developer,UWCancellation_QC"));
            result.Add(new Menu(5, 3, 0, "ปิดคิวงานสำเร็จ", "QueueManageAssign", "CloseQueueSuccess", "fa fa-check", "", "Developer,UWCancellation_QC"));
            result.Add(new Menu(6, 0, 0, "จัดการคิวงาน", "QueueManageAssign", "QueueManageAssign", "fa fa-phone", "", "Developer,UWCancellation_QCManager,UWCancellation_QC"));
            result.Add(new Menu(7, 0, 0, "คิวงานมีประเด็น", "", "Queuepending", "fa fa-exclamation", "", "Developer,UWCancellation_QCManager,UWCancellation_BusinessPromoteTeam"));
            result.Add(new Menu(8, 7, 0, "รอดำเนินการ", "QueueManageAssign", "Queuepending", "fa fa-hourglass-o", "", "Developer,UWCancellation_QCManager,UWCancellation_BusinessPromoteTeam"));
            result.Add(new Menu(9, 7, 0, "ดำเนินการแล้ว", "QueueManageAssign", "QueueIssueSuccess", "fa fa-hourglass", "", "Developer,UWCancellation_QCManager,UWCancellation_BusinessPromoteTeam"));
            result.Add(new Menu(10, 0, 0, "ระบบจัดการทีม", "TeamManageAssign", "TeamManageAssignCancellation", "fa fa-user-plus", "", "Developer,UWCancellation_QCManager"));
            result.Add(new Menu(11, 0, 0, "รายงาน", "", "", "fa fa-folder-o", "", "Developer,UWCancellation_QCManager,UWCancellation_QC,UWCancellation_RiskManagement,UWCancellation_BusinessPromoteTeam"));
            result.Add(new Menu(12, 11, 0, "รายงานการโทรหาลูกค้ายกเลิก", "Report", "ReportCancellation", "fa fa-folder-o", "", "Developer,UWCancellation_QCManager,UWCancellation_QC,UWCancellation_RiskManagement"));
            result.Add(new Menu(13, 11, 0, "รายงานคิวงานมีประเด็น", "Report", "ReportQueueIssueSuccess", "fa fa-folder-o", "", "Developer,UWCancellation_QCManager,UWCancellation_BusinessPromoteTeam"));

            return result;
        }

        /// <summary>
        /// Get Menu By username
        /// </summary>
        /// <param name="userName">username</param>
        /// <returns></returns>
        public static List<Menu> GetMenuByUserName(string userName)
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
        public static List<Menu> GetMenuByRole(string role)
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
        public static List<string> GetAllRoles()
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