using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SetLinksTelecom.Data;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonRepo _personRepo;
        private readonly IDesignationRepo _designationRepo;

        public PersonController(IPersonRepo personRepo, IDesignationRepo designationRepo)
        {
            _personRepo = personRepo;
            _designationRepo = designationRepo;
        }

        // GET: Person
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            var persons = _personRepo.GetData();
            return Json(new { data = persons }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                Person person = new Person();
                person.Designations = _designationRepo.GetData().ToList();
                return View(person);
            }
            else
                return View(_personRepo.GetPerson(id));
        }

        [HttpPost]
        public ActionResult AddOrEdit(Person person)
        {
            try
            {
                //_personRepo.SavePerson(person);
                //return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                if (person.PersonId == 0)
                {
                    _personRepo.SavePerson(person);
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _personRepo.UpdatePerson(person);
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Error in " + (person.PersonId == 0 ? "saving" : "updation") + "\n" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _personRepo.DeletePerson(id);
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Error in deletion" + "\n" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}