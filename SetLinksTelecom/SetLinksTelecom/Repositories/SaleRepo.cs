using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;
using SetLinksTelecom.GeneralFolder;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Repositories
{
    public class SaleRepo : ISaleRepo
    {
        private readonly DataContext _db;

        public SaleRepo(DataContext db)
        {
            _db = db;
        }

        public DtoSale GetSale(int id)
        {
            DtoSale dtoSale = new DtoSale();
            return dtoSale;
        }

        public void SaveTangibleSale(DtoTangibleSale dtoTangibleSale)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                Sale sale = new Sale
                {
                    PersonId = dtoTangibleSale.PersonId,
                    Date = dtoTangibleSale.Date,
                    OverAllTotal = dtoTangibleSale.OverAllTotal,
                    Remarks = dtoTangibleSale.Remarks
                };
                sale.SaleDetails = new List<SaleDetail>();
                dtoTangibleSale.ItemSales.ForEach(dt =>
                {
                    sale.SaleDetails.Add(new SaleDetail
                    {
                        Rate = dt.Rate,
                        PurchaseId = dt.PurchaseId,
                        Qty = dt.Qty,
                        LineId = 0,
                        SubTotal = dt.SubTotal
                    });

                    Purchase purchase = _db.Purchases.Single(p => p.PurchaseId.Equals(dt.PurchaseId));
                    purchase.StockOut += dt.SubTotal;

                    _db.Entry(purchase).State = EntityState.Modified;
                    _db.SaveChanges();
                });


                _db.Sales.Add(sale);
                _db.SaveChanges();


                #region Account


                //**********Customer Account Entry***************
                Person person = _db.Persons.Single(p => p.PersonId.Equals(sale.PersonId));
                var maxVno = _db.AccVouchers.Max(v => (int?)v.VNo) ?? 0;
                AccAccount acc = _db.AccAccounts.Single(a => a.AccString.Equals(person.AccString));

                maxVno = ++maxVno;
                int VSrNo = 1;
                AccVoucher voucher = new AccVoucher
                {
                    VDate = sale.Date,
                    SessionId = 0,
                    AccString = person.AccString,
                    VNo = maxVno,
                    VType = "JV",
                    VSrNo = VSrNo,
                    VDescription = sale.Remarks,//sale.Remarks,
                    Debit = sale.OverAllTotal,
                    Credit = 0,
                    UserCode = 0,
                    OID = 0,
                    BID = 0,
                    CID = 0,
                    HeadCode = acc.HeadCode,
                    SubHeadCode = acc.SubHeadCode,
                    AccCode = acc.AccCode,
                    ChequeNo = "0",
                    InvNo = sale.SaleId.ToString(),
                    InvType = "Sale"
                };
                _db.AccVouchers.Add(voucher);
                _db.SaveChanges();


                //************Sale Revenue Account Entry************
                dtoTangibleSale.ItemSales.ForEach(dt =>
                {
                    Purchase purchase = _db.Purchases.Single(p => p.PurchaseId.Equals(dt.PurchaseId));
                    Item item = _db.Items.Single(i => i.ItemId.Equals(purchase.ItemId));
                    acc = new AccAccount();
                    acc = _db.AccAccounts.Single(a => a.AccString.Equals(item.AccString));
                    voucher = new AccVoucher();
                    VSrNo = ++VSrNo;
                    voucher = new AccVoucher
                    {
                        VDate = sale.Date,
                        SessionId = 0,
                        AccString = item.RevString,
                        VNo = maxVno,
                        VType = "JV",
                        VSrNo = VSrNo,
                        VDescription = sale.Remarks,//sale.Remarks,
                        Debit = 0,
                        Credit = dt.SubTotal,
                        UserCode = 0,
                        OID = 0,
                        BID = 0,
                        CID = 0,
                        HeadCode = acc.HeadCode,
                        SubHeadCode = acc.SubHeadCode,
                        AccCode = acc.AccCode,
                        ChequeNo = "0",
                        InvNo = sale.SaleId.ToString(),
                        InvType = "Sale"
                    };
                    _db.AccVouchers.Add(voucher);
                    _db.SaveChanges();
                });


                //*********Items Value Account***********

                dtoTangibleSale.ItemSales.ForEach(dt =>
                {
                    Purchase purchase = _db.Purchases.Single(p => p.PurchaseId.Equals(dt.PurchaseId));
                    Item item = _db.Items.Single(i => i.ItemId.Equals(purchase.ItemId));
                    acc = new AccAccount();
                    acc = _db.AccAccounts.Single(a => a.AccString.Equals(item.AccString));
                    voucher = new AccVoucher();
                    VSrNo = ++VSrNo;
                    voucher = new AccVoucher
                    {
                        VDate = sale.Date,
                        SessionId = 0,
                        AccString = item.AccString,
                        VNo = maxVno,
                        VType = "JV",
                        VSrNo = VSrNo,
                        VDescription = sale.Remarks,//sale.Remarks,
                        Debit = 0,
                        Credit = dt.Qty * purchase.Rate,
                        UserCode = 0,
                        OID = 0,
                        BID = 0,
                        CID = 0,
                        HeadCode = acc.HeadCode,
                        SubHeadCode = acc.SubHeadCode,
                        AccCode = acc.AccCode,
                        ChequeNo = "0",
                        InvNo = sale.SaleId.ToString(),
                        InvType = "Sale"
                    };
                    _db.AccVouchers.Add(voucher);
                    _db.SaveChanges();
                });


                //******Sale Cost Account************
                dtoTangibleSale.ItemSales.ForEach(dt =>
                {
                    Purchase purchase = _db.Purchases.Single(p => p.PurchaseId.Equals(dt.PurchaseId));
                    Item item = _db.Items.Single(i => i.ItemId.Equals(purchase.ItemId));
                    acc = new AccAccount();
                    acc = _db.AccAccounts.Single(a => a.AccString.Equals(item.CosString));
                    voucher = new AccVoucher();
                    VSrNo = ++VSrNo;
                    voucher = new AccVoucher
                    {
                        VDate = sale.Date,
                        SessionId = 0,
                        AccString = item.AccString,
                        VNo = maxVno,
                        VType = "JV",
                        VSrNo = VSrNo,
                        VDescription = sale.Remarks,//sale.Remarks,
                        Debit = dt.Qty * purchase.Rate,
                        Credit = 0,
                        UserCode = 0,
                        OID = 0,
                        BID = 0,
                        CID = 0,
                        HeadCode = acc.HeadCode,
                        SubHeadCode = acc.SubHeadCode,
                        AccCode = acc.AccCode,
                        ChequeNo = "0",
                        InvNo = sale.SaleId.ToString(),
                        InvType = "Sale"
                    };
                    _db.AccVouchers.Add(voucher);
                    _db.SaveChanges();
                });



                #endregion

                transaction.Commit();
            }
        }
        public void SaveInTangibleSale(DtoInTangibleSale dtoInTangibleSale)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                Sale sale = new Sale
                {
                    PersonId = dtoInTangibleSale.PersonId,
                    Date = dtoInTangibleSale.Date,
                    OverAllTotal = dtoInTangibleSale.OverAllTotal,
                    Remarks = dtoInTangibleSale.Remarks
                };
                sale.SaleDetails = new List<SaleDetail>();
                dtoInTangibleSale.ItemSales.ForEach(dt =>
                {
                    sale.SaleDetails.Add(new SaleDetail
                    {
                        Rate = dt.Rate,
                        PurchaseId = dt.PurchaseId,
                        Qty = dt.Qty,
                        LineId = dt.LineId,
                        SubTotal = dt.SubTotal,
                        CommProfit = (((dt.Rate * dt.Qty) / 100) * dt.Rate) / 100
                    });

                    Purchase purchase = _db.Purchases.Single(p => p.PurchaseId.Equals(dt.PurchaseId));
                    purchase.StockOut += dt.SubTotal;

                    _db.Entry(purchase).State = EntityState.Modified;
                    _db.SaveChanges();
                });
                _db.Sales.Add(sale);
                _db.SaveChanges();


                #region Account
                var maxVno = _db.AccVouchers.Max(v => (int?)v.VNo) ?? 0;
                ++maxVno;

                Person customer = _db.Persons.Single(p => p.PersonId.Equals(sale.PersonId));
                AccAccount custAccount = _db.AccAccounts.Single(c => c.AccString.Equals(customer.AccString));

                IList<AccVoucher> vouchers = new List<AccVoucher>();
                int vSrNo = 0;

                sale.SaleDetails.ForEach(dt =>
                {
                    ++vSrNo;
                    //1
                    vouchers.Add(
                        //Customer Debit
                        new AccVoucher
                        {
                            VDate = sale.Date,
                            SessionId = 0,
                            AccString = custAccount.AccString,
                            VNo = maxVno,
                            VType = "JV",
                            VSrNo = vSrNo,
                            VDescription = sale.Remarks,
                            Debit = dt.Qty,
                            Credit = 0,
                            UserCode = 0,
                            OID = 0,
                            BID = 0,
                            CID = dt.LineId,
                            HeadCode = custAccount.HeadCode,
                            SubHeadCode = custAccount.SubHeadCode,
                            AccCode = custAccount.AccCode,
                            ChequeNo = "0",
                            InvNo = sale.SaleId.ToString(),
                            InvType = "Sale"
                        }
                        );

                    ++vSrNo;
                    //2
                    Purchase purchase = _db.Purchases.Single(p => p.PurchaseId.Equals(dt.PurchaseId));
                    Item item = _db.Items.Single(i => i.ItemId.Equals(purchase.ItemId));
                    AccAccount revAccount = _db.AccAccounts.Single(re => re.AccString.Equals(item.RevString));
                    vouchers.Add(
                        //Item Revenue
                        new AccVoucher
                        {
                            VDate = sale.Date,
                            SessionId = 0,
                            AccString = revAccount.AccString,
                            VNo = maxVno,
                            VType = "JV",
                            VSrNo = vSrNo,
                            VDescription = sale.Remarks,
                            Debit = 0,
                            Credit = dt.Qty,
                            UserCode = 0,
                            OID = 0,
                            BID = 0,
                            CID = dt.LineId,
                            HeadCode = revAccount.HeadCode,
                            SubHeadCode = revAccount.SubHeadCode,
                            AccCode = revAccount.AccCode,
                            ChequeNo = "0",
                            InvNo = sale.SaleId.ToString(),
                            InvType = "Sale"
                        }
                        );

                    ++vSrNo;
                    //3
                    AccAccount valAccount = _db.AccAccounts.Single(re => re.AccString.Equals(item.AccString));
                    vouchers.Add(
                        //Item Value
                        new AccVoucher
                        {
                            VDate = sale.Date,
                            SessionId = 0,
                            AccString = valAccount.AccString,
                            VNo = maxVno,
                            VType = "JV",
                            VSrNo = vSrNo,
                            VDescription = sale.Remarks,
                            Debit = 0,
                            Credit = dt.SubTotal,
                            UserCode = 0,
                            OID = 0,
                            BID = 0,
                            CID = 0,
                            HeadCode = valAccount.HeadCode,
                            SubHeadCode = valAccount.SubHeadCode,
                            AccCode = valAccount.AccCode,
                            ChequeNo = "0",
                            InvNo = sale.SaleId.ToString(),
                            InvType = "Sale"
                        }
                        );
                    ++vSrNo;
                    //4
                    AccAccount costAccount = _db.AccAccounts.Single(re => re.AccString.Equals(item.CosString));
                    vouchers.Add(
                        //Item Cost
                        new AccVoucher
                        {
                            VDate = sale.Date,
                            SessionId = 0,
                            AccString = costAccount.AccString,
                            VNo = maxVno,
                            VType = "JV",
                            VSrNo = vSrNo,
                            VDescription = sale.Remarks,
                            Debit = dt.SubTotal,
                            Credit = 0,
                            UserCode = 0,
                            OID = 0,
                            BID = 0,
                            CID = 0,
                            HeadCode = costAccount.HeadCode,
                            SubHeadCode = costAccount.SubHeadCode,
                            AccCode = costAccount.AccCode,
                            ChequeNo = "0",
                            InvNo = sale.SaleId.ToString(),
                            InvType = "Sale"
                        }
                        );

                    ++vSrNo;
                    //5
                    AccAccount commAccount = _db.AccAccounts.Single(re => re.AccString.Equals(item.SaleCommString));
                    vouchers.Add(
                        //Sale Commission Revenue
                        new AccVoucher
                        {
                            VDate = sale.Date,
                            SessionId = 0,
                            AccString = commAccount.AccString,
                            VNo = maxVno,
                            VType = "JV",
                            VSrNo = vSrNo,
                            VDescription = sale.Remarks,
                            Debit = 0,
                            Credit = dt.CommProfit,
                            UserCode = 0,
                            OID = 0,
                            BID = 0,
                            CID = 0,
                            HeadCode = commAccount.HeadCode,
                            SubHeadCode = commAccount.SubHeadCode,
                            AccCode = commAccount.AccCode,
                            ChequeNo = "0",
                            InvNo = sale.SaleId.ToString(),
                            InvType = "Sale"
                        }
                        );

                    ++vSrNo;
                    //6
                    vouchers.Add(
                        //Sale Commission Revenue
                        new AccVoucher
                        {
                            VDate = sale.Date,
                            SessionId = 0,
                            AccString = custAccount.AccString,
                            VNo = maxVno,
                            VType = "JV",
                            VSrNo = vSrNo,
                            VDescription = sale.Remarks,
                            Debit = dt.CommProfit,
                            Credit = 0,
                            UserCode = 0,
                            OID = 0,
                            BID = 0,
                            CID = 0,
                            HeadCode = custAccount.HeadCode,
                            SubHeadCode = custAccount.SubHeadCode,
                            AccCode = custAccount.AccCode,
                            ChequeNo = "0",
                            InvNo = sale.SaleId.ToString(),
                            InvType = "Sale"
                        }
                        );
                });

                _db.AccVouchers.AddRange(vouchers);
                _db.SaveChanges();

                #endregion
                transaction.Commit();
            }
        }

        public IList<DtoSaleReturnView> GetDataForReturn()
        {
            var sale = (from sd in _db.SaleDetails
                        join s in _db.Sales on sd.SaleId equals s.SaleId
                        join p in _db.Purchases on sd.PurchaseId equals p.PurchaseId
                        join po in _db.Portals on p.PortalId equals po.PortalId
                        join i in _db.Items on p.ItemId equals i.ItemId
                        join cat in _db.ProductCategories on i.ProductCategoryId equals cat.ProductCategoryId
                        join ty in _db.InventoryTypes on cat.InventoryTypeId equals ty.InventoryTypeId
                        select new DtoSaleReturnView
                        {
                            PortalName = po.Name,
                            InventoryType = ty.Name,
                            CategoryName = cat.Name,
                            ItemId = i.ItemId,
                            ItemName = i.Name,
                            DatePurchased = p.DatePurchased,
                            DateSold = s.Date,
                            PurchaseId = p.PurchaseId,
                            PortalId = po.PortalId,
                            SaleDetailId = sd.SaleDetailId
                        }).ToList();

            return sale;

            //var salesrEturn = (from sd in _db.SaleDetails
            //    join p in _db.Purchases on sd.PurchaseId equals p.PurchaseId
            //    join i in _db.Items on p.ItemId equals i.ItemId
            //    select new DtoSaleReturnView
            //    {
            //        PortalName = p.Portal.Name,
            //        InventoryType = i.ProductCategory.InventoryType.Name,
            //        CategoryName = i.ProductCategory.Name,
            //        ItemId = i.ItemId,
            //        ItemName = i.Name,
            //        DatePurchased = p.DatePurchased,
            //        DateSold = sd.Sale.Date,
            //        PurchaseId = p.PurchaseId,
            //        PortalId = p.Portal.PortalId,
            //        SaleDetailId = sd.SaleDetailId
            //    }).ToList();
            //return salesrEturn;
        }

        public DtoTangibleSaleDetailItem GetSpecificSaleDetailItem(int id)
        {
            DtoTangibleSaleDetailItem purchase = (from sd in _db.SaleDetails
                                                  join p in _db.Purchases on sd.PurchaseId equals p.PurchaseId
                                                  join i in _db.Items on p.ItemId equals i.ItemId
                                                  where sd.SaleDetailId == id
                                                  select new DtoTangibleSaleDetailItem
                                                  {
                                                      SaleDetailId = sd.SaleDetailId,
                                                      ItemCode = i.ItemCode,
                                                      ItemName = i.Name,
                                                      Rate = i.SaleRate,
                                                      //PurchaseId = p.PurchaseId,
                                                      Qty = 1,
                                                      SubTotal = i.SaleRate
                                                  }).FirstOrDefault();
            return purchase;
        }


        public DisplayDtoJazzCashExcel SaveJazzCashSales(DisplayDtoJazzCashExcel cashExcels)
        {
            cashExcels.ErrorMessage = string.Empty;
            List<string> errors = new List<string>();
            List<DtoJazzCashExcel> jazzCashExcel = new List<DtoJazzCashExcel>();
            try
            {
                using (var transaction = _db.Database.BeginTransaction())
                {
                    foreach (DtoJazzCashExcel sale in cashExcels.jazzCashExcel)
                    {
                        var itemId = _db.Purchases.Single(p => p.PurchaseId.Equals(cashExcels.PurchaseId)).ItemId;
                        IList<SalePurchaseStockOutMap> maps = new List<SalePurchaseStockOutMap>();




                        if (sale.TransactionStatus.Equals("Completed"))
                        {
                            if (!_db.Sales.Any(s => s.TransactionId.Equals(sale.TransactionId)))
                            {
                                //Console.WriteLine();
                                CultureInfo culture = new CultureInfo("en-GB");

                                Person person = _db.Persons.FirstOrDefault(p =>
                                    p.MobileBusiness.Substring(p.MobileBusiness.Length - 10)
                                        .Equals(sale.MSISDN.Substring(sale.MSISDN.Length - 10)) || p.MobilePersonal
                                        .Substring(p.MobilePersonal.Length - 10)
                                        .Equals(sale.MSISDN.Substring(sale.MSISDN.Length - 10)));
                                if (person != null)
                                {

                                    var line = person.MobileBusiness.Substring(person.MobileBusiness.Length - 10)
                                        .Equals(sale.MSISDN.Substring(sale.MSISDN.Length - 10))
                                        ? person.BusinessLineMap
                                        : person.PersonalLineMap;

                                    if (line != null)
                                    {
                                        if (sale.BalanceBeforeTransaction < sale.BalanceAfterTransaction)
                                        {

                                            var records = (from p1 in _db.Purchases
                                                           join p2 in _db.Purchases on p1.ItemId equals p2.ItemId
                                                           where
                                                               p1.ItemId == itemId &&
                                                               p2.PurchaseId <= p1.PurchaseId &&
                                                               p1.Qty > p1.StockOut
                                                           group new { p1, p2 } by new
                                                           {
                                                               p1.PurchaseId,
                                                               p1.Qty,
                                                               p1.StockOut,
                                                               p1.DatePurchased
                                                           }
                                                into g
                                                           //where g.Sum(p => p.p2.Qty) <= 10000
                                                           orderby
                                                               g.Key.DatePurchased
                                                           select new
                                                           {
                                                               PurchaseId = g.Key.PurchaseId,
                                                               stock = g.Key.Qty - g.Key.StockOut,
                                                               DatePurchased = g.Key.DatePurchased,
                                                               SumTotal = (int?)g.Sum(p => p.p2.Qty - p.p2.StockOut)
                                                           }).ToList();
                                            //DataTable dt = records.ToDataTable();

                                            if (records.Count > 0)
                                            {
                                                // Sale
                                                var remainingAmount = sale.TransactionAmount;
                                                foreach (var record in records.OrderBy(r => r.DatePurchased))
                                                {
                                                    if (remainingAmount > 0)
                                                    {
                                                        Purchase purchase = _db.Purchases.Single(p =>
                                                            p.PurchaseId.Equals(record.PurchaseId));
                                                        decimal trancsaction = 0M;
                                                        if (record.stock > remainingAmount)
                                                        {
                                                            purchase.StockOut += remainingAmount;
                                                            trancsaction = remainingAmount;
                                                            remainingAmount -= remainingAmount;
                                                        }
                                                        else
                                                        {
                                                            purchase.StockOut += record.stock;
                                                            trancsaction = record.stock;
                                                            remainingAmount -= record.stock;
                                                        }

                                                        _db.Entry(purchase).State = EntityState.Modified;
                                                        _db.SaveChanges();
                                                        maps.Add(new SalePurchaseStockOutMap
                                                        {
                                                            PurchaseId = purchase.PurchaseId,
                                                            Amount = trancsaction
                                                        });
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }

                                                Sale saleEntry = new Sale
                                                {
                                                    PersonId = person.PersonId,
                                                    Date = Convert.ToDateTime(sale.TransactionTime,
                                                        culture), //DateTime.ParseExact(sale.TransactionTime, "dd/MM/yyyy HH:mm:SS", null),//Convert.ToDateTime(sale.TransactionTime),
                                                    OverAllTotal = sale.TransactionAmount,
                                                    Remarks = sale.TransactionStatus,
                                                    TransactionId = sale.TransactionId
                                                };
                                                saleEntry.SaleDetails = new List<SaleDetail>();
                                                saleEntry.SaleDetails.Add(new SaleDetail
                                                {
                                                    Rate = sale.TransactionAmount,
                                                    PurchaseId = 0,
                                                    Qty = sale.TransactionAmount,
                                                    LineId = line ?? 0,
                                                    SubTotal = sale.TransactionAmount
                                                });
                                                _db.Sales.Add(saleEntry);
                                                _db.SaveChanges();

                                                foreach (SalePurchaseStockOutMap map in maps)
                                                {
                                                    map.SaleId = saleEntry.SaleId;
                                                }

                                                _db.StockOutMaps.AddRange(maps);




                                                #region Account

                                                var maxVno = _db.AccVouchers.Max(v => (int?)v.VNo) ?? 0;
                                                ++maxVno;

                                                AccAccount custAccount =
                                                    _db.AccAccounts.Single(
                                                        c => c.AccString.Equals(person.AccString));

                                                IList<AccVoucher> vouchers = new List<AccVoucher>();
                                                int vSrNo = 0;

                                                ++vSrNo;
                                                //1
                                                vouchers.Add(
                                                    //Customer Debit
                                                    new AccVoucher
                                                    {
                                                        VDate = Convert.ToDateTime(sale.TransactionTime,
                                                            culture), //DateTime.ParseExact(sale.TransactionTime, "dd/MM/yyyy HH:mm:SS", null),//Convert.ToDateTime(sale.TransactionTime),
                                                            SessionId = 0,
                                                        AccString = custAccount.AccString,
                                                        VNo = maxVno,
                                                        VType = "JV",
                                                        VSrNo = vSrNo,
                                                        VDescription = sale.TransactionStatus,
                                                        Debit = sale.TransactionAmount,
                                                        Credit = 0,
                                                        UserCode = 0,
                                                        OID = 0,
                                                        BID = 0,
                                                        CID = line ?? 0,
                                                        HeadCode = custAccount.HeadCode,
                                                        SubHeadCode = custAccount.SubHeadCode,
                                                        AccCode = custAccount.AccCode,
                                                        ChequeNo = "0",
                                                        InvNo = saleEntry.SaleId.ToString(),
                                                        InvType = "Sale"
                                                    }
                                                );

                                                ++vSrNo;
                                                //2
                                                //Purchase purchase = _db.Purchases.Single(p => p.PurchaseId.Equals(dt.PurchaseId));
                                                Item item = _db.Items.Single(i => i.ItemId.Equals(itemId));
                                                AccAccount revAccount =
                                                    _db.AccAccounts.Single(
                                                        re => re.AccString.Equals(item.RevString));
                                                vouchers.Add(
                                                    //Item Revenue
                                                    new AccVoucher
                                                    {
                                                        VDate = Convert.ToDateTime(sale.TransactionTime,
                                                            culture), //DateTime.ParseExact(sale.TransactionTime, "dd/MM/yyyy HH:mm:SS", null),//Convert.ToDateTime(sale.TransactionTime),
                                                            SessionId = 0,
                                                        AccString = revAccount.AccString,
                                                        VNo = maxVno,
                                                        VType = "JV",
                                                        VSrNo = vSrNo,
                                                        VDescription = sale.TransactionStatus,
                                                        Debit = 0,
                                                        Credit = sale.TransactionAmount,
                                                        UserCode = 0,
                                                        OID = 0,
                                                        BID = 0,
                                                        CID = line ?? 0,
                                                        HeadCode = revAccount.HeadCode,
                                                        SubHeadCode = revAccount.SubHeadCode,
                                                        AccCode = revAccount.AccCode,
                                                        ChequeNo = "0",
                                                        InvNo = saleEntry.SaleId.ToString(),
                                                        InvType = "Sale"
                                                    }
                                                );

                                                ++vSrNo;
                                                //3
                                                AccAccount valAccount =
                                                    _db.AccAccounts.Single(
                                                        re => re.AccString.Equals(item.AccString));
                                                vouchers.Add(
                                                    //Item Value
                                                    new AccVoucher
                                                    {
                                                        VDate = Convert.ToDateTime(sale.TransactionTime,
                                                            culture), //DateTime.ParseExact(sale.TransactionTime, "dd/MM/yyyy HH:mm:SS", null),//Convert.ToDateTime(sale.TransactionTime),
                                                            SessionId = 0,
                                                        AccString = valAccount.AccString,
                                                        VNo = maxVno,
                                                        VType = "JV",
                                                        VSrNo = vSrNo,
                                                        VDescription = sale.TransactionStatus,
                                                        Debit = 0,
                                                        Credit = sale.TransactionAmount,
                                                        UserCode = 0,
                                                        OID = 0,
                                                        BID = 0,
                                                        CID = 0,
                                                        HeadCode = valAccount.HeadCode,
                                                        SubHeadCode = valAccount.SubHeadCode,
                                                        AccCode = valAccount.AccCode,
                                                        ChequeNo = "0",
                                                        InvNo = saleEntry.SaleId.ToString(),
                                                        InvType = "Sale"
                                                    }
                                                );

                                                ++vSrNo;
                                                //4
                                                AccAccount costAccount =
                                                    _db.AccAccounts.Single(
                                                        re => re.AccString.Equals(item.CosString));
                                                vouchers.Add(
                                                    //Item Cost
                                                    new AccVoucher
                                                    {
                                                        VDate = Convert.ToDateTime(sale.TransactionTime,
                                                            culture), //DateTime.ParseExact(sale.TransactionTime, "dd/MM/yyyy HH:mm:SS", null),//Convert.ToDateTime(sale.TransactionTime),
                                                            SessionId = 0,
                                                        AccString = costAccount.AccString,
                                                        VNo = maxVno,
                                                        VType = "JV",
                                                        VSrNo = vSrNo,
                                                        VDescription = sale.TransactionStatus,
                                                        Debit = sale.TransactionAmount,
                                                        Credit = 0,
                                                        UserCode = 0,
                                                        OID = 0,
                                                        BID = 0,
                                                        CID = 0,
                                                        HeadCode = costAccount.HeadCode,
                                                        SubHeadCode = costAccount.SubHeadCode,
                                                        AccCode = costAccount.AccCode,
                                                        ChequeNo = "0",
                                                        InvNo = saleEntry.SaleId.ToString(),
                                                        InvType = "Sale"
                                                    }
                                                );


                                                _db.AccVouchers.AddRange(vouchers);
                                                _db.SaveChanges();

                                                #endregion
                                            }
                                            else
                                            {
                                                errors.Add("Stock empty");
                                                jazzCashExcel.Add(sale);
                                            }
                                        }
                                        else
                                        {
                                            Purchase purchase = new Purchase
                                            {
                                                //PurchaseId = dtoPurchase.PurchaseId,
                                                ItemId = itemId,
                                                Total = sale.TransactionAmount,
                                                Remarks = sale.TransactionStatus,
                                                PortalId = person.PersonId,
                                                Qty = (int)sale.TransactionAmount,
                                                Subname = "Cash",
                                                Percentage = 0,
                                                Rate = 0,
                                                //StockOut = (type.Name == "Tangible") ? dtoPurchase.Qty : dtoPurchase.Total,
                                                DatePurchased = Convert.ToDateTime(sale.TransactionTime, culture),//DateTime.ParseExact(sale.TransactionTime,"dd/MM/yyyy HH:mm:SS", System.Globalization.CultureInfo("en-GB")),
                                                Return = true
                                            };
                                            _db.Purchases.Add(purchase);
                                            _db.SaveChanges();

                                            Sale saleEntry = new Sale
                                            {
                                                PersonId = person.PersonId,
                                                Date = Convert.ToDateTime(sale.TransactionTime, culture),//DateTime.ParseExact(sale.TransactionTime, "dd/MM/yyyy HH:mm:SS", null),//Convert.ToDateTime(sale.TransactionTime),
                                                OverAllTotal = (-1) * sale.TransactionAmount,
                                                Remarks = sale.TransactionStatus,
                                                TransactionId = sale.TransactionId
                                            };
                                            saleEntry.SaleDetails = new List<SaleDetail>();
                                            saleEntry.SaleDetails.Add(new SaleDetail
                                            {
                                                Rate = (-1) * sale.TransactionAmount,
                                                PurchaseId = 0,
                                                Qty = (-1) * sale.TransactionAmount,
                                                LineId = line ?? 0,
                                                SubTotal = (-1) * sale.TransactionAmount
                                            });
                                            _db.Sales.Add(saleEntry);
                                            _db.SaveChanges();

                                            var maxVno = _db.AccVouchers.Max(v => (int?)v.VNo) ?? 0;
                                            ++maxVno;

                                            AccAccount personAcc = _db.AccAccounts.Single(a => a.AccString.Equals(person.AccString));
                                            Item item = _db.Items.Single(i => i.ItemId.Equals(itemId));

                                            AccAccount valueAccount =
                                                _db.AccAccounts.Single(a => a.AccString.Equals(item.AccString));
                                            AccAccount revenueAccount =
                                                _db.AccAccounts.Single(a => a.AccString.Equals(item.RevString));
                                            AccAccount costAccount =
                                                _db.AccAccounts.Single(a => a.AccString.Equals(item.CosString));

                                            IList<AccVoucher> vouchers = new List<AccVoucher>
                                        {
                                            new AccVoucher
                                            {
                                                VDate = purchase.DatePurchased, SessionId = 0,
                                                AccString = person.AccString, VNo = maxVno, VType = "JV", VSrNo = 1,
                                                VDescription = purchase.Remarks,
                                                Debit = 0, Credit = purchase.Total, UserCode = 0, OID = 0, BID = 0,
                                                CID = 0, HeadCode = personAcc.HeadCode,
                                                SubHeadCode = personAcc.SubHeadCode,
                                                AccCode = personAcc.AccCode, ChequeNo = "0",
                                                InvNo = saleEntry.SaleId.ToString(), InvType = "Sale Return"
                                            },
                                            //Item Value
                                            new AccVoucher
                                            {
                                                VDate = purchase.DatePurchased, SessionId = 0,
                                                AccString = valueAccount.AccString, VNo = maxVno, VType = "JV", VSrNo = 2,
                                                VDescription = purchase.Remarks, Debit = purchase.Total, Credit = 0,
                                                UserCode = 0, OID = 0, BID = 0, CID = 0, HeadCode = valueAccount.HeadCode,
                                                SubHeadCode = valueAccount.SubHeadCode, AccCode = valueAccount.AccCode,
                                                ChequeNo = "0", InvNo = saleEntry.SaleId.ToString(),
                                                InvType = "Sale Return"
                                            },
                                            //Item Revenue
                                            new AccVoucher
                                            {
                                                VDate = purchase.DatePurchased, SessionId = 0,
                                                AccString = revenueAccount.AccString, VNo = maxVno, VType = "JV", VSrNo = 3,
                                                VDescription = purchase.Remarks, Debit = purchase.Total, Credit = 0,
                                                UserCode = 0, OID = 0, BID = 0, CID = 0, HeadCode = revenueAccount.HeadCode,
                                                SubHeadCode = revenueAccount.SubHeadCode, AccCode = revenueAccount.AccCode,
                                                ChequeNo = "0", InvNo = saleEntry.SaleId.ToString(),
                                                InvType = "Sale Return"
                                            },
                                            //Item Cost
                                            new AccVoucher
                                            {
                                                VDate = purchase.DatePurchased, SessionId = 0,
                                                AccString = costAccount.AccString, VNo = maxVno, VType = "JV", VSrNo = 4,
                                                VDescription = purchase.Remarks, Debit = 0, Credit = purchase.Total,
                                                UserCode = 0, OID = 0, BID = 0, CID = 0, HeadCode = costAccount.HeadCode,
                                                SubHeadCode = costAccount.SubHeadCode, AccCode = costAccount.AccCode,
                                                ChequeNo = "0", InvNo = saleEntry.SaleId.ToString(),
                                                InvType = "Sale Return"
                                            }
                                        };


                                            _db.AccVouchers.AddRange(vouchers);
                                            _db.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        errors.Add("Line not found");
                                        jazzCashExcel.Add(sale);
                                    }
                                }
                                else
                                {
                                    errors.Add("Person not found");
                                    jazzCashExcel.Add(sale);
                                }
                            }
                            else
                            {
                                errors.Add("Transaction Id already exist");
                                jazzCashExcel.Add(sale);
                            }

                        }
                        else
                        {
                            errors.Add("Status is not completed");
                            jazzCashExcel.Add(sale);
                        }
                    }

                    if (jazzCashExcel.Count == 0)
                    {
                        transaction.Commit();
                        return new DisplayDtoJazzCashExcel
                        {

                            PurchaseId = cashExcels.PurchaseId,
                            ItemName = cashExcels.ItemName,
                            ErrorMessage = "File Uploaded"
                        };
                    }
                    else
                    {
                        var customErrors = errors.Select(e => e).Distinct();
                        return new DisplayDtoJazzCashExcel
                        {
                            jazzCashExcel = jazzCashExcel,
                            ErrorMessage = string.Join(" | ", customErrors),
                            PurchaseId = cashExcels.PurchaseId,
                            ItemName = cashExcels.ItemName
                        };
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join(" ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ",
                    fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                //throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                cashExcels.ErrorMessage = exceptionMessage;
                return cashExcels;
            }
            catch (Exception ex)
            {
                cashExcels.ErrorMessage = ex.Message;
                return cashExcels;
            }
        }
    }
}