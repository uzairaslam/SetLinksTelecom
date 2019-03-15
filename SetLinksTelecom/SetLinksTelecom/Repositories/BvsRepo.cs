using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Repositories
{
    public class BvsRepo : IBvsRepo
    {
        private readonly DataContext _db;

        public BvsRepo(DataContext db)
        {
            _db = db;
        }

        public BvsService GetBVS(int id)
        {
            return _db.BvsServices.FirstOrDefault(b => b.BvsServiceId.Equals(id));
        }

        public void Save(BvsService bvsService)
        {
            if (bvsService.BvsServiceId == 0)
            {
                _db.BvsServices.Add(bvsService);
            }
            else
            {
                _db.Entry(bvsService).State = EntityState.Modified;
            }

            _db.SaveChanges();
        }

        public IList<BvsService> GetData()
        {
            return _db.BvsServices.ToList();
        }
    }
}