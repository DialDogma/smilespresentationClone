using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using SmileSCriticalIllness.Helper;

namespace SmileSCriticalIllness.Controllers
{
    [Authorization]
    public class DataImportController:Controller
    {
        #region Declare Variables

        //private readonly  _context;

        //public DataImportController()
        //{
        //    // Set entity
        //    _context = new ();
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    _context.Dispose();
        //}

        // TODO: Prepare invalid model and extend model for this import
        //public class DataImportInvalid:
        //{
        //    public List<string> InvalidList { get; set; }
        //}

        #endregion Declare Variables

        // GET: DataImport
        public ActionResult Index()
        {
            // TODO: Prepare valid and invalid model for initial table
            //valid list
            ViewBag.Valid = new List<string>();
            ViewBag.ValidJson = "[]";
            //invalid list
            ViewBag.Invalid = new List<string>();
            ViewBag.InvalidJson = "[]";
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            //    var helpers = new ValidationHelpers();

            //    // TODO: Prepare valid and invalid model
            //    var valid = new List<D_Area>();
            //    var invalid = new List<AreaInvalid>();

            //    // Check file
            //    if(file != null)
            //    {
            //        System.IO.Stream fileContent = file.InputStream;

            //        using(ExcelPackage excelPackage = new ExcelPackage(fileContent))
            //        {
            //            // Select first worksheet
            //            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];

            //            var rows = ((object[,])worksheet.Cells.Value);

            //            #region Validate Column

            //            // TODO: Validate column, if not match not allow to process.

            //            var columns = new List<String> {
            //                "StudyAreaCode",
            //                "StudyArea",
            //                "BranchCode",
            //                "Branch",
            //                "ProvinceID",
            //                "ProvinceName"
            //            };

            //            var missingColumn = helpers.ValidateColumn(rows,columns);

            //            if(missingColumn.Count > 0)
            //            {
            //                ViewBag.isInvalidColumn = true;

            //                ViewBag.isInvalidMsg = "ไม่พบ column : " + String.Join(", ",missingColumn);

            //                ViewBag.Valid = new List<D_Area>();
            //                ViewBag.ValidJson = "[]";
            //                ViewBag.Invalid = new List<AreaInvalid>();
            //                ViewBag.InvalidJson = "[]";

            //                return View("Index");
            //            }

            //            #endregion Validate Column

            //            //loop all rows
            //            for(int i = worksheet.Dimension.Start.Row;i <= worksheet.Dimension.End.Row - 1;i++)
            //            {
            //                // TODO: Prepare valid and invalid model for each row
            //                var rowResult = new D_Area();
            //                var invalidList = new List<string>();

            //                #region Validate Column

            //                // TODO: Check type and require column from document condition

            //                // Column StudyAreaCode
            //                var StudyAreaCode = helpers.ValidateString("StudyAreaCode",rows[i,0],true);

            //                if(StudyAreaCode.IsValid) rowResult.StudyAreaCode = StudyAreaCode.Value;
            //                if(!StudyAreaCode.IsValid) invalidList.Add(StudyAreaCode.InvalidMessage);

            //                // Column StudyArea
            //                var StudyArea = helpers.ValidateString("StudyArea",rows[i,1],true);

            //                if(StudyArea.IsValid) rowResult.StudyArea = StudyArea.Value;
            //                if(!StudyArea.IsValid) invalidList.Add(StudyArea.InvalidMessage);

            //                // Column BranchCode
            //                var BranchCode = helpers.ValidateString("BranchCode",rows[i,2],true);

            //                if(BranchCode.IsValid) rowResult.BranchCode = BranchCode.Value;
            //                if(!BranchCode.IsValid) invalidList.Add(BranchCode.InvalidMessage);

            //                // Column Branch
            //                var Branch = helpers.ValidateString("Branch",rows[i,3],true);

            //                if(Branch.IsValid) rowResult.Branch = Branch.Value;
            //                if(!Branch.IsValid) invalidList.Add(Branch.InvalidMessage);

            //                // Column ProvinceID
            //                var ProvinceID = helpers.ValidateString("ProvinceID",rows[i,4],true);

            //                if(ProvinceID.IsValid) rowResult.ProvinceID = ProvinceID.Value;
            //                if(!ProvinceID.IsValid) invalidList.Add(ProvinceID.InvalidMessage);

            //                // Column ProvinceName
            //                var ProvinceName = helpers.ValidateString("ProvinceName",rows[i,5],true);

            //                if(ProvinceName.IsValid) rowResult.ProvinceName = ProvinceName.Value;
            //                if(!ProvinceName.IsValid) invalidList.Add(ProvinceName.InvalidMessage);

            //                // column periodId
            //                var periodResult = GlobalFunction.GetLatestPeriod();
            //                rowResult.Period_Id = periodResult.periodId;

            //                #endregion Validate Column

            //                // Check Valid Row
            //                if(invalidList.Count == 0)
            //                {
            //                    valid.Add(rowResult);
            //                }
            //                else
            //                {
            //                    // TODO: Map valid item to invalid
            //                    var config = new MapperConfiguration(cfg =>
            //                    {
            //                        cfg.CreateMap<D_Area,AreaInvalid>();
            //                    });

            //                    IMapper mapper = config.CreateMapper();
            //                    var dest = mapper.Map<D_Area,AreaInvalid>(rowResult);
            //                    dest.InvalidList = invalidList;

            //                    // Add item to invalid list
            //                    invalid.Add(dest);
            //                }
            //            }
            //        }
            //    }

            //    // TODO: Call webservice to trigger log and update status

            //    if(invalid.Count == 0)
            //    {
            //        var tmpValid = valid.ToList();

            //        #region Check Duplicate

            //        // TODO: Check Duplicate in File
            //        var getDupplidateInFile = valid.GroupBy(x => x.StudyAreaCode.Trim()).Where(x => x.Count() > 1)
            //            .Select(x => x.Key).ToList();

            //        // TODO: Find in valid
            //        foreach(var item in valid.Where(x => getDupplidateInFile.Any(y => y.Equals(x.StudyAreaCode.Trim()))).ToList())
            //        {
            //            // Remove item from valid list
            //            valid.Remove(item);

            //            // Map valid item to invalid
            //            var config = new MapperConfiguration(cfg =>
            //            {
            //                cfg.CreateMap<D_Area,AreaInvalid>();
            //            });

            //            IMapper mapper = config.CreateMapper();
            //            var dest = mapper.Map<D_Area,AreaInvalid>(item);
            //            dest.InvalidList = new List<string>() { "Dupplicate column StudyAreaCode in upload file" };

            //            // Add item to invalid list
            //            invalid.Add(dest);
            //        }

            //        #endregion Check Duplicate

            //    }

            //    // Set data for pageview
            //    ViewBag.isInvalidColumn = false;

            //    ViewBag.Valid = valid;
            //    ViewBag.ValidJson = JsonConvert.SerializeObject(valid);

            //    ViewBag.Invalid = invalid;
            //    ViewBag.InvalidJson = JsonConvert.SerializeObject(invalid);

            //    // Generate key for mapping data
            //    var dataKey = Guid.NewGuid().ToString();

            //    ViewBag.DataKey = dataKey;

            //    // Save valid data to session
            //    Session[dataKey] = valid;

            //    // Session will expire in 30 minutes
            //    Session.Timeout = 30;

            return View("Index");
        }

        [HttpPost]
        public JsonResult Save(string dataKey)
        {
            // Get data from session by datakey
            //var valid = (List<D_Area>)Session[dataKey];

            //EFBatchOperation.For(_context,_context.D_Area).InsertAll(valid);

            //var user = Authorization.GetLoginDetail().EmployeeCode;
            //_context.usp_DatasourceImportedLogManual_Update(periodResult.periodId,"D_Area",user);

            //Session[dataKey] = null;

            return Json("บันทึกข้อมูลเรียบร้อย");
        }
    }
}