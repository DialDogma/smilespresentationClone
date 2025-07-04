using SmileSClaimOnLine.Controllers;
using SmileSClaimOnLine.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSClaimOnLine.Models
{
    public class MenuRoles
    {
        private static List<Menu> GetAllMenu()
        {
            var result = new List<Menu>();
            //insert all menu here
            //operation  version 1
            //result.Add(new Menu(1, 0, 0, "บันทึกเคลมออนไลน์", "ClaimOnLine", "ClaimOnLine", "fa fa-check-square-o", "", "SmileSClaimOnLine_Manager,SmileSClaimOnLine_Operation,Developer"));
            //result.Add(new Menu(17, 0, 0, "บันทึกเคลม", "Claim", "ClaimOnLineSearch", "fa  fa-edit ", "", "SmileSClaimOnLine_Manager,SmileSClaimOnLine_Operation,Developer"));

            int TimeStartReport = Properties.Settings.Default.TimeStartReport;
            int TimeEndReport = Properties.Settings.Default.TimeEndReport;
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            int CurrentHours = currentTime.Hours;

            //version  2
            result.Add(new Menu(17, 0, 0, "บริการเคลมออนไลน์", "Claim", "ClaimDetailMonitoring", "fa  fa-edit ", "", "SmileSClaimOnLine_Manager,SmileSClaimOnLine_Operation,Developer,SmileSClaimOnLine_Supervisor"));

            //manager
            var isAllowClaimAI = new ClaimOnLineController().CheckAllowClaimAIByEmployeeCode();
            if (isAllowClaimAI)
            {
                result.Add(new Menu(33, 0, 0, "เปิดการใช้งาน AI", "ClaimOnLine", "ClaimAIPayAutoSettings", "fa fa-gear", "", "Developer"));
            }
            result.Add(new Menu(22, 0, 0, "บันทึกการโอนเงิน", "ClaimOnLine", "ClaimOnLineMonitoring", "fa  fa-edit ", "", "Developer"));
            result.Add(new Menu(2, 0, 0, "Monitor และบันทึกโอนเงิน", "ClaimOnLine", "MonitorClaimOnLine", "fa fa-check-square-o", "", "Developer"));
            result.Add(new Menu(3, 0, 0, "บันทึกโอนเพิ่ม", "Premium", "TransferPremium", "fa fa-check-square-o", "", "Developer"));
            result.Add(new Menu(4, 0, 0, "บันทึกคืนเงิน", "Premium", "ReturnPremium", "fa fa-check-square-o", "", "Developer"));
            result.Add(new Menu(5, 0, 0, "ยกเลิก ClaimOnLine", "ClaimOnLine", "CancelClaimOnLine", "fa  fa-edit ", "", "Developer"));
            result.Add(new Menu(6, 0, 0, "บันทึกรับเงิน", "Premium", "FundTransfer", "fa  fa-edit ", "", "Developer"));
            result.Add(new Menu(30, 0, 0, "บันทึกตัดรับชำระเคลมคงค้าง", "Premium", "FundTransferImport", "fa  fa-edit ", "", "Developer"));
            result.Add(new Menu(7, 0, 0, "แก้ไข ClaimOnLine", "ClaimOnLine", "MonitorForEditClaimOnLine", "fa  fa-edit ", "", "Developer"));
            result.Add(new Menu(8, 0, 0, "บันทึกปรับยอดบัญชี", "Premium", "ReceivePremium", "fa  fa-edit ", "", "Developer"));
            result.Add(new Menu(9, 0, 0, "แก้ไขโอนเงิน", "ClaimOnLine", "TransferEndorse", "fa  fa-edit ", "", "Developer"));
            result.Add(new Menu(9, 0, 0, "ยกเลิกรับเงิน", "ClaimOnLine", "CancelClaimOnLineTransferItem", "fa  fa-edit ", "", "Developer"));
            result.Add(new Menu(15, 0, 0, "แก้ไข ClaimOnLineCode", "ClaimOnLine", "COLEndorse", "fa  fa-edit ", "", "Developer"));
            result.Add(new Menu(16, 0, 0, "COL Query", "Manage", "ManageClaimOnline", "fa  fa-edit ", "", "Developer"));
            result.Add(new Menu(32, 0, 0, "นำเข้ารายชื่อ WhiteList", "ClaimOnLine", "WhiteListImport", "fa  fa-edit ", "", "Developer"));

            //report
            result.Add(new Menu(10, 0, 0, "รายงาน", "", "", "fa fa-pie-chart", "", "SmileSClaimOnLine_Manager,Developer,SmileSClaimOnLine_Supervisor,SmileSClaimOnLine_Operation"));
            if (CurrentHours >= TimeStartReport || CurrentHours < TimeEndReport)
            {
                result.Add(new Menu(11, 10, 0, "รายงานสำรองจ่ายเคลม(สาขา)", "Report", "ClaimOnLineReserveForBranchReport", "fa fa-pie-chart", "", "SmileSClaimOnLine_Manager,Developer,SmileSClaimOnLine_Supervisor,SmileSClaimOnLine_Operation"));
            }
            result.Add(new Menu(12, 10, 0, "รายงานสรุปชุดข้อมูล", "Report", "ClaimOnLineHeaderGroupReport", "fa fa-pie-chart", "", "SmileSClaimOnLine_Manager,Developer,SmileSClaimOnLine_Supervisor,SmileSClaimOnLine_Operation"));
            result.Add(new Menu(13, 10, 0, "รายงานชุดเคลมสำรองจ่าย", "Report", "ClaimOnLineReport", "fa fa-pie-chart", "", "SmileSClaimOnLine_Manager,Developer,SmileSClaimOnLine_Supervisor"));
            result.Add(new Menu(14, 10, 0, "รายงานสมุดบัญชี", "Report", "ClaimOnLineAccountReport", "fa fa-pie-chart", "", "SmileSClaimOnLine_Manager,Developer,SmileSClaimOnLine_Supervisor"));
            result.Add(new Menu(18, 10, 0, "รายงานส่งค่าตอบแทน", "Report", "ClaimOnLineForCompensateReport", "fa fa-pie-chart", "", "SmileSClaimOnLine_Manager,Developer,SmileSClaimOnLine_Supervisor"));
            result.Add(new Menu(19, 10, 0, "รายงานติดตามเคลมออนไลน์สาขา", "Report", "ClaimOnLineFollowClaimBranchReport", "fa fa-pie-chart", "", "SmileSClaimOnLine_Manager,Developer,SmileSClaimOnLine_Supervisor"));
            result.Add(new Menu(20, 10, 0, "รายงานการสร้างและบันทึกเคลม", "Report", "CreatedClaimReport", "fa fa-pie-chart", "", "SmileSClaimOnLine_Manager,Developer,SmileSClaimOnLine_Supervisor"));
            result.Add(new Menu(21, 10, 0, "รายงานแก้ไขผู้ให้บริการ", "Report", "ClaimOnLineRemarkReport", "fa fa-pie-chart", "", "SmileSClaimOnLine_Manager,Developer"));
            result.Add(new Menu(23, 10, 0, "รายงานชุดเคลมสำรองจ่ายทั้งหมด", "Report", "ClaimOnLineAllReport", "fa fa-pie-chart", "", "SmileSClaimOnLine_Manager,Developer"));
            result.Add(new Menu(24, 10, 0, "รายงานค่าตอบแทน", "Report", "ClaimOnLineCompensationReport", "fa fa-pie-chart", "", "SmileSClaimOnLine_Manager,Developer"));
            result.Add(new Menu(25, 10, 0, "รายงานตัดรับชำระ", "Report", "ClaimOnLineCutOffPaymentReport", "fa fa-pie-chart", "", "SmileSClaimOnLine_Manager,Developer,SmileSClaimOnLine_Supervisor"));
            result.Add(new Menu(27, 10, 0, "รายงานรีเจคเคลม", "Report", "ClaimOnLineRejectClaimReport", "fa fa-pie-chart", "", "SmileSClaimOnLine_Manager,Developer,SmileSClaimOnLine_Supervisor"));
            result.Add(new Menu(28, 10, 0, "รายงานคืนเงิน", "Report", "RefundTransferReport",
                "fa fa-pie-chart", "", "SmileSClaimOnLine_Manager,Developer,SmileSClaimOnLine_Supervisor"));
            result.Add(new Menu(29, 10, 0, "รายงานการโอนเงินเคลมออนไลน์", "Report", "ConsiderTransferPremiumReport", "fa fa-pie-chart", "", "SmileSClaimOnLine_Manager,Developer,SmileSClaimOnLine_Supervisor"));
            result.Add(new Menu(31, 10, 0, "รายงานการโอนเงินประจำวัน", "Report", "ConsiderTransferPremiumDailyReport", "fa fa-pie-chart", "", "SmileSClaimOnLine_Manager,Developer,SmileSClaimOnLine_Supervisor"));
            if (isAllowClaimAI)
            {
                result.Add(new Menu(34, 10, 0, "รายงานผลการพิจารณา AI", "Report", "ClaimAIReviewReport", "fa fa-pie-chart", "", "*"));
            } 
                result.Add(new Menu(35, 10, 0, "รายงานกระทบยอดการโอนเงิน", "Report", "ClaimOnlineTranferReconcileReport", "fa fa-pie-chart", "", "Developer,SmileSClaimOnLine_Manager"));
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