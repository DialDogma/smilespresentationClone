using Newtonsoft.Json;
using RestSharp;
using SmilesSSytemMonitor.Models;
using System;
using System.Linq;
using System.Web.Http;

namespace ServerMonitor.Controllers
{
    public class ServerMonitorController : ApiController
    {
        //private SystemMonitorEntities _context;
        //public ServerMonitorController()
        //{
        //    _context = new SystemMonitorEntities();
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        if (_context != null)
        //        {
        //            _context.Dispose();
        //        }
        //    }
        //    base.Dispose(disposing);
        //}

        #region Context

        private readonly SystemMonitorEntities _context;

        public ServerMonitorController()
        {
            _context = new SystemMonitorEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        /// <summary>
        /// CreateHealthLog
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CreateHealthLog(ServerMonitorForCreate data)
        {
            var result = true;
            try
            {
                var xx = _context.usp_InsertCurrentServerMonitor(data.ServerName, data.Disk_C_Free, data.Disk_D_Free, data.Disk_E_Free, data.Disk_F_Free, data.Disk_C_Total, data.Disk_D_Total, data.Disk_E_Total, data.Disk_F_Total, data.RAM, data.CPU, data.DiskIO);
            }
            catch (System.Exception)
            {
                throw;
            }

            return Json(result);
        }

        /// <summary>
        /// GetHealthLog
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetHealthLog()
        {
            var result = _context.usp_GetCurrentServerMonitor();

            return Json(result);
        }
    }
}