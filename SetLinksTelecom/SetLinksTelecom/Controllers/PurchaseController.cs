using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;

namespace SetLinksTelecom.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly IPurchaseRepo _purchaseRepo;

        public PurchaseController(IPurchaseRepo purchaseRepo)
        {
            _purchaseRepo = purchaseRepo;
        }

        // GET: Purchase
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PurchaseGrid()
        {
            return View();
        }

        public ActionResult GetData()
        {
            return Json(new {data = _purchaseRepo.GetData()}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            return View(_purchaseRepo.GetPurchase(id));
        }

        [HttpPost]
        public ActionResult AddOrEdit(dtoPurchase dtoPurchase)
        {
            try
            {
                if (dtoPurchase.PurchaseId == 0)
                {
                    _purchaseRepo.SavePurchase(dtoPurchase);
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _purchaseRepo.UpdatePurchase(dtoPurchase);
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Error in " + (dtoPurchase.PurchaseId == 0 ? "saving" : "updation") + "\n" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _purchaseRepo.DeletePurchase(id);
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Error in deletion" + "\n" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult TangiblePurchase(int id =0)
        {
            return View(_purchaseRepo.GetTangiblePurchase(id));
        }
        [HttpPost]
        public ActionResult TangiblePurchase(DtoTangiblePurchase purchase)
        {
            _purchaseRepo.SaveTangiblePurchase(purchase);
            return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
        }
    }
}