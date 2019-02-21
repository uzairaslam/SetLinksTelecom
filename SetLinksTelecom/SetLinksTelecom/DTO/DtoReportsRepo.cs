﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.GeneralFolder;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.DTO
{
    public class DtoReportsRepo : IReports
    {
        private readonly DataContext _db;

        public DtoReportsRepo(DataContext db)
        {
            _db = db;
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
                        Balance = v.HeadCode.ToString().Substring(0,1) == "1" || v.HeadCode.ToString().Substring(0, 1) == "5" ? (v.Debit - v.Credit) : (v.Credit - v.Debit)
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
    }
}