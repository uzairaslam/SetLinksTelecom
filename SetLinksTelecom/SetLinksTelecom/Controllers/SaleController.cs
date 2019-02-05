using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;

namespace SetLinksTelecom.Controllers
{
    public class SaleController : Controller
    {
        private readonly ISaleRepo _saleRepo;
        private readonly IPurchaseRepo _purchaseRepo;

        public SaleController(ISaleRepo saleRepo, IPurchaseRepo purchaseRepo)
        {
            _saleRepo = saleRepo;
            _purchaseRepo = purchaseRepo;
        }

        // GET: Sale
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddOrEdit(int id = 0)
        {
            //var purchases = _purchaseRepo.GetData()
            //    .Select(s => new
            //    {
            //        Text = s.ItemName + " | " + s.Qty + " | " + s.Rate + " | " + s.Total,
            //        Value = s.PurchaseId
            //    });
            //ViewBag.PurchsesList = new SelectList(purchases, "Value", "Text");
            return View(_saleRepo.GetSale(id));
        }

        [HttpGet]
        public ActionResult TangibleSaleIndex()
        {
            DtoTangibleSale dtoTangibleSale = new DtoTangibleSale();
            dtoTangibleSale.Date = DateTime.Now;
            return View(dtoTangibleSale);
        }

        [HttpGet]
        public ActionResult SaleItemRow(int id)
        {
            //DtoTangibleItemSale itemSaleRow = new DtoTangibleItemSale();
            return View(_purchaseRepo.GetSpecificPurchase(id));
        }

        [HttpPost]
        public ActionResult SaveSale(DtoTangibleSale dtoTangibleSale)
        {
            return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
        }
    }
}