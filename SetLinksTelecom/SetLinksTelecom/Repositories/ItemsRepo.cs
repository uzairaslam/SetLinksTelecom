using System;
using System.Collections;
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
            using (var transaction = _db.Database.BeginTransaction())
            {
                ProductCategory category =
                    _db.ProductCategories.Single(pc => pc.ProductCategoryId.Equals(item.ProductCategoryId));
                InventoryType type = _db.InventoryTypes.Single(i => i.InventoryTypeId.Equals(category.InventoryTypeId));



                if (type.Name == "Tangible")
                {
                    var maxAcc = _db.AccAccounts.Where(acc => acc.HeadCode == 11 && acc.SubHeadCode == 1)
                                     .Max(a => (int?) a.AccCode) ?? 0;
                    ++maxAcc;
                    var maxRevAcc = _db.AccAccounts.Where(acc => acc.HeadCode == 41 && acc.SubHeadCode == 1)
                                        .Max(a => (int?) a.AccCode) ?? 0;
                    var maxCosAcc = _db.AccAccounts.Where(acc => acc.HeadCode == 51 && acc.SubHeadCode == 1)
                                        .Max(a => (int?) a.AccCode) ?? 0;
                    ++maxRevAcc;
                    ++maxCosAcc;
                    var purDisc = _db.AccAccounts.Where(acc => acc.HeadCode == 42 && acc.SubHeadCode == 1)
                                      .Max(a => (int?) a.AccCode) ?? 0;
                    ++purDisc;
                    var saleComm = _db.AccAccounts.Where(acc => acc.HeadCode == 43 && acc.SubHeadCode == 1)
                                       .Max(a => (int?) a.AccCode) ?? 0;
                    ++saleComm;
                    var CRev = _db.AccAccounts.Where(acc => acc.HeadCode == 44 && acc.SubHeadCode == 1)
                                       .Max(a => (int?) a.AccCode) ?? 0;
                    ++CRev;

                    IList<AccAccount> accounts = new List<AccAccount>()
                    {
                        //Value
                        new AccAccount
                        {
                            HeadCode = 11, SubHeadCode = 01, OID = 0, AccCode = maxAcc, AccMade = 1,
                            AccName = item.Name, AccString = "11-01-" + (maxAcc.ToAccString())
                        },
                        //Revenue
                        new AccAccount
                        {
                            HeadCode = 41, SubHeadCode = 01, OID = 0, AccCode = maxRevAcc, AccMade = 1,
                            AccName = item.Name, AccString = "41-01-" + (maxRevAcc.ToAccString())
                        },
                        //Cost
                        new AccAccount
                        {
                            HeadCode = 51, SubHeadCode = 01, OID = 0, AccCode = maxCosAcc, AccMade = 1,
                            AccName = item.Name, AccString = "51-01-" + (maxCosAcc.ToAccString())
                        },
                        //Purchase Discount
                        new AccAccount
                        {
                            HeadCode = 42, SubHeadCode = 01, OID = 0, AccCode = purDisc, AccMade = 1,
                            AccName = item.Name, AccString = "42-01-" + (purDisc.ToAccString())
                        },
                        //Sale Commission
                        new AccAccount
                        {
                            HeadCode = 43, SubHeadCode = 01, OID = 0, AccCode = saleComm, AccMade = 1,
                            AccName = item.Name, AccString = "43-01-" + (saleComm.ToAccString())
                        },
                        //Contra Revenue
                        new AccAccount
                        {
                            HeadCode = 44, SubHeadCode = 01, OID = 0, AccCode = saleComm, AccMade = 1,
                            AccName = item.Name, AccString = "44-01-" + (CRev.ToAccString())
                        }
                    };

                    //_db.AccAccounts.Add();
                    _db.AccAccounts.AddRange(accounts);
                    item.AccString = "11-01-" + (maxAcc.ToAccString());
                    item.RevString = "41-01-" + maxRevAcc.ToAccString();
                    item.CosString = "51-01-" + maxCosAcc.ToAccString();
                    item.PurDiscString = "42-01-" + (purDisc.ToAccString());
                    item.SaleCommString = "43-01-" + (saleComm.ToAccString());
                    item.CRevString = "44-01-" + (CRev.ToAccString());
                }
                else
                {
                    var maxAcc = _db.AccAccounts.Where(acc => acc.HeadCode == 11 && acc.SubHeadCode == 2)
                                     .Max(a => (int?) a.AccCode) ?? 0;
                    maxAcc = ++maxAcc;
                    var maxRevAcc = _db.AccAccounts.Where(acc => acc.HeadCode == 41 && acc.SubHeadCode == 2)
                                        .Max(a => (int?) a.AccCode) ?? 0;
                    var maxCosAcc = _db.AccAccounts.Where(acc => acc.HeadCode == 51 && acc.SubHeadCode == 2)
                                        .Max(a => (int?) a.AccCode) ?? 0;
                    maxRevAcc = ++maxRevAcc;
                    maxCosAcc = ++maxCosAcc;
                    var purDisc = _db.AccAccounts.Where(acc => acc.HeadCode == 42 && acc.SubHeadCode == 2)
                                      .Max(a => (int?) a.AccCode) ?? 0;
                    ++purDisc;
                    var saleComm = _db.AccAccounts.Where(acc => acc.HeadCode == 43 && acc.SubHeadCode == 2)
                                       .Max(a => (int?) a.AccCode) ?? 0;
                    ++saleComm;
                    var CRev = _db.AccAccounts.Where(acc => acc.HeadCode == 44 && acc.SubHeadCode == 2)
                                   .Max(a => (int?)a.AccCode) ?? 0;
                    ++CRev;

                    IList<AccAccount> accounts = new List<AccAccount>()
                    {
                        //Value
                        new AccAccount
                        {
                            HeadCode = 11, SubHeadCode = 02, OID = 0, AccCode = maxAcc, AccMade = 1,
                            AccName = item.Name, AccString = "11-02-" + (maxAcc.ToAccString())
                        },
                        //Revenue
                        new AccAccount
                        {
                            HeadCode = 41, SubHeadCode = 02, OID = 0, AccCode = maxRevAcc, AccMade = 1,
                            AccName = item.Name, AccString = "41-02-" + (maxRevAcc.ToAccString())
                        },
                        //Cost
                        new AccAccount
                        {
                            HeadCode = 51, SubHeadCode = 02, OID = 0, AccCode = maxCosAcc, AccMade = 1,
                            AccName = item.Name, AccString = "51-02-" + (maxCosAcc.ToAccString())
                        },
                        //Purchase Discount
                        new AccAccount
                        {
                            HeadCode = 42, SubHeadCode = 02, OID = 0, AccCode = purDisc, AccMade = 1,
                            AccName = item.Name, AccString = "42-02-" + (purDisc.ToAccString())
                        },
                        //Sale Commission
                        new AccAccount
                        {
                            HeadCode = 43, SubHeadCode = 02, OID = 0, AccCode = saleComm, AccMade = 1,
                            AccName = item.Name, AccString = "43-02-" + (saleComm.ToAccString())
                        },
                        //Contra Revenue
                        new AccAccount
                        {
                            HeadCode = 44, SubHeadCode = 02, OID = 0, AccCode = saleComm, AccMade = 1,
                            AccName = item.Name, AccString = "44-02-" + (CRev.ToAccString())
                        }
                    };

                    //_db.AccAccounts.Add();
                    _db.AccAccounts.AddRange(accounts);
                    item.AccString = "11-02-" + (maxAcc.ToAccString());
                    item.RevString = "41-02-" + maxRevAcc.ToAccString();
                    item.CosString = "51-02-" + maxCosAcc.ToAccString();
                    item.PurDiscString = "42-02-" + (purDisc.ToAccString());
                    item.SaleCommString = "43-02-" + (saleComm.ToAccString());
                    item.CRevString = "44-02-" + (CRev.ToAccString());
                }

                _db.Items.Add(item);
                _db.SaveChanges();

                transaction.Commit();
            }
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