using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Repositories
{
    public class InventoryTypeRepo : IInventoryType
    {
        private readonly DataContext _db;

        public InventoryTypeRepo(DataContext db)
        {
            _db = db;
        }

        public List<InventoryType> GetData()
        {
            return _db.InventoryTypes.ToList();
        }

        public InventoryType GetInventoryType(int id)
        {
            return _db.InventoryTypes.FirstOrDefault(i => i.InventoryTypeId.Equals(id));
        }

        public void SaveInventoryType(InventoryType inventoryType)
        {
            _db.InventoryTypes.Add(inventoryType);
            _db.SaveChanges();
        }

        public void UpdateInventoryType(InventoryType inventoryType)
        {
            _db.Entry(inventoryType).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}