using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;

namespace SetLinksTelecom.Controllers
{
    public class SaleReturnController : Controller
    {
        private readonly ISaleRepo _repo;

        public SaleReturnController(ISaleRepo repo)
        {
            _repo = repo;
        }

        // GET: SaleReturn
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaleReturnGrid()
        {
            return View();
        }

        public ActionResult GetData()
        {
            return Json(new { data = _repo.GetDataForReturn() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TangibleSaleReturnIndex()
        {
            DtoTangibleSale dtoTangibleSale = new DtoTangibleSale();
            dtoTangibleSale.Date = DateTime.Now;
            return View(dtoTangibleSale);
        }


    }
}