using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSCommissionDataImport.Models
{
    public class MenuRoles
    {
        private static List<Menu> GetAllMenu()
        {
            var result = new List<Menu>();

            //insert all menu here
            //Commission Period Manager
            //result.Add(new Menu(2,0,0,"D_Area_Calculation_LastMonth","Area_Calculation_LastMonth","Index","","","Developer"));
            //result.Add(new Menu(3,0,0,"D_Area","Area","Index","","","Developer"));
            //result.Add(new Menu(4,0,0,"D_Branch","Branch","Index","","","Developer"));
            //result.Add(new Menu(4,0,0,"D_Branch_Calculation_Lastmonth","Branch_Calculation_Lastmonth","Index","","","Developer"));
            //result.Add(new Menu(5,0,0,"D_ClaimHouse","ClaimHouse","Index","","","Developer"));
            //result.Add(new Menu(6,0,0,"D_ClaimMotor","ClaimMotor","Index","","","Developer"));
            //result.Add(new Menu(7,0,0,"D_Employee","Employee","Index","","","Developer"));
            //result.Add(new Menu(8,0,0,"D_Employee_CB_PB","Employee_CB_PB","Index","","","Developer"));
            //result.Add(new Menu(9,0,0,"D_Employee_DA","Employee_DA","Index","","","Developer"));
            //result.Add(new Menu(10,0,0,"D_Employee_DB","Employee_DB","Index","","","Developer"));
            //result.Add(new Menu(11,0,0,"D_Employee_DM","Employee_DM","Index","","","Developer"));
            //result.Add(new Menu(12,0,0,"D_Employee_DS","Employee_DS","Index","","","Developer"));
            //result.Add(new Menu(13,0,0,"D_Employee_HCM","Employee_HCM","Index","","","Developer"));
            //result.Add(new Menu(14,0,0,"D_Employee_HO_AHO","Employee_HO_AHO","Index","","","Developer"));
            //result.Add(new Menu(15,0,0,"D_Employee_MM","Employee_MM","Index","","","Developer"));
            //result.Add(new Menu(16,0,0,"D_Employee_MMR","Employee_MMR","Index","","","Developer"));
            //result.Add(new Menu(17,0,0,"D_Employee_MS","Employee_MS","Index","","","Developer"));
            //result.Add(new Menu(17,0,0,"D_Employee_MSR","Employee_MSR","Index","","","Developer"));
            //result.Add(new Menu(18,0,0,"D_Employee_Zebra","Employee_Zebra","Index","","","Developer"));
            //result.Add(new Menu(19,0,0,"D_NewApp_Goft","NewApp_Goft","Index","","","Developer"));
            //result.Add(new Menu(20,0,0,"D_NewApp_PA200","NewApp_PA200","Index","","","Developer"));
            //result.Add(new Menu(21,0,0,"D_NewApp_SmilePA","NewApp_SmilePA","Index","","","Developer"));
            //result.Add(new Menu(22,0,0,"D_NewApp_TA_Domestic","NewApp_TA_Domestic","Index","","","Developer"));
            //result.Add(new Menu(23,0,0,"D_NewApp_TA_Inter","NewApp_TA_Inter","Index","","","Developer"));
            //result.Add(new Menu(24,0,0,"D_NewAppHQ","NewAppHQ","Index","","","Developer"));
            //result.Add(new Menu(25,0,0,"D_NewAppMQ","NewAppMQ","Index","","","Developer"));
            //result.Add(new Menu(26,0,0,"D_OperatingCapital","OperatingCapital","Index","","","Developer"));
            //result.Add(new Menu(27,0,0,"D_PA_Premium","PA_Premium","Index","","","Developer"));
            //result.Add(new Menu(28,0,0,"D_Personal_Calculation_LastMonth","Personal_Calculation_LastMonth","Index","","","Developer"));
            //result.Add(new Menu(29,0,0,"D_ReportBeforeCover","ReportBeforeCover","Index","","","Developer"));
            //result.Add(new Menu(30,0,0,"D_Team","Team","Index","","","Developer"));
            //result.Add(new Menu(31,0,0,"D_UnApprovedUnderwrite","UnApprovedUnderwrite","Index","","","Developer"));
            //result.Add(new Menu(32,0,0,"D_UnApprovedUnderwrite_PriorMonth","UnApprovedUnderwrite_PriorMonth","Index","","","Developer"));
            //result.Add(new Menu(33,0,0,"D_UnApprovedUnderwrite_PriorMonth","UnApprovedUnderwrite_PriorMonth","Index","","","Developer"));
            //result.Add(new Menu(34,0,0,"D_UnderwriteCondition","UnderwriteCondition","Index","","","Developer"));
            //result.Add(new Menu(35,0,0,"D_UnderwriteOverdue","UnderwriteOverdue","Index","","","Developer"));
            //result.Add(new Menu(36,0,0,"D_WeeklySaleTarget","WeeklySaleTarget","Index","","","Developer"));
            //result.Add(new Menu(37,0,0,"D_WeeklySaleTargetBranchPB_CB","WeeklySaleTargetBranchPB_CB","Index","","","Developer"));
            //result.Add(new Menu(38,0,0,"D_Zebra","Zebra","Index","","","Developer"));

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

            foreach(var item in GetAllMenu())
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
        public static List<Menu> GetMenuByRole(string role)
        {
            var result = new List<Menu>();

            foreach(var item in GetAllMenu())
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
        public static List<string> GetAllRoles()
        {
            var roles = new List<String>();
            foreach(var mnu in GetAllMenu())
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