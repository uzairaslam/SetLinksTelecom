using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SetLinksTelecom.Data;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Controllers
{
    public class BVSController : Controller //Biometric verified system
    {
        private readonly IBvsRepo _repo;

        public BVSController(IBvsRepo repo)
        {
            _repo = repo;
        }

        // GET: BVS
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetData()
        {
            return Json(new { data = _repo.GetData() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new BvsService());
            else
                return View(_repo.GetBVS(id));
        }

        [HttpPost]
        public ActionResult AddOrEdit(BvsService bvsService)
        {
            try
            {
                _repo.Save(bvsService);
                return Json(new { success = true, message = bvsService.BvsServiceId == 0 ? "Saved Successfully" : "Updated Successfully" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Error in " + (bvsService.BvsServiceId == 0 ? "saving" : "updation") + "\n" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}