using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SetLinksTelecom.Data;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Controllers
{
    public class DesignationController : Controller
    {
        private readonly IDesignationRepo _designationRepo;

        public DesignationController(IDesignationRepo designationRepo)
        {
            _designationRepo = designationRepo;
        }

        // GET: Designation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            return Json(new {data = _designationRepo.GetData()}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Designation());
            else
                return View(_designationRepo.GetDesignation(id));
        }

        [HttpPost]
        public ActionResult AddOrEdit(Designation designation)
        {
            try
            {
                if (designation.Id == 0)
                {
                    _designationRepo.SaveDesignation(designation);
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _designationRepo.UpdateDesignation(designation);
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Error in " + (designation.Id == 0 ? "saving" : "updation") + "\n" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _designationRepo.DeleteDesignation(id);
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Error in deletion"+ "\n" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}