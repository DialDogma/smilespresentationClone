using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Http.Results;
using OfficeOpenXml;
using SmileSCommission.Helper;
using SmileSCommission.Models;

namespace SmileSCommission.Controllers
{
    [Authorization]
    public class ConfigurationController : Controller
    {
        #region Context

        private readonly CommissionCalculateEntities _context;
        private static readonly CultureInfo _dtEn = new CultureInfo("en-US");

        public ConfigurationController()

        {
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        #region View

        public ActionResult cf_Datasource()
        {
            return View();
        }

        public ActionResult ModelManagement_dictionary()
        {
            return View();
        }

        public ActionResult ModelManagementDetail_dictionary(int modelId)
        {
            ViewBag.modelId = modelId;
            var modelName = _context.usp_Model_Select(modelId, null, null, null, null, null).FirstOrDefault().ModelName;
            if (modelName != null)
            {
                ViewBag.modelName = modelName;
            }

            return View();
        }

        public ActionResult cf_Model()
        {
            var modelResult = _context.usp_CommissionPeriod_GetLastedPeriod_Select().FirstOrDefault();

            if (modelResult.CommissionPeriodStatusId != 2)
            {
                return RedirectToAction("InternalServerError", "Error", new { exception = "รอบค่าตอบแทนได้ถูกนำส่งแล้ว!" });
            }

            if (modelResult == null)
            {
                return RedirectToAction("InternalServerError", "Error", new { exception = "กรุณาสร้างรอบการคิดค่าตอบแทนใหม่" });
            }
            else
            {
                ViewBag.periodId = modelResult.PeriodId;
                ViewBag.PeriodDetail = modelResult.PayPeriod?.ToString("dd/MM/yyyy");
            }

            return View();
        }

        public ActionResult cf_ModelShowInReport()
        {
            return View();
        }

        #endregion View

        #region api

        #region api_CfDT

        //get datasource
        [HttpGet]
        public JsonResult GetCFDataSource(int? draw = null, int? indexStart = null, int? pageSize = null,
            string sortField = null, string orderType = null, string search = null)
        {
            var result = _context.usp_DataSourceMaster_Select(indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        //generate datasource config
        [HttpGet]
        public JsonResult GenerateCFDataSource()
        {
            var result = _context.usp_DataSourceMaster_Generate().FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //save assign datasource
        [HttpPost]
        public JsonResult SetAssignDataSource(int dtsMasterId, string empCode)
        {
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            //var user = "02752";
            var result = _context.usp_DataSourceMaster_Allowlist_Update(dtsMasterId, empCode, user).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion api_CfDT

        #region api_CFModelDic

        //get model datatable
        [HttpGet]
        public JsonResult GetModel_DT(int? draw = null, int? indexStart = null, int? pageSize = null,
            string sortField = null, string orderType = null, string search = null)
        {
            var result = _context.usp_Model_Select(null, indexStart, pageSize, sortField, orderType, search).ToList();
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        //update model description
        [HttpPost]
        public JsonResult SetModelDescription(int modelId, string description)
        {
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            //var user = "02752";

            var result = _context.usp_Model_Description_Update(modelId, description, user).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //get model parameter description
        [HttpPost]
        public JsonResult GetModelParamDescription(int modelId, int? draw = null, int? indexStart = null,
            int? pageSize = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _context.usp_ModelParameterDescription_Select(modelId, indexStart, pageSize, sortField, orderType,
                search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        //update model parameter description
        [HttpPost]
        public JsonResult SetModelParamDescription(int paramId, string description)
        {
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            //var user = "02752";
            var result = _context.usp_ModelParameterDescription_Update(paramId, description, user).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //generate model parameters
        [HttpGet]
        public JsonResult GenerateModelParam(int modelId)
        {
            var result = _context.usp_ModelParameterDescription_Generate(modelId).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion api_CFModelDic

        #region CFModel

        //get latest model for datatable
        [HttpGet]
        public JsonResult GetModelPeriod_DT(int periodId, int? draw = null, int? indexStart = null, int? pageSize = null,
            string sortField = null, string orderType = null, string search = null)
        {
            var result = _context.usp_ModelCalculationIndex_Select(periodId, indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        //get latest model for re order
        [HttpGet]
        public JsonResult GetModelPeriod(int periodId)
        {
            var result = _context.usp_ModelCalculationIndex_Select(periodId, null, 9999, null, null, null).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetExistingModel(int periodId)
        {
            var result = _context.usp_ApplyDesignerToCalculationIndex_Select(periodId, null, 999, null, null, null).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //add existing model
        [HttpPost]
        public JsonResult AddModelFromBase(int designerModelId, int periodId)
        {
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;

            var result = _context.usp_ApplyDesignerToCalculationIndex(designerModelId, periodId, user).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //upload model from excel
        //[HttpPost]
        //public JsonResult UploadModelFromExcel(HttpPostedFileBase uploadFile)
        //{
        //    //request period id from post
        //    var periodId = Convert.ToInt32(Request["periodId"]);
        //    var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
        //    //var user = "02752";
        //    var _model = new CalculateModelFromExcel(periodId);

        //    if(uploadFile != null && uploadFile.ContentLength > 0)
        //    {
        //        //check extension
        //        if(Path.GetExtension(uploadFile.FileName) == ".xlsx" || Path.GetExtension(uploadFile.FileName) == ".xls")
        //        {
        //            _model.ReadFromExcel(uploadFile);

        //            if(_model.IsValid)
        //            {
        //                if(_model.SaveModelToDatabase())
        //                {
        //                    //call index
        //                    var objCallIndex = _context.ModelCalculationIndex.FirstOrDefault(x => x.Period_Id == _model.Period_Id &&
        //                    x.Model_Id == _model.);
        //                    //if exits
        //                    if(objCallIndex == null)
        //                    {
        //                        //Get count
        //                        var count = _context.ModelCalculationIndex.Count(x => x.Period_Id == _model.Period_Id);

        //                        //Create new one
        //                        objCallIndex = new ModelCalculationIndex() { Period_Id = _model.Period_Id,ModelName = _model.ModelName,No = count };

        //                        _context.ModelCalculationIndex.Add(objCallIndex);
        //                    }
        //                    //if not exists
        //                    var objModel = _context.Model.FirstOrDefault(x => x.ModelName == _model.ModelName);
        //                    if(objModel == null)
        //                    {
        //                        objModel = new Model() { ModelName = _model.ModelName,CreatedBy = user,CreatedDate = DateTime.Now };
        //                        _context.Model.Add(objModel);
        //                    }

        //                    _context.SaveChanges();

        //                    var result = new { IsResult = true,Msg = "Success!" };
        //                    return Json(result,JsonRequestBehavior.AllowGet);
        //                }
        //                else
        //                {
        //                    var result = new { IsResult = false,Msg = "เกิดข้อผิดพลาด กรุณาแจ้งผู้พัฒนาระบบ!" + _model.ErrorText };
        //                    return Json(result,JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //            else
        //            {
        //                var result = new { IsResult = false,Msg = "ไฟล์ไม่ถูกต้องตามแบบ!" };
        //                return Json(result,JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        else
        //        {
        //            var result = new { IsResult = false,Msg = "นามสกุลไฟล์ไม่ถูกต้อง!" };
        //            return Json(result,JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        var result = new { IsResult = false,Msg = "ไม่พบข้อมูลในไฟล์!" };
        //        return Json(result,JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        public JsonResult DeleteModelPeriod(int modelId)
        {
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            var result = _context.usp_ModelCalculationIndex_Delete(modelId, user).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetReOrderModel(int modelId, int order)
        {
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            //var user = "02752";

            var result = _context.usp_ModelCalculationIndex_ReOrder_Update(modelId, order, user).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion CFModel

        #endregion api
    }
}