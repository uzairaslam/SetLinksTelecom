using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SetLinksTelecom.Data;

namespace SetLinksTelecom.Controllers
{
    public class VoucherController : Controller
    {
        private readonly IAccVoucher _repo;

        public VoucherController(IAccVoucher repo)
        {
            _repo = repo;
        }

        // GET: Voucher
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult VoucherGrid()
        {
            return View();
        }
        public ActionResult GetData()
        {
            var accounts = _repo.GetData();
            return Json(new { data = accounts }, JsonRequestBehavior.AllowGet);
        }
    }
}