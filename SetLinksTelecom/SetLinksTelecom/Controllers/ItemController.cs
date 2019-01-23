using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SetLinksTelecom.Data;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemsRepo _itemsRepo;

        public ItemController(IItemsRepo itemsRepo)
        {
            _itemsRepo = itemsRepo;
        }

        // GET: Item
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            return Json(new { data = _itemsRepo.GetData().ToList() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            return View(_itemsRepo.GetItem(id));
        }

        [HttpPost]
        public ActionResult AddOrEdit(Item item)
        {
            //try
            //{
                if (item.ItemId == 0)
                {
                    _itemsRepo.SaveItem(item);
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                    //return View(item);
                }
                else
                {
                    _itemsRepo.UpdateItem(item);
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            //}
            //catch (Exception e)
            //{
            //    return Json(new { success = false, message = "Error in " + (item.ItemId == 0 ? "saving" : "updation") + "\n" + e.Message }, JsonRequestBehavior.AllowGet);
            //}
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _itemsRepo.DeleteItem(id);
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Error in deletion" + "\n" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}