using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetLinksTelecom.DTO;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Data
{
    public interface IReports
    {
        DataTable GetLedgers(DtoLedger ledger);
        DataTable GetVouchers(DtoVoucher voucher);
        DataTable GetTrailBalance();
        DataTable GetBalanceSheet();
    }
}
