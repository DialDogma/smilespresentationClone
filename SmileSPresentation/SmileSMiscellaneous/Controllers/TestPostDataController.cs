using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmileSMiscellaneous.Controllers
{
    public class DocumentDataList
    {
        public string documentId { get; set; }
        public string code { get; set; }
        public string documentDetail { get; set; }
        public string mainIndex { get; set; }
        public int? documentSubType { get; set; }
    }
    
    public class DocumentPreviewDataList
    {
        public string documentId { get; set; }
        public string code { get; set; }
    }

    public class DocumentPreviewResult
    {
         public DocumentDetail data { get; set; }
    }
    public class DocumentDetail
    {
        public string documentId { get; set; }
        public string documentCode { get; set; }
        public string mainIndex { get; set; }
        public string searchIndex { get; set; }
        public string documentTypeName { get; set; }
        public string documentSubTypeName { get; set; }
        public int fileCount { get; set; }
    }

    public class TestPostDataController : Controller
    {
        // GET: TestPostData
        public ActionResult TestPostData()
        {
            return View();
        }

        public JsonResult GetDocumentDetailList(int? draw = null)
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            String r2 = generator.Next(0, 1000000).ToString("D6");
            
            var result = new List<DocumentDataList>();
            result.Add(new DocumentDataList { documentId = Guid.NewGuid().ToString(), code = "MVC"+r, documentDetail = "Test1", documentSubType = 3 ,mainIndex="MVC_MainIndex"});
            result.Add(new DocumentDataList { documentId = Guid.NewGuid().ToString(), code = "MVC"+r2, documentDetail = "Test2", documentSubType = 3, mainIndex = "Test_MainIndex" });

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count},
                {"recordsFiltered", result.Count},
                {"data", result}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDocumentFilePreview(int? draw = null)
        {
            var result = new List<DocumentPreviewDataList>();
            result.Add(new DocumentPreviewDataList { documentId = "E8380515-A849-4AFA-B3BF-A866C546A8F5", code = "DC20222201"});
            result.Add(new DocumentPreviewDataList { documentId = "96F2FEA1-899D-4FBD-B3BA-017146E9282F", code = "DC660800000005"});
            result.Add(new DocumentPreviewDataList { documentId = Guid.NewGuid().ToString(), code = "TestDoc00001"});

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count},
                {"recordsFiltered", result.Count},
                {"data", result}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFileCountDocStorege(string documentId)
        {
            var documentPreview = new DocumentPreviewResult();
            var apiUrl = "https://docstorageapi.uatsiamsmile.com/api/document/";
            string url = String.Format(apiUrl + documentId);

            try
            {
                var client = new RestClient();
                var request = new RestRequest(url);

                // request.AddHeader("Authorization", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwcm9qZWN0aWQiOjl9.XX7R_Ik8pydv2e04ZvrDew8tlszSDrjvTEYKdgO4t7A");

                var response = client.ExecuteAsync(request);
                documentPreview = JsonConvert.DeserializeObject<DocumentPreviewResult>(response.Result.Content);

                return Json(documentPreview.data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


    }
   
}