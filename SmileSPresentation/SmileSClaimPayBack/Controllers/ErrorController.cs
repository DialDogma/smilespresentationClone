using SmileSClaimPayBack.Helper;
using SmileSClaimPayBack.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace SmileSClaimPayBack.Controllers
{
    public class ErrorController : Controller
    {
        private ClaimPayBackEntities _context;
        public ErrorController()
        {
            _context = new ClaimPayBackEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [HttpGet]
        public ActionResult InternalServerError()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NotFound()
        {
            return View();
        }



    }
}
