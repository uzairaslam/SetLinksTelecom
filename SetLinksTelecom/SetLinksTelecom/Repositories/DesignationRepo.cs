using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.GeneralFolder;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Repositories
{
    public class DesignationRepo : IDesignationRepo
    {
        private readonly DataContext _db;

        public DesignationRepo(DataContext db)
        {
            _db = db;
        }

        public IList<Designation> GetData()
        {
            return _db.Designations.ToList();
        }

        public Designation GetDesignation(int id)
        {
            return _db.Designations.FirstOrDefault(d => d.Id.Equals(id));
        }

        public void SaveDesignation(Designation designation)
        {
            
            var maxSubHead = _db.AccSubHead.Where(s => s.HeadCode == 13).Max(a => (int?) a.SubHeadCode) ?? 0;
            maxSubHead = ++maxSubHead;
            var head = _db.AccHead.Single(h => h.HeadCode.Equals(13));
            head.SubHeads.Add(new AccSubHead
            {
                HeadCode = 13,
                SubHeadCode = maxSubHead,
                OID = 0,
                SubHeadName = designation.Name + " Customers",
                SubHeadString = 13 + "-" + maxSubHead.ToSubHeadString()
            });
            _db.Entry(head).State = EntityState.Modified;
            designation.AccString = 13 + "-" + maxSubHead.ToSubHeadString();
            _db.Designations.Add(designation);
            //_db.AccSubHead.Add(new AccSubHead
            //{
            //    HeadCode = 13,
            //    SubHeadCode = maxSubHead,
            //    OID = 0,
            //    SubHeadName = designation.Name + " Customers",
            //    SubHeadString = 13 + "-" + maxSubHead.ToSubHeadString()
            //});
            //var maxAcc = _db.AccAccounts.Where(acc => acc.HeadCode == 13 && acc.SubHeadCode == 1)
            //                 .Max(a => (int?)a.AccCode) ?? 0;
            _db.SaveChanges();
        }

        public void UpdateDesignation(Designation designation)
        {
            //string AccString = String.Empty;
            //using (var db = new DataContext())
            //{
            //    Designation desig = db.Designations.Single(d => d.Id.Equals(designation.Id));
            //    AccString = desig.AccString;
            //}
            
            //designation.AccString = AccString;
            _db.Entry(designation).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void DeleteDesignation(int id)
        {
            var persons = _db.Persons.Any(p => p.DesignationId.Equals(id));
            if (!persons)
            {
                Designation designation = _db.Designations.FirstOrDefault(d => d.Id.Equals(id));
                _db.Designations.Remove(designation);
                _db.SaveChanges();
            }
        }
    }
}