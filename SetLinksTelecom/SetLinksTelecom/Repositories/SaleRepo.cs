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
                            CID = 0,
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
                            CID = 0,
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
    }
}