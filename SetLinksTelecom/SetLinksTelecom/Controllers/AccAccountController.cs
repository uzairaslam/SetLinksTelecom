using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SetLinksTelecom.Data;

namespace SetLinksTelecom.Controllers
{
    public class AccAccountController : Controller
    {
        private readonly IAccAccount _repo;

        public AccAccountController(IAccAccount repo)
        {
            _repo = repo;
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
    }
}