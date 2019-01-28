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
            return from p in _db.Purchases
                   join po in _db.Portals on p.PortalId equals po.PortalId
                join i in _db.Items on p.ItemId equals i.ItemId
                select new dtoDisplayPurchase
                {
                    PortalName = po.Name,
                    InventoryType = i.ProductCategory.InventoryType.Name,
                    CategoryName = i.ProductCategory.Name,
                    ItemName = i.Name,
                    PurchaseId = p.PurchaseId,
                    Subname = p.Subname,
                    Comments = p.Remarks,
                    Qty = p.Qty,
                    PaidAmount = p.Total
                };
        }

        public dtoPurchase GetPurchase(int id)
        {
            dtoPurchase purchase = new dtoPurchase();
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
                        PaidAmount = p.Total,
                        Comments = p.Remarks
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
                Total = dtoPurchase.PaidAmount,
                Remarks = dtoPurchase.Comments,
                PortalId = dtoPurchase.PortalId,
                Qty = dtoPurchase.Qty,
                Subname = dtoPurchase.Subname
            };
            _db.Purchases.Add(purchase);
            _db.SaveChanges();

        }

        public void UpdatePurchase(dtoPurchase dtoPurchase)
        {
            Purchase purchase = _db.Purchases.FirstOrDefault(p => p.PurchaseId.Equals(dtoPurchase.PurchaseId));
            purchase.ItemId = dtoPurchase.ItemId;
            purchase.Total = dtoPurchase.PaidAmount;
            purchase.Remarks = dtoPurchase.Comments;
            purchase.PortalId = dtoPurchase.PortalId;
            purchase.Qty = dtoPurchase.Qty;
            purchase.Subname = dtoPurchase.Subname;
            _db.Entry(purchase).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void DeletePurchase(int id)
        {
            Purchase purchase = _db.Purchases.FirstOrDefault(p => p.PurchaseId.Equals(id));
            _db.Purchases.Remove(purchase);
            _db.SaveChanges();
        }
    }
}