using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmileSMiscellaneous.Models;

namespace SmileSMiscellaneous.Helper
{
    public class JsonLog
    {
        public string GetJsonOutputByApplicationId(int applicationID)
        {
            var appFullDetail = new ApplicationFullDetail();
            List<usp_ApplicationHeir_Select_Result> HeirDetail = new List<usp_ApplicationHeir_Select_Result>();

            using (var db = new MiscellaneousDBContext())
            {
                //HeirDetail heirdetail = null;
                var app = db.usp_ApplicationFullDetail_Select(applicationID, 0, 999, null, null, null).Single();

                appFullDetail.DataAppDetail = app;

                var heir = db.usp_ApplicationHeir_Select(applicationID, 0, 999, null, null, null).ToList();

                if (heir.Count > 0) 
                {

                    //var heirdetail = new HeirDetail();

                    //heirdetail.DataHeir = heir;
                    appFullDetail.DataHeirDetail = heir;

                }
            }

            var jsonOutput = Newtonsoft.Json.JsonConvert.SerializeObject(appFullDetail);

            return jsonOutput;
        }
    }

   


    public class ApplicationFullDetail
    {
        public usp_ApplicationFullDetail_Select_Result DataAppDetail { get; set; }
        public List<usp_ApplicationHeir_Select_Result> DataHeirDetail { get; set; }  
    }


    public class HeirDetail
    {
        public List<usp_ApplicationHeir_Select_Result> DataHeir { get; set; }

        //public HeirDetail(List<string> dataHeir)  
        //{
        //    DataHeir = dataHeir;
        //}
        
    }
}