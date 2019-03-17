using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Repositories
{
    public class AccAccountRepo : IAccAccount
    {
        private readonly DataContext _db;

        public AccAccountRepo(DataContext db)
        {
            _db = db;
        }

        public List<AccAccount> GetData(int includeHead = 0)
        {
            if(includeHead == 0)
                return _db.AccAccounts.ToList();
            else
                return _db.AccAccounts.Where(a => a.HeadCode == includeHead).ToList();
        }
    }
}