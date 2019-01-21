namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InventoryTypeTableChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InventoryType", "Name", c => c.String(maxLength: 12));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InventoryType", "Name", c => c.String());
        }
    }
}
