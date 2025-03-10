﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using SetLinksTelecom.GeneralFolder;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("name=SetLinksTelecomDBContext")
        {

            //Database.SetInitializer<DataContext>(new DropCreateDatabaseAlways<DataContext>());
            //Database.SetInitializer<DataContext>(new CreateDatabaseIfNotExists<DataContext>());

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
        public DbSet<AccType> AccTypes { get; set; }
        public DbSet<AccHead> AccHead { get; set; }
        public DbSet<AccSubHead> AccSubHead { get; set; }
        public DbSet<AccAccount> AccAccounts { get; set; }
        public DbSet<AccVoucher> AccVouchers { get; set; }
        public DbSet<BvsService> BvsServices { get; set; }
        public DbSet<BVSAllot> BvsAllots { get; set; }
        public DbSet<BVSAllotService> BvsAllotServices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<SalePurchaseStockOutMap> StockOutMaps { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Purchase>().Property(p => p.Total).HasPrecision(20, 4);
            modelBuilder.Entity<Purchase>().Property(p => p.Rate).HasPrecision(20, 4);
            modelBuilder.Entity<Purchase>().Property(p => p.Percentage).HasPrecision(20, 4);
            modelBuilder.Entity<Purchase>().Property(p => p.StockOut).HasPrecision(20, 4);
            modelBuilder.Entity<Item>().Property(p => p.SaleRate).HasPrecision(20, 4);
            modelBuilder.Entity<Stock>().Property(s => s.AvgRate).HasPrecision(20, 4);
            modelBuilder.Entity<SaleDetail>().Property(s => s.Rate).HasPrecision(20, 4);
            modelBuilder.Entity<SaleDetail>().Property(s => s.SubTotal).HasPrecision(20, 4);
            modelBuilder.Entity<SaleDetail>().Property(s => s.CommProfit).HasPrecision(20, 4);
            modelBuilder.Entity<SaleDetail>().Property(s => s.Qty).HasPrecision(20, 4);
            modelBuilder.Entity<AccVoucher>().Property(s => s.Debit).HasPrecision(20, 4);
            modelBuilder.Entity<AccVoucher>().Property(s => s.Credit).HasPrecision(20, 4);
            modelBuilder.Entity<Line>().Property(s => s.Percentage).HasPrecision(20, 2);
            modelBuilder.Entity<SalePurchaseStockOutMap>().Property(s => s.Amount).HasPrecision(20, 4);
            modelBuilder.Configurations.Add(new PersonMapping());
        }
    }

    public class DBInitializer : CreateDatabaseIfNotExists<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            base.Seed(context);
        }
    }
}