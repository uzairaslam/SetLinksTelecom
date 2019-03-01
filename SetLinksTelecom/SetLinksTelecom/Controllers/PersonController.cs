using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonRepo _personRepo;
        private readonly IDesignationRepo _designationRepo;
        private readonly ILineRepo _lineRepo;

        public PersonController(IPersonRepo personRepo, IDesignationRepo designationRepo, ILineRepo lineRepo)
        {
            _personRepo = personRepo;
            _designationRepo = designationRepo;
            _lineRepo = lineRepo;
        }

        // GET: Person
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData(int BossId = 0, int DesignationId = 0, string withoutBoss = "")
        {
            IList<Person> persons = _personRepo.GetData(BossId, DesignationId);
            return Json(new { data = persons }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PersonGrid()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                Person person = new Person();
                person.Designations = _designationRepo.GetData().ToList();
                person.Lines = _lineRepo.GetLines().ToList();
                person.CNICIssueDate = DateTime.Now;
                person.DOB = DateTime.Now;
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

        [HttpGet]
        public ActionResult PersonMapping()
        {
            DtoPersonMapping mapping = new DtoPersonMapping();
            mapping.Designations = _designationRepo.GetData().ToList();
            return View(mapping);
        }
    }
}