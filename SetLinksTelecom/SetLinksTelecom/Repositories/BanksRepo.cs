using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Repositories
{
    public class BanksRepo : IBanksRepo
    {
        private readonly DataContext _db;

        public BanksRepo(DataContext db)
        {
            _db = db;
        }

        public List<Bank> GetData()
        {
            return _db.Banks.ToList();
        }

        public Bank GetBank(int BankId)
        {
            Bank bank = _db.Banks.Single(b => b.BankId.Equals(BankId));
            return bank;
        }
    }
}