using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SetLinksTelecom.Data;
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
            _db.Designations.Add(designation);
            _db.SaveChanges();
        }

        public void UpdateDesignation(Designation designation)
        {
            _db.Entry(designation).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void DeleteDesignation(int id)
        {
            Designation designation = _db.Designations.FirstOrDefault(d => d.Id.Equals(id));
            _db.Designations.Remove(designation);
            _db.SaveChanges();
        }
    }
}