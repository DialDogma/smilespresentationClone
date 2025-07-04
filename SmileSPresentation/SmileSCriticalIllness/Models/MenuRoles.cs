using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmileSCriticalIllness.Helper;

namespace SmileSCriticalIllness.Models
{
    public class MenuRoles
    {
        private static List<Menu> GetAllMenu(int branchId)
        {
            var result = new List<Menu>();

            //insert all menu here

            result.Add(new Menu(1, 0, 0, " ประกันมะเร็ง", "", "", "fa fa-search", "", "*"));
            result.Add(new Menu(2, 1, 0, " ค้นหา Application", "Monitor", "FindApplication", "fa fa-search", "", "CriticalIllness_Callcenter,CriticalIllness_Underwrite,Developer"));
            result.Add(new Menu(3, 1, 0, " Monitor Application", "Monitor", "ApplicationMonitor", "fa fa-users", "", "*"));
            result.Add(new Menu(4, 1, 0, " ตรวจสอบ Application", "Monitor", "ApplicationUnderwriteMonitor", "fa fa-check", "", "CriticalIllness_Underwrite,Developer"));
            result.Add(new Menu(5, 1, 0, " บันทึกแจ้งยกเลิก", "CancelApplication", "RequestCancelApplication", "fa fa-exclamation", "", "*"));
            result.Add(new Menu(6, 1, 0, " รายการรอยกเลิก", "CancelApplication", "UnderwriteCancelApplication", "fa fa-trash", "", "CriticalIllness_Underwrite,Developer"));
            result.Add(new Menu(7, 1, 0, " ยืนยันความคุ้มครอง", "ConfirmPolicy", "ConfirmPolicy", "fa fa-file", "", "CriticalIllness_Underwrite,Developer"));
            result.Add(new Menu(8, 1, 0, " รายงาน Application", "Report", "ApplicationReport", "fa fa-tv", "", "*"));
            //result.Add(new Menu(8,0,0," จัดการค่าตอบแทน","","","fa fa-dollar ","","CriticalIllness_Underwrite,Developer"));
            //result.Add(new Menu(9,0,0," รายงานความคุ้มครอง","","","","","CriticalIllness_Underwrite,Developer"));
            result.Add(new Menu(9, 1, 0, " รายการปฎิเสธจาก บ.ประกัน", "ConfirmPolicy", "CancelFromInsurance", "", "", "CriticalIllness_Underwrite,Developer"));
            result.Add(new Menu(10, 0, 0, " ประกัน Covid-19", "", "", "fa fa-heartbeat", "", "*"));
            result.Add(new Menu(11, 10, 0, " บันทึก Application", "Covid19Application", "Index", "fa fa-save", "", "*"));
            result.Add(new Menu(12, 10, 0, " ค้นหา Application", "Covid19Application", "SearchApplication", "fa fa-search", "", "*"));
            result.Add(new Menu(13, 10, 0, " สลักหลังผู้เอาประกัน", "Covid19Application", "EditTable", "fa fa-marker", "", "CriticalIllness_Underwrite,Developer"));
            //result.Add(new Menu(14, 10, 0, " Underwrite", "Covid19Application", "ReportForUnderwrite", "fa fa-search", "", "*"));
            result.Add(new Menu(14, 10, 0, " รายงาน", "", "", "fa fa-print", "", "CriticalIllness_Underwrite,Developer"));
            result.Add(new Menu(15, 14, 0, " รายงานส่งบริษัทประกัน", "Covid19Application", "ReportBroker", "fa fa-building", "", "CriticalIllness_Underwrite,Developer"));
            result.Add(new Menu(16, 14, 0, " รายงานส่งฝ่ายค่าตอบแทน", "Covid19Application", "ReportCompensation", "fa fa-hand-holding-usd", "", "CriticalIllness_Underwrite,Developer"));
            result.Add(new Menu(17, 14, 0, " รายงานสลักหลังผู้เอาประกัน", "Covid19Application", "ReportBeneficiary", "fa fa-people-carry", "", "CriticalIllness_Underwrite,Developer"));

            return result;
        }

        /// <summary>
        /// Get Menu By username
        /// </summary>
        /// <param name="userName">username</param>
        /// <returns></returns>
        public static List<Menu> GetMenuByUserName(string userName, int branchId)
        {
            var roles = new SSOService.SSOServiceClient().GetRoleByUserName(userName);

            var result = new List<Menu>();

            var lstRoles = roles.Split(',').ToList();

            foreach (var item in GetAllMenu(branchId))
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

            foreach (var item in GetAllMenu(branchId))
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
            foreach (var mnu in GetAllMenu(branchId))
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