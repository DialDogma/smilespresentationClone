using SmileSBankDirectDebit.Helper;
using SmileSBankDirectDebit.Models;
using System.Globalization;
using System.Web.Mvc;

namespace SmileSBankDirectDebit.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        #region Context

        private readonly BankDirectDebitDBContext _context;
        private static readonly CultureInfo _dtEn = new CultureInfo("en-US");

        public HomeController()

        {
            _context = new BankDirectDebitDBContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AnotherLink()
        {
            return View("Index");
        }

        public ActionResult AddBankDirectDebitHeader()
        {
            return View();
        }



    }
}