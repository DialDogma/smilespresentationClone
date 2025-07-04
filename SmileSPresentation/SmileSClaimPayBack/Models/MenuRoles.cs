using SmileSClaimPayBack.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSClaimPayBack.Models
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

            ////version 2
            //result.Add(new Menu(1, 0, 0, "การตั้งเบิกเคลม", "ClaimPayBack", "ClaimPayBackMonitor", " ", "", "SmileClaimPayBack_Operation,Developer"));
            //result.Add(new Menu(2, 0, 0, "บันทึกโอนเงิน", "ClaimPayBack", "ClaimPayBackTransferPaymentMonitor", " ", "", "SmileClaimPayBack_Finance,Developer"));
            //result.Add(new Menu(3, 0, 0, "รายงาน", "", "", " ", "", "*"));
            //result.Add(new Menu(4, 3, 0, "รายงานการโอนเงิน", "ClaimPayBack", "TransferPaymentReport", " ", "", "SmileClaimPayBack_Operation,SmileClaimPayBack_Finance,Developer"));
            //result.Add(new Menu(4, 3, 0, "รายงานการยกเลิก", "ClaimPayBack", "ClaimPayBackCancelledReport", " ", "", "SmileClaimPayBack_Operation,SmileClaimPayBack_Finance,Developer"));

            //version 2.1 20221018 06074 Add new Menu 
            var environment = Properties.Settings.Default.Environment;
            if (environment == "UAT")
            {
                //Menu in UAT
                result.Add(new Menu(1, 0, 0, "การตั้งเบิกเคลม", "ClaimPayBack", "ClaimPayBackMonitor", " ", "", "SmileClaimPayBack_Operation,Developer"));
                result.Add(new Menu(2, 0, 0, "บันทึกการโอนเงิน", "", "", " ", "", "*"));
                result.Add(new Menu(20, 2, 0, "สร้างรายการโอนเงิน", "ClaimPayBack", "CreateClaimPayBackSubGroup", " ", "", "SmileClaimPayBack_FundManage,Developer"));
                result.Add(new Menu(14, 2, 0, "การโอนเงิน", "ClaimPayBack", "ClaimPayBackTransferPaymentMonitor", " ", "", "SmileClaimPayBack_Payer,SmileClaimPayBack_FundManage,Developer"));
                result.Add(new Menu(15, 2, 0, "แก้ไขการโอนเงิน", "ClaimPayBack", "ClaimPayBackRePayTransferMonitor", " ", "", "SmileClaimPayBack_Payer,SmileClaimPayBack_FundManage,Developer"));

                result.Add(new Menu(7, 0, 0, "บันทึกยอดเงินบริษัทประกัน", "", "", " ", "", "Developer,SmileClaimPayBack_Finance"));
                result.Add(new Menu(8, 7, 0, "บันทึกยอดเงินบริษัทประกัน", "ClaimPayBack", "ClaimPayBackSaveBalance", " ", " ", "Developer,SmileClaimPayBack_Finance"));
                result.Add(new Menu(8, 7, 0, "Import ไฟล์บันทึกยอดเงิน", "ClaimPayBack", "ClaimPayBackImportSaveBalance", " ", " ", "Developer,SmileClaimPayBack_Finance"));
                // Report
                result.Add(new Menu(3, 0, 0, "รายงานการจ่าย", "", "", " ", "", "*"));
                result.Add(new Menu(4, 3, 0, "รายงานการโอนเงิน(เดิม)", "ClaimPayBack", "TransferPaymentReport", " ", "", "SmileClaimPayBack_Operation,SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(16, 3, 0, "รายงานส่งการเงิน", "ClaimPayBack", "FinancialTransactionReport", " ", "", "SmileClaimPayBack_Operation,SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(16, 3, 0, "รายงานส่งการเงิน SMI", "ClaimPayBack", "FinancialSMITransactionReport", " ", "", "SmileClaimPayBack_Operation,SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(17, 3, 0, "รายงานการโอนเงิน", "ClaimPayBack", "PaymentTransferReport", " ", "", "SmileClaimPayBack_Operation,SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(18, 3, 0, "รายงานรายละเอียดการโอนเงิน", "ClaimPayBack", "PaymentTransferDetailReport", " ", "", "SmileClaimPayBack_Operation,SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(18, 3, 0, "รายงานรายละเอียดการโอนเงิน SMI", "ClaimPayBack", "PaymentTransferDetailSMIReport", " ", "", "SmileClaimPayBack_Operation,SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(13, 3, 0, "รายงานกระทบยอดเคลม", "ClaimPayBack", "ReconcileMonitor", " ", "", "SmileClaimPayBack_FundManage,Developer"));// add by bunchuai 13/06/2024
                result.Add(new Menu(4, 3, 0, "รายงานการยกเลิก", "ClaimPayBack", "ClaimPayBackCancelledReport", " ", "", "SmileClaimPayBack_Operation,SmileClaimPayBack_Finance,Developer"));

                result.Add(new Menu(19, 0, 0, "รายงานการวางบิล", "", "", " ", "", "*"));
                result.Add(new Menu(4, 19, 0, "รายงานกระทบยอดส่งบริษัทประกัน", "ClaimPayBack", "BillingRequestReport", " ", "", "SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(4, 19, 0, "รายงานการวางบิลบริษัทประกัน", "ClaimPayBack", "BillingReport", " ", "", "SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(4, 19, 0, "รายงานยอดเงินที่คาดว่าจะได้รับ", "ClaimPayBack", "EstimatePaymentReport", " ", "", "SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(4, 19, 0, "รายงานรับชำระจากบริษัทประกัน", "ClaimPayBack", "AmountPaymentReport", " ", "", "SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(4, 19, 0, "รายงานผลต่างการชำระเงิน", "ClaimPayBack", "PaymentDifferenceResultReport", " ", "", "SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(4, 19, 0, "รายงานรายละเอียดยอดคงค้าง", "ClaimPayBack", "OutStandingBalanceDetailReport", " ", "", "SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(4, 19, 0, "รายงานเคลมปฏิเสธ", "ClaimPayBack", "BillingRejectClaimReport", " ", "", "SmileClaimPayBack_Finance,Developer"));
                // EndReport

                result.Add(new Menu(5, 0, 0, "รายงานไฟล์นำส่ง บริษัทประกัน", "", "", " ", "", "Developer,SmileClaimPayBack_Finance"));
                result.Add(new Menu(6, 5, 0, "Import บ.ส.", "ClaimPayBack", "ClaimPayBackBillingRequestGroupImport", " ", "", "Developer,SmileClaimPayBack_Finance"));
                result.Add(new Menu(6, 5, 0, "Export ไฟล์นำส่ง บริษัทประกัน", "ClaimPayBack", "ClaimPayBackBillingRequestGroupExport", " ", "", "Developer,SmileClaimPayBack_Finance"));

                // update 2024-05-17 bunchuai
                result.Add(new Menu(13, 0, 0, "อนุมัติตอบกลับบริษัทประกัน", "ClaimPayBack", "Out2ApproveMonitor", " ", "", "SmileClaimPayBack_Finance,Developer"));

                //update 2023-05-05 Chanadol
                result.Add(new Menu(9, 0, 0, "Dashboard ผลตอบกลับ", "", "", " ", "", "Developer,SmileClaimPayBack_Finance,"));
                result.Add(new Menu(10, 9, 0, "ผลอนุมัติเคลม (จาก บริษัทประกัน)", "ClaimPayBack", "DashboardClaimApproveResult", " ", "", "SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(10, 9, 0, "ยอดคงค้าง บริษัทประกัน", "ClaimPayBack", "DashboardOutStandingBalance", " ", "", "SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(10, 9, 0, "เคลมปฏิเสธ", "ClaimPayBack", "DashboardRejectClaim", " ", "", "SmileClaimPayBack_Finance,Developer"));

                // import excel data by bunchuai 2024/02/28
                result.Add(new Menu(11, 0, 0, "ตัดรับชำระ COL", "", "", " ", "", "SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(12, 11, 0, "ตัดรับชำระสินไหม OnLine", "ClaimPayBack", "NonPerformingloanMonitor", " ", "", "SmileClaimPayBack_Finance,Developer"));

            }
            else
            {
                //Menu in Production
                result.Add(new Menu(1, 0, 0, "การตั้งเบิกเคลม", "ClaimPayBack", "ClaimPayBackMonitor", " ", "", "SmileClaimPayBack_Operation,Developer"));
                result.Add(new Menu(2, 0, 0, "บันทึกการโอนเงิน", "", "", " ", "", "*"));
                result.Add(new Menu(20, 2, 0, "สร้างรายการโอนเงิน", "ClaimPayBack", "CreateClaimPayBackSubGroup", " ", "", "SmileClaimPayBack_FundManage,Developer"));
                result.Add(new Menu(14, 2, 0, "การโอนเงิน", "ClaimPayBack", "ClaimPayBackTransferPaymentMonitor", " ", "", "SmileClaimPayBack_Payer,SmileClaimPayBack_FundManage,Developer"));
                result.Add(new Menu(15, 2, 0, "แก้ไขการโอนเงิน", "ClaimPayBack", "ClaimPayBackRePayTransferMonitor", " ", "", "SmileClaimPayBack_Payer,SmileClaimPayBack_FundManage,Developer"));

                //update 2023-10-05 add role กองทุน
                result.Add(new Menu(7, 0, 0, "บันทึกยอดเงินบริษัทประกัน", "", "", " ", "", "Developer,SmileClaimPayBack_Finance"));
                result.Add(new Menu(8, 7, 0, "บันทึกยอดเงินบริษัทประกัน", "ClaimPayBack", "ClaimPayBackSaveBalance", " ", " ", "Developer,SmileClaimPayBack_Finance"));
                result.Add(new Menu(8, 7, 0, "Import ไฟล์บันทึกยอดเงิน", "ClaimPayBack", "ClaimPayBackImportSaveBalance", " ", " ", "Developer,SmileClaimPayBack_Finance"));

                result.Add(new Menu(3, 0, 0, "รายงานการจ่าย", "", "", " ", "", "*"));
                result.Add(new Menu(4, 3, 0, "รายงานการโอนเงิน(เดิม)", "ClaimPayBack", "TransferPaymentReport", " ", "", "SmileClaimPayBack_Operation,SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(16, 3, 0, "รายงานส่งการเงิน", "ClaimPayBack", "FinancialTransactionReport", " ", "", "SmileClaimPayBack_Operation,SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(16, 3, 0, "รายงานส่งการเงิน SMI", "ClaimPayBack", "FinancialSMITransactionReport", " ", "", "SmileClaimPayBack_Operation,SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(17, 3, 0, "รายงานการโอนเงิน", "ClaimPayBack", "PaymentTransferReport", " ", "", "SmileClaimPayBack_Operation,SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(18, 3, 0, "รายงานรายละเอียดการโอนเงิน", "ClaimPayBack", "PaymentTransferDetailReport", " ", "", "SmileClaimPayBack_Operation,SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(18, 3, 0, "รายงานรายละเอียดการโอนเงิน SMI", "ClaimPayBack", "PaymentTransferDetailSMIReport", " ", "", "SmileClaimPayBack_Operation,SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(13, 3, 0, "รายงานกระทบยอดเคลม", "ClaimPayBack", "ReconcileMonitor", " ", "", "SmileClaimPayBack_FundManage,Developer"));// add by bunchuai 13/06/2024
                result.Add(new Menu(4, 3, 0, "รายงานการยกเลิก", "ClaimPayBack", "ClaimPayBackCancelledReport", " ", "", "SmileClaimPayBack_Operation,SmileClaimPayBack_Finance,Developer"));

                result.Add(new Menu(19, 0, 0, "รายงานการวางบิล", "", "", " ", "", "*"));
                result.Add(new Menu(4, 19, 0, "รายงานกระทบยอดส่งบริษัทประกัน", "ClaimPayBack", "BillingRequestReport", " ", "", "SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(4, 19, 0, "รายงานการวางบิลบริษัทประกัน", "ClaimPayBack", "BillingReport", " ", "", "SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(4, 19, 0, "รายงานยอดเงินที่คาดว่าจะได้รับ", "ClaimPayBack", "EstimatePaymentReport", " ", "", "SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(4, 19, 0, "รายงานรับชำระจากบริษัทประกัน", "ClaimPayBack", "AmountPaymentReport", " ", "", "SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(4, 19, 0, "รายงานผลต่างการชำระเงิน", "ClaimPayBack", "PaymentDifferenceResultReport", " ", "", "SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(4, 19, 0, "รายงานรายละเอียดยอดคงค้าง", "ClaimPayBack", "OutStandingBalanceDetailReport", " ", "", "SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(4, 19, 0, "รายงานเคลมปฏิเสธ", "ClaimPayBack", "BillingRejectClaimReport", " ", "", "SmileClaimPayBack_Finance,Developer"));

                result.Add(new Menu(5, 0, 0, "รายงานไฟล์นำส่ง บริษัทประกัน", "", "", " ", "", "Developer,SmileClaimPayBack_Finance"));
                result.Add(new Menu(6, 5, 0, "Import บ.ส.", "ClaimPayBack", "ClaimPayBackBillingRequestGroupImport", " ", "", "Developer,SmileClaimPayBack_Finance"));
                result.Add(new Menu(6, 5, 0, "Export ไฟล์นำส่ง บริษัทประกัน", "ClaimPayBack", "ClaimPayBackBillingRequestGroupExport", " ", "", "Developer,SmileClaimPayBack_Finance"));

                // update 2024-05-17 bunchuai
                result.Add(new Menu(13, 0, 0, "อนุมัติตอบกลับบริษัทประกัน", "ClaimPayBack", "Out2ApproveMonitor", " ", "", "SmileClaimPayBack_Finance,Developer"));

                result.Add(new Menu(9, 0, 0, "Dashboard ผลตอบกลับ", "", "", " ", "", "Developer,SmileClaimPayBack_Finance"));
                result.Add(new Menu(10, 9, 0, "ผลอนุมัติเคลม (จาก บริษัทประกัน)", "ClaimPayBack", "DashboardClaimApproveResult", " ", "", "Developer,SmileClaimPayBack_Finance"));
                result.Add(new Menu(10, 9, 0, "ยอดคงค้าง บริษัทประกัน", "ClaimPayBack", "DashboardOutStandingBalance", " ", "", "Developer,SmileClaimPayBack_Finance"));
                result.Add(new Menu(10, 9, 0, "เคลมปฏิเสธ", "ClaimPayBack", "DashboardRejectClaim", " ", "", "Developer,SmileClaimPayBack_Finance"));

                // import excel data by bunchuai 2024/02/28
                result.Add(new Menu(11, 0, 0, "ตัดรับชำระ COL", "", "", " ", "", "SmileClaimPayBack_Finance,Developer"));
                result.Add(new Menu(12, 11, 0, "ตัดรับชำระสินไหม OnLine", "ClaimPayBack", "NonPerformingloanMonitor", " ", "", "SmileClaimPayBack_Finance,Developer"));
            }

            //manager
            //version 2
            //result.Add(new Menu(22, 0, 0, "บันทึกการโอนเงิน", "ClaimOnLine", "ClaimOnLineMonitoring", "fa  fa-edit ", "", "SmileSClaimOnLine_Manager,Developer,SmileSClaimOnLine_Supervisor"));
            //result.Add(new Menu(2, 0, 0, "Monitor และบันทึกโอนเงิน", "ClaimOnLine", "MonitorClaimOnLine", "fa fa-check-square-o", "", "SmileSClaimOnLine_Manager,Developer,SmileSClaimOnLine_Supervisor"));
            //result.Add(new Menu(3, 0, 0, "บันทึกโอนเพิ่ม", "Premium", "TransferPremium", "fa fa-check-square-o", "", "SmileSClaimOnLine_Manager,Developer,SmileSClaimOnLine_Supervisor"));
            //result.Add(new Menu(4, 0, 0, "บันทึกคืนเงิน", "Premium", "ReturnPremium", "fa fa-check-square-o", "", "SmileSClaimOnLine_Manager,Developer,SmileSClaimOnLine_Supervisor"));
            //result.Add(new Menu(5, 0, 0, "ยกเลิก ClaimOnLine", "ClaimOnLine", "CancelClaimOnLine", "fa  fa-edit ", "", "SmileSClaimOnLine_Manager,Developer,SmileSClaimOnLine_Supervisor"));
            //result.Add(new Menu(6, 0, 0, "บันทึกรับเงิน", "Premium", "FundTransfer", "fa  fa-edit ", "", "SmileSClaimOnLine_Manager,Developer"));
            //result.Add(new Menu(7, 0, 0, "แก้ไข ClaimOnLine", "ClaimOnLine", "MonitorForEditClaimOnLine", "fa  fa-edit ", "", "SmileSClaimOnLine_Manager,Developer,SmileSClaimOnLine_Supervisor"));
            //result.Add(new Menu(8, 0, 0, "บันทึกปรับยอดบัญชี", "Premium", "ReceivePremium", "fa  fa-edit ", "", "SmileSClaimOnLine_Manager,Developer"));
            //result.Add(new Menu(9, 0, 0, "แก้ไขโอนเงิน", "ClaimOnLine", "TransferEndorse", "fa  fa-edit ", "", "SmileSClaimOnLine_Manager,Developer"));
            //result.Add(new Menu(9, 0, 0, "ยกเลิกรับเงิน", "ClaimOnLine", "CancelClaimOnLineTransferItem", "fa  fa-edit ", "", "SmileSClaimOnLine_Manager,Developer"));
            //result.Add(new Menu(15, 0, 0, "แก้ไข ClaimOnLineCode", "ClaimOnLine", "COLEndorse", "fa  fa-edit ", "", "Developer,SmileSClaimOnLine_Manager"));
            //result.Add(new Menu(16, 0, 0, "COL Query", "Manage", "ManageClaimOnline", "fa  fa-edit ", "", "Developer"));


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