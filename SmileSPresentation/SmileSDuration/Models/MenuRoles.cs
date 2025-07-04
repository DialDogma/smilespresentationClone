using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmileSDuration.Helper;

namespace SmileSDuration.Models
{
    public class MenuRoles
    {
        private static List<Menu> GetAllMenu(int branchId)
        {
            var result = new List<Menu>();

            //insert all menu here
            result.Add(new Menu(1, 0, 0, "Duration_Version1", "", "", "fa fa-link ", "", "Developer"));
            result.Add(new Menu(2, 1, 0, "Upload Files", "Upload", "Index", "fa fa-link ", "", "Developer"));
            result.Add(new Menu(3, 1, 0, "Reports", "Reports", "Index", "fa fa-link ", "", "Developer"));

            result.Add(new Menu(4, 0, 0, "DisclosureSMS", "", "", "fa fa-link ", "", "Developer"));
            result.Add(new Menu(5, 4, 0, "Send", "DisclosureSMS", "Index", "fa fa-link ", "", "Developer"));
            result.Add(new Menu(6, 4, 0, "Search", "DisclosureSMS", "SearchSMS", "fa fa-link ", "", "Developer"));

            result.Add(new Menu(7, 0, 0, "Duration_Version2", "", "", "fa fa-link ", "", "SmileDuration_Operation,SmileDuration_Admin,Developer"));
            result.Add(new Menu(8, 7, 0, "รายงานการส่งข้อความ", "Reports", "ReportNew", "fa fa-link", "", "SmileDuration_Operation,SmileDuration_Admin,Developer"));

            result.Add(new Menu(9, 0, 0, "Duration_Message", "", "", "fa fa-link ", "", "Developer"));
            result.Add(new Menu(10, 9, 0, "Send", "DurationMessage", "Index", "fa fa-link", "", "Developer"));
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