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
    public class ItemsRepo : IItemsRepo
    {
        private readonly DataContext _db;

        public ItemsRepo(DataContext db)
        {
            _db = db;
        }
        public IEnumerable<dtoItem> GetData()
        {
            //return _db.Items.Include(i => i.ProductCategory).Include(pc => pc.ProductCategories.Select(x => x.InventoryType)).ToList();
            return (from i in _db.Items
                join cat in _db.ProductCategories on i.ProductCategoryId equals cat.ProductCategoryId
                join type in _db.InventoryTypes on cat.InventoryTypeId equals type.InventoryTypeId 
                select new dtoItem
                {
                    ItemId = i.ItemId,
                    Name = i.Name,
                    ItemCode = i.ItemCode,
                    //Subname = i.Subname,
                    ProductCategoryId = i.ProductCategoryId,
                    ProductCategoryName = cat.Name,
                    InventoryTypeId = type.InventoryTypeId,
                    InventoryTypeName = type.Name,
                    SaleRate = i.SaleRate
                }).ToList();
        }

        public Item GetItem(int id)
        {
            var item = new Item();
            if (id != 0)
            {
                //item = _db.Items.Include(i => i.ProductCategories.Select(pc => pc.InventoryTypes)).FirstOrDefault(d => d.ItemId.Equals(id));
                item = (from i in _db.Items
                    where i.ItemId == id
                    select new Item
                    {
                        ItemId = i.ItemId,
                        Name = i.Name,
                        ItemCode = i.ItemCode,
                        //Subname = i.Subname,
                        ProductCategoryId = i.ProductCategoryId,
                        ProductCategory = new ProductCategory
                        {
                            ProductCategoryId = i.ProductCategory.ProductCategoryId,
                            Name = i.ProductCategory.Name,
                            InventoryType = new InventoryType()
                            {
                                InventoryTypeId = i.ProductCategory.InventoryTypeId,
                                Name = i.ProductCategory.InventoryType.Name
                            }
                        }
                    }).FirstOrDefault();
            }
            item.ProductCategories = _db.ProductCategories.ToList();
            item.ProductCategory.InventoryTypes = _db.InventoryTypes.ToList();
            return item;
        }

        public void SaveItem(Item item)
        {
            ProductCategory productCategory = _db.ProductCategories.Include(pc => pc.InventoryType)
                .FirstOrDefault(c => c.ProductCategoryId.Equals(item.ProductCategoryId));
            productCategory.Items.Add(item);
            //productCategory.it
            //_db.Items.Add(item);
            _db.Entry(productCategory).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void UpdateItem(Item item)
        {
            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void DeleteItem(int id)
        {
            Item item = _db.Items.FirstOrDefault(d => d.ItemId.Equals(id));
            _db.Items.Remove(item);
            _db.SaveChanges();
        }
    }
}