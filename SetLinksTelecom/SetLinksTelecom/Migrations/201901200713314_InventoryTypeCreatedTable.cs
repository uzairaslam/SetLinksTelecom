namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InventoryTypeCreatedTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InventoryType",
                c => new
                    {
                        InventoryTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.InventoryTypeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.InventoryType");
        }
    }
}
