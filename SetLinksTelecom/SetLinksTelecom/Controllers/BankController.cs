using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SetLinksTelecom.Data;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Controllers
{
    public class BankController : Controller
    {
        private readonly IBanksRepo _repo;

        public BankController(IBanksRepo repo)
        {
            _repo = repo;
        }

        // GET: Bank
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            return Json(new { data = _repo.GetData() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int BankId = 0)
        {
            if (BankId == 0)
                return View(new Bank());
            else
                return View(_repo.GetBank(BankId));
        }
    }
}