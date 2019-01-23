namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProductCategoryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductCategory",
                c => new
                    {
                        ProductCategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        InventoryTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductCategoryId)
                .ForeignKey("dbo.InventoryType", t => t.InventoryTypeId, cascadeDelete: true)
                .Index(t => t.InventoryTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductCategory", "InventoryTypeId", "dbo.InventoryType");
            DropIndex("dbo.ProductCategory", new[] { "InventoryTypeId" });
            DropTable("dbo.ProductCategory");
        }
    }
}
