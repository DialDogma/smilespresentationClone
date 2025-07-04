using SmileSLogin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SmileSLogin.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISSOService" in both code and config file together.
    [ServiceContract]
    public interface ISSOService
    {
        [OperationContract]
        string GetRoleByUserName(string username);

        [OperationContract]
        bool ValidateKey(string key);

        [OperationContract]
        void LogOut(string key);

        [OperationContract]
        List<MenuListByRoles> GetMenuList(string username, string projectName);

        [OperationContract]
        int DefaultExpireTokenDays();
    }
}