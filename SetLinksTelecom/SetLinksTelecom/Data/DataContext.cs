using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("name=SetLinksTelecomDBContext")
        {
            
        }

        public DbSet<Designation> Designations { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<InventoryType> InventoryTypes { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Portal> Portals { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Purchase>().Property(p => p.Total).HasPrecision(20, 2);
            modelBuilder.Entity<Purchase>().Property(p => p.Rate).HasPrecision(20, 2);
        }
    }
}