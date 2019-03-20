using System.Collections.Generic;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Data
{
    public interface IBanksRepo
    {
        List<Bank> GetData();
        Bank GetBank(int BankId);
    }
}
