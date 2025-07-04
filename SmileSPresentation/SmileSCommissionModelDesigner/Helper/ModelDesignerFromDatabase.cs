using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using OfficeOpenXml;
using SmileSCommissionModelDesigner.Models;
using System.IO;
using System.Data.SqlClient;

namespace SmileSCommissionModelDesigner.Helper
{
   public class ModelDesignerFromDatabase
    {
        public int Period_Id { get; set; }
        public int DesignerModel_Id { get; set; }
        public string DesignerModel_Name { get; set; }
        public DataTable CalculatedTable { get; set; }
        public ExcelPackage ExcelModelDesigner { get; set; }
        public bool IsValid { get; set; }
        public string ErrorText { get; set; }
        private CommissionCalculateEntities _context;
        public string FilePath { get; set; }

        public ModelDesignerFromDatabase(int designerModel_Id)
        {
            _context = new CommissionCalculateEntities();
            //Get last period id
            Period_Id = _context.CommissionPeriods.OrderByDescending(x => x.Id).FirstOrDefault().Id;
            this.DesignerModel_Id = designerModel_Id;
            this.DesignerModel_Name = _context.DesignerModels.Where(x => x.Id == designerModel_Id).FirstOrDefault().Name;
        }

        public void CreateExcelModelDesigner()
        {
            //Create Excel File
            ExcelModelDesigner = new ExcelPackage();

            string targetFolder = HttpContext.Current.Server.MapPath("~/Content/CreatedExcelFiles/");

            //Create Calculate folder
            bool exists = System.IO.Directory.Exists(targetFolder);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(targetFolder);
            }
            //Add file name
            this.FilePath = targetFolder + this.DesignerModel_Name + ".xlsx";

            //Create Sheet Config
            CreateConfigSheet();

            //Create Sheet Datasource
            CreateDatasourceSheet();

            //Create Sheet Calculate
            CreateCalculateSheet();

            //Save file
            Stream stream = File.Create(this.FilePath);
            ExcelModelDesigner.SaveAs(stream);
            stream.Close();
        }

        private bool CreateDatasourceSheet()
        {
            var db = new CommissionCalculateEntities();
            bool result = false;

            try
            {
                // Get Datasource list
                var dsList = db.DesignerModelDatasources.Where(x => x.DesignerModel_Id == this.DesignerModel_Id).OrderByDescending(x => x.IsMainKey).ToList();

                //Loop in list
                foreach (DesignerModelDatasource ds in dsList)
                {
                    //Create Sheet
                    ExcelModelDesigner.Workbook.Worksheets.Add(ds.DatasourceName);
                    //Bind Sheet
                    if (!BindDatasourceSheet(ExcelModelDesigner.Workbook.Worksheets[ds.DatasourceName]))
                    {
                        this.IsValid = false;
                        this.ErrorText = "Error on creating sheet:" + ds.DatasourceName;
                        return false;
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                this.ErrorText = ex.Message;
                this.IsValid = false;
            }
            finally
            {
                db.Dispose();
            }

            return result;
        }

        /// <summary>
        /// Bind Datasource Sheet
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private bool BindDatasourceSheet(ExcelWorksheet sheet)
        {
            //Bind Sheet
            //Use SQLConnection instead of entity framework
            //Because usp returns table with dynamic columns
            SqlConnection connection = new SqlConnection(new CommissionCalculateEntities().Database.Connection.ConnectionString);

            SqlCommand command = new SqlCommand("usp_GetDatasourceByTableNameAndPeriodId", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("TableName", sheet.Name));
            command.Parameters.Add(new SqlParameter("Period_Id", this.Period_Id.ToString()));
            connection.Open();

            //Get Datatable
            System.Data.DataTable dt = new System.Data.DataTable();

            dt.Load(command.ExecuteReader());

            connection.Close();

            //Bind Table To sheet
            if (dt.Rows.Count > 0)
            {
                sheet.Cells["A1"].LoadFromDataTable(dt, true);
            }

            //Naming Ranges
            for (int i = 1; i < dt.Columns.Count + 1; i++)
            {
                //Get numberofrow
                var nameToRow = dt.Rows.Count + 1;
                //Get address
                var address = sheet.Cells[2, i, nameToRow, i].Address;
                //gat name
                var name = sheet.Name + "_" + sheet.Cells[1, i].Text;
                //Naming Range
                var nr = new ExcelNamedRange(name, null, sheet, address, 1);
                sheet.Workbook.Names.Add(name, nr);
            }

            //Naming Ranges for table
            var tableAddress = sheet.Cells[2, 1, dt.Rows.Count + 1, dt.Columns.Count].Address;
            var tableNamingRange = new ExcelNamedRange(sheet.Name, null, sheet, tableAddress, 1);
            sheet.Workbook.Names.Add(sheet.Name, tableNamingRange);

            return true;
        }

        private bool CreateCalculateSheet()
        {
            bool result = false;
            var db = new CommissionCalculateEntities();

            try
            {
                //Create Sheet
                ExcelModelDesigner.Workbook.Worksheets.Add(Properties.Settings.Default.Model_CalculateSheetName);

                //Get Sheet
                var sheet = ExcelModelDesigner.Workbook.Worksheets[Properties.Settings.Default.Model_CalculateSheetName];

                //Get MainIndex Datasource Sheetname
                var mainIndexSheetName = db.DesignerModelDatasources.Where(x => x.DesignerModel_Id == this.DesignerModel_Id && x.IsMainKey == true).FirstOrDefault().DatasourceName;

                //Get MainIndex last row
                var mainIndexSheet = ExcelModelDesigner.Workbook.Worksheets[mainIndexSheetName];
                var mainIndexSheetRowsCount = ExcelHelper.GetDataTableFromExcel(mainIndexSheet, true).Rows.Count;

                //Create Parameter
                var lstParameter = db.DesignerParameters.Where(x => x.DesignerModel_Id == this.DesignerModel_Id).OrderBy(x => x.ParameterIndex).ToList();

                var loopCount = 0;

                foreach (var param in lstParameter)
                {
                    //Create Header
                    sheet.Cells[1, param.ParameterIndex.Value].Value = param.ParameterName;
                    //Get Range
                    var address = sheet.Cells[2, param.ParameterIndex.Value, mainIndexSheetRowsCount + 1, param.ParameterIndex.Value].Address;

                    //Bind formula
                    sheet.Cells[address].Formula = param.ParameterFormula;

                    //Naming ranges
                    var name = param.ParameterName;
                    var nr = new ExcelNamedRange(name, null, sheet, address, 1);
                    sheet.Workbook.Names.Add(name, nr);

                    loopCount += 1;
                    if (loopCount == 118)
                    {
                        //TODO: debug
                        loopCount = 118;
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                this.ErrorText = ex.Message;
            }
            finally
            {
                db.Dispose();
            }

            return result;
        }

        /// <summary>
        /// Create Config Sheet
        /// </summary>
        private bool CreateConfigSheet()
        {
            bool result = false;

            var configSheet = ExcelModelDesigner.Workbook.Worksheets.Add(Properties.Settings.Default.Model_ConfigSheetName);

            try
            {
                var lstConfig = _context.usp_DesignModel_GetConfigForCreateSheet(DesignerModel_Id).ToList();

                if (lstConfig.Count() > 0)
                {
                    //Create data
                    var dt = new GlobalFunction().ConvertToDataTable(lstConfig);
                    configSheet.Cells["A1"].Value = Properties.Settings.Default.ConfigSheet_ColumnName_Name;
                    configSheet.Cells["B1"].Value = Properties.Settings.Default.ConfigSheet_ColumnName_From;
                    configSheet.Cells["C1"].Value = Properties.Settings.Default.ConfigSheet_ColumnName_To;
                    configSheet.Cells["D1"].Value = Properties.Settings.Default.ConfigSheet_ColumnName_Value;
                    configSheet.Cells["E1"].Value = Properties.Settings.Default.ConfigSheet_ColumnName_Description;

                    //Bind Data
                    var rowIndex = 2;
                    foreach (var item in lstConfig)
                    {
                        configSheet.Cells[rowIndex, 1].Value = item.Name;
                        configSheet.Cells[rowIndex, 2].Value = item.From;
                        configSheet.Cells[rowIndex, 3].Value = item.To;
                        configSheet.Cells[rowIndex, 4].Value = item.Value;
                        configSheet.Cells[rowIndex, 5].Value = item.Description;

                        //Replace if numeric value
                        if (GlobalFunction.IsNumeric(item.From))
                        {
                            configSheet.Cells[rowIndex, 2].Value = Convert.ToDouble(item.From);
                        }
                        if (GlobalFunction.IsNumeric(item.To))
                        {
                            configSheet.Cells[rowIndex, 3].Value = Convert.ToDouble(item.To);
                        }
                        if (GlobalFunction.IsNumeric(item.Value))
                        {
                            configSheet.Cells[rowIndex, 4].Value = Convert.ToDouble(item.Value);
                        }
                        rowIndex++;
                    }

                    //Naming ranges
                    for (int i = 2; i < dt.Rows.Count + 2; i++)
                    {
                        var name = configSheet.Cells[i, 1].Text;
                        var nr = new ExcelNamedRange(name, null, configSheet, "D" + i.ToString(), 1);
                        configSheet.Workbook.Names.Add(name, nr);
                    }

                    result = true;
                }
                else
                {
                    this.ErrorText = "Could not read config from database";
                }
            }
            catch (Exception ex)
            {
                this.ErrorText = ex.Message;
            }

            return result;
        }
    }
}