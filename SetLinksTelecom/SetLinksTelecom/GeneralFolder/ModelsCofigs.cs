using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.GeneralFolder
{
    public class PersonMapping : EntityTypeConfiguration<Person>
    {
        public PersonMapping()
        {
            HasOptional(p => p.Boss)
                .WithMany(p => p.Workers)
                .HasForeignKey(p => p.BossId)
                .WillCascadeOnDelete(false);
        }
    }
}