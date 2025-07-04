using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSDataCenterV2.Models;

namespace SmileSDataCenterV2.Controllers
{
    public class RoleTemplateController:Controller
    {
        private DataCenterV1Entities _context;

        public RoleTemplateController()
        {
            _context = new DataCenterV1Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
    }
}