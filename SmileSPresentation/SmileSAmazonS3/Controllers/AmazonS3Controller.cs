using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using SmileSAmazonS3.Models;
using System.Globalization;

namespace SmileSAmazonS3.Controllers
{
    public class AmazonS3Controller : Controller
    {
        #region Context

        private readonly S3UploaderSSSEntities db;
        private readonly S3UploaderSmileEntities dbsmile;

        #endregion Context

        public AmazonS3Controller()
        {
            db = new S3UploaderSSSEntities();
            dbsmile = new S3UploaderSmileEntities();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            dbsmile.Dispose();
        }

        // GET: AmazonS3
        public ActionResult Monitor()
        {
            try
            {
                double LocalDiskTotal = 0;
                double LocalDiskAvailable = 0;
                double LocalDiskUsage = 0;
                double sssS3UploadUsage = 0;
                double smileS3UploadUsage = 0;
                int sssS3UploadCountItem = 0;
                int sssS3NotUploadfile = 0;
                int smileS3UploadCountItem = 0;
                int smileS3NotUploadfile = 0;
                DateTime sssrecdt = DateTime.Now;
                DateTime smilerecdt = DateTime.Now;
                var lst = db.usp_Monitor_Select(DateTime.Now.AddDays(-1), DateTime.Now, true).FirstOrDefault();
                if (lst == null)
                {
                    var lsts = (from a in db.Monitor_Log orderby a.CreatedDate descending select a).FirstOrDefault();
                    if (lsts != null)
                    {
                        LocalDiskTotal = (double)lsts.LocalDiskTotal;
                        LocalDiskAvailable = (double)lsts.LocalDiskAvailable;
                        LocalDiskUsage = (double)lsts.LocalDiskUsage;
                        sssS3UploadUsage = (double)lsts.S3UploadUsage;
                        sssS3UploadCountItem = (int)lsts.S3UploadCountItem;
                        sssS3NotUploadfile = (int)lsts.S3NotUploadfile;
                        sssrecdt = (DateTime)lsts.CreatedDate;
                    }
                }
                else
                {
                    LocalDiskTotal = (double)lst.LocalDiskTotal;
                    LocalDiskAvailable = (double)lst.LocalDiskAvailable;
                    LocalDiskUsage = (double)lst.LocalDiskUsage;
                    sssS3UploadUsage = (double)lst.S3UploadUsage;
                    sssS3UploadCountItem = (int)lst.S3UploadCountItem;
                    sssS3NotUploadfile = (int)lst.S3NotUploadfile;
                    sssrecdt = (DateTime)lst.CreatedDate;
                }

                var lst2 = dbsmile.usp_Monitor_Select(DateTime.Now.AddDays(-1), DateTime.Now, false).FirstOrDefault();
                if (lst2 == null)
                {
                    var lsta = (from a in dbsmile.Monitor_Log orderby a.CreatedDate descending select a).FirstOrDefault();
                    if (lsta != null)
                    {
                        smileS3UploadUsage = (double)lsta.S3UploadUsage;
                        smileS3UploadCountItem = (int)lsta.S3UploadCountItem;
                        smileS3NotUploadfile = (int)lsta.S3NotUploadfile;
                        smilerecdt = (DateTime)lsta.CreatedDate;
                    }
                }
                else
                {
                    smileS3UploadUsage = (double)lst2.S3UploadUsage;
                    smileS3UploadCountItem = (int)lst2.S3UploadCountItem;
                    smileS3NotUploadfile = (int)lst2.S3NotUploadfile;
                    smilerecdt = (DateTime)lst2.CreatedDate;
                }

                //Server Disk Space
                ViewBag.spacetotal = convertSize(LocalDiskTotal);
                ViewBag.spacefree = convertSize(LocalDiskAvailable);
                ViewBag.spaceusage = convertSize(LocalDiskUsage);
                ViewBag.spacepercent = Math.Round(((LocalDiskUsage / LocalDiskTotal) * 100), 2);

                //SSSDoc
                ViewBag.s3size_SSSDoc = convertSize(sssS3UploadUsage);
                ViewBag.s3file_SSSDoc = string.Format("{0:n0}", sssS3UploadCountItem) + " Files";
                ViewBag.s3fileUnup_SSSSDoc = string.Format("{0:n0}", sssS3NotUploadfile) + " Files";
                ViewBag.s3sssdocpercent = Math.Round(((double)sssS3UploadCountItem / (double)(sssS3NotUploadfile + sssS3UploadCountItem)) * 100, 2);
                ViewBag.updatedt = (sssrecdt).ToString("dd/MM/yyyy HH:mm");

                ///SmileDoc
                ViewBag.s3size_SmileDoc = convertSize(smileS3UploadUsage);
                ViewBag.s3file_SmileDoc = string.Format("{0:n0}", smileS3UploadCountItem) + " Files";
                ViewBag.s3fileUnup_SmileSDoc = string.Format("{0:n0}", smileS3NotUploadfile) + " Files";
                ViewBag.s3smiledocpercent = Math.Round(((double)smileS3UploadCountItem / (double)(smileS3NotUploadfile + smileS3UploadCountItem)) * 100, 2);
                ViewBag.updatedt2 = (smilerecdt).ToString("dd/MM/yyyy HH:mm");
            }
            catch (Exception)
            {
                //
            }

            return View();
        }

        private JsonResult DoTest(string id)
        {
            var rs = db.usp_Monitor_Select(DateTime.Now.AddDays(-1), DateTime.Now, true).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLocalSpacePerDay(string dtstart, string dtend)
        {
            string[,] str = null;
            //Array[][] ss = null;
            List<usp_Monitor_Select_Result> lst = new List<usp_Monitor_Select_Result>();
            DateTime? EndDt;
            DateTime? StartDt;
            if (dtstart != "")
            {
                string[] s_Start = dtstart.Split('/');

                StartDt = Convert.ToDateTime(s_Start[0] + "-" + s_Start[1] + "-" + s_Start[2]);
                if (dtend != "")
                {
                    string[] s_End = dtend.Split('/');
                    EndDt = Convert.ToDateTime(s_End[0] + "-" + s_End[1] + "-" + s_End[2]);
                }
                else { EndDt = DateTime.Now; }

                List<string> lll = new List<string>();
                if (false)
                {
                    var lst1 = dbsmile.usp_Monitor_Select(StartDt, EndDt, true).ToList();
                    return Json(lst1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var lst2 = db.usp_Monitor_Select(StartDt, EndDt, true).ToList();
                    return Json(lst2, JsonRequestBehavior.AllowGet);
                }

                //lst = db.usp_Monitor_Select(StartDt, EndDt, true).ToList();

                //lll.Add(l2.ToString());
                //lll.Add(lst.ToString());
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDBUploadCount()
        {
            string[] result = new string[2];
            int sss_up = 0;
            int smile_up = 0;
            int sss_noup = 0;
            int smile_noup = 0;
            var SSS_res = (from a in db.Monitor_Log orderby a.CreatedDate descending select a).FirstOrDefault();
            var Smile_res = (from a in dbsmile.Monitor_Log orderby a.CreatedDate descending select a).FirstOrDefault();
            if (SSS_res != null)
            {
                sss_up = Convert.ToInt32(SSS_res.S3Uploadfile.Value);
                sss_noup = Convert.ToInt32(SSS_res.S3NotUploadfile.Value);
            }
            if (Smile_res != null)
            {
                smile_up = Convert.ToInt32(Smile_res.S3Uploadfile.Value);
                smile_noup = Convert.ToInt32(Smile_res.S3NotUploadfile.Value);
            }
            result[0] = (sss_up + smile_up).ToString();
            result[1] = (sss_noup + smile_noup).ToString();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCountUploadAll(string dtstart, string dtend)
        {
            string[] str = null;
            DateTime? EndDt;
            DateTime? StartDt;
            if (dtstart != "")
            {
                string[] s_Start = dtstart.Split('/');

                StartDt = Convert.ToDateTime(s_Start[0] + "-" + s_Start[1] + "-" + s_Start[2]);
                if (dtend != "")
                {
                    string[] s_End = dtend.Split('/');
                    EndDt = Convert.ToDateTime(s_End[0] + "-" + s_End[1] + "-" + s_End[2]);
                }
                else { EndDt = DateTime.Now; }

                var SSS_res = db.usp_Monitor_Select(StartDt, EndDt, true).ToList();
                var Smile_res = dbsmile.usp_Monitor_Select(StartDt, EndDt, false).ToList();

                if (SSS_res != null && Smile_res != null)
                {
                    if (SSS_res.Count > 0 || Smile_res.Count > 0)
                    {
                        if (SSS_res.Count > Smile_res.Count)
                        {
                            str = new string[SSS_res.Count];
                            for (int i = 0; i < SSS_res.Count; i++)
                            {
                                if (i < Smile_res.Count)
                                {
                                    if (Smile_res[i] != null)
                                    {
                                        str[i] = (SSS_res[i].S3UploadCountItem.Value + Smile_res[i].S3UploadCountItem.Value).ToString();
                                    }
                                    else
                                    {
                                        if (SSS_res[i] != null)
                                        { str[i] = SSS_res[i].S3UploadCountItem.Value.ToString(); }
                                    }
                                }
                                else
                                {
                                    if (SSS_res[i] != null)
                                    { str[i] = SSS_res[i].S3UploadCountItem.Value.ToString(); }
                                }
                            }
                        }
                        else
                        {
                            str = new string[Smile_res.Count];
                            for (int i = 0; i < Smile_res.Count; i++)
                            {
                                if (i <= SSS_res.Count - 1)
                                {
                                    if (Smile_res[i] != null)
                                    {
                                        str[i] = (Smile_res[i].S3UploadCountItem.Value + SSS_res[i].S3UploadCountItem.Value).ToString();
                                    }
                                    else
                                    {
                                        if (SSS_res[i] != null)
                                        { str[i] = Smile_res[i].S3UploadCountItem.Value.ToString(); }
                                    }
                                }
                                else
                                {
                                    if (SSS_res[i] != null)
                                    { str[i] = Smile_res[i].S3UploadCountItem.Value.ToString(); }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (SSS_res != null)
                    {
                        str = new string[SSS_res.Count];
                        for (int i = 0; i < SSS_res.Count; i++)
                        {
                            if (SSS_res[i] != null)
                                str[i] = SSS_res[i].S3UploadCountItem.Value.ToString();
                        }
                    }
                    else if (Smile_res != null)
                    {
                        str = new string[Smile_res.Count];
                        for (int i = 0; i < Smile_res.Count; i++)
                        {
                            if (Smile_res[i] != null)
                                str[i] = Smile_res[i].S3UploadCountItem.Value.ToString();
                        }
                    }
                }
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        private String convertSize(double size)
        {
            String[] units = new String[] { " B", " KB", " MB", " GB", " TB", " PB" };

            double mod = 1024.0;

            int i = 0;

            while (size >= mod)
            {
                size /= mod;
                i++;
            }
            return Math.Round(size, 2) + units[i];//with 2 decimals
        }
    }
}