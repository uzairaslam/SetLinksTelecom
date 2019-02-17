namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateAccStringInPortal : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE dbo.Portal SET AccString = '22-01-0001' WHERE Name = 'Jazz Cash'");
            Sql("UPDATE dbo.Portal SET AccString = '22-01-0002' WHERE Name = 'Efics'");
        }
        
        public override void Down()
        {
            Sql("UPDATE dbo.Portal SET AccString = '22-01-0001' WHERE Name = ''");
            Sql("UPDATE dbo.Portal SET AccString = '22-01-0002' WHERE Name = ''");
        }
    }
}
