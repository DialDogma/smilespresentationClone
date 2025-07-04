using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using SmileSCommission.Models;

namespace SmileSCommission.Helper
{
    internal class CalculateModelFromExcel
    {
        public int Period_Id { get; set; }
        public string ModelName { get; set; }
        public ExcelPackage ExcelModel { get; set; }
        public List<ModelConfig> ConfigList { get; set; }
        public List<ModelParameter> ParameterList { get; set; }
        public List<ModelDatasouce> DatasourceList { get; set; }
        public bool IsValid { get; set; }
        public string ErrorText { get; set; }

        public CalculateModelFromExcel(int periodId)
        {
            this.Period_Id = periodId;
            ConfigList = new List<ModelConfig>();
            ParameterList = new List<ModelParameter>();
            DatasourceList = new List<ModelDatasouce>();
            IsValid = false;
            ErrorText = "Call ReadFromExcel() required";
        }

        public void ReadFromExcel(HttpPostedFileBase file)
        {
            //...first fetch excel file from project folder or any others location…

            //var fi = new FileInfo(file.FileName);
            var package = new ExcelPackage(file.InputStream);

            //Validate package format
            if(!ValidateExcelWorkbookFormat(package.Workbook))
            {
                return;
            }

            //Validate config
            var configSheet = package.Workbook.Worksheets[Properties.Settings.Default.Model_ConfigSheetName];
            if(!ValidateConfig(configSheet))
            {
                return;
            }

            //Validate Datasourcelist
            if(!ValidateDatasourceList(package.Workbook))
            {
                return;
            }

            //Read Config
            if(!ReadConfig(configSheet))
            {
                return;
            }

            //Read Datsource
            if(!ReadDatasource(package.Workbook))
            {
                return;
            }

            //Read Calculate Formula Parameter
            var calculateSheet = package.Workbook.Worksheets[Properties.Settings.Default.Model_CalculateSheetName];
            if(!ReadParameter(calculateSheet))
            {
                return;
            }

            //Set Validate
            this.IsValid = true;
        }

        public bool SaveModelToDatabase()
        {
            bool result = false;
            var db = new CommissionCalculateEntities();

            //Save To Database
            try
            {
                //Add Model if not exsists
                var modelCount = db.Model.Count(x => x.ModelName == this.ModelName);
                if(modelCount == 0)
                {
                    var model = new Model() { ModelName = this.ModelName,CreatedBy = Authorization.GetLoginDetail().EmployeeCode,CreatedDate = DateTime.Now };
                    db.Model.Add(model);
                    db.SaveChanges();
                }

                //Clear Previous values in database
                //clear config
                var configToRemove = db.ModelConfig.Where(x => x.Period_Id == this.Period_Id && x.ModelName == this.ModelName).ToList();
                db.ModelConfig.RemoveRange(configToRemove);

                //clear datasource
                var dsToRemove = db.ModelDatasouce.Where(x => x.Period_Id == this.Period_Id && x.ModelName == this.ModelName).ToList();
                db.ModelDatasouce.RemoveRange(dsToRemove);

                //clear parameter
                var paramToRemove = db.ModelParameter.Where(x => x.Period_Id == this.Period_Id && x.ModelName == this.ModelName).ToList();
                db.ModelParameter.RemoveRange(paramToRemove);

                //Insert new record
                //Insert Config
                db.ModelConfig.AddRange(this.ConfigList);

                //Insert Datasource
                db.ModelDatasouce.AddRange(this.DatasourceList);

                //Insert Parameter
                db.ModelParameter.AddRange(this.ParameterList);

                //Execute
                db.SaveChanges();
                result = true;
            }
            catch(Exception ex)
            {
                this.IsValid = false;
                this.ErrorText = ex.Message;
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        #region ModelImport

        /// <summary>
        /// Validate Config
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private bool ValidateConfig(ExcelWorksheet sheet)
        {
            //Get column Name list from sheet Config
            List<String> lstSheetColumnName;
            List<String> lstColumnToCheck = new List<String>();

            lstSheetColumnName = cExcelHelper.GetHeaderColumns(sheet);

            //Get lstColumnNameToCheck
            lstColumnToCheck.Add(Properties.Settings.Default.ConfigSheet_ColumnName_Name);
            lstColumnToCheck.Add(Properties.Settings.Default.ConfigSheet_ColumnName_Description);
            lstColumnToCheck.Add(Properties.Settings.Default.ConfigSheet_ColumnName_From);
            lstColumnToCheck.Add(Properties.Settings.Default.ConfigSheet_ColumnName_To);
            lstColumnToCheck.Add(Properties.Settings.Default.ConfigSheet_ColumnName_Value);

            //Validate column Name
            foreach(var c in lstColumnToCheck)
            {
                if(!lstSheetColumnName.Contains(c))
                {
                    this.IsValid = false;
                    this.ErrorText = "ไม่พบ column " + c + " ใน sheet " + Properties.Settings.Default.Model_ConfigSheetName;
                    return false;
                }
            }

            //Validate sys_ModelName
            var model = sheet.Cells[Properties.Settings.Default.Config_sys_ModelName_Cell].Text;
            if(string.IsNullOrEmpty(model))
            {
                this.IsValid = false;
                this.ErrorText = "ModelName Require at D:2";
                return false;
            }

            //validate MainKey_Sheet
            var mainKey = sheet.Cells[Properties.Settings.Default.Config_sys_MainKeySheet_Cell].Text;
            if(string.IsNullOrEmpty(mainKey))
            {
                this.IsValid = false;
                this.ErrorText = "MainKey Require at D:2";
                return false;
            }

            //Set value
            this.ModelName = model;

            return true;
        }

        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        private bool ValidateExcelWorkbookFormat(ExcelWorkbook book)
        {
            //Get worksheet name list
            List<String> lstWorkSheetNames;

            lstWorkSheetNames = book.Worksheets.Select(x => x.Name).ToList();

            //Require Sheetname "Config"
            if(!lstWorkSheetNames.Contains(Properties.Settings.Default.Model_ConfigSheetName))
            {
                this.IsValid = false;
                this.ErrorText = "ไม่พบ Sheet:" + Properties.Settings.Default.Model_ConfigSheetName;
                return false;
            }

            //Require Sheetname "Calculate"
            if(!lstWorkSheetNames.Contains(Properties.Settings.Default.Model_CalculateSheetName))
            {
                this.IsValid = false;
                this.ErrorText = "ไม่พบ Sheet:" + Properties.Settings.Default.Model_CalculateSheetName;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validate datasource list
        /// </summary>
        /// <returns></returns>
        private bool ValidateDatasourceList(ExcelWorkbook book)
        {
            var _db = new CommissionCalculateEntities();
            var datasourceTableNames = _db.usp_GetTableName().Where(x => x.StartsWith("D_")).ToList();

            //check datasourceSheetName exsist in database
            foreach(var sheet in book.Worksheets)
            {
                if(sheet.Name.StartsWith("D_"))
                {
                    if(!datasourceTableNames.Contains(sheet.Name))
                    {
                        this.IsValid = false;
                        this.ErrorText = "ไม่พบ:" + sheet.Name + "ในฐานข้อมุล";
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Read Config
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private bool ReadConfig(ExcelWorksheet sheet)
        {
            try
            {
                //GetDatatable
                var dt = cExcelHelper.GetDataTableFromExcel(sheet,true);

                int index = 0;

                foreach(DataRow dr in dt.Rows)
                {
                    if(!String.IsNullOrEmpty(dr[Properties.Settings.Default.ConfigSheet_ColumnName_Name].ToString()))
                    {
                        index += 1;
                        var objConfig = new ModelConfig();
                        objConfig.Period_Id = this.Period_Id;
                        objConfig.ModelName = this.ModelName;
                        objConfig.No = index;
                        objConfig.ConfigName = dr[Properties.Settings.Default.ConfigSheet_ColumnName_Name].ToString();
                        objConfig.ConfigFrom = dr[Properties.Settings.Default.ConfigSheet_ColumnName_From].ToString();
                        objConfig.ConfigTo = dr[Properties.Settings.Default.ConfigSheet_ColumnName_To].ToString();
                        objConfig.ConfigValue = dr[Properties.Settings.Default.ConfigSheet_ColumnName_Value].ToString();
                        objConfig.Description = dr[Properties.Settings.Default.ConfigSheet_ColumnName_Description].ToString();
                        this.ConfigList.Add(objConfig);
                    }
                }

                return true;
            }
            catch(Exception ex)
            {
                this.IsValid = false;
                this.ErrorText = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Read Datasource
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        private bool ReadDatasource(ExcelWorkbook book)
        {
            try
            {
                int Index = 0;
                //Loop for add datasource
                foreach(var sheet in book.Worksheets)
                {
                    if(sheet.Name.StartsWith("D_") || sheet.Name.StartsWith("C_"))
                    {
                        Index += 1;
                        this.DatasourceList.Add(new ModelDatasouce()
                        {
                            Period_Id = this.Period_Id,
                            No = Index,
                            ModelName = this.ModelName,
                            DatasourceName = sheet.Name
                        });
                    }
                }

                return true;
            }
            catch(Exception ex)
            {
                this.IsValid = false;
                this.ErrorText = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Read Parameter
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private bool ReadParameter(ExcelWorksheet sheet)
        {
            try
            {
                //Get column list
                List<String> lstParameters = cExcelHelper.GetHeaderColumns(sheet);

                int index = 0;

                foreach(string paramName in lstParameters)
                {
                    //Add parameter
                    var readFormula = sheet.Cells[2,index + 1].Formula.ToString();
                    if(readFormula.Substring(0,1) != "=")
                    {
                        readFormula = "=" + readFormula;
                    }
                    this.ParameterList.Add(new ModelParameter()
                    {
                        ModelName = this.ModelName,
                        Period_Id = this.Period_Id,
                        ParameterName = paramName,
                        No = index + 1,
                        Formula = readFormula
                    });

                    index += 1;
                }

                return true;
            }
            catch(Exception ex)
            {
                this.IsValid = false;
                this.ErrorText = ex.Message;
                return false;
            }
        }

        #endregion ModelImport
    }
}