using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmileSUnderwriteAudit.SSOService;

namespace SmileSUnderwriteAudit.Models
{
    public class MenuRolesMotor
    {
        private static List<MenuMotor> GetAllMenu()
        {
            var result = new List<MenuMotor>
            {
                //insert all menu here
                new MenuMotor(1, 0, 0, " หน้าแรก", "MotorAuditHome", "Index", "", "", "*"),
                new MenuMotor(2, 0, 0, " สร้างคิวงาน", "MotorQueue", "MotorQueueCreate", "", "", "*"),
                new MenuMotor(3, 0, 0, " ตรวจสอบผลการคัดกรอง", "MotorQueue", "MotorQueueCheckUnderwriteResult", "", "", "*"),
                new MenuMotor(4, 0, 0, " รายงาน", "MotorReport", "MotorReportCheckUnderwrite", "", "", "*"),
                new MenuMotor(5, 0, 0, " จัดการคิวงาน", "MotorQueue", "MotorQueueManageAssign", "", "", "*"),
            };

            return result;
        }

        /// <summary>
        /// Get Menu By username
        /// </summary>
        /// <param name="userName">username</param>
        /// <returns></returns>
        public static List<MenuMotor> GetMenuByUserName(string userName, int branchId)
        {
            var roles = new SSOServiceClient().GetRoleByUserName(userName);

            var result = new List<MenuMotor>();

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
        public static List<MenuMotor> GetMenuByRole(string role, int branchId)
        {
            var result = new List<MenuMotor>();

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

    public class MenuMotor
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
        public MenuMotor(int menuId, int mainMenuId, int sortOrder, string detail, string controller, string action, string iconClassMenu, string queryString, string allowRoleNames)
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