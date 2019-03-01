using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SetLinksTelecom.Data;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Controllers
{
    public class InventoryTypeController : Controller
    {
        private readonly IInventoryType _repoInventoryType;

        public InventoryTypeController(IInventoryType repoInventoryType)
        {
            _repoInventoryType = repoInventoryType;
        }

        // GET: InventoryType
        public ActionResult InventoryType()
        {
            return View();
        }

        public ActionResult GetData()
        {
            return Json(new {data = _repoInventoryType.GetData()}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new InventoryType());
            else
                return View(_repoInventoryType.GetInventoryType(id));
        }

        [HttpPost]
        public ActionResult AddOrEdit(InventoryType inventoryType)
        {
            try
            {
                if (inventoryType.InventoryTypeId == 0)
                {
                    _repoInventoryType.SaveInventoryType(inventoryType);
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _repoInventoryType.UpdateInventoryType(inventoryType);
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Error in " + (inventoryType.InventoryTypeId == 0 ? "saving" : "updation") + "\n" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}