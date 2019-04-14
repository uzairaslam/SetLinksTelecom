using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinqToExcel;
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
        [HttpGet]
        public ActionResult SaleReturnItemRow(int id)
        {
            //DtoTangibleItemSale itemSaleRow = new DtoTangibleItemSale();
            return View(_saleRepo.GetSpecificSaleDetailItem(id));
        }
        [HttpGet]
        public ActionResult SaleInTangibleItemRow(int PurchaseId, int PersonId)
        {
            //DtoTangibleItemSale itemSaleRow = new DtoTangibleItemSale();
            return View(_purchaseRepo.GetSpecificInTangiblePurchase(PurchaseId, PersonId));
        }

        [HttpPost]
        public ActionResult SaveSale(DtoTangibleSale dtoTangibleSale)
        {
            _saleRepo.SaveTangibleSale(dtoTangibleSale);
            return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveInTangibleSale(DtoInTangibleSale dtoInTangibleSale)
        {
            _saleRepo.SaveInTangibleSale(dtoInTangibleSale);
            return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult InTangibleSaleIndex()
        {
            DtoInTangibleSale dtoInTangibleSale = new DtoInTangibleSale();
            dtoInTangibleSale.Date = DateTime.Now;
            return View(dtoInTangibleSale);
        }

        [HttpGet]
        public ActionResult JazzCashExcel()
        {
            DisplayDtoJazzCashExcel model = new DisplayDtoJazzCashExcel();
            return View(model);
        }

        [HttpPost]
        public ActionResult JazzCashExcel(DisplayDtoJazzCashExcel model, HttpPostedFileBase FileUpload)
        {
            if (FileUpload != null)
            {
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string filename = FileUpload.FileName;
                    string targetpath = Server.MapPath("~/Docs/");
                    FileUpload.SaveAs(targetpath + filename);
                    string pathToExcelFile = targetpath + filename;
                    
                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    excelFile.AddMapping<DtoJazzCashExcel>(x => x.TransactionId, "Transaction ID");
                    excelFile.AddMapping<DtoJazzCashExcel>(x => x.MSISDN, "Organization MSISDN");
                    excelFile.AddMapping<DtoJazzCashExcel>(x => x.BalanceBeforeTransaction, "The Balance before Transaction");
                    excelFile.AddMapping<DtoJazzCashExcel>(x => x.TransactionAmount, "Transaction Amount");
                    excelFile.AddMapping<DtoJazzCashExcel>(x => x.BalanceAfterTransaction, "The Balance after Transaction");
                    excelFile.AddMapping<DtoJazzCashExcel>(x => x.TransactionTime, "Transaction Time");
                    excelFile.AddMapping<DtoJazzCashExcel>(x => x.TransactionStatus, "Transaction Status");
                    var worksheetNames = excelFile.GetWorksheetNames();

                    var artistAlbums = (from a in excelFile.Worksheet<DtoJazzCashExcel>(worksheetNames.First()) select a).ToList();
                    model = _saleRepo.SaveJazzCashSales(new DisplayDtoJazzCashExcel
                    {
                        jazzCashExcel = artistAlbums,
                        PurchaseId = model.PurchaseId,
                        ItemName = model.ItemName
                    });


                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }

                    //foreach (DtoJazzCashExcel sale in artistAlbums)
                    //{
                    //    if (sale.TransactionStatus.ToLower().Equals("Completed"))
                    //    {
                    //        if (sale.BalanceBeforeTransaction < sale.BalanceAfterTransaction)
                    //        {

                    //        }
                    //    }
                    //}
                }
            }

            return View(model);
        }
    }
}