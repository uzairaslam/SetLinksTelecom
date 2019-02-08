using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SetLinksTelecom.Data;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryRepo _repo;

        public ProductCategoryController(IProductCategoryRepo repo)
        {
            _repo = repo;
        }

        // GET: ProductCategory
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            return Json(new { data = _repo.GetData() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            return View(_repo.GetProductCategory(id));
        }

        [HttpPost]
        public ActionResult AddOrEdit(ProductCategory productCategory)
        {
            try
            {
                if (productCategory.ProductCategoryId == 0)
                {
                    _repo.SaveProductCategory(productCategory);
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _repo.UpdateProductCategory(productCategory);
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Error in " + (productCategory.ProductCategoryId == 0 ? "saving" : "updation") + "\n" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}