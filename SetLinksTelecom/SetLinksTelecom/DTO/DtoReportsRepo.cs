using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.GeneralFolder;
using SetLinksTelecom.Models;
using System.Data.Entity.SqlServer;

namespace SetLinksTelecom.DTO
{
    public class DtoReportsRepo : IReports
    {
        private readonly DataContext _db;

        public DtoReportsRepo(DataContext db)
        {
            _db = db;
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
                                v.Credit
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
                        v.Credit
                    }).ToList().ToDataTable();
            }

            return Vouchers;
        }
    }
}