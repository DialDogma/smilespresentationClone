using SmileSBankDirectDebit.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmileSBankDirectDebit.Models
{
    public class MenuRoles
    {
        private static List<Menu> GetAllMenu()
        {
            var result = new List<Menu>();

            //insert all menu here
            //operation
            result.Add(new Menu(1, 0, 0, "ค้นหาเลขบัญชี", "BankDirectDebit", "Search", "fa fa-link", "", "*"));

            result.Add(new Menu(2, 0, 0, "นำเข้าข้อมูล", "", "", "fa fa-link", "", "Developer,BankDirectDebit_Premium"));
            result.Add(new Menu(3, 2, 0, "Import Data", "BankDirectDebit", "ImportData", "fa fa-link", "", "Developer,BankDirectDebit_Premium"));
            result.Add(new Menu(4, 2, 0, "Import Excel", "BankDirectDebit", "ImportExcel", "fa fa-link", "", "Developer,BankDirectDebit_Premium"));

            result.Add(new Menu(5, 0, 0, "ส่งข้อมูล KTB", "", "", "fa fa-link", "", "Developer,BankDirectDebit_Premium"));
            result.Add(new Menu(6, 5, 0, "นำส่งข้อมูล KTB", "BankDirectDebit", "ExcelForExport", "fa fa-link", "", "Developer,BankDirectDebit_Premium"));
            result.Add(new Menu(7, 5, 0, "รายงานนำส่งข้อมูล KTB", "BankDirectDebit", "ReportForExport", "fa fa-link", "", "Developer,BankDirectDebit_Premium"));

            result.Add(new Menu(8, 0, 0, "หนังสือยินยอม", "", "", "fa fa-link", "", "*"));
            result.Add(new Menu(9, 8, 0, "ส่งหนังสือยินยอมหักธนาคาร", "BankDirectDebit", "AddBankDirectDebitHeader", "fa fa-link", "", "*"));
            result.Add(new Menu(10, 8, 0, "ยกเลิกการนำส่งหนังสือยินยอม", "BankDirectDebit", "ManageDirectDebit", "fa fa-link", "", "*"));

            result.Add(new Menu(11, 0, 0, "รายงาน", "", "", "fa fa-link", "", "*"));
            result.Add(new Menu(12, 11, 0, "ผลการนำเข้าข้อมูล Excel", "BankDirectDebit", "ReportImportData", "fa fa-link", "", "Developer,BankDirectDebit_Premium"));
            result.Add(new Menu(13, 11, 0, "ผลการนำเข้าไฟล์ Text", "BankDirectDebit", "ReportImportFile", "fa fa-link", "", "Developer,BankDirectDebit_Premium"));
            result.Add(new Menu(14, 11, 0, "ผลนำส่งเอกสารหนังสือยินยอม", "BankDirectDebit", "ReportForPremium", "fa fa-link", "", "*"));
            result.Add(new Menu(15, 11, 0, "รับรองบัญชีแยกตาม AppID", "BankDirectDebit", "ReportApprovedByAppID", "fa fa-link", "", "Developer,BankDirectDebit_Premium"));
            result.Add(new Menu(16, 11, 0, "ติดตามการรับรองบัญชี (สาขา)", "BankDirectDebit", "ReportFollowAccountBranch", "fa fa-link", "", "Developer,BankDirectDebit_Branch"));
            result.Add(new Menu(17, 11, 0, "ติดตามการรับรองบัญชี (ตัวแทน)", "BankDirectDebit", "ReportFollowAccountAgent", "fa fa-link", "", "*"));

            result.Add(new Menu(18, 0, 0, "ตรวจสอบเอกสาร", "BankDirectDebit", "DocumentMonitor", "fa fa-link", "", "*"));
            result.Add(new Menu(19, 0, 0, "ตรวจสอบการรับรองบัญชี", "BankDirectDebit", "CheckApproved", "fa fa-link", "", "Developer,BankDirectDebit_Premium"));

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