namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateInventoryType : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.InventoryType ( Name ) VALUES ( N'Tangible')");
            Sql("INSERT INTO dbo.InventoryType ( Name ) VALUES ( N'Intangible')");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM dbo.InventoryType WHERE Name = 'Tangible' AND Name = 'Intangible'");
        }
    }
}
