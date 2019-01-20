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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}