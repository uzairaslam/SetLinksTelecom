using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;

namespace SetLinksTelecom.Controllers
{
    public class AccAccountController : Controller
    {
        private readonly IAccAccount _repo;
        private readonly IAccVoucher _voucher;

        public AccAccountController(IAccAccount repo, IAccVoucher voucher)
        {
            _repo = repo;
            _voucher = voucher;
        }

        // GET: AccAccount
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AccountGrid()
        {
            return View();
        }

        public ActionResult GetData()
        {
            var accounts = _repo.GetData();
            return Json(new { data = accounts }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public ActionResult JvEntry()
        {
            DtoJvEntry jvEntry = _voucher.GetJvEntry();
            return View(jvEntry);
        }

        [HttpPost]
        public ActionResult JvEntry(DtoSaveJv dtoSaveJv)
        {
            _voucher.SaveJv(dtoSaveJv);
            return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
        }

    }
}