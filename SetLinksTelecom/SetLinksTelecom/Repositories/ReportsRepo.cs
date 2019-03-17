using System;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Linq;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;
using SetLinksTelecom.GeneralFolder;
using System.Linq.Expressions;

namespace SetLinksTelecom.Repositories
{
    public class ReportsRepo : IReports
    {
        private readonly DataContext _db;

        public ReportsRepo(DataContext db)
        {
            _db = db;
        }

        public DataTable GetFullStock()
        {
            DataTable DT = new DataTable();
            DT = ((from StockSumary in
                       (
                           (from P in _db.Purchases
                            join I in _db.Items on P.ItemId equals I.ItemId
                            join PC in _db.ProductCategories on I.ProductCategoryId equals PC.ProductCategoryId
                            join IT in _db.InventoryTypes on PC.InventoryTypeId equals IT.InventoryTypeId
                            where
                              (bool)P.Return == false
                            group new { P, I, PC, IT} by new
                            {
                                P.ItemId,
                                InventoryType = IT.Name,
                                ProCat = PC.Name,
                                ItemName = I.Name
                            } into g
                            select new
                            {
                                ItemId = (int?)g.Key.ItemId,
                                InventoryType = g.Key.InventoryType,
                                ProductCategory = g.Key.ProCat,
                                ItemName = g.Key.ItemName,
                                StockIn = (decimal?)g.Sum(p => p.P.Qty),
                                StockOut = (decimal?)g.Sum(p => p.P.StockOut),
                                StockReturn =
                                  (from Purchases in _db.Purchases
                                   where
                                     Purchases.ItemId == g.Key.ItemId &&
                                     (bool)Purchases.Return == true
                                   select new
                                   {
                                       StockReturn = ((decimal?)Purchases.Qty ?? (decimal?)0)
                                   }).Sum(p => ((decimal?)p.StockReturn ?? (decimal?)0))
                            }))
                   select new
                   {
                       StockSumary.ItemId,
                       StockSumary.InventoryType,
                       StockSumary.ProductCategory,
                       StockSumary.ItemName,
                       StockSumary.StockIn,
                       StockSumary.StockOut,
                       StockSumary.StockReturn,
                       NetStock = ((StockSumary.StockIn + ((decimal?)StockSumary.StockReturn ?? (decimal?)0)) - StockSumary.StockOut)
                   })).ToList().ToDataTable();
            return DT;
        }

        public DataTable GetCustomLedger(DtoCustomLedger CLdr)
        {
            DataTable DT = new DataTable();
            DT = (from Main in
                             ((from MainLDR in
                                   ((from AVOO in
                                         ((from AVO in
                                               ((from AV in _db.AccVouchers
                                                 where
                                                 ((
                                                     from AAV in _db.AccVouchers
                                                     where AAV.AccString == CLdr.AccString
                                                     select new { AAV.VNo }
                                                     ).Distinct()).Contains(new { VNo = AV.VNo })
                                                 select new
                                                 {
                                                     AV.VDate,
                                                     AV.VNo,
                                                     AV.AccString,
                                                     AV.Debit,
                                                     AV.Credit,
                                                     AV.HeadCode,
                                                     AV.SubHeadCode,
                                                     AV.AccCode
                                                 }
                                                   ))
                                           where (
                                               (AVO.HeadCode == 41 ||
                                               AVO.HeadCode == 44 ||
                                               AVO.HeadCode == 43 ||
                                               AVO.HeadCode == 14) 
                                               //||
                                               //AVO.HeadCode == 13
                                               )
                                           select new
                                           {
                                               AVO.VDate,
                                               AVO.AccString,
                                               AVO.Debit,
                                               AVO.Credit,
                                               AVO.HeadCode,
                                               AVO.SubHeadCode,
                                               AVO.AccCode
                                           }) //AVO
                                             )
                                     join AA in _db.AccAccounts on AVOO.AccString equals AA.AccString
                                     join AH in _db.AccHead on AVOO.HeadCode equals AH.HeadCode
                                     join AT in _db.AccTypes on AH.TypeCode equals AT.TypeCode
                                     group new { AA, AVOO, AT } by new
                                     {
                                         AVOO.VDate,
                                         AVOO.AccString,
                                         AA.AccName,
                                         AT.TypeCode
                                     } into g
                                     where (g.Key.VDate >= CLdr.StartDate && g.Key.VDate <= CLdr.EndDate)
                                     select new
                                     {
                                         VDate = (DateTime?)g.Key.VDate,
                                         g.Key.AccString,
                                         g.Key.AccName,
                                         SDebit = (decimal?)g.Sum(p => p.AVOO.Debit),
                                         SCredit = (decimal?)g.Sum(p => p.AVOO.Credit),
                                         SBalance =
                                         g.Key.TypeCode == 1 ||
                                         g.Key.TypeCode == 5 ? (g.Sum(p => ((Decimal?)p.AVOO.Debit ?? (Decimal?)0)) - g.Sum(p => ((Decimal?)p.AVOO.Credit ?? (Decimal?)0))) : (g.Sum(p => ((Decimal?)p.AVOO.Credit ?? (Decimal?)0)) - g.Sum(p => ((Decimal?)p.AVOO.Debit ?? (Decimal?)0)))
                                     }
                                       ))
                               select new
                               {
                                   MainLDR.VDate,
                                   MainLDR.AccString,
                                   MainLDR.AccName,
                                   MainLDR.SDebit,
                                   MainLDR.SCredit,
                                   MainLDR.SBalance
                               }
                                 ))
                         join Opening in
                             (from O in
                                  (
                                      (from AV in _db.AccVouchers
                                       where
                                         AV.AccString == CLdr.AccString
                                       group AV by new
                                       {
                                           AV.VDate
                                       } into g
                                       select new
                                       {
                                           VVDate = g.Key.VDate,
                                           Balance = (decimal?)(g.Sum(p => p.Debit) - g.Sum(p => p.Credit))
                                       }))
                              select new
                              {
                                  O.VVDate,
                                  O.Balance,
                                  OBalance = (decimal?)
                                    (from OC in
                                         (
                                             (from AV in _db.AccVouchers
                                              where
                                                AV.AccString == CLdr.AccString
                                              group AV by new
                                              {
                                                  AV.VDate
                                              } into g
                                              select new
                                              {
                                                  VVDate = g.Key.VDate,
                                                  Balance = (decimal?)(g.Sum(p => p.Debit) - g.Sum(p => p.Credit))
                                              }))
                                     where
                                       OC.VVDate <= (DateTime)O.VVDate
                                     select new
                                     {
                                         OC.Balance
                                     }).Sum(p => p.Balance)-O.Balance
                              }) on Main.VDate equals Opening.VVDate into Opn
                         from Op in Opn.DefaultIfEmpty()
                         join Closing in
                             (from C in
                                  (
                                      (from AV in _db.AccVouchers
                                       where
                                         AV.AccString == CLdr.AccString
                                       group AV by new
                                       {
                                           AV.VDate
                                       } into g
                                       select new
                                       {
                                           VVDate = g.Key.VDate,
                                           Balance = (decimal?)(g.Sum(p => p.Debit) - g.Sum(p => p.Credit))
                                       }))
                              select new
                              {
                                  C.VVDate,
                                  C.Balance,
                                  CBalance = (decimal?)
                                    (from CC in
                                         (
                                             (from AV in _db.AccVouchers
                                              where
                                                AV.AccString == CLdr.AccString
                                              group AV by new
                                              {
                                                  AV.VDate
                                              } into g
                                              select new
                                              {
                                                  VVDate = g.Key.VDate,
                                                  Balance = (decimal?)(g.Sum(p => p.Debit) - g.Sum(p => p.Credit))
                                              }))
                                     where
                                       CC.VVDate <= (DateTime)C.VVDate
                                     select new
                                     {
                                         CC.Balance
                                     }).Sum(p => p.Balance)
                              }) on Main.VDate equals Closing.VVDate into CLO
                         from CL in CLO.DefaultIfEmpty()
                         select new
                         {
                             Main.VDate,
                             Main.AccString,
                             Main.AccName,
                             Main.SDebit,
                             Main.SCredit,
                             Main.SBalance,
                             Op.OBalance,
                             CL.CBalance
                         }).ToList().ToDataTable();
            return DT;
        }

        public DataTable GetBalanceSheet() //DtoTrailBalance TrailBalance
        {
            DataTable DTBalanceSheet = new DataTable();
            DataTable DTAssetLib = new DataTable();
            DataTable DTRevExp = new DataTable();

            DTAssetLib = (
                                  from QAA in _db.AccAccounts
                                  join ASH in _db.AccSubHead
                                        on new { QAA.HeadCode, QAA.SubHeadCode }
                                    equals new { ASH.HeadCode, ASH.SubHeadCode }
                                  join AH in _db.AccHead on ASH.HeadCode equals AH.HeadCode
                                  join AT in _db.AccTypes on AH.TypeCode equals AT.TypeCode
                                  join QAV in _db.AccVouchers
                                        on new { ASH.HeadCode, ASH.SubHeadCode, QAA.AccCode }
                                    equals new { QAV.HeadCode, QAV.SubHeadCode, QAV.AccCode } into QAV_join
                                  from QAV in QAV_join.DefaultIfEmpty()
                                  where
                                    AH.TypeCode == 1
                                  group new { AT, ASH, AH, QAA, QAV } by new
                                  {
                                      AT.TypeName,
                                      ASH.HeadCode,
                                      ASH.SubHeadCode,
                                      ASH.SubHeadString,
                                      ASH.SubHeadName,
                                      AH.HeadName,
                                      QAA.AccString,
                                      QAA.AccName
                                  } into g
                                  select new
                                  {
                                      HeadCode = (int)g.Key.HeadCode,
                                      SubHeadCode = (int)g.Key.SubHeadCode,
                                      g.Key.SubHeadString,
                                      g.Key.SubHeadName,
                                      g.Key.HeadName,
                                      g.Key.AccString,
                                      g.Key.AccName,
                                      Balance = (decimal)(g.Sum(p => ((Decimal?)p.QAV.Debit ?? (Decimal?)0)) - g.Sum(p => ((Decimal?)p.QAV.Credit ?? (Decimal?)0))),
                                      g.Key.TypeName
                                  }
                              ).Concat
                              (
                                  from QAA in _db.AccAccounts
                                  join ASH in _db.AccSubHead
                                        on new { QAA.HeadCode, QAA.SubHeadCode }
                                    equals new { ASH.HeadCode, ASH.SubHeadCode }
                                  join AH in _db.AccHead on ASH.HeadCode equals AH.HeadCode
                                  join AT in _db.AccTypes on AH.TypeCode equals AT.TypeCode
                                  join QAV in _db.AccVouchers
                                        on new { ASH.HeadCode, ASH.SubHeadCode, QAA.AccCode }
                                    equals new { QAV.HeadCode, QAV.SubHeadCode, QAV.AccCode } into QAV_join
                                  from QAV in QAV_join.DefaultIfEmpty()
                                  where
                                    AH.TypeCode == 2 ||
                                    AH.TypeCode == 3
                                  group new { AT, ASH, AH, QAA, QAV } by new
                                  {
                                      AT.TypeName,
                                      ASH.HeadCode,
                                      ASH.SubHeadCode,
                                      ASH.SubHeadString,
                                      ASH.SubHeadName,
                                      AH.HeadName,
                                      QAA.AccString,
                                      QAA.AccName
                                  } into g
                                  select new 
                                  {
                                      HeadCode = (int)g.Key.HeadCode,
                                      SubHeadCode = (int)g.Key.SubHeadCode,
                                      SubHeadString = g.Key.SubHeadString,
                                      SubHeadName = g.Key.SubHeadName,
                                      HeadName = g.Key.HeadName,
                                      AccString = g.Key.AccString,
                                      AccName = g.Key.AccName,
                                      Balance = (decimal)(g.Sum(p => ((Decimal?)p.QAV.Credit ?? (Decimal?)0)) - g.Sum(p => ((Decimal?)p.QAV.Debit ?? (Decimal?)0))),
                                      TypeName = g.Key.TypeName
                                  }
                              ).ToList().ToDataTable();
            DTRevExp = (
                              from QAV in _db.AccAccounts
                              select new 
                              {
                                  HeadCode = (int)0,
                                  SubHeadCode = (int)0,
                                   SubHeadString = "00-00",
                                   SubHeadName = "Earned Profit/Loss",
                                   HeadName = "Earned Profit/Loss",
                                   AccString = "00-00-0000",
                                   AccName = "Earned Profit/Loss",
                                  Balance = ((decimal)((from QAV1 in
                                                                            (from QAV1 in _db.AccVouchers
                                                                             join AH in _db.AccHead on QAV1.HeadCode equals AH.HeadCode
                                                                             join AT in _db.AccTypes on AH.TypeCode equals AT.TypeCode
                                                                             where
                                                                              AH.TypeCode == 4
                                                                             select new
                                                                             {
                                                                                 Debit = ((decimal?)QAV1.Debit ?? (decimal?)0),
                                                                                 Credit = ((decimal?)QAV1.Credit ?? (decimal?)0),
                                                                                 Dummy ="x"
                                                                             })
                                                                        group QAV1 by new { QAV1.Dummy } into k
                                                                        select new
                                                                        {
                                                                            Balance = (decimal)(k.Sum(p => ((Decimal?)p.Credit ?? (Decimal?)0)) - k.Sum(p => ((Decimal?)p.Debit ?? (Decimal?)0)))
                                                                        }
                                  ).AsEnumerable().Select(x=>x.Balance).FirstOrDefault())
                                - (decimal)((from QAV1 in
                                                         (from QAV1 in _db.AccVouchers
                                                          join AH in _db.AccHead on QAV1.HeadCode equals AH.HeadCode
                                                          join AT in _db.AccTypes on AH.TypeCode equals AT.TypeCode
                                                          where
                                                           AH.TypeCode == 5
                                                          select new
                                                          {
                                                              Debit = ((decimal?)QAV1.Debit ?? (decimal?)0),
                                                              Credit = ((decimal?)QAV1.Credit ?? (decimal?)0),
                                                              Dummy = "x"
                                                          })
                                                     group QAV1 by new { QAV1.Dummy } into k
                                                     select new
                                                     {
                                                         Balance = (decimal)(k.Sum(p => ((Decimal?)p.Debit ?? (Decimal?)0)) - k.Sum(p => ((Decimal?)p.Credit ?? (Decimal?)0)))
                                                     }
                                  ).AsEnumerable().Select(y=>y.Balance).FirstOrDefault())),
                                  TypeName = "RESERVE & FUNDS"
                              }).Take(1).ToList().ToDataTable();

            DTBalanceSheet = DTAssetLib.Copy();
            DTBalanceSheet.Merge(DTRevExp);
            return DTBalanceSheet;
        }

       

        public DataTable GetTrailBalance() //DtoTrailBalance TrailBalance
        {
            DataTable DTTrailBalance = new DataTable();

            DTTrailBalance = (from QAA in _db.AccAccounts
                              join ASH in _db.AccSubHead
                                    on new { QAA.HeadCode, QAA.SubHeadCode }
                                equals new { ASH.HeadCode, ASH.SubHeadCode }
                              join AH in _db.AccHead on ASH.HeadCode equals AH.HeadCode
                              join AT in _db.AccTypes on AH.TypeCode equals AT.TypeCode
                              join QAV in _db.AccVouchers
                                    on new { ASH.HeadCode, ASH.SubHeadCode, QAA.AccCode }
                                equals new { QAV.HeadCode, QAV.SubHeadCode, QAV.AccCode } into QAV_join
                              from QAV in QAV_join.DefaultIfEmpty()
                              group new { AT, AH, ASH, QAA, QAV } by new
                              {
                                  AT.TypeCode,
                                  AT.TypeName,
                                  AH.HeadName,
                                  ASH.SubHeadName,
                                  QAA.HeadCode,
                                  QAA.SubHeadCode,
                                  QAA.AccCode,
                                  QAA.AccString,
                                  QAA.AccName
                              } into g
                              select new
                              {
                                  g.Key.SubHeadName,
                                  g.Key.HeadName,
                                  HeadCode = (int?)g.Key.HeadCode,
                                  SubHeadCode = (int?)g.Key.SubHeadCode,
                                  AccCode = (int?)g.Key.AccCode,
                                  g.Key.AccString,
                                  g.Key.AccName,
                                  Opening = 0,
                                  Debit = g.Sum(p => ((Decimal?)p.QAV.Debit ?? (Decimal?)0)),
                                  Credit = g.Sum(p => ((Decimal?)p.QAV.Credit ?? (Decimal?)0)),
                                  Balance =
                                  g.Key.TypeCode == 1 ||
                                  g.Key.TypeCode == 5 ? (g.Sum(p => ((Decimal?)p.QAV.Debit ?? (Decimal?)0)) - g.Sum(p => ((Decimal?)p.QAV.Credit ?? (Decimal?)0))) : (g.Sum(p => ((Decimal?)p.QAV.Credit ?? (Decimal?)0)) - g.Sum(p => ((Decimal?)p.QAV.Debit ?? (Decimal?)0))),
                                  Status =
                                  g.Key.TypeCode == 1 ||
                                  g.Key.TypeCode == 5 ? (
                                  SqlFunctions.StringConvert((Double)(
                                  (g.Sum(p => ((Decimal?)p.QAV.Debit ?? (Decimal?)0)) - g.Sum(p => ((Decimal?)p.QAV.Credit ?? (Decimal?)0))) > 0 ? 1 :
                                  (g.Sum(p => ((Decimal?)p.QAV.Debit ?? (Decimal?)0)) - g.Sum(p => ((Decimal?)p.QAV.Credit ?? (Decimal?)0))) < 0 ? (-1) : 0)) == "1" ? "DR" :
                                  SqlFunctions.StringConvert((Double)(
                                  (g.Sum(p => ((Decimal?)p.QAV.Debit ?? (Decimal?)0)) - g.Sum(p => ((Decimal?)p.QAV.Credit ?? (Decimal?)0))) > 0 ? 1 :
                                  (g.Sum(p => ((Decimal?)p.QAV.Debit ?? (Decimal?)0)) - g.Sum(p => ((Decimal?)p.QAV.Credit ?? (Decimal?)0))) < 0 ? (-1) : 0)) == "-1" ? "CR" : "") : (
                                  SqlFunctions.StringConvert((Double)(
                                  (g.Sum(p => ((Decimal?)p.QAV.Credit ?? (Decimal?)0)) - g.Sum(p => ((Decimal?)p.QAV.Debit ?? (Decimal?)0))) > 0 ? 1 :
                                  (g.Sum(p => ((Decimal?)p.QAV.Credit ?? (Decimal?)0)) - g.Sum(p => ((Decimal?)p.QAV.Debit ?? (Decimal?)0))) < 0 ? (-1) : 0)) == "1" ? "CR" :
                                  SqlFunctions.StringConvert((Double)(
                                  (g.Sum(p => ((Decimal?)p.QAV.Credit ?? (Decimal?)0)) - g.Sum(p => ((Decimal?)p.QAV.Debit ?? (Decimal?)0))) > 0 ? 1 :
                                  (g.Sum(p => ((Decimal?)p.QAV.Credit ?? (Decimal?)0)) - g.Sum(p => ((Decimal?)p.QAV.Debit ?? (Decimal?)0))) < 0 ? (-1) : 0)) == "-1" ? "DR" : ""),
                                  g.Key.TypeName
                              }).ToList().ToDataTable();
            return DTTrailBalance;
        }

        public DataTable GetLedgers(DtoLedger ledger)
        {
            DataTable ledgers = new DataTable();
            //var ledgers = _db.AccVouchers.Where(v => v.AccString.Equals(ledger.AccString)).ToList();
            if (ledger.WithoutDate)
            {
                ledgers = (from v in _db.AccVouchers
                           where v.AccString.Equals(ledger.AccString)
                           select new
                           {
                               VoucherDate = v.VDate,
                               Ref = v.VType + " - " + v.VNo,
                               Description = v.VDescription,
                               Debit = v.Debit,
                               Credit = v.Credit,
                               //v.HeadCode, // if 1 or 5 then debit-credit=balance else credit-debit=balance
                               Balance = v.HeadCode.ToString().Substring(0, 1) == "1" || v.HeadCode.ToString().Substring(0, 1) == "5" ? (v.Debit - v.Credit) : (v.Credit - v.Debit)
                           }).ToList().ToDataTable();
            }
            else
            {
                ledgers = (from v in _db.AccVouchers
                           where v.AccString.Equals(ledger.AccString) && v.VDate <= ledger.EndDate && v.VDate >= ledger.StartDate
                           select new
                           {
                               VoucherDate = v.VDate,
                               Ref = v.VType + " - " + v.VNo,
                               Description = v.VDescription,
                               Debit = v.Debit,
                               Credit = v.Credit,
                               Balance = 0
                           }).ToList().ToDataTable();
            }
            return ledgers;
        }

        public DataTable GetVouchers(DtoVoucher voucher)
        {
            DataTable Vouchers = new DataTable();
            if (voucher.WithoutDate)
            {
                Vouchers = (from v in _db.AccVouchers
                    join A in _db.AccAccounts on v.AccString equals A.AccString
                            where v.VNo.Equals(voucher.VId)
                            orderby v.VSrNo
                            select new
                            {
                                v.VDate,
                                A.AccName,
                                v.VDescription,
                                v.VSrNo,
                                v.Debit,
                                v.Credit,
                                v.VNo,
                                v.VType,
                                v.UserCode,
                                v.InvNo,
                                v.InvType,
                                DCType =
                                        (Decimal)((Decimal?)v.Debit ?? (Decimal?)0) > 0 ? "Debit Details" :
                                        (Decimal)((Decimal?)v.Credit ?? (Decimal?)0) > 0 ? "Credit Details" : null 
                               
                            }).ToList().ToDataTable();
            }
            else
            {
                Vouchers = (from v in _db.AccVouchers
                    join A in _db.AccAccounts on v.AccString equals A.AccString
                            where v.VNo.Equals(voucher.VId) && v.VDate >= voucher.StartDate && v.VDate <= voucher.EndDate
                            orderby v.VSrNo
                            select new
                    {
                        v.VDate,
                        A.AccName,
                        v.VDescription,
                        v.VSrNo,
                        v.Debit,
                        v.Credit,
                        v.VNo,
                        v.VType,
                        v.UserCode,
                        v.InvNo,
                        v.InvType,
                        DCType =
                                        (Decimal)((Decimal?)v.Debit ?? (Decimal?)0) > 0 ? "Debit Details" :
                                        (Decimal)((Decimal?)v.Credit ?? (Decimal?)0) > 0 ? "Credit Details" : null 
                    }).ToList().ToDataTable();
            }

            return Vouchers;
        }

       

    }
}