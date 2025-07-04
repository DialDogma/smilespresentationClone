using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using SmileSCommission.Models;
using SmileSCommission.Helper;
using SmileSCommission.Properties;
using Newtonsoft.Json;

namespace SmileSCommission.Controllers
{
    public class ModelDesignController : Controller
    {
        private CommissionCalculateEntities _context;

        public ModelDesignController()
        {
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #region Index

        public ActionResult Index()
        {
            //ViewBag.periodList = periodList;

            return View();
        }

        public JsonResult GetDatatable(int draw, int pageSize, int pageStart, string sortField, string orderType, string search)
        {
            var list = _context.usp_DesignModel_Datatable(search, pageStart, pageSize, sortField, orderType).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteDesigner(int desingerId)
        {
            var result = false;
            var errorText = "";
            try
            {
                _context.usp_DesignerModel_Delete(desingerId);
                result = true;
            }
            catch (Exception ex)
            {
                errorText = ex.Message;
            }
            return Json(new { success = result, error = errorText }, JsonRequestBehavior.AllowGet);
        }

        #endregion Index

        #region NewModel

        public ActionResult NewModel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewModel(FormCollection form)
        {
            //Save New Model
            var modelType = form["ModelType"].ToString().Trim();
            var modelName = form["ModelName"].ToString().Trim();
            var objModel = new DesignerModel()
            {
                ConfigurationCount = 0,
                DatasourceCount = 0,
                ParameterCount = 0,
                CreatedBy = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode,
                CreatedDate = DateTime.Now,
                Name = modelName,
                Type = modelType,
                UpdatedBy = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode,
                UpdatedDate = DateTime.Now
            };
            _context.DesignerModel.Add(objModel);
            _context.SaveChanges();

            //Redirect to next page with Id
            return RedirectToAction("DatasourceSelection", new { Id = objModel.Id.ToString(), act = "New" });
        }

        #endregion NewModel

        #region EditModel

        public ActionResult EditModel(int id)
        {
            ViewBag.DesignModel_Id = id.ToString();
            var objModel = _context.DesignerModel.FirstOrDefault(x => x.Id == id);
            ViewBag.Model = objModel;
            return View();
        }

        [HttpPost]
        public ActionResult EditModel(FormCollection form)
        {
            //Get Id
            var designModel_Id = Convert.ToInt32(form["DesignModel_Id"]);
            var modelType = form["ModelType"].ToString().Trim();
            var modelName = form["ModelName"].ToString().Trim();
            var disignnModel = _context.DesignerModel.FirstOrDefault(x => x.Id == designModel_Id);
            disignnModel.Type = modelType;
            disignnModel.Name = modelName;
            _context.SaveChanges();
            return RedirectToAction("DatasourceSelection", new { Id = designModel_Id.ToString(), act = "Edit" });
        }

        #endregion EditModel

        #region Datasource Selection

        public ActionResult DatasourceSelection(int id, string act)
        {
            var lst = _context.usp_DesignModel_Datasource_Get(id).ToList();
            ViewBag.Act = act;
            ViewBag.SourceList = lst;
            ViewBag.DesignModel_Id = id.ToString();
            return View();
        }

        [HttpPost]
        public ActionResult DatasourceSelection(FormCollection form)
        {
            //Get Id
            var designModel_Id = Convert.ToInt32(form["DesignModel_Id"]);

            //keep mainkey if exsist
            var mainKeyDatasource = _context.DesignerModelDatasource.SingleOrDefault(x => x.DesignerModel_Id == designModel_Id && x.IsMainKey == true);

            //Clear Datasource
            _context.usp_DesignModel_ClearDatasource(designModel_Id);

            var selected = form["SelectedDatasouce"].ToString();
            var selectedList = selected.Split(',').ToList();

            foreach (var item in selectedList)
            {
                if (mainKeyDatasource == null)
                {
                    //Add Datasource
                    _context.DesignerModelDatasource.Add(new DesignerModelDatasource() { DatasourceName = item, DesignerModel_Id = designModel_Id, IsMainKey = false });
                    _context.SaveChanges();
                }
                else
                {
                    //Check ว่าเป็น mainkey เดิมหรือไม่
                    var isMainKey = (mainKeyDatasource.DatasourceName == item) ? true : false;
                    //Add Datasource
                    _context.DesignerModelDatasource.Add(new DesignerModelDatasource() { DatasourceName = item, DesignerModel_Id = designModel_Id, IsMainKey = isMainKey });
                    _context.SaveChanges();
                }
            }

            //Add Config sys_ModelName
            var modelName = _context.DesignerModel.FirstOrDefault(x => x.Id == designModel_Id).Name;
            var objModelNameConfig = _context.DesignerModelConfig.FirstOrDefault(x => x.DesignerModel_Id == designModel_Id &&
                                                                        x.ConfigName == Settings.Default.Config_ModelName);

            if (objModelNameConfig == null)
            {
                //ยังไม่มี config
                objModelNameConfig = new DesignerModelConfig()
                {
                    ConfigIndex = 1,
                    DesignerModel_Id = designModel_Id,
                    ConfigName = Properties.Settings.Default.Config_ModelName,
                    ConfigValue = modelName,
                    ConfigFrom = "",
                    ConfigTo = "",
                    ConfigDescription = ""
                };

                //Add config modelname
                _context.DesignerModelConfig.Add(objModelNameConfig);

                //Add config Mainkey Sheet
                _context.DesignerModelConfig.Add(new DesignerModelConfig()
                {
                    ConfigIndex = 1,
                    DesignerModel_Id = designModel_Id,
                    ConfigName = Properties.Settings.Default.Config_MainKeySheet,
                    ConfigValue = _context.DesignerModelDatasource.FirstOrDefault(x => x.DesignerModel_Id == designModel_Id).DatasourceName,
                    ConfigFrom = "",
                    ConfigTo = "",
                    ConfigDescription = ""
                });
            }
            else
            {
                objModelNameConfig.ConfigValue = modelName;
            }
            _context.SaveChanges();

            //Recalculate
            _context.usp_DesignModel_ReCalculate(designModel_Id);

            return RedirectToAction("ModelConfiguration", new { Id = designModel_Id.ToString(), act = form["Act"] });
        }

        #endregion Datasource Selection

        #region Model Configuration

        public ActionResult ModelConfiguration(int Id, string act)
        {
            //Get Datasource
            var datasourceList = _context.DesignerModelDatasource.Where(x => x.DesignerModel_Id == Id).ToList();

            ViewBag.DatasourceList = datasourceList;
            ViewBag.DesignModel_Id = Id.ToString();
            ViewBag.Act = act;
            return View();
        }

        public JsonResult GetConfigTable(int draw, int pageSize, int pageStart, string sortField, string orderType, string search, int desingerModel_Id)
        {
            var list = _context.usp_DesignModel_Configuration_Datatable(search, pageStart, pageSize, sortField, orderType, desingerModel_Id).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ModelConfiguration(FormCollection form)
        {
            //Get Id
            var designModel_Id = Convert.ToInt32(form["DesignModel_Id"]);
            var mainKeyDatasource = form["MainkeyDatasource"].ToString();
            var objmodelDatasource = _context.DesignerModelDatasource.FirstOrDefault(x => x.DesignerModel_Id == designModel_Id && x.DatasourceName == mainKeyDatasource);

            //Update Mainkey Datasource
            objmodelDatasource.IsMainKey = true;

            //Create or update Mainkey Parameter
            var mainKeyFormula = _context.usp_DesignModel_GetMainKey(objmodelDatasource.DatasourceName).FirstOrDefault();
            var objMainKeyParameter = _context.DesignerParameter.FirstOrDefault(x => x.DesignerModel_Id == designModel_Id && x.ParameterIndex == 1);
            if (objMainKeyParameter == null)
            {
                objMainKeyParameter = new DesignerParameter()
                {
                    ParameterIndex = 1,
                    DesignerModel_Id = designModel_Id,
                    ParameterName = "MainKey",
                    ParameterFormula = mainKeyFormula,
                    ParameterDataType = 1
                };

                _context.DesignerParameter.Add(objMainKeyParameter);
            }
            else
            {
                objMainKeyParameter.ParameterFormula = mainKeyFormula;
            }
            _context.SaveChanges();

            return RedirectToAction("ModelParameter", new { Id = designModel_Id.ToString(), act = form["Act"] });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetModelConfigurationDetail(int Id)
        {
            var result = _context.DesignerModelConfig.Where(x => x.Id == Id)
                        .Select(x => new { x.ConfigDescription, x.ConfigFrom, x.ConfigIndex, x.ConfigName, x.ConfigTo, x.ConfigValue, x.DesignerModel_Id, x.Id })
                        .FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ValidateForAddConfig(int designModelId, string name)
        {
            var result = true;

            //Validate name not exsist in database
            var count = _context.DesignerModelConfig.Count(x => x.DesignerModel_Id == designModelId && x.ConfigName == name.Trim());
            if (count == 1)
            {
                //already exsists in the database
                result = false;
            }

            return Json(new { validate = result, error = name + " already exsist in the database. please enter new valid name" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ValidateForEditConfig(int designModelId, int ConfigId, string newName)
        {
            //Validate name not exsist in database
            var result = true;
            var count = _context.DesignerModelConfig.Count(x => x.DesignerModel_Id == designModelId &&
                                                            x.ConfigName == newName &&
                                                            x.Id != ConfigId);

            if (count == 1)
            {
                result = false;
            }

            return Json(new { validate = result, error = newName + " already exsist in the database. please enter new valid name" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddModelConfig(DesignerModelConfig cf)
        {
            var result = true;
            var errorText = "";
            cf.ConfigIndex = 1;

            try
            {
                var lastModelConfigIndex = _context.DesignerModelConfig.Where(x => x.DesignerModel_Id == cf.DesignerModel_Id).OrderByDescending(x => x.ConfigIndex).FirstOrDefault();
                if (lastModelConfigIndex != null)
                {
                    cf.ConfigIndex = lastModelConfigIndex.ConfigIndex.Value + 1;
                }
                _context.DesignerModelConfig.Add(cf);
                _context.SaveChanges();

                _context.usp_DesignModel_ReCalculate(cf.DesignerModel_Id);
            }
            catch (Exception ex)
            {
                errorText = ex.Message;
                result = false;
            }

            return Json(new { success = result, obj = cf, error = errorText }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditModelConfig(DesignerModelConfig cf)
        {
            var result = true;
            var errorText = "";

            try
            {
                var objConfig = _context.DesignerModelConfig.Where(x => x.Id == cf.Id).FirstOrDefault();
                objConfig.ConfigName = cf.ConfigName;
                objConfig.ConfigFrom = cf.ConfigFrom;
                objConfig.ConfigTo = cf.ConfigTo;
                objConfig.ConfigValue = cf.ConfigValue;
                objConfig.ConfigDescription = cf.ConfigDescription;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                errorText = ex.Message;
                result = false;
            }

            return Json(new { success = result, obj = cf, error = errorText }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetModelConfigurationList(int designModelId)
        {
            var result = _context.DesignerModelConfig.Where(x => x.DesignerModel_Id == designModelId &&
                                                             x.ConfigName != Properties.Settings.Default.Config_ModelName &&
                                                             x.ConfigName != Properties.Settings.Default.Config_MainKeySheet
                                                            ).OrderBy(x => x.ConfigIndex)
                                                            .Select(x => new { x.ConfigName, x.Id, x.ConfigValue }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet); ;
        }

        [HttpPost]
        public ActionResult UpdateModelConfigIndex(List<String> values)
        {
            var result = true;
            var errorText = "";

            try
            {
                //re index model config
                for (int i = 0; i < (values.Count); i++)
                {
                    var id = Convert.ToInt32(values[i]);
                    var objModelConfig = _context.DesignerModelConfig.Where(x => x.Id == id).FirstOrDefault();
                    objModelConfig.ConfigIndex = i + 3; // (เริ่มจาก 1) ข้าม ModelName และ Mainkey Sheet
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                result = false;
                errorText = ex.Message;
            }

            return Json(new { success = result, errorText });
        }

        [HttpPost]
        public ActionResult UpdateModelConfigMainKeyDatasource(int designerModelId, string mainKeyDatasouceName)
        {
            var result = true;
            var errorText = "";
            try
            {
                //Update Mainkey Datasource
                //Remove previous mainkey datasource
                var objPreviuosMainkey = _context.DesignerModelDatasource.Where(x => x.DesignerModel_Id == designerModelId && x.IsMainKey == true).FirstOrDefault();
                if (objPreviuosMainkey != null)
                {
                    objPreviuosMainkey.IsMainKey = false;
                    _context.SaveChanges();
                }

                //Set new main key
                var objmodelDatasource = _context.DesignerModelDatasource.Where(x => x.DesignerModel_Id == designerModelId && x.DatasourceName == mainKeyDatasouceName).FirstOrDefault();
                objmodelDatasource.IsMainKey = true;
                _context.SaveChanges();
                _context.usp_DesignModel_ReCalculate(designerModelId);

                //Update config datasource
                var objConfig = _context.DesignerModelConfig.Where(x => x.DesignerModel_Id == designerModelId
                                                                 && x.ConfigName == Properties.Settings.Default.Config_MainKeySheet).FirstOrDefault();
                objConfig.ConfigValue = mainKeyDatasouceName;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
                errorText = ex.Message;
            }
            return Json(new { success = result, error = errorText }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteModelConfig(int designerModelConfigId)
        {
            var result = true;
            var errorText = "";

            try
            {
                // get object
                var objDesignerModelConfig = _context.DesignerModelConfig.Where(x => x.Id == designerModelConfigId).Single();

                //Re Index for higher items (make it index = index-1)
                List<DesignerModelConfig> lstHigherIndex = _context.DesignerModelConfig.Where(x => x.DesignerModel_Id == objDesignerModelConfig.DesignerModel_Id
                                                                         && x.ConfigIndex > objDesignerModelConfig.ConfigIndex).ToList();

                for (int i = 0; i < lstHigherIndex.Count; i++)
                {
                    lstHigherIndex[i].ConfigIndex -= 1;
                }

                //Delete DesignerModelConfig
                _context.DesignerModelConfig.Remove(objDesignerModelConfig);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                errorText = ex.Message;
                result = false;
                throw;
            }
            return Json(new { success = result, error = errorText }, JsonRequestBehavior.AllowGet);
        }

        #endregion Model Configuration

        #region Model Parameter

        public ActionResult DeleteModelParameter(int designerModelParameterId)
        {
            var result = true;
            var errorText = "";

            try
            {
                // get object
                var objDesignerModelParameter = _context.DesignerParameter.Single(x => x.Id == designerModelParameterId);

                //Re Index for higher items (make it index = index-1)
                List<DesignerParameter> lstHigherIndex = _context.DesignerParameter.Where(x => x.DesignerModel_Id == objDesignerModelParameter.DesignerModel_Id
                                                                         && x.ParameterIndex > objDesignerModelParameter.ParameterIndex).ToList();

                for (int i = 0; i < lstHigherIndex.Count; i++)
                {
                    lstHigherIndex[i].ParameterIndex -= 1;
                }

                //Delete DesignerModelConfig
                _context.DesignerParameter.Remove(objDesignerModelParameter);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                errorText = ex.Message;
                result = false;
                throw;
            }
            return Json(new { success = result, error = errorText }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ModelParameter(int Id, string act)
        {
            var modelName = _context.DesignerModel.Where(x => x.Id == Id).Select(x => x.Name).FirstOrDefault();

            ViewBag.modelName = modelName;

            ViewBag.DesignModel_Id = Id.ToString();
            ViewBag.Act = act;
            //TODO: Manage Parameter
            return View();
        }

        public JsonResult GetParameterTable(int draw, int pageSize, int pageStart, string sortField, string orderType, string search, int desingerModel_Id)
        {
            var list = _context.usp_DesignModel_Parameter_Datatable(search, pageStart, pageSize, sortField, orderType, desingerModel_Id).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddModelParameter()
        {
            var result = true;
            var errorText = "";
            Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();

            DesignerParameter param = JsonConvert.DeserializeObject<DesignerParameter>(json);

            param.ParameterIndex = 1;

            try
            {
                var lastParameterIndex = _context.DesignerParameter.Where(x => x.DesignerModel_Id == param.DesignerModel_Id).OrderByDescending(x => x.ParameterIndex).FirstOrDefault();
                if (lastParameterIndex != null)
                {
                    param.ParameterIndex = lastParameterIndex.ParameterIndex.Value + 1;
                    param.ParameterDataType = param.ParameterDataType;
                }
                _context.DesignerParameter.Add(param);
                _context.SaveChanges();
                _context.usp_DesignModel_ReCalculate(param.DesignerModel_Id);
            }
            catch (Exception ex)
            {
                errorText = ex.Message;
                result = false;
            }

            return Json(new { success = result, obj = param, error = errorText }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditModelParameter()
        {
            var result = true;
            var errorText = "";
            Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();

            DesignerParameter param = JsonConvert.DeserializeObject<DesignerParameter>(json);

            try
            {
                var objParam = _context.DesignerParameter.FirstOrDefault(x => x.Id == param.Id);
                objParam.ParameterName = param.ParameterName;
                objParam.ParameterFormula = param.ParameterFormula;
                objParam.ParameterDataType = param.ParameterDataType;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                errorText = ex.Message;
                result = false;
            }

            return Json(new { success = result, obj = param, error = errorText }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetModelParameterList(int designModelId)
        {
            var result = _context.DesignerParameter.Where(x => x.DesignerModel_Id == designModelId && x.ParameterIndex != 1)
                                                    .OrderBy(x => x.ParameterIndex)
                                                    .Select(x => new { x.Id, x.ParameterDataType, x.ParameterFormula, x.ParameterIndex, x.ParameterName, x.DesignerModel_Id }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateModelParameterIndex(List<String> values)
        {
            var result = true;
            var errorText = "";

            try
            {
                //re index model config
                for (int i = 0; i < (values.Count); i++)
                {
                    var id = Convert.ToInt32(values[i]);
                    var objDesignerParameter = _context.DesignerParameter.FirstOrDefault(x => x.Id == id);
                    objDesignerParameter.ParameterIndex = i + 2; //(เริ่มจาก 1 ) ข้าม Main Key
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                result = false;
                errorText = ex.Message;
            }

            return Json(new { success = result, errorText });
        }

        [HttpGet]
        public ActionResult ValidateForAddParameter(int designModelId, string name)
        {
            var result = true;
            //Validate name not exsist in database
            var count = _context.DesignerParameter.Count(x => x.DesignerModel_Id == designModelId && x.ParameterName == name.Trim());
            if (count == 1)
            {
                //already exsists in the database
                result = false;
            }

            return Json(new { validate = result, error = name + " already exsist in the database. please enter new valid name" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ValidateForEditParameter(int designModelId, int ConfigId, string newName)
        {
            //Validate name not exsist in database
            var result = true;
            var count = _context.DesignerParameter.Count(x => x.DesignerModel_Id == designModelId &&
                                                            x.ParameterName == newName &&
                                                            x.Id != ConfigId);

            if (count == 1)
            {
                result = false;
            }

            return Json(new { validate = result, error = newName + " already exsist in the database. please enter new valid name" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetModelParameterDetail(int Id)
        {
            var result = _context.DesignerParameter.Where(x => x.Id == Id).Select(x => new { x.Id, x.DesignerModel_Id, x.ParameterDataType, x.ParameterFormula, x.ParameterIndex, x.ParameterName }).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddUpdateDesignerTable(int designModelId)
        {
            try
            {
                int periodId = _context.CommissionPeriod.Where(x => x.CommissionPeriodStatusId == 2).OrderByDescending(x => x.Id).FirstOrDefault().Id;

                var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
                var result = _context.usp_ApplyDesignerToCalculationIndex(designModelId, periodId, user);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Model Parameter

        #region Download

        public ActionResult DownloadDesignModel(int id)
        {
            //Create File
            var x = new ModelDesignerFromDatabase(id);
            x.CreateExcelModelDesigner();

            //Download File
            var rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            var url = rootUrl + "Content/CreatedExcelFiles/" + x.DesignerModel_Name + ".xlsx";

            return Redirect(url);
        }

        #endregion Download
    }
}