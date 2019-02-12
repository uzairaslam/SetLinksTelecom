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
    public class PurchaseRepo : IPurchaseRepo
    {
        private readonly DataContext _db;

        public PurchaseRepo(DataContext db)
        {
            _db = db;
        }
        public IEnumerable<dtoDisplayPurchase> GetData()
        {
            IEnumerable<dtoDisplayPurchase> purchases = from A in (
                    (from p in _db.Purchases
                        join po in _db.Portals on p.PortalId equals po.PortalId
                        join i in _db.Items on p.ItemId equals i.ItemId
                        join cat in _db.ProductCategories on i.ProductCategoryId equals cat.ProductCategoryId
                        select new
                        {
                            PortalName = po.Name,
                            CategoryName = cat.Name,
                            ItemName = i.Name,
                            InventoryTypeId = cat.InventoryTypeId,
                            p.PurchaseId,
                            Subname = p.Subname,
                            Remarks = p.Remarks,
                            Qty = p.Qty,
                            Total = p.Total,
                            Percentage = p.Percentage,
                            Rate = p.Rate
                        }))
                join t in _db.InventoryTypes on new {InventoryTypeId = A.InventoryTypeId} equals new
                    {InventoryTypeId = t.InventoryTypeId}
                select new dtoDisplayPurchase
                {
                    InventoryType = t.Name,
                    PortalName = A.PortalName,
                    CategoryName = A.CategoryName,
                    ItemName = A.ItemName,
                    //InventoryTypeId = (int?)A.InventoryTypeId,
                    PurchaseId = A.PurchaseId,
                    Subname = A.Subname,
                    Remarks = A.Remarks,
                    Qty = A.Qty,
                    Total = A.Total,
                    Percentage = A.Percentage,
                    Rate = A.Rate
                };
            return purchases;
            //return from p in _db.Purchases
            //       join po in _db.Portals on p.PortalId equals po.PortalId
            //    join i in _db.Items on p.ItemId equals i.ItemId
            //    select new dtoDisplayPurchase
            //    {
            //        PortalName = po.Name,
            //        InventoryType = i.ProductCategory.InventoryType.Name,
            //        CategoryName = i.ProductCategory.Name,
            //        ItemName = i.Name,
            //        PurchaseId = p.PurchaseId,
            //        Subname = p.Subname,
            //        Remarks = p.Remarks,
            //        Qty = p.Qty,
            //        Total = p.Total,
            //        Percentage = p.Percentage,
            //        Rate = p.Rate
            //    };
        }

        public dtoPurchase GetPurchase(int id)
        {
            dtoPurchase purchase = new dtoPurchase();
            purchase.DatePurchased = DateTime.Now;
            if (id != 0)
            {
                purchase = (from p in _db.Purchases
                    join i in _db.Items on p.ItemId equals i.ItemId
                    where p.PurchaseId == id
                    select new dtoPurchase
                    {
                        PurchaseId = p.PurchaseId,
                        PortalId = p.PortalId,
                        InventoryTypeId = i.ProductCategory.InventoryTypeId,
                        ProductCategoryId = i.ProductCategoryId,
                        ItemId = p.ItemId,
                        Subname = p.Subname,
                        Qty = p.Qty,
                        Total = p.Total,
                        Remarks = p.Remarks,
                        Percentage = p.Percentage,
                        Rate = p.Rate,
                        StockOut = p.StockOut,
                        DatePurchased = p.DatePurchased
                    }).FirstOrDefault();
            }
                
            purchase.InventoryTypes = _db.InventoryTypes.ToList();
            purchase.ProductCategories = _db.ProductCategories.ToList();
            purchase.Items = _db.Items.ToList();
            purchase.Portals = _db.Portals.ToList();
            return purchase;
        }

        public void SavePurchase(dtoPurchase dtoPurchase)
        {
            Purchase purchase = new Purchase
            {
                PurchaseId = dtoPurchase.PurchaseId,
                ItemId = dtoPurchase.ItemId,
                Total = dtoPurchase.Total,
                Remarks = dtoPurchase.Remarks,
                PortalId = dtoPurchase.PortalId,
                Qty = dtoPurchase.Qty,
                Subname = dtoPurchase.Subname,
                Percentage = dtoPurchase.Percentage,
                Rate = dtoPurchase.Rate,
                StockOut = dtoPurchase.Qty,
                DatePurchased = dtoPurchase.DatePurchased
            };
            int purchaseId = _db.Purchases.Add(purchase).PurchaseId;
            InventoryType type =
                _db.InventoryTypes.FirstOrDefault(t => t.InventoryTypeId.Equals(dtoPurchase.InventoryTypeId));
            Stock stock = _db.Stocks.FirstOrDefault(s => s.ItemId.Equals(dtoPurchase.ItemId));
            if (stock == null)
            {
                stock = new Stock
                {
                    ItemId = dtoPurchase.ItemId,
                    NetQty = dtoPurchase.Qty,
                    PurchaseId = purchaseId,
                    AvgRate = type.Name == "Tangible" ? dtoPurchase.Rate : dtoPurchase.Percentage
                };
                _db.Stocks.Add(stock);
            }
            else
            {
                //stock.ItemId = dtoPurchase.ItemId,
                stock.NetQty = dtoPurchase.Qty + stock.NetQty;
                stock.PurchaseId = purchaseId;
                stock.AvgRate = ((type.Name == "Tangible" ? dtoPurchase.Rate : dtoPurchase.Percentage) + stock.AvgRate) / 2;
                _db.Entry(stock).State = EntityState.Modified;
            }
            _db.SaveChanges();

        }

        public void UpdatePurchase(dtoPurchase dtoPurchase)
        {
            Purchase purchase = _db.Purchases.FirstOrDefault(p => p.PurchaseId.Equals(dtoPurchase.PurchaseId));
            Stock stock = _db.Stocks.FirstOrDefault(s => s.ItemId.Equals(dtoPurchase.ItemId));
            if (stock != null)
            {
                //sto
            }
            purchase.ItemId = dtoPurchase.ItemId;
            purchase.Total = dtoPurchase.Total;
            purchase.Remarks = dtoPurchase.Remarks;
            purchase.PortalId = dtoPurchase.PortalId;
            purchase.Qty = dtoPurchase.Qty;
            purchase.Subname = dtoPurchase.Subname;
            purchase.Percentage = dtoPurchase.Percentage;
            purchase.Rate = dtoPurchase.Rate;
            purchase.StockOut = dtoPurchase.StockOut;
            purchase.DatePurchased = dtoPurchase.DatePurchased;
            _db.Entry(purchase).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void DeletePurchase(int id)
        {
            Purchase purchase = _db.Purchases.FirstOrDefault(p => p.PurchaseId.Equals(id));
            _db.Purchases.Remove(purchase);
            _db.SaveChanges();
        }

        public DtoTangibleItemSale GetSpecificPurchase(int id)
        {
            DtoTangibleItemSale purchase = (from p in _db.Purchases
                join i in _db.Items on p.ItemId equals i.ItemId
                where p.PurchaseId == id 
                select new DtoTangibleItemSale
                {
                    ItemCode = i.ItemCode,
                    ItemName = i.Name,
                    Rate = p.Rate,
                    PurchaseId = p.PurchaseId,
                    Qty = 1,
                    SubTotal = p.Rate
                }).FirstOrDefault();
            return purchase;
        }

        public DtoInTangibleItemSale GetSpecificInTangiblePurchase(int id,int PersonId)
        {
            DtoInTangibleItemSale purchase = (from p in _db.Purchases
                join i in _db.Items on p.ItemId equals i.ItemId
                where p.PurchaseId == id
                select new DtoInTangibleItemSale
                {
                    ItemCode = i.ItemCode,
                    ItemName = i.Name,
                    Rate = p.Rate,
                    PurchaseId = p.PurchaseId,
                    Qty = 1,
                    SubTotal = p.Rate,
                    //Lines = _db.Lines.ToList()
                }).FirstOrDefault();
            Person person = _db.Persons.FirstOrDefault(p => p.PersonId.Equals(PersonId));
            if (person != null && person.BusinessLineMap != null && person.BusinessLineMap != 0)
            {
                purchase.Lines.Add(new DtoLinesWithNumbers
                {
                    LineId = (int) person.BusinessLineMap,
                    Number = person.MobileBusiness
                });
            }
            if (person != null && person.PersonalLineMap != null && person.PersonalLineMap != 0)
            {
                purchase.Lines.Add(new DtoLinesWithNumbers
                {
                    LineId = (int)person.PersonalLineMap,
                    Number = person.MobilePersonal
                });
            }
            return purchase;
        }
    }
}