using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SmileSPoint.Models;
using SmileSPoint.Helper;

namespace SmileSPoint.Controllers
{
    [Authorization]
    public class PointAccountController : Controller
    {
        private SmilePointEntities _context;

        public PointAccountController()
        {
            _context = new SmilePointEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #region Index

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetDatatable(int draw, int pageSize, int pageStart, string sortField, string orderType, string search)
        {
            var list = _context.SP_PointAccount_Datatable(pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion Index

        #region Add

        public ActionResult Add()
        {
            ViewBag.PointAccountTypes = _context.PointAccountTypes;

            return View();
        }

        [HttpPost]
        public ActionResult Add(FormCollection form)
        {
            var add = new PointAccount
            {
                PointAccountId = Convert.ToInt32(form["PointAccountId"]),
                CreatedById = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID,
                CreatedDate = System.DateTime.Now,
                IsActive = true,
                PointAccountName = form["PointAccountName"],
                PointAccountTypeId = Convert.ToInt32(form["PointAccountTypeId"]),
                ReferenceCode = form["ReferenceCode"],
                ReferenceDetail = form["ReferenceDetail"],
                PointRemain = 0,
                UpdatedById = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID,
                UpdatedDate = System.DateTime.Now
            };

            _context.PointAccounts.Add(add);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        #endregion Add

        #region Edit

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.PointAccountTypes = _context.PointAccountTypes.Where(x => x.IsActive.Value);
            ViewBag.Form = _context.PointAccounts.FirstOrDefault(x => x.PointAccountId == id);

            return View();
        }

        [HttpPost]
        public ActionResult Edit(FormCollection form)
        {
            var id = Convert.ToInt32(form["Id"]);
            var edit = _context.PointAccounts.FirstOrDefault(x => x.PointAccountId == id);

            if (edit != null)
            {
                edit.PointAccountName = form["PointAccountName"];
                edit.PointAccountTypeId = Convert.ToInt32(form["PointAccountTypeId"]);
                edit.ReferenceCode = form["ReferenceCode"];
                edit.ReferenceDetail = form["ReferenceDetail"];
                edit.IsActive = form["IsActive"] == "on";
                edit.UpdatedDate = DateTime.Now;
                edit.UpdatedById = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID; ;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        #endregion Edit

        #region Delete

        public ActionResult Delete(int id)
        {
            var delete = _context.PointAccounts.FirstOrDefault(x => x.PointAccountId == id);

            if (delete != null)
            {
                delete.UpdatedById = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                delete.UpdatedDate = DateTime.Now;
                delete.IsActive = false;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        #endregion Delete

        #region List

        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        #endregion List

        #region Detail

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("List");
            }
            var detail = _context.PointAccounts.FirstOrDefault(x => x.PointAccountId == id);

            if (detail == null)
            {
                return RedirectToAction("List");
            }

            var type = _context.PointAccountTypes.FirstOrDefault(x => x.PointAccountTypeId == detail.PointAccountTypeId).PointAccountTypeName;

            ViewBag.Detail = detail;
            ViewBag.Type = type;
            return View();
        }

        #endregion Detail

        #region Use

        public ActionResult UsePoint(int? id)
        {
            var pointAccount = _context.PointAccounts.Where(x => x.PointAccountId == id).SingleOrDefault();

            ViewBag.AccountDetail = pointAccount;

            ViewBag.AccountTypeName = _context.PointAccountTypes.Where(x => x.PointAccountTypeId == pointAccount.PointAccountTypeId).SingleOrDefault().PointAccountTypeName;

            ViewBag.TransactionGroups = _context.TransactionGroups;

            return View();
        }

        [HttpPost]
        public ActionResult UsePoint(FormCollection form)
        {
            var amount = Convert.ToDouble(form["Amount"]);
            var pointAccountId = Convert.ToInt32(form["PointAccountId"]);
            var transactionTypeId = Convert.ToInt32(form["TransactionTypeId"]);
            var pointAccount = _context.PointAccounts.Where(x => x.PointAccountId == pointAccountId).SingleOrDefault();
            var remark = form["Remark"];

            //Check Validate Point
            if (!(pointAccount.PointRemain.Value >= amount))
            {
                return RedirectToAction("UsePoint", "PointAccount", new { id = pointAccountId, errorText = "จำนวน Point ไม่พอในการหัก" });
            }

            //Create transaction
            _context.usp_CreateTransaction(pointAccountId, transactionTypeId, amount, remark, GlobalFunction.GetLoginDetail(this.HttpContext).User_ID);

            return RedirectToAction("Detail", "PointAccount", new { id = pointAccountId });
        }

        #endregion Use

        #region Synchronize

        [HttpGet]
        public ActionResult SynchronizeAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SynchronizeAccount(FormCollection form)
        {
            _context.usp_PointAccount_Synchronize_SSS(GlobalFunction.GetLoginDetail(this.HttpContext).User_ID);
            ViewBag.Result = "ทำรายการเรียบร้อยแล้ว";
            return View();
        }

        #endregion Synchronize
    }
}