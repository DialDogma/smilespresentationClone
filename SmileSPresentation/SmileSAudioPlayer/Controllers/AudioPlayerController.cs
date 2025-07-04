using SmileSAudioPlayer.Helper;
using SmileSAudioPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SmileSAudioPlayer.Controllers
{
    //[Authorization]
    public class AudioPlayerController : Controller
    {
        #region dbContext

        private SSBVoiceRecordDBContext _context;
        public AudioPlayerController()
        {
            _context = new SSBVoiceRecordDBContext();
        }
        

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion dbContext
        
        // GET: AudioPlayer
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AudioPlayer(string id = null)
        {
            //set default
            ViewBag.Id = "-";
            ViewBag.AudioURL = "";
            ViewBag.Extension = "-";
            ViewBag.DialNumber = "-";
            ViewBag.DestinationNumber = "-";
            ViewBag.CallTypeName = "-";
            ViewBag.ReceivedDatetime = "-";
            ViewBag.EndDatetime = "-";
            ViewBag.SystemName = "-";
            ViewBag.IsExpired = "";
            if (!(string.IsNullOrEmpty(id)))
            {
                //Decode
                var base64EncodedBytes = Convert.FromBase64String(id);
                var decode = Encoding.UTF8.GetString(base64EncodedBytes);
                string guidStr = decode;
                //Convert string to GUID
                Guid newGuid = Guid.Parse(guidStr);
                //Find id from GUID
                var voiceRecDe = _context.VoiceRecordURLRequests.Where(x => x.Id == newGuid).SingleOrDefault();
                if (voiceRecDe is null) {
                    ViewBag.IsExpired = "File not found";
                    return View();
                }
                int voiceId = Convert.ToInt32(voiceRecDe.VoiceRecordDetailId);
                VoiceRecordDetail voiceRecordDetail = _context.VoiceRecordDetails.Where(x => x.Id == voiceId).SingleOrDefault();
                if (voiceRecordDetail is null)
                {
                    ViewBag.IsExpired = "File not found";
                    return View();
                }
                string apiAddress = _context.VoiceRecordConfigurations.Where(x => x.ParameterName == "InternalAPIAdressFormat").Select(x => x.ValueString).SingleOrDefault();
                if (string.IsNullOrEmpty(apiAddress))
                {
                    ViewBag.IsExpired = "Internal API address not found";
                    return View();
                }
                string apiStr = apiAddress.Replace("{GUID}", newGuid.ToString());
                //test get url is exist
                bool exist = false;
                try
                {
                    HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(apiStr);
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        exist = response.StatusCode == HttpStatusCode.OK;
                    }
                }
                catch
                {
                    apiStr = "";
                    ViewBag.IsExpired = "File not found";
                }

                if (!exist)
                {
                    apiStr = "";
                    ViewBag.IsExpired = "File not found";
                }


                //Define parameters
                ViewBag.Id = guidStr;
                ViewBag.AudioURL = apiStr;
                ViewBag.Extension = voiceRecordDetail.ExtensionNo;
                ViewBag.DialNumber = voiceRecordDetail.PhoneNumberFrom;
                ViewBag.DestinationNumber = voiceRecordDetail.PhoneNumberTo;
                ViewBag.CallTypeName = _context.CallTypes.Where(x => x.Id == voiceRecordDetail.CallTypeId).Select(x => x.Detail).SingleOrDefault();
                ViewBag.ReceivedDatetime = voiceRecordDetail.FileCreateDatetime.ToString("yyyy-MM-dd HH:mm:ss");
                ViewBag.EndDatetime = voiceRecordDetail.FileModifyDatetime.ToString("yyyy-MM-dd HH:mm:ss");
                ViewBag.SystemName = _context.VoiceRecordProviders.Where(x => x.Id == voiceRecordDetail.VoiceRecordProvidersId).Select(x => x.Detail).SingleOrDefault();
                ViewBag.FileName = voiceRecordDetail.FileName;
                if (voiceRecDe.ExpireDatetime <= DateTime.Now)
                { //show text when file is expired
                    ViewBag.IsExpired = "The audio has been expired";
                }
            }
            return View();
        }
       
        public JsonResult IsFileExpire(string Id)
        {
            Guid newGuid = Guid.Parse(Id);
            var expireDT = _context.VoiceRecordURLRequests.Where(x => x.Id == newGuid).Select(x => x.ExpireDatetime).SingleOrDefault();
            if (expireDT <= DateTime.Now)
            {
                return Json(new { IsResult = false, Message = "Video expired" }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { IsResult = true, Message = "Available" }, JsonRequestBehavior.AllowGet);

        }
    }
}