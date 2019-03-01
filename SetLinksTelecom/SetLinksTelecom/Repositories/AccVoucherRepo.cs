using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;

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
    }
}