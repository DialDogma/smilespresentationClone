using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmileSPettyCash.Helper;

namespace SmileSPettyCash.Models
{
    public class MenuRoles
    {
        private static List<Menu> GetAllMenu(int branchId)
        {
            var result = new List<Menu>();
            //get total row of Edit No Pass Payment Manage
            string totalRow = "0";
            try
            {
                using(var dbCon = new PettyCashEntities())
                {
                    int pettyCashId = dbCon.usp_PettyCash_Select(branchId,2).SingleOrDefault().PettyCashId;

                    var c = dbCon.usp_PettyCashTransaction_Select(pettyCashId,5,null,null,null,null,null).FirstOrDefault();
                    if(c != null)
                    {
                        totalRow = dbCon.usp_PettyCashTransaction_Select(pettyCashId,5,null,null,null,null,null).FirstOrDefault().TotalCount.ToString();
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            //insert all menu here
            //operation
            result.Add(new Menu(1,0,0," เงินสดย่อย","","","fa fa-wallet ","","PettyCash_MO,Developer"));
            result.Add(new Menu(2,1,0,"รายการรับ-จ่ายเงินสด","PettyCash","ManagePettyCashTransaction","fa fa-money","","PettyCash_MO,Developer"));
            result.Add(new Menu(3,1,0,"แก้ไขรายการไม่ผ่านปิดยอด:(" + totalRow + ")","PettyCash","EditNoPassPaymentManage","fa fa-check-square-o","","PettyCash_MO,Developer"));
            result.Add(new Menu(4,1,0,"สรุปยืนยันปิดยอด","PettyCash","CloseDayTransaction","fa fa-check-square-o","","PettyCash_MO,Developer"));
            //checker
            result.Add(new Menu(5,0,0," บัญชี 1 ตรวจ","","","fa  fa-edit","","PettyCash_ACC_01,Developer"));
            result.Add(new Menu(6,5,0,"ตรวจสอบยอดปิด","FinanceChecker","CheckTransactionManage","fa fa-edit ","","PettyCash_ACC_01,Developer"));
            //operation
            result.Add(new Menu(7,0,0," สรุปรายการตั้งเบิก","","","fa fa-file-invoice-dollar","","PettyCash_MO,Developer"));
            result.Add(new Menu(8,7,0,"รายการตั้งเบิกแยกตามบริษัท","PettyCash","Withdrawal","fa fa-file-invoice-dollar","","PettyCash_MO,Developer"));
            result.Add(new Menu(9,7,0,"รายการที่ไม่ผ่านการอนุมัติจากบัญชี 2","PettyCash","WithdrawalNoPass","fa fa-file-invoice-dollar","","PettyCash_MO,Developer"));
            result.Add(new Menu(13,7,0,"รายการผ่านการอนุมัติจากบัญชี 2","PettyCash","WaitScanWithdraw","fa fa-file-invoice-dollar","","PettyCash_MO,Developer"));

            //checker 2
            result.Add(new Menu(10,0,0," บัญชี 2 ตรวจ","","","fa fa-sticky-note","","PettyCash_ACC_02,Developer"));
            result.Add(new Menu(11,10,0," ตรวจสอบตั้งเบิก","FinanceChecker","CheckWithdrawalManage","fa fa-sticky-note","","PettyCash_ACC_02,Developer"));
            result.Add(new Menu(12,10,0," แจ้งโอนเงิน","FinanceChecker","TransferAlert","fa fa-file-invoice-dollar","","PettyCash_ACC_02,Developer"));
            result.Add(new Menu(13,10,0,"ตั้งค่ายอดเงินขั้นต่ำ","FinanceChecker","EditWithdrawalAmount","","","PettyCash_ACC_02,Developer"));

            //report
            result.Add(new Menu(14,0,0," รายงานการโอนเงิน","Report","TransactionReport","fa fa-file","","*"));
            result.Add(new Menu(15,0,0," รายงานการปิดยอด","Report","CloseDayReport","fa fa-file","","*"));
            result.Add(new Menu(15,0,0," รายงานรายการเบิก-จ่าย","Report","PettycashTransactionReport","fa fa-file","","*"));
            return result;
        }

        /// <summary>
        /// Get Menu By username
        /// </summary>
        /// <param name="userName">username</param>
        /// <returns></returns>
        public static List<Menu> GetMenuByUserName(string userName,int branchId)
        {
            var roles = new SSOService.SSOServiceClient().GetRoleByUserName(userName);

            var result = new List<Menu>();

            var lstRoles = roles.Split(',').ToList();

            foreach(var item in GetAllMenu(branchId))
            {
                var intersecCount = lstRoles.Intersect(item.AllowRoles).Count();
                if(intersecCount != 0 || item.AllowRoles.Contains("*"))
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
        public static List<Menu> GetMenuByRole(string role,int branchId)
        {
            var result = new List<Menu>();

            foreach(var item in GetAllMenu(branchId))
            {
                if(item.AllowRoles.Contains(role) || item.AllowRoles.Contains("*"))
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
            foreach(var mnu in GetAllMenu(branchId))
            {
                foreach(var role in mnu.AllowRoles)
                {
                    if(!roles.Contains(role))
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
        public Menu(int menuId,int mainMenuId,int sortOrder,string detail,string controller,string action,string iconClassMenu,string queryString,string allowRoleNames)
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