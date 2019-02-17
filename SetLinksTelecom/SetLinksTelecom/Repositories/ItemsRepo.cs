using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;
using SetLinksTelecom.GeneralFolder;
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
                    join cat in _db.ProductCategories on i.ProductCategoryId equals cat.ProductCategoryId
                    join type in _db.InventoryTypes on cat.InventoryTypeId equals type.InventoryTypeId 
                    where i.ItemId == id
                    select i
                        //new Item
                        //    {
                        //        ItemId =  i.ItemId,
                        //        Name = i.Name,
                        //        ItemCode = i.ItemCode,
                        //        ProductCategoryId = i.ProductCategoryId,
                        //        SaleRate = i.SaleRate
                        //        //ItemId = i.ItemId,
                        //        //Name = i.Name,
                        //        //ItemCode = i.ItemCode,
                        //        ////Subname = i.Subname,
                        //        //ProductCategoryId = i.ProductCategoryId//,
                        //        ////ProductCategory = new ProductCategory
                        //        ////{
                        //        ////    ProductCategoryId = cat.ProductCategoryId,
                        //        ////    Name = cat.Name,
                        //        ////    InventoryType = new InventoryType()
                        //        ////    {
                        //        ////        InventoryTypeId = type.InventoryTypeId,
                        //        ////        Name = type.Name
                        //        ////    }
                        //        ////}
                        //    }
                        ).FirstOrDefault();
            }
            item.ProductCategories = _db.ProductCategories.ToList();
            item.ProductCategory.InventoryTypes = _db.InventoryTypes.ToList();
            return item;
        }

        public void SaveItem(Item item)
        {
            //ProductCategory productCategory = _db.ProductCategories.Include(pc => pc.InventoryType)
            //    .FirstOrDefault(c => c.ProductCategoryId.Equals(item.ProductCategoryId));
            
            ProductCategory category =
                _db.ProductCategories.Single(pc => pc.ProductCategoryId.Equals(item.ProductCategoryId));
            InventoryType type = _db.InventoryTypes.Single(i => i.InventoryTypeId.Equals(category.InventoryTypeId));
            if (type.Name == "Tangible")
            {
                var maxAcc = _db.AccAccounts.Where(acc => acc.HeadCode == 11 && acc.SubHeadCode == 1)
                    .Max(a => (int?)a.AccCode) ?? 0;
                maxAcc = ++maxAcc;
                _db.AccAccounts.Add(new AccAccount
                {
                    HeadCode = 11,
                    SubHeadCode = 01,
                    OID = 0,
                    AccCode = maxAcc,
                    AccMade = 1,
                    AccName = item.Name,
                    AccString = "11-01-" + (maxAcc.ToAccString())
                });
                item.AccString = "11-01-" + (maxAcc.ToAccString());
            }
            else
            {
                var maxAcc = _db.AccAccounts.Where(acc => acc.HeadCode == 11 && acc.SubHeadCode == 2)
                    .Max(a => (int?)a.AccCode) ?? 0;
                maxAcc = ++maxAcc;
                _db.AccAccounts.Add(new AccAccount
                {
                    HeadCode = 11,
                    SubHeadCode = 02,
                    OID = 0,
                    AccCode = maxAcc,
                    AccMade = 1,
                    AccName = item.Name,
                    AccString = "11-02-" + (maxAcc.ToAccString())
                });
                item.AccString = "11-02-" + (maxAcc.ToAccString());
            }
            _db.Items.Add(item);
            //productCategory.it
            //_db.Items.Add(item);
            //_db.Entry(productCategory).State = EntityState.Modified;
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