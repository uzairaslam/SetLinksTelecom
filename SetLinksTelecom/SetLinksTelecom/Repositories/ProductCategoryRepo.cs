using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Repositories
{
    public class ProductCategoryRepo : IProductCategoryRepo
    {
        private readonly DataContext _db;

        public ProductCategoryRepo(DataContext db)
        {
            _db = db;
        }
        public List<DtoProductCategory> GetData()
        {
            return (from pc in _db.ProductCategories
                join it in _db.InventoryTypes on pc.InventoryTypeId equals it.InventoryTypeId
                    select new DtoProductCategory
                {
                    InventoryTypeId = pc.InventoryTypeId,
                    InventoryTypeName = it.Name,
                    ProductCategoryId = pc.ProductCategoryId,
                    ProductCategoryName = pc.Name
                }).ToList();
        }

        public ProductCategory GetProductCategory(int id)
        {
            ProductCategory productCategory = new ProductCategory();
            productCategory = id == 0
                ? new ProductCategory()
                : _db.ProductCategories.FirstOrDefault(pc => pc.ProductCategoryId.Equals(id));
            productCategory.InventoryTypes = _db.InventoryTypes.ToList();
            return productCategory;
            
        }

        public void SaveProductCategory(ProductCategory productCategory)
        {
            _db.ProductCategories.Add(productCategory);
            _db.SaveChanges();
        }

        public void UpdateProductCategory(ProductCategory productCategory)
        {
            _db.Entry(productCategory).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}