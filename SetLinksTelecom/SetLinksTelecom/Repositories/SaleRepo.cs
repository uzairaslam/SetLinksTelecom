using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;
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
                    OverAllTotal = dtoTangibleSale.OverAllTotal
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
                    VDescription = "",//sale.Remarks,
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
                        VDescription = "",//sale.Remarks,
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
                        VDescription = "",//sale.Remarks,
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
                        VDescription = "",//sale.Remarks,
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
            Sale sale = new Sale
            {
                PersonId = dtoInTangibleSale.PersonId,
                Date = dtoInTangibleSale.Date,
                OverAllTotal = dtoInTangibleSale.OverAllTotal
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
                    CommProfit = (((dt.Rate * dt.Qty) / 100) * dt.Rate)/100
                });
            });
            _db.Sales.Add(sale);
            _db.SaveChanges();
        }
    }
}