using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Repositories
{
    public class ItemsRepo : IItemsRepo
    {
        private readonly DataContext _db;

        public ItemsRepo(DataContext db)
        {
            _db = db;
        }
        public IList<Item> GetData()
        {
            return _db.Items.ToList();
        }

        public Item GetItem(int id)
        {
            return _db.Items.FirstOrDefault(d => d.ItemId.Equals(id));
        }

        public void SaveItem(Item item)
        {
            _db.Items.Add(item);
            _db.SaveChanges();
        }

        public void UpdateItem(Item item)
        {
            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void DeleteItem(int id)
        {
            Item item = _db.Items.FirstOrDefault(d => d.ItemId.Equals(id));
            _db.Items.Remove(item);
            _db.SaveChanges();
        }
    }
}