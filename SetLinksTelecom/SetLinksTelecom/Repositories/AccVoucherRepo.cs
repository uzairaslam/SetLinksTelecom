using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Repositories
{
    public class AccVoucherRepo : IAccVoucher
    {
        private readonly DataContext _db;

        public AccVoucherRepo(DataContext db)
        {
            _db = db;
        }
        public List<DtoVoucherDisplay> GetData()
        {
            return (from V in _db.AccVouchers
                join A in _db.AccAccounts on V.AccString equals A.AccString
                select new DtoVoucherDisplay
                {
                    Id = V.AccVoucherId,
                    AccName = A.AccName,
                    VDescription = V.VDescription,
                    AccString = V.AccString,
                    InvNo = V.InvNo,
                    VType = V.VType,
                    ChequeNo = V.ChequeNo,
                    InvType = V.InvType,
                    VNo = V.VNo
                }).ToList();
        }

        public DtoJvEntry GetJvEntry()
        {
            DtoJvEntry jvEntry = new DtoJvEntry();
            jvEntry.Accounts = _db.AccAccounts.ToList();
            jvEntry.TransactionId = _db.AccVouchers.Max(v => (int?) v.AccVoucherId) ?? 0;
            ++jvEntry.TransactionId;
            jvEntry.Date = DateTime.Now;
            return jvEntry;
        }

        public void SaveJv(DtoSaveJv dtoSaveJv)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                var maxVno = _db.AccVouchers.Max(v => (int?)v.VNo) ?? 0;
                ++maxVno;
                int vSrNo = 0;
                foreach (DtoSaveJvDetail detail in dtoSaveJv.JvDetails)
                {
                    ++vSrNo;
                    AccAccount account = _db.AccAccounts.Single(re => re.AccString.Equals(detail.AccString));
                    AccVoucher voucher = new AccVoucher
                    {
                        AccString = detail.AccString,
                        AccCode = account.AccCode,
                        BID = 0,
                        CID = 0,
                        ChequeNo = "0",
                        Credit = detail.Credit,
                        Debit = detail.Debit,
                        HeadCode = account.HeadCode,
                        OID = 0,
                        SubHeadCode = account.SubHeadCode,
                        SessionId = 0,
                        VType = dtoSaveJv.VoucherType,
                        InvType = "Manual-Entry",
                        InvNo = "0",
                        VDate = dtoSaveJv.Date,
                        VDescription = detail.Remarks,
                        UserCode = 0,
                        VSrNo = vSrNo,
                        VNo = maxVno
                    };
                    _db.AccVouchers.Add(voucher);
                    _db.SaveChanges();
                }

                transaction.Commit();
            }
        }
    }
}