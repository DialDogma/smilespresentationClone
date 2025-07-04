using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using OfficeOpenXml;
using SmileSDataCenterV2.Helper;
using SmileSDataCenterV2.SSSLoginService;
using SmileSDataCenterV2.WebServices;

namespace SmileSDataCenterV2.Controllers
{
    //[Authorization(Roles = "Developer")]
    public class UploadUserFileController:Controller
    {
        public class ChangePasswordUser
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        public class ChangePasswordUserInvalid:ChangePasswordUser
        {
            public List<string> InvalidList { get; set; }
        }

        public ActionResult BatchPasswordChange()
        {
            ViewBag.Valid = new List<ChangePasswordUser>();
            ViewBag.ValidJson = "[]";
            ViewBag.Invalid = new List<ChangePasswordUserInvalid>();
            ViewBag.InvalidJson = "[]";

            return View();
        }

        public ActionResult Upload(HttpPostedFileBase file)
        {
            var helpers = new Helper.ValidationHelper();

            var valid = new List<ChangePasswordUser>();
            var inValid = new List<ChangePasswordUserInvalid>();

            if(file != null)
            {
                Stream fileContent = file.InputStream;

                using(ExcelPackage excelPackage = new ExcelPackage(fileContent))
                {
                    // Select first worksheet
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];

                    var rows = ((object[,])worksheet.Cells.Value);

                    var columns = new List<string>
                    {
                        "UserName",
                        "Password"
                    };

                    var missingColumns = helpers.ValidateColumn(rows,columns);

                    if(missingColumns.Count > 0)
                    {
                        ViewBag.isInvalidColumn = true;

                        ViewBag.isInvalidMsg = "ไม่พบ column : " + String.Join(", ",missingColumns);

                        ViewBag.Valid = new List<ChangePasswordUser>();
                        ViewBag.ValidJson = "[]";
                        ViewBag.Invalid = new List<ChangePasswordUserInvalid>();
                        ViewBag.InvalidJson = "[]";

                        return View("BatchPasswordChange");
                    }

                    for(int i = worksheet.Dimension.Start.Row;i <= worksheet.Dimension.End.Row - 1;i++)
                    {
                        var rowResult = new ChangePasswordUser();
                        var invalidList = new List<string>();

                        var userName = helpers.ValidateString("UserName",rows[i,0],true);
                        if(userName.IsValid) rowResult.UserName = userName.Value;
                        if(!userName.IsValid) invalidList.Add(userName.InvalidMessage);

                        var password = helpers.ValidatePassword("Password",rows[i,1],true);
                        if(password.IsValid) rowResult.Password = password.Value;
                        if(!password.IsValid) invalidList.Add(password.InvalidMessage);

                        if(invalidList.Count == 0)
                        {
                            valid.Add(rowResult);
                        }
                        else
                        {
                            var config = new MapperConfiguration(cfg =>
                            {
                                cfg.CreateMap<ChangePasswordUser,ChangePasswordUserInvalid>();
                            });
                            IMapper mapper = config.CreateMapper();
                            var dest = mapper.Map<ChangePasswordUser,ChangePasswordUserInvalid>(rowResult);
                            dest.InvalidList = invalidList;

                            inValid.Add(dest);
                        }
                    }
                }
            }

            if(inValid.Count == 0)
            {
                var tmpValid = valid.ToList();

                #region Check Duplicate

                //check duplicate in file
                var getduplicateInFile = valid.GroupBy(x => x.UserName.Trim()).Where(x => x.Count() > 1).Select(x => x.Key).ToList();

                foreach(var item in valid.Where(x => getduplicateInFile.Any(y => y.Equals(x.UserName.Trim()))).ToList())
                {
                    valid.Remove(item);

                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<ChangePasswordUser,ChangePasswordUserInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<ChangePasswordUser,ChangePasswordUserInvalid>(item);
                    dest.InvalidList = new List<string>() { "Duplicate column Username in upload file" };

                    inValid.Add(dest);
                }

                #endregion Check Duplicate

                #region Valid UserName

                var notExistUsername = helpers.ValidateUser(tmpValid.Where(x => !string.IsNullOrEmpty(x.UserName)).Select(x => x.UserName.Trim()).ToList());

                //find in exist invalid
                foreach(var item in inValid.Where(x => notExistUsername.Any(y => y.Equals(x.UserName.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found UserName");
                }

                //find in valid
                foreach(var item in valid.Where(x => notExistUsername.Any(y => y.Equals(x.UserName.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<ChangePasswordUser,ChangePasswordUserInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<ChangePasswordUser,ChangePasswordUserInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found UserName" };

                    // Add item to invalid list
                    inValid.Add(dest);
                }

                #endregion Valid UserName
            }
            // Set data for pageview
            ViewBag.isInvalidColumn = false;

            ViewBag.Valid = valid;
            ViewBag.ValidJson = JsonConvert.SerializeObject(valid);

            ViewBag.Invalid = inValid;
            ViewBag.InvalidJson = JsonConvert.SerializeObject(inValid);

            // Generate key for mapping data
            var dataKey = Guid.NewGuid().ToString();

            ViewBag.DataKey = dataKey;

            // Save valid data to session
            Session[dataKey] = valid;

            // Session will expire in 30 minutes
            Session.Timeout = 30;

            return View("BatchPasswordChange");
        }

        [HttpPost]
        public JsonResult SaveUpload(string dataKey)
        {
            var valid = (List<ChangePasswordUser>)Session[dataKey];

            try
            {
                foreach(var item in valid)
                {
                    using(var _service = new SSSLoginServiceSoapClient())
                    {
                        _service.ChangePasswordSmileS(item.UserName,item.Password);
                        _service.ChangePasswordSSS(item.UserName,item.Password);
                    }
                }
                Session[dataKey] = null;

                return Json("บันทึกข้อมูลเรียบร้อย");
            }
            catch(Exception e)
            {
                return Json("error:" + e.Message);
            }
        }
    }
}