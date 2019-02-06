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
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Purchase>().Property(p => p.Total).HasPrecision(20, 2);
            modelBuilder.Entity<Purchase>().Property(p => p.Rate).HasPrecision(20, 2);
            modelBuilder.Entity<Purchase>().Property(p => p.Percentage).HasPrecision(20, 2);
            modelBuilder.Entity<Item>().Property(p => p.SaleRate).HasPrecision(20, 2);
            modelBuilder.Entity<Stock>().Property(s => s.AvgRate).HasPrecision(20, 2);
            modelBuilder.Entity<SaleDetail>().Property(s => s.Rate).HasPrecision(20, 2);
            modelBuilder.Entity<SaleDetail>().Property(s => s.SubTotal).HasPrecision(20, 2);
            modelBuilder.Entity<Line>().Property(s => s.Percentage).HasPrecision(20, 2);
        }
    }
}