using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace SmileSBankDirectDebit.Controllers
{
    public class TransferFileFTPController : Controller
    {
        // GET: TransferFileFTP
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CallBatchFile()
        {
            return View();
        }

        public ActionResult CallFile()
        {

            Boolean result = false;
            //Process process = Process.Start(@"F:\Console\Test.exe");



            try
            {

                var exePath = Properties.Settings.Default.PathBackupFile;
                using (Process proc = Process.Start(exePath))
                {
                    proc.WaitForExit();
                    result = true;

                }

            }
            catch (Exception e)
            {
                result = false;
            }




            return Json(result, JsonRequestBehavior.AllowGet); ;
        }
    }
}