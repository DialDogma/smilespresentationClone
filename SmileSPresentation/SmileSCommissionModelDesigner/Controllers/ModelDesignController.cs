using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSCommissionModelDesigner.Models;
using SmileSCommissionModelDesigner.Helper;

namespace SmileSCommissionModelDesigner.Controllers
{
    public class ModelDesignController:Controller
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
            return View();
        }

        public JsonResult GetDatatable(int draw,int pageSize,int pageStart,string sortField,string orderType,string search)
        {
            var list = _context.usp_DesignModel_Datatable(search,pageStart,pageSize,sortField,orderType).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
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
            catch(Exception ex)
            {
                errorText = ex.Message;
            }
            return Json(new { success = result,error = errorText },JsonRequestBehavior.AllowGet);
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
                CreatedBy = GlobalFunction.GetCurrentUserId(),
                CreatedDate = DateTime.Now,
                Name = modelName,
                Type = modelType,
                UpdatedBy = GlobalFunction.GetCurrentUserId(),
                UpdatedDate = DateTime.Now
            };
            _context.DesignerModels.Add(objModel);
            _context.SaveChanges();

            //Redirect to next page with Id
            return RedirectToAction("DatasourceSelection",new { Id = objModel.Id.ToString(),act = "New" });
        }

        #endregion NewModel

        #region EditModel

        public ActionResult EditModel(int id)
        {
            ViewBag.DesignModel_Id = id.ToString();
            var objModel = _context.DesignerModels.Where(x => x.Id == id).FirstOrDefault();
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
            var disignnModel = _context.DesignerModels.Where(x => x.Id == designModel_Id).FirstOrDefault();
            disignnModel.Type = modelType;
            disignnModel.Name = modelName;
            _context.SaveChanges();
            return RedirectToAction("DatasourceSelection",new { Id = designModel_Id.ToString(),act = "Edit" });
        }

        #endregion EditModel

        #region Datasource Selection

        public ActionResult DatasourceSelection(int id,string act)
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
            //Clear Datasource
            _context.usp_DesignModel_ClearDatasource(designModel_Id);

            var selected = form["SelectedDatasouce"].ToString();
            var selectedList = selected.Split(',').ToList();

            //keep mainkey if exsist
            var mainKeyDatasource = _context.DesignerModelDatasources.Where(x => x.DesignerModel_Id == designModel_Id && x.IsMainKey == true).SingleOrDefault();

            foreach(var item in selectedList)
            {
                if(mainKeyDatasource == null)
                {
                    //Add Datasource
                    _context.DesignerModelDatasources.Add(new DesignerModelDatasource() { DatasourceName = item,DesignerModel_Id = designModel_Id,IsMainKey = false });
                    _context.SaveChanges();
                }
                else
                {
                    //Add ถ้าไม่ใช่ mainkey
                    if(mainKeyDatasource.DatasourceName != item)
                    {
                        //Add Datasource
                        _context.DesignerModelDatasources.Add(new DesignerModelDatasource() { DatasourceName = item,DesignerModel_Id = designModel_Id,IsMainKey = false });
                        _context.SaveChanges();
                    }
                }
            }

            //Add Config sys_ModelName
            var modelName = _context.DesignerModels.Where(x => x.Id == designModel_Id).FirstOrDefault().Name;
            var objModelNameConfig = _context.DesignerModelConfigs.Where(x => x.DesignerModel_Id == designModel_Id &&
                                                                        x.ConfigName == Properties.Settings.Default.Config_ModelName).FirstOrDefault();

            if(objModelNameConfig == null)
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
                _context.DesignerModelConfigs.Add(objModelNameConfig);

                //Add config Mainkey Sheet
                _context.DesignerModelConfigs.Add(new DesignerModelConfig()
                {
                    ConfigIndex = 1,
                    DesignerModel_Id = designModel_Id,
                    ConfigName = Properties.Settings.Default.Config_MainKeySheet,
                    ConfigValue = _context.DesignerModelDatasources.Where(x => x.DesignerModel_Id == designModel_Id).FirstOrDefault().DatasourceName,
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

            return RedirectToAction("ModelConfiguration",new { Id = designModel_Id.ToString(),act = form["Act"] });
        }

        #endregion Datasource Selection

        #region Model Configuration

        public ActionResult ModelConfiguration(int Id,string act)
        {
            //Get Datasource
            var datasourceList = _context.DesignerModelDatasources.Where(x => x.DesignerModel_Id == Id).ToList();

            ViewBag.DatasourceList = datasourceList;
            ViewBag.DesignModel_Id = Id.ToString();
            ViewBag.Act = act;
            return View();
        }

        public JsonResult GetConfigTable(int draw,int pageSize,int pageStart,string sortField,string orderType,string search,int desingerModel_Id)
        {
            var list = _context.usp_DesignModel_Configuration_Datatable(search,pageStart,pageSize,sortField,orderType,desingerModel_Id).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ModelConfiguration(FormCollection form)
        {
            //Get Id
            var designModel_Id = Convert.ToInt32(form["DesignModel_Id"]);
            var mainKeyDatasource = form["MainkeyDatasource"].ToString();
            var objmodelDatasource = _context.DesignerModelDatasources.Where(x => x.DesignerModel_Id == designModel_Id && x.DatasourceName == mainKeyDatasource).FirstOrDefault();

            //Update Mainkey Datasource
            objmodelDatasource.IsMainKey = true;

            //Create or update Mainkey Parameter
            var mainKeyFormula = _context.usp_DesignModel_GetMainKey(objmodelDatasource.DatasourceName).FirstOrDefault();
            var objMainKeyParameter = _context.DesignerParameters.Where(x => x.DesignerModel_Id == designModel_Id && x.ParameterIndex == 1).FirstOrDefault();
            if(objMainKeyParameter == null)
            {
                objMainKeyParameter = new DesignerParameter()
                {
                    ParameterIndex = 1,
                    DesignerModel_Id = designModel_Id,
                    ParameterName = "MainKey",
                    ParameterFormula = mainKeyFormula
                };

                _context.DesignerParameters.Add(objMainKeyParameter);
            }
            else
            {
                objMainKeyParameter.ParameterFormula = mainKeyFormula;
            }
            _context.SaveChanges();

            return RedirectToAction("ModelParameter",new { Id = designModel_Id.ToString(),act = form["Act"] });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetModelConfigurationDetail(int Id)
        {
            var result = _context.DesignerModelConfigs.Where(x => x.Id == Id).FirstOrDefault();
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ValidateForAddConfig(int designModelId,string name)
        {
            var result = true;

            //Validate name not exsist in database
            var count = _context.DesignerModelConfigs.Where(x => x.DesignerModel_Id == designModelId && x.ConfigName == name.Trim()).Count();
            if(count == 1)
            {
                //already exsists in the database
                result = false;
            }

            return Json(new { validate = result,error = name + " already exsist in the database. please enter new valid name" },JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ValidateForEditConfig(int designModelId,int ConfigId,string newName)
        {
            //Validate name not exsist in database
            var result = true;
            var count = _context.DesignerModelConfigs.Where(x => x.DesignerModel_Id == designModelId &&
                                                            x.ConfigName == newName &&
                                                            x.Id != ConfigId).Count();

            if(count == 1)
            {
                result = false;
            }

            return Json(new { validate = result,error = newName + " already exsist in the database. please enter new valid name" },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddModelConfig(DesignerModelConfig cf)
        {
            var result = true;
            var errorText = "";
            cf.ConfigIndex = 1;

            try
            {
                var lastModelConfigIndex = _context.DesignerModelConfigs.Where(x => x.DesignerModel_Id == cf.DesignerModel_Id).OrderByDescending(x => x.ConfigIndex).FirstOrDefault();
                if(lastModelConfigIndex != null)
                {
                    cf.ConfigIndex = lastModelConfigIndex.ConfigIndex.Value + 1;
                }
                _context.DesignerModelConfigs.Add(cf);
                _context.SaveChanges();

                _context.usp_DesignModel_ReCalculate(cf.DesignerModel_Id);
            }
            catch(Exception ex)
            {
                errorText = ex.Message;
                result = false;
            }

            return Json(new { success = result,obj = cf,error = errorText },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditModelConfig(DesignerModelConfig cf)
        {
            var result = true;
            var errorText = "";

            try
            {
                var objConfig = _context.DesignerModelConfigs.Where(x => x.Id == cf.Id).FirstOrDefault();
                objConfig.ConfigName = cf.ConfigName;
                objConfig.ConfigFrom = cf.ConfigFrom;
                objConfig.ConfigTo = cf.ConfigTo;
                objConfig.ConfigValue = cf.ConfigValue;
                objConfig.ConfigDescription = cf.ConfigDescription;
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                errorText = ex.Message;
                result = false;
            }

            return Json(new { success = result,obj = cf,error = errorText },JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetModelConfigurationList(int designModelId)
        {
            var result = _context.DesignerModelConfigs.Where(x => x.DesignerModel_Id == designModelId &&
                                                             x.ConfigName != Properties.Settings.Default.Config_ModelName &&
                                                             x.ConfigName != Properties.Settings.Default.Config_MainKeySheet).OrderBy(x => x.ConfigIndex).ToList();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateModelConfigIndex(List<String> values)
        {
            var result = true;
            var errorText = "";

            try
            {
                //re index model config
                for(int i = 0;i < (values.Count);i++)
                {
                    var id = Convert.ToInt32(values[i]);
                    var objModelConfig = _context.DesignerModelConfigs.Where(x => x.Id == id).FirstOrDefault();
                    objModelConfig.ConfigIndex = i + 3; // (เริ่มจาก 1) ข้าม ModelName และ Mainkey Sheet
                    _context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                result = false;
                errorText = ex.Message;
            }

            return Json(new { success = result,errorText });
        }

        [HttpPost]
        public ActionResult UpdateModelConfigMainKeyDatasource(int designerModelId,string mainKeyDatasouceName)
        {
            var result = true;
            var errorText = "";
            try
            {
                //Update Mainkey Datasource
                //Remove previous mainkey datasource
                var objPreviuosMainkey = _context.DesignerModelDatasources.Where(x => x.DesignerModel_Id == designerModelId && x.IsMainKey == true).FirstOrDefault();
                if(objPreviuosMainkey != null)
                {
                    objPreviuosMainkey.IsMainKey = false;
                    _context.SaveChanges();
                }

                //Set new main key
                var objmodelDatasource = _context.DesignerModelDatasources.Where(x => x.DesignerModel_Id == designerModelId && x.DatasourceName == mainKeyDatasouceName).FirstOrDefault();
                objmodelDatasource.IsMainKey = true;
                _context.SaveChanges();
                _context.usp_DesignModel_ReCalculate(designerModelId);

                //Update config datasource
                var objConfig = _context.DesignerModelConfigs.Where(x => x.DesignerModel_Id == designerModelId
                                                                 && x.ConfigName == Properties.Settings.Default.Config_MainKeySheet).FirstOrDefault();
                objConfig.ConfigValue = mainKeyDatasouceName;
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                result = false;
                errorText = ex.Message;
            }
            return Json(new { success = result,error = errorText },JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteModelConfig(int designerModelConfigId)
        {
            var result = true;
            var errorText = "";

            try
            {
                // get object
                var objDesignerModelConfig = _context.DesignerModelConfigs.Where(x => x.Id == designerModelConfigId).Single();

                //Re Index for higher items (make it index = index-1)
                List<DesignerModelConfig> lstHigherIndex = _context.DesignerModelConfigs.Where(x => x.DesignerModel_Id == objDesignerModelConfig.DesignerModel_Id
                                                                         && x.ConfigIndex > objDesignerModelConfig.ConfigIndex).ToList();

                for(int i = 0;i < lstHigherIndex.Count;i++)
                {
                    lstHigherIndex[i].ConfigIndex -= 1;
                }

                //Delete DesignerModelConfig
                _context.DesignerModelConfigs.Remove(objDesignerModelConfig);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                errorText = ex.Message;
                result = false;
                throw;
            }
            return Json(new { success = result,error = errorText },JsonRequestBehavior.AllowGet);
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
                var objDesignerModelParameter = _context.DesignerParameters.Where(x => x.Id == designerModelParameterId).Single();

                //Re Index for higher items (make it index = index-1)
                List<DesignerParameter> lstHigherIndex = _context.DesignerParameters.Where(x => x.DesignerModel_Id == objDesignerModelParameter.DesignerModel_Id
                                                                         && x.ParameterIndex > objDesignerModelParameter.ParameterIndex).ToList();

                for(int i = 0;i < lstHigherIndex.Count;i++)
                {
                    lstHigherIndex[i].ParameterIndex -= 1;
                }

                //Delete DesignerModelConfig
                _context.DesignerParameters.Remove(objDesignerModelParameter);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                errorText = ex.Message;
                result = false;
                throw;
            }
            return Json(new { success = result,error = errorText },JsonRequestBehavior.AllowGet);
        }

        public ActionResult ModelParameter(int Id,string act)
        {
            ViewBag.DesignModel_Id = Id.ToString();
            ViewBag.Act = act;
            //TODO: Manage Parameter
            return View();
        }

        public JsonResult GetParameterTable(int draw,int pageSize,int pageStart,string sortField,string orderType,string search,int desingerModel_Id)
        {
            var list = _context.usp_DesignModel_Parameter_Datatable(search,pageStart,pageSize,sortField,orderType,desingerModel_Id).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddModelParameter(DesignerParameter param)
        {
            var result = true;
            var errorText = "";
            param.ParameterIndex = 1;

            try
            {
                var lastParameterIndex = _context.DesignerParameters.Where(x => x.DesignerModel_Id == param.DesignerModel_Id).OrderByDescending(x => x.ParameterIndex).FirstOrDefault();
                if(lastParameterIndex != null)
                {
                    param.ParameterIndex = lastParameterIndex.ParameterIndex.Value + 1;
                }
                _context.DesignerParameters.Add(param);
                _context.SaveChanges();
                _context.usp_DesignModel_ReCalculate(param.DesignerModel_Id);
            }
            catch(Exception ex)
            {
                errorText = ex.Message;
                result = false;
            }

            return Json(new { success = result,obj = param,error = errorText },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditModelParameter(DesignerParameter param)
        {
            var result = true;
            var errorText = "";

            try
            {
                var objParam = _context.DesignerParameters.Where(x => x.Id == param.Id).FirstOrDefault();
                objParam.ParameterName = param.ParameterName;
                objParam.ParameterFormula = param.ParameterFormula;
                objParam.ParameterDataType = param.ParameterDataType;
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                errorText = ex.Message;
                result = false;
            }

            return Json(new { success = result,obj = param,error = errorText },JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetModelParameterList(int designModelId)
        {
            var result = _context.DesignerParameters.Where(x => x.DesignerModel_Id == designModelId && x.ParameterIndex != 1).OrderBy(x => x.ParameterIndex).ToList();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateModelParameterIndex(List<String> values)
        {
            var result = true;
            var errorText = "";

            try
            {
                //re index model config
                for(int i = 0;i < (values.Count);i++)
                {
                    var id = Convert.ToInt32(values[i]);
                    var objDesignerParameter = _context.DesignerParameters.Where(x => x.Id == id).FirstOrDefault();
                    objDesignerParameter.ParameterIndex = i + 2; //(เริ่มจาก 1 ) ข้าม Main Key
                    _context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                result = false;
                errorText = ex.Message;
            }

            return Json(new { success = result,errorText });
        }

        [HttpGet]
        public ActionResult ValidateForAddParameter(int designModelId,string name)
        {
            var result = true;
            //Validate name not exsist in database
            var count = _context.DesignerParameters.Where(x => x.DesignerModel_Id == designModelId && x.ParameterName == name.Trim()).Count();
            if(count == 1)
            {
                //already exsists in the database
                result = false;
            }

            return Json(new { validate = result,error = name + " already exsist in the database. please enter new valid name" },JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ValidateForEditParameter(int designModelId,int ConfigId,string newName)
        {
            //Validate name not exsist in database
            var result = true;
            var count = _context.DesignerParameters.Where(x => x.DesignerModel_Id == designModelId &&
                                                            x.ParameterName == newName &&
                                                            x.Id != ConfigId).Count();

            if(count == 1)
            {
                result = false;
            }

            return Json(new { validate = result,error = newName + " already exsist in the database. please enter new valid name" },JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetModelParameterDetail(int Id)
        {
            var result = _context.DesignerParameters.Where(x => x.Id == Id).FirstOrDefault();
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddUpdateDesignerTable(int designModelId)
        {
            try
            {
                var result = _context.usp_CreateUpdateDesignerTable(designModelId);
                return Json(true,JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(false,JsonRequestBehavior.AllowGet);
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
            var rootUrl = string.Format("{0}://{1}{2}",Request.Url.Scheme,Request.Url.Authority,Url.Content("~"));
            var url = rootUrl + "Content/CreatedExcelFiles/" + x.DesignerModel_Name + ".xlsx";

            return Redirect(url);
        }

        #endregion Download
    }
}