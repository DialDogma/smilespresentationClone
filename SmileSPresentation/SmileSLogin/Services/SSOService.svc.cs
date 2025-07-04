using JWT.Builder;
using SmileSLogin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SmileSLogin.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SSOService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SSOService.svc or SSOService.svc.cs at the Solution Explorer and start debugging.
    public class SSOService : ISSOService
    {
        public string GetRoleByUserName(string username)
        {
            var db = new Models.DataCenterV1Entities();
            var result = string.Join(",", db.usp_GetUserRoles(username).Select(x => x.Name).ToList());
            db.Dispose();
            return result;
        }

        /// <summary>
        /// Get menu list
        /// </summary>
        /// <param name="username"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public List<MenuListByRoles> GetMenuList(string username, string projectName)
        {
            var db = new DataCenterV1Entities();

            //Get roles
            var roles = string.Join(",", db.usp_GetUserRoles(username).Select(x => x.Name).ToList());
            //get result

            var result = db.usp_GetMenuByRoles(roles, projectName).ToList();

            db.Dispose();
            //return result
            return result;
        }

        /// <summary>
        /// Validate key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        //public bool ValidateKey(string key)
        //{
        //    //  TODO: เพื่อ tuning เปลี่ยนจาก ส่ง key เป็นส่ง UserName
        //    //  เนื่องจากตัว key ไม่สามารถทำ index ได้ (column size ใหญ่เกินไป)
        //    //  (ถอด username ออกมาจาก key)
        //    //  (ระหว่างถอด username ถ้า expire จะโดน Exception จาก JWT อยู่แล้ว)
        //    //  แล้วรับ list กลับมาเช็คว่ามี key ที่ตรงกันหรือเปล่า
        //    var db = new Models.DataCenterV1Entities();

        //    var t = db.usp_ValidateToken(key).FirstOrDefault();

        //    db.Dispose();

        //    return t != null ? true : false;
        //}

        public bool ValidateKey(string key)
        {
            var result = false;
            try
            {
                //get username by key
                var username = GetUserNameByKey(key);

                var db = new Models.DataCenterV1Entities();

                var lst = db.usp_ValidateToken_V2(username).ToList();

                //check if exsist
                result = lst.Where(x => x.Detail == key).Any();

                db.Dispose();

            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        public int DefaultExpireTokenDays()
        {
            return Properties.Settings.Default.TokenExpireDays;
        }

        public void LogOut(string key)
        {
            var db = new Models.DataCenterV1Entities();

            var t = db.usp_DeleteToken(key);

            db.Dispose();
        }

        private string GetUserNameByKey(string key)
        {
            var secretKey = Properties.Settings.Default.SecretKey;

            var result = "";

            //Decode token
            try
            {
                var payload = new JwtBuilder()
                        .WithSecret(secretKey)
                        .MustVerifySignature()
                        .Decode<IDictionary<string, object>>(key);

                //Valid Token,Get username
                result = (payload["Username"]).ToString();
            }

            //Token Expire
            catch (JWT.TokenExpiredException)
            {
                
            }

            //Invalid Signature
            catch (JWT.SignatureVerificationException)
            {

            }

            //other
            catch (Exception ex)
            {

            }

            return result;
        }
    }

}
