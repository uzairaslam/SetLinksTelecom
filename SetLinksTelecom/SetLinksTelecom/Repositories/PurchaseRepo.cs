using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
        public IEnumerable<dtoDisplayPurchase> GetData(string inventoryType = "")
        {
            IEnumerable<dtoDisplayPurchase> purchases = new List<dtoDisplayPurchase>();
            if (inventoryType == "")
            {
                purchases = from A in (
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
                            join t in _db.InventoryTypes on new { InventoryTypeId = A.InventoryTypeId } equals new
                            { InventoryTypeId = t.InventoryTypeId }
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

            }
            else
            {
                purchases = from A in (
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
                            join t in _db.InventoryTypes on new { InventoryTypeId = A.InventoryTypeId } equals new
                            { InventoryTypeId = t.InventoryTypeId }
                            where t.Name.Equals(inventoryType)
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

            }
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

            try
            {
                using (var transaction = _db.Database.BeginTransaction())
                {
                    #region Save In PurchaseTable
                    InventoryType type =
                        _db.InventoryTypes.FirstOrDefault(t => t.InventoryTypeId.Equals(dtoPurchase.InventoryTypeId));

                    int purchaseId = _db.Purchases.Max(v => (int?)v.Pid) ?? 0;
                    ++purchaseId;

                    Purchase purchase = new Purchase
                    {
                        //PurchaseId = dtoPurchase.PurchaseId,
                        ItemId = dtoPurchase.ItemId,
                        Total = dtoPurchase.Total,
                        Remarks = dtoPurchase.Remarks,
                        PortalId = dtoPurchase.PortalId,
                        Qty = dtoPurchase.Qty,
                        Subname = dtoPurchase.Subname,
                        Percentage = dtoPurchase.Percentage,
                        Rate = dtoPurchase.Rate,
                        StockOut = (type.Name == "Tangible") ? dtoPurchase.Qty : dtoPurchase.Total,
                        DatePurchased = dtoPurchase.DatePurchased
                    };
                    _db.Purchases.Add(purchase);
                    _db.SaveChanges();

                    #endregion

                    #region Save In Stock

                    //int purchaseId = purchase.PurchaseId;

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

                    #endregion

                    #region PurchaseVoucher Entry

                    if (type.Name == "Tangible")
                    {
                        Portal portal = _db.Portals.Single(p => p.PortalId.Equals(purchase.PortalId));
                        var maxVno = _db.AccVouchers.Max(v => (int?)v.VNo) ?? 0;
                        AccAccount acc = _db.AccAccounts.Single(a => a.AccString.Equals(portal.AccString));

                        maxVno = ++maxVno;
                        AccVoucher voucher = new AccVoucher
                        {
                            VDate = purchase.DatePurchased,
                            SessionId = 0,
                            AccString = portal.AccString,
                            VNo = maxVno,
                            VType = "JV",
                            VSrNo = 1,
                            VDescription = purchase.Remarks,
                            Debit = 0,
                            Credit = purchase.Total,
                            UserCode = 0,
                            OID = 0,
                            BID = 0,
                            CID = 0,
                            HeadCode = acc.HeadCode,
                            SubHeadCode = acc.SubHeadCode,
                            AccCode = acc.AccCode,
                            ChequeNo = "0",
                            InvNo = purchase.PurchaseId.ToString(),
                            InvType = "Purchase"
                        };
                        _db.AccVouchers.Add(voucher);
                        _db.SaveChanges();

                        Item item = _db.Items.Single(i => i.ItemId.Equals(purchase.ItemId));
                        acc = new AccAccount();
                        acc = _db.AccAccounts.Single(a => a.AccString.Equals(item.AccString));
                        voucher = new AccVoucher();
                        voucher = new AccVoucher
                        {
                            VDate = purchase.DatePurchased,
                            SessionId = 0,
                            AccString = item.AccString,
                            VNo = maxVno,
                            VType = "JV",
                            VSrNo = 2,
                            VDescription = purchase.Remarks,
                            Debit = purchase.Total,
                            Credit = 0,
                            UserCode = 0,
                            OID = 0,
                            BID = 0,
                            CID = 0,
                            HeadCode = acc.HeadCode,
                            SubHeadCode = acc.SubHeadCode,
                            AccCode = acc.AccCode,
                            ChequeNo = "0",
                            InvNo = purchase.PurchaseId.ToString(),
                            InvType = "Purchase"
                        };
                        _db.AccVouchers.Add(voucher);
                        _db.SaveChanges();

                    }
                    else
                    {
                        Portal portal = _db.Portals.Single(p => p.PortalId.Equals(purchase.PortalId));
                        var maxVno = _db.AccVouchers.Max(v => (int?)v.VNo) ?? 0;
                        ++maxVno;
                        AccAccount acc = _db.AccAccounts.Single(a => a.AccString.Equals(portal.AccString));

                        Item item = _db.Items.Single(i => i.ItemId.Equals(dtoPurchase.ItemId));
                        AccAccount itemAcc = _db.AccAccounts.Single(a => a.AccString.Equals(item.AccString));
                        AccAccount purchDisc = _db.AccAccounts.Single(a => a.AccString.Equals(item.PurDiscString));
                        IList<AccVoucher> vouchers = new List<AccVoucher>()
                        {
                            //Supplier Credit
                            new AccVoucher
                            {
                                VDate = purchase.DatePurchased, SessionId = 0, AccString = portal.AccString, VNo = maxVno, VType = "JV", VSrNo = 1, VDescription = purchase.Remarks,
                                Debit = 0, Credit = purchase.Total, UserCode = 0, OID = 0, BID = 0, CID = 0, HeadCode = acc.HeadCode, SubHeadCode = acc.SubHeadCode,
                                AccCode = acc.AccCode, ChequeNo = "0", InvNo = purchase.PurchaseId.ToString(), InvType = "Purchase"
                            },
                            //Item Value
                            new AccVoucher
                            {
                                VDate = purchase.DatePurchased, SessionId = 0, AccString = itemAcc.AccString, VNo = maxVno, VType = "JV", VSrNo = 2,
                                VDescription = purchase.Remarks, Debit = purchase.Qty, Credit = 0, UserCode = 0, OID = 0, BID = 0, CID = 0, HeadCode = itemAcc.HeadCode,
                                SubHeadCode = itemAcc.SubHeadCode, AccCode = itemAcc.AccCode, ChequeNo = "0", InvNo = purchase.PurchaseId.ToString(), InvType = "Purchase"
                            },
                            //Cash Daily
                            new AccVoucher
                            {
                                VDate = purchase.DatePurchased, SessionId = 0, AccString = "14-02-0001", VNo = maxVno, VType = "JV", VSrNo = 3, VDescription = purchase.Remarks,
                                Debit = 0, Credit = purchase.Total, UserCode = 0, OID = 0, BID = 0, CID = 0, HeadCode = 14, SubHeadCode = 2,
                                AccCode = 1, ChequeNo = "0", InvNo = purchase.PurchaseId.ToString(), InvType = "Purchase"
                            },
                            //Supplier Debit
                            new AccVoucher
                            {
                                VDate = purchase.DatePurchased, SessionId = 0, AccString = portal.AccString, VNo = maxVno, VType = "JV", VSrNo = 4,
                                VDescription = purchase.Remarks, Debit = purchase.Total, Credit = 0, UserCode = 0, OID = 0, BID = 0, CID = 0, HeadCode = acc.HeadCode,
                                SubHeadCode = acc.SubHeadCode, AccCode = acc.AccCode, ChequeNo = "0", InvNo = purchase.PurchaseId.ToString(), InvType = "Purchase"
                            },
                            //Purchase Discount
                            new AccVoucher
                            {
                                VDate = purchase.DatePurchased, SessionId = 0, AccString = purchDisc.AccString, VNo = maxVno, VType = "JV", VSrNo = 5,
                                VDescription = purchase.Remarks, Debit = 0, Credit = purchase.Qty - purchase.Total, UserCode = 0, OID = 0, BID = 0, CID = 0,
                                HeadCode = purchDisc.HeadCode,
                                SubHeadCode = purchDisc.SubHeadCode, AccCode = purchDisc.AccCode, ChequeNo = "0", InvNo = purchase.PurchaseId.ToString(), InvType = "Purchase"
                            }
                        };

                        _db.AccVouchers.AddRange(vouchers);
                        _db.SaveChanges();
                    }

                    #endregion
                    transaction.Commit();
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }


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
                                                Rate = i.SaleRate,
                                                PurchaseId = p.PurchaseId,
                                                Qty = 1,
                                                SubTotal = i.SaleRate
                                            }).FirstOrDefault();
            return purchase;
        }

        public DtoInTangibleItemSale GetSpecificInTangiblePurchase(int id, int PersonId)
        {
            DtoInTangibleItemSale purchase = (from p in _db.Purchases
                                              join i in _db.Items on p.ItemId equals i.ItemId
                                              where p.PurchaseId == id
                                              select new DtoInTangibleItemSale
                                              {
                                                  ItemCode = i.ItemCode,
                                                  ItemName = i.Name,
                                                  Rate = i.SaleRate,
                                                  PurchaseId = p.PurchaseId,
                                                  Qty = 1,
                                                  SubTotal = i.SaleRate,
                                                  //Lines = _db.Lines.ToList()
                                              }).FirstOrDefault();
            Person person = _db.Persons.FirstOrDefault(p => p.PersonId.Equals(PersonId));
            if (person != null && person.BusinessLineMap != null && person.BusinessLineMap != 0)
            {
                purchase.Lines.Add(new DtoLinesWithNumbers
                {
                    LineId = (int)person.BusinessLineMap,
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

        public DtoTangiblePurchase GetTangiblePurchase(int id = 0)
        {
            DtoTangiblePurchase purchase = new DtoTangiblePurchase();
            purchase.DatePurchased = DateTime.Now;
            purchase.Items = (from i in _db.Items
                              join c in _db.ProductCategories on i.ProductCategoryId equals c.ProductCategoryId
                              join t in _db.InventoryTypes on c.InventoryTypeId equals t.InventoryTypeId
                              where t.Name == "Tangible"
                              select i).ToList();
            purchase.Portals = _db.Portals.ToList();
            return purchase;
        }

        public void SaveTangiblePurchase(DtoTangiblePurchase dtoTangible)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                #region Save In PurchaseTable

                int purchaseId = _db.Purchases.Max(v => (int?)v.Pid) ?? 0;
                ++purchaseId;
                //InventoryType type =
                //    _db.InventoryTypes.FirstOrDefault(t => t.InventoryTypeId.Equals(dtoPurchase.InventoryTypeId));
                dtoTangible.Numbers.ForEach(a =>
                {
                    Purchase purchase = new Purchase
                    {
                        //PurchaseId = dtoPurchase.PurchaseId,
                        ItemId = dtoTangible.ItemId,
                        Total = dtoTangible.Rate,//dtoTangible.Total,
                        Remarks = dtoTangible.Remarks,
                        PortalId = dtoTangible.PortalId,
                        Qty = 1,//dtoTangible.Numbers.Count,
                        Subname = dtoTangible.Subname,
                        Percentage = 0,
                        Rate = dtoTangible.Rate,
                        StockOut = 1,//dtoTangible.Numbers.Count,
                        DatePurchased = dtoTangible.DatePurchased,
                        Pid = purchaseId,
                        Number = a
                    };
                    _db.Purchases.Add(purchase);
                    _db.SaveChanges();

                    //purchaseId = purchase.PurchaseId;
                });
                #endregion



                Stock stock = _db.Stocks.FirstOrDefault(s => s.ItemId.Equals(dtoTangible.ItemId));
                if (stock == null)
                {
                    stock = new Stock
                    {
                        ItemId = dtoTangible.ItemId,
                        NetQty = dtoTangible.Numbers.Count,
                        PurchaseId = purchaseId,
                        AvgRate = dtoTangible.Rate
                    };
                    _db.Stocks.Add(stock);
                }
                else
                {
                    //stock.ItemId = dtoPurchase.ItemId,
                    stock.NetQty = dtoTangible.Numbers.Count + stock.NetQty;
                    stock.PurchaseId = purchaseId;
                    stock.AvgRate = (dtoTangible.Rate + stock.AvgRate) / 2;
                    _db.Entry(stock).State = EntityState.Modified;
                }
                _db.SaveChanges();


                #region Coucher Entry

                Portal portal = _db.Portals.Single(p => p.PortalId.Equals(dtoTangible.PortalId));
                var maxVno = _db.AccVouchers.Max(v => (int?)v.VNo) ?? 0;
                AccAccount acc = _db.AccAccounts.Single(a => a.AccString.Equals(portal.AccString));

                maxVno = ++maxVno;
                AccVoucher voucher = new AccVoucher
                {
                    VDate = dtoTangible.DatePurchased,
                    SessionId = 0,
                    AccString = portal.AccString,
                    VNo = maxVno,
                    VType = "JV",
                    VSrNo = 1,
                    VDescription = dtoTangible.Remarks,
                    Debit = 0,
                    Credit = dtoTangible.Total,
                    UserCode = 0,
                    OID = 0,
                    BID = 0,
                    CID = 0,
                    HeadCode = acc.HeadCode,
                    SubHeadCode = acc.SubHeadCode,
                    AccCode = acc.AccCode,
                    ChequeNo = "0",
                    InvNo = purchaseId.ToString(),
                    InvType = "Purchase"
                };
                _db.AccVouchers.Add(voucher);
                _db.SaveChanges();

                Item item = _db.Items.Single(i => i.ItemId.Equals(dtoTangible.ItemId));
                acc = new AccAccount();
                acc = _db.AccAccounts.Single(a => a.AccString.Equals(item.AccString));
                voucher = new AccVoucher();
                voucher = new AccVoucher
                {
                    VDate = dtoTangible.DatePurchased,
                    SessionId = 0,
                    AccString = item.AccString,
                    VNo = maxVno,
                    VType = "JV",
                    VSrNo = 2,
                    VDescription = dtoTangible.Remarks,
                    Debit = dtoTangible.Total,
                    Credit = 0,
                    UserCode = 0,
                    OID = 0,
                    BID = 0,
                    CID = 0,
                    HeadCode = acc.HeadCode,
                    SubHeadCode = acc.SubHeadCode,
                    AccCode = acc.AccCode,
                    ChequeNo = "0",
                    InvNo = purchaseId.ToString(),
                    InvType = "Purchase"
                };
                _db.AccVouchers.Add(voucher);
                _db.SaveChanges();


                #endregion

                transaction.Commit();
            }
        }
    }
}