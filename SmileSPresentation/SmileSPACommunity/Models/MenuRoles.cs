using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSPACommunity.Models
{
    public class MenuRoles
    {
        private static List<Menu> GetAllMenu()
        {
            var result = new List<Menu>();

            //insert all menu here
            result.Add(new Menu(100, 0, 0, "PACommunity", "", "", "glyphicon glyphicon-user", "", "*"));
            result.Add(new Menu(10, 100, 0, "ค้นหา Application", "PACommunity", "ApplicationSearch", "glyphicon glyphicon-search", "", "*"));

            //NewApp
            result.Add(new Menu(1, 100, 0, "PAชุมชน NewApp", "PACommunity", "PACommunityMonitor", "glyphicon glyphicon-plus", "", "PACommunity_MO,PACommunity_PAUnderwrite,Developer"));
            result.Add(new Menu(2, 100, 0, "PAชุมชน ไม่อนุมัติ", "PACommunity", "PAMonitorUnapprove", "glyphicon glyphicon-warning-sign", "", "PACommunity_MO,PACommunity_PAUnderwrite,Developer"));

            //Approve
            result.Add(new Menu(6, 100, 0, "PAชุมชน รออนุมัติ", "Approve", "PAMonitorApprove", "glyphicon glyphicon-list-alt", "", "PACommunity_PAUnderwrite,Developer"));

            //Manage
            result.Add(new Menu(9, 100, 0, "การจัดการชื่อกลุ่มกรมธรรม์", "Manage", "ManagePACommunitySystem", "glyphicon glyphicon-folder-open", "", "PACommunity_PAUnderwrite,Developer"));

            //Endorse
            result.Add(new Menu(3, 100, 0, " บันทึกสลักหลัง PAชุมชน", "", "", "glyphicon glyphicon-pencil", "", "PACommunity_MO,PACommunity_PAUnderwrite,Developer"));
            result.Add(new Menu(4, 3, 0, "ยกเลิกกรมธรรม์", "Endorse", "EndorseCancelPolicyMonitor", "glyphicon glyphicon-pencil", "", "PACommunity_MO,PACommunity_PAUnderwrite,Developer"));
            result.Add(new Menu(12, 3, 0, "แก้ไขผู้ประสานงานชุมชน", "Monitor", "EndorseApplicationContactMonitor", "glyphicon glyphicon-pencil", "", "PACommunity_MO,PACommunity_PAUnderwrite,Developer"));
            //result.Add(new Menu(5, 3, 0, "ยกเลิกผู้เอาประกัน", "Endorse", "EndorseCancelCustomerDetailMonitor", "glyphicon glyphicon-pencil", "", "PACommunity_MO,PACommunity_PAUnderwrite,Developer"));

            //Report
            result.Add(new Menu(7, 100, 0, " รายงาน", "", "", "glyphicon glyphicon-folder-open", "", "PACommunity_PAUnderwrite,Developer"));
            result.Add(new Menu(8, 7, 0, "รายงานยืนยันออกกรมธรรม์", "Report", "PolicyReport", "glyphicon glyphicon-folder-open", "", "PACommunity_PAUnderwrite,Developer"));
            result.Add(new Menu(9, 7, 0, "รายงานยกเลิกกรมธรรม์", "Report", "CancelReport", "glyphicon glyphicon-folder-open", "", "PACommunity_PAUnderwrite,Developer"));
            result.Add(new Menu(11, 7, 0, "รายงานคิดค่าตอบแทน", "Report", "CompensationReport", "glyphicon glyphicon-folder-open", "", "PACommunity_PAUnderwrite,Developer"));

            //Import Policy Number
            result.Add(new Menu(15, 100, 0, " นำเข้าเลขกรมธรรม์", "PACommunity", "ImportPolicyNumber", "glyphicon glyphicon-open", "", "PACommunity_PAUnderwrite,Developer"));
            //Import Group Control
            //result.Add(new Menu(15, 100, 0, " นำเข้าไฟล์คุมเอกสาร", "PAPersonnel", "PAPersonnelImportGroupControl", "glyphicon glyphicon-open", "", "PACommunity_PAUnderwrite,Developer"));

            //Manage Policy Number
            result.Add(new Menu(16, 100, 0, " บันทึกเลขกรมธรรม์", "PACommunity", "PolicyNumberManagement", "glyphicon glyphicon-floppy-disk", "", "PACommunity_PAUnderwrite,Developer"));

            result.Add(new Menu(16, 100, 0, " พิมพ์บัตรประกัน", "Report", "PrintCardMonitor", "glyphicon glyphicon-print", "", "*"));

            //PAPersonnel Menu
            result.Add(new Menu(200, 0, 0, "PA บุคลากร ยิ้มแฉ่ง", "", "", "glyphicon glyphicon-user", "", "*"));
            //PAPersonnel Search
            result.Add(new Menu(17, 200, 0, "ค้นหาข้อมูลกรมธรรม์", "PAPersonnel", "PAPersonnelSearch", "glyphicon glyphicon-search", "", "*"));
            //PAPersonnel NewApp
            result.Add(new Menu(18, 200, 0, "บันทึกกรมธรรม์", "PAPersonnel", "PAPersonnelMonitor", "glyphicon glyphicon-plus", "", "*"));
            //PAPersonnel Approve
            result.Add(new Menu(19, 200, 0, "รออนุมัติกรมธรรม์", "PAPersonnelApprove", "PAPersonnelApproveMonitor", "glyphicon glyphicon-info-sign", "", "PAPersonnel_PAUnderwrite,Developer"));
            //PAPersonnel UnApprove
            result.Add(new Menu(20, 200, 0, "ไม่อนุมัติกรมธรรม์", "PAPersonnel", "PAPersonnelUnApproveMonitor", "glyphicon glyphicon-remove-circle", "", "*"));
            //Import Policy Number
            result.Add(new Menu(21, 200, 0, "นำเข้าเลขกรมธรรม์", "PAPersonnel", "PAPersonnelImportPolicyNumber", "glyphicon glyphicon-open", "", "PAPersonnel_PAUnderwrite,Developer"));
            //Import Group Control
            //result.Add(new Menu(22, 200, 0, "นำเข้าไฟล์คุมเอกสาร", "PAPersonnel", "PAPersonnelImportGroupControl", "glyphicon glyphicon-open", "", "PAPersonnel_PAUnderwrite,Developer"));

            //Endorse
            result.Add(new Menu(23, 200, 0, "บันทึกสลักหลัง PAยิ้มแฉ่ง", "", "", "glyphicon glyphicon-pencil", "", "*"));
            result.Add(new Menu(24, 23, 0, "เพิ่มรายชื่อผู้เอาประกัน", "PAPersonnelEndorse", "PAPersonnelAddCustomerMonitor", "glyphicon glyphicon-pencil", "", "*"));
            //result.Add(new Menu(25,24,0, "บันทึกขอเพิ่มรายชื่อผู้เอาประกัน", "PAPersonnelEndorse", "AddCustomerManage", "glyphicon glyphicon-pencil", "","*"));
            result.Add(new Menu(26, 23, 0, "ขอยกเลิกกรมธรรม์", "PAPersonnelEndorse", "PAPersonnelEndorseRequestCancelMonitor", "glyphicon glyphicon-pencil", "", "*"));
            result.Add(new Menu(32, 23, 0, "แก้ไขชื่อผู้เอาประกัน", "PAPersonnelCustomerData", "Index", "glyphicon glyphicon-pencil", "", "*")); 
            result.Add(new Menu(31, 200, 0, "ผลการส่ง SMS", "PAPersonnelReport", "ResultSendSMSReport", "glyphicon glyphicon-folder-open", "", "PAPersonnel_PAUnderwrite,Developer"));

            //PAPersonnel Report
            result.Add(new Menu(27, 200, 0, "รายงาน", "", "", "glyphicon glyphicon-folder-open", "", "PAPersonnel_PAUnderwrite,Developer"));
            result.Add(new Menu(28, 27, 0, "รายงานขอออกกรมธรรม์", "PAPersonnelReport", "RequestPolicyReport", "glyphicon glyphicon-folder-open", "", "PAPersonnel_PAUnderwrite,Developer"));
            result.Add(new Menu(29, 27, 0, "รายงาน Application", "PAPersonnelReport", "ApplicationReport", "glyphicon glyphicon-folder-open", "", "PAPersonnel_PAUnderwrite,Developer"));          
            result.Add(new Menu(30, 27, 0, "รายงานผลการส่ง SMS", "PAPersonnelReport", "ResultSendSMSForExport", "glyphicon glyphicon-folder-open", "", "PAPersonnel_PAUnderwrite,Developer"));

            // create 09/12/2023 
            result.Add(new Menu(33, 27, 0, "รายงานแก้ไขรายชื่อผู้เอาประกัน", "PAPersonnelCustomerDataReport", "Index", "glyphicon glyphicon-folder-open", "", "PAPersonnel_PAUnderwrite,Developer"));



            return result;
        }

        /// <summary>
        /// Get Menu By username
        /// </summary>
        /// <param name="userName">username</param>
        /// <returns></returns>
        public static List<Menu> GetMenuByUserName(string userName)
        {
            var roles = new SSOService.SSOServiceClient().GetRoleByUserName(userName);

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