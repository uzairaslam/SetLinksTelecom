using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SetLinksTelecom.Data;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Controllers
{
    public class LineController : Controller
    {
        private readonly ILineRepo _lineRepo;

        public LineController(ILineRepo lineRepo)
        {
            _lineRepo = lineRepo;
        }

        // GET: Line
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            return Json(new { data = _lineRepo.GetLines() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Line());
            else
                return View(_lineRepo.GetLine(id));
        }

        [HttpPost]
        public ActionResult AddOrEdit(Line line)
        {
            try
            {
                if (line.LineId == 0)
                {
                    _lineRepo.SaveLine(line);
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _lineRepo.UpdateLine(line);
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Error in " + (line.LineId == 0 ? "saving" : "updation") + "\n" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}