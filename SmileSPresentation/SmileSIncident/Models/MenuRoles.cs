﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmileSIncident.Helper;

namespace SmileSIncident.Models
{
    public class MenuRoles
    {
        private static List<Menu> GetAllMenu()
        {
            var result = new List<Menu>();

            //string totalRow = "0";

            //insert all menu here
            //operation
            result.Add(new Menu(1, 0, 0, " กล่องขาเข้า", "Home", "Index", "fa fa-inbox", "", "*"));
            result.Add(new Menu(2, 0, 0, " ถาดงาน", "Home", "JobDetail", "fa fa-calendar", "", "Developer,IncidentDev"));
            result.Add(new Menu(3, 0, 0, " ข้อความ", "Home", "JobNewMessage", "fas fa-comment-dots", "", "*"));
            result.Add(new Menu(4, 0, 0, " รายงาน", "", "", "fa fa-file", "", "Developer,IncidentDev"));
            result.Add(new Menu(5, 4, 0, " รายงาน Incident", "Report", "ReportIncident", "fa fa-report", "", "Developer,IncidentDev"));

            return result;
        }

        /// <summary>
        /// Get Menu By username
        /// </summary>
        /// <param name="userName">username</param>
        /// <returns></returns>
        public static List<Menu> GetMenuByUserName()
        {
            var roles = GlobalFunction.GetRoles();

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